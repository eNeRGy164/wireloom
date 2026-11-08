#include "01-primitives.hpp"
#include "../peer-qos.hpp"

#include <array>
#include <chrono>
#include <cstdint>
#include <fstream>
#include <functional>
#include <iomanip>
#include <iostream>
#include <limits>
#include <sstream>
#include <stdexcept>
#include <string>
#include <thread>
#include <type_traits>
#include <utility>
#include <vector>

#include <dds/core/xtypes/StructType.hpp>
#include <dds/core/xtypes/TypeKind.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/sub/DataReader.hpp>
#include <dds/topic/Topic.hpp>

namespace {
using CorpusPrimitives::Sample;
using TypeKind = dds::core::xtypes::TypeKind;
using Clock = std::chrono::steady_clock;
using Reader = dds::sub::DataReader<Sample>;
using Writer = dds::pub::DataWriter<Sample>;

const std::array<const char*, 5> phase_names{{"phase1", "phase2", "phase3", "phase4", "phase5"}};

void require(bool condition, const std::string& message)
{
    if (!condition) {
        throw std::runtime_error(message);
    }
}

rti::core::LongDouble finite_long_double(unsigned char pattern)
{
#if RTI_CDR_LONG_DOUBLE_PRIMITIVE_TYPE
    return static_cast<long double>(pattern);
#else
    rti::core::LongDouble value{};
    auto& native = value.native();
    for (std::size_t index = 0; index < sizeof(native.bytes); ++index) {
        native.bytes[index] = static_cast<char>(pattern);
    }
    return value;
#endif
}

void check_cpp_field_types()
{
    static_assert(std::is_same<decltype(Sample::signed16), int16_t>::value, "short maps to int16_t");
    static_assert(std::is_same<decltype(Sample::signed32), int32_t>::value, "long maps to int32_t");
    static_assert(std::is_same<decltype(Sample::signed64), int64_t>::value, "long long maps to int64_t");
    static_assert(std::is_same<decltype(Sample::unsigned16), uint16_t>::value, "unsigned short maps to uint16_t");
    static_assert(std::is_same<decltype(Sample::unsigned32), uint32_t>::value, "unsigned long maps to uint32_t");
    static_assert(std::is_same<decltype(Sample::unsigned64), uint64_t>::value, "unsigned long long maps to uint64_t");
    static_assert(std::is_same<decltype(Sample::fixedInt8), int8_t>::value, "int8 maps to int8_t");
    static_assert(std::is_same<decltype(Sample::fixedInt64), int64_t>::value, "int64 maps to int64_t");
    static_assert(std::is_same<decltype(Sample::fixedUint8), uint8_t>::value, "uint8 maps to uint8_t");
    static_assert(std::is_same<decltype(Sample::fixedUint64), uint64_t>::value, "uint64 maps to uint64_t");
    static_assert(std::is_same<decltype(Sample::octetValue), uint8_t>::value, "octet stores as uint8_t");
    static_assert(std::is_same<decltype(Sample::boolValue), bool>::value, "boolean maps to bool");
    static_assert(std::is_same<decltype(Sample::charValue), char>::value, "char maps to char");
    static_assert(std::is_same<decltype(Sample::wideCharValue), wchar_t>::value, "wchar maps to wchar_t");
    static_assert(std::is_same<decltype(Sample::floatValue), float>::value, "float maps to float");
    static_assert(std::is_same<decltype(Sample::doubleValue), double>::value, "double maps to double");
    static_assert(std::is_same<decltype(Sample::longDoubleValue), rti::core::LongDouble>::value,
        "long double maps to RTI LongDouble");
}

void write_metadata(const std::string& output_directory, const dds::core::xtypes::StructType& type)
{
    if (output_directory.empty()) {
        return;
    }

    std::ofstream report(output_directory + "/metadata.txt", std::ios::out | std::ios::trunc);
    require(static_cast<bool>(report), "Could not create the metadata report");
    report << "type-name=" << dds::topic::topic_type_name<Sample>::value() << '\n';
    report << "member-count=" << type.member_count() << '\n';
    report << "extensibility=" << type.extensibility_kind().underlying() << '\n';
    report << "final=" << dds::core::xtypes::is_final(type) << '\n';
    report << "extensible=" << dds::core::xtypes::is_extensible(type) << '\n';
    report << "mutable=" << dds::core::xtypes::is_mutable(type) << '\n';

    for (std::size_t index = 0; index < type.member_count(); ++index) {
        const auto& member = type.member(index);
        report << "member=" << index << '|' << member.name() << '|' << member.get_id() << '|'
            << member.type().kind().underlying() << '|' << member.is_key() << '|'
            << member.is_optional() << '|' << member.is_must_understand() << '|'
            << member.is_pointer() << '\n';
    }
    require(static_cast<bool>(report), "Could not finish the metadata report");
}

void check_metadata(const std::string& output_directory)
{
    using namespace dds::core::xtypes;

    const StructType& type = rti::topic::dynamic_type<Sample>::get();
    require(type.member_count() == 17, "Expected all 17 fields in the reflected type");
    require(is_extensible(type), "Expected the RTI C++ extensible type profile");
    require(!is_final(type) && !is_mutable(type), "Expected extensible, non-final, non-mutable metadata");

    const char* names[] = {
        "signed16", "signed32", "signed64", "unsigned16", "unsigned32", "unsigned64",
        "fixedInt8", "fixedInt64", "fixedUint8", "fixedUint64", "octetValue", "boolValue",
        "charValue", "wideCharValue", "floatValue", "doubleValue", "longDoubleValue"};
    const TypeKind::type kinds[] = {
        TypeKind::INT_16_TYPE, TypeKind::INT_32_TYPE, TypeKind::INT_64_TYPE,
        TypeKind::UINT_16_TYPE, TypeKind::UINT_32_TYPE, TypeKind::UINT_64_TYPE,
        TypeKind::INT_8_TYPE, TypeKind::INT_64_TYPE, TypeKind::UINT_8_TYPE, TypeKind::UINT_64_TYPE,
        TypeKind::OCTET_TYPE, TypeKind::BOOLEAN_TYPE, TypeKind::CHAR_8_TYPE, TypeKind::CHAR_16_TYPE,
        TypeKind::FLOAT_32_TYPE, TypeKind::FLOAT_64_TYPE, TypeKind::FLOAT_128_TYPE};

    for (std::size_t index = 0; index < type.member_count(); ++index) {
        const auto& member = type.member(index);
        require(member.name() == names[index], "Reflected members must retain IDL order and names");
        require(member.type().kind() == kinds[index], std::string("Unexpected reflected kind for ") + names[index]);
        require(member.get_id() == static_cast<int32_t>(index), "Expected default member IDs 0 through 16");
        require(!member.is_key(), "The primitive sample has no key annotations");
        require(!member.is_optional(), "All primitive members are required");
        require(!member.is_must_understand(), "The primitive sample has no must-understand annotations");
        require(!member.is_pointer(), "Primitive members use inline value storage");
    }

    require(dds::topic::topic_type_name<Sample>::value() == "CorpusPrimitives::Sample",
        "Unexpected registered topic type name");
    static_assert(dds::topic::is_topic_type<Sample>::value, "Sample must satisfy the RTI topic trait");
    write_metadata(output_directory, type);
}

Sample phase_value(std::size_t index);

void check_value_and_cdr(const std::string& output_directory)
{
    using Support = dds::topic::topic_type_support<Sample>;
    Sample defaults;
    require(defaults.signed16 == 0 && defaults.signed32 == 0 && defaults.signed64 == 0,
        "Signed defaults must be zero");
    require(defaults.unsigned16 == 0 && defaults.unsigned32 == 0 && defaults.unsigned64 == 0,
        "Unsigned defaults must be zero");
    require(defaults.fixedInt8 == 0 && defaults.fixedInt64 == 0 && defaults.fixedUint8 == 0
        && defaults.fixedUint64 == 0 && defaults.octetValue == 0,
        "Fixed-width and octet defaults must be zero");
    require(!defaults.boolValue && defaults.charValue == 0 && defaults.wideCharValue == 0,
        "Boolean and character defaults must be zero");
    require(defaults.floatValue == 0.0f && defaults.doubleValue == 0.0,
        "Floating-point defaults must be zero");

    const auto finite_value = finite_long_double(0x11);
    Sample value(-1234, -1234567, -1234567890123ll, 2345, 2345678u, 2345678901234ull,
        -42, -1234567890123ll, 42, 1234567890123ull, 255, true, 'Q', L'\u03A9', 1.25f, -2.5, finite_value);
    value.octetValue = 255;

    Sample copied(value);
    require(copied == value && !(copied != value), "Copy construction and equality must preserve values");
    Sample moved(std::move(copied));
    require(moved == value, "Move construction must preserve values");
    Sample move_assigned;
    move_assigned = std::move(moved);
    require(move_assigned == value, "Move assignment must preserve values");
    Sample assigned;
    assigned = value;
    require(assigned == value, "Copy assignment must preserve values");
    Sample other;
    swap(assigned, other);
    require(other == value, "Generated noexcept swap must exchange values");

    std::ostringstream formatted;
    formatted << std::scientific << std::setprecision(3);
    formatted.fill('#');
    const auto flags_before = formatted.flags();
    formatted << value;
    require(!formatted.str().empty(), "Generated stream output must produce a value representation");
    require(formatted.flags() == flags_before,
        "Generated stream output must restore the caller's formatting flags");
    require(formatted.fill() == '0',
        "RTI LongDouble stream output leaves the fill character set to zero");
    require(formatted.precision() == 15,
        "RTI stream output leaves precision at the final double formatting precision");
    if (!output_directory.empty()) {
        std::ofstream stream_report(output_directory + "/consumer-state.txt", std::ios::out | std::ios::trunc);
        require(static_cast<bool>(stream_report), "Could not create the consumer state report");
        stream_report << "stream-flags-restored=" << (formatted.flags() == flags_before) << '\n';
        stream_report << "stream-fill-after=" << formatted.fill() << '\n';
        stream_report << "stream-precision-after=" << formatted.precision() << '\n';
        stream_report << "nan-equality=true\n";
        stream_report << "same-sign-infinity-equality=true\n";
        stream_report << "signed-zero-equality=true\n";
        stream_report << "long-double-distinct-value-equality=false\n";
        require(static_cast<bool>(stream_report), "Could not finish the consumer state report");
    }

    const dds::core::policy::DataRepresentationId representations[] = {
        dds::core::policy::DataRepresentation::xcdr(),
        dds::core::policy::DataRepresentation::xcdr2()};
    const char* representation_names[] = {"xcdr1", "xcdr2"};
    for (std::size_t representation_index = 0; representation_index < 2; ++representation_index) {
        const auto representation = representations[representation_index];
        for (std::size_t phase_index = 0; phase_index < phase_names.size(); ++phase_index) {
            const auto expected = phase_value(phase_index);
            std::vector<char> bytes;
            Support::to_cdr_buffer(bytes, expected, representation);
            require(!bytes.empty(), "CDR serialization must produce bytes");
            if (!output_directory.empty()) {
                const auto file_name = output_directory + "/" + representation_names[representation_index]
                    + "-" + phase_names[phase_index] + ".cdr";
                std::ofstream file(file_name, std::ios::out | std::ios::binary | std::ios::trunc);
                require(static_cast<bool>(file), "Could not create serialized payload: " + file_name);
                file.write(bytes.data(), static_cast<std::streamsize>(bytes.size()));
                require(static_cast<bool>(file), "Could not write serialized payload: " + file_name);
            }

            Sample decoded;
            Support::from_cdr_buffer(decoded, bytes);
            require(decoded == expected, "CDR deserialization must recover the original sample");
            Support::allocate_sample(decoded, 0, 0);
            Support::reset_sample(decoded);
            Sample empty;
            require(decoded == empty, "The type support reset operation must restore the default value");
        }
    }

}

void check_floating_edge_semantics()
{
    const auto quiet_nan = (std::numeric_limits<double>::quiet_NaN)();
    const auto positive_infinity = (std::numeric_limits<double>::infinity)();
    Sample nan_left;
    nan_left.doubleValue = quiet_nan;
    Sample nan_right;
    nan_right.doubleValue = quiet_nan;
    require(nan_left == nan_right,
        "RTI equality treats equal NaN positions as equal because the epsilon comparison is unordered");

    Sample infinity_left;
    infinity_left.doubleValue = positive_infinity;
    Sample infinity_right;
    infinity_right.doubleValue = positive_infinity;
    require(infinity_left == infinity_right,
        "RTI equality treats same-sign infinities as equal because their difference is unordered");

    Sample positive_zero;
    Sample negative_zero;
    negative_zero.floatValue = -0.0f;
    negative_zero.doubleValue = -0.0;
    require(positive_zero == negative_zero, "Positive and negative zero compare equal");

    Sample first_long_double;
    first_long_double.longDoubleValue = finite_long_double(0x11);
    Sample same_long_double;
    same_long_double.longDoubleValue = finite_long_double(0x11);
    Sample different_long_double;
    different_long_double.longDoubleValue = finite_long_double(0x22);
    require(first_long_double == same_long_double, "Equal LongDouble values compare equal");
    require(first_long_double != different_long_double, "Distinct LongDouble values compare unequal");
}

Sample phase_value(std::size_t index)
{
    Sample value;

    if (index == 0 || index == 4) {
        value.signed16 = -1234;
        value.signed32 = -1234567;
        value.signed64 = -1234567890123ll;
        value.unsigned16 = 2345;
        value.unsigned32 = 2345678u;
        value.unsigned64 = 2345678901234ull;
        value.fixedInt8 = -42;
        value.fixedInt64 = -1234567890123ll;
        value.fixedUint8 = 42;
        value.fixedUint64 = 1234567890123ull;
        value.octetValue = 255;
        value.boolValue = true;
        value.charValue = 'Q';
        value.wideCharValue = L'\u03A9';
        value.floatValue = 1.25f;
        value.doubleValue = -2.5;
        value.longDoubleValue = finite_long_double(0x11);
    } else if (index == 1) {
        value.signed16 = (std::numeric_limits<int16_t>::min)();
        value.signed32 = (std::numeric_limits<int32_t>::max)();
        value.signed64 = (std::numeric_limits<int64_t>::min)();
        value.unsigned16 = (std::numeric_limits<uint16_t>::max)();
        value.unsigned32 = (std::numeric_limits<uint32_t>::max)();
        value.unsigned64 = (std::numeric_limits<uint64_t>::max)();
        value.fixedInt8 = (std::numeric_limits<int8_t>::max)();
        value.fixedInt64 = (std::numeric_limits<int64_t>::max)();
        value.fixedUint8 = (std::numeric_limits<uint8_t>::max)();
        value.fixedUint64 = (std::numeric_limits<uint64_t>::max)();
        value.octetValue = (std::numeric_limits<uint8_t>::max)();
        value.boolValue = false;
        value.charValue = static_cast<char>(0x7e);
        value.wideCharValue = L'\u03A9';
        value.floatValue = (std::numeric_limits<float>::max)();
        value.doubleValue = (std::numeric_limits<double>::lowest)();
        value.longDoubleValue = finite_long_double(0x22);
    } else if (index == 2) {
        // The default-constructed value is the zero/default phase.
    } else if (index == 3) {
        value.signed16 = 7;
        value.signed32 = 8;
        value.signed64 = 9;
        value.unsigned16 = 10;
        value.unsigned32 = 11;
        value.unsigned64 = 12;
        value.fixedInt8 = -13;
        value.fixedInt64 = -14;
        value.fixedUint8 = 15;
        value.fixedUint64 = 16;
        value.octetValue = 17;
        value.boolValue = true;
        value.charValue = 'M';
        value.wideCharValue = L'\u03BB';
        value.floatValue = -18.5f;
        value.doubleValue = 19.25;
        value.longDoubleValue = finite_long_double(0x33);
    }

    return value;
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
        writer.write(phase_value(index));
        await([&] { return std::ifstream(acknowledgement_path(directory, index)).good(); },
            "reader acknowledgement for " + std::string(phase_names[index]));
        std::cout << "Writer confirmed " << phase_names[index] << std::endl;
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
            if (samples.length() != 1 || !samples.begin()->info().valid()) {
                throw std::runtime_error("Expected one valid primitive sample in each phase");
            }
            received = samples.begin()->data();
            return true;
        }, "reading " + std::string(phase_names[index]));

        require(received == phase_value(index), "Received sample does not match " + std::string(phase_names[index]));
        std::ofstream marker(acknowledgement_path(directory, index));
        require(static_cast<bool>(marker << "validated\n"), "Could not record phase acknowledgement");
        std::cout << "Reader validated " << phase_names[index] << std::endl;
    }
}

