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

#include <dds/core/Optional.hpp>
#include <dds/core/xtypes/StructType.hpp>
#include <dds/core/xtypes/TypeKind.hpp>
#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/sub/DataReader.hpp>
#include <dds/sub/Subscriber.hpp>
#include <dds/topic/Topic.hpp>
#include <omg/types/optional.hpp>
#include <rti/topic/cdr.hpp>

namespace {
using CorpusOptionalAggregate::Choice;
using CorpusOptionalAggregate::ChoiceAlias;
using CorpusOptionalAggregate::Holder;
using CorpusOptionalAggregate::Payload;
using CorpusOptionalAggregate::PayloadAlias;
using dds::core::optional;
using dds::core::xtypes::TypeKind;

using OptionalPayload = optional<Payload>;
using OptionalPayloadAlias = optional<PayloadAlias>;
using OptionalChoice = optional<Choice>;
using OptionalChoiceAlias = optional<ChoiceAlias>;

static_assert(dds::topic::is_topic_type<Holder>::value, "Holder must be a Modern C++ topic type");
static_assert(std::is_same<decltype(Holder::payload), OptionalPayload>::value, "payload optional API type mismatch");
static_assert(std::is_same<decltype(Holder::payloadAlias), OptionalPayloadAlias>::value, "payloadAlias optional API type mismatch");
static_assert(std::is_same<decltype(Holder::choice), OptionalChoice>::value, "choice optional API type mismatch");
static_assert(std::is_same<decltype(Holder::choiceAlias), OptionalChoiceAlias>::value, "choiceAlias optional API type mismatch");
static_assert(std::is_same<omg::types::optional<Payload>, OptionalPayload>::value, "RTI OMG optional must match Modern C++ optional");
static_assert(std::is_same<omg::types::optional<Choice>, OptionalChoice>::value, "RTI union optional must match Modern C++ optional");

void require(bool value, const char* message) { if (!value) throw std::runtime_error(message); }

Holder sample(const std::string& shape) {
    Holder value;
    if (shape == "absent") return value;
    if (shape == "payload") value.payload = Payload(17);
    else if (shape == "payload-alias") value.payloadAlias = PayloadAlias(29);
    else if (shape == "choice-payload") { Choice choice; choice.payload(Payload(41)); value.choice = choice; }
    else if (shape == "choice-number") { Choice choice; choice.number(53); value.choice = choice; }
    else if (shape == "choice-alias") { ChoiceAlias choice; choice.payload(Payload(67)); value.choiceAlias = choice; }
    else throw std::runtime_error("unknown Holder state");
    return value;
}

void check_api() {
    Holder value;
    require(!value.payload.has_value() && !value.payloadAlias.has_value()
        && !value.choice.has_value() && !value.choiceAlias.has_value(),
        "all optional aggregate members must default absent");
    value.payload = Payload(17);
    value.payloadAlias = PayloadAlias(29);
    Choice nested; nested.payload(Payload(41)); value.choice = nested;
    ChoiceAlias nested_alias; nested_alias.number(53); value.choiceAlias = nested_alias;
    require(value.payload.value().value == 17 && value.payloadAlias.value().value == 29
        && value.choice.value().payload().value == 41 && value.choiceAlias.value().number() == 53,
        "present optional aggregate/union values mismatch");
    value.choice.reset();
    require(!value.choice.has_value(), "reset must clear optional union branch presence");
    const Holder same = value;
    require(same == value && !(same != value), "Holder equality must preserve optional states");
    Holder different; different.payload = Payload(17);
    require(different != Holder{}, "Holder equality must distinguish absent and present Payload");
}

void check_dynamic_type() {
    const auto& type = rti::topic::dynamic_type<Holder>::get();
    require(type.name() == "CorpusOptionalAggregate::Holder" && type.member_count() == 4,
        "Holder DynamicType identity/member count mismatch");
    const char* names[] = {"payload", "payloadAlias", "choice", "choiceAlias"};
    const TypeKind kinds[] = {TypeKind::STRUCTURE_TYPE, TypeKind::ALIAS_TYPE,
        TypeKind::UNION_TYPE, TypeKind::ALIAS_TYPE};
    for (std::size_t i = 0; i < 4; ++i) {
        require(type.member(i).name() == names[i] && type.member(i).get_id() == i
            && type.member(i).is_optional() && !type.member(i).is_key()
            && type.member(i).type().kind() == kinds[i],
            "Holder optional member identity, id, type, or optional metadata mismatch");
    }
    require(type.member(1).type().name() == "CorpusOptionalAggregate::PayloadAlias"
        && type.member(3).type().name() == "CorpusOptionalAggregate::ChoiceAlias",
        "Holder alias fields must retain their declared alias DynamicTypes");
}

void check_registration() {
    dds::domain::DomainParticipant participant(72);
    const std::string name = dds::topic::topic_type_name<Holder>::value();
    require(name == "CorpusOptionalAggregate::Holder", "Holder topic name mismatch");
    dds::topic::topic_type_support<Holder>::register_type(participant, name);
    dds::topic::Topic<Holder> topic(participant, name);
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Holder> writer(publisher, topic);
    (void)writer;
}

void append_frame(std::ofstream& out, const Holder& expected,
    dds::core::policy::DataRepresentationId representation) {
    std::vector<char> bytes;
    dds::topic::topic_type_support<Holder>::to_cdr_buffer(bytes, expected, representation);
    Holder actual;
    dds::topic::topic_type_support<Holder>::from_cdr_buffer(actual, bytes);
    require(actual == expected, "Holder CDR roundtrip changed optional state");
    const std::uint32_t size = static_cast<std::uint32_t>(bytes.size());
    out.write(reinterpret_cast<const char*>(&size), sizeof(size));
    out.write(bytes.data(), static_cast<std::streamsize>(bytes.size()));
}
void write_cdr(const std::string& path, const std::string& shape) {
    const Holder expected = sample(shape);
    std::ofstream out(path.c_str(), std::ios::binary | std::ios::trunc);
    append_frame(out, expected, dds::core::policy::DataRepresentation::xcdr());
    append_frame(out, expected, dds::core::policy::DataRepresentation::xcdr2());
    require(static_cast<bool>(out), "Holder CDR capture write failed");
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
    const dds::topic::Topic<Holder> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representation(ids);
    const std::string marker = std::string(argv[4]) + "/received";
    const std::string states[] = {"absent", "payload", "payload-alias", "choice-payload", "choice-number", "choice-alias"};
    if (role == "writer") {
        const dds::pub::Publisher publisher(participant); auto qos = publisher.default_datawriter_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::pub::DataWriter<Holder> writer(publisher, topic, qos);
        await([&] { return writer.publication_matched_status().current_count() == 1; }, "reader discovery");
        for (const std::string& state : states) writer.write(sample(state));
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant); auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<Holder> reader(subscriber, topic, qos);
        bool seen[6] = {false,false,false,false,false,false};
        await([&] { const auto values=reader.take(); for(auto it=values.begin();it!=values.end();++it){if(!it->info().valid())continue;for(std::size_t i=0;i<6;++i)if(it->data()==sample(states[i]))seen[i]=true;} for(bool value:seen)if(!value)return false; return true; }, "receiving all optional Holder states");
        std::ofstream ack(marker.c_str()); require(static_cast<bool>(ack << "validated optional Holder states\n"), "writing Holder acknowledgement");
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
