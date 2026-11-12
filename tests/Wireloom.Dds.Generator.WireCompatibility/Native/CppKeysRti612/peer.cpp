#include "keys.hpp"

#include <dds/core/xtypes/StructType.hpp>
#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/topic/Topic.hpp>
#include <rti/pub/DataWriterImpl.hpp>

#include <iostream>
#include <stdexcept>

namespace {
using Keys::Composite;
using Keys::Coordinates;
using Keys::Direct;
using Keys::Unkeyed;

void require(bool condition, const char* message)
{
    if (!condition) {
        throw std::runtime_error(message);
    }
}

template<typename Type>
void check_instance_handles(dds::domain::DomainParticipant& participant,
    const char* type_name, const Type& first, const Type& same_key, const Type& other_key)
{
    dds::topic::topic_type_support<Type>::register_type(participant, type_name);
    dds::topic::Topic<Type> topic(participant, type_name);
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Type> writer(publisher, topic);
    require(writer.register_instance(first) == writer.register_instance(same_key),
        "changing a non-key member changed the instance handle");
    require(writer.register_instance(first) != writer.register_instance(other_key),
        "changing a key member did not change the instance handle");
}

void check_direct_key(dds::domain::DomainParticipant& participant)
{
    Direct first;
    first.id = 10;
    first.payload = 1;
    Direct same_key;
    same_key.id = 10;
    same_key.payload = 2;
    Direct other_key;
    other_key.id = 11;
    other_key.payload = 1;
    check_instance_handles(participant, "Keys::Direct", first, same_key, other_key);
}

void check_composite_key(dds::domain::DomainParticipant& participant)
{
    Composite first;
    first.tenant = 4;
    first.name = "first";
    first.payload = 1;
    Composite same_key;
    same_key.tenant = 4;
    same_key.name = "first";
    same_key.payload = 2;
    Composite other_key;
    other_key.tenant = 4;
    other_key.name = "other";
    other_key.payload = 1;
    check_instance_handles(participant, "Keys::Composite", first, same_key, other_key);
}

void check_array_key(dds::domain::DomainParticipant& participant)
{
    Coordinates first;
    first.coordinates[0] = 2;
    first.coordinates[1] = 3;
    first.payload = 1;
    Coordinates same_key;
    same_key.coordinates[0] = 2;
    same_key.coordinates[1] = 3;
    same_key.payload = 2;
    Coordinates other_key;
    other_key.coordinates[0] = 2;
    other_key.coordinates[1] = 4;
    other_key.payload = 1;
    check_instance_handles(participant, "Keys::Coordinates", first, same_key, other_key);
}

void check_keyless_topic(dds::domain::DomainParticipant& participant)
{
    dds::topic::topic_type_support<Unkeyed>::register_type(participant, "Keys::Unkeyed");
    dds::topic::Topic<Unkeyed> topic(participant, "Keys::Unkeyed");
    dds::pub::Publisher publisher(participant);
    dds::pub::DataWriter<Unkeyed> writer(publisher, topic);
    Unkeyed first;
    Unkeyed second;
    first.value = 1;
    second.value = 2;
    require(writer.register_instance(first) == writer.register_instance(second),
        "keyless instances must share one handle");
}

void check_dynamic_type_keys()
{
    const auto& direct = rti::topic::dynamic_type<Direct>::get();
    const auto& composite = rti::topic::dynamic_type<Composite>::get();
    const auto& coordinates = rti::topic::dynamic_type<Coordinates>::get();
    const auto& unkeyed = rti::topic::dynamic_type<Unkeyed>::get();

    require(direct.member(0).name() == "id" && direct.member(0).is_key(),
        "Direct.id DynamicType key flag mismatch");
    require(!direct.member(1).is_key(), "Direct.payload must not be a key");
    require(composite.member(0).is_key() && composite.member(1).is_key(),
        "Composite key flags mismatch");
    require(!composite.member(2).is_key(), "Composite.payload must not be a key");
    require(coordinates.member(0).is_key() && !coordinates.member(1).is_key(),
        "Coordinates key flags mismatch");
    require(unkeyed.member(0).name() == "value" && !unkeyed.member(0).is_key(),
        "Unkeyed.value must remain unkeyed");
}
}

int main()
{
    try {
        check_dynamic_type_keys();
        dds::domain::DomainParticipant participant(0);
        check_direct_key(participant);
        check_composite_key(participant);
        check_array_key(participant);
        check_keyless_topic(participant);
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
