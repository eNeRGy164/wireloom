#include "07-union-short-boundaries.hpp"

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
using CorpusUnionShortBoundaries::Choice;
using dds::core::xtypes::TypeKind;

static_assert(std::is_same<decltype(std::declval<Choice&>()._d()), std::int16_t&>::value,
    "_d() must return a mutable short discriminator reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>()._d()), const std::int16_t&>::value,
    "const _d() must return a const short discriminator reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().minimum()), std::int32_t&>::value,
    "minimum() must return a mutable long branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().minimum()), const std::int32_t&>::value,
    "const minimum() must return a const long branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().maximum()), std::int32_t&>::value,
    "maximum() must return a mutable long branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().maximum()), const std::int32_t&>::value,
    "const maximum() must return a const long branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().fallback()), std::int32_t&>::value,
    "fallback() must return a mutable long branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().fallback()), const std::int32_t&>::value,
    "const fallback() must return a const long branch reference");
static_assert(std::is_same<decltype(Choice::default_discriminator()), std::int16_t>::value,
    "default_discriminator() must preserve the short discriminator type");

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
    require(Choice::default_discriminator() == 0 && sample._d() == 0,
        "RTI default construction must use unused short discriminator 0");
    require(sample.fallback() == 0,
        "default construction must activate the fallback branch at discriminator 0");
    require_precondition([&sample] { sample.minimum(); });
    require_precondition([&sample] { sample.maximum(); });

    sample.fallback(81);
    require(sample._d() == 0 && sample.fallback() == 81,
        "one-argument fallback setter must update the already-active default branch");
    sample.fallback(82, 1);
    require(sample._d() == 1 && sample.fallback() == 82,
        "two-argument fallback setter must activate an unused discriminator");
    sample.fallback(83, -32767);
    require(sample._d() == -32767 && sample.fallback() == 83,
        "two-argument fallback setter must accept any unmatched short value");
    require_precondition([&sample] { sample.fallback(90, -32768); });
    require(sample._d() == -32767 && sample.fallback() == 83,
        "rejecting the minimum explicit label must preserve the current value");
    require_precondition([&sample] { sample.fallback(91, 32767); });
    require(sample._d() == -32767 && sample.fallback() == 83,
        "rejecting the maximum explicit label must preserve the current value");

    sample.minimum(-327680);
    require(sample._d() == -32768 && sample.minimum() == -327680,
        "minimum branch setter must select the minimum short label");
    require_precondition([&sample] { sample.maximum(); });
    sample.maximum(327670);
    require(sample._d() == 32767 && sample.maximum() == 327670,
        "maximum branch setter must select the maximum short label");
    require_precondition([&sample] { sample.fallback(1); });
}

void check_dynamic_type()
{
    const dds::core::xtypes::UnionType& type = rti::topic::dynamic_type<Choice>::get();
    require(type.name() == "CorpusUnionShortBoundaries::Choice"
        && type.member_count() == 3
        && type.discriminator().kind() == TypeKind::INT_16_TYPE,
        "Choice DynamicType name, member count, or discriminator mismatch");
    require(type.member(0).name() == "minimum"
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && type.member(0).labels().size() == 1
        && type.member(0).labels()[0] == -32768,
        "minimum branch DynamicType metadata mismatch");
    require(type.member(1).name() == "maximum"
        && type.member(1).type().kind() == TypeKind::INT_32_TYPE
        && type.member(1).labels().size() == 1
        && type.member(1).labels()[0] == 32767,
        "maximum branch DynamicType metadata mismatch");
    require(type.member(2).name() == "fallback"
        && type.member(2).type().kind() == TypeKind::INT_32_TYPE
        && type.member(2).labels().size() == 1
        && type.member(2).labels()[0] == dds::core::xtypes::UnionMember::DEFAULT_LABEL,
        "fallback default-label DynamicType metadata mismatch");

    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_UNION && native->_memberCount == 3
        && native->_default_index == 2 && native->_typeCode != nullptr
        && native->_typeCode->_kind == RTI_XCDR_TK_SHORT,
        "native TypeCode must retain short discriminator and fallback branch index");
    for (std::size_t index = 0; index < 3; ++index) {
        require(native->_members[index]._representation._typeCode != nullptr
            && native->_members[index]._representation._typeCode->_kind == RTI_XCDR_TK_LONG
            && native->_members[index]._memberFlags == 0,
            "native branch TypeCodes and required flags must match RTI");
    }
}

void check_topic_registration()
{
    dds::domain::DomainParticipant participant(0);
    dds::topic::topic_type_support<Choice>::register_type(
        participant, "CorpusUnionShortBoundaries::Choice");
    dds::topic::Topic<Choice> topic(
        participant, "CorpusUnionShortBoundaries::ChoiceTopic");
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
    if (shape == "minimum") {
        sample.minimum(-327680);
        return sample;
    }
    if (shape == "maximum") {
        sample.maximum(327670);
        return sample;
    }
    if (shape == "fallback") {
        sample.fallback(64001, 1);
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
        require(argument_count == 3,
            "usage: CppShortBoundariesUnion <cdr-output-file> <sample-shape>");
        check_consumer_api();
        check_dynamic_type();
        write_cdr_evidence(arguments[1], arguments[2]);
        return 0;
    } catch (const std::exception& exception) {
        std::cerr << exception.what() << '\n';
        return 1;
    }
}
