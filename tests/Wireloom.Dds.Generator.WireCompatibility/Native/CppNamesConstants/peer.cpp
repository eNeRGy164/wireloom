#include "02-names-constants.hpp"

#include <cstdint>
#include <fstream>
#include <type_traits>
#include <vector>

#include <dds/core/xtypes/StructType.hpp>
#include <dds/core/xtypes/TypeKind.hpp>

namespace
{
using CorpusNames::KeywordRecord;
using CorpusNames::Nested::Point;
using CorpusNames::State;

void check_type_metadata()
{
    using dds::core::xtypes::TypeKind;
    const auto& state_type = rti::topic::dynamic_type<State>::get();
    if (state_type.member_count() != 3
        || state_type.member(0).name() != "Idle"
        || state_type.member(1).name() != "Running"
        || state_type.member(2).name() != "Complete") {
        throw 3;
    }

    const auto& point_type = rti::topic::dynamic_type<Point>::get();
    if (point_type.member_count() != 2
        || point_type.member(0).name() != "x"
        || point_type.member(1).name() != "y"
        || point_type.member(0).type().kind() != TypeKind::INT_32_TYPE
        || point_type.member(1).type().kind() != TypeKind::INT_32_TYPE) {
        throw 4;
    }

    const auto& record_type = rti::topic::dynamic_type<KeywordRecord>::get();
    if (record_type.member_count() != 3
        || record_type.member(0).name() != "event"
        || record_type.member(1).name() != "state"
        || record_type.member(2).name() != "point"
        || record_type.member(1).type().kind() != TypeKind::ENUMERATION_TYPE
        || record_type.member(2).type().kind() != TypeKind::STRUCTURE_TYPE) {
        throw 5;
    }
}

template<typename Integer>
void write_integer(std::ofstream& output, Integer value)
{
    output.write(reinterpret_cast<const char*>(&value), sizeof(value));
}

template<typename Type>
void append_cdr(
    std::ofstream& output,
    const Type& expected,
    dds::core::policy::DataRepresentationId representation)
{
    using Support = dds::topic::topic_type_support<Type>;
    std::vector<char> buffer;
    Support::to_cdr_buffer(buffer, expected, representation);

    Type actual;
    Support::from_cdr_buffer(actual, buffer);
    if (!(actual == expected)) {
        throw 1;
    }

    const std::uint32_t length = static_cast<std::uint32_t>(buffer.size());
    write_integer(output, length);
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    if (!output) {
        throw 2;
    }
}
}

int main(int argument_count, char** arguments)
{
    if (argument_count != 2) {
        return 2;
    }

    static_assert(CorpusNames::Base == 4, "Base constant");
    static_assert(CorpusNames::Derived == 17, "Derived expression constant");
    static_assert(std::is_same<decltype(KeywordRecord::event), std::int32_t>::value, "long field mapping");
    static_assert(std::is_same<decltype(KeywordRecord::state), State>::value, "enum member mapping");
    static_assert(std::is_same<decltype(KeywordRecord::point), Point>::value, "nested struct mapping");
    static_assert(dds::topic::is_topic_type<Point>::value, "Point retains RTI topic support");
    static_assert(dds::topic::is_topic_type<KeywordRecord>::value, "KeywordRecord topic support");

    KeywordRecord defaults;
    if (defaults.event != 0 || defaults.state != State::Idle
        || defaults.point.x != 0 || defaults.point.y != 0) {
        return 3;
    }

    check_type_metadata();
    dds::topic::topic_type_support<KeywordRecord>::reset_sample(defaults);
    if (defaults.state != State::Idle || defaults.point.x != 0 || defaults.point.y != 0) {
        return 5;
    }
    dds::topic::topic_type_support<KeywordRecord>::allocate_sample(defaults, 1, 1);
    if (defaults.state != State::Idle || defaults.point.x != 0 || defaults.point.y != 0) {
        return 6;
    }

    const Point point(31, -47);
    const KeywordRecord record(17, State::Running, point);
    const dds::core::policy::DataRepresentationId representations[] = {
        dds::core::policy::DataRepresentation::xcdr(),
        dds::core::policy::DataRepresentation::xcdr2()};
    std::ofstream output(arguments[1], std::ios::binary | std::ios::trunc);
    if (!output) {
        return 4;
    }

    for (const auto representation : representations) {
        append_cdr(output, point, representation);
        append_cdr(output, record, representation);
    }

    return 0;
}
