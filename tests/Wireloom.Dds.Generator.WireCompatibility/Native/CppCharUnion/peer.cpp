#include "07-union-char.hpp"

#include <cstdint>
#include <fstream>
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
using CorpusCharUnion::Choice;
using dds::core::xtypes::TypeKind;

static_assert(std::is_same<decltype(std::declval<Choice&>()._d()), char&>::value,
    "_d() must return a mutable plain-char discriminator reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>()._d()), const char&>::value,
    "const _d() must return a const plain-char discriminator reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().letter()), std::int32_t&>::value,
    "letter() must return a mutable long branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().letter()), const std::int32_t&>::value,
    "const letter() must return a const long branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().text()), std::string&>::value,
    "text() must return a mutable string branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().text()), const std::string&>::value,
    "const text() must return a const string branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().other()), bool&>::value,
    "other() must return a mutable boolean branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().other()), const bool&>::value,
    "const other() must return a const boolean branch reference");
static_assert(std::is_same<decltype(Choice::default_discriminator()), char>::value,
    "default_discriminator() must preserve the plain-char type");

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
    require(threw, "invalid or inactive branch operation must throw PreconditionNotMetError");
}

void check_consumer_api()
{
    Choice sample;
    require(Choice::default_discriminator() == '\0' && sample._d() == '\0'
        && !sample.other(),
        "RTI default construction must use NUL and select the default boolean branch");
    require_precondition([&sample] { sample.letter(); });
    require_precondition([&sample] { sample.text(); });

    sample.other(true);
    require(sample._d() == '\0' && sample.other(),
        "one-argument default setter must preserve the active NUL discriminator");
    sample.other(false, 'q');
    require(sample._d() == 'q' && !sample.other(),
        "two-argument default setter must select an unused char discriminator");
    require_precondition([&sample] { sample.other(true, 'a'); });
    require(sample._d() == 'q' && !sample.other(),
        "rejected explicit letter label must preserve the current value");
    require_precondition([&sample] { sample.other(true, 'z'); });
    require(sample._d() == 'q' && !sample.other(),
        "rejected explicit text label must preserve the current value");

    sample.letter(9701);
    require(sample._d() == 'a' && sample.letter() == 9701,
        "letter branch setter must select ASCII label 'a'");
    require_precondition([&sample] { sample.text(); });
    sample.text("char-union-text");
    require(sample._d() == 'z' && sample.text() == "char-union-text",
        "text branch setter must select ASCII label 'z'");

    Choice left;
    left.other(true, 'q');
    Choice right;
    right.other(false, 'r');
    left.swap(right);
    require(left._d() == 'r' && left.other()
        && right._d() == 'q' && !right.other(),
        "RTI member swap must swap distinct default discriminators twice through the same branch");
}

