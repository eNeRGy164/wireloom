#include "06-alias-composition.hpp"

#include <cstdint>
#include <fstream>
#include <string>
#include <type_traits>
#include <vector>

#include <dds/core/xtypes/AliasType.hpp>
#include <dds/core/xtypes/CollectionTypes.hpp>
#include <dds/core/xtypes/EnumType.hpp>
#include <dds/core/xtypes/StructType.hpp>
#include <dds/core/xtypes/TypeKind.hpp>
#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace
{
using CorpusAggregateComposition::Identifier;
using CorpusAggregateComposition::Named;
using CorpusAggregateComposition::Sample;
using CorpusAggregateComposition::State;
using dds::core::xtypes::TypeKind;

void check_type_metadata()
{
    const auto& identifier_alias =
        rti::topic::dynamic_type<CorpusAggregateComposition::Identifier_AliasTag_t>::get();
    if (identifier_alias.name() != "CorpusAggregateComposition::Identifier"
        || identifier_alias.related_type().kind() != TypeKind::INT_32_TYPE) {
        throw 1;
    }

    const auto& state_type = rti::topic::dynamic_type<State>::get();
    if (state_type.name() != "CorpusAggregateComposition::State"
        || state_type.member_count() != 2
        || state_type.member(0).name() != "idle"
        || state_type.member(0).ordinal() != 0
        || state_type.member(1).name() != "run"
        || state_type.member(1).ordinal() != 1) {
        throw 2;
    }

    const auto& sample_type = rti::topic::dynamic_type<Sample>::get();
    if (sample_type.member_count() != 3
        || sample_type.member(0).name() != "id"
        || sample_type.member(0).type().kind() != TypeKind::ALIAS_TYPE
        || sample_type.member(1).name() != "state"
        || sample_type.member(1).type().kind() != TypeKind::ENUMERATION_TYPE
        || sample_type.member(2).name() != "values"
        || sample_type.member(2).type().kind() != TypeKind::SEQUENCE_TYPE) {
        throw 3;
    }

    const auto& sample_id_alias = static_cast<const dds::core::xtypes::AliasType&>(
        sample_type.member(0).type());
    const auto& values_type = static_cast<const dds::core::xtypes::SequenceType&>(
        sample_type.member(2).type());
    if (sample_id_alias.name() != "CorpusAggregateComposition::Identifier"
        || sample_id_alias.related_type().kind() != TypeKind::INT_32_TYPE
        || values_type.bounds() != CorpusAggregateComposition::Max
        || values_type.content_type().kind() != TypeKind::FLOAT_32_TYPE) {
        throw 4;
    }

    const auto& named_type = rti::topic::dynamic_type<Named>::get();
    if (named_type.member_count() != 1
        || named_type.member(0).name() != "sample"
        || named_type.member(0).type().kind() != TypeKind::STRUCTURE_TYPE
        || named_type.member(0).type().name() != "CorpusAggregateComposition::Sample") {
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
        throw 6;
    }

    const std::uint32_t length = static_cast<std::uint32_t>(buffer.size());
    write_integer(output, length);
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    if (!output) {
        throw 7;
    }
}

Sample make_non_default_sample()
{
    Sample sample;
    sample.id = 1201;
    sample.state = State::run;
    sample.values.push_back(1.25F);
    sample.values.push_back(-2.5F);
    sample.values.push_back(3.75F);
    sample.values.push_back(4.5F);
    return sample;
}

void check_topic_registration()
{
    dds::domain::DomainParticipant participant(0);
    dds::topic::topic_type_support<Sample>::register_type(
        participant, "CorpusAggregateComposition::Sample");
    dds::topic::topic_type_support<Named>::register_type(
        participant, "CorpusAggregateComposition::Named");

    dds::topic::Topic<Sample> sample_topic(
        participant, "CorpusAggregateComposition::SampleTopic");
    dds::topic::Topic<Named> named_topic(
        participant, "CorpusAggregateComposition::NamedTopic");
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Sample> sample_writer(publisher, sample_topic);
    dds::pub::DataWriter<Named> named_writer(publisher, named_topic);
    (void) sample_writer;
    (void) named_writer;
}
} // namespace

int main(int argument_count, char** arguments)
{
    static_assert(std::is_same<decltype(Sample::id), Identifier>::value,
        "Identifier keeps its Modern C++ storage mapping");
    static_assert(std::is_same<decltype(Sample::state), State>::value,
        "State remains the generated enum type");
    static_assert(std::is_same<decltype(Sample::values),
                  omg::types::bounded_sequence<float, CorpusAggregateComposition::Max>>::value,
        "values uses the IDL constant as its sequence bound");
    static_assert(std::is_same<decltype(Named::sample), Sample>::value,
        "Named stores Sample directly");

    check_type_metadata();
    if (argument_count == 2 && std::string(arguments[1]) == "--register") {
        check_topic_registration();
        return 0;
    }

    if (argument_count != 3) {
        return 2;
    }

    Sample sample;
    if (std::string(arguments[2]) == "non-default") {
        sample = make_non_default_sample();
    } else if (std::string(arguments[2]) != "defaults") {
        return 2;
    }
    Named named;
    named.sample = sample;

    const dds::core::policy::DataRepresentationId representations[] = {
        dds::core::policy::DataRepresentation::xcdr(),
        dds::core::policy::DataRepresentation::xcdr2()};
    std::ofstream output(arguments[1], std::ios::binary | std::ios::trunc);
    if (!output) {
        return 3;
    }

    for (const auto representation : representations) {
        append_cdr(output, sample, representation);
        append_cdr(output, named, representation);
    }
    return 0;
}
