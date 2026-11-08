#include "02-multiple.hpp"
#include "../peer-qos.hpp"

#include <array>
#include <chrono>
#include <cstdint>
#include <fstream>
#include <functional>
#include <iostream>
#include <limits>
#include <stdexcept>
#include <string>
#include <thread>
#include <type_traits>
#include <vector>

#include <dds/core/xtypes/StructType.hpp>
#include <dds/core/xtypes/TypeKind.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/sub/DataReader.hpp>
#include <dds/topic/Topic.hpp>

namespace
{
using WireType = ::First;
using Clock = std::chrono::steady_clock;
using Reader = dds::sub::DataReader<WireType>;
using Writer = dds::pub::DataWriter<WireType>;

const std::array<const char*, 4> states{{"default", "minimum", "maximum", "alternate"}};

void require(bool condition, const std::string& message)
{
    if (!condition) {
        throw std::runtime_error(message);
    }
}

void check_public_api()
{
    static_assert(std::is_same<decltype(WireType::value), std::int32_t>::value,
        "IDL long maps to int32_t");
    static_assert(dds::topic::is_topic_type<WireType>::value,
        "First must satisfy the RTI Modern C++ topic trait");
    static_assert(dds::topic::topic_type_support<WireType>::type_plugin_kind ==
        rti::topic::TypePluginKind::STL, "First must use the STL type plugin");
    require(dds::topic::topic_type_name<WireType>::value() == "First",
        "Unexpected topic type name");
}

void check_dynamic_type(const std::string& output_directory)
{
    using namespace dds::core::xtypes;
    const StructType& type = rti::topic::dynamic_type<WireType>::get();
    require(type.member_count() == 1, "Expected one reflected int32 member");
    require(is_extensible(type) && !is_final(type) && !is_mutable(type),
        "Expected RTI's extensible, non-final, non-mutable type profile");
    const auto& member = type.member(0);
    require(member.name() == "value", "DynamicType member name differs from IDL");
    require(member.type().kind() == TypeKind::INT_32_TYPE, "Expected INT_32_TYPE metadata");
    require(member.get_id() == 0, "Expected default member ID 0");
    require(!member.is_key() && !member.is_optional() && !member.is_must_understand()
        && !member.is_pointer(), "Required int32 uses inline value storage");

    std::ofstream metadata(output_directory + "/metadata.txt", std::ios::out | std::ios::trunc);
    require(static_cast<bool>(metadata), "Could not create the metadata report");
    metadata << "type-name=" << dds::topic::topic_type_name<WireType>::value() << '\n';
    metadata << "member-count=" << type.member_count() << '\n';
    metadata << "extensible=" << is_extensible(type) << '\n';
    metadata << "final=" << is_final(type) << '\n';
    metadata << "mutable=" << is_mutable(type) << '\n';
    metadata << "member=0|" << member.name() << '|' << member.get_id() << '|'
        << member.type().kind().underlying() << '|' << member.is_key() << '|'
        << member.is_optional() << '|' << member.is_must_understand() << '|'
        << member.is_pointer() << '\n';
    require(static_cast<bool>(metadata), "Could not finish the metadata report");
}

WireType value_for(std::size_t index)
{
    WireType value;
    if (index == 1) {
        value.value = (std::numeric_limits<std::int32_t>::min)();
    } else if (index == 2) {
        value.value = (std::numeric_limits<std::int32_t>::max)();
    } else if (index == 3) {
        value.value = -42000;
    }
    return value;
}

void check_cdr(const std::string& output_directory)
{
    using Support = dds::topic::topic_type_support<WireType>;
    const dds::core::policy::DataRepresentationId representations[] = {
        dds::core::policy::DataRepresentation::xcdr(),
        dds::core::policy::DataRepresentation::xcdr2()};
    const char* representation_names[] = {"xcdr1", "xcdr2"};

    for (std::size_t representation_index = 0; representation_index < 2; ++representation_index) {
        for (std::size_t state_index = 0; state_index < states.size(); ++state_index) {
            const WireType expected = value_for(state_index);
            std::vector<char> bytes;
            Support::to_cdr_buffer(bytes, expected, representations[representation_index]);
            require(!bytes.empty(), "CDR serialization produced no bytes");
            WireType actual;
            Support::from_cdr_buffer(actual, bytes);
            require(actual == expected, "CDR round trip did not preserve the int32 value");
            const std::string path = output_directory + "/" + representation_names[representation_index]
                + "-" + states[state_index] + ".cdr";
            std::ofstream output(path, std::ios::out | std::ios::binary | std::ios::trunc);
            require(static_cast<bool>(output), "Could not create capture " + path);
            output.write(bytes.data(), static_cast<std::streamsize>(bytes.size()));
            require(static_cast<bool>(output), "Could not write capture " + path);
        }
    }
}

void register_and_create_writer()
{
    dds::domain::DomainParticipant participant(68);
    const auto name = dds::topic::topic_type_name<WireType>::value();
    dds::topic::topic_type_support<WireType>::register_type(participant, name);
    const dds::topic::Topic<WireType> topic(participant, "WireloomCppMultipleFirstRegistration");
    const dds::pub::Publisher publisher(participant);
    const dds::pub::DataWriter<WireType> writer(publisher, topic);
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

std::string marker_path(const std::string& directory, std::size_t index)
{
    return directory + "/" + states[index];
}

void write_states(Writer& writer, const std::string& directory)
{
    await([&] { return writer.publication_matched_status().current_count() == 1; }, "writer discovery");
    for (std::size_t index = 0; index < states.size(); ++index) {
        writer.write(value_for(index));
        await([&] { return std::ifstream(marker_path(directory, index)).good(); },
            "reader acknowledgement for " + std::string(states[index]));
    }
}

void read_states(Reader& reader, const std::string& directory)
{
    for (std::size_t index = 0; index < states.size(); ++index) {
        WireType received;
        await([&] {
            const auto samples = reader.take();
            if (samples.length() == 0) {
                return false;
            }
            require(samples.length() == 1 && samples.begin()->info().valid(),
                "Expected one valid First sample");
            received = samples.begin()->data();
            return true;
        }, "reading " + std::string(states[index]));
        require(received == value_for(index), "Received First sample differs from expected value");
        std::ofstream marker(marker_path(directory, index));
        require(static_cast<bool>(marker << "validated\n"), "Could not record reader acknowledgement");
    }
}

void run_wire_peer(int argc, char** argv)
{
    if (argc != 5) {
        throw std::runtime_error("Usage: peer <reader|writer> <xcdr1|xcdr2> <topic> <scenario-directory>");
    }
    const std::string role(argv[1]);
    const std::string encoding(argv[2]);
    require((role == "reader" || role == "writer") && (encoding == "xcdr1" || encoding == "xcdr2"),
        "Invalid role or representation");

    using namespace dds::core::policy;
    auto participant_qos = dds::domain::qos::DomainParticipantQos();
    participant_qos << rti::core::policy::TransportBuiltin::UDPv4();
    dds::domain::DomainParticipant participant(68, participant_qos);
    const auto name = dds::topic::topic_type_name<WireType>::value();
    dds::topic::topic_type_support<WireType>::register_type(participant, name);
    const dds::topic::Topic<WireType> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{
        encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representations(ids);

    if (role == "writer") {
        const dds::pub::Publisher publisher(participant);
        auto qos = reliable_writer_qos(publisher);
        qos << representations << History::KeepAll();
        Writer writer(publisher, topic, qos);
        write_states(writer, argv[4]);
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representations << History::KeepAll();
        auto consistency = TypeConsistencyEnforcement::DisallowTypeCoercion();
        consistency.force_type_validation(true);
        qos << consistency;
        Reader reader(subscriber, topic, qos);
        read_states(reader, argv[4]);
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
            register_and_create_writer();
            return 0;
        }
        if (argc != 3 || std::string(argv[1]) != "contract") {
            throw std::runtime_error("Usage: peer <contract output-directory|register|reader|writer ...>");
        }

        check_public_api();
        check_dynamic_type(argv[2]);
        check_cdr(argv[2]);
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << '\n';
        return 1;
    }
}
