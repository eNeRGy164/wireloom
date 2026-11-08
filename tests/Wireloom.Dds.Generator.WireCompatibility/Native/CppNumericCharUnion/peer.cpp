#include "07-union-char-numeric.hpp"

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
using CorpusNumericCharUnion::Choice;
using dds::core::xtypes::TypeKind;

static_assert(std::is_same<decltype(std::declval<Choice&>()._d()), char&>::value,
    "_d() must return a mutable plain-char discriminator reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>()._d()), const char&>::value,
    "const _d() must return a const plain-char discriminator reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().value()), std::int32_t&>::value,
    "value() must return a mutable long branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().value()), const std::int32_t&>::value,
    "const value() must return a const long branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().fallback()), std::int32_t&>::value,
    "fallback() must return a mutable long branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().fallback()), const std::int32_t&>::value,
    "const fallback() must return a const long branch reference");
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
        && sample.fallback() == 0,
        "default construction must use NUL and select the default long branch");
    require_precondition([&sample] { sample.value(); });

    sample.fallback(17);
    require(sample._d() == '\0' && sample.fallback() == 17,
        "one-argument fallback setter must preserve active NUL discriminator");
    sample.fallback(2301, 'q');
    require(sample._d() == 'q' && sample.fallback() == 2301,
        "two-argument fallback setter must select an unused char discriminator");
    require_precondition([&sample] { sample.fallback(9, static_cast<char>(127)); });
    require(sample._d() == 'q' && sample.fallback() == 2301,
        "rejected explicit label must preserve the existing fallback value");

    sample.value(12700);
    require(sample._d() == static_cast<char>(127) && sample.value() == 12700,
        "numeric char label 127 must select the explicit long branch");
    require_precondition([&sample] { sample.fallback(1); });

    Choice left;
    left.value(30127);
    Choice right;
    right.fallback(64001, 'r');
    left.swap(right);
    require(left._d() == 'r' && left.fallback() == 64001
        && right._d() == static_cast<char>(127) && right.value() == 30127,
        "RTI member swap must exchange discriminators and active branch values");
}

void check_dynamic_type()
{
    const dds::core::xtypes::UnionType& type = rti::topic::dynamic_type<Choice>::get();
    require(type.name() == "CorpusNumericCharUnion::Choice" && type.member_count() == 2
        && type.discriminator().kind() == TypeKind::CHAR_8_TYPE,
        "Choice DynamicType name, member count, or char discriminator mismatch");
    require(type.member(0).name() == "value"
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && type.member(0).labels().size() == 1
        && type.member(0).labels()[0] == static_cast<char>(127),
        "explicit branch DynamicType metadata must preserve numeric label 127");
    require(type.member(1).name() == "fallback"
        && type.member(1).type().kind() == TypeKind::INT_32_TYPE
        && type.member(1).labels().size() == 1
        && type.member(1).labels()[0] == dds::core::xtypes::UnionMember::DEFAULT_LABEL,
        "default branch DynamicType metadata mismatch");

    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_UNION && native->_memberCount == 2
        && native->_default_index == 1 && native->_typeCode != nullptr
        && native->_typeCode->_kind == RTI_XCDR_TK_CHAR,
        "native TypeCode must retain char discriminator and default branch index");
    require(native->_members[0]._representation._typeCode->_kind == RTI_XCDR_TK_LONG
        && native->_members[0]._label == 127
        && native->_members[1]._representation._typeCode->_kind == RTI_XCDR_TK_LONG
        && native->_members[1]._label == RTI_XCDR_TYPE_CODE_UNION_DEFAULT_LABEL,
        "native TypeCode branch kinds or numeric/default labels mismatch");
}

void check_topic_registration()
{
    dds::domain::DomainParticipant participant(0);
    dds::topic::topic_type_support<Choice>::register_type(
        participant, "CorpusNumericCharUnion::Choice");
    dds::topic::Topic<Choice> topic(participant, "CorpusNumericCharUnion::ChoiceTopic");
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
    if (shape == "explicit") {
        sample.value(12700);
        return sample;
    }
    if (shape == "fallback") {
        sample.fallback(64001, 'q');
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
        require(argument_count == 3,
            "usage: CppNumericCharUnion <cdr-output-file> <sample-shape>");
        check_consumer_api();
        check_dynamic_type();
        write_cdr_evidence(arguments[1], arguments[2]);
        return 0;
    } catch (const std::exception& exception) {
        std::cerr << exception.what() << '\n';
        return 1;
    }
}
