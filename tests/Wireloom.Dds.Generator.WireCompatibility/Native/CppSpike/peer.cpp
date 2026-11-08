#include "10-annotations.hpp"
#include "../peer-qos.hpp"

#include <array>
#include <chrono>
#include <fstream>
#include <iostream>
#include <stdexcept>
#include <thread>

namespace {
using CorpusAnnotations::Sample;
using Clock = std::chrono::steady_clock;
using Reader = dds::sub::DataReader<Sample>;
using Writer = dds::pub::DataWriter<Sample>;
using Handle = dds::core::InstanceHandle;

struct Phase {
    int id;
    int value;
    bool dispose;
};

const std::array<Phase, 5> phases{{
    {10101, 10, false},
    {10102, 20, false},
    {10101, 11, false},
    {10101, 0, true},
    {10102, 21, false},
}};

template<class Condition>
void await(Condition condition, const std::string& context)
{
    const auto deadline = Clock::now() + std::chrono::seconds(15);
    while (!condition()) {
        if (Clock::now() >= deadline) {
            throw std::runtime_error("Timed out: " + context);
        }
        std::this_thread::sleep_for(std::chrono::milliseconds(25));
    }
}

std::string acknowledgement(const std::string& directory, std::size_t index)
{
    return directory + "/read-" + std::to_string(index + 1);
}

void write_phases(Writer& writer, const std::string& directory)
{
    await([&] { return writer.publication_matched_status().current_count() == 1; }, "writer discovery");
    for (std::size_t index = 0; index < phases.size(); ++index) {
        const auto& phase = phases[index];
        const Sample value(phase.id, phase.value);
        if (phase.dispose) {
            const auto handle = writer.lookup_instance(value);
            if (handle == Handle::nil()) {
                throw std::runtime_error("The writer could not find the instance to dispose");
            }
            writer.dispose_instance(handle);
        } else {
            writer.write(value);
        }

        await([&] { return std::ifstream(acknowledgement(directory, index)).good(); },
            "reader acknowledgement for phase " + std::to_string(index + 1));
        std::cout << "Writer confirmed phase " << index + 1 << std::endl;
    }
}

void read_phases(Reader& reader, const std::string& directory)
{
    auto first = Handle::nil();
    auto second = Handle::nil();
    for (std::size_t index = 0; index < phases.size(); ++index) {
        const auto& phase = phases[index];
        await([&] {
            const auto samples = reader.take();
            if (samples.length() == 0) {
                return false;
            }
            if (samples.length() != 1) {
                throw std::runtime_error("Expected exactly one sample for the current phase");
            }

            const auto& sample = *samples.begin();
            const auto& info = sample.info();
            const auto handle = info.instance_handle();
            auto& expected_handle = phase.id == phases.front().id ? first : second;
            if (phase.dispose) {
                if (info.valid() || handle != first
                    || info.state().instance_state() != dds::sub::status::InstanceState::not_alive_disposed()) {
                    throw std::runtime_error("Expected a disposal for the first key");
                }
            } else {
                if (!info.valid() || sample.data().id != phase.id || sample.data().value != phase.value) {
                    throw std::runtime_error("Received sample key or payload did not match the current phase");
                }
                if (expected_handle == Handle::nil()) {
                    expected_handle = handle;
                }
                if (handle == Handle::nil() || handle != expected_handle
                    || (second != Handle::nil() && first == second)) {
                    throw std::runtime_error("Received sample used the wrong keyed instance");
                }
            }
            return true;
        }, "reading phase " + std::to_string(index + 1));

        std::ofstream marker(acknowledgement(directory, index));
        if (!(marker << "validated\n")) {
            throw std::runtime_error("Could not acknowledge the validated phase");
        }
        std::cout << "Reader validated phase " << index + 1 << std::endl;
    }
}
}

int main(int argc, char** argv)
{
    try {
        if (argc != 5) {
            throw std::runtime_error("Usage: peer <reader|writer> <xcdr1|xcdr2> <topic> <scenario-directory>");
        }
        const std::string role = argv[1];
        const std::string encoding = argv[2];
        if ((role != "reader" && role != "writer") || (encoding != "xcdr1" && encoding != "xcdr2")) {
            throw std::runtime_error("Invalid role or data representation");
        }
        const Sample initial;
        if (initial.id != 0 || initial.value != 3) {
            throw std::runtime_error("Default construction differs from the corpus contract");
        }
        Sample reset(901, 902);
        dds::topic::topic_type_support<Sample>::reset_sample(reset);
        if (reset.id != 0 || reset.value != 3) {
            throw std::runtime_error("Type-support reset differs from the IDL defaults");
        }
        dds::topic::topic_type_support<Sample>::allocate_sample(reset, 1, 1);
        if (reset.id != 0 || reset.value != 3) {
            throw std::runtime_error("Type-support allocation differs from the IDL defaults");
        }

        using namespace dds::core::policy;
        auto participant_qos = dds::domain::qos::DomainParticipantQos();
        participant_qos << rti::core::policy::TransportBuiltin::UDPv4();
        const dds::domain::DomainParticipant participant(67, participant_qos);
        const dds::topic::Topic<Sample> topic(participant, argv[3]);
        const DataRepresentationIdSeq ids{
            encoding == "xcdr1" ? DataRepresentation::xcdr() : DataRepresentation::xcdr2()};
        const DataRepresentation representations(ids);

        if (role == "writer") {
            const dds::pub::Publisher publisher(participant);
            auto qos = reliable_writer_qos(publisher);
            qos << representations << History::KeepAll();
            Writer writer(publisher, topic, qos);
            write_phases(writer, argv[4]);
        } else {
            const dds::sub::Subscriber subscriber(participant);
            auto qos = subscriber.default_datareader_qos();
            qos << Reliability::Reliable() << representations << History::KeepAll();
            Reader reader(subscriber, topic, qos);
            read_phases(reader, argv[4]);
        }
    } catch (const std::exception& error) {
        std::cerr << error.what() << '\n';
        return 1;
    }
    return 0;
}
