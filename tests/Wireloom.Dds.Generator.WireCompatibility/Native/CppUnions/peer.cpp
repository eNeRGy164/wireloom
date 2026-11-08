#include "07-unions.hpp"

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
#include <dds/topic/Topic.hpp>
#include <rti/topic/cdr.hpp>

namespace {
using CorpusUnions::Choice;

static_assert(std::is_same<decltype(std::declval<Choice&>()._d()), int32_t&>::value,
    "_d() must return a mutable discriminator reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>()._d()), const int32_t&>::value,
    "const _d() must return a const discriminator reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().number()), int32_t&>::value,
    "number() must return a mutable branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().number()), const int32_t&>::value,
    "const number() must return a const branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().text()), std::string&>::value,
    "text() must return a mutable branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().text()), const std::string&>::value,
    "const text() must return a const branch reference");
static_assert(std::is_same<decltype(std::declval<Choice&>().flag()), bool&>::value,
    "flag() must return a mutable branch reference");
static_assert(std::is_same<decltype(std::declval<const Choice&>().flag()), const bool&>::value,
    "const flag() must return a const branch reference");
static_assert(std::is_same<decltype(Choice::default_discriminator()), int32_t>::value,
    "default_discriminator() must return the discriminator type");
static_assert(std::is_same<decltype(&Choice::operator==), bool (Choice::*)(const Choice&) const>::value,
    "union equality must be a member operator");
static_assert(std::is_same<decltype(&Choice::operator!=), bool (Choice::*)(const Choice&) const>::value,
    "union inequality must be a member operator");
static_assert(std::is_same<decltype(&Choice::swap), void (Choice::*)(Choice&) noexcept>::value,
    "union swap must be a noexcept member");

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
    require(Choice::default_discriminator() == 0, "default discriminator mismatch");
    require(value._d() == 0 && value.number() == 0, "default construction mismatch");

    require_inactive([&value] { value.flag(); });
    value._d(2);
    value.flag(true);
    require(value._d() == 2 && value.flag(), "one-argument default setter must preserve discriminator");
    value.flag(false, 3);
    require(value._d() == 3 && !value.flag(), "two-argument default setter must select the requested discriminator");
    require_inactive([&value] { value.flag(true, 0); });

    value.number(7301);
    require(value._d() == 0 && value.number() == 7301, "number setter mismatch");
    require_inactive([&value] { value.text(); });
    value.text("union-text");
    require(value._d() == 1 && value.text() == "union-text", "text setter mismatch");

    Choice other;
    other.number(7301);
    require(value == value && value != other, "member comparisons mismatch");
    value.swap(other);
    require(value.number() == 7301 && other.text() == "union-text", "member swap mismatch");
    using std::swap;
    swap(value, other);
    require(value.text() == "union-text" && other.number() == 7301, "ADL swap mismatch");
}

void compile_typed_topic_consumer(dds::domain::DomainParticipant& participant)
{
    dds::topic::topic_type_support<Choice>::register_type(participant, "CorpusUnions::Choice");
    dds::topic::Topic<Choice> topic(participant, "CorpusUnions::Choice");
    dds::pub::DataWriter<Choice> writer(topic);
    (void) writer;
}

