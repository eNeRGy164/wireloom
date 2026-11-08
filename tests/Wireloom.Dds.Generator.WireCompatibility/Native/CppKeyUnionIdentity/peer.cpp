#include "08-key-union.hpp"

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

#include <dds/core/xtypes/TypeKind.hpp>
#include <dds/core/xtypes/UnionType.hpp>
#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/sub/DataReader.hpp>
#include <dds/sub/Subscriber.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace
{
using CorpusUnionKeys::Identity;
using dds::core::xtypes::TypeKind;
using Clock = std::chrono::steady_clock;

static_assert(dds::topic::is_topic_type<Identity>::value,
    "Identity must be recognized as a Modern C++ topic type");
static_assert(std::is_same<decltype(std::declval<Identity&>()._d()), std::int32_t&>::value,
    "_d() must expose the int32 discriminator");
static_assert(std::is_same<decltype(std::declval<Identity&>().number()), std::int32_t&>::value,
    "number() must expose its mutable int32 branch");
static_assert(std::is_same<decltype(std::declval<Identity&>().text()), std::string&>::value,
    "text() must expose its mutable std::string branch");
static_assert(std::is_same<decltype(Identity::default_discriminator()), std::int32_t>::value,
    "default_discriminator() must retain the int32 discriminator type");

void require(bool condition, const char* message)
{
    if (!condition) { throw std::runtime_error(message); }
}

template<typename Action>
void require_inactive(Action action)
{
    bool threw = false;
    try { action(); }
    catch (const dds::core::PreconditionNotMetError&) { threw = true; }
    require(threw, "inactive union branch access must throw PreconditionNotMetError");
}

Identity make_value(const std::string& state)
{
    Identity value;
    if (state == "default") { return value; }
    if (state == "number") { value.number(7301); return value; }
    if (state == "text") { value.text("identity-key-union"); return value; }
    throw std::runtime_error("unknown Identity state");
}

void check_api()
{
    Identity value;
    require(Identity::default_discriminator() == 0 && value._d() == 0
        && value.number() == 0,
        "default Identity must select the number branch at discriminator 0");
    require_inactive([&value] { value.text(); });
    value.number(7301);
    require(value._d() == 0 && value.number() == 7301,
        "number setter must activate discriminator 0");
    value.text("identity-key-union");
    require(value._d() == 1 && value.text() == "identity-key-union",
        "text setter must activate discriminator 1");
    require_inactive([&value] { value.number(); });
    const Identity copy(value);
    require(copy == value && !(copy != value), "Identity value comparison mismatch");
}

void check_dynamic_type()
{
    const dds::core::xtypes::UnionType& type = rti::topic::dynamic_type<Identity>::get();
    require(type.name() == "CorpusUnionKeys::Identity" && type.member_count() == 2
        && type.discriminator().kind() == TypeKind::INT_32_TYPE,
        "Identity DynamicType name, member count, or discriminator mismatch");
    require(type.member(0).name() == "number"
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && type.member(0).labels().size() == 1 && type.member(0).labels()[0] == 0,
        "Identity.number branch metadata mismatch");
    require(type.member(1).name() == "text"
        && type.member(1).type().kind() == TypeKind::STRING_TYPE
        && type.member(1).labels().size() == 1 && type.member(1).labels()[0] == 1,
        "Identity.text branch metadata mismatch");
    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_UNION && native->_memberCount == 2
        && native->_typeCode != nullptr && native->_typeCode->_kind == RTI_XCDR_TK_LONG,
        "Identity native TypeCode must have an int32 discriminator and two branches");
}

void check_registration()
{
    dds::domain::DomainParticipant participant(68);
    const std::string name = dds::topic::topic_type_name<Identity>::value();
    require(name == "CorpusUnionKeys::Identity", "Identity topic type name mismatch");
    dds::topic::topic_type_support<Identity>::register_type(participant, name);
    const dds::topic::Topic<Identity> topic(participant, "CorpusUnionKeys::IdentityTopic");
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Identity> writer(publisher, topic);
    const auto number_handle = writer.register_instance(make_value("number"));
    const auto text_handle = writer.register_instance(make_value("text"));
    const auto default_handle = writer.register_instance(make_value("default"));
    // Identity is a topic root without @key annotations; all values therefore share the keyless instance.
    require(number_handle == text_handle && text_handle == default_handle,
        "keyless Identity topic values must use the same instance handle");
}

void append_frame(std::ofstream& output, const Identity& expected,
    dds::core::policy::DataRepresentationId representation)
{
    std::vector<char> buffer;
    dds::topic::topic_type_support<Identity>::to_cdr_buffer(buffer, expected, representation);
    Identity actual;
    dds::topic::topic_type_support<Identity>::from_cdr_buffer(actual, buffer);
    require(actual == expected, "Identity CDR round trip changed union state");
    const std::uint32_t size = static_cast<std::uint32_t>(buffer.size());
    output.write(reinterpret_cast<const char*>(&size), sizeof(size));
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    require(static_cast<bool>(output), "Could not write Identity CDR frame");
}

void check_cdr(const std::string& encoding, const std::string& path)
{
    const auto representation = encoding == "xcdr1"
        ? dds::core::policy::DataRepresentation::xcdr()
        : dds::core::policy::DataRepresentation::xcdr2();
    std::ofstream output(path.c_str(), std::ios::binary | std::ios::trunc);
    require(static_cast<bool>(output), "Could not create Identity CDR capture");
    for (const std::string state : {"default", "number", "text"}) {
        append_frame(output, make_value(state), representation);
    }
    require(static_cast<bool>(output), "Could not finish Identity CDR capture");
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
    const dds::topic::Topic<Identity> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{
        encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representation(ids);
    const std::string marker = std::string(argv[4]) + "/received";
    if (role == "writer") {
        const dds::pub::Publisher publisher(participant);
        auto qos = publisher.default_datawriter_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::pub::DataWriter<Identity> writer(publisher, topic, qos);
        await([&] { return writer.publication_matched_status().current_count() == 1; }, "reader discovery");
        writer.write(make_value("text"));
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<Identity> reader(subscriber, topic, qos);
        Identity received;
        await([&] {
            const auto samples = reader.take();
            if (samples.length() == 0) { return false; }
            std::size_t valid_count = 0;
            for (auto sample = samples.begin(); sample != samples.end(); ++sample) {
                if (!sample->info().valid()) { continue; }
                received = sample->data();
                ++valid_count;
            }
            require(valid_count <= 1, "Received multiple valid Identity samples");
            return valid_count == 1;
        }, "receiving Identity sample");
        require(received == make_value("text"), "Live Identity sample differed from expected value");
        std::ofstream acknowledgement(marker.c_str());
        require(static_cast<bool>(acknowledgement << "validated\n"), "Could not acknowledge live sample");
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
        std::cout << command << " passed for CorpusUnionKeys::Identity" << std::endl;
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
