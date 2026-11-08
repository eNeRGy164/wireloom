#include "09-optional-aggregate-member.hpp"

#include <chrono>
#include <cstdint>
#include <fstream>
#include <functional>
#include <stdexcept>
#include <string>
#include <type_traits>
#include <thread>
#include <vector>

#include <cdr/cdr_typeCode.h>
#include <dds/core/xtypes/TypeKind.hpp>
#include <dds/core/xtypes/UnionType.hpp>
#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/sub/DataReader.hpp>
#include <dds/sub/Subscriber.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace {
using CorpusOptionalAggregate::Choice;
using CorpusOptionalAggregate::Payload;
using dds::core::xtypes::TypeKind;
static_assert(dds::topic::is_topic_type<Choice>::value, "Choice must be a standalone topic type");
static_assert(std::is_same<decltype(std::declval<Choice&>().payload()), Payload&>::value, "payload accessor mismatch");
static_assert(std::is_same<decltype(std::declval<Choice&>().number()), std::int32_t&>::value, "number accessor mismatch");

void require(bool value, const char* message) { if (!value) throw std::runtime_error(message); }
Choice sample(const std::string& shape) {
    Choice value;
    if (shape == "payload") value.payload(Payload(7301));
    else if (shape == "number") value.number(27);
    else if (shape != "defaults") throw std::runtime_error("unknown sample shape");
    return value;
}
void check_api() {
    Choice value;
    require(Choice::default_discriminator() == 0 && value._d() == 0 && value.payload().value == 0,
        "default discriminator/payload branch mismatch");
    value.payload(Payload(7301));
    require(value._d() == 0 && value.payload().value == 7301, "aggregate payload setter mismatch");
    value.number(27);
    require(value._d() == 1 && value.number() == 27, "number setter mismatch");
}
void check_dynamic_type() {
    const auto& type = rti::topic::dynamic_type<Choice>::get();
    require(type.name() == "CorpusOptionalAggregate::Choice" && type.member_count() == 2,
        "Choice DynamicType identity/member count mismatch");
    require(type.discriminator().kind() == TypeKind::INT_32_TYPE, "Choice discriminator must be int32");
    require(type.member(0).name() == "payload" && type.member(0).type().kind() == TypeKind::STRUCTURE_TYPE
        && type.member(0).labels().size() == 1 && type.member(0).labels()[0] == 0,
        "optional aggregate payload branch metadata mismatch");
    require(type.member(1).name() == "number" && type.member(1).type().kind() == TypeKind::INT_32_TYPE
        && type.member(1).labels().size() == 1 && type.member(1).labels()[0] == 1,
        "number branch metadata mismatch");
    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_UNION && native->_memberCount == 2
        && native->_typeCode != nullptr && native->_typeCode->_kind == RTI_XCDR_TK_LONG,
        "native union/discriminator TypeCode mismatch");
    require(native->_members[0]._representation._typeCode != nullptr
        && native->_members[0]._representation._typeCode->_kind == RTI_XCDR_TK_STRUCT
        && native->_members[0]._memberFlags == 0
        && native->_members[1]._memberFlags == 0,
        "aggregate branch native TypeCode link missing");
}
void check_registration() {
    dds::domain::DomainParticipant participant(72);
    const std::string name = dds::topic::topic_type_name<Choice>::value();
    require(name == "CorpusOptionalAggregate::Choice", "Choice topic name mismatch");
    dds::topic::topic_type_support<Choice>::register_type(participant, name);
    dds::topic::Topic<Choice> topic(participant, name);
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Choice> writer(publisher, topic);
    (void)writer;
}
void write_cdr(const std::string& path, const std::string& shape) {
    const Choice expected = sample(shape);
    std::vector<char> bytes;
    rti::topic::to_cdr_buffer(bytes, expected, dds::core::policy::DataRepresentation::xcdr());
    const Choice actual = rti::topic::from_cdr_buffer<Choice>(bytes);
    require(actual == expected, "XCDR1 roundtrip mismatch");
    std::ofstream out(path.c_str(), std::ios::binary | std::ios::trunc);
    const std::uint32_t size = static_cast<std::uint32_t>(bytes.size());
    out.write(reinterpret_cast<const char*>(&size), sizeof(size)); out.write(bytes.data(), bytes.size());
    bytes.clear();
    rti::topic::to_cdr_buffer(bytes, expected, dds::core::policy::DataRepresentation::xcdr2());
    const std::uint32_t size2 = static_cast<std::uint32_t>(bytes.size());
    out.write(reinterpret_cast<const char*>(&size2), sizeof(size2)); out.write(bytes.data(), bytes.size());
    require(static_cast<bool>(out), "CDR output failed");
}
void await(const std::function<bool()>& condition, const char* description) {
    const auto deadline = std::chrono::steady_clock::now() + std::chrono::seconds(15);
    while (!condition()) {
        if (std::chrono::steady_clock::now() >= deadline) throw std::runtime_error(description);
        std::this_thread::sleep_for(std::chrono::milliseconds(20));
    }
}
void run_live(int argc, char** argv) {
    require(argc == 5, "usage: peer <reader|writer> <xcdr1|xcdr2> <topic> <ack-dir>");
    const std::string role(argv[1]), encoding(argv[2]);
    require((role == "reader" || role == "writer") && (encoding == "xcdr1" || encoding == "xcdr2"), "invalid live arguments");
    using namespace dds::core::policy;
    const dds::domain::DomainParticipant participant(72);
    const dds::topic::Topic<Choice> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representation(ids);
    const std::string marker = std::string(argv[4]) + "/received";
    if (role == "writer") {
        const dds::pub::Publisher publisher(participant);
        auto qos = publisher.default_datawriter_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::pub::DataWriter<Choice> writer(publisher, topic, qos);
        await([&] { return writer.publication_matched_status().current_count() == 1; }, "reader discovery");
        writer.write(sample("defaults")); writer.write(sample("payload")); writer.write(sample("number"));
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<Choice> reader(subscriber, topic, qos);
        bool defaults = false, payload = false, number = false;
        await([&] {
            const auto samples = reader.take();
            for (auto it = samples.begin(); it != samples.end(); ++it) {
                if (!it->info().valid()) continue;
                const Choice& value = it->data();
                if (value == sample("defaults")) defaults = true;
                else if (value == sample("payload")) payload = true;
                else if (value == sample("number")) number = true;
                else throw std::runtime_error("unexpected live Choice value");
            }
            return defaults && payload && number;
        }, "receiving Choice branch values");
        std::ofstream acknowledgement(marker.c_str());
        require(static_cast<bool>(acknowledgement << "validated Choice branches\n"), "writing reader acknowledgement");
    }
}
}
int main(int argc, char** argv) {
    try {
        if (argc >= 2 && (std::string(argv[1]) == "reader" || std::string(argv[1]) == "writer")) { run_live(argc, argv); return 0; }
        require(argc >= 2, "expected --api, --dynamic-type, --registration, or --cdr");
        const std::string command(argv[1]);
        if (command == "--api") check_api();
        else if (command == "--dynamic-type") check_dynamic_type();
        else if (command == "--registration") check_registration();
        else if (command == "--cdr" && argc == 4) write_cdr(argv[2], argv[3]);
        else throw std::runtime_error("invalid arguments");
        return 0;
    } catch (const std::exception& error) { std::cerr << error.what() << '\n'; return 1; }
}
