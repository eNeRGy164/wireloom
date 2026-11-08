#include "07-union-octet.hpp"

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
using CorpusUnionOctet::Choice;
using dds::core::xtypes::TypeKind;
static_assert(dds::topic::is_topic_type<Choice>::value, "Choice must be a Modern C++ topic type");
static_assert(std::is_same<decltype(std::declval<Choice&>()._d()), std::uint8_t&>::value,
    "_d() must return the octet discriminator by reference");
static_assert(std::is_same<decltype(Choice::default_discriminator()), std::uint8_t>::value,
    "default_discriminator must preserve the octet type");
static_assert(std::is_same<decltype(std::declval<Choice&>().minimum()), std::int32_t&>::value,
    "minimum() must expose a long branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().maximum()), std::int32_t&>::value,
    "maximum() must expose a long branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().fallback()), std::int32_t&>::value,
    "fallback() must expose a long branch reference");
void require(bool ok, const char* message) { if (!ok) throw std::runtime_error(message); }
template<class F> void require_inactive(F f) { bool threw=false; try { f(); } catch (const dds::core::PreconditionNotMetError&) { threw=true; } require(threw,"inactive branch access must throw PreconditionNotMetError"); }
Choice sample(const std::string& state) {
    Choice value;
    if (state == "defaults") return value;
    if (state == "minimum") { value.minimum(-7301); return value; }
    if (state == "maximum") { value.maximum(7301); return value; }
    if (state == "fallback") { value.fallback(29, 1); return value; }
    throw std::runtime_error("unknown octet union state");
}
void check_api() {
    require(dds::topic::topic_type_name<Choice>::value() == "CorpusUnionOctet::Choice",
        "Choice topic name mismatch");
    Choice value;
    require(Choice::default_discriminator() == 0 && value._d() == 0 && value.minimum() == 0,
        "default discriminator must select first octet label minimum");
    require_inactive([&] { value.maximum(); }); require_inactive([&] { value.fallback(); });
    value.maximum(7301); require(value._d() == 255 && value.maximum() == 7301,"maximum label 255 selection failed");
    require_inactive([&] { value.minimum(); });
    value.fallback(-29, 1); require(value._d() == 1 && value.fallback() == -29,"fallback label 1 selection failed");
    value._d(7); value.fallback(31); require(value._d() == 7 && value.fallback() == 31,"one-argument fallback must preserve unmatched discriminator");
    require_inactive([&] { value.minimum(); });
    value.minimum(-7301); require(value._d() == 0 && value.minimum() == -7301,"minimum label 0 selection failed");
    require_inactive([&] { value.fallback(1, 255); }); require_inactive([&] { value.fallback(); });
    Choice left=sample("maximum"), right=sample("fallback"); using std::swap; swap(left,right);
    require(left == sample("fallback") && right == sample("maximum"),"union swap must exchange active discriminator and payload");
}
void check_dynamic_type() {
    const auto& type=rti::topic::dynamic_type<Choice>::get();
    require(type.name()=="CorpusUnionOctet::Choice","union DynamicType name mismatch");
    require(type.member_count()==3,"union DynamicType member count mismatch");
    require(type.discriminator().kind()==TypeKind::UINT_8_TYPE,"union DynamicType discriminator kind is not uint8");
    require(type.member(0).name()=="minimum" && type.member(0).type().kind()==TypeKind::INT_32_TYPE
        && type.member(0).labels().size()==1 && type.member(0).labels()[0]==0,"minimum branch metadata mismatch");
    require(type.member(1).name()=="maximum" && type.member(1).type().kind()==TypeKind::INT_32_TYPE
        && type.member(1).labels().size()==1 && type.member(1).labels()[0]==255,"maximum branch metadata mismatch");
    require(type.member(2).name()=="fallback" && type.member(2).type().kind()==TypeKind::INT_32_TYPE
        && type.member(2).labels().size()==1 && type.member(2).labels()[0]==dds::core::xtypes::UnionMember::DEFAULT_LABEL,
        "default branch metadata mismatch");
    const auto* native=reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind==RTI_XCDR_TK_UNION && native->_memberCount==3 && native->_typeCode
        && native->_typeCode->_kind==RTI_XCDR_TK_OCTET,"native union TypeCode discriminator mismatch");
    require(native->_members[0]._representation._typeCode && native->_members[0]._representation._typeCode->_kind==RTI_XCDR_TK_LONG
        && native->_members[1]._representation._typeCode && native->_members[1]._representation._typeCode->_kind==RTI_XCDR_TK_LONG
        && native->_members[2]._representation._typeCode && native->_members[2]._representation._typeCode->_kind==RTI_XCDR_TK_LONG,
        "native branch TypeCodes must all be long");
}
void check_registration() {
    dds::domain::DomainParticipant participant(72); const auto name=dds::topic::topic_type_name<Choice>::value();
    dds::topic::topic_type_support<Choice>::register_type(participant,name);
    dds::topic::Topic<Choice> topic(participant,name); dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Choice> writer(publisher,topic);
    require(writer.register_instance(sample("minimum"))==writer.register_instance(sample("maximum")),"keyless union values must share an instance handle");
}
void append_frame(std::ofstream& out,const Choice& value,dds::core::policy::DataRepresentationId representation) {
    std::vector<char> bytes; dds::topic::topic_type_support<Choice>::to_cdr_buffer(bytes,value,representation);
    Choice actual; dds::topic::topic_type_support<Choice>::from_cdr_buffer(actual,bytes); require(actual==value,"CDR roundtrip mismatch");
    const std::uint32_t n=static_cast<std::uint32_t>(bytes.size()); out.write(reinterpret_cast<const char*>(&n),sizeof(n)); out.write(bytes.data(),static_cast<std::streamsize>(bytes.size()));
}
void write_cdr(const std::string& path,const std::string& state) { std::ofstream out(path.c_str(),std::ios::binary|std::ios::trunc); const auto value=sample(state); append_frame(out,value,dds::core::policy::DataRepresentation::xcdr()); append_frame(out,value,dds::core::policy::DataRepresentation::xcdr2()); require(static_cast<bool>(out),"CDR capture write failed"); }
void await(const std::function<bool()>& predicate,const char* message) { const auto deadline=std::chrono::steady_clock::now()+std::chrono::seconds(15); while(!predicate()){if(std::chrono::steady_clock::now()>=deadline)throw std::runtime_error(message);std::this_thread::sleep_for(std::chrono::milliseconds(20));} }
void run_live(int argc,char** argv) {
    require(argc==5,"usage: peer <reader|writer> <xcdr1|xcdr2> <topic> <ack-dir>"); const std::string role(argv[1]),encoding(argv[2]);
    const dds::domain::DomainParticipant participant(72); const dds::topic::Topic<Choice> topic(participant,argv[3]);
    using namespace dds::core::policy; const DataRepresentationIdSeq ids{encoding=="xcdr1"?DataRepresentation::xcdr():DataRepresentation::xcdr2()}; const DataRepresentation representation(ids); const std::string ack=std::string(argv[4])+"/received";
    if(role=="writer"){const dds::pub::Publisher publisher(participant);auto qos=publisher.default_datawriter_qos();qos<<Reliability::Reliable()<<representation<<History::KeepAll();dds::pub::DataWriter<Choice> writer(publisher,topic,qos);await([&]{return writer.publication_matched_status().current_count()==1;},"reader discovery");for(const auto& s:std::vector<std::string>{"defaults","minimum","maximum","fallback"})writer.write(sample(s));await([&]{return std::ifstream(ack.c_str()).good();},"reader acknowledgement");}
    else{const dds::sub::Subscriber subscriber(participant);auto qos=subscriber.default_datareader_qos();qos<<Reliability::Reliable()<<representation<<History::KeepAll();dds::sub::DataReader<Choice> reader(subscriber,topic,qos);bool seen[4]={false,false,false,false};const std::string states[]={"defaults","minimum","maximum","fallback"};await([&]{const auto samples=reader.take();for(auto it=samples.begin();it!=samples.end();++it){if(!it->info().valid())continue;bool known=false;for(std::size_t i=0;i<4;++i)if(it->data()==sample(states[i])){seen[i]=true;known=true;}require(known,"unexpected union state");}return seen[0]&&seen[1]&&seen[2]&&seen[3];},"receiving all union states");std::ofstream out(ack.c_str());require(static_cast<bool>(out<<"validated octet union states\n"),"writing reader acknowledgement");}
}
}
int main(int argc,char** argv){try{if(argc>=2&&(std::string(argv[1])=="reader"||std::string(argv[1])=="writer")){run_live(argc,argv);return 0;}require(argc>=2,"expected --api, --dynamic-type, --registration, or --cdr");const std::string command(argv[1]);if(command=="--api")check_api();else if(command=="--dynamic-type")check_dynamic_type();else if(command=="--registration")check_registration();else if(command=="--cdr"&&argc==4)write_cdr(argv[2],argv[3]);else throw std::runtime_error("invalid arguments");return 0;}catch(const std::exception& e){std::cerr<<e.what()<<'\n';return 1;}}
