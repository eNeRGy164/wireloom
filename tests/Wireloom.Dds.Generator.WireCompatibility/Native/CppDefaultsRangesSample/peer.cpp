#include "09-defaults-ranges.hpp"

#include <chrono>
#include <cstdint>
#include <fstream>
#include <functional>
#include <iostream>
#include <stdexcept>
#include <thread>
#include <type_traits>
#include <vector>

#include <cdr/cdr_typeCode.h>
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
using CorpusDefaultsAndRanges::Color;
using CorpusDefaultsAndRanges::Sample;
using dds::core::xtypes::ExtensibilityKind;
using dds::core::xtypes::TypeKind;
using Clock = std::chrono::steady_clock;

static_assert(dds::topic::is_topic_type<Sample>::value,
    "Sample must be recognized as a Modern C++ topic type");
static_assert(std::is_same<decltype(Sample::value), std::int32_t>::value,
    "Sample.value must be int32_t");
static_assert(std::is_same<decltype(Sample::ranged), std::int32_t>::value,
    "Sample.ranged must be int32_t");
static_assert(std::is_same<decltype(Sample::color), Color>::value,
    "Sample.color must use CorpusDefaultsAndRanges::Color");
static_assert(std::is_constructible<Sample, std::int32_t, std::int32_t, const Color&>::value,
    "Sample constructor must accept the three IDL fields");
static_assert(rti::topic::extensibility<Sample>::kind == ExtensibilityKind::EXTENSIBLE,
    "@appendable Sample must use RTI's appendable profile");

void require(bool condition, const char* message)
{
    if (!condition) { throw std::runtime_error(message); }
}

Sample make_value(std::int32_t value, std::int32_t ranged, Color color)
{
    return Sample(value, ranged, color);
}

void check_api()
{
    const Sample defaults;
    require(defaults.value == 50 && defaults.ranged == 0 && defaults.color == Color::BLUE,
        "Sample default fields must be value=50, ranged=0, color=BLUE");
    const Sample minimum = make_value(0, -32, Color::GREEN);
    require(minimum.value == 0 && minimum.ranged == -32,
        "Sample minimum inclusive values must be representable");
    const Sample maximum = make_value(100, 31, Color::RED);
    require(maximum.value == 100 && maximum.ranged == 31,
        "Sample maximum inclusive values must be representable");
    const Sample copy(maximum);
    require(copy == maximum && !(copy != maximum), "Sample equality must preserve field values");
}

void check_dynamic_type()
{
    using namespace dds::core::xtypes;
    const StructType& type = rti::topic::dynamic_type<Sample>::get();
    require(type.name() == "CorpusDefaultsAndRanges::Sample" && type.member_count() == 3,
        "Sample DynamicType identity/count mismatch");
    require(type.extensibility_kind() == ExtensibilityKind::EXTENSIBLE
        && is_extensible(type) && !is_final(type) && !is_mutable(type),
        "Sample appendable DynamicType mismatch");
    require(type.member(0).name() == "value" && type.member(0).get_id() == 0
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE,
        "value member ID/type mismatch");
    require(type.member(1).name() == "ranged" && type.member(1).get_id() == 1
        && type.member(1).type().kind() == TypeKind::INT_32_TYPE,
        "ranged member ID/type mismatch");
    require(type.member(2).name() == "color" && type.member(2).get_id() == 2
        && type.member(2).type().kind() == TypeKind::ENUMERATION_TYPE,
        "color member ID/type mismatch");

    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_STRUCT && native->_memberCount == 3,
        "Sample native TypeCode kind/member count mismatch");
    const auto& value_annotations = native->_members[0]._annotations;
    const auto& ranged_annotations = native->_members[1]._annotations;
    const auto& color_annotations = native->_members[2]._annotations;
    std::cout << "native.value.default=" << value_annotations._defaultValue._d << ':'
        << value_annotations._defaultValue._u.long_value
        << ",min=" << value_annotations._minValue._d << ':'
        << value_annotations._minValue._u.long_value
        << ",max=" << value_annotations._maxValue._d << ':'
        << value_annotations._maxValue._u.long_value << '\n'
        << "native.ranged.default=" << ranged_annotations._defaultValue._d << ':'
        << ranged_annotations._defaultValue._u.long_value
        << ",min=" << ranged_annotations._minValue._d << ':'
        << ranged_annotations._minValue._u.long_value
        << ",max=" << ranged_annotations._maxValue._d << ':'
        << ranged_annotations._maxValue._u.long_value << '\n'
        << "native.color.default=" << color_annotations._defaultValue._d << ':'
        << color_annotations._defaultValue._u.enumerated_value << std::endl;
    require(value_annotations._defaultValue._d == RTI_XCDR_TK_LONG
        && value_annotations._defaultValue._u.long_value == 50
        && value_annotations._minValue._d == RTI_XCDR_TK_LONG
        && value_annotations._minValue._u.long_value == 0
        && value_annotations._maxValue._d == RTI_XCDR_TK_LONG
        && value_annotations._maxValue._u.long_value == 100,
        "value default/min/max metadata mismatch");
    require(ranged_annotations._defaultValue._d == RTI_XCDR_TK_LONG
        && ranged_annotations._defaultValue._u.long_value == 0
        && ranged_annotations._minValue._d == RTI_XCDR_TK_LONG
        && ranged_annotations._minValue._u.long_value == -32
        && ranged_annotations._maxValue._d == RTI_XCDR_TK_LONG
        && ranged_annotations._maxValue._u.long_value == 31,
        "ranged default/range metadata mismatch");
    require(color_annotations._defaultValue._d == RTI_XCDR_TK_ENUM
        && color_annotations._defaultValue._u.enumerated_value == 2,
        "color @default(BLUE) metadata mismatch");
}

