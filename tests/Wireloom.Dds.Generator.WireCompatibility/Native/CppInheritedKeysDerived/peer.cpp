#include "08-key-inherited.hpp"

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

#include <dds/core/xtypes/StructType.hpp>
#include <dds/dds.hpp>
#include <rti/topic/cdr.hpp>

namespace {
using CorpusInheritedKeys::Base;
using CorpusInheritedKeys::Derived;
using Clock = std::chrono::steady_clock;

void require(bool condition, const char* message)
{
    if (!condition) { throw std::runtime_error(message); }
}

Derived populated_value()
{
#ifdef RTI_ORACLE
    return Derived(41, -7301, 8, 9001);
#else
    return Derived(Base(41, -7301), 8, 9001);
#endif
}

void check_api()
{
    static_assert(dds::topic::is_topic_type<Derived>::value,
        "Derived must be recognized as an RTI Modern C++ topic type");
    static_assert(std::is_base_of<Base, Derived>::value,
        "Derived must inherit the public Base C++ type");
    static_assert(std::is_same<decltype(Derived::localId), std::int32_t>::value,
        "Derived.localId must be int32_t");
    static_assert(std::is_same<decltype(Derived::payload), std::int32_t>::value,
        "Derived.payload must be int32_t");
#ifdef RTI_ORACLE
    static_assert(std::is_constructible<Derived, std::int32_t, std::int32_t,
        std::int32_t, std::int32_t>::value,
        "RTI oracle exposes a flattened four-value Derived constructor");
    static_assert(!std::is_constructible<Derived, const Base&,
        std::int32_t, std::int32_t>::value,
        "RTI oracle does not expose Wireloom's Base-subobject constructor");
#else
    static_assert(std::is_constructible<Derived, const Base&,
        std::int32_t, std::int32_t>::value,
        "Wireloom exposes a Base-subobject Derived constructor");
    static_assert(!std::is_constructible<Derived, std::int32_t,
        std::int32_t, std::int32_t, std::int32_t>::value,
        "Wireloom does not expose RTI's flattened four-value constructor");
#endif

    const Derived defaults;
    require(defaults.tenant == 0 && defaults.baseValue == 0
        && defaults.localId == 0 && defaults.payload == 0,
        "Derived inherited and local defaults must be zero");
    const Derived value = populated_value();
    require(value.tenant == 41 && value.baseValue == -7301
        && value.localId == 8 && value.payload == 9001,
        "Derived constructor must preserve inherited and local values");
    const Derived copied(value);
    require(copied == value && !(copied != value),
        "Derived equality must include inherited and local members");
}

void check_dynamic_type()
{
    using dds::core::xtypes::TypeKind;
    const dds::core::xtypes::StructType& type = rti::topic::dynamic_type<Derived>::get();
    require(type.name() == "CorpusInheritedKeys::Derived" && type.member_count() == 2,
        "Derived DynamicType local member identity/count mismatch");
    require(type.member(0).name() == "localId"
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && type.member(0).is_key(), "Derived.localId DynamicType/key metadata mismatch");
    require(type.member(1).name() == "payload"
        && type.member(1).type().kind() == TypeKind::INT_32_TYPE
        && !type.member(1).is_key(), "Derived.payload DynamicType/key metadata mismatch");

    const RTIXCdrTypeCode* native =
        reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_VALUE && native->_memberCount == 2,
        "Derived native TypeCode kind/count mismatch");
    const DDS_TypeCode* native_dds =
        reinterpret_cast<const DDS_TypeCode*>(&type.native());
    const DDS_TypeCode* base_native_dds = reinterpret_cast<const DDS_TypeCode*>(
        &rti::topic::dynamic_type<Base>::get().native());
    require(reinterpret_cast<const void*>(native_dds->_data._typeCode)
            == reinterpret_cast<const void*>(base_native_dds),
        "Derived native TypeCode must link the Base DynamicType");
    require(native->_members[0]._memberFlags == RTI_XCDR_KEY_MEMBER,
        "Derived.localId native TypeCode key flag mismatch");
    require(native->_members[1]._memberFlags == RTI_XCDR_REQUIRED_MEMBER,
        "Derived.payload native TypeCode required flag mismatch");
}

void check_registration()
{
    dds::domain::DomainParticipant participant(68);
    const std::string type_name = dds::topic::topic_type_name<Derived>::value();
    require(type_name == "CorpusInheritedKeys::Derived", "Derived topic type name mismatch");
    dds::topic::topic_type_support<Derived>::register_type(participant, type_name);
    const dds::topic::Topic<Derived> topic(participant, "CorpusInheritedKeys::Derived");
    dds::pub::DataWriter<Derived> writer(topic);

    Derived first = populated_value();
    Derived same_key = first;
    same_key.baseValue = 555;
    same_key.payload = 777;
    Derived different_tenant = first;
    different_tenant.tenant++;
    Derived different_local_id = first;
    different_local_id.localId++;
    require(writer.register_instance(first) == writer.register_instance(same_key),
        "Derived instance identity must ignore non-key inherited/local fields");
    require(writer.register_instance(first) != writer.register_instance(different_tenant),
        "Derived instance identity must include inherited tenant key");
    require(writer.register_instance(first) != writer.register_instance(different_local_id),
        "Derived instance identity must include localId key");
}

void check_cdr(const std::string& representation, const std::string& path)
{
    using dds::core::policy::DataRepresentation;
    using dds::core::policy::DataRepresentationId;
    const DataRepresentationId format = representation == "xcdr1"
        ? DataRepresentation::xcdr() : DataRepresentation::xcdr2();
    const Derived states[] = {Derived(), populated_value()};
    std::ofstream output(path.c_str(), std::ios::binary | std::ios::trunc);
    require(static_cast<bool>(output), "Could not create CDR capture");
    for (const Derived& value : states) {
        std::vector<char> buffer;
        dds::topic::topic_type_support<Derived>::to_cdr_buffer(buffer, value, format);
        const std::uint32_t size = static_cast<std::uint32_t>(buffer.size());
        output.write(reinterpret_cast<const char*>(&size), sizeof(size));
        output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
        Derived decoded;
        dds::topic::topic_type_support<Derived>::from_cdr_buffer(decoded, buffer);
        require(decoded == value, "Derived local CDR round trip changed a value");
    }
    require(static_cast<bool>(output), "Could not finish CDR capture");
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
    const dds::topic::Topic<Derived> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{
        encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representation(ids);
    const std::string marker = std::string(argv[4]) + "/received";
    if (role == "writer") {
        const dds::pub::Publisher publisher(participant);
        auto qos = publisher.default_datawriter_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::pub::DataWriter<Derived> writer(publisher, topic, qos);
        await([&] { return writer.publication_matched_status().current_count() == 1; }, "reader discovery");
        writer.write(populated_value());
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<Derived> reader(subscriber, topic, qos);
        Derived received;
        await([&] {
            const auto samples = reader.take();
            if (samples.length() == 0) { return false; }
            std::size_t valid_sample_count = 0;
            for (auto sample = samples.begin(); sample != samples.end(); ++sample) {
                if (!sample->info().valid()) { continue; }
                received = sample->data();
                ++valid_sample_count;
            }
            require(valid_sample_count <= 1,
                "Received more than one valid Derived live sample");
            return valid_sample_count == 1;
        }, "receiving Derived sample");
        require(received == populated_value(), "Live DDS Derived sample differed from expected value");
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
        std::cout << command << " passed for CorpusInheritedKeys::Derived" << std::endl;
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
