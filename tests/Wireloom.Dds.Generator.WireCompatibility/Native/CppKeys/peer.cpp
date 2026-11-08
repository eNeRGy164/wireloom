#include "08-keys.hpp"

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
using CorpusKeys::Identity;
using CorpusKeys::Sample;
using CorpusKeys::Simple;

static_assert(dds::topic::is_topic_type<Simple>::value,
    "Simple must be recognized as a topic type");
static_assert(dds::topic::is_topic_type<Identity>::value,
    "Identity must be recognized as a topic type");
static_assert(dds::topic::is_topic_type<Sample>::value,
    "Sample must be recognized as a topic type");

void require(bool condition, const char* message)
{
    if (!condition) {
        throw std::runtime_error(message);
    }
}

template<typename Type>
void check_topic_type_name(const char* expected)
{
    require(dds::topic::topic_type_name<Type>::value() == expected,
        "topic_type_name mismatch");
}

template<typename Type>
void register_and_create_writer(
    dds::domain::DomainParticipant& participant,
    const char* type_name)
{
    dds::topic::topic_type_support<Type>::register_type(participant, type_name);
    dds::topic::Topic<Type> topic(participant, type_name);
    dds::pub::DataWriter<Type> writer(topic);
    (void) writer;
}

void check_key_instance_behavior(dds::domain::DomainParticipant& participant)
{
    {
        dds::topic::Topic<Simple> topic(participant, "CorpusKeys::Simple");
        dds::pub::DataWriter<Simple> writer(topic);
        Simple first;
        first.id = 4;
        first.name = "first";
        Simple same_key;
        same_key.id = 4;
        same_key.name = "other";
        Simple different_key;
        different_key.id = 5;
        different_key.name = "first";
        require(writer.register_instance(first) == writer.register_instance(same_key),
            "Simple instance identity must use id only");
        require(writer.register_instance(first) != writer.register_instance(different_key),
            "Simple instance identity must distinguish id values");
    }

    {
        dds::topic::Topic<Identity> topic(participant, "CorpusKeys::Identity");
        dds::pub::DataWriter<Identity> writer(topic);
        Identity first;
        first.tenant = 4;
        first.name = "first";
        Identity same_key;
        same_key.tenant = 4;
        same_key.name = "first";
        Identity different_name;
        different_name.tenant = 4;
        different_name.name = "other";
        Identity different_tenant;
        different_tenant.tenant = 5;
        different_tenant.name = "first";
        require(writer.register_instance(first) == writer.register_instance(same_key),
            "Identity instance identity must use the composite key");
        require(writer.register_instance(first) != writer.register_instance(different_name),
            "Identity instance identity must include name");
        require(writer.register_instance(first) != writer.register_instance(different_tenant),
            "Identity instance identity must include tenant");
    }

    {
        dds::topic::Topic<Sample> topic(participant, "CorpusKeys::Sample");
        dds::pub::DataWriter<Sample> writer(topic);
        Sample first;
        first.identity.tenant = 4;
        first.identity.name = "first";
        first.payload = 1;
        Sample same_key;
        same_key.identity.tenant = 4;
        same_key.identity.name = "first";
        same_key.payload = 2;
        Sample different_key;
        different_key.identity.tenant = 4;
        different_key.identity.name = "other";
        different_key.payload = 1;
        Sample different_tenant;
        different_tenant.identity.tenant = 5;
        different_tenant.identity.name = "first";
        different_tenant.payload = 1;
        require(writer.register_instance(first) == writer.register_instance(same_key),
            "Sample instance identity must propagate the nested key");
        require(writer.register_instance(first) != writer.register_instance(different_key),
            "Sample instance identity must include nested name");
        require(writer.register_instance(first) != writer.register_instance(different_tenant),
            "Sample instance identity must include nested tenant");
    }
}

void check_native_members(
    const dds::core::xtypes::StructType& type,
    const RTIXCdrMemberFlags* expected_flags,
    std::size_t count)
{
    const RTIXCdrTypeCode* native =
        reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_STRUCT, "native TypeCode is not a struct");
    require(native->_memberCount == count, "native TypeCode member count mismatch");
    for (std::size_t index = 0; index < count; ++index) {
        if (native->_members[index]._memberFlags != expected_flags[index]) {
            std::cerr << type.member(index).name()
                << " native member flags actual="
                << static_cast<unsigned int>(native->_members[index]._memberFlags)
                << " expected=" << static_cast<unsigned int>(expected_flags[index])
                << std::endl;
            throw std::runtime_error("native TypeCode key/required flags mismatch");
        }
    }
}

