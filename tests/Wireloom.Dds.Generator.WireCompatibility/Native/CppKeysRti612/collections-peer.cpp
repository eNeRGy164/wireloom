#include "collections.hpp"
#include "optional-collections.hpp"

#include <dds/domain/DomainParticipant.hpp>
#include <dds/pub/DataWriter.hpp>
#include <dds/pub/Publisher.hpp>
#include <dds/sub/DataReader.hpp>
#include <dds/sub/Subscriber.hpp>
#include <dds/topic/Topic.hpp>

#include <chrono>
#include <cstdint>
#include <iostream>
#include <iostream>
#include <stdexcept>
#include <thread>

namespace {
void require(bool condition, const char* message)
{
    if (!condition) {
        throw std::runtime_error(message);
    }
}

template<typename Type>
void round_trip(dds::domain::DomainParticipant& participant,
    const char* type_name, const Type& first, const Type& second)
{
    dds::topic::topic_type_support<Type>::register_type(participant, type_name);
    dds::topic::Topic<Type> topic(participant, type_name);
    dds::pub::Publisher publisher(participant);
    dds::sub::Subscriber subscriber(participant);
    dds::pub::DataWriter<Type> writer(publisher, topic);
    dds::sub::DataReader<Type> reader(subscriber, topic);

    for (int attempt = 0; attempt < 500
            && reader.subscription_matched_status().current_count() == 0; ++attempt) {
        std::this_thread::sleep_for(std::chrono::milliseconds(10));
    }
    require(reader.subscription_matched_status().current_count() > 0,
        "DataWriter did not match the DataReader");
    const auto write_and_receive = [&](const Type& expected) {
        writer.write(expected);
        writer.wait_for_acknowledgments(dds::core::Duration(5, 0));
        for (int attempt = 0; attempt < 500; ++attempt) {
            auto samples = reader.take();
            for (const auto& loaned_sample : samples) {
                if (!loaned_sample.info().valid()) {
                    continue;
                }
                if (loaned_sample.data() != expected) {
                    throw std::runtime_error("received CDR sample did not match expected value");
                }
                return;
            }
            std::this_thread::sleep_for(std::chrono::milliseconds(10));
        }
        throw std::runtime_error("timed out waiting for a CDR round-trip sample");
    };
    write_and_receive(first);
    write_and_receive(second);
}

void check_collections(dds::domain::DomainParticipant& participant)
{
    CollectionSample first;
    first.fixed_values[0] = 11;
    first.values.push_back(12);
    first.aliased_values.push_back(13);
    first.label = "label-one";
    first.aliased_label = "alias-one";
    first.labels.push_back("sequence-one");
    first.color = Color::GREEN;
    first.colors.push_back(Color::BLUE);
    first.alias_value = 14;
    first.alias_color = Color::RED;
    first.optional_value = 15;
    first.optional_label = "optional-one";

    CollectionSample second;
    second.fixed_values[2] = 21;
    second.values.push_back(22);
    second.aliased_values.push_back(23);
    second.label = "label-two";
    second.aliased_label = "alias-two";
    second.labels.push_back("sequence-two");
    second.color = Color::BLUE;
    second.colors.push_back(Color::GREEN);
    second.alias_value = 24;
    second.alias_color = Color::GREEN;
    second.optional_value = 25;
    second.optional_label = "optional-two";

    round_trip(participant, "CollectionSample", first, second);
}

void check_optional_collections(dds::domain::DomainParticipant& participant)
{
    OptionalCollections absent;
    OptionalCollections present;
    ::omg::types::bounded_sequence<std::int32_t, 4> values;
    values.push_back(31);
    present.values = values;
    ::dds::core::array<std::int32_t, 2> items{};
    items[0] = 32;
    items[1] = 33;
    present.items = items;

    round_trip(participant, "OptionalCollections", absent, present);
}
}

int main()
{
    try {
        dds::domain::DomainParticipant participant(0);
        check_collections(participant);
        check_optional_collections(participant);
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
