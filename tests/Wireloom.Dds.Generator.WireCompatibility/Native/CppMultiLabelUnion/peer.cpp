#include "07-union-multilabel.hpp"

#include <cstdint>
#include <fstream>
#include <iostream>
#include <stdexcept>
#include <string>
#include <type_traits>
#include <utility>
#include <vector>

#include <dds/core/xtypes/TypeKind.hpp>
#include <dds/core/xtypes/UnionType.hpp>
#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace
{
using CorpusMultiLabelUnion::Choice;
using dds::core::xtypes::TypeKind;

static_assert(std::is_same<decltype(std::declval<Choice&>()._d()), std::int32_t&>::value,
    "_d() must return a mutable discriminator reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>()._d()), const std::int32_t&>::value,
    "const _d() must return a const discriminator reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().number()), std::int32_t&>::value,
    "number() must return a mutable branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().number()), const std::int32_t&>::value,
    "const number() must return a const branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().text()), std::string&>::value,
    "text() must return a mutable branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().text()), const std::string&>::value,
    "const text() must return a const branch reference");
static_assert(std::is_same<decltype(Choice::default_discriminator()), std::int32_t>::value,
    "default_discriminator() must preserve the discriminator type");
static_assert(std::is_same<decltype(std::declval<Choice&>().number(std::int32_t{}, std::int32_t{})), void>::value,
    "number(value, discriminator) must select one of the branch labels");

void require(bool condition, const char* message)
{
    if (!condition) {
        throw std::runtime_error(message);
    }
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
    require(threw, "invalid/inactive union operation must throw PreconditionNotMetError");
}

template<typename Action>
void require_inactive(Action action)
{
    require_precondition(action);
}

void check_consumer_api()
{
    Choice value;
    require(Choice::default_discriminator() == 0 && value._d() == 0,
        "default construction must use unmatched discriminator zero");
    require_inactive([&value] { value.number(); });
    require_inactive([&value] { value.text(); });

    value.number(7301);
    require(value._d() == 1 && value.number() == 7301,
        "one-argument number setter must choose its first label, 1");
    require_inactive([&value] { value.text(); });

    value.number(7302, 5);
    require(value._d() == 5 && value.number() == 7302,
        "two-argument number setter must select label 5");
    require_inactive([&value] { value.text(); });

    value.text("multilabel-text");
    require(value._d() == 2 && value.text() == "multilabel-text",
        "text setter must select label 2");
    require_inactive([&value] { value.number(); });

    require_precondition([&value] { value.number(999, 3); });
    require(value._d() == 2 && value.text() == "multilabel-text",
        "invalid number label must leave the current union value unchanged");
    require_precondition([&value] { value.number(999, 2); });
    require(value._d() == 2 && value.text() == "multilabel-text",
        "label 2 must not activate the number branch");
}

void check_dynamic_type()
{
    const dds::core::xtypes::UnionType& type =
        rti::topic::dynamic_type<Choice>::get();
    require(type.name() == "CorpusMultiLabelUnion::Choice"
        && type.member_count() == 2
        && type.discriminator().kind() == TypeKind::INT_32_TYPE,
        "Choice DynamicType shape or discriminator mismatch");
    require(type.member(0).name() == "number"
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && type.member(0).labels().size() == 2
        && type.member(0).labels()[0] == 1
        && type.member(0).labels()[1] == 5,
        "number member must retain labels 1 and 5");
    require(type.member(1).name() == "text"
        && type.member(1).type().kind() == TypeKind::STRING_TYPE
        && type.member(1).labels().size() == 1
        && type.member(1).labels()[0] == 2,
        "text member must retain label 2");
    require(type.member(1).type().native()._data._maximumLength == 255,
        "RTI default string bound must remain 255");

    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_UNION
        && native->_memberCount == 2
        && native->_typeCode != nullptr
        && native->_typeCode->_kind == RTI_XCDR_TK_LONG,
        "native union TypeCode must have two members and int32 discriminator");
    require(native->_members[0]._labelsCount == 2
        && native->_members[0]._labels != nullptr
        && native->_members[0]._labels[0] == 1
        && native->_members[0]._labels[1] == 5
        && native->_members[0]._representation._typeCode != nullptr
        && native->_members[0]._representation._typeCode->_kind == RTI_XCDR_TK_LONG,
        "native number branch TypeCode or label sequence mismatch");
    require(native->_members[1]._labelsCount == 1
        && native->_members[1]._label == 2
        && native->_members[1]._representation._typeCode != nullptr
        && native->_members[1]._representation._typeCode->_kind == RTI_XCDR_TK_STRING
        && native->_members[1]._representation._typeCode->_maximumLength == 255,
        "native text branch TypeCode or label mismatch");
    require(native->_members[0]._memberFlags == 0
        && native->_members[1]._memberFlags == 0,
        "required branch flags must match the RTI oracle");
}

void check_topic_registration()
{
    dds::domain::DomainParticipant participant(0);
    dds::topic::topic_type_support<Choice>::register_type(
        participant, "CorpusMultiLabelUnion::Choice");
    dds::topic::Topic<Choice> topic(participant, "CorpusMultiLabelUnion::ChoiceTopic");
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
    if (shape == "number-label-1") {
        sample.number(7301);
        return sample;
    }
    if (shape == "number-label-5") {
        sample.number(7302, 5);
        return sample;
    }
    if (shape == "text") {
        sample.text("multilabel-text");
        return sample;
    }
    throw std::runtime_error("unknown CDR shape");
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
