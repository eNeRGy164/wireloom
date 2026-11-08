#include "09-extensibility.hpp"

#include <chrono>
#include <cstdint>
#include <fstream>
#include <functional>
#include <iostream>
#include <stdexcept>
#include <string>
#include <thread>
#include <type_traits>
#include <utility>
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
using CorpusExtensibility::Mutable;
using OptionalInt = dds::core::optional<std::int32_t>;
using dds::core::xtypes::ExtensibilityKind;
using dds::core::xtypes::TypeKind;
using Clock = std::chrono::steady_clock;

static_assert(dds::topic::is_topic_type<Mutable>::value,
    "Mutable must be recognized as a Modern C++ topic type");
static_assert(std::is_same<decltype(Mutable::id), std::int32_t>::value,
    "Mutable.id must be int32_t");
static_assert(std::is_same<decltype(Mutable::optionalValue), OptionalInt>::value,
    "Mutable.optionalValue must use dds::core::optional<int32_t>");
static_assert(std::is_same<omg::types::optional<std::int32_t>, OptionalInt>::value,
    "RTI's OMG optional alias must resolve to the same Modern C++ optional type");
static_assert(std::is_constructible<Mutable, std::int32_t, const OptionalInt&>::value,
    "Mutable constructor must accept id and the shared optional type");
static_assert(rti::topic::extensibility<Mutable>::kind == ExtensibilityKind::MUTABLE,
    "Mutable topic trait must map to RTI's mutable profile");

void require(bool condition, const char* message)
{
    if (!condition) { throw std::runtime_error(message); }
}

Mutable absent_value(std::int32_t id)
{
    return Mutable(id, OptionalInt{});
}

Mutable present_value(std::int32_t id, std::int32_t optional_value)
{
    OptionalInt value;
    value = optional_value;
    return Mutable(id, value);
}

void check_api()
{
    Mutable defaults;
    require(defaults.id == 0 && !defaults.optionalValue.has_value(),
        "Mutable defaults must use zero id and absent optional value");
    defaults.id = 7;
    defaults.optionalValue = 0;
    require(defaults.optionalValue.has_value() && defaults.optionalValue.value() == 0,
        "Mutable optional value must preserve an explicitly present zero");
    defaults.optionalValue.reset();
    require(!defaults.optionalValue.has_value(), "Mutable optional reset must clear presence");

    const Mutable present = present_value(11, 42);
    require(present.id == 11 && present.optionalValue.has_value()
        && present.optionalValue.value() == 42,
        "Mutable constructor must preserve optional presence and value");
    const Mutable copy(present);
    require(copy == present && !(copy != present), "Mutable equality must preserve optional state");
    require(absent_value(11) != present,
        "Mutable equality must distinguish absent from present optional values");
}

void check_dynamic_type()
{
    using namespace dds::core::xtypes;
    const StructType& type = rti::topic::dynamic_type<Mutable>::get();
    require(type.name() == "CorpusExtensibility::Mutable" && type.member_count() == 2,
        "Mutable DynamicType identity/count mismatch");
    require(type.extensibility_kind() == ExtensibilityKind::MUTABLE
        && is_mutable(type) && !is_final(type) && !is_extensible(type),
        "Mutable DynamicType extensibility mismatch");
    require(type.member(0).name() == "id" && type.member(0).get_id() == 1
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && !type.member(0).is_optional(),
        "Mutable id member metadata mismatch");
    require(type.member(1).name() == "optionalValue" && type.member(1).get_id() == 2
        && type.member(1).type().kind() == TypeKind::INT_32_TYPE
        && type.member(1).is_optional() && !type.member(1).is_key()
        && !type.member(1).is_must_understand(),
        "Mutable optionalValue metadata mismatch");
    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == (RTI_XCDR_TK_STRUCT | RTI_XCDR_TK_FLAGS_IS_MUTABLE)
        && native->_memberCount == 2
        && native->_members[0]._memberFlags == RTI_XCDR_REQUIRED_MEMBER,
        "Mutable native TypeCode kind or required-member flags mismatch");
}

void check_registration()
{
    dds::domain::DomainParticipant participant(68);
    const std::string name = dds::topic::topic_type_name<Mutable>::value();
    require(name == "CorpusExtensibility::Mutable", "Mutable topic type name mismatch");
    dds::topic::topic_type_support<Mutable>::register_type(participant, name);
    const dds::topic::Topic<Mutable> topic(participant, name);
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Mutable> writer(publisher, topic);
    require(writer.register_instance(absent_value(1))
        == writer.register_instance(present_value(2, 42)),
        "Mutable without @key members must use a keyless instance handle");
}

void append_frame(std::ofstream& output, const Mutable& expected,
    dds::core::policy::DataRepresentationId representation)
{
    std::vector<char> buffer;
    dds::topic::topic_type_support<Mutable>::to_cdr_buffer(buffer, expected, representation);
    Mutable actual;
    dds::topic::topic_type_support<Mutable>::from_cdr_buffer(actual, buffer);
    require(actual == expected, "Mutable CDR round trip changed optional presence/value");
    const std::uint32_t size = static_cast<std::uint32_t>(buffer.size());
    output.write(reinterpret_cast<const char*>(&size), sizeof(size));
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    require(static_cast<bool>(output), "Could not write Mutable CDR frame");
}

void check_cdr(const std::string& encoding, const std::string& path)
{
    const auto representation = encoding == "xcdr1"
        ? dds::core::policy::DataRepresentation::xcdr()
        : dds::core::policy::DataRepresentation::xcdr2();
    const Mutable states[] = {
        Mutable(), absent_value(7), present_value(7, 0), present_value(-17, 42)};
    std::ofstream output(path.c_str(), std::ios::binary | std::ios::trunc);
    require(static_cast<bool>(output), "Could not create Mutable CDR capture");
    for (const Mutable& state : states) { append_frame(output, state, representation); }
    require(static_cast<bool>(output), "Could not finish Mutable CDR capture");
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
    const dds::topic::Topic<Mutable> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{
        encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representation(ids);
    const std::string marker = std::string(argv[4]) + "/received";
    if (role == "writer") {
        const dds::pub::Publisher publisher(participant);
        auto qos = publisher.default_datawriter_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::pub::DataWriter<Mutable> writer(publisher, topic, qos);
        await([&] { return writer.publication_matched_status().current_count() == 1; }, "reader discovery");
        writer.write(absent_value(101));
        writer.write(present_value(102, 42));
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<Mutable> reader(subscriber, topic, qos);
        bool received_absent = false;
        bool received_present = false;
        await([&] {
            const auto samples = reader.take();
            for (auto sample = samples.begin(); sample != samples.end(); ++sample) {
                if (!sample->info().valid()) { continue; }
                const Mutable& value = sample->data();
                if (value.id == 101 && !value.optionalValue.has_value()) {
                    received_absent = true;
                } else if (value.id == 102 && value.optionalValue.has_value()
                    && value.optionalValue.value() == 42) {
                    received_present = true;
                } else {
                    throw std::runtime_error("Unexpected Mutable live sample or optional state");
                }
            }
            return received_absent && received_present;
        }, "receiving absent and present Mutable samples");
        std::ofstream acknowledgement(marker.c_str());
        require(static_cast<bool>(acknowledgement << "validated absent and present\n"),
            "Could not acknowledge Mutable samples");
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
        std::cout << command << " passed for CorpusExtensibility::Mutable" << std::endl;
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
