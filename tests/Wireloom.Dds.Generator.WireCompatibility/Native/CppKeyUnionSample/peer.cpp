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

#include <dds/core/xtypes/StructType.hpp>
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
using CorpusUnionKeys::Sample;
using dds::core::xtypes::TypeKind;
using Clock = std::chrono::steady_clock;

static_assert(dds::topic::is_topic_type<Sample>::value,
    "Sample must be recognized as a Modern C++ topic type");
static_assert(std::is_same<decltype(Sample::identity), Identity>::value,
    "Sample.identity must expose the nested Identity union");
static_assert(std::is_same<decltype(Sample::payload), std::int32_t>::value,
    "Sample.payload must expose int32_t");
static_assert(std::is_constructible<Sample, const Identity&, std::int32_t>::value,
    "Sample value constructor must accept Identity and payload");
static_assert(std::is_same<decltype(std::declval<Identity&>()._d()), std::int32_t&>::value,
    "Identity discriminator API must use int32_t");

void require(bool condition, const char* message)
{
    if (!condition) { throw std::runtime_error(message); }
}

Identity make_identity(const std::string& branch, std::int32_t number,
    const std::string& text)
{
    Identity value;
    if (branch == "number") { value.number(number); }
    else if (branch == "text") { value.text(text); }
    else { throw std::runtime_error("unknown Identity branch"); }
    return value;
}

Sample make_sample(const std::string& branch, std::int32_t number,
    const std::string& text, std::int32_t payload)
{
    return Sample(make_identity(branch, number, text), payload);
}

void check_api()
{
    const Sample defaults;
    require(defaults.identity._d() == 0 && defaults.identity.number() == 0
        && defaults.payload == 0,
        "default Sample must contain default Identity and zero payload");
    const Sample number = make_sample("number", 7301, "", 19);
    require(number.identity._d() == 0 && number.identity.number() == 7301
        && number.payload == 19,
        "Sample constructor must preserve the nested number-key branch");
    const Sample text = make_sample("text", 0, "nested-identity-key", 23);
    require(text.identity._d() == 1 && text.identity.text() == "nested-identity-key"
        && text.payload == 23,
        "Sample constructor must preserve the nested text-key branch");
    const Sample copy(text);
    require(copy == text && !(copy != text), "Sample equality must include nested union state");
}

void check_dynamic_type()
{
    const dds::core::xtypes::StructType& type = rti::topic::dynamic_type<Sample>::get();
    require(type.name() == "CorpusUnionKeys::Sample" && type.member_count() == 2,
        "Sample DynamicType identity/count mismatch");
    require(type.member(0).name() == "identity" && type.member(0).is_key()
        && type.member(0).type().kind() == TypeKind::UNION_TYPE
        && type.member(0).type().name() == "CorpusUnionKeys::Identity",
        "Sample.identity must be a key member linked to Identity union DynamicType");
    const auto& identity_type =
        static_cast<const dds::core::xtypes::UnionType&>(type.member(0).type());
    require(identity_type.member_count() == 2
        && identity_type.discriminator().kind() == TypeKind::INT_32_TYPE
        && identity_type.member(0).name() == "number"
        && identity_type.member(0).labels().size() == 1
        && identity_type.member(0).labels()[0] == 0
        && identity_type.member(1).name() == "text"
        && identity_type.member(1).labels().size() == 1
        && identity_type.member(1).labels()[0] == 1,
        "Nested Identity discriminator or branch metadata mismatch");
    require(type.member(1).name() == "payload"
        && type.member(1).type().kind() == TypeKind::INT_32_TYPE
        && !type.member(1).is_key(),
        "Sample.payload must remain a non-key int32 member");

    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    const auto* identity_native = reinterpret_cast<const RTIXCdrTypeCode*>(
        &identity_type.native());
    require(native->_kind == RTI_XCDR_TK_STRUCT && native->_memberCount == 2
        && native->_members[0]._memberFlags == RTI_XCDR_KEY_MEMBER
        && native->_members[1]._memberFlags == RTI_XCDR_REQUIRED_MEMBER,
        "Sample native TypeCode key/required flags mismatch");
    const auto* nested_native = native->_members[0]._representation._typeCode;
    require(nested_native != nullptr && nested_native == identity_native
        && nested_native->_kind == RTI_XCDR_TK_UNION
        && nested_native->_memberCount == 2
        && nested_native->_sampleAccessInfo != nullptr
        && nested_native->_typePlugin != nullptr,
        "Sample.identity native TypeCode must link to the complete Identity union plugin graph");
}

void check_registration()
{
    dds::domain::DomainParticipant participant(68);
    const std::string name = dds::topic::topic_type_name<Sample>::value();
    require(name == "CorpusUnionKeys::Sample", "Sample topic type name mismatch");
    dds::topic::topic_type_support<Sample>::register_type(participant, name);
    const dds::topic::Topic<Sample> topic(participant, name);
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Sample> writer(publisher, topic);

    const auto number = make_sample("number", 27, "", 1);
    const auto same_number_key = make_sample("number", 27, "", 2);
    const auto other_number_key = make_sample("number", 28, "", 1);
    const auto text = make_sample("text", 0, "nested-identity-key-long", 1);
    const auto same_text_key = make_sample("text", 0, "nested-identity-key-long", 2);
    const auto other_text_key = make_sample("text", 0, "nested-identity-key-other", 1);
    require(writer.register_instance(number) == writer.register_instance(same_number_key),
        "Sample nested number key must ignore payload");
    require(writer.register_instance(number) != writer.register_instance(other_number_key),
        "Sample nested number key must distinguish the number branch value");
    require(writer.register_instance(number) != writer.register_instance(text),
        "Sample nested union key must distinguish discriminator branches");
    require(writer.register_instance(text) == writer.register_instance(same_text_key),
        "Sample nested text key must ignore payload");
    require(writer.register_instance(text) != writer.register_instance(other_text_key),
        "Sample nested text key must distinguish active branch string values");
}

void append_frame(std::ofstream& output, const Sample& expected,
    dds::core::policy::DataRepresentationId representation)
{
    std::vector<char> buffer;
    dds::topic::topic_type_support<Sample>::to_cdr_buffer(buffer, expected, representation);
    Sample actual;
    dds::topic::topic_type_support<Sample>::from_cdr_buffer(actual, buffer);
    require(actual == expected, "Sample CDR round trip changed nested Identity key state");
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
        Sample(),
        make_sample("number", 7301, "", 19),
        make_sample("text", 0, "nested-identity-key", 23)
    };
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
        writer.write(make_sample("text", 0, "live-nested-identity", 9101));
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<Sample> reader(subscriber, topic, qos);
        Sample received;
        await([&] {
            const auto samples = reader.take();
            if (samples.length() == 0) { return false; }
            std::size_t valid_count = 0;
            for (auto sample = samples.begin(); sample != samples.end(); ++sample) {
                if (!sample->info().valid()) { continue; }
                received = sample->data();
                ++valid_count;
            }
            require(valid_count <= 1, "Received multiple valid Sample values");
            return valid_count == 1;
        }, "receiving Sample");
        require(received == make_sample("text", 0, "live-nested-identity", 9101),
            "Live Sample nested Identity key differed from expected value");
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
        std::cout << command << " passed for CorpusUnionKeys::Sample" << std::endl;
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
