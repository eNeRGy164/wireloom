#include "06-valuetypes.hpp"
#include "../peer-qos.hpp"

#include <chrono>
#include <cstdint>
#include <fstream>
#include <functional>
#include <iostream>
#include <stdexcept>
#include <string>
#include <thread>
#include <type_traits>
#include <vector>

#include <dds/core/xtypes/CollectionTypes.hpp>
#include <dds/core/xtypes/StructType.hpp>
#include <dds/core/xtypes/TypeKind.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/sub/DataReader.hpp>
#include <dds/topic/Topic.hpp>
#include <omg/types/string_view.hpp>

namespace
{
using Base = CorpusValueTypes::BaseValue;
using Value = CorpusValueTypes::DerivedValue;
using Clock = std::chrono::steady_clock;
using Reader = dds::sub::DataReader<Value>;
using Writer = dds::pub::DataWriter<Value>;

void require(bool condition, const std::string& message)
{
    if (!condition) {
        throw std::runtime_error(message);
    }
}

std::int32_t get_base_value(const Value& value)
{
#ifdef RTI_ORACLE
    return value.baseValue();
#else
    return value.baseValue;
#endif
}

void set_base_value(Value& value, std::int32_t field)
{
#ifdef RTI_ORACLE
    value.baseValue(field);
#else
    value.baseValue = field;
#endif
}

std::string get_name(const Value& value)
{
#ifdef RTI_ORACLE
    return value.name();
#else
    return value.name;
#endif
}

void set_name(Value& value, const std::string& field)
{
#ifdef RTI_ORACLE
    value.name(field);
#else
    value.name = field;
#endif
}

Value state_value(const std::string& state)
{
    Value value;
    if (state == "alternate") {
        set_base_value(value, -17531);
        set_name(value, "derived-value");
    } else if (state != "default") {
        throw std::runtime_error("Unknown sample state: " + state);
    }
    return value;
}

void check_api(const std::string& output_directory)
{
    static_assert(std::is_base_of<Base, Value>::value, "DerivedValue must inherit BaseValue");
    static_assert(dds::topic::is_topic_type<Value>::value,
        "RTI's retained DerivedValue is a Modern C++ topic type");
#ifdef RTI_ORACLE
    static_assert(std::is_same<decltype(std::declval<Value&>().baseValue()), std::int32_t&>::value,
        "RTI exposes inherited baseValue as an accessor");
    static_assert(std::is_same<decltype(std::declval<Value&>().name()), std::string&>::value,
        "RTI exposes name as an accessor");
    static_assert(std::is_constructible<Value, std::int32_t, omg::types::string_view>::value,
        "RTI constructor accepts base fields and string_view");
#else
    static_assert(std::is_same<decltype(Value::baseValue), std::int32_t>::value,
        "Wireloom exposes inherited baseValue as a public field");
    static_assert(std::is_same<decltype(Value::name), std::string>::value,
        "Wireloom exposes name as a public field");
    static_assert(std::is_constructible<Value, const Base&, const std::string&>::value,
        "Wireloom constructor accepts a base value and std::string");
#endif
    require(dds::topic::topic_type_name<Value>::value() == "CorpusValueTypes::DerivedValue",
        "Unexpected DerivedValue topic type name");
    Value defaults;
    require(get_base_value(defaults) == 0 && get_name(defaults).empty(),
        "Base and string defaults must match the IDL");
    const Value alternate = state_value("alternate");
    require(get_base_value(alternate) == -17531 && get_name(alternate) == "derived-value",
        "Consumer adapters must preserve both inherited and local values");

    std::ofstream api(output_directory + "/api.txt", std::ios::out | std::ios::trunc);
    require(static_cast<bool>(api), "Could not create public API evidence");
#ifdef RTI_ORACLE
    api << "baseValue=inherited-accessor\nname=accessor\nconstructor=(int32,string_view)\n";
#else
    api << "baseValue=inherited-public-field\nname=public-field\nconstructor=(BaseValue,string)\n";
#endif
    api << "base=CorpusValueTypes::BaseValue\ntopic-type=true\n";
    require(static_cast<bool>(api), "Could not finish public API evidence");
}

void check_dynamic_type(const std::string& output_directory)
{
    using namespace dds::core::xtypes;
    const StructType& type = rti::topic::dynamic_type<Value>::get();
    require(type.name() == "CorpusValueTypes::DerivedValue", "Unexpected DynamicType name");
    require(type.has_parent(), "DerivedValue DynamicType must retain its base type");
    require(type.parent().name() == "CorpusValueTypes::BaseValue", "Unexpected DynamicType base type");
    require(type.member_count() == 1, "Expected one local DynamicType member");
    require(is_extensible(type) && !is_final(type) && !is_mutable(type),
        "Expected RTI's extensible, non-final, non-mutable struct profile");
    const auto& member = type.member(0);
    require(member.name() == "name", "Unexpected local DynamicType member name");
    require(member.type().kind() == TypeKind::STRING_TYPE, "Expected string member metadata");
    const auto& string_type = static_cast<const StringType&>(member.type());
    require(string_type.bounds() == 16, "DynamicType must preserve name's string<16> bound");
    require(member.get_id() == 1, "DerivedValue local member must use ID 1 after its base member");

    const StructType& base_type = rti::topic::dynamic_type<Base>::get();
    require(base_type.name() == "CorpusValueTypes::BaseValue" && base_type.member_count() == 1,
        "BaseValue DynamicType must contain its own member");
    require(base_type.member(0).name() == "baseValue"
        && base_type.member(0).type().kind() == TypeKind::INT_32_TYPE,
        "BaseValue DynamicType member must be int32 baseValue");

    std::ofstream metadata(output_directory + "/metadata.txt", std::ios::out | std::ios::trunc);
    require(static_cast<bool>(metadata), "Could not create DynamicType evidence");
    metadata << "type-name=" << type.name() << '\n';
    metadata << "base-type=" << type.parent().name() << '\n';
    metadata << "member-count=" << type.member_count() << '\n';
    metadata << "extensible=" << is_extensible(type) << '\n';
    metadata << "final=" << is_final(type) << '\n';
    metadata << "mutable=" << is_mutable(type) << '\n';
    metadata << "member=0|" << member.name() << '|' << member.get_id() << '|'
        << member.type().kind().underlying() << '|' << string_type.bounds() << '\n';
    metadata << "base-member=0|" << base_type.member(0).name() << '|'
        << base_type.member(0).type().kind().underlying() << '\n';
}

void check_cdr(const std::string& output_directory)
{
    using Support = dds::topic::topic_type_support<Value>;
    const dds::core::policy::DataRepresentationId representations[] = {
        dds::core::policy::DataRepresentation::xcdr(),
        dds::core::policy::DataRepresentation::xcdr2()};
    const char* names[] = {"xcdr1", "xcdr2"};
    const char* states[] = {"default", "alternate"};
    for (std::size_t representation = 0; representation < 2; ++representation) {
        for (const char* state : states) {
            const Value expected = state_value(state);
            std::vector<char> bytes;
            Support::to_cdr_buffer(bytes, expected, representations[representation]);
            require(!bytes.empty(), "CDR serialization returned an empty buffer");
            Value actual;
            Support::from_cdr_buffer(actual, bytes);
            require(actual == expected, "CDR round trip changed DerivedValue");
            const std::string path = output_directory + "/" + names[representation] + "-" + state + ".cdr";
            std::ofstream capture(path, std::ios::out | std::ios::binary | std::ios::trunc);
            require(static_cast<bool>(capture), "Could not create CDR capture");
            capture.write(bytes.data(), static_cast<std::streamsize>(bytes.size()));
            require(static_cast<bool>(capture), "Could not write CDR capture");
        }
    }
}

void register_writer()
{
    dds::domain::DomainParticipant participant(68);
    const auto type_name = dds::topic::topic_type_name<Value>::value();
    dds::topic::topic_type_support<Value>::register_type(participant, type_name);
    const dds::topic::Topic<Value> topic(participant, "WireloomCppValueTypesDerivedRegistration");
    const dds::pub::Publisher publisher(participant);
    const dds::pub::DataWriter<Value> writer(publisher, topic);
    (void) writer;
}

void await(const std::function<bool()>& condition, const std::string& context)
{
    const auto deadline = Clock::now() + std::chrono::seconds(15);
    while (!condition()) {
        if (Clock::now() >= deadline) {
            throw std::runtime_error("Timed out: " + context);
        }
        std::this_thread::sleep_for(std::chrono::milliseconds(25));
    }
}

void run_wire_peer(int argc, char** argv)
{
    require(argc == 5, "Usage: peer <reader|writer> <xcdr1|xcdr2> <topic> <scenario-directory>");
    const std::string role(argv[1]);
    const std::string encoding(argv[2]);
    require((role == "reader" || role == "writer")
        && (encoding == "xcdr1" || encoding == "xcdr2"), "Invalid role or representation");
    using namespace dds::core::policy;
    auto participant_qos = dds::domain::qos::DomainParticipantQos();
    participant_qos << rti::core::policy::TransportBuiltin::UDPv4();
    dds::domain::DomainParticipant participant(68, participant_qos);
    const auto type_name = dds::topic::topic_type_name<Value>::value();
    dds::topic::topic_type_support<Value>::register_type(participant, type_name);
    const dds::topic::Topic<Value> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{
        encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representations(ids);

    if (role == "writer") {
        const dds::pub::Publisher publisher(participant);
        auto qos = reliable_writer_qos(publisher);
        qos << representations << History::KeepAll();
        Writer writer(publisher, topic, qos);
        await([&] { return writer.publication_matched_status().current_count() == 1; }, "writer discovery");
        for (const char* state : {"default", "alternate"}) {
            writer.write(state_value(state));
            await([&] { return std::ifstream(std::string(argv[4]) + "/" + state).good(); },
                std::string("reader acknowledgement for ") + state);
        }
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representations << History::KeepAll();
        auto consistency = TypeConsistencyEnforcement::DisallowTypeCoercion();
        consistency.force_type_validation(true);
        qos << consistency;
        Reader reader(subscriber, topic, qos);
        for (const char* state : {"default", "alternate"}) {
            Value received;
            await([&] {
                const auto samples = reader.take();
                if (samples.length() == 0) {
                    return false;
                }
                require(samples.length() == 1 && samples.begin()->info().valid(),
                    "Expected one valid DerivedValue sample");
                received = samples.begin()->data();
                return true;
            }, std::string("reading ") + state);
            require(received == state_value(state), std::string("Received incorrect ") + state + " sample");
            std::ofstream marker(std::string(argv[4]) + "/" + state);
            require(static_cast<bool>(marker << "validated\n"), "Could not record reader acknowledgement");
        }
    }
}
}

int main(int argc, char** argv)
{
    try {
        if (argc >= 2 && (std::string(argv[1]) == "reader" || std::string(argv[1]) == "writer")) {
            run_wire_peer(argc, argv);
            return 0;
        }
        if (argc == 2 && std::string(argv[1]) == "register") {
            register_writer();
            return 0;
        }
        require(argc == 3 && std::string(argv[1]) == "contract",
            "Usage: peer <contract output-directory|register|reader|writer ...>");
        check_api(argv[2]);
        check_dynamic_type(argv[2]);
        check_cdr(argv[2]);
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << '\n';
        return 1;
    }
}
