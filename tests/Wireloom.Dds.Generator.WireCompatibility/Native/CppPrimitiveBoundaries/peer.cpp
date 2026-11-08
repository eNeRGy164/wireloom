#include "01-boundaries.hpp"
#include "../peer-qos.hpp"

#include <array>
#include <chrono>
#include <cstdint>
#include <fstream>
#include <functional>
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
using CorpusPrimitiveBoundaries::Sample;
using Clock = std::chrono::steady_clock;
using Reader = dds::sub::DataReader<Sample>;
using Writer = dds::pub::DataWriter<Sample>;

const std::array<const char*, 4> phase_names{{"default", "minimum", "maximum", "alternate"}};

void require(bool condition, const std::string& message)
{
    if (!condition) {
        throw std::runtime_error(message);
    }
}

void check_public_api()
{
    static_assert(std::is_same<decltype(Sample::shortValue), std::int16_t>::value,
        "short maps to int16_t");
    static_assert(std::is_same<decltype(Sample::longValue), std::int32_t>::value,
        "long maps to int32_t");
    static_assert(std::is_same<decltype(Sample::longLongValue), std::int64_t>::value,
        "long long maps to int64_t");
    static_assert(std::is_same<decltype(Sample::int64Value), std::int64_t>::value,
        "int64 maps to int64_t");
    static_assert(std::is_same<decltype(Sample::floatValue), float>::value,
        "float maps to float");
    static_assert(std::is_same<decltype(Sample::doubleValue), double>::value,
        "double maps to double");
    static_assert(dds::topic::is_topic_type<Sample>::value,
        "Sample must satisfy the RTI Modern C++ topic trait");
    static_assert(dds::topic::topic_type_support<Sample>::type_plugin_kind ==
        rti::topic::TypePluginKind::STL, "Sample must use the STL type plugin");

    require(dds::topic::topic_type_name<Sample>::value() == "CorpusPrimitiveBoundaries::Sample",
        "Unexpected topic type name");
    require(CorpusPrimitiveBoundaries::shortMin == (std::numeric_limits<std::int16_t>::min)(),
        "IDL shortMin constant must equal the signed 16-bit minimum");
    require(CorpusPrimitiveBoundaries::shortMax == (std::numeric_limits<std::int16_t>::max)(),
        "IDL shortMax constant must equal the signed 16-bit maximum");
    require(CorpusPrimitiveBoundaries::longMin == (std::numeric_limits<std::int32_t>::min)(),
        "IDL longMin constant must equal the signed 32-bit minimum");
    require(CorpusPrimitiveBoundaries::longMax == (std::numeric_limits<std::int32_t>::max)(),
        "IDL longMax constant must equal the signed 32-bit maximum");
    require(CorpusPrimitiveBoundaries::longLongMin == (std::numeric_limits<std::int64_t>::min)(),
        "IDL longLongMin constant must equal the signed 64-bit minimum");
    require(CorpusPrimitiveBoundaries::longLongMax == (std::numeric_limits<std::int64_t>::max)(),
        "IDL longLongMax constant must equal the signed 64-bit maximum");
    require(CorpusPrimitiveBoundaries::int64Min == (std::numeric_limits<std::int64_t>::min)(),
        "IDL int64Min constant must equal the signed 64-bit minimum");
    require(CorpusPrimitiveBoundaries::int64Max == (std::numeric_limits<std::int64_t>::max)(),
        "IDL int64Max constant must equal the signed 64-bit maximum");
}

void check_dynamic_type(const std::string& output_directory)
{
    using namespace dds::core::xtypes;

    const StructType& type = rti::topic::dynamic_type<Sample>::get();
    require(dds::topic::topic_type_name<Sample>::value() == "CorpusPrimitiveBoundaries::Sample",
        "DynamicType uses an unexpected type name");
    require(type.member_count() == 6, "Expected six reflected primitive members");
    require(is_extensible(type) && !is_final(type) && !is_mutable(type),
        "Expected RTI's extensible, non-final, non-mutable type profile");

    const char* names[] = {
        "shortValue", "longValue", "longLongValue", "int64Value", "floatValue", "doubleValue"};
    const TypeKind::type kinds[] = {
        TypeKind::INT_16_TYPE,
        TypeKind::INT_32_TYPE,
        TypeKind::INT_64_TYPE,
        TypeKind::INT_64_TYPE,
        TypeKind::FLOAT_32_TYPE,
        TypeKind::FLOAT_64_TYPE};

    std::ofstream metadata(output_directory + "/metadata.txt", std::ios::out | std::ios::trunc);
    require(static_cast<bool>(metadata), "Could not create the metadata report");
    metadata << "type-name=" << dds::topic::topic_type_name<Sample>::value() << '\n';
    metadata << "member-count=" << type.member_count() << '\n';
    metadata << "extensible=" << is_extensible(type) << '\n';
    metadata << "final=" << is_final(type) << '\n';
    metadata << "mutable=" << is_mutable(type) << '\n';

    for (std::size_t index = 0; index < type.member_count(); ++index) {
        const auto& member = type.member(index);
        require(member.name() == names[index], "DynamicType member order/name differs from IDL");
        require(member.type().kind() == kinds[index], "DynamicType primitive kind differs from IDL");
        require(member.get_id() == static_cast<std::int32_t>(index), "Unexpected default member ID");
        require(!member.is_key() && !member.is_optional() && !member.is_must_understand()
            && !member.is_pointer(), "Primitive members must not carry non-primitive flags");
        metadata << "member=" << index << '|' << member.name() << '|' << member.get_id() << '|'
            << member.type().kind().underlying() << '|' << member.is_key() << '|'
            << member.is_optional() << '|' << member.is_must_understand() << '|'
            << member.is_pointer() << '\n';
    }
    require(static_cast<bool>(metadata), "Could not finish the metadata report");
}

