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
using Clock = std::chrono::steady_clock;

void require(bool condition, const char* message)
{
    if (!condition) {
        throw std::runtime_error(message);
    }
}

Base populated_value()
{
    return Base(41, -7301);
}

void check_api()
{
    static_assert(dds::topic::is_topic_type<Base>::value,
        "Base must be recognized as an RTI Modern C++ topic type");
    static_assert(std::is_same<decltype(Base::tenant), std::int32_t>::value,
        "Base.tenant must be int32_t");
    static_assert(std::is_same<decltype(Base::baseValue), std::int32_t>::value,
        "Base.baseValue must be int32_t");

    const Base defaults;
    require(defaults.tenant == 0 && defaults.baseValue == 0,
        "Base default values must be zero");
    const Base value(41, -7301);
    require(value.tenant == 41 && value.baseValue == -7301,
        "Base value constructor must preserve member values");
    const Base copied(value);
    require(copied == value && !(copied != value),
        "Base equality must include both members");
}

void check_dynamic_type()
{
    using dds::core::xtypes::TypeKind;
    const dds::core::xtypes::StructType& type = rti::topic::dynamic_type<Base>::get();
    require(type.name() == "CorpusInheritedKeys::Base" && type.member_count() == 2,
        "Base DynamicType identity/member count mismatch");
    require(type.member(0).name() == "tenant"
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && type.member(0).is_key(), "Base.tenant DynamicType/key metadata mismatch");
    require(type.member(1).name() == "baseValue"
        && type.member(1).type().kind() == TypeKind::INT_32_TYPE
        && !type.member(1).is_key(), "Base.baseValue DynamicType/key metadata mismatch");

    const RTIXCdrTypeCode* native =
        reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_STRUCT && native->_memberCount == 2,
        "Base native TypeCode identity/count mismatch");
    require(native->_members[0]._memberFlags == RTI_XCDR_KEY_MEMBER,
        "Base.tenant native TypeCode key flag mismatch");
    require(native->_members[1]._memberFlags == RTI_XCDR_REQUIRED_MEMBER,
        "Base.baseValue native TypeCode required flag mismatch");
}

void check_registration()
{
    dds::domain::DomainParticipant participant(68);
    const std::string type_name = dds::topic::topic_type_name<Base>::value();
    require(std::string(type_name) == "CorpusInheritedKeys::Base",
        "Base topic type name mismatch");
    dds::topic::topic_type_support<Base>::register_type(participant, type_name);
    const dds::topic::Topic<Base> topic(participant, "CorpusInheritedKeys::Base");
    dds::pub::DataWriter<Base> writer(topic);

    Base first(9, 10);
    Base same_key(9, 99);
    Base different_key(10, 10);
    require(writer.register_instance(first) == writer.register_instance(same_key),
        "Base instance identity must depend on tenant, not baseValue");
    require(writer.register_instance(first) != writer.register_instance(different_key),
        "Base instance identity must distinguish tenant values");
}

void check_cdr(const std::string& representation, const std::string& path)
{
    using dds::core::policy::DataRepresentation;
    using dds::core::policy::DataRepresentationId;
    const DataRepresentationId format = representation == "xcdr1"
        ? DataRepresentation::xcdr() : DataRepresentation::xcdr2();

    const Base states[] = {Base(), populated_value()};
    std::ofstream output(path.c_str(), std::ios::binary | std::ios::trunc);
    require(static_cast<bool>(output), "Could not create CDR capture");
    for (const Base& value : states) {
        std::vector<char> buffer;
        dds::topic::topic_type_support<Base>::to_cdr_buffer(buffer, value, format);
        const std::uint32_t size = static_cast<std::uint32_t>(buffer.size());
        output.write(reinterpret_cast<const char*>(&size), sizeof(size));
        output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));

        Base decoded;
        dds::topic::topic_type_support<Base>::from_cdr_buffer(decoded, buffer);
        require(decoded == value, "Base local CDR round trip changed a value");
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
    const dds::topic::Topic<Base> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{
        encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representation(ids);
    const std::string marker = std::string(argv[4]) + "/received";

    if (role == "writer") {
        const dds::pub::Publisher publisher(participant);
        auto qos = publisher.default_datawriter_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::pub::DataWriter<Base> writer(publisher, topic, qos);
        await([&] { return writer.publication_matched_status().current_count() == 1; }, "reader discovery");
        writer.write(populated_value());
        await([&] { return std::ifstream(marker.c_str()).good(); }, "reader acknowledgement");
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representation << History::KeepAll();
        dds::sub::DataReader<Base> reader(subscriber, topic, qos);
        Base received;
        await([&] {
            const auto samples = reader.take();
            if (samples.length() == 0) { return false; }
            require(samples.length() == 1 && samples.begin()->info().valid(),
                "Expected one valid Base live sample");
            received = samples.begin()->data();
            return true;
        }, "receiving Base sample");
        require(received == populated_value(), "Live DDS Base sample differed from expected value");
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
        std::cout << command << " passed for CorpusInheritedKeys::Base" << std::endl;
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
