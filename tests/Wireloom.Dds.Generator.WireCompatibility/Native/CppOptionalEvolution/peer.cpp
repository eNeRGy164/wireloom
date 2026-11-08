#include "09-evolution-optional.hpp"

#include <chrono>
#include <cstdint>
#include <fstream>
#include <functional>
#include <iostream>
#include <stdexcept>
#include <string>
#include <thread>
#include <type_traits>
#include <vector>

#include <dds/core/Optional.hpp>
#include <dds/core/xtypes/StructType.hpp>
#include <dds/core/xtypes/TypeKind.hpp>
#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/sub/DataReader.hpp>
#include <dds/sub/Subscriber.hpp>
#include <dds/topic/Topic.hpp>
#include <omg/types/optional.hpp>
#include <rti/topic/cdr.hpp>

namespace
{
using CorpusOptionalEvolution::Message;
using OptionalInt = dds::core::optional<std::int32_t>;
using OptionalText = dds::core::optional<std::string>;
using dds::core::xtypes::ExtensibilityKind;
using dds::core::xtypes::TypeKind;
using Clock = std::chrono::steady_clock;

static_assert(dds::topic::is_topic_type<Message>::value,
    "Message must be recognized as a Modern C++ topic type");
static_assert(std::is_same<decltype(Message::requiredValue), std::int32_t>::value,
    "Message.requiredValue must be int32_t");
static_assert(std::is_same<decltype(Message::optionalValue), OptionalInt>::value,
    "Message.optionalValue must use dds::core::optional<int32_t>");
static_assert(std::is_same<decltype(Message::optionalText), OptionalText>::value,
    "Message.optionalText must use dds::core::optional<std::string>");
static_assert(std::is_same<omg::types::optional<std::int32_t>, OptionalInt>::value,
    "RTI OMG optional<int32_t> must alias the Modern C++ optional type");
static_assert(std::is_same<omg::types::optional<std::string>, OptionalText>::value,
    "RTI OMG optional<std::string> must alias the Modern C++ optional type");
static_assert(std::is_constructible<Message, std::int32_t,
        const OptionalInt&, const OptionalText&>::value,
    "Message constructor must accept required and optional values");
static_assert(rti::topic::extensibility<Message>::kind == ExtensibilityKind::EXTENSIBLE,
    "@appendable Message must map to RTI's appendable profile");

void require(bool condition, const char* message)
{
    if (!condition) { throw std::runtime_error(message); }
}

Message make_value(std::int32_t required, const OptionalInt& number,
    const OptionalText& text)
{
    return Message(required, number, text);
}

Message absent_value(std::int32_t required)
{
    return make_value(required, OptionalInt{}, OptionalText{});
}

Message present_number(std::int32_t required, std::int32_t value)
{
    OptionalInt number;
    number = value;
    return make_value(required, number, OptionalText{});
}

Message present_text(std::int32_t required, const std::string& value)
{
    OptionalText text;
    text = value;
    return make_value(required, OptionalInt{}, text);
}

Message present_both(std::int32_t required, std::int32_t number_value,
    const std::string& text_value)
{
    OptionalInt number;
    number = number_value;
    OptionalText text;
    text = text_value;
    return make_value(required, number, text);
}

void check_api()
{
    Message defaults;
    require(defaults.requiredValue == 0 && !defaults.optionalValue.has_value()
        && !defaults.optionalText.has_value(),
        "Message defaults must use zero and absent optional members");
    defaults.optionalValue = 0;
    defaults.optionalText = std::string();
    require(defaults.optionalValue.has_value() && defaults.optionalValue.value() == 0
        && defaults.optionalText.has_value() && defaults.optionalText.value().empty(),
        "Optional members must preserve present zero and empty string");
    defaults.optionalValue.reset();
    defaults.optionalText.reset();
    require(!defaults.optionalValue.has_value() && !defaults.optionalText.has_value(),
        "Optional reset must clear presence");
    const Message value = present_both(17, 42, "optional");
    require(value.requiredValue == 17 && value.optionalValue.has_value()
        && value.optionalValue.value() == 42 && value.optionalText.has_value()
        && value.optionalText.value() == "optional",
        "Message constructor must preserve optional values");
    require(absent_value(17) != value,
        "Message equality must distinguish absent and present optionals");
    const Message copy(value);
    require(copy == value && !(copy != value),
        "Message equality must preserve both optional states");
}

void check_dynamic_type()
{
    using namespace dds::core::xtypes;
    const StructType& type = rti::topic::dynamic_type<Message>::get();
    require(type.name() == "CorpusOptionalEvolution::Message" && type.member_count() == 3,
        "Message DynamicType identity/count mismatch");
    require(type.extensibility_kind() == ExtensibilityKind::EXTENSIBLE
        && is_extensible(type) && !is_final(type) && !is_mutable(type),
        "Message appendable DynamicType mismatch");
    require(type.member(0).name() == "requiredValue" && type.member(0).get_id() == 1
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && !type.member(0).is_optional(),
        "requiredValue DynamicType metadata mismatch");
    require(type.member(1).name() == "optionalValue" && type.member(1).get_id() == 2
        && type.member(1).type().kind() == TypeKind::INT_32_TYPE
        && type.member(1).is_optional() && !type.member(1).is_key(),
        "optionalValue DynamicType metadata mismatch");
    require(type.member(2).name() == "optionalText" && type.member(2).get_id() == 3
        && type.member(2).type().kind() == TypeKind::STRING_TYPE
        && type.member(2).is_optional() && !type.member(2).is_key()
        && type.member(2).type().native()._data._maximumLength == 16,
        "optionalText DynamicType metadata or bound mismatch");
    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_STRUCT && native->_memberCount == 3
        && native->_members[0]._memberFlags == RTI_XCDR_REQUIRED_MEMBER,
        "Message native TypeCode kind or required-member flags mismatch");
}

