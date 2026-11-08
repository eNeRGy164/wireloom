#include "11-conditionals.hpp"

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
using CorpusConditionals::UndefBranch;
using dds::core::xtypes::ExtensibilityKind;
using dds::core::xtypes::TypeKind;
using Clock = std::chrono::steady_clock;

static_assert(dds::topic::is_topic_type<UndefBranch>::value,
    "UndefBranch must be recognized as a Modern C++ topic type");
static_assert(std::is_same<decltype(UndefBranch::value), std::int32_t>::value,
    "UndefBranch.value must be int32_t");
static_assert(std::is_constructible<UndefBranch, std::int32_t>::value,
    "UndefBranch constructor must accept the IDL field");
static_assert(rti::topic::extensibility<UndefBranch>::kind == ExtensibilityKind::EXTENSIBLE,
    "UndefBranch must use RTI's extensible profile");

void require(bool condition, const char* message)
{
    if (!condition) { throw std::runtime_error(message); }
}

void check_api()
{
    const UndefBranch defaults;
    require(defaults.value == 0, "UndefBranch default value must be zero");
    const UndefBranch assigned(27);
    require(assigned.value == 27, "UndefBranch must preserve its assigned value");
    require(UndefBranch(defaults) == defaults && !(defaults != UndefBranch()),
        "UndefBranch equality must preserve default values");
}

void check_dynamic_type()
{
    using namespace dds::core::xtypes;
    const StructType& type = rti::topic::dynamic_type<UndefBranch>::get();
    require(type.name() == "CorpusConditionals::UndefBranch" && type.member_count() == 1,
        "UndefBranch DynamicType identity/count mismatch");
    require(type.extensibility_kind() == ExtensibilityKind::EXTENSIBLE
        && is_extensible(type) && !is_final(type) && !is_mutable(type),
        "UndefBranch extensibility metadata mismatch");
    require(type.member(0).name() == "value" && type.member(0).get_id() == 0
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && !type.member(0).is_key(),
        "UndefBranch value member name, ID, type, or key metadata mismatch");
    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_STRUCT && native->_memberCount == 1,
        "UndefBranch native TypeCode kind/member count mismatch");
    const auto& annotations = native->_members[0]._annotations;
    require(annotations._defaultValue._d == RTI_XCDR_TK_LONG
        && annotations._defaultValue._u.long_value == 0
        && annotations._minValue._d == RTI_XCDR_TK_LONG
        && annotations._minValue._u.long_value == INT32_MIN
        && annotations._maxValue._d == RTI_XCDR_TK_LONG
        && annotations._maxValue._u.long_value == INT32_MAX,
        "UndefBranch native default/min/max metadata mismatch");
}

void check_registration()
{
    dds::domain::DomainParticipant participant(72);
    const std::string name = dds::topic::topic_type_name<UndefBranch>::value();
    require(name == "CorpusConditionals::UndefBranch", "UndefBranch topic name mismatch");
    dds::topic::topic_type_support<UndefBranch>::register_type(participant, name);
    const dds::topic::Topic<UndefBranch> topic(participant, name);
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<UndefBranch> writer(publisher, topic);
    require(writer.register_instance(UndefBranch(1)) == writer.register_instance(UndefBranch(2)),
        "Unkeyed UndefBranch values must use the same keyless instance handle");
}

void append_frame(std::ofstream& output, const UndefBranch& expected,
    dds::core::policy::DataRepresentationId representation)
{
    std::vector<char> buffer;
    dds::topic::topic_type_support<UndefBranch>::to_cdr_buffer(buffer, expected, representation);
    UndefBranch actual;
    dds::topic::topic_type_support<UndefBranch>::from_cdr_buffer(actual, buffer);
    require(actual == expected, "UndefBranch CDR round trip changed its value");
    const std::uint32_t size = static_cast<std::uint32_t>(buffer.size());
    output.write(reinterpret_cast<const char*>(&size), sizeof(size));
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    require(static_cast<bool>(output), "Could not write UndefBranch CDR frame");
}

void check_cdr(const std::string& encoding, const std::string& path)
{
    const auto representation = encoding == "xcdr1"
        ? dds::core::policy::DataRepresentation::xcdr()
        : dds::core::policy::DataRepresentation::xcdr2();
    const UndefBranch states[] = {UndefBranch(), UndefBranch(2), UndefBranch(27), UndefBranch(-1)};
    std::ofstream output(path.c_str(), std::ios::binary | std::ios::trunc);
    require(static_cast<bool>(output), "Could not create UndefBranch CDR capture");
    for (const UndefBranch& state : states) { append_frame(output, state, representation); }
    require(static_cast<bool>(output), "Could not finish UndefBranch CDR capture");
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
    const dds::domain::DomainParticipant participant(72);
    const dds::topic::Topic<UndefBranch> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{
        encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representation(ids);
    const std::string marker = std::string(argv[4]) + "/received";
    if (role == "writer") {
        const dds::pub::Publisher publisher(participant);
        auto qos = publisher.default_datawriter_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::pub::DataWriter<UndefBranch> writer(publisher, topic, qos);
        await([&] { return writer.publication_matched_status().current_count() == 1; }, "reader discovery");
        writer.write(UndefBranch());
        writer.write(UndefBranch(2));
        writer.write(UndefBranch(27));
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<UndefBranch> reader(subscriber, topic, qos);
        bool saw_default = false;
        bool saw_two = false;
        bool saw_twenty_seven = false;
        await([&] {
            const auto samples = reader.take();
            for (auto sample = samples.begin(); sample != samples.end(); ++sample) {
                if (!sample->info().valid()) { continue; }
                const UndefBranch& value = sample->data();
                if (value == UndefBranch()) { saw_default = true; }
                else if (value == UndefBranch(2)) { saw_two = true; }
                else if (value == UndefBranch(27)) { saw_twenty_seven = true; }
                else { throw std::runtime_error("Unexpected live UndefBranch value"); }
            }
            return saw_default && saw_two && saw_twenty_seven;
        }, "receiving UndefBranch branch values");
        std::ofstream acknowledgement(marker.c_str());
        require(static_cast<bool>(acknowledgement << "validated UndefBranch values\n"),
            "Could not acknowledge UndefBranch states");
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
        std::cout << command << " passed for CorpusConditionals::UndefBranch" << std::endl;
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
