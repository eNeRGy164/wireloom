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

#include <dds/core/xtypes/StructType.hpp>
#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/sub/DataReader.hpp>
#include <dds/sub/Subscriber.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace
{
using CorpusExtensibility::Appendable;
using dds::core::xtypes::ExtensibilityKind;
using dds::core::xtypes::TypeKind;
using Clock = std::chrono::steady_clock;

static_assert(dds::topic::is_topic_type<Appendable>::value,
    "Appendable must be recognized as a Modern C++ topic type");
static_assert(std::is_same<decltype(Appendable::id), std::int32_t>::value,
    "Appendable.id must be int32_t");
static_assert(std::is_same<decltype(Appendable::text), std::string>::value,
    "Appendable.text must be std::string");
static_assert(std::is_constructible<Appendable, std::int32_t, const std::string&>::value,
    "Appendable constructor must accept id and std::string");
static_assert(rti::topic::extensibility<Appendable>::kind == ExtensibilityKind::EXTENSIBLE,
    "Appendable topic trait must map to RTI's extensible profile");

void require(bool condition, const char* message)
{
    if (!condition) { throw std::runtime_error(message); }
}

Appendable make_value(std::int32_t id, const std::string& text)
{
    return Appendable(id, text);
}

void check_api()
{
    const Appendable defaults;
    require(defaults.id == 0 && defaults.text.empty(),
        "Appendable default values must be zero and empty");
    const Appendable value = make_value(7301, "appendable");
    require(value.id == 7301 && value.text == "appendable",
        "Appendable constructor must preserve values");
    const Appendable copy(value);
    require(copy == value && !(copy != value), "Appendable equality must preserve values");
}

void check_dynamic_type()
{
    using namespace dds::core::xtypes;
    const StructType& type = rti::topic::dynamic_type<Appendable>::get();
    require(type.name() == "CorpusExtensibility::Appendable" && type.member_count() == 2,
        "Appendable DynamicType identity/count mismatch");
    require(type.extensibility_kind() == ExtensibilityKind::EXTENSIBLE
        && is_extensible(type) && !is_final(type) && !is_mutable(type),
        "Appendable DynamicType extensibility mismatch");
    require(type.member(0).name() == "id" && type.member(0).get_id() == 1
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE,
        "Appendable id member metadata mismatch");
    require(type.member(1).name() == "text" && type.member(1).get_id() == 2
        && type.member(1).type().kind() == TypeKind::STRING_TYPE,
        "Appendable text member metadata mismatch");
    require(type.member(1).type().native()._data._maximumLength == 16,
        "Appendable text must preserve the string<16> bound");
    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_STRUCT && native->_memberCount == 2
        && native->_members[0]._memberFlags == RTI_XCDR_REQUIRED_MEMBER
        && native->_members[1]._memberFlags == RTI_XCDR_REQUIRED_MEMBER,
        "Appendable native TypeCode member metadata mismatch");
}

void check_registration()
{
    dds::domain::DomainParticipant participant(68);
    const std::string name = dds::topic::topic_type_name<Appendable>::value();
    require(name == "CorpusExtensibility::Appendable", "Appendable topic type name mismatch");
    dds::topic::topic_type_support<Appendable>::register_type(participant, name);
    const dds::topic::Topic<Appendable> topic(participant, name);
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Appendable> writer(publisher, topic);
    const auto first = writer.register_instance(make_value(1, "one"));
    const auto second = writer.register_instance(make_value(2, "two"));
    require(first == second, "Appendable without @key members must use a keyless instance handle");
}

void append_frame(std::ofstream& output, const Appendable& expected,
    dds::core::policy::DataRepresentationId representation)
{
    std::vector<char> buffer;
    dds::topic::topic_type_support<Appendable>::to_cdr_buffer(buffer, expected, representation);
    Appendable actual;
    dds::topic::topic_type_support<Appendable>::from_cdr_buffer(actual, buffer);
    require(actual == expected, "Appendable CDR round trip changed values");
    const std::uint32_t size = static_cast<std::uint32_t>(buffer.size());
    output.write(reinterpret_cast<const char*>(&size), sizeof(size));
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    require(static_cast<bool>(output), "Could not write Appendable CDR frame");
}

void check_cdr(const std::string& encoding, const std::string& path)
{
    const auto representation = encoding == "xcdr1"
        ? dds::core::policy::DataRepresentation::xcdr()
        : dds::core::policy::DataRepresentation::xcdr2();
    const Appendable states[] = {
        Appendable(), make_value(7301, "appendable"), make_value(-17, std::string(16, 'a'))};
    std::ofstream output(path.c_str(), std::ios::binary | std::ios::trunc);
    require(static_cast<bool>(output), "Could not create Appendable CDR capture");
    for (const Appendable& state : states) { append_frame(output, state, representation); }
    require(static_cast<bool>(output), "Could not finish Appendable CDR capture");
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
    const dds::topic::Topic<Appendable> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{
        encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representation(ids);
    const std::string marker = std::string(argv[4]) + "/received";
    if (role == "writer") {
        const dds::pub::Publisher publisher(participant);
        auto qos = publisher.default_datawriter_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::pub::DataWriter<Appendable> writer(publisher, topic, qos);
        await([&] { return writer.publication_matched_status().current_count() == 1; }, "reader discovery");
        writer.write(make_value(7301, "appendable-live"));
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<Appendable> reader(subscriber, topic, qos);
        Appendable received;
        await([&] {
            const auto samples = reader.take();
            if (samples.length() == 0) { return false; }
            std::size_t valid_count = 0;
            for (auto sample = samples.begin(); sample != samples.end(); ++sample) {
                if (!sample->info().valid()) { continue; }
                received = sample->data();
                ++valid_count;
            }
            require(valid_count <= 1, "Received multiple valid Appendable samples");
            return valid_count == 1;
        }, "receiving Appendable sample");
        require(received == make_value(7301, "appendable-live"),
            "Live Appendable sample differed from expected value");
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
        std::cout << command << " passed for CorpusExtensibility::Appendable" << std::endl;
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
