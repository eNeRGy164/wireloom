#include "06-aggregates.hpp"

#include <cstdint>
#include <fstream>
#include <iostream>
#include <stdexcept>
#include <string>
#include <vector>

#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace
{
using CorpusAggregates::Base;
using CorpusAggregates::Composed;
using CorpusAggregates::Derived;
using CorpusAggregates::Recursive;

template<typename Integer>
void write_integer(std::ofstream& output, Integer value)
{
    output.write(reinterpret_cast<const char*>(&value), sizeof(value));
}

template<typename Type>
void append_cdr(std::ofstream& output, const Type& expected,
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

Recursive make_recursive(std::int32_t value, int depth)
{
    Recursive node;
    node.value = value;
    if (depth > 1) {
        node.children.push_back(make_recursive(value + 1, depth - 1));
    }
    return node;
}

template<typename Type>
void append_both_representations(std::ofstream& output, const Type& sample)
{
    append_cdr(output, sample, dds::core::policy::DataRepresentation::xcdr());
    append_cdr(output, sample, dds::core::policy::DataRepresentation::xcdr2());
}

void register_types()
{
    dds::domain::DomainParticipant participant(0);
    dds::topic::topic_type_support<Base>::register_type(participant, "CorpusAggregates::Base");
    dds::topic::topic_type_support<Derived>::register_type(participant, "CorpusAggregates::Derived");
    dds::topic::topic_type_support<Recursive>::register_type(participant, "CorpusAggregates::Recursive");
    dds::topic::topic_type_support<Composed>::register_type(participant, "CorpusAggregates::Composed");
    dds::topic::Topic<Base> base_topic(participant, "CorpusAggregates::BaseTopic");
    dds::topic::Topic<Derived> derived_topic(participant, "CorpusAggregates::DerivedTopic");
    dds::topic::Topic<Recursive> recursive_topic(participant, "CorpusAggregates::RecursiveTopic");
    dds::topic::Topic<Composed> composed_topic(participant, "CorpusAggregates::ComposedTopic");
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Base> base_writer(publisher, base_topic);
    dds::pub::DataWriter<Derived> derived_writer(publisher, derived_topic);
    dds::pub::DataWriter<Recursive> recursive_writer(publisher, recursive_topic);
    dds::pub::DataWriter<Composed> composed_writer(publisher, composed_topic);
    (void) base_writer;
    (void) derived_writer;
    (void) recursive_writer;
    (void) composed_writer;
}

void register_recursive_type()
{
    dds::domain::DomainParticipant participant(0);
    dds::topic::topic_type_support<Recursive>::register_type(
        participant, "CorpusAggregates::Recursive");
}

void create_recursive_topic()
{
    dds::domain::DomainParticipant participant(0);
    dds::topic::topic_type_support<Recursive>::register_type(
        participant, "CorpusAggregates::Recursive");
    dds::topic::Topic<Recursive> topic(participant, "CorpusAggregates::RecursiveTopic");
    (void) topic;
}

void verify_recursive_registration_is_unsupported()
{
    try {
        register_recursive_type();
    } catch (const std::exception& exception) {
        if (std::string(exception.what()).find("Strongly Connected Components")
            != std::string::npos) {
            return;
        }

        throw;
    }

    throw std::runtime_error("Recursive type registration unexpectedly succeeded.");
}

void register_nonrecursive_types()
{
    dds::domain::DomainParticipant participant(0);
    dds::topic::topic_type_support<Base>::register_type(participant, "CorpusAggregates::Base");
    dds::topic::topic_type_support<Derived>::register_type(participant, "CorpusAggregates::Derived");
    dds::topic::topic_type_support<Composed>::register_type(participant, "CorpusAggregates::Composed");
    dds::topic::Topic<Base> base_topic(participant, "CorpusAggregates::BaseTopic");
    dds::topic::Topic<Derived> derived_topic(participant, "CorpusAggregates::DerivedTopic");
    dds::topic::Topic<Composed> composed_topic(participant, "CorpusAggregates::ComposedTopic");
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Base> base_writer(publisher, base_topic);
    dds::pub::DataWriter<Derived> derived_writer(publisher, derived_topic);
    dds::pub::DataWriter<Composed> composed_writer(publisher, composed_topic);
    (void) base_writer;
    (void) derived_writer;
    (void) composed_writer;
}

}

int main(int argument_count, char** arguments)
{
    try {
    if (argument_count == 2 && std::string(arguments[1]) == "--register") {
        register_types();
        return 0;
    }
    if (argument_count == 2 && std::string(arguments[1]) == "--register-recursive") {
        register_recursive_type();
        return 0;
    }
    if (argument_count == 2 && std::string(arguments[1]) == "--create-recursive-topic") {
        create_recursive_topic();
        return 0;
    }
    if (argument_count == 2 && std::string(arguments[1]) == "--expect-recursive-registration-unsupported") {
        verify_recursive_registration_is_unsupported();
        return 0;
    }
    if (argument_count == 2 && std::string(arguments[1]) == "--register-nonrecursive") {
        register_nonrecursive_types();
        return 0;
    }

    if (argument_count != 3) {
        return 3;
    }

    const std::string shape(arguments[2]);
    std::ofstream output(arguments[1], std::ios::binary | std::ios::trunc);
    if (!output) {
        return 4;
    }

    if (shape == "base-default" || shape == "base-value") {
        Base sample{};
        if (shape == "base-value") sample.base = 6101;
        append_both_representations(output, sample);
    } else if (shape == "derived-empty" || shape == "derived-value" || shape == "derived-boundary") {
        Derived sample{};
        if (shape == "derived-value") {
            sample.base = 6201;
            sample.derived = "derived-6201";
        } else if (shape == "derived-boundary") {
            sample.base = 6216;
            sample.derived = "0123456789abcdef";
        }
        append_both_representations(output, sample);
    } else if (shape == "recursive-empty" || shape == "recursive-depth") {
        Recursive sample{};
        if (shape == "recursive-depth") sample = make_recursive(6301, 3);
        append_both_representations(output, sample);
    } else if (shape == "composed-default" || shape == "composed-value") {
        Composed sample{};
        if (shape == "composed-value") {
            sample.base.base = 6401;
            sample.derived.base = 6402;
            sample.derived.derived = "composed-derived";
        }
        append_both_representations(output, sample);
    } else {
        return 3;
    }

    return 0;
    } catch (const std::exception& exception) {
        std::cerr << "C019 peer failed: " << exception.what() << '\n';
        return 10;
    }
}
