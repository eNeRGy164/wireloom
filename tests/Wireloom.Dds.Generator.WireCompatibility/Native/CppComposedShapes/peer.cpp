#include "05-shapes.hpp"

#include <cstdint>
#include <fstream>
#include <string>
#include <vector>

#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace
{
using CorpusCollectionShapes::BoundedLongs;
using CorpusCollectionShapes::Item;
using CorpusCollectionShapes::Row;
using CorpusCollectionShapes::Rows;
using CorpusCollectionShapes::Sample;

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
        throw 1;
    }

    const std::uint32_t length = static_cast<std::uint32_t>(buffer.size());
    write_integer(output, length);
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    if (!output) {
        throw 2;
    }
}

void set_fixed_values(Sample& sample)
{
    sample.rows[0][0] = 101;
    sample.rows[0][1] = 102;
    sample.rows[0][2] = 103;
    sample.rows[1][0] = 201;
    sample.rows[1][1] = 202;
    sample.rows[1][2] = 203;
}

Item make_item(std::int32_t id, const std::string& label)
{
    Item item;
    item.id = id;
    item.label = label;
    return item;
}

Row make_row(std::int32_t value)
{
    Row row{};
    row[0] = value;
    row[1] = value + 1;
    row[2] = value + 2;
    return row;
}

void set_item_sequences(Sample& sample, const std::string& shape)
{
    if (shape == "sequence-empty" || shape == "sequence-mixed-empty") {
        return;
    }

    const int unbounded_count = shape == "sequence-boundary" ? 100 : 2;
    for (int index = 0; index < unbounded_count; ++index) {
        sample.unboundedItems.push_back(make_item(
            static_cast<std::int32_t>(1000 + index),
            index == 0 ? "item-bound-1" : "item"));
    }

    const int bounded_count = shape == "sequence-boundary" ? 3 : 1;
    for (int index = 0; index < bounded_count; ++index) {
        sample.boundedItems.push_back(make_item(
            static_cast<std::int32_t>(2000 + index),
            index == 0 ? "bounded-12!!" : "item"));
    }
}

void set_nested_sequences(Sample& sample, const std::string& shape)
{
    if (shape == "sequence-empty") {
        return;
    }

    if (shape == "sequence-mixed-empty") {
        BoundedLongs empty_values;
        BoundedLongs partial_values;
        partial_values.push_back(301);
        sample.nestedSequences.push_back(empty_values);
        sample.nestedSequences.push_back(partial_values);

        Rows empty_rows;
        Rows populated_rows;
        populated_rows.push_back(make_row(401));
        sample.sequenceOfArrayAliases.push_back(empty_rows);
        sample.sequenceOfArrayAliases.push_back(populated_rows);
        return;
    }

    const int outer_count = shape == "sequence-boundary" ? 2 : 1;
    for (int outer_index = 0; outer_index < outer_count; ++outer_index) {
        BoundedLongs values;
        const int inner_count = shape == "sequence-boundary" ? 4 : 2;
        for (int inner_index = 0; inner_index < inner_count; ++inner_index) {
            values.push_back(static_cast<std::int32_t>(300 + outer_index * 10 + inner_index));
        }
        sample.nestedSequences.push_back(values);
    }
}

void set_array_sequences(Sample& sample, const std::string& shape)
{
    if (shape == "sequence-empty") {
        return;
    }

    const int count = shape == "sequence-boundary" ? 2 : 1;
    for (int index = 0; index < count; ++index) {
        sample.sequenceOfArrays.push_back(make_row(500 + index * 10));
    }

    if (shape != "sequence-mixed-empty") {
        const int outer_count = shape == "sequence-boundary" ? 2 : 1;
        for (int outer_index = 0; outer_index < outer_count; ++outer_index) {
            Rows rows;
            const int inner_count = shape == "sequence-boundary" ? 2 : 1;
            for (int inner_index = 0; inner_index < inner_count; ++inner_index) {
                rows.push_back(make_row(600 + outer_index * 10 + inner_index * 3));
            }
            sample.sequenceOfArrayAliases.push_back(rows);
        }
    }
}

void set_names(Sample& sample, const std::string& shape)
{
    if (shape == "sequence-empty" || shape == "sequence-mixed-empty") {
        return;
    }

    const int count = shape == "sequence-boundary" ? 4 : 1;
    for (int index = 0; index < count; ++index) {
        sample.names.push_back(index == 0 && shape == "sequence-boundary"
            ? "0123456789abcdefghijklmnopqrstuv"
            : "sample-name");
    }
}

void fill_sample(Sample& sample, const std::string& shape)
{
    set_fixed_values(sample);
    set_item_sequences(sample, shape);
    set_nested_sequences(sample, shape);
    set_array_sequences(sample, shape);
    set_names(sample, shape);
}
}

int main(int argument_count, char** arguments)
{
    if (argument_count == 2 && std::string(arguments[1]) == "--register") {
        dds::domain::DomainParticipant participant(0);
        dds::topic::topic_type_support<Sample>::register_type(
            participant, "CorpusCollectionShapes::Sample");
        dds::topic::Topic<Sample> topic(participant, "CorpusCollectionShapes::SampleTopic");
        dds::pub::Publisher publisher(participant);
        dds::pub::DataWriter<Sample> writer(publisher, topic);
        (void) writer;
        return 0;
    }

    if (argument_count != 3) {
        return 3;
    }

    const std::string shape(arguments[2]);
    if (shape != "sequence-empty"
        && shape != "sequence-unequal"
        && shape != "sequence-mixed-empty"
        && shape != "sequence-boundary") {
        return 3;
    }

    Sample sample;
    fill_sample(sample, shape);

    const dds::core::policy::DataRepresentationId representations[] = {
        dds::core::policy::DataRepresentation::xcdr(),
        dds::core::policy::DataRepresentation::xcdr2()};
    std::ofstream output(arguments[1], std::ios::binary | std::ios::trunc);
    if (!output) {
        return 4;
    }

    for (const auto representation : representations) {
        append_cdr(output, sample, representation);
    }

    return 0;
}
