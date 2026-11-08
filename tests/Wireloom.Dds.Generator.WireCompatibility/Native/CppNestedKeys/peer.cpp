#include "08-key-nested.hpp"

#include <cstdint>
#include <fstream>
#include <iostream>
#include <stdexcept>
#include <string>
#include <type_traits>
#include <vector>

#include <dds/core/xtypes/StructType.hpp>
#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace {
using CorpusNestedKeys::Inner;
using CorpusNestedKeys::Outer;

static_assert(dds::topic::is_topic_type<Inner>::value,
    "Inner must be recognized as a topic type");
static_assert(dds::topic::is_topic_type<Outer>::value,
    "Outer must be recognized as a topic type");
static_assert(dds::topic::topic_type_support<Inner>::type_plugin_kind
        == rti::topic::TypePluginKind::STL,
    "Inner must use the STL type plugin");
static_assert(dds::topic::topic_type_support<Outer>::type_plugin_kind
        == rti::topic::TypePluginKind::STL,
    "Outer must use the STL type plugin");

void require(bool condition, const char* message)
{
    if (!condition) {
        throw std::runtime_error(message);
    }
}

void check_native_key_flags(
    const dds::core::xtypes::StructType& type,
    const RTIXCdrMemberFlags* expected,
    std::size_t count)
{
    const RTIXCdrTypeCode* native =
        reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_STRUCT, "native TypeCode is not a struct");
    require(native->_memberCount == count, "native TypeCode member count mismatch");

    for (std::size_t index = 0; index < count; ++index) {
        require(native->_members[index]._memberFlags == expected[index],
            "native TypeCode key/required member flags mismatch");
    }
}

void check_dynamic_type()
{
    using dds::core::xtypes::TypeKind;

    const dds::core::xtypes::StructType& inner =
        rti::topic::dynamic_type<Inner>::get();
    const dds::core::xtypes::StructType& outer =
        rti::topic::dynamic_type<Outer>::get();

    require(inner.name() == "CorpusNestedKeys::Inner" && inner.member_count() == 2,
        "Inner DynamicType identity mismatch");
    require(inner.member(0).name() == "code"
        && inner.member(0).type().kind() == TypeKind::INT_32_TYPE,
        "Inner.code DynamicType mismatch");
    require(inner.member(1).name() == "revision"
        && inner.member(1).type().kind() == TypeKind::INT_32_TYPE,
        "Inner.revision DynamicType mismatch");

    require(outer.name() == "CorpusNestedKeys::Outer" && outer.member_count() == 2,
        "Outer DynamicType identity mismatch");
    require(outer.member(0).name() == "identity"
        && outer.member(0).type().kind() == TypeKind::STRUCTURE_TYPE
        && outer.member(0).type().name() == "CorpusNestedKeys::Inner",
        "Outer.identity nested DynamicType mismatch");
    require(outer.member(1).name() == "payload"
        && outer.member(1).type().kind() == TypeKind::INT_32_TYPE,
        "Outer.payload DynamicType mismatch");

    const RTIXCdrMemberFlags inner_flags[] = {
        RTI_XCDR_KEY_MEMBER,
        RTI_XCDR_REQUIRED_MEMBER};
    const RTIXCdrMemberFlags outer_flags[] = {
        RTI_XCDR_KEY_MEMBER,
        RTI_XCDR_REQUIRED_MEMBER};
    check_native_key_flags(inner, inner_flags, 2);
    check_native_key_flags(outer, outer_flags, 2);
}

void check_topic_names()
{
    require(dds::topic::topic_type_name<Inner>::value() == "CorpusNestedKeys::Inner",
        "Inner topic name mismatch");
    require(dds::topic::topic_type_name<Outer>::value() == "CorpusNestedKeys::Outer",
        "Outer topic name mismatch");
}

void register_and_create_writers(dds::domain::DomainParticipant& participant)
{
    dds::topic::topic_type_support<Inner>::register_type(
        participant, "CorpusNestedKeys::Inner");
    dds::topic::Topic<Inner> inner_topic(participant, "CorpusNestedKeys::Inner");
    dds::pub::DataWriter<Inner> inner_writer(inner_topic);
    (void) inner_writer;

    dds::topic::topic_type_support<Outer>::register_type(
        participant, "CorpusNestedKeys::Outer");
    dds::topic::Topic<Outer> outer_topic(participant, "CorpusNestedKeys::Outer");
    dds::pub::DataWriter<Outer> outer_writer(outer_topic);
    (void) outer_writer;
}

