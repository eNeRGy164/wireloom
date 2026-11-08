#include "07-union-boolean.hpp"

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
using CorpusBooleanUnion::Choice;
using dds::core::xtypes::TypeKind;

static_assert(std::is_same<decltype(std::declval<Choice&>()._d()), bool&>::value,
    "_d() must return a mutable bool discriminator reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>()._d()), const bool&>::value,
    "const _d() must return a const bool discriminator reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().enabled()), std::int32_t&>::value,
    "enabled() must return a mutable long branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().enabled()), const std::int32_t&>::value,
    "const enabled() must return a const long branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().disabled()), std::string&>::value,
    "disabled() must return a mutable string branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().disabled()), const std::string&>::value,
    "const disabled() must return a const string branch reference");
static_assert(std::is_same<decltype(Choice::default_discriminator()), bool>::value,
    "default_discriminator() must preserve the bool type");

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
    require(threw, "inactive branch access must throw PreconditionNotMetError");
}

void check_consumer_api()
{
    Choice sample;
    require(!Choice::default_discriminator() && !sample._d() && sample.disabled().empty(),
        "RTI default construction must select the explicit false/disabled branch");
    require_precondition([&sample] { sample.enabled(); });

    sample.enabled(7301);
    require(sample._d() && sample.enabled() == 7301,
        "enabled setter must select the true branch and assign its value");
    require_precondition([&sample] { sample.disabled(); });

    sample.disabled("boolean-union-text");
    require(!sample._d() && sample.disabled() == "boolean-union-text",
        "disabled setter must select the false branch and assign its string");
    require_precondition([&sample] { sample.enabled(); });

    Choice left;
    left.enabled(17);
    Choice right;
    right.disabled("swap-text");
    left.swap(right);
    require(!left._d() && left.disabled() == "swap-text"
        && right._d() && right.enabled() == 17,
        "member swap must exchange active values across the boolean branches");
}

void check_dynamic_type()
{
    const dds::core::xtypes::UnionType& type = rti::topic::dynamic_type<Choice>::get();
    require(type.name() == "CorpusBooleanUnion::Choice" && type.member_count() == 2
        && type.discriminator().kind() == TypeKind::BOOLEAN_TYPE,
        "Choice DynamicType name, member count, or bool discriminator mismatch");
    require(type.member(0).name() == "enabled"
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && type.member(0).labels().size() == 1 && type.member(0).labels()[0] == 1,
        "enabled branch DynamicType metadata must use the TRUE label 1");
    require(type.member(1).name() == "disabled"
        && type.member(1).type().kind() == TypeKind::STRING_TYPE
        && type.member(1).labels().size() == 1 && type.member(1).labels()[0] == 0,
        "disabled branch DynamicType metadata must use the FALSE label 0");
    require(type.member(1).type().native()._data._maximumLength == 255,
        "unbounded IDL string must retain RTI's 255 metadata maximum");

    const auto* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_UNION && native->_memberCount == 2
        && native->_default_index == -1
        && native->_typeCode != nullptr && native->_typeCode->_kind == RTI_XCDR_TK_BOOLEAN,
        "native TypeCode must retain a bool discriminator, two members, and no default arm");
    for (std::size_t index = 0; index < 2; ++index) {
        const auto* member_type = native->_members[index]._representation._typeCode;
        require(member_type != nullptr && native->_members[index]._memberFlags == 0,
            "native TypeCode branch representation and flags must match RTI");
        if (index == 0) {
            require(member_type->_kind == RTI_XCDR_TK_LONG
                && native->_members[index]._label == 1,
                "native enabled branch TypeCode or TRUE label mismatch");
        } else {
            require(member_type->_kind == RTI_XCDR_TK_STRING
                && native->_members[index]._label == 0,
                "native disabled branch TypeCode or FALSE label mismatch");
        }
    }
}

void check_topic_registration()
{
    dds::domain::DomainParticipant participant(0);
    dds::topic::topic_type_support<Choice>::register_type(
        participant, "CorpusBooleanUnion::Choice");
    dds::topic::Topic<Choice> topic(participant, "CorpusBooleanUnion::ChoiceTopic");
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
    if (shape == "enabled") {
        sample.enabled(7301);
        return sample;
    }
    if (shape == "disabled-text") {
        sample.disabled("boolean-union-text");
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
            "usage: CppBooleanUnion <cdr-output-file> <sample-shape>");
        check_consumer_api();
        check_dynamic_type();
        write_cdr_evidence(arguments[1], arguments[2]);
        return 0;
    } catch (const std::exception& exception) {
        std::cerr << exception.what() << '\n';
        return 1;
    }
}