void check_registration()
{
    dds::domain::DomainParticipant participant(68);
    const std::string name = dds::topic::topic_type_name<Sample>::value();
    require(name == "CorpusDefaultsAndRanges::Sample", "Sample topic type name mismatch");
    dds::topic::topic_type_support<Sample>::register_type(participant, name);
    const dds::topic::Topic<Sample> topic(participant, name);
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Sample> writer(publisher, topic);
    require(writer.register_instance(make_value(1, 2, Color::GREEN))
        == writer.register_instance(make_value(3, 4, Color::BLUE)),
        "Sample without @key members must use a keyless instance handle");
}

void append_frame(std::ofstream& output, const Sample& expected,
    dds::core::policy::DataRepresentationId representation)
{
    std::vector<char> buffer;
    dds::topic::topic_type_support<Sample>::to_cdr_buffer(buffer, expected, representation);
    Sample actual;
    dds::topic::topic_type_support<Sample>::from_cdr_buffer(actual, buffer);
    require(actual == expected, "Sample CDR round trip changed default/range values");
    const std::uint32_t size = static_cast<std::uint32_t>(buffer.size());
    output.write(reinterpret_cast<const char*>(&size), sizeof(size));
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    require(static_cast<bool>(output), "Could not write Sample CDR frame");
}

void check_cdr(const std::string& encoding, const std::string& path)
{
    const auto representation = encoding == "xcdr1"
        ? dds::core::policy::DataRepresentation::xcdr()
        : dds::core::policy::DataRepresentation::xcdr2();
    const Sample states[] = {
        Sample(), make_value(0, -32, Color::GREEN), make_value(100, 31, Color::RED),
        make_value(42, -1, Color::BLUE)};
    std::ofstream output(path.c_str(), std::ios::binary | std::ios::trunc);
    require(static_cast<bool>(output), "Could not create Sample CDR capture");
    for (const Sample& state : states) { append_frame(output, state, representation); }
    require(static_cast<bool>(output), "Could not finish Sample CDR capture");
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
    const dds::topic::Topic<Sample> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{
        encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representation(ids);
    const std::string marker = std::string(argv[4]) + "/received";
    if (role == "writer") {
        const dds::pub::Publisher publisher(participant);
        auto qos = publisher.default_datawriter_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::pub::DataWriter<Sample> writer(publisher, topic, qos);
        await([&] { return writer.publication_matched_status().current_count() == 1; }, "reader discovery");
        writer.write(Sample());
        writer.write(make_value(0, -32, Color::GREEN));
        writer.write(make_value(100, 31, Color::RED));
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<Sample> reader(subscriber, topic, qos);
        bool saw_default = false;
        bool saw_minimum = false;
        bool saw_maximum = false;
        await([&] {
            const auto samples = reader.take();
            for (auto sample = samples.begin(); sample != samples.end(); ++sample) {
                if (!sample->info().valid()) { continue; }
                const Sample& value = sample->data();
                if (value == Sample()) { saw_default = true; }
                else if (value == make_value(0, -32, Color::GREEN)) { saw_minimum = true; }
                else if (value == make_value(100, 31, Color::RED)) { saw_maximum = true; }
                else { throw std::runtime_error("Unexpected live Sample range/default state"); }
            }
            return saw_default && saw_minimum && saw_maximum;
        }, "receiving default and range boundary Samples");
        std::ofstream acknowledgement(marker.c_str());
        require(static_cast<bool>(acknowledgement << "validated defaults and inclusive boundaries\n"),
            "Could not acknowledge Sample states");
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
        std::cout << command << " passed for CorpusDefaultsAndRanges::Sample" << std::endl;
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
