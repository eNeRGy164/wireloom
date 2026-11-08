#include "07-union-enum.hpp"

#include <cstdint>
#include <fstream>
#include <string>
#include <type_traits>
#include <vector>

#include <dds/core/xtypes/EnumType.hpp>
#include <dds/core/xtypes/StructType.hpp>
#include <dds/core/xtypes/TypeKind.hpp>
#include <dds/core/xtypes/UnionType.hpp>
#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace
{
using CorpusEnumUnion::Choice;
using CorpusEnumUnion::Kind;
using CorpusEnumUnion::Payload;
using dds::core::xtypes::TypeKind;

static_assert(std::is_same<decltype(std::declval<Choice&>()._d()), Kind&>::value,
    "_d() must return a mutable enum discriminator reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>()._d()), const Kind&>::value,
    "const _d() must return a const enum discriminator reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().number()), std::int32_t&>::value,
    "number() must return a mutable branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().number()), const std::int32_t&>::value,
    "const number() must return a const branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().text()), std::string&>::value,
    "text() must return a mutable branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().text()), const std::string&>::value,
    "const text() must return a const branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().payload()), Payload&>::value,
    "payload() must return a mutable aggregate branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().payload()), const Payload&>::value,
    "const payload() must return a const aggregate branch reference");
static_assert(std::is_same<decltype(Choice::default_discriminator()), Kind>::value,
    "default_discriminator() must preserve the enum type");

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
    require(threw, "inactive branch access must throw PreconditionNotMetError");
}

void check_consumer_api()
{
    Choice value;
    require(Choice::default_discriminator() == Kind::Number
        && value._d() == Kind::Number && value.number() == 0,
        "default construction must select the first enumerator / number branch");

    require_inactive([&value] { value.text(); });
    require_inactive([&value] { value.payload(); });
    value.number(7301);
    require(value._d() == Kind::Number && value.number() == 7301,
        "number setter must activate the matching enum discriminator");

    value.text("enum-union-text");
    require(value._d() == Kind::Text && value.text() == "enum-union-text",
        "text setter must activate the matching enum discriminator");
    require_inactive([&value] { value.number(); });

    Payload payload;
    payload.code = 901;
    payload.label.assign(16, 'p');
    value.payload(payload);
    require(value._d() == Kind::PayloadValue
        && value.payload().code == 901 && value.payload().label == payload.label,
        "aggregate setter must activate the matching enum discriminator");
    require_inactive([&value] { value.text(); });
}

void check_dynamic_type()
{
    const dds::core::xtypes::EnumType& kind_type =
        rti::topic::dynamic_type<Kind>::get();
    require(kind_type.name() == "CorpusEnumUnion::Kind"
        && kind_type.member_count() == 3
        && kind_type.member(0).name() == "Number" && kind_type.member(0).ordinal() == 0
        && kind_type.member(1).name() == "Text" && kind_type.member(1).ordinal() == 1
        && kind_type.member(2).name() == "PayloadValue" && kind_type.member(2).ordinal() == 2,
        "enum DynamicType names and ordinals mismatch");

    const dds::core::xtypes::StructType& payload_type =
        rti::topic::dynamic_type<Payload>::get();
    require(payload_type.name() == "CorpusEnumUnion::Payload"
        && payload_type.member_count() == 2
        && payload_type.member(0).name() == "code"
        && payload_type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && payload_type.member(1).name() == "label"
        && payload_type.member(1).type().kind() == TypeKind::STRING_TYPE
        && payload_type.member(1).type().native()._data._maximumLength == 16,
        "Payload DynamicType structure or string<16> metadata mismatch");

    const dds::core::xtypes::UnionType& choice_type =
        rti::topic::dynamic_type<Choice>::get();
    require(choice_type.name() == "CorpusEnumUnion::Choice"
        && choice_type.member_count() == 3
        && choice_type.discriminator().kind() == TypeKind::ENUMERATION_TYPE
        && choice_type.discriminator().name() == "CorpusEnumUnion::Kind",
        "Choice DynamicType enum discriminator or member count mismatch");
    const auto* choice_native = reinterpret_cast<const RTIXCdrTypeCode*>(
        &choice_type.native());
    const auto* payload_native = reinterpret_cast<const RTIXCdrTypeCode*>(
        &payload_type.native());
    const auto* payload_member_native =
        choice_native->_members[2]._representation._typeCode;
    require(choice_native->_typeCode != nullptr
        && choice_native->_typeCode->_kind == RTI_XCDR_TK_ENUM
        && choice_native->_typeCode->_name != nullptr
        && std::string(choice_native->_typeCode->_name) == "CorpusEnumUnion::Kind",
        "native union discriminator TypeCode must link to CorpusEnumUnion::Kind");
    require(payload_member_native != nullptr
        && payload_member_native == payload_native
        && payload_member_native->_kind == RTI_XCDR_TK_STRUCT
        && payload_member_native->_name != nullptr
        && std::string(payload_member_native->_name) == "CorpusEnumUnion::Payload",
        "payload union branch must carry a native struct TypeCode");
    require(payload_member_native->_sampleAccessInfo != nullptr
        && payload_member_native->_typePlugin != nullptr
        && payload_native->_sampleAccessInfo != nullptr
        && payload_native->_typePlugin != nullptr,
        "nested payload TypeCode must preserve RTI's sample-access and type-plugin metadata");
    for (std::size_t index = 0; index < choice_type.member_count(); ++index) {
        require(choice_native->_members[index]._memberFlags == 0,
            "required union branches must match RTI's non-optional member flags");
    }
    require(choice_type.member(0).name() == "number"
        && choice_type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && choice_type.member(0).labels().size() == 1
        && choice_type.member(0).labels()[0] == 0,
        "number union member metadata mismatch");
    require(choice_type.member(1).name() == "text"
        && choice_type.member(1).type().kind() == TypeKind::STRING_TYPE
        && choice_type.member(1).labels().size() == 1
        && choice_type.member(1).labels()[0] == 1,
        "text union member metadata mismatch");
    require(choice_type.member(2).name() == "payload"
        && choice_type.member(2).type().kind() == TypeKind::STRUCTURE_TYPE
        && choice_type.member(2).type().name() == "CorpusEnumUnion::Payload"
        && choice_type.member(2).labels().size() == 1
        && choice_type.member(2).labels()[0] == 2,
        "payload union member must link to the Payload aggregate TypeCode");
    const dds::core::xtypes::StructType& nested_payload =
        static_cast<const dds::core::xtypes::StructType&>(choice_type.member(2).type());
    require(nested_payload.member(1).type().native()._data._maximumLength == 16,
        "nested Payload TypeCode must retain its bounded string metadata");
}

void check_topic_registration()
{
    dds::domain::DomainParticipant participant(0);
    dds::topic::topic_type_support<Choice>::register_type(
        participant, "CorpusEnumUnion::Choice");
    dds::topic::Topic<Choice> topic(participant, "CorpusEnumUnion::ChoiceTopic");
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
    if (shape == "number") {
        sample.number(7301);
    } else if (shape == "text") {
        sample.text("enum-union-text");
    } else if (shape == "payload") {
        Payload value;
        value.code = 901;
        value.label.assign(16, 'p');
        sample.payload(value);
    } else {
        throw std::runtime_error("unknown CDR shape");
    }
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
