#include "08-key-boundaries.hpp"

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

#include <dds/core/xtypes/StructType.hpp>
#include <dds/dds.hpp>
#include <rti/topic/cdr.hpp>

namespace {
using CorpusKeyBoundaries::UnboundedStringKey;
using Clock = std::chrono::steady_clock;
const std::size_t observed_rti_key_limit = 255;

void require(bool condition, const char* message)
{
    if (!condition) { throw std::runtime_error(message); }
}

UnboundedStringKey make_value(const std::string& name, std::int32_t payload)
{
    return UnboundedStringKey(name, payload);
}

UnboundedStringKey max_value()
{
    return make_value(std::string(observed_rti_key_limit, 'x'), 9001);
}

void check_api()
{
    static_assert(dds::topic::is_topic_type<UnboundedStringKey>::value,
        "UnboundedStringKey must be recognized as an RTI Modern C++ topic type");
    static_assert(std::is_same<decltype(UnboundedStringKey::name), std::string>::value,
        "UnboundedStringKey.name must use std::string");
    static_assert(std::is_same<decltype(UnboundedStringKey::payload), std::int32_t>::value,
        "UnboundedStringKey.payload must be int32_t");
    static_assert(std::is_constructible<UnboundedStringKey, const std::string&,
        std::int32_t>::value,
        "UnboundedStringKey must accept a std::string consumer argument");

    const UnboundedStringKey defaults;
    require(defaults.name.empty() && defaults.payload == 0,
        "UnboundedStringKey default values must be empty and zero");
    const UnboundedStringKey value = make_value("short", 9001);
    require(value.name == "short" && value.payload == 9001,
        "UnboundedStringKey constructor must preserve values");
    const UnboundedStringKey copied(value);
    require(copied == value && !(copied != value),
        "UnboundedStringKey equality must preserve values");
}

void check_dynamic_type()
{
    using dds::core::xtypes::TypeKind;
    const dds::core::xtypes::StructType& type =
        rti::topic::dynamic_type<UnboundedStringKey>::get();
    require(type.name() == "CorpusKeyBoundaries::UnboundedStringKey"
        && type.member_count() == 2, "UnboundedStringKey DynamicType identity/count mismatch");
    require(type.member(0).name() == "name"
        && type.member(0).type().kind() == TypeKind::STRING_TYPE
        && type.member(0).is_key(), "UnboundedStringKey.name DynamicType/key metadata mismatch");
    require(type.member(1).name() == "payload"
        && type.member(1).type().kind() == TypeKind::INT_32_TYPE
        && !type.member(1).is_key(), "UnboundedStringKey.payload DynamicType/key metadata mismatch");

    const RTIXCdrTypeCode* native =
        reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_STRUCT && native->_memberCount == 2,
        "UnboundedStringKey native TypeCode identity/count mismatch");
    require(native->_members[0]._memberFlags == RTI_XCDR_KEY_MEMBER,
        "UnboundedStringKey.name native TypeCode key flag mismatch");
    require(native->_members[1]._memberFlags == RTI_XCDR_REQUIRED_MEMBER,
        "UnboundedStringKey.payload native TypeCode required flag mismatch");
}

void check_registration()
{
    dds::domain::DomainParticipant participant(68);
    const std::string type_name = dds::topic::topic_type_name<UnboundedStringKey>::value();
    require(type_name == "CorpusKeyBoundaries::UnboundedStringKey",
        "UnboundedStringKey topic type name mismatch");
    dds::topic::topic_type_support<UnboundedStringKey>::register_type(participant, type_name);
    const dds::topic::Topic<UnboundedStringKey> topic(participant, type_name);
    dds::pub::DataWriter<UnboundedStringKey> writer(topic);

    UnboundedStringKey first = make_value("different-key", 9001);
    UnboundedStringKey same_key = first;
    same_key.payload = 1;
    UnboundedStringKey different_key = first;
    different_key.name[0] = 'D';
    UnboundedStringKey maximum_key = max_value();
    UnboundedStringKey same_maximum_key = maximum_key;
    same_maximum_key.payload = 7;
    require(writer.register_instance(first) == writer.register_instance(same_key),
        "UnboundedStringKey instance identity must ignore payload");
    require(writer.register_instance(first) != writer.register_instance(different_key),
        "UnboundedStringKey instance identity must include name");
    require(writer.register_instance(maximum_key) == writer.register_instance(same_maximum_key),
        "UnboundedStringKey maximum-length instance identity must ignore payload");
}

