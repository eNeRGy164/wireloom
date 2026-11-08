#include "09-optional-aggregate-member.hpp"

#include <chrono>
#include <cstdint>
#include <fstream>
#include <functional>
#include <stdexcept>
#include <string>
#include <thread>
#include <type_traits>
#include <vector>

#include <cdr/cdr_typeCode.h>
#include <dds/core/xtypes/AliasType.hpp>
#include <dds/core/xtypes/TypeKind.hpp>
#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/sub/DataReader.hpp>
#include <dds/sub/Subscriber.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace {
using CorpusOptionalAggregate::Choice;
using CorpusOptionalAggregate::ChoiceAlias;
using CorpusOptionalAggregate::ChoiceAlias_AliasTag_t;
using CorpusOptionalAggregate::Payload;
static_assert(std::is_same<ChoiceAlias, Choice>::value, "IDL typedef must alias the canonical C++ union type");
static_assert(dds::topic::is_topic_type<Choice>::value, "canonical Choice must be a Modern C++ topic type");
static_assert(dds::topic::is_topic_type<ChoiceAlias>::value, "ChoiceAlias must inherit topic-type support from Choice");
static_assert(std::is_same<decltype(std::declval<ChoiceAlias&>().payload()), Payload&>::value, "alias payload API mismatch");
static_assert(std::is_same<decltype(std::declval<ChoiceAlias&>().number()), std::int32_t&>::value, "alias number API mismatch");
void require(bool value, const char* message) { if (!value) throw std::runtime_error(message); }
ChoiceAlias sample(const std::string& shape) {
    ChoiceAlias value;
    if (shape == "payload") value.payload(Payload(7301));
    else if (shape == "number") value.number(27);
    else if (shape != "defaults") throw std::runtime_error("unknown sample shape");
    return value;
}
void check_api() {
    require(dds::topic::topic_type_name<Choice>::value() == "CorpusOptionalAggregate::Choice"
        && dds::topic::topic_type_name<ChoiceAlias>::value() == "CorpusOptionalAggregate::Choice",
        "Choice and its typedef must resolve to the canonical topic type name");
    ChoiceAlias value;
    require(ChoiceAlias::default_discriminator() == 0 && value._d() == 0 && value.payload().value == 0,
        "alias default branch mismatch");
    value.payload(Payload(7301));
    require(value._d() == 0 && value.payload().value == 7301, "alias aggregate branch setter mismatch");
    value.number(27);
    require(value._d() == 1 && value.number() == 27, "alias number branch setter mismatch");
}
void check_dynamic_type() {
    const auto& alias = rti::topic::dynamic_type<ChoiceAlias_AliasTag_t>::get();
    require(alias.name() == "CorpusOptionalAggregate::ChoiceAlias"
        && alias.related_type().name() == "CorpusOptionalAggregate::Choice"
        && alias.related_type().kind() == dds::core::xtypes::TypeKind::UNION_TYPE,
        "ChoiceAlias DynamicType must relate to the canonical union");
    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&alias.native());
    require(native->_kind == RTI_XCDR_TK_ALIAS && native->_typeCode != nullptr
        && native->_typeCode->_kind == RTI_XCDR_TK_UNION,
        "ChoiceAlias native TypeCode must link to union TypeCode");
}
void check_registration() {
    dds::domain::DomainParticipant participant(72);
    const std::string name = dds::topic::topic_type_name<ChoiceAlias>::value();
    require(name == "CorpusOptionalAggregate::Choice", "C++ typedef topic name must resolve to canonical Choice");
    dds::topic::topic_type_support<ChoiceAlias>::register_type(participant, name);
    dds::topic::Topic<ChoiceAlias> topic(participant, name);
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<ChoiceAlias> writer(publisher, topic);
    (void)writer;
}
void write_cdr(const std::string& path, const std::string& shape) {
    const ChoiceAlias expected = sample(shape);
    std::vector<char> bytes;
    rti::topic::to_cdr_buffer(bytes, expected, dds::core::policy::DataRepresentation::xcdr());
    require(rti::topic::from_cdr_buffer<ChoiceAlias>(bytes) == expected, "alias XCDR1 roundtrip mismatch");
    std::ofstream out(path.c_str(), std::ios::binary | std::ios::trunc);
    const std::uint32_t size = static_cast<std::uint32_t>(bytes.size());
    out.write(reinterpret_cast<const char*>(&size), sizeof(size)); out.write(bytes.data(), bytes.size());
    bytes.clear();
    rti::topic::to_cdr_buffer(bytes, expected, dds::core::policy::DataRepresentation::xcdr2());
    const std::uint32_t size2 = static_cast<std::uint32_t>(bytes.size());
    out.write(reinterpret_cast<const char*>(&size2), sizeof(size2)); out.write(bytes.data(), bytes.size());
    require(static_cast<bool>(out), "alias CDR output failed");
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
    const dds::topic::Topic<ChoiceAlias> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representation(ids);
    const std::string marker = std::string(argv[4]) + "/received";
    if (role == "writer") {
        const dds::pub::Publisher publisher(participant); auto qos = publisher.default_datawriter_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::pub::DataWriter<ChoiceAlias> writer(publisher, topic, qos);
        await([&] { return writer.publication_matched_status().current_count() == 1; }, "reader discovery");
        writer.write(sample("defaults")); writer.write(sample("payload")); writer.write(sample("number"));
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant); auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<ChoiceAlias> reader(subscriber, topic, qos);
        bool defaults=false, payload=false, number=false;
        await([&] { const auto values=reader.take(); for(auto it=values.begin();it!=values.end();++it){if(!it->info().valid())continue;const ChoiceAlias& v=it->data();if(v==sample("defaults"))defaults=true;else if(v==sample("payload"))payload=true;else if(v==sample("number"))number=true;else throw std::runtime_error("unexpected alias sample");} return defaults&&payload&&number; }, "receiving alias branch values");
        std::ofstream ack(marker.c_str()); require(static_cast<bool>(ack << "validated ChoiceAlias branches\n"), "writing alias reader acknowledgement");
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
