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
using CorpusKeyBoundaries::ArrayKey;
using CoordinateArray = dds::core::array<std::int32_t, 2>;
using Clock = std::chrono::steady_clock;

void require(bool condition, const char* message)
{
    if (!condition) { throw std::runtime_error(message); }
}

ArrayKey populated_value()
{
    CoordinateArray coordinates{};
    coordinates[0] = 7;
    coordinates[1] = -19;
    return ArrayKey(coordinates, 9001);
}

void check_api()
{
    static_assert(dds::topic::is_topic_type<ArrayKey>::value,
        "ArrayKey must be recognized as an RTI Modern C++ topic type");
    static_assert(std::is_same<decltype(ArrayKey::coordinates), CoordinateArray>::value,
        "ArrayKey.coordinates must use the RTI Modern C++ fixed-array type");
    static_assert(std::is_same<decltype(ArrayKey::payload), std::int32_t>::value,
        "ArrayKey.payload must be int32_t");
    static_assert(std::is_constructible<ArrayKey, const CoordinateArray&, std::int32_t>::value,
        "ArrayKey value constructor must accept coordinates and payload");

    const ArrayKey defaults;
    require(defaults.coordinates[0] == 0 && defaults.coordinates[1] == 0
        && defaults.payload == 0, "ArrayKey default values must be zero");
    const ArrayKey value = populated_value();
    require(value.coordinates[0] == 7 && value.coordinates[1] == -19
        && value.payload == 9001, "ArrayKey constructor must preserve values");
    const ArrayKey copied(value);
    require(copied == value && !(copied != value), "ArrayKey equality must preserve all values");
}

void check_dynamic_type()
{
    using dds::core::xtypes::TypeKind;
    const dds::core::xtypes::StructType& type = rti::topic::dynamic_type<ArrayKey>::get();
    require(type.name() == "CorpusKeyBoundaries::ArrayKey" && type.member_count() == 2,
        "ArrayKey DynamicType identity/count mismatch");
    require(type.member(0).name() == "coordinates"
        && type.member(0).type().kind() == TypeKind::ARRAY_TYPE
        && type.member(0).is_key(), "ArrayKey.coordinates DynamicType/key metadata mismatch");
    require(type.member(1).name() == "payload"
        && type.member(1).type().kind() == TypeKind::INT_32_TYPE
        && !type.member(1).is_key(), "ArrayKey.payload DynamicType/key metadata mismatch");

    const RTIXCdrTypeCode* native =
        reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_STRUCT && native->_memberCount == 2,
        "ArrayKey native TypeCode identity/count mismatch");
    require(native->_members[0]._memberFlags == RTI_XCDR_KEY_MEMBER,
        "ArrayKey.coordinates native TypeCode key flag mismatch");
    require(native->_members[1]._memberFlags == RTI_XCDR_REQUIRED_MEMBER,
        "ArrayKey.payload native TypeCode required flag mismatch");
}

void check_registration()
{
    dds::domain::DomainParticipant participant(68);
    const std::string type_name = dds::topic::topic_type_name<ArrayKey>::value();
    require(type_name == "CorpusKeyBoundaries::ArrayKey", "ArrayKey topic type name mismatch");
    dds::topic::topic_type_support<ArrayKey>::register_type(participant, type_name);
    const dds::topic::Topic<ArrayKey> topic(participant, "CorpusKeyBoundaries::ArrayKey");
    dds::pub::DataWriter<ArrayKey> writer(topic);

    ArrayKey first = populated_value();
    ArrayKey same_key = first;
    same_key.payload = 123;
    ArrayKey different_first_coordinate = first;
    different_first_coordinate.coordinates[0]++;
    ArrayKey different_second_coordinate = first;
    different_second_coordinate.coordinates[1]++;
    require(writer.register_instance(first) == writer.register_instance(same_key),
        "ArrayKey instance identity must ignore non-key payload");
    require(writer.register_instance(first) != writer.register_instance(different_first_coordinate),
        "ArrayKey instance identity must include coordinate 0");
    require(writer.register_instance(first) != writer.register_instance(different_second_coordinate),
        "ArrayKey instance identity must include coordinate 1");
}

void check_cdr(const std::string& representation, const std::string& path)
{
    using dds::core::policy::DataRepresentation;
    using dds::core::policy::DataRepresentationId;
    const DataRepresentationId format = representation == "xcdr1"
        ? DataRepresentation::xcdr() : DataRepresentation::xcdr2();
    const ArrayKey states[] = {ArrayKey(), populated_value()};
    std::ofstream output(path.c_str(), std::ios::binary | std::ios::trunc);
    require(static_cast<bool>(output), "Could not create CDR capture");
    for (const ArrayKey& value : states) {
        std::vector<char> buffer;
        dds::topic::topic_type_support<ArrayKey>::to_cdr_buffer(buffer, value, format);
        const std::uint32_t size = static_cast<std::uint32_t>(buffer.size());
        output.write(reinterpret_cast<const char*>(&size), sizeof(size));
        output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
        ArrayKey decoded;
        dds::topic::topic_type_support<ArrayKey>::from_cdr_buffer(decoded, buffer);
        require(decoded == value, "ArrayKey local CDR round trip changed a value");
    }
    require(static_cast<bool>(output), "Could not finish CDR capture");
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
    const dds::topic::Topic<ArrayKey> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{
        encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representation(ids);
    const std::string marker = std::string(argv[4]) + "/received";
    if (role == "writer") {
        const dds::pub::Publisher publisher(participant);
        auto qos = publisher.default_datawriter_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::pub::DataWriter<ArrayKey> writer(publisher, topic, qos);
        await([&] { return writer.publication_matched_status().current_count() == 1; }, "reader discovery");
        writer.write(populated_value());
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<ArrayKey> reader(subscriber, topic, qos);
        ArrayKey received;
        await([&] {
            const auto samples = reader.take();
            if (samples.length() == 0) { return false; }
            std::size_t valid_count = 0;
            for (auto sample = samples.begin(); sample != samples.end(); ++sample) {
                if (!sample->info().valid()) { continue; }
                received = sample->data();
                ++valid_count;
            }
            require(valid_count <= 1, "Received multiple valid ArrayKey samples");
            return valid_count == 1;
        }, "receiving ArrayKey sample");
        require(received == populated_value(), "Live DDS ArrayKey sample differed from expected value");
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
        std::cout << command << " passed for CorpusKeyBoundaries::ArrayKey" << std::endl;
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
