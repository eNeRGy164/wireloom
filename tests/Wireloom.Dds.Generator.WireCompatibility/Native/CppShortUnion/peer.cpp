#include "07-union-short.hpp"

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
using CorpusShortUnion::Choice;
using dds::core::xtypes::TypeKind;

static_assert(std::is_same<decltype(std::declval<Choice&>()._d()), std::int16_t&>::value,
    "_d() must return a mutable short discriminator reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>()._d()), const std::int16_t&>::value,
    "const _d() must return a const short discriminator reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().negative()), std::int32_t&>::value,
    "negative() must return a mutable long branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().negative()), const std::int32_t&>::value,
    "const negative() must return a const long branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().text()), std::string&>::value,
    "text() must return a mutable string branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().text()), const std::string&>::value,
    "const text() must return a const string branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().other()), bool&>::value,
    "other() must return a mutable boolean branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().other()), const bool&>::value,
    "const other() must return a const boolean branch reference");
static_assert(std::is_same<decltype(Choice::default_discriminator()), std::int16_t>::value,
    "default_discriminator() must preserve the short discriminator type");

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
    Choice sample;
    require(Choice::default_discriminator() == 0 && sample._d() == 0,
        "default construction must use discriminator 0");
    require(sample.other() == false,
        "default branch must be active for the default short discriminator");
    require_inactive([&sample] { sample.negative(); });
    require_inactive([&sample] { sample.text(); });

    sample._d(4);
    sample.other(true);
    require(sample._d() == 4 && sample.other(),
        "one-argument default setter must preserve the selected discriminator");
    sample.other(false, 3);
    require(sample._d() == 3 && !sample.other(),
        "two-argument default setter must select a valid unmatched discriminator");
    require_inactive([&sample] { sample.other(true, -1); });
    require_inactive([&sample] { sample.other(true, 2); });

    sample.negative(-7301);
    require(sample._d() == -1 && sample.negative() == -7301,
        "negative branch setter must select label -1");
    require_inactive([&sample] { sample.text(); });
    sample.text("short-union-text");
    require(sample._d() == 2 && sample.text() == "short-union-text",
        "text branch setter must select label 2");
}

void check_dynamic_type()
{
    const dds::core::xtypes::UnionType& type = rti::topic::dynamic_type<Choice>::get();
    require(type.name() == "CorpusShortUnion::Choice" && type.member_count() == 3
        && type.discriminator().kind() == TypeKind::INT_16_TYPE,
        "Choice DynamicType name, member count, or short discriminator mismatch");
    require(type.member(0).name() == "negative"
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && type.member(0).labels().size() == 1 && type.member(0).labels()[0] == -1,
        "negative branch DynamicType metadata mismatch");
    require(type.member(1).name() == "text"
        && type.member(1).type().kind() == TypeKind::STRING_TYPE
        && type.member(1).labels().size() == 1 && type.member(1).labels()[0] == 2,
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
        && native->_typeCode != nullptr && native->_typeCode->_kind == RTI_XCDR_TK_SHORT,
        "native TypeCode must retain the signed short discriminator");
    require(native->_members[0]._representation._typeCode != nullptr
        && native->_members[0]._representation._typeCode->_kind == RTI_XCDR_TK_LONG
        && native->_members[1]._representation._typeCode != nullptr
        && native->_members[1]._representation._typeCode->_kind == RTI_XCDR_TK_STRING
        && native->_members[2]._representation._typeCode != nullptr
        && native->_members[2]._representation._typeCode->_kind == RTI_XCDR_TK_BOOLEAN,
        "native TypeCode branch kinds must match the RTI oracle");
    require(native->_members[0]._memberFlags == 0
        && native->_members[1]._memberFlags == 0
        && native->_members[2]._memberFlags == 0,
        "required union branches must have RTI's non-optional member flags");
}

void check_topic_registration()
{
    dds::domain::DomainParticipant participant(0);
    dds::topic::topic_type_support<Choice>::register_type(
        participant, "CorpusShortUnion::Choice");
    dds::topic::Topic<Choice> topic(participant, "CorpusShortUnion::ChoiceTopic");
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
    if (shape == "negative") {
        sample.negative(-7301);
        return sample;
    }
    if (shape == "text") {
        sample.text("short-union-text");
        return sample;
    }
    if (shape == "default-branch") {
        sample.other(true, 3);
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
        require(argument_count == 3, "usage: CppShortUnion <cdr-output-file> <sample-shape>");
        check_consumer_api();
        check_dynamic_type();
        write_cdr_evidence(arguments[1], arguments[2]);
        return 0;
    } catch (const std::exception& exception) {
        std::cerr << exception.what() << '\n';
        return 1;
    }
}
