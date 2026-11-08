#include "04-strings.hpp"

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
using CorpusStrings::Sample;

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
    if (shape == "empty") {
        return;
    }

    if (shape == "partial") {
        sample.unbounded = "narrow text";
        sample.bounded = "12345678";
        sample.wideUnbounded = L"wide text";
        sample.wideBounded = L"abcdefgh";
        return;
    }

    sample.unbounded.assign(255, 'n');
    sample.bounded = "12345678";
    sample.wideUnbounded.assign(255, L'w');
    sample.wideBounded = L"abcdefgh";
}
}

int main(int argument_count, char** arguments)
{
    if (argument_count == 2 && std::string(arguments[1]) == "--register") {
        dds::domain::DomainParticipant participant(0);
        dds::topic::topic_type_support<Sample>::register_type(
            participant, "CorpusStrings::Sample");
        dds::topic::Topic<Sample> topic(participant, "CorpusStrings::SampleTopic");
        dds::pub::Publisher publisher(participant);
        dds::pub::DataWriter<Sample> writer(publisher, topic);
        (void) writer;
        return 0;
    }

    if (argument_count != 3) {
        return 3;
    }
    const std::string shape(arguments[2]);
    if (shape != "empty" && shape != "partial" && shape != "boundary") {
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
