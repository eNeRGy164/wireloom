#include "09-optional-string-sequences.hpp"

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
using CorpusOptionalStringSequences::Sample;

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

std::string narrow_boundary()
{
    return "0123456789abcdef";
}

std::wstring wide_boundary()
{
    return L"0123456789abcdef";
}

void populate(Sample& sample, int narrow_state, int wide_state)
{
    if (narrow_state > 0) {
        sample.narrowValues = ::omg::types::bounded_sequence<std::string, 4>{};
    }
    if (wide_state > 0) {
        sample.wideValues = ::omg::types::bounded_sequence<std::wstring, 4>{};
    }

    if (narrow_state >= 2) {
        sample.narrowValues.value().push_back("");
        sample.narrowValues.value().push_back("partial");
    }
    if (wide_state >= 2) {
        sample.wideValues.value().push_back(L"");
        sample.wideValues.value().push_back(L"partial");
    }
    if (narrow_state == 4) {
        sample.narrowValues.value().push_back("value");
        sample.narrowValues.value().push_back(narrow_boundary());
    }
    if (wide_state == 4) {
        sample.wideValues.value().push_back(L"value");
        sample.wideValues.value().push_back(wide_boundary());
    }
}
}

int main(int argument_count, char** arguments)
{
    if (argument_count == 2 && std::string(arguments[1]) == "--register") {
        dds::domain::DomainParticipant participant(0);
        dds::topic::topic_type_support<Sample>::register_type(
            participant, "CorpusOptionalStringSequences::Sample");
        dds::topic::Topic<Sample> topic(participant, "CorpusOptionalStringSequences::SampleTopic");
        dds::pub::Publisher publisher(participant);
        dds::pub::DataWriter<Sample> writer(publisher, topic);
        (void) writer;
        return 0;
    }

    if (argument_count != 4) {
        return 3;
    }
    const int narrow_state = std::atoi(arguments[2]);
    const int wide_state = std::atoi(arguments[3]);
    if (narrow_state < 0 || narrow_state > 4 || narrow_state == 3
        || wide_state < 0 || wide_state > 4 || wide_state == 3) {
        return 3;
    }

    Sample sample;
    populate(sample, narrow_state, wide_state);

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