Sample sample_for(std::size_t index)
{
    Sample sample;
    if (index == 1) {
        sample.shortValue = CorpusPrimitiveBoundaries::shortMin;
        sample.longValue = CorpusPrimitiveBoundaries::longMin;
        sample.longLongValue = CorpusPrimitiveBoundaries::longLongMin;
        sample.int64Value = CorpusPrimitiveBoundaries::int64Min;
        sample.floatValue = (std::numeric_limits<float>::lowest)();
        sample.doubleValue = (std::numeric_limits<double>::lowest)();
    } else if (index == 2) {
        sample.shortValue = CorpusPrimitiveBoundaries::shortMax;
        sample.longValue = CorpusPrimitiveBoundaries::longMax;
        sample.longLongValue = CorpusPrimitiveBoundaries::longLongMax;
        sample.int64Value = CorpusPrimitiveBoundaries::int64Max;
        sample.floatValue = (std::numeric_limits<float>::max)();
        sample.doubleValue = (std::numeric_limits<double>::max)();
    } else if (index == 3) {
        sample.shortValue = 123;
        sample.longValue = -456;
        sample.longLongValue = 7890123456789ll;
        sample.int64Value = -9876543210123ll;
        sample.floatValue = 1.25f;
        sample.doubleValue = -2.5;
    }

    return sample;
}

void check_cdr(const std::string& output_directory)
{
    using Support = dds::topic::topic_type_support<Sample>;
    const dds::core::policy::DataRepresentationId representations[] = {
        dds::core::policy::DataRepresentation::xcdr(),
        dds::core::policy::DataRepresentation::xcdr2()};
    const char* representation_names[] = {"xcdr1", "xcdr2"};

    for (std::size_t representation_index = 0; representation_index < 2; ++representation_index) {
        for (std::size_t phase_index = 0; phase_index < phase_names.size(); ++phase_index) {
            const Sample expected = sample_for(phase_index);
            std::vector<char> bytes;
            Support::to_cdr_buffer(bytes, expected, representations[representation_index]);
            require(!bytes.empty(), "CDR serialization produced no bytes");

            Sample actual;
            Support::from_cdr_buffer(actual, bytes);
            require(actual == expected, "CDR round trip did not preserve the sample");

            const std::string path = output_directory + "/" + representation_names[representation_index]
                + "-" + phase_names[phase_index] + ".cdr";
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
    const auto type_name = dds::topic::topic_type_name<Sample>::value();
    dds::topic::topic_type_support<Sample>::register_type(participant, type_name);
    const dds::topic::Topic<Sample> topic(participant, "WireloomCppPrimitiveBoundariesRegistration");
    const dds::pub::Publisher publisher(participant);
    const dds::pub::DataWriter<Sample> writer(publisher, topic);
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

std::string acknowledgement_path(const std::string& directory, std::size_t index)
{
    return directory + "/" + phase_names[index];
}

void write_phases(Writer& writer, const std::string& directory)
{
    await([&] { return writer.publication_matched_status().current_count() == 1; }, "writer discovery");
    for (std::size_t index = 0; index < phase_names.size(); ++index) {
        writer.write(sample_for(index));
        await([&] { return std::ifstream(acknowledgement_path(directory, index)).good(); },
            "reader acknowledgement for " + std::string(phase_names[index]));
    }
}

void read_phases(Reader& reader, const std::string& directory)
{
    for (std::size_t index = 0; index < phase_names.size(); ++index) {
        Sample received;
        await([&] {
            const auto samples = reader.take();
            if (samples.length() == 0) {
                return false;
            }
            require(samples.length() == 1 && samples.begin()->info().valid(),
                "Expected one valid primitive-boundary sample");
            received = samples.begin()->data();
            return true;
        }, "reading " + std::string(phase_names[index]));
        require(received == sample_for(index), "Received sample differs from the expected boundary state");
        std::ofstream marker(acknowledgement_path(directory, index));
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
        "Invalid role or data representation");

    using namespace dds::core::policy;
    auto participant_qos = dds::domain::qos::DomainParticipantQos();
    participant_qos << rti::core::policy::TransportBuiltin::UDPv4();
    dds::domain::DomainParticipant participant(68, participant_qos);
    const auto type_name = dds::topic::topic_type_name<Sample>::value();
    dds::topic::topic_type_support<Sample>::register_type(participant, type_name);
    const dds::topic::Topic<Sample> topic(participant, argv[3]);
    const DataRepresentationIdSeq ids{
        encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
    const DataRepresentation representations(ids);

    if (role == "writer") {
        const dds::pub::Publisher publisher(participant);
        auto qos = reliable_writer_qos(publisher);
        qos << representations << History::KeepAll();
        Writer writer(publisher, topic, qos);
        write_phases(writer, argv[4]);
    } else {
        const dds::sub::Subscriber subscriber(participant);
        auto qos = subscriber.default_datareader_qos();
        qos << Reliability::Reliable() << representations << History::KeepAll();
        auto type_consistency = TypeConsistencyEnforcement::DisallowTypeCoercion();
        type_consistency.force_type_validation(true);
        qos << type_consistency;
        Reader reader(subscriber, topic, qos);
        read_phases(reader, argv[4]);
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