void check_dynamic_type()
{
    using dds::core::xtypes::TypeKind;

    const dds::core::xtypes::StructType& simple =
        rti::topic::dynamic_type<Simple>::get();
    const dds::core::xtypes::StructType& identity =
        rti::topic::dynamic_type<Identity>::get();
    const dds::core::xtypes::StructType& sample =
        rti::topic::dynamic_type<Sample>::get();

    require(simple.name() == "CorpusKeys::Simple" && simple.member_count() == 2,
        "Simple DynamicType identity mismatch");
    require(simple.member(0).name() == "id"
        && simple.member(0).type().kind() == TypeKind::INT_32_TYPE,
        "Simple.id DynamicType mismatch");
    require(simple.member(1).name() == "name"
        && simple.member(1).type().kind() == TypeKind::STRING_TYPE,
        "Simple.name DynamicType mismatch");

    require(identity.name() == "CorpusKeys::Identity" && identity.member_count() == 2,
        "Identity DynamicType identity mismatch");
    require(identity.member(0).name() == "tenant"
        && identity.member(0).type().kind() == TypeKind::INT_32_TYPE,
        "Identity.tenant DynamicType mismatch");
    require(identity.member(1).name() == "name"
        && identity.member(1).type().kind() == TypeKind::STRING_TYPE,
        "Identity.name DynamicType mismatch");

    require(sample.name() == "CorpusKeys::Sample" && sample.member_count() == 2,
        "Sample DynamicType identity mismatch");
    require(sample.member(0).name() == "identity"
        && sample.member(0).type().name() == "CorpusKeys::Identity",
        "Sample.identity nested DynamicType mismatch");
    require(sample.member(1).name() == "payload"
        && sample.member(1).type().kind() == TypeKind::INT_32_TYPE,
        "Sample.payload DynamicType mismatch");

    const RTIXCdrMemberFlags simple_flags[] = {
        RTI_XCDR_KEY_MEMBER,
        RTI_XCDR_REQUIRED_MEMBER};
    const RTIXCdrMemberFlags identity_flags[] = {
        RTI_XCDR_KEY_MEMBER,
        RTI_XCDR_KEY_MEMBER};
    const RTIXCdrMemberFlags sample_flags[] = {
        RTI_XCDR_KEY_MEMBER,
        RTI_XCDR_REQUIRED_MEMBER};
    check_native_members(simple, simple_flags, 2);
    check_native_members(identity, identity_flags, 2);
    check_native_members(sample, sample_flags, 2);
}

void check_api_and_registration()
{
    check_topic_type_name<Simple>("CorpusKeys::Simple");
    check_topic_type_name<Identity>("CorpusKeys::Identity");
    check_topic_type_name<Sample>("CorpusKeys::Sample");
    check_dynamic_type();

    dds::domain::DomainParticipant participant(0);
    register_and_create_writer<Simple>(participant, "CorpusKeys::Simple");
    register_and_create_writer<Identity>(participant, "CorpusKeys::Identity");
    register_and_create_writer<Sample>(participant, "CorpusKeys::Sample");
    check_key_instance_behavior(participant);
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
    require(static_cast<bool>(output), "failed to write serialized sample bytes");

    Type decoded;
    Support::from_cdr_buffer(decoded, buffer);
    require(decoded == sample, "CDR round trip changed a keyed sample");
}

void write_cdr_evidence(const std::string& path)
{
    std::ofstream output(path.c_str(), std::ios::binary | std::ios::trunc);
    require(static_cast<bool>(output), "could not create CDR output file");

    const dds::core::policy::DataRepresentationId representations[] = {
        dds::core::policy::DataRepresentation::xcdr(),
        dds::core::policy::DataRepresentation::xcdr2()};

    Simple simple_default;
    Simple simple_value;
    simple_value.id = 41;
    simple_value.name = "simple-key";

    Identity identity_default;
    Identity identity_value;
    identity_value.tenant = 7;
    identity_value.name = "identity-key";

    Sample sample_default;
    Sample sample_value;
    sample_value.identity.tenant = 19;
    sample_value.identity.name = "nested-key";
    sample_value.payload = 7301;

    for (const auto representation : representations) {
        append_frame(output, simple_default, representation);
        append_frame(output, simple_value, representation);
        append_frame(output, identity_default, representation);
        append_frame(output, identity_value, representation);
        append_frame(output, sample_default, representation);
        append_frame(output, sample_value, representation);
    }
}
}

int main(int argument_count, char** arguments)
{
    try {
        if (argument_count == 2 && std::string(arguments[1]) == "--api") {
            check_api_and_registration();
            std::cout << "API, key metadata, registration, and DataWriter passed" << std::endl;
            return 0;
        }
        require(argument_count == 2, "usage: CppKeys <cdr-output-file> or --api");
        write_cdr_evidence(arguments[1]);
        std::cout << "CDR round trips passed" << std::endl;
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