void check_instance_keys(dds::domain::DomainParticipant& participant)
{
    {
        dds::topic::Topic<Inner> topic(participant, "CorpusNestedKeys::Inner");
        dds::pub::DataWriter<Inner> writer(topic);
        Inner first;
        first.code = 41;
        first.revision = 1;
        Inner same_key;
        same_key.code = 41;
        same_key.revision = 2;
        Inner different_key;
        different_key.code = 42;
        different_key.revision = 1;

        require(writer.register_instance(first) == writer.register_instance(same_key),
            "Inner key must be code only");
        require(writer.register_instance(first) != writer.register_instance(different_key),
            "Inner key must distinguish code values");
    }

    {
        dds::topic::Topic<Outer> topic(participant, "CorpusNestedKeys::Outer");
        dds::pub::DataWriter<Outer> writer(topic);
        Outer first;
        first.identity.code = 51;
        first.identity.revision = 7;
        first.payload = 100;
        Outer same_key;
        same_key.identity.code = 51;
        same_key.identity.revision = 7;
        same_key.payload = 200;
        Outer different_code;
        different_code.identity.code = 52;
        different_code.identity.revision = 7;
        different_code.payload = 100;
        Outer different_revision;
        different_revision.identity.code = 51;
        different_revision.identity.revision = 8;
        different_revision.payload = 100;

        require(writer.register_instance(first) == writer.register_instance(same_key),
            "Outer key must ignore the non-key payload");
        require(writer.register_instance(first) != writer.register_instance(different_code),
            "Outer nested key must distinguish Inner.code");

        const bool revision_is_part_of_outer_key =
            writer.register_instance(first) != writer.register_instance(different_revision);
        std::cout << "outer_identity_revision_is_key="
            << (revision_is_part_of_outer_key ? "true" : "false") << std::endl;
        require(!revision_is_part_of_outer_key,
            "Outer nested key must follow Inner.code and ignore Inner.revision");
    }
}

void check_api_and_registration()
{
    check_topic_names();
    check_dynamic_type();

    dds::domain::DomainParticipant participant(0);
    register_and_create_writers(participant);
    check_instance_keys(participant);
}

template<typename Type>
void append_frame(
    std::ofstream& output,
    const Type& sample,
    dds::core::policy::DataRepresentationId representation)
{
    using Support = dds::topic::topic_type_support<Type>;
    std::vector<char> buffer;
    Support::to_cdr_buffer(buffer, sample, representation);
    const std::uint32_t length = static_cast<std::uint32_t>(buffer.size());
    output.write(reinterpret_cast<const char*>(&length), sizeof(length));
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    require(static_cast<bool>(output), "failed to write CDR bytes");

    Type decoded;
    Support::from_cdr_buffer(decoded, buffer);
    require(decoded == sample, "CDR round trip changed a nested-key sample");
}

void write_cdr_evidence(const std::string& path)
{
    std::ofstream output(path.c_str(), std::ios::binary | std::ios::trunc);
    require(static_cast<bool>(output), "could not create CDR output file");

    const dds::core::policy::DataRepresentationId representations[] = {
        dds::core::policy::DataRepresentation::xcdr(),
        dds::core::policy::DataRepresentation::xcdr2()};

    Inner inner_default;
    Inner inner_value;
    inner_value.code = 73;
    inner_value.revision = 19;

    Outer outer_default;
    Outer outer_value;
    outer_value.identity.code = 7301;
    outer_value.identity.revision = 37;
    outer_value.payload = -23;

    for (const auto representation : representations) {
        append_frame(output, inner_default, representation);
        append_frame(output, inner_value, representation);
        append_frame(output, outer_default, representation);
        append_frame(output, outer_value, representation);
    }
}
}

int main(int argument_count, char** arguments)
{
    try {
        if (argument_count == 2 && std::string(arguments[1]) == "--api") {
            check_api_and_registration();
            return 0;
        }

        if (argument_count != 2) {
            throw std::runtime_error("expected --api or a CDR output path");
        }

        write_cdr_evidence(arguments[1]);
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