void check_dynamic_type()
{
    const dds::core::xtypes::UnionType& type = rti::topic::dynamic_type<Choice>::get();
    require(type.name() == "CorpusCharUnion::Choice" && type.member_count() == 3
        && type.discriminator().kind() == TypeKind::CHAR_8_TYPE,
        "Choice DynamicType name, member count, or char discriminator mismatch");
    require(type.member(0).name() == "letter"
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && type.member(0).labels().size() == 1 && type.member(0).labels()[0] == 'a',
        "letter branch DynamicType metadata mismatch");
    require(type.member(1).name() == "text"
        && type.member(1).type().kind() == TypeKind::STRING_TYPE
        && type.member(1).labels().size() == 1 && type.member(1).labels()[0] == 'z',
        "text branch DynamicType metadata mismatch");
    require(type.member(2).name() == "other"
        && type.member(2).type().kind() == TypeKind::BOOLEAN_TYPE
        && type.member(2).labels().size() == 1
        && type.member(2).labels()[0] == dds::core::xtypes::UnionMember::DEFAULT_LABEL,
        "default boolean branch DynamicType metadata mismatch");
    require(type.member(1).type().native()._data._maximumLength == 255,
        "unbounded IDL string must retain RTI's 255 metadata maximum");

    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_UNION && native->_memberCount == 3
        && native->_default_index == 2 && native->_typeCode != nullptr
        && native->_typeCode->_kind == RTI_XCDR_TK_CHAR,
        "native TypeCode must retain char discriminator and default branch index");
    const RTIXCdrUnsignedLong expected_labels[] = {'a', 'z'};
    for (std::size_t index = 0; index < 3; ++index) {
        const auto* member_type = native->_members[index]._representation._typeCode;
        require(member_type != nullptr && native->_members[index]._memberFlags == 0,
            "native TypeCode branch representation and required flags must match RTI");
        if (index == 0) {
            require(member_type->_kind == RTI_XCDR_TK_LONG
                && native->_members[index]._label == expected_labels[index],
                "native letter branch TypeCode or label mismatch");
        } else if (index == 1) {
            require(member_type->_kind == RTI_XCDR_TK_STRING
                && native->_members[index]._label == expected_labels[index],
                "native text branch TypeCode or label mismatch");
        } else {
            require(member_type->_kind == RTI_XCDR_TK_BOOLEAN,
                "native default branch TypeCode mismatch");
        }
    }
}

void check_topic_registration()
{
    dds::domain::DomainParticipant participant(0);
    dds::topic::topic_type_support<Choice>::register_type(
        participant, "CorpusCharUnion::Choice");
    dds::topic::Topic<Choice> topic(participant, "CorpusCharUnion::ChoiceTopic");
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Choice> writer(publisher, topic);
    (void) writer;
}

void write_frame(std::ofstream& output, const Choice& expected,
    dds::core::policy::DataRepresentationId representation)
{
    using Support = dds::topic::topic_type_support<Choice>;
    std::vector<char> buffer;
    Support::to_cdr_buffer(buffer, expected, representation);
    Choice actual;
    Support::from_cdr_buffer(actual, buffer);
    require(actual == expected, "local CDR round trip changed the union value");
    const std::uint32_t length = static_cast<std::uint32_t>(buffer.size());
    output.write(reinterpret_cast<const char*>(&length), sizeof(length));
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    require(static_cast<bool>(output), "failed to write serialized union bytes");
}

Choice make_sample(const std::string& shape)
{
    Choice sample;
    if (shape == "defaults") {
        return sample;
    }
    if (shape == "letter") {
        sample.letter(9701);
        return sample;
    }
    if (shape == "text") {
        sample.text("char-union-text");
        return sample;
    }
    if (shape == "other") {
        sample.other(true, 'q');
        return sample;
    }
    throw std::runtime_error("unknown sample shape");
}

void write_cdr_evidence(const std::string& path, const std::string& shape)
{
    std::ofstream output(path.c_str(), std::ios::binary | std::ios::trunc);
    require(static_cast<bool>(output), "could not create CDR output file");
    const Choice sample = make_sample(shape);
    const dds::core::policy::DataRepresentationId representations[] = {
        dds::core::policy::DataRepresentation::xcdr(),
        dds::core::policy::DataRepresentation::xcdr2()};
    for (const auto representation : representations) {
        write_frame(output, sample, representation);
    }
}
}

int main(int argument_count, char** arguments)
{
    try {
        if (argument_count == 2 && std::string(arguments[1]) == "--register") {
            check_topic_registration();
            return 0;
        }
        if (argument_count == 2 && std::string(arguments[1]) == "--metadata") {
            check_dynamic_type();
            return 0;
        }
        if (argument_count == 2 && std::string(arguments[1]) == "--api") {
            check_consumer_api();
            return 0;
        }
        require(argument_count == 3, "usage: CppCharUnion <cdr-output-file> <sample-shape>");
        check_consumer_api();
        check_dynamic_type();
        write_cdr_evidence(arguments[1], arguments[2]);
        return 0;
    } catch (const std::exception& exception) {
        std::cerr << exception.what() << '\n';
        return 1;
    }
}
