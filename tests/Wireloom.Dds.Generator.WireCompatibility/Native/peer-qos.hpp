#pragma once
#include <dds/dds.hpp>

// Share reliable-writer timing between DynamicData and typed native fixtures.
static dds::pub::qos::DataWriterQos reliable_writer_qos(const dds::pub::Publisher &publisher)
{
    auto qos = publisher.default_datawriter_qos();
    qos << dds::core::policy::Reliability(
        dds::core::policy::ReliabilityKind::RELIABLE);

    // Normalize RTI LTS defaults while preserving the 30-second inactivity window.
    auto &protocol = qos.policy<rti::core::policy::DataWriterProtocol>().rtps_reliable_writer();
    const auto heartbeat_period = dds::core::Duration::from_millisecs(50);
    protocol.heartbeat_period(heartbeat_period)
        .fast_heartbeat_period(heartbeat_period)
        .late_joiner_heartbeat_period(heartbeat_period)
        .max_heartbeat_retries(600);

    return qos;
}