void append_frame(std::ofstream& output, const UnboundedStringKey& value,
    dds::core::policy::DataRepresentationId representation)
{
    std::vector<char> buffer;
    dds::topic::topic_type_support<UnboundedStringKey>::to_cdr_buffer(buffer, value, representation);
    const std::uint32_t size = static_cast<std::uint32_t>(buffer.size());
    output.write(reinterpret_cast<const char*>(&size), sizeof(size));
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    UnboundedStringKey decoded;
    dds::topic::topic_type_support<UnboundedStringKey>::from_cdr_buffer(decoded, buffer);
    require(decoded == value, "UnboundedStringKey local CDR round trip changed a value");
}

void check_cdr(const std::string& representation, const std::string& path)
{
    using dds::core::policy::DataRepresentation;
    const auto format = representation == "xcdr1"
        ? DataRepresentation::xcdr() : DataRepresentation::xcdr2();
    const UnboundedStringKey states[] = {
        UnboundedStringKey(), make_value("x", -7), max_value()};
    std::ofstream output(path.c_str(), std::ios::binary | std::ios::trunc);
    require(static_cast<bool>(output), "Could not create CDR capture");
    for (const UnboundedStringKey& value : states) { append_frame(output, value, format); }
    require(static_cast<bool>(output), "Could not finish CDR capture");
}

void probe_length(std::size_t length, const std::string& path)
{
    const UnboundedStringKey value = make_value(std::string(length, 'p'), 1);
    std::string result = "accepted";
    try {
        std::vector<char> buffer;
        dds::topic::topic_type_support<UnboundedStringKey>::to_cdr_buffer(
            buffer, value, dds::core::policy::DataRepresentation::xcdr());
    } catch (const std::exception& error) {
        result = "rejected: ";
        result += error.what();
    }
    std::ofstream output(path.c_str(), std::ios::out | std::ios::trunc);
    require(static_cast<bool>(output << result << '\n'), "Could not write string-length outcome");
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
    const dds::topic::Topic<UnboundedStringKey> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{
        encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representation(ids);
    const std::string marker = std::string(argv[4]) + "/received";
    if (role == "writer") {
        const dds::pub::Publisher publisher(participant);
        auto qos = publisher.default_datawriter_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::pub::DataWriter<UnboundedStringKey> writer(publisher, topic, qos);
        await([&] { return writer.publication_matched_status().current_count() == 1; }, "reader discovery");
        writer.write(max_value());
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<UnboundedStringKey> reader(subscriber, topic, qos);
        UnboundedStringKey received;
        await([&] {
            const auto samples = reader.take();
            if (samples.length() == 0) { return false; }
            std::size_t valid_count = 0;
            for (auto sample = samples.begin(); sample != samples.end(); ++sample) {
                if (!sample->info().valid()) { continue; }
                received = sample->data();
                ++valid_count;
            }
            require(valid_count <= 1, "Received multiple valid UnboundedStringKey samples");
            return valid_count == 1;
        }, "receiving UnboundedStringKey sample");
        require(received == max_value(), "Live DDS UnboundedStringKey differed from expected value");
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
        require(argc >= 2, "Expected --api, --dynamic-type, --registration, --cdr, or --probe");
        const std::string command = argv[1];
        if (command == "--api") { check_api(); }
        else if (command == "--dynamic-type") { check_dynamic_type(); }
        else if (command == "--registration") { check_registration(); }
        else if (command == "--cdr" && argc == 4) { check_cdr(argv[2], argv[3]); }
        else if (command == "--probe" && argc == 4) { probe_length(static_cast<std::size_t>(std::stoul(argv[2])), argv[3]); }
        else { throw std::runtime_error("Invalid command line"); }
        std::cout << command << " passed for CorpusKeyBoundaries::UnboundedStringKey" << std::endl;
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
