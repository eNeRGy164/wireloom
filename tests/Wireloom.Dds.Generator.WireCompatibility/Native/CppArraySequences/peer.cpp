#ifndef ARRAY_SEQUENCE_HEADER
#define ARRAY_SEQUENCE_HEADER "05-array-of-sequences.hpp"
#endif
#include ARRAY_SEQUENCE_HEADER

#include <cstdint>
#include <cstdlib>
#include <fstream>
#include <limits>
#include <string>
#include <vector>

#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace
{
using CorpusArrayOfSequences::Sample;

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

void set_values(Sample& sample, const std::string& shape)
{
    if (shape == "sequence-empty") {
        return;
    }

    if (shape == "sequence-unequal") {
        sample.values[0].push_back(101);
        sample.values[0].push_back(-102);
        sample.values[1].push_back(203);
        sample.names[0].push_back("first");
        sample.names[1].push_back("second");
        sample.names[1].push_back("third");
        return;
    }

    if (shape == "sequence-mixed-empty") {
        sample.values[1].push_back(-301);
        sample.values[1].push_back(302);
        sample.names[0].push_back("only-first");
        return;
    }

    if (shape == "sequence-boundary") {
        sample.values[0].push_back((std::numeric_limits<std::int32_t>::min)());
        sample.values[1].push_back((std::numeric_limits<std::int32_t>::max)());
        sample.names[0].push_back("1234567890123456");
        sample.names[0].push_back("");
        sample.names[0].push_back("last");
        sample.names[1].push_back("other");
    }
}
}

int main(int argument_count, char** arguments)
{
    if (argument_count == 2 && std::string(arguments[1]) == "--register") {
        dds::domain::DomainParticipant participant(0);
        dds::topic::topic_type_support<Sample>::register_type(
            participant, "CorpusArrayOfSequences::Sample");
        dds::topic::Topic<Sample> topic(participant, "CorpusArrayOfSequences::SampleTopic");
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
    set_values(sample, shape);

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
