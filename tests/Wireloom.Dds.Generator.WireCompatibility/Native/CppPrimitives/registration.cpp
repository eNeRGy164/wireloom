#include "01-primitives.hpp"

#include <dds/dds.hpp>

int main()
{
    using CorpusPrimitives::Sample;

    dds::domain::DomainParticipant participant(68);
    const auto type_name = dds::topic::topic_type_name<Sample>::value();
    dds::topic::topic_type_support<Sample>::register_type(participant, type_name);

    const dds::topic::Topic<Sample> topic(participant, "WireloomCppPrimitivesRegistration");
    const dds::pub::Publisher publisher(participant);
    const dds::pub::DataWriter<Sample> writer(publisher, topic);
    (void) writer;

    return 0;
}
