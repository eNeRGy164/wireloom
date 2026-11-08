#include "01-full-widths.hpp"

#include <cstdint>
#include <fstream>
#include <type_traits>
#include <vector>

namespace
{
template<typename Integer>
void write_integer(std::ofstream& output, Integer value)
{
    output.write(reinterpret_cast<const char*>(&value), sizeof(value));
}
}

int main(int argument_count, char** arguments)
{
    if (argument_count != 2) {
        return 2;
    }

    using CorpusFullWidths::Sample;
    using Support = dds::topic::topic_type_support<Sample>;

    static_assert(std::is_same<decltype(Sample::i8), std::int8_t>::value, "int8 mapping");
    static_assert(std::is_same<decltype(Sample::i16), std::int16_t>::value, "int16 mapping");
    static_assert(std::is_same<decltype(Sample::i32), std::int32_t>::value, "int32 mapping");
    static_assert(std::is_same<decltype(Sample::i64), std::int64_t>::value, "int64 mapping");
    static_assert(std::is_same<decltype(Sample::u8), std::uint8_t>::value, "uint8 mapping");
    static_assert(std::is_same<decltype(Sample::u16), std::uint16_t>::value, "uint16 mapping");
    static_assert(std::is_same<decltype(Sample::u32), std::uint32_t>::value, "uint32 mapping");
    static_assert(std::is_same<decltype(Sample::u64), std::uint64_t>::value, "uint64 mapping");

    const Sample expected(-1, -2, -3, -4, 1, 2, 3, 4);
    const dds::core::policy::DataRepresentationId representations[] = {
        dds::core::policy::DataRepresentation::xcdr(),
        dds::core::policy::DataRepresentation::xcdr2()};
    std::ofstream output(arguments[1], std::ios::binary | std::ios::trunc);
    if (!output) {
        return 3;
    }

    for (const auto representation : representations) {
        std::vector<char> buffer;
        Support::to_cdr_buffer(buffer, expected, representation);

        Sample actual;
        Support::from_cdr_buffer(actual, buffer);
        if (!(actual == expected)) {
            return 1;
        }

        const std::uint32_t length = static_cast<std::uint32_t>(buffer.size());
        write_integer(output, length);
        output.write(buffer.data(), static_cast<std::streamsize>(buffer.size()));
        if (!output) {
            return 4;
        }
    }

    return 0;
}