void check_registration()
{
    dds::domain::DomainParticipant participant(68);
    const std::string name = dds::topic::topic_type_name<Message>::value();
    require(name == "CorpusOptionalEvolution::Message", "Message topic type name mismatch");
    dds::topic::topic_type_support<Message>::register_type(participant, name);
    const dds::topic::Topic<Message> topic(participant, name);
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Message> writer(publisher, topic);
    require(writer.register_instance(absent_value(1))
        == writer.register_instance(present_both(2, 0, "keyless")),
        "Message without @key members must use a keyless instance handle");
}

void append_frame(std::ofstream& output, const Message& expected,
    dds::core::policy::DataRepresentationId representation)
{
    std::vector<char> buffer;
    dds::topic::topic_type_support<Message>::to_cdr_buffer(buffer, expected, representation);
    Message actual;
    dds::topic::topic_type_support<Message>::from_cdr_buffer(actual, buffer);
    require(actual == expected, "Message CDR round trip changed optional state/value");
    const std::uint32_t size = static_cast<std::uint32_t>(buffer.size());
    output.write(reinterpret_cast<const char*>(&size), sizeof(size));
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    require(static_cast<bool>(output), "Could not write Message CDR frame");
}

void check_cdr(const std::string& encoding, const std::string& path)
{
    const auto representation = encoding == "xcdr1"
        ? dds::core::policy::DataRepresentation::xcdr()
        : dds::core::policy::DataRepresentation::xcdr2();
    const Message states[] = {
        absent_value(0), absent_value(7), present_number(7, 0),
        present_text(-4, ""), present_both(42, 42, "optional"),
        present_text(-17, std::string(16, 'x'))};
    std::ofstream output(path.c_str(), std::ios::binary | std::ios::trunc);
    require(static_cast<bool>(output), "Could not create Message CDR capture");
    for (const Message& state : states) { append_frame(output, state, representation); }
    require(static_cast<bool>(output), "Could not finish Message CDR capture");
}

void await(const std::function<bool()>& condition, const std::string& context)
{
    const auto deadline = Clock::now() + std::chrono::seconds(15);
    while (!condition()) {
        if (Clock::now() >= deadline) { throw std::runtime_error("Timed out: " + context); }
        std::this_thread::sleep_for(std::chrono::milliseconds(25));
    }
}

void run_live(int argc, char** argv)
{
    require(argc == 5, "Usage: peer <reader|writer> <xcdr1|xcdr2> <topic> <ack-directory>");
    const std::string role = argv[1];
    const std::string encoding = argv[2];
    require((role == "reader" || role == "writer")
        && (encoding == "xcdr1" || encoding == "xcdr2"), "Invalid live DDS role/encoding");
    using namespace dds::core::policy;
    const dds::domain::DomainParticipant participant(68);
    const dds::topic::Topic<Message> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{
        encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representation(ids);
    const std::string marker = std::string(argv[4]) + "/received";
    if (role == "writer") {
        const dds::pub::Publisher publisher(participant);
        auto qos = publisher.default_datawriter_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::pub::DataWriter<Message> writer(publisher, topic, qos);
        await([&] { return writer.publication_matched_status().current_count() == 1; }, "reader discovery");
        writer.write(absent_value(101));
        writer.write(present_number(102, 42));
        writer.write(present_text(103, "live"));
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<Message> reader(subscriber, topic, qos);
        bool received_absent = false;
        bool received_number = false;
        bool received_text = false;
        await([&] {
            const auto samples = reader.take();
            for (auto sample = samples.begin(); sample != samples.end(); ++sample) {
                if (!sample->info().valid()) { continue; }
                const Message& value = sample->data();
                if (value.requiredValue == 101 && !value.optionalValue.has_value()
                    && !value.optionalText.has_value()) {
                    received_absent = true;
                } else if (value.requiredValue == 102 && value.optionalValue.has_value()
                    && value.optionalValue.value() == 42 && !value.optionalText.has_value()) {
                    received_number = true;
                } else if (value.requiredValue == 103 && !value.optionalValue.has_value()
                    && value.optionalText.has_value() && value.optionalText.value() == "live") {
                    received_text = true;
                } else {
                    throw std::runtime_error("Unexpected live Message value or optional state");
                }
            }
            return received_absent && received_number && received_text;
        }, "receiving all Message optional states");
        std::ofstream acknowledgement(marker.c_str());
        require(static_cast<bool>(acknowledgement << "validated absent, number, and text\n"),
            "Could not acknowledge Message samples");
    }
}
}

int main(int argc, char** argv)
{
    try {
        if (argc >= 2 && (std::string(argv[1]) == "reader" || std::string(argv[1]) == "writer")) {
            run_live(argc, argv);
            return 0;
        }
        require(argc >= 2, "Expected --api, --dynamic-type, --registration, or --cdr");
        const std::string command = argv[1];
        if (command == "--api") { check_api(); }
        else if (command == "--dynamic-type") { check_dynamic_type(); }
        else if (command == "--registration") { check_registration(); }
        else if (command == "--cdr" && argc == 4) { check_cdr(argv[2], argv[3]); }
        else { throw std::runtime_error("Invalid command line"); }
        std::cout << command << " passed for CorpusOptionalEvolution::Message" << std::endl;
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
