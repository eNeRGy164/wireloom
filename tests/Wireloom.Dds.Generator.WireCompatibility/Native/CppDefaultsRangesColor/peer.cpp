#include "09-defaults-ranges.hpp"

#include <cstdint>
#include <fstream>
#include <iostream>
#include <sstream>
#include <stdexcept>
#include <string>
#include <type_traits>

#include <dds/core/xtypes/EnumType.hpp>

namespace
{
using Color = CorpusDefaultsAndRanges::Color;

void require(bool condition, const char* message)
{
    if (!condition) { throw std::runtime_error(message); }
}

std::string to_string(Color value)
{
    std::ostringstream output;
    output << value;
    return output.str();
}

void check_api(const std::string& api_path, const std::string& stream_path)
{
    static_assert(std::is_enum<Color>::value, "Color must be a C++ enum");
    static_assert(std::is_same<std::underlying_type<Color>::type, int>::value,
        "Color must retain the RTI Modern C++ enum representation");
    static_assert(static_cast<int>(Color::GREEN) == 0, "GREEN ordinal mismatch");
    static_assert(static_cast<int>(Color::RED) == 1, "RED ordinal mismatch");
    static_assert(static_cast<int>(Color::BLUE) == 2, "BLUE ordinal mismatch");
    static_assert(!dds::topic::is_topic_type<Color>::value,
        "The RTI Modern C++ standalone enum is not a topic type");
    require(rti::topic::default_enumerator<Color>::value == Color::RED,
        "Declared default literal RED must be available through the RTI trait");

    std::ofstream api(api_path.c_str(), std::ios::out | std::ios::trunc);
    require(static_cast<bool>(api), "Could not create Color API evidence");
    api << "green=" << static_cast<int>(Color::GREEN) << '\n';
    api << "red=" << static_cast<int>(Color::RED) << '\n';
    api << "blue=" << static_cast<int>(Color::BLUE) << '\n';
    api << "default=" << static_cast<int>(rti::topic::default_enumerator<Color>::value) << '\n';
    api << "topic-type=" << dds::topic::is_topic_type<Color>::value << '\n';
    require(static_cast<bool>(api), "Could not finish Color API evidence");

    std::ofstream stream(stream_path.c_str(), std::ios::out | std::ios::trunc);
    require(static_cast<bool>(stream), "Could not create Color stream evidence");
    stream << "green=[" << to_string(Color::GREEN) << "]\n";
    stream << "red=[" << to_string(Color::RED) << "]\n";
    stream << "blue=[" << to_string(Color::BLUE) << "]\n";
    require(static_cast<bool>(stream), "Could not finish Color stream evidence");
}

void check_dynamic_type(const std::string& output_path)
{
    using namespace dds::core::xtypes;
    const EnumType& type = rti::topic::dynamic_type<Color>::get();
    require(type.name() == "CorpusDefaultsAndRanges::Color", "Color DynamicType name mismatch");
    require(type.member_count() == 3, "Color must expose three enum literals");
    require(type.extensibility_kind() == ExtensibilityKind::EXTENSIBLE,
        "Color default extensibility mismatch");
    const char* names[] = {"GREEN", "RED", "BLUE"};
    const std::int32_t ordinals[] = {0, 1, 2};
    std::ofstream metadata(output_path.c_str(), std::ios::out | std::ios::trunc);
    require(static_cast<bool>(metadata), "Could not create Color DynamicType evidence");
    metadata << "type-name=" << type.name() << '\n';
    metadata << "member-count=" << type.member_count() << '\n';
    metadata << "extensibility=" << type.extensibility_kind().underlying() << '\n';
    for (std::size_t i = 0; i < type.member_count(); ++i) {
        const auto& member = type.member(i);
        require(member.name() == names[i], "Color DynamicType literal order/name mismatch");
        require(member.ordinal() == ordinals[i], "Color DynamicType ordinal mismatch");
        metadata << "literal=" << i << '|' << member.name() << '|' << member.ordinal() << '\n';
    }
    require(static_cast<bool>(metadata), "Could not finish Color DynamicType evidence");
}
}

int main(int argc, char** argv)
{
    try {
        if (argc == 4 && std::string(argv[1]) == "--api") {
            check_api(argv[2], argv[3]);
        } else if (argc == 3 && std::string(argv[1]) == "--dynamic-type") {
            check_dynamic_type(argv[2]);
        } else {
            throw std::runtime_error("Expected --api <api-path> <stream-path> or --dynamic-type <path>");
        }
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << std::endl;
        return 1;
    }
}
