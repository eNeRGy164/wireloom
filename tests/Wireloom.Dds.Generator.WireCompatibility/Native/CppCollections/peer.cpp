#include "05-collections.hpp"

#include <cstdint>
#include <cstdlib>
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
using CorpusCollections::Sample;

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
    sample.values[0][0] = 101;
    sample.values[0][1] = 102;
    sample.values[0][2] = 103;
    sample.values[1][0] = 201;
    sample.values[1][1] = 202;
    sample.values[1][2] = 203;

    sample.grid[0][0] = 501;
    sample.grid[0][1] = 502;
    sample.grid[0][2] = 503;
    sample.grid[1][0] = 601;
    sample.grid[1][1] = 602;
    sample.grid[1][2] = 603;
}

void set_sequences(Sample& sample, const std::string& shape)
{
    if (shape == "empty") {
        return;
    }

    const int unbounded_count = shape == "full" ? 100 : 1;
    for (int index = 0; index < unbounded_count; ++index) {
        sample.unbounded.push_back(static_cast<std::int32_t>(301 + index));
    }

    const int bounded_count = shape == "full" ? 4 : 1;
    for (int index = 0; index < bounded_count; ++index) {
        sample.bounded.push_back(static_cast<std::int32_t>(401 + index));
    }

    const int item_count = shape == "full" ? 2 : 1;
    for (int index = 0; index < item_count; ++index) {
        CorpusCollections::Item item;
        item.id = static_cast<std::int32_t>(701 + index);
        item.label = index == 0 ? "0123456789abcdef" : "item-702";
        sample.items.push_back(item);
    }
}
}

int main(int argument_count, char** arguments)
{
    if (argument_count == 2 && std::string(arguments[1]) == "--register") {
        dds::domain::DomainParticipant participant(0);
        dds::topic::topic_type_support<Sample>::register_type(
            participant, "CorpusCollections::Sample");
        dds::topic::Topic<Sample> topic(participant, "CorpusCollections::SampleTopic");
        dds::pub::Publisher publisher(participant);
        dds::pub::DataWriter<Sample> writer(publisher, topic);
        (void) writer;
        return 0;
    }

    if (argument_count != 3) {
        return 3;
    }
    const std::string shape(arguments[2]);
    if (shape != "empty" && shape != "single" && shape != "full") {
        return 3;
    }

    Sample sample;
    set_fixed_values(sample);
    set_sequences(sample, shape);

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
