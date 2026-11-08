#include "09-optional-aggregate-member.hpp"

#include <chrono>
#include <cstdint>
#include <fstream>
#include <functional>
#include <stdexcept>
#include <string>
#include <thread>
#include <type_traits>
#include <utility>
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

namespace {
using CorpusOptionalAggregate::Payload;
using dds::core::xtypes::TypeKind;
static_assert(dds::topic::is_topic_type<Payload>::value, "Payload must be a Modern C++ topic type");
static_assert(std::is_same<decltype(Payload::value), std::int32_t>::value, "Payload.value must be int32");
static_assert(std::is_constructible<Payload, std::int32_t>::value, "Payload must expose its IDL value constructor");

void require(bool value, const char* message) { if (!value) throw std::runtime_error(message); }
Payload sample(const std::string& shape) {
    if (shape == "defaults") return Payload();
    if (shape == "positive") return Payload(7301);
    if (shape == "negative") return Payload(-91);
    throw std::runtime_error("unknown Payload state");
}
void check_api() {
    const Payload defaults;
    require(defaults.value == 0, "Payload default value must be zero");
    const Payload positive(7301), negative(-91);
    require(positive.value == 7301 && negative.value == -91, "Payload constructor must preserve int32 values");
    require(positive == Payload(7301) && positive != negative, "Payload equality mismatch");
    Payload left(11), right(29); using std::swap; swap(left, right);
    require(left.value == 29 && right.value == 11, "Payload member swap mismatch");
}
void check_dynamic_type() {
    const auto& type = rti::topic::dynamic_type<Payload>::get();
    require(type.name() == "CorpusOptionalAggregate::Payload" && type.member_count() == 1,
        "Payload DynamicType identity/member count mismatch");
    require(type.member(0).name() == "value" && type.member(0).get_id() == 0
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && !type.member(0).is_optional() && !type.member(0).is_key(),
        "Payload member name, id, int32 kind, or required metadata mismatch");
    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_STRUCT && native->_memberCount == 1,
        "Payload native TypeCode shape mismatch");
}
void check_registration() {
    dds::domain::DomainParticipant participant(72);
    const std::string name = dds::topic::topic_type_name<Payload>::value();
    require(name == "CorpusOptionalAggregate::Payload", "Payload topic name mismatch");
    dds::topic::topic_type_support<Payload>::register_type(participant, name);
    dds::topic::Topic<Payload> topic(participant, name);
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Payload> writer(publisher, topic);
    require(writer.register_instance(Payload(1)) == writer.register_instance(Payload(2)),
        "unkeyed Payload values must share one instance handle");
}
void append_frame(std::ofstream& out, const Payload& expected, dds::core::policy::DataRepresentationId representation) {
    std::vector<char> bytes;
    dds::topic::topic_type_support<Payload>::to_cdr_buffer(bytes, expected, representation);
    Payload actual;
    dds::topic::topic_type_support<Payload>::from_cdr_buffer(actual, bytes);
    require(actual == expected, "Payload CDR roundtrip mismatch");
    const std::uint32_t size = static_cast<std::uint32_t>(bytes.size());
    out.write(reinterpret_cast<const char*>(&size), sizeof(size));
    out.write(bytes.data(), static_cast<std::streamsize>(bytes.size()));
}
void write_cdr(const std::string& path, const std::string& shape) {
    const Payload expected = sample(shape);
    std::ofstream out(path.c_str(), std::ios::binary | std::ios::trunc);
    append_frame(out, expected, dds::core::policy::DataRepresentation::xcdr());
    append_frame(out, expected, dds::core::policy::DataRepresentation::xcdr2());
    require(static_cast<bool>(out), "Payload CDR capture write failed");
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
    const dds::topic::Topic<Payload> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representation(ids);
    const std::string marker=std::string(argv[4])+"/received";
    if(role=="writer") {
        const dds::pub::Publisher publisher(participant); auto qos=publisher.default_datawriter_qos(); qos<<Reliability::Reliable()<<representation<<History::KeepAll();
        dds::pub::DataWriter<Payload> writer(publisher,topic,qos);
        await([&]{return writer.publication_matched_status().current_count()==1;},"reader discovery");
        writer.write(sample("defaults")); writer.write(sample("positive")); writer.write(sample("negative"));
        await([&]{return std::ifstream(marker.c_str()).good();},"reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant); auto qos=subscriber.default_datareader_qos(); qos<<Reliability::Reliable()<<representation<<History::KeepAll();
        dds::sub::DataReader<Payload> reader(subscriber,topic,qos); bool seen[3]={false,false,false};
        await([&]{const auto values=reader.take();for(auto it=values.begin();it!=values.end();++it){if(!it->info().valid())continue;for(std::size_t i=0;i<3;++i){const std::string states[]={"defaults","positive","negative"};if(it->data()==sample(states[i]))seen[i]=true;}}return seen[0]&&seen[1]&&seen[2];},"receiving Payload values");
        std::ofstream ack(marker.c_str()); require(static_cast<bool>(ack<<"validated Payload values\n"),"writing Payload acknowledgement");
    }
}
}
int main(int argc,char** argv){try{
    if(argc>=2&&(std::string(argv[1])=="reader"||std::string(argv[1])=="writer")){run_live(argc,argv);return 0;}
    require(argc>=2,"expected --api, --dynamic-type, --registration, or --cdr");const std::string command(argv[1]);
    if(command=="--api")check_api();else if(command=="--dynamic-type")check_dynamic_type();else if(command=="--registration")check_registration();else if(command=="--cdr"&&argc==4)write_cdr(argv[2],argv[3]);else throw std::runtime_error("invalid arguments");return 0;
}catch(const std::exception& e){std::cerr<<e.what()<<'\n';return 1;}}
