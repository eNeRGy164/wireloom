#include "07-union-scoped.hpp"

#include <cstdint>
#include <fstream>
#include <stdexcept>
#include <string>
#include <type_traits>
#include <utility>
#include <vector>

#include <dds/core/xtypes/StructType.hpp>
#include <dds/core/xtypes/CollectionTypes.hpp>
#include <dds/core/xtypes/TypeKind.hpp>
#include <dds/core/xtypes/UnionType.hpp>
#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace
{
using CorpusScopedUnion::Choice;
using CorpusScopedUnion::Payload;
using Values = omg::types::bounded_sequence<std::int32_t, 4>;
using dds::core::xtypes::TypeKind;

static_assert(std::is_same<decltype(std::declval<Choice&>()._d()), std::int32_t&>::value,
    "_d() must return a mutable discriminator reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>()._d()), const std::int32_t&>::value,
    "const _d() must return a const discriminator reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().qualifiedPayload()), Payload&>::value,
    "aggregate branch accessor must return a mutable reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().qualifiedPayload()), const Payload&>::value,
    "const aggregate branch accessor must return a const reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().values()), Values&>::value,
    "sequence branch accessor must return a mutable bounded-sequence reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().values()), const Values&>::value,
    "const sequence branch accessor must return a const bounded-sequence reference");
static_assert(std::is_same<decltype(Choice::default_discriminator()), std::int32_t>::value,
    "default_discriminator() must preserve the discriminator type");

void require(bool condition, const char* message)
{
    if (!condition) {
        throw std::runtime_error(message);
    }
}

template<typename Action>
void require_inactive(Action action)
{
    bool threw = false;
    try {
        action();
    } catch (const dds::core::PreconditionNotMetError&) {
        threw = true;
    }
    require(threw, "inactive union branch access must throw PreconditionNotMetError");
}

void check_consumer_api()
{
    Choice value;
    require(Choice::default_discriminator() == 0 && value._d() == 0,
        "default construction must use unmatched discriminator 0");
    require_inactive([&value] { value.qualifiedPayload(); });
    require_inactive([&value] { value.values(); });

    Payload payload;
    payload.code = 901;
    payload.label = "0123456789abcdef";
    value.qualifiedPayload(payload);
    require(value._d() == 10 && value.qualifiedPayload().code == 901
        && value.qualifiedPayload().label == payload.label,
        "aggregate setter must activate label 10 and preserve its value");
    require_inactive([&value] { value.values(); });

    value._d(0);
    require(value._d() == 0,
        "setting unmatched discriminator 0 must leave no branch active");
    require_inactive([&value] { value.qualifiedPayload(); });
    require_inactive([&value] { value.values(); });

    Values sequence;
    value.values(sequence);
    require(value._d() == 11 && value.values().empty(),
        "empty sequence setter must activate label 11");
    sequence.push_back(21);
    sequence.push_back(22);
    value.values(sequence);
    require(value._d() == 11 && value.values().size() == 2
        && value.values()[0] == 21 && value.values()[1] == 22,
        "partial sequence setter must preserve its ordered values");
    sequence.push_back(23);
    sequence.push_back(24);
    value.values(sequence);
    require(value.values().size() == 4 && value.values()[3] == 24,
        "full bounded sequence must preserve four values");
}

void check_dynamic_type()
{
    const dds::core::xtypes::StructType& payload_type =
        rti::topic::dynamic_type<Payload>::get();
    require(payload_type.name() == "CorpusScopedUnion::Payload"
        && payload_type.member_count() == 2
        && payload_type.member(0).name() == "code"
        && payload_type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && payload_type.member(1).name() == "label"
        && payload_type.member(1).type().kind() == TypeKind::STRING_TYPE
        && payload_type.member(1).type().native()._data._maximumLength == 16,
        "Payload DynamicType metadata mismatch");

    const dds::core::xtypes::UnionType& choice_type =
        rti::topic::dynamic_type<Choice>::get();
    require(choice_type.name() == "CorpusScopedUnion::Choice"
        && choice_type.member_count() == 2
        && choice_type.discriminator().kind() == TypeKind::INT_32_TYPE,
        "Choice DynamicType name, member count, or discriminator mismatch");
    require(choice_type.member(0).name() == "qualifiedPayload"
        && choice_type.member(0).type().kind() == TypeKind::STRUCTURE_TYPE
        && choice_type.member(0).type().name() == "CorpusScopedUnion::Payload"
        && choice_type.member(0).labels().size() == 1
        && choice_type.member(0).labels()[0] == 10,
        "qualified aggregate branch DynamicType metadata mismatch");
    require(choice_type.member(1).name() == "values"
        && choice_type.member(1).type().kind() == TypeKind::SEQUENCE_TYPE
        && choice_type.member(1).labels().size() == 1
        && choice_type.member(1).labels()[0] == 11,
        "bounded sequence branch DynamicType metadata mismatch");
    const auto& sequence_type = static_cast<const dds::core::xtypes::SequenceType&>(
        choice_type.member(1).type());
    require(sequence_type.content_type().kind() == TypeKind::INT_32_TYPE
        && sequence_type.bounds() == 4,
        "sequence branch must be bounded sequence<long, 4>");

    // Union TypeCode normalization follows RTI's type-plugin registration path
    // for collection-bearing unions, so inspect the native graph after it runs.
    dds::domain::DomainParticipant participant(0);
    dds::topic::topic_type_support<Choice>::register_type(
        participant, "CorpusScopedUnion::ChoiceMetadata");
    const auto* choice_native = reinterpret_cast<const RTIXCdrTypeCode*>(
        &choice_type.native());
    const auto* payload_native = reinterpret_cast<const RTIXCdrTypeCode*>(
        &payload_type.native());
    require(choice_native->_typeCode != nullptr
        && choice_native->_typeCode->_kind == RTI_XCDR_TK_LONG,
        "native Choice discriminator TypeCode must be int32");
    require(choice_native->_members[0]._representation._typeCode == payload_native
        && payload_native->_sampleAccessInfo != nullptr
        && payload_native->_typePlugin != nullptr,
        "aggregate branch TypeCode must link to Payload plugin metadata");
    const auto* sequence_native =
        choice_native->_members[1]._representation._typeCode;
    require(sequence_native != nullptr
        && sequence_native->_kind == RTI_XCDR_TK_SEQUENCE
        && sequence_native->_maximumLength == 4
        && sequence_native->_typeCode != nullptr
        && sequence_native->_typeCode->_kind == RTI_XCDR_TK_LONG,
        "native sequence branch TypeCode must preserve bound 4 and int32 element type");
    require(choice_native->_members[0]._memberFlags == 0
        && choice_native->_members[1]._memberFlags == 0,
        "required union branch member flags must match RTI metadata");
}

void check_topic_registration()
{
    dds::domain::DomainParticipant participant(0);
    dds::topic::topic_type_support<Choice>::register_type(
        participant, "CorpusScopedUnion::Choice");
    dds::topic::Topic<Choice> topic(participant, "CorpusScopedUnion::ChoiceTopic");
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Choice> writer(publisher, topic);
    (void) writer;
}

template<typename Integer>
void write_integer(std::ofstream& output, Integer value)
{
    output.write(reinterpret_cast<const char*>(&value), sizeof(value));
}

void append_cdr(std::ofstream& output, const Choice& expected,
    dds::core::policy::DataRepresentationId representation)
{
    using Support = dds::topic::topic_type_support<Choice>;
    std::vector<char> buffer;
    Support::to_cdr_buffer(buffer, expected, representation);
    Choice actual;
    Support::from_cdr_buffer(actual, buffer);
    require(actual == expected, "CDR round trip changed the union value");
    write_integer(output, static_cast<std::uint32_t>(buffer.size()));
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    require(static_cast<bool>(output), "failed to write serialized union bytes");
}

Choice make_sample(const std::string& shape)
{
    Choice sample;
    if (shape == "defaults") {
        return sample;
    }
    if (shape == "payload") {
        Payload value;
        value.code = 901;
        value.label = "0123456789abcdef";
        sample.qualifiedPayload(value);
        return sample;
    }
    Values values;
    if (shape == "sequence-empty") {
        // Keep the active sequence branch empty.
    } else if (shape == "sequence-partial") {
        values.push_back(21);
        values.push_back(22);
    } else if (shape == "sequence-full") {
        values.push_back(21);
        values.push_back(22);
        values.push_back(23);
        values.push_back(24);
    } else {
        throw std::runtime_error("unknown CDR shape");
    }
    sample.values(values);
    return sample;
}
} // namespace

int main(int argument_count, char** arguments)
{
    try {
        check_consumer_api();
        check_dynamic_type();
        if (argument_count == 2 && std::string(arguments[1]) == "--metadata") {
            return 0;
        }
        if (argument_count == 2 && std::string(arguments[1]) == "--register") {
            check_topic_registration();
            return 0;
        }
        if (argument_count != 3) {
            return 2;
        }
        const Choice sample = make_sample(arguments[2]);
        const dds::core::policy::DataRepresentationId representations[] = {
            dds::core::policy::DataRepresentation::xcdr(),
            dds::core::policy::DataRepresentation::xcdr2()};
        std::ofstream output(arguments[1], std::ios::binary | std::ios::trunc);
        require(static_cast<bool>(output), "could not create CDR output file");
        for (const auto representation : representations) {
            append_cdr(output, sample, representation);
        }
        return 0;
    } catch (const std::exception& exception) {
        std::cerr << exception.what() << '\n';
        return 1;
    }
}
