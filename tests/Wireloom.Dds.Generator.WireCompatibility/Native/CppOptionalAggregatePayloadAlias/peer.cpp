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
using CorpusOptionalAggregate::Payload;
using CorpusOptionalAggregate::PayloadAlias;
using CorpusOptionalAggregate::PayloadAlias_AliasTag_t;
static_assert(std::is_same<PayloadAlias, Payload>::value, "IDL typedef must alias the canonical C++ struct type");
static_assert(dds::topic::is_topic_type<PayloadAlias>::value, "PayloadAlias must inherit topic type support");
static_assert(std::is_same<decltype(PayloadAlias::value), std::int32_t>::value, "PayloadAlias member API mismatch");
static_assert(std::is_constructible<PayloadAlias, std::int32_t>::value, "PayloadAlias must expose the underlying constructor");
void require(bool value, const char* message) { if (!value) throw std::runtime_error(message); }
PayloadAlias sample(const std::string& shape) {
    if (shape == "defaults") return PayloadAlias();
    if (shape == "positive") return PayloadAlias(7301);
    if (shape == "negative") return PayloadAlias(-91);
    throw std::runtime_error("unknown PayloadAlias state");
}
void check_api() {
    require(dds::topic::topic_type_name<Payload>::value() == "CorpusOptionalAggregate::Payload"
        && dds::topic::topic_type_name<PayloadAlias>::value() == "CorpusOptionalAggregate::Payload",
        "C++ typedef topic name must resolve to canonical Payload");
    const PayloadAlias defaults;
    require(defaults.value == 0, "PayloadAlias default value mismatch");
    const PayloadAlias positive(7301), negative(-91);
    require(positive.value == 7301 && negative.value == -91, "PayloadAlias constructor/access mismatch");
    require(positive == PayloadAlias(7301) && positive != negative, "PayloadAlias equality mismatch");
}
void check_dynamic_type() {
    const auto& alias = rti::topic::dynamic_type<PayloadAlias_AliasTag_t>::get();
    require(alias.name() == "CorpusOptionalAggregate::PayloadAlias"
        && alias.related_type().name() == "CorpusOptionalAggregate::Payload"
        && alias.related_type().kind() == dds::core::xtypes::TypeKind::STRUCTURE_TYPE,
        "PayloadAlias DynamicType must relate to the canonical structure");
    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&alias.native());
    require(native->_kind == RTI_XCDR_TK_ALIAS && native->_typeCode != nullptr
        && native->_typeCode->_kind == RTI_XCDR_TK_STRUCT,
        "PayloadAlias native TypeCode must link to Payload structure TypeCode");
}
void check_registration() {
    dds::domain::DomainParticipant participant(72);
    const std::string name = dds::topic::topic_type_name<PayloadAlias>::value();
    require(name == "CorpusOptionalAggregate::Payload", "PayloadAlias registration must use canonical Payload name");
    dds::topic::topic_type_support<PayloadAlias>::register_type(participant, name);
    dds::topic::Topic<PayloadAlias> topic(participant, name);
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<PayloadAlias> writer(publisher, topic);
    require(writer.register_instance(PayloadAlias(1)) == writer.register_instance(PayloadAlias(2)),
        "unkeyed PayloadAlias values must share one instance handle");
}
void append_frame(std::ofstream& out, const PayloadAlias& expected, dds::core::policy::DataRepresentationId representation) {
    std::vector<char> bytes;
    dds::topic::topic_type_support<PayloadAlias>::to_cdr_buffer(bytes, expected, representation);
    PayloadAlias actual;
    dds::topic::topic_type_support<PayloadAlias>::from_cdr_buffer(actual, bytes);
    require(actual == expected, "PayloadAlias CDR roundtrip mismatch");
    const std::uint32_t size = static_cast<std::uint32_t>(bytes.size());
    out.write(reinterpret_cast<const char*>(&size), sizeof(size));
    out.write(bytes.data(), static_cast<std::streamsize>(bytes.size()));
}
void write_cdr(const std::string& path, const std::string& shape) {
    const PayloadAlias expected = sample(shape);
    std::ofstream out(path.c_str(), std::ios::binary | std::ios::trunc);
    append_frame(out, expected, dds::core::policy::DataRepresentation::xcdr());
    append_frame(out, expected, dds::core::policy::DataRepresentation::xcdr2());
    require(static_cast<bool>(out), "PayloadAlias CDR capture write failed");
}
void await(const std::function<bool()>& condition, const char* message) {
    const auto deadline = std::chrono::steady_clock::now() + std::chrono::seconds(15);
    while (!condition()) {
        if (std::chrono::steady_clock::now() >= deadline) throw std::runtime_error(message);
        std::this_thread::sleep_for(std::chrono::milliseconds(20));
    }
}
void run_live(int argc, char** argv) {
    require(argc == 5, "usage: peer <reader|writer> <xcdr1|xcdr2> <topic> <ack-dir>");
    const std::string role(argv[1]), encoding(argv[2]);
    require((role == "reader" || role == "writer") && (encoding == "xcdr1" || encoding == "xcdr2"), "invalid live arguments");
    using namespace dds::core::policy;
    const dds::domain::DomainParticipant participant(72);
    const dds::topic::Topic<PayloadAlias> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representation(ids);
    const std::string marker = std::string(argv[4]) + "/received";
    if (role == "writer") {
        const dds::pub::Publisher publisher(participant); auto qos = publisher.default_datawriter_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::pub::DataWriter<PayloadAlias> writer(publisher, topic, qos);
        await([&] { return writer.publication_matched_status().current_count() == 1; }, "reader discovery");
        writer.write(sample("defaults")); writer.write(sample("positive")); writer.write(sample("negative"));
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant); auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<PayloadAlias> reader(subscriber, topic, qos);
        bool defaults=false, positive=false, negative=false;
        await([&] { const auto values=reader.take(); for(auto it=values.begin();it!=values.end();++it){if(!it->info().valid())continue;const auto& v=it->data();if(v==sample("defaults"))defaults=true;else if(v==sample("positive"))positive=true;else if(v==sample("negative"))negative=true;else throw std::runtime_error("unexpected PayloadAlias value");} return defaults&&positive&&negative; }, "receiving PayloadAlias states");
        std::ofstream ack(marker.c_str()); require(static_cast<bool>(ack << "validated PayloadAlias states\n"), "writing alias reader acknowledgement");
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