void run_wire_peer(int argc, char** argv)
{
    if (argc != 5) {
        throw std::runtime_error("Usage: peer <reader|writer> <xcdr1|xcdr2> <topic> <scenario-directory>");
    }
    const std::string role = argv[1];
    const std::string encoding = argv[2];
    if ((role != "reader" && role != "writer") || (encoding != "xcdr1" && encoding != "xcdr2")) {
        throw std::runtime_error("Invalid role or data representation");
    }

    using namespace dds::core::policy;
    auto participant_qos = dds::domain::qos::DomainParticipantQos();
    participant_qos << rti::core::policy::TransportBuiltin::UDPv4();
    const dds::domain::DomainParticipant participant(68, participant_qos);
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
        if (argc >= 2 && std::string(argv[1]) != "contract") {
            throw std::runtime_error("Usage: peer <contract [output-directory]|reader|writer ...>");
        }

        const std::string output_directory = argc == 3 ? argv[2] : std::string();
        if (argc > 3) {
            throw std::runtime_error("Usage: peer <contract [output-directory]|reader|writer ...>");
        }
        check_cpp_field_types();
        check_metadata(output_directory);
        check_value_and_cdr(output_directory);
        check_floating_edge_semantics();
        std::cout << "C++ first-case API, metadata, value, and CDR checks passed\n";
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << '\n';
        return 1;
    }
}
