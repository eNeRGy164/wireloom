#include "11-preprocessing.hpp"

#include <chrono>
#include <cstdint>
#include <fstream>
#include <functional>
#include <iostream>
#include <stdexcept>
#include <thread>
#include <type_traits>
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
using ::CorpusPreprocessing::Sample;
using ::IncludedType;
using dds::core::xtypes::ExtensibilityKind;
using dds::core::xtypes::TypeKind;
using Clock = std::chrono::steady_clock;

static_assert(dds::topic::is_topic_type<Sample>::value,
    "Sample must be recognized as a Modern C++ topic type");
static_assert(std::is_same<decltype(IncludedType::value), std::int32_t>::value,
    "IncludedType.value must be int32_t");
static_assert(std::is_same<decltype(Sample::item), IncludedType>::value,
    "Sample.item must use the included IDL struct");
static_assert(std::is_same<decltype(Sample::value), std::int32_t>::value,
    "Sample.value must be int32_t");
static_assert(std::is_constructible<Sample, const IncludedType&, std::int32_t>::value,
    "Sample constructor must accept both IDL fields");
static_assert(rti::topic::extensibility<Sample>::kind == ExtensibilityKind::EXTENSIBLE,
    "Sample must use RTI's extensible profile");

void require(bool condition, const char* message)
{
    if (!condition) { throw std::runtime_error(message); }
}

Sample make_value(std::int32_t item, std::int32_t value)
{
    return Sample(IncludedType(item), value);
}

void check_api()
{
    require(IncludedConstant == 7 && ::BranchValue == 8,
        "Included constant or preprocessed BranchValue mismatch");
    const Sample defaults;
    require(defaults.item.value == 0 && defaults.value == 0,
        "Sample default fields must initialize nested item and value to zero");
    const Sample populated = make_value(17, -42);
    require(populated.item.value == 17 && populated.value == -42,
        "Sample fields must preserve nested and scalar values");
    require(Sample(defaults) == defaults && !(defaults != Sample()),
        "Sample equality must preserve default values");
}

void check_dynamic_type()
{
    using namespace dds::core::xtypes;
    const StructType& nested = rti::topic::dynamic_type<IncludedType>::get();
    require(nested.name() == "IncludedType" && nested.member_count() == 1
        && nested.member(0).name() == "value" && nested.member(0).get_id() == 0
        && nested.member(0).type().kind() == TypeKind::INT_32_TYPE,
        "IncludedType nested DynamicType mismatch");

    const StructType& type = rti::topic::dynamic_type<Sample>::get();
    require(type.name() == "CorpusPreprocessing::Sample" && type.member_count() == 2,
        "Sample DynamicType identity/count mismatch");
    require(type.extensibility_kind() == ExtensibilityKind::EXTENSIBLE
        && is_extensible(type) && !is_final(type) && !is_mutable(type),
        "Sample extensibility metadata mismatch");
    require(type.member(0).name() == "item" && type.member(0).get_id() == 0
        && type.member(0).type().kind() == TypeKind::STRUCTURE_TYPE
        && type.member(0).type().name() == "IncludedType",
        "Nested item member name, ID, or type metadata mismatch");
    require(type.member(1).name() == "value" && type.member(1).get_id() == 1
        && type.member(1).type().kind() == TypeKind::INT_32_TYPE,
        "value member name, ID, or type metadata mismatch");
}

void check_registration()
{
    dds::domain::DomainParticipant participant(70);
    const std::string name = dds::topic::topic_type_name<Sample>::value();
    require(name == "CorpusPreprocessing::Sample", "Sample topic type name mismatch");
    dds::topic::topic_type_support<Sample>::register_type(participant, name);
    const dds::topic::Topic<Sample> topic(participant, name);
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Sample> writer(publisher, topic);
    require(writer.register_instance(make_value(1, 2))
        == writer.register_instance(make_value(3, 4)),
        "Unkeyed Sample values must use the same keyless instance handle");
}

void append_frame(std::ofstream& output, const Sample& expected,
    dds::core::policy::DataRepresentationId representation)
{
    std::vector<char> buffer;
    dds::topic::topic_type_support<Sample>::to_cdr_buffer(buffer, expected, representation);
    Sample actual;
    dds::topic::topic_type_support<Sample>::from_cdr_buffer(actual, buffer);
    require(actual == expected, "Sample CDR round trip changed nested/scalar values");
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
    const Sample states[] = {Sample(), make_value(7, 8), make_value(1, -42), make_value(9, 0)};
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
    const dds::domain::DomainParticipant participant(70);
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
        writer.write(make_value(7, 8));
        writer.write(make_value(1, -42));
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<Sample> reader(subscriber, topic, qos);
        bool saw_default = false;
        bool saw_included = false;
        bool saw_alternate = false;
        await([&] {
            const auto samples = reader.take();
            for (auto sample = samples.begin(); sample != samples.end(); ++sample) {
                if (!sample->info().valid()) { continue; }
                const Sample& value = sample->data();
                if (value == Sample()) { saw_default = true; }
                else if (value == make_value(7, 8)) { saw_included = true; }
                else if (value == make_value(1, -42)) { saw_alternate = true; }
                else { throw std::runtime_error("Unexpected live Sample nested/scalar state"); }
            }
            return saw_default && saw_included && saw_alternate;
        }, "receiving default and nested Sample states");
        std::ofstream acknowledgement(marker.c_str());
        require(static_cast<bool>(acknowledgement << "validated nested/default Sample states\n"),
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
        std::cout << command << " passed for CorpusPreprocessing::Sample" << std::endl;
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
