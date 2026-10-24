#include <dds/dds.hpp>
#include <chrono>
#include <iostream>
#include <stdexcept>
#include <string>
#include <thread>
#include <utility>
#include <vector>
#include "idl.hpp"
#include "idlPlugin.hpp"

using CorpusAggregates::Recursive;

static Recursive expected_sample()
{
    Recursive root;
    Recursive child;
    Recursive leaf;
#ifdef WIRELOOM_RTI_7_3_1
    leaf.value(6103);
    child.value(6102);
    root.value(6101);
    std::vector<Recursive> grandchildren;
    grandchildren.push_back(leaf);
    child.children(std::move(grandchildren));
    std::vector<Recursive> children;
    children.push_back(child);
    root.children(std::move(children));
#else
    leaf.value = 6103;
    child.value = 6102;
    root.value = 6101;
    child.children.push_back(leaf);
    root.children.push_back(child);
#endif
    return root;
}

static bool matches(const Recursive& actual)
{
#ifdef WIRELOOM_RTI_7_3_1
    return actual.value() == 6101
        && actual.children().size() == 1
        && actual.children()[0].value() == 6102
        && actual.children()[0].children().size() == 1
        && actual.children()[0].children()[0].value() == 6103;
#else
    return actual.value == 6101
        && actual.children.size() == 1
        && actual.children[0].value == 6102
        && actual.children[0].children.size() == 1
        && actual.children[0].children[0].value == 6103;
#endif
}

int main(int argc, char** argv)
{
    if (argc != 6) {
        std::cerr << "usage: wire-peer-recursive-sequence <writer|reader> <topic> "
            "<timeout-seconds> <case> <fixture>\n";
        return 2;
    }

    try {
        const std::string role(argv[1]);
        const std::string topic_name(argv[2]);
        const int timeout_seconds = std::stoi(argv[3]);
        const std::string case_id(argv[4]);
        const std::string fixture(argv[5]);
        if (case_id != "06-aggregates" || fixture != "direct-recursive-sequence") {
            throw std::runtime_error("Unexpected recursive-sequence matrix fixture.");
        }

        auto participant_qos = dds::domain::DomainParticipant::default_participant_qos();
        auto resource_limits = participant_qos.policy<
            rti::core::policy::DomainParticipantResourceLimits>();
        resource_limits.type_object_max_serialized_length(8192);
        participant_qos << resource_limits;
        auto discovery = participant_qos.policy<rti::core::policy::DiscoveryConfig>();
        discovery.enabled_builtin_channels(
            rti::core::policy::DiscoveryConfigBuiltinChannelKindMask::service_request());
        participant_qos << discovery;

        dds::domain::DomainParticipant participant(0, participant_qos);
        dds::topic::Topic<Recursive> topic(participant, topic_name);
        if (role == "writer") {
            dds::pub::Publisher publisher(participant);
            auto writer_qos = publisher.default_datawriter_qos();
            writer_qos << dds::core::policy::Reliability(
                dds::core::policy::ReliabilityKind::RELIABLE);
            dds::pub::DataWriter<Recursive> writer(publisher, topic, writer_qos);
            const auto deadline = std::chrono::steady_clock::now()
                + std::chrono::seconds(timeout_seconds);
            while (writer.publication_matched_status().current_count() == 0
                && std::chrono::steady_clock::now() < deadline) {
                std::this_thread::sleep_for(std::chrono::milliseconds(50));
            }
            if (writer.publication_matched_status().current_count() == 0) {
                throw std::runtime_error("No reader matched the recursive-sequence topic.");
            }

            std::cout << "WRITER_MATCHED case=" << case_id << " fixture=" << fixture
                << " readers_current=1" << std::endl;
            writer.write(expected_sample());
            writer.wait_for_acknowledgments(dds::core::Duration(10, 0));
            std::cout << "SENT direct-recursive-sequence" << std::endl;
            std::cout << "PASS " << case_id << " Writer" << std::endl;
            return 0;
        }

        if (role == "reader") {
            dds::sub::Subscriber subscriber(participant);
            auto reader_qos = subscriber.default_datareader_qos();
            reader_qos << dds::core::policy::Reliability(
                dds::core::policy::ReliabilityKind::RELIABLE);
            dds::sub::DataReader<Recursive> reader(subscriber, topic, reader_qos);
            std::cout << "READY case=" << case_id << " fixture=" << fixture
                << " Reader" << std::endl;

            const auto deadline = std::chrono::steady_clock::now()
                + std::chrono::seconds(timeout_seconds);
            while (std::chrono::steady_clock::now() < deadline) {
                const auto samples = reader.take();
                for (const auto& sample : samples) {
                    if (!sample.info().valid()) {
                        continue;
                    }

                    if (!matches(sample.data())) {
                        throw std::runtime_error(
                            "The received recursive sample did not contain 6101, 6102, and 6103.");
                    }

                    std::cout << "RECEIVED direct-recursive-sequence "
                        << "root=6101 child=6102 leaf=6103" << std::endl;
                    std::cout << "PASS " << case_id << " Reader" << std::endl;
                    return 0;
                }

                std::this_thread::sleep_for(std::chrono::milliseconds(50));
            }

            throw std::runtime_error("Timed out waiting for a recursive sample.");
        }

        throw std::runtime_error("Role must be writer or reader.");
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
