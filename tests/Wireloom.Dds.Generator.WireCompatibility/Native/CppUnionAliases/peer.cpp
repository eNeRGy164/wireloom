#include "07-union-aliases.hpp"

#include <cstdint>
#include <fstream>
#include <iostream>
#include <stdexcept>
#include <string>
#include <type_traits>
#include <utility>
#include <vector>

#include <dds/core/xtypes/AliasType.hpp>
#include <dds/core/xtypes/EnumType.hpp>
#include <dds/core/xtypes/StructType.hpp>
#include <dds/core/xtypes/UnionType.hpp>
#include <dds/core/xtypes/TypeKind.hpp>
#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace
{
using CorpusUnionAliases::Choice;
using CorpusUnionAliases::ChoiceAlias;
using CorpusUnionAliases::ChoiceAlias2;
using CorpusUnionAliases::Holder;
using CorpusUnionAliases::Kind;
using dds::core::xtypes::TypeKind;

static_assert(std::is_same<ChoiceAlias, Choice>::value,
    "ChoiceAlias must use the canonical Choice value class");
static_assert(std::is_same<ChoiceAlias2, Choice>::value,
    "ChoiceAlias2 must use the canonical Choice value class");
static_assert(std::is_same<decltype(Holder::value), Choice>::value,
    "Holder must store the canonical union value through ChoiceAlias2");
static_assert(std::is_same<decltype(std::declval<Choice&>()._d()), Kind&>::value,
    "_d() must return a mutable Kind discriminator reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>()._d()), const Kind&>::value,
    "const _d() must return a const Kind discriminator reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().zero()), std::int32_t&>::value,
    "zero() must return the mutable long branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().zero()), const std::int32_t&>::value,
    "const zero() must return the const long branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().ten()), std::string&>::value,
    "ten() must return the mutable string branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().ten()), const std::string&>::value,
    "const ten() must return the const string branch reference");
static_assert(std::is_same<decltype(Choice::default_discriminator()), Kind>::value,
    "default_discriminator() must retain the enum discriminator type");

void require(bool condition, const char* message)
{
    if (!condition) {
        throw std::runtime_error(message);
    }
}

void describe_sample(const Choice& value)
{
    std::cerr << "Choice discriminator=" << static_cast<int>(value._d());
    if (value._d() == Kind::Zero) {
        std::cerr << " zero=" << value.zero();
    } else if (value._d() == Kind::Ten) {
        std::cerr << " ten=" << value.ten();
    }
    std::cerr << '\n';
}

void describe_sample(const Holder& value)
{
    std::cerr << "Holder value: ";
    describe_sample(value.value);
}

template<typename Action>
void require_precondition(Action action)
{
    bool threw = false;
    try {
        action();
    } catch (const dds::core::PreconditionNotMetError&) {
        threw = true;
    }
    require(threw, "inactive union branch access must throw PreconditionNotMetError");
}

void check_api()
{
    Choice choice;
    require(Choice::default_discriminator() == Kind::Zero
        && choice._d() == Kind::Zero
        && choice.zero() == 0,
        "default construction must select the zero branch through the alias chain");
    require_precondition([&choice] { choice.ten(); });

    choice.zero(7001);
    require(choice._d() == Kind::Zero && choice.zero() == 7001,
        "zero setter must select the canonical union's Zero arm");
    choice.ten("union-alias-text");
    require(choice._d() == Kind::Ten && choice.ten() == "union-alias-text",
        "ten setter must select the canonical union's Ten arm");
    require_precondition([&choice] { choice.zero(); });

    Choice left;
    left.zero(17);
    Choice right;
    right.ten("swapped-alias-text");
    left.swap(right);
    require(left._d() == Kind::Ten && left.ten() == "swapped-alias-text"
        && right._d() == Kind::Zero && right.zero() == 17,
        "member swap must exchange active values across canonical union arms");

    ChoiceAlias first_alias;
    first_alias.zero(31);
    ChoiceAlias2 second_alias;
    second_alias.ten("second-alias");
    require(first_alias.zero() == 31 && second_alias.ten() == "second-alias",
        "both typedef chains must expose the canonical union API");
}

void check_dynamic_type()
{
    const auto& kind_type = rti::topic::dynamic_type<Kind>::get();
    require(kind_type.name() == "CorpusUnionAliases::Kind"
        && kind_type.member_count() == 2
        && kind_type.member(0).name() == "Zero"
        && kind_type.member(0).ordinal() == 0
        && kind_type.member(1).name() == "Ten"
        && kind_type.member(1).ordinal() == 10,
        "Kind DynamicType must retain enum values Zero=0 and Ten=10");

    const auto& choice_type = rti::topic::dynamic_type<Choice>::get();
    require(choice_type.name() == "CorpusUnionAliases::Choice"
        && choice_type.member_count() == 2
        && choice_type.discriminator().kind() == TypeKind::ENUMERATION_TYPE
        && choice_type.discriminator().name() == "CorpusUnionAliases::Kind",
        "Choice DynamicType must retain its canonical enum discriminator");
    require(choice_type.member(0).name() == "zero"
        && choice_type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && choice_type.member(0).labels().size() == 1
        && choice_type.member(0).labels()[0] == 0,
        "zero branch DynamicType must retain the Zero label");
    require(choice_type.member(1).name() == "ten"
        && choice_type.member(1).type().kind() == TypeKind::STRING_TYPE
        && choice_type.member(1).labels().size() == 1
        && choice_type.member(1).labels()[0] == 10
        && choice_type.member(1).type().native()._data._maximumLength == 255,
        "ten branch DynamicType must retain Ten and RTI's string maximum 255");

    const auto& first_alias = rti::topic::dynamic_type<
        CorpusUnionAliases::ChoiceAlias_AliasTag_t>::get();
    require(first_alias.name() == "CorpusUnionAliases::ChoiceAlias"
        && first_alias.related_type().kind() == TypeKind::UNION_TYPE
        && first_alias.related_type().name() == "CorpusUnionAliases::Choice",
        "ChoiceAlias DynamicType must relate directly to canonical Choice");

    const auto& second_alias = rti::topic::dynamic_type<
        CorpusUnionAliases::ChoiceAlias2_AliasTag_t>::get();
    require(second_alias.name() == "CorpusUnionAliases::ChoiceAlias2"
        && second_alias.related_type().kind() == TypeKind::ALIAS_TYPE
        && second_alias.related_type().name() == "CorpusUnionAliases::ChoiceAlias",
        "ChoiceAlias2 DynamicType must preserve the second AliasType layer");

    const auto& holder_type = rti::topic::dynamic_type<Holder>::get();
    require(holder_type.name() == "CorpusUnionAliases::Holder"
        && holder_type.member_count() == 1
        && holder_type.member(0).name() == "value"
        && holder_type.member(0).type().kind() == TypeKind::ALIAS_TYPE,
        "Holder DynamicType must retain the declared final alias member");
    const auto& holder_alias = static_cast<const dds::core::xtypes::AliasType&>(
        holder_type.member(0).type());
    require(holder_alias.name() == "CorpusUnionAliases::ChoiceAlias2"
        && holder_alias.related_type().kind() == TypeKind::ALIAS_TYPE
        && holder_alias.related_type().name() == "CorpusUnionAliases::ChoiceAlias",
        "Holder member metadata must preserve ChoiceAlias2 then ChoiceAlias");

    const auto* native_enum = reinterpret_cast<const RTIXCdrTypeCode*>(&kind_type.native());
    const auto* native_choice = reinterpret_cast<const RTIXCdrTypeCode*>(&choice_type.native());
    const auto* native_first_alias = reinterpret_cast<const RTIXCdrTypeCode*>(&first_alias.native());
    const auto* native_second_alias = reinterpret_cast<const RTIXCdrTypeCode*>(&second_alias.native());
    const auto* native_holder = reinterpret_cast<const RTIXCdrTypeCode*>(&holder_type.native());
    Choice sample;
    const auto* sample_address = reinterpret_cast<const char*>(&sample);
    const auto* discriminator_address = reinterpret_cast<const char*>(&sample._d());
    sample._d(Kind::Zero);
    const auto* zero_address = reinterpret_cast<const char*>(&sample.zero());
    sample._d(Kind::Ten);
    const auto* ten_address = reinterpret_cast<const char*>(&sample.ten());
    require(native_enum->_kind == RTI_XCDR_TK_ENUM && native_enum->_memberCount == 2,
        "native Kind TypeCode must be an enum with two values");
    require(native_choice->_kind == RTI_XCDR_TK_UNION
        && native_choice->_default_index == -1
        && native_choice->_typeCode != nullptr
        && native_choice->_typeCode->_kind == RTI_XCDR_TK_ENUM
        && native_choice->_memberCount == 2
        && native_choice->_members[0]._label == 0
        && native_choice->_members[1]._label == 10
        && native_choice->_members[0]._representation._typeCode->_kind == RTI_XCDR_TK_LONG
        && native_choice->_members[1]._representation._typeCode->_kind == RTI_XCDR_TK_STRING,
        "native Choice TypeCode must preserve enum labels and branch representations");
    require(native_first_alias->_kind == RTI_XCDR_TK_ALIAS
        && native_first_alias->_typeCode != nullptr
        && native_first_alias->_typeCode->_kind == RTI_XCDR_TK_UNION
        && native_second_alias->_kind == RTI_XCDR_TK_ALIAS
        && native_second_alias->_typeCode != nullptr
        && native_second_alias->_typeCode->_kind == RTI_XCDR_TK_ALIAS,
        "native AliasType graph must preserve both typedef layers");
    require(native_choice->_sampleAccessInfo != nullptr
        && native_choice->_sampleAccessInfo->languageBinding == RTI_XCDR_TYPE_BINDING_CPP_11_STL
        && native_choice->_sampleAccessInfo->typeSize[0] == sizeof(Choice)
        && native_choice->_sampleAccessInfo->getMemberValuePointerFcn != nullptr
        && native_choice->_sampleAccessInfo->memberAccessInfos[0].bindingMemberValueOffset[0]
            == static_cast<unsigned long>(discriminator_address - sample_address)
        && native_choice->_sampleAccessInfo->memberAccessInfos[1].bindingMemberValueOffset[0]
            == static_cast<unsigned long>(zero_address - sample_address)
        && native_choice->_sampleAccessInfo->memberAccessInfos[2].bindingMemberValueOffset[0]
            == static_cast<unsigned long>(ten_address - sample_address),
        "native Choice sample access must describe this peer's discriminator and branch layout");
    require(native_first_alias->_sampleAccessInfo != nullptr
        && native_first_alias->_sampleAccessInfo->languageBinding == RTI_XCDR_TYPE_BINDING_CPP_11_STL
        && native_first_alias->_sampleAccessInfo->typeSize[0] == sizeof(Choice)
        && native_first_alias->_sampleAccessInfo->getMemberValuePointerFcn != nullptr
        && native_second_alias->_sampleAccessInfo != nullptr
        && native_second_alias->_sampleAccessInfo->languageBinding == RTI_XCDR_TYPE_BINDING_CPP_11_STL
        && native_second_alias->_sampleAccessInfo->typeSize[0] == sizeof(Choice)
        && native_second_alias->_sampleAccessInfo->getMemberValuePointerFcn != nullptr,
        "native alias-chain sample access must retain the C++11 value layout at both levels");
    require(native_holder->_kind == RTI_XCDR_TK_STRUCT
        && native_holder->_memberCount == 1
        && native_holder->_members[0]._representation._typeCode != nullptr
        && native_holder->_members[0]._representation._typeCode->_kind == RTI_XCDR_TK_ALIAS
        && native_holder->_sampleAccessInfo != nullptr
        && native_holder->_sampleAccessInfo->languageBinding == RTI_XCDR_TYPE_BINDING_CPP_11_STL
        && native_holder->_sampleAccessInfo->typeSize[0] == sizeof(Holder)
        && native_holder->_sampleAccessInfo->getMemberValuePointerFcn != nullptr
        && native_holder->_sampleAccessInfo->memberAccessInfos[0].bindingMemberValueOffset[0]
            == offsetof(Holder, value),
        "native Holder TypeCode must retain its declared alias member representation");
}

void check_topic_registration()
{
    dds::domain::DomainParticipant participant(0);
    dds::topic::topic_type_support<Choice>::register_type(
        participant, "CorpusUnionAliases::Choice");
    dds::topic::topic_type_support<Holder>::register_type(
        participant, "CorpusUnionAliases::Holder");

    dds::topic::Topic<Choice> choice_topic(participant, "CorpusUnionAliases::ChoiceTopic");
    dds::topic::Topic<Holder> holder_topic(participant, "CorpusUnionAliases::HolderTopic");
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Choice> choice_writer(publisher, choice_topic);
    dds::pub::DataWriter<Holder> holder_writer(publisher, holder_topic);
    (void) choice_writer;
    (void) holder_writer;
}

template<typename Type>
void write_frame(std::ofstream& output, const Type& expected,
    dds::core::policy::DataRepresentationId representation)
{
    using Support = dds::topic::topic_type_support<Type>;
    std::vector<char> buffer;
    Support::to_cdr_buffer(buffer, expected, representation);

    Type actual;
    Support::from_cdr_buffer(actual, buffer);
    if (!(actual == expected)) {
        std::cerr << "local CDR mismatch, expected: ";
        describe_sample(expected);
        std::cerr << "actual: ";
        describe_sample(actual);
        std::cerr << "serialized bytes=";
        for (const auto byte : buffer) {
            std::cerr << std::hex << static_cast<unsigned int>(
                static_cast<unsigned char>(byte)) << ' ';
        }
        std::cerr << std::dec << '\n';
        throw std::runtime_error("local CDR round-trip changed an alias-chain value");
    }

    const std::uint32_t length = static_cast<std::uint32_t>(buffer.size());
    output.write(reinterpret_cast<const char*>(&length), sizeof(length));
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    require(static_cast<bool>(output), "failed to write alias-chain CDR evidence");
}

void write_cdr_evidence(const std::string& path, const std::string& shape)
{
    Choice choice;
    Holder holder;
    if (shape == "non-default") {
        choice.ten("alias-chain-text");
        holder.value = choice;
    } else if (shape != "defaults") {
        throw std::runtime_error("unknown sample shape");
    }

    std::ofstream output(path.c_str(), std::ios::binary | std::ios::trunc);
    require(static_cast<bool>(output), "could not create alias-chain CDR output file");
    const dds::core::policy::DataRepresentationId representations[] = {
        dds::core::policy::DataRepresentation::xcdr(),
        dds::core::policy::DataRepresentation::xcdr2()};
    for (const auto representation : representations) {
        write_frame(output, choice, representation);
        write_frame(output, holder, representation);
    }
}
}

int main(int argument_count, char** arguments)
{
    try {
        check_api();
        check_dynamic_type();
        if (argument_count == 2 && std::string(arguments[1]) == "--register") {
            check_topic_registration();
            return 0;
        }
        if (argument_count == 2 && std::string(arguments[1]) == "--api") {
            return 0;
        }
        if (argument_count == 2 && std::string(arguments[1]) == "--metadata") {
            return 0;
        }
        require(argument_count == 3,
            "usage: CppUnionAliases <cdr-output-file> <sample-shape>");
        write_cdr_evidence(arguments[1], arguments[2]);
        return 0;
    } catch (const std::exception& exception) {
        std::cerr << exception.what() << '\n';
        return 1;
    }
}