void check_dynamic_type()
{
    using dds::core::xtypes::TypeKind;
    const dds::core::xtypes::UnionType& type = rti::topic::dynamic_type<Choice>::get();
    const RTIXCdrSampleAccessInfo* access = type.native()._data._sampleAccessInfo;
    const RTIXCdrTypeCode* native = reinterpret_cast<const RTIXCdrTypeCode*>(&type.native());
    require(native->_kind == RTI_XCDR_TK_UNION && native->_memberCount == 3,
        "union native TypeCode shape mismatch");
    require(native->_default_index == 2 && native->_typeCode != NULL
        && native->_typeCode->_kind == RTI_XCDR_TK_LONG,
        "union native discriminator metadata mismatch");
    require(native->_annotations._allowedDataRepresentationMask == 5,
        "union native data representation metadata mismatch");
    require(access != NULL, "union sample-access metadata is missing");
    require(access->languageBinding == RTI_XCDR_TYPE_BINDING_CPP_11_STL
        && access->useGetMemberValueOnlyWithRef == RTI_XCDR_TRUE
        && access->getMemberValuePointerFcn != NULL,
        "union sample-access binding metadata mismatch");
    for (std::size_t index = 0; index < type.member_count(); ++index) {
        const RTIXCdrTypeCode* member_type = native->_members[index]._representation._typeCode;
        require(member_type != NULL, "union member TypeCode is missing");
        require(native->_members[index]._memberFlags == 0,
            "union member flags must match RTI's non-optional member metadata");
    }
    require(type.member_count() == 3, "union DynamicType member count mismatch");
    require(type.discriminator().kind() == TypeKind::INT_32_TYPE,
        "union discriminator DynamicType must be INT32");
    require(type.member(0).name() == "number"
        && type.member(0).type().kind() == TypeKind::INT_32_TYPE
        && type.member(0).labels().size() == 1
        && type.member(0).labels()[0] == 0,
        "number member DynamicType mismatch");
    require(type.member(1).name() == "text"
        && type.member(1).type().kind() == TypeKind::STRING_TYPE
        && type.member(1).labels().size() == 1
        && type.member(1).labels()[0] == 1,
        "text member DynamicType mismatch");
    require(type.member(2).name() == "flag"
        && type.member(2).type().kind() == TypeKind::BOOLEAN_TYPE
        && type.member(2).labels().size() == 1
        && type.member(2).labels()[0] == dds::core::xtypes::UnionMember::DEFAULT_LABEL,
        "default member DynamicType mismatch");
    Choice sample;
    const char* base = reinterpret_cast<const char*>(&sample);
    const char* discriminator = reinterpret_cast<const char*>(&sample._d());
    sample._d(0);
    const char* number = reinterpret_cast<const char*>(&sample.number());
    sample._d(1);
    const char* text = reinterpret_cast<const char*>(&sample.text());
    sample._d(2);
    const char* flag = reinterpret_cast<const char*>(&sample.flag());
    const std::ptrdiff_t expected_offsets[] = {
        discriminator - base, number - base, text - base, flag - base};
    for (std::size_t index = 0; index < 4; ++index) {
        require(access->memberAccessInfos[index].bindingMemberValueOffset[0]
            == static_cast<RTIXCdrUnsignedLong>(expected_offsets[index]),
            "union sample-access member offset mismatch");
    }
    require(access->typeSize[0] == sizeof(Choice), "union sample-access size mismatch");
    require(type.member(1).type().native()._data._maximumLength == 255,
        "union string bound mismatch");
}

void write_cdr(std::ofstream& output, const Choice& sample,
    dds::core::policy::DataRepresentationId representation)
{
    using Support = dds::topic::topic_type_support<Choice>;
    std::vector<char> buffer;
    Support::to_cdr_buffer(buffer, sample, representation);
    const std::uint32_t length = static_cast<std::uint32_t>(buffer.size());
    output.write(reinterpret_cast<const char*>(&length), sizeof(length));
    output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
    output.flush();
    require(static_cast<bool>(output), "failed to write serialized union bytes");
    Choice decoded;
    Support::from_cdr_buffer(decoded, buffer);
    std::cout << "Deserialized discriminator " << decoded._d() << std::endl;
    if (!(decoded == sample)) {
        std::cerr << "CDR mismatch disc expected=" << sample._d()
            << " actual=" << decoded._d() << " bytes=" << buffer.size() << std::endl;
    }
    require(decoded == sample, "CDR round trip changed the union value");
}

void write_cdr_evidence(const std::string& path)
{
    std::ofstream output(path.c_str(), std::ios::binary | std::ios::trunc);
    require(static_cast<bool>(output), "could not create CDR output file");
    const dds::core::policy::DataRepresentationId representations[] = {
        dds::core::policy::DataRepresentation::xcdr(),
        dds::core::policy::DataRepresentation::xcdr2()};
    Choice initial;
    Choice number;
    number.number(7301);
    Choice text;
    text.text("union-text");
    Choice default_branch;
    default_branch.flag(true, 2);
    const Choice samples[] = {initial, number, text, default_branch};
    for (const auto representation : representations) {
        for (const auto& sample : samples) {
            write_cdr(output, sample, representation);
        }
    }
}
}

int main(int argument_count, char** arguments)
{
    try {
        require(argument_count == 2, "usage: CppUnions <cdr-output-file>");
        check_consumer_api();
        std::cout << "API passed" << std::endl;
        check_dynamic_type();
        std::cout << "DynamicType passed" << std::endl;
        dds::domain::DomainParticipant participant(0);
        std::cout << "Participant created" << std::endl;
        compile_typed_topic_consumer(participant);
        std::cout << "Topic registration passed" << std::endl;
        write_cdr_evidence(arguments[1]);
        std::cout << "C022 public API assertions passed\n";
        return 0;
    } catch (const std::exception& exception) {
        std::cerr << exception.what() << '\n';
        return 1;
    }
}
