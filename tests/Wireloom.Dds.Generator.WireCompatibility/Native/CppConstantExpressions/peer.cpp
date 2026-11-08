#include "02-constant-expressions.hpp"

#include <cstdint>
#include <cstdlib>
#include <fstream>
#include <sstream>
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
using CorpusConstantExpressions::Sample;
using dds::core::xtypes::TypeKind;

void check_metadata_and_lifecycle()
{
    const auto& type = rti::topic::dynamic_type<Sample>::get();
    if (type.member_count() != 2
        || type.member(0).name() != "values"
        || type.member(1).name() != "array"
        || type.member(0).type().kind() != TypeKind::SEQUENCE_TYPE
        || type.member(1).type().kind() != TypeKind::ARRAY_TYPE) {
        throw 1;
    }

    Sample sample;
    if (!sample.values.empty()) {
        throw 2;
    }
    for (const auto value : sample.array) {
        if (value != 0) {
            throw 3;
        }
    }

    dds::topic::topic_type_support<Sample>::reset_sample(sample);
    dds::topic::topic_type_support<Sample>::allocate_sample(sample, 0, 0);
    if (!sample.values.empty()) {
        throw 4;
    }
    for (const auto value : sample.array) {
        if (value != 0) {
            throw 5;
        }
    }
}

void check_array_value_semantics()
{
    Sample left;
    Sample right;
    left.array[0] = 10;
    left.array[4] = 20;
    right.array[0] = 10;
    right.array[4] = 20;
    if (!left.values.empty() || !right.values.empty() || !(left == right)) {
        throw 8;
    }

    right.array[4] = 21;
    if (left == right) {
        throw 9;
    }

    std::ostringstream rendered;
    rendered << left;
    if (rendered.str().find("array:") == std::string::npos
        || rendered.str().find("10") == std::string::npos
        || rendered.str().find("20") == std::string::npos) {
        throw 10;
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
        throw 6;
    }
    const std::uint32_t size = static_cast<std::uint32_t>(buffer.size());
    write_integer(output, size);
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    if (!output) {
        throw 7;
    }
}
}

int main(int argument_count, char** arguments)
{
    static_assert(CorpusConstantExpressions::Add == 5, "Add");
    static_assert(CorpusConstantExpressions::Shifted == 21, "Shifted");
    static_assert(CorpusConstantExpressions::Masked == 6, "Masked");
    static_assert(CorpusConstantExpressions::Bound == 5, "Bound");
    static_assert(std::is_same<decltype(Sample::values),
                  omg::types::bounded_sequence<std::int32_t, CorpusConstantExpressions::Bound>>::value,
        "bounded sequence C++ mapping");
    static_assert(std::is_same<decltype(Sample::array),
                  dds::core::array<std::int32_t, CorpusConstantExpressions::Add>>::value,
        "fixed array C++ mapping");

    if (argument_count == 2 && std::string(arguments[1]) == "--register") {
        check_metadata_and_lifecycle();
        dds::domain::DomainParticipant participant(0);
        dds::topic::topic_type_support<Sample>::register_type(
            participant, "CorpusConstantExpressions::Sample");
        dds::topic::Topic<Sample> topic(participant, "CorpusConstantExpressions::SampleTopic");
        dds::pub::Publisher publisher(participant);
        dds::pub::DataWriter<Sample> writer(publisher, topic);
        (void) writer;
        return 0;
    }

    check_metadata_and_lifecycle();
    check_array_value_semantics();
    if (argument_count == 2 && std::string(arguments[1]) == "--metadata") {
        return 0;
    }
    if (argument_count != 3) {
        return 2;
    }
    const int sequence_size = std::atoi(arguments[2]);
    if (sequence_size < 0 || sequence_size > CorpusConstantExpressions::Bound) {
        return 2;
    }

    Sample sample;
    for (int index = 0; index < sequence_size; ++index) {
        sample.values.push_back(static_cast<std::int32_t>(index * 11 + 3));
    }
    for (std::size_t index = 0; index < sample.array.size(); ++index) {
        sample.array[index] = static_cast<std::int32_t>(index * 7 - 9);
    }

    const dds::core::policy::DataRepresentationId representations[] = {
        dds::core::policy::DataRepresentation::xcdr(),
        dds::core::policy::DataRepresentation::xcdr2()};
    std::ofstream output(arguments[1], std::ios::binary | std::ios::trunc);
    if (!output) {
        return 3;
    }
    for (const auto representation : representations) {
        append_cdr(output, sample, representation);
    }
    return 0;
}
