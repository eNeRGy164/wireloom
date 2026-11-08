#include "09-id-gaps.hpp"

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

#include <dds/core/xtypes/StructType.hpp>
#include <dds/core/xtypes/TypeKind.hpp>
#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/sub/DataReader.hpp>
#include <dds/sub/Subscriber.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace
{
using CorpusIdGaps::Message;
using dds::core::xtypes::ExtensibilityKind;
using dds::core::xtypes::TypeKind;
using Clock = std::chrono::steady_clock;

static_assert(dds::topic::is_topic_type<Message>::value,
    "Message must be recognized as a Modern C++ topic type");
static_assert(std::is_same<decltype(Message::first), std::int32_t>::value,
    "Message.first must be int32_t");
static_assert(std::is_same<decltype(Message::seventh), std::int32_t>::value,
    "Message.seventh must be int32_t");
static_assert(std::is_constructible<Message, std::int32_t, std::int32_t>::value,
    "Message constructor must accept the two int32 values");
static_assert(rti::topic::extensibility<Message>::kind == ExtensibilityKind::EXTENSIBLE,
    "@appendable IDL struct must use RTI's appendable profile");

void require(bool condition, const char* message)
{
    if (!condition) { throw std::runtime_error(message); }
}

Message make_value(std::int32_t first, std::int32_t seventh)
{
    return Message(first, seventh);
}

void check_api()
{
    const Message defaults;
    require(defaults.first == 0 && defaults.seventh == 0,
        "Message default values must be zero");
    const Message value = make_value(7301, 0);
    require(value.first == 7301 && value.seventh == 0,
        "Message constructor must preserve values");
    const Message copy(value);
    require(copy == value && !(copy != value), "Message equality must preserve values");
}

void check_dynamic_type()
{
    using namespace dds::core::xtypes;
    const StructType& type = rti::topic::dynamic_type<Message>::get();
    require(type.name() == "CorpusIdGaps::Message" && type.member_count() == 2,
        "Message DynamicType identity/count mismatch");
    require(type.extensibility_kind() == ExtensibilityKind::EXTENSIBLE
        && is_extensible(type) && !is_final(type) && !is_mutable(type),
        "Message appendable DynamicType mismatch");
    require(type.member(0).name() == "first" && type.member(0).get_id() == 1
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE,
        "first member ID/type mismatch");
    require(type.member(1).name() == "seventh" && type.member(1).get_id() == 7
        && type.member(1).type().kind() == TypeKind::INT_32_TYPE,
        "seventh member ID/type mismatch");
    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == (RTI_XCDR_TK_STRUCT) && native->_memberCount == 2
        && native->_members[0]._memberFlags == RTI_XCDR_REQUIRED_MEMBER
        && native->_members[1]._memberFlags == RTI_XCDR_REQUIRED_MEMBER,
        "Appendable Message native TypeCode member flags mismatch");
}

void check_registration()
{
    dds::domain::DomainParticipant participant(68);
    const std::string name = dds::topic::topic_type_name<Message>::value();
    require(name == "CorpusIdGaps::Message", "Message topic type name mismatch");
    dds::topic::topic_type_support<Message>::register_type(participant, name);
    const dds::topic::Topic<Message> topic(participant, name);
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Message> writer(publisher, topic);
    require(writer.register_instance(make_value(1, 0))
        == writer.register_instance(make_value(0, 2)),
        "Message without @key members must use a keyless instance handle");
}

void append_frame(std::ofstream& output, const Message& expected,
    dds::core::policy::DataRepresentationId representation)
{
    std::vector<char> buffer;
    dds::topic::topic_type_support<Message>::to_cdr_buffer(buffer, expected, representation);
    Message actual;
    dds::topic::topic_type_support<Message>::from_cdr_buffer(actual, buffer);
    require(actual == expected, "Message CDR round trip changed values");
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
        Message(), make_value(7301, 0), make_value(0, 77), make_value(-17, 17)};
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
        writer.write(make_value(7301, 7));
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<Message> reader(subscriber, topic, qos);
        Message received;
        await([&] {
            const auto samples = reader.take();
            if (samples.length() == 0) { return false; }
            std::size_t valid_count = 0;
            for (auto sample = samples.begin(); sample != samples.end(); ++sample) {
                if (!sample->info().valid()) { continue; }
                received = sample->data();
                ++valid_count;
            }
            require(valid_count <= 1, "Received multiple valid Message samples");
            return valid_count == 1;
        }, "receiving Message sample");
        require(received == make_value(7301, 7),
            "Live Message sample differed from expected value");
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
        std::cout << command << " passed for CorpusIdGaps::Message" << std::endl;
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
