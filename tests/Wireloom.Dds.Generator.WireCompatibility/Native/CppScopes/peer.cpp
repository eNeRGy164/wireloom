#include "02-scopes.hpp"

#include <cstdint>
#include <cstdlib>
#include <fstream>
#include <string>
#include <type_traits>
#include <vector>

#include <dds/domain/DomainParticipant.hpp>
#include <dds/core/xtypes/StructType.hpp>
#include <dds/core/xtypes/TypeKind.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace
{
using CorpusScopes::KeywordRecord;
using CorpusScopes::Nested::Point;
using CorpusScopes::Reopened;
using dds::core::xtypes::TypeKind;

void check_metadata()
{
    const auto& point = rti::topic::dynamic_type<Point>::get();
    if (point.member_count() != 2 || point.member(0).name() != "x"
        || point.member(1).name() != "y") {
        throw 1;
    }

    const auto& record = rti::topic::dynamic_type<KeywordRecord>::get();
    if (record.member_count() != 4 || record.member(0).name() != "event"
        || record.member(1).name() != "relative" || record.member(2).name() != "absolute"
        || record.member(3).name() != "values"
        || record.member(1).type().kind() != TypeKind::STRUCTURE_TYPE
        || record.member(2).type().kind() != TypeKind::STRUCTURE_TYPE
        || record.member(3).type().kind() != TypeKind::SEQUENCE_TYPE) {
        throw 2;
    }

    const auto& reopened = rti::topic::dynamic_type<Reopened>::get();
    if (reopened.member_count() != 1 || reopened.member(0).name() != "value"
        || reopened.member(0).type().kind() != TypeKind::STRUCTURE_TYPE) {
        throw 3;
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
    std::vector<char> buffer;
    rti::topic::to_cdr_buffer(buffer, expected, representation);

    const Type actual = rti::topic::from_cdr_buffer<Type>(buffer);
    if (!(actual == expected)) {
        throw 4;
    }

    const std::uint32_t length = static_cast<std::uint32_t>(buffer.size());
    write_integer(output, length);
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    if (!output) {
        throw 5;
    }
}
}

int main(int argument_count, char** arguments)
{
    if (argument_count == 2 && std::string(arguments[1]) == "--register") {
        check_metadata();
        dds::domain::DomainParticipant participant(0);
        dds::topic::topic_type_support<KeywordRecord>::register_type(
            participant, "CorpusScopes::KeywordRecord");
        dds::topic::topic_type_support<Reopened>::register_type(
            participant, "CorpusScopes::Reopened");
        dds::topic::Topic<KeywordRecord> record_topic(participant, "CorpusScopes::KeywordRecordTopic");
        dds::topic::Topic<Reopened> reopened_topic(participant, "CorpusScopes::ReopenedTopic");
        dds::pub::Publisher publisher(participant);
        dds::pub::DataWriter<KeywordRecord> record_writer(publisher, record_topic);
        dds::pub::DataWriter<Reopened> reopened_writer(publisher, reopened_topic);
        (void) record_writer;
        (void) reopened_writer;
        return 0;
    }

    if (argument_count != 3) {
        return 2;
    }
    const int sequence_size = std::atoi(arguments[2]);
    if (sequence_size < 0 || sequence_size > CorpusScopes::Bound) {
        return 2;
    }

    static_assert(CorpusScopes::Bound == 4, "sequence bound constant");
    static_assert(std::is_same<decltype(KeywordRecord::relative), Point>::value, "relative nested reference");
    static_assert(std::is_same<decltype(KeywordRecord::absolute), Point>::value, "absolute nested reference");
    static_assert(std::is_same<decltype(KeywordRecord::values),
                  omg::types::bounded_sequence<std::int32_t, CorpusScopes::Bound>>::value,
        "bounded sequence mapping");
    static_assert(std::is_same<decltype(Reopened::value), KeywordRecord>::value, "reopened module reference");
    static_assert(dds::topic::is_topic_type<Point>::value, "nested type topic support");
    static_assert(dds::topic::is_topic_type<Reopened>::value, "reopened type topic support");

    check_metadata();

    KeywordRecord defaults;
    if (defaults.event != 0 || !defaults.values.empty()) {
        return 3;
    }
    dds::topic::topic_type_support<KeywordRecord>::reset_sample(defaults);
    dds::topic::topic_type_support<KeywordRecord>::allocate_sample(defaults, 1, 1);
    if (!defaults.values.empty()) {
        return 4;
    }

    const Point point(31, -47);
    const dds::core::policy::DataRepresentationId representations[] = {
        dds::core::policy::DataRepresentation::xcdr(),
        dds::core::policy::DataRepresentation::xcdr2()};
    std::ofstream output(arguments[1], std::ios::binary | std::ios::trunc);
    if (!output) {
        return 5;
    }

    for (const auto representation : representations) {
        append_cdr(output, point, representation);
        omg::types::bounded_sequence<std::int32_t, CorpusScopes::Bound> values;
        for (int index = 0; index < sequence_size; ++index) {
            values.push_back(static_cast<std::int32_t>(3 + index * 7));
        }
        const KeywordRecord record(17, point, Point(-5, 9), values);
        const Reopened reopened(record);
        append_cdr(output, record, representation);
        append_cdr(output, reopened, representation);
    }

    return 0;
}
