#include "03-enum-values-explicit.hpp"

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
using Enum = CorpusEnumValues::Explicit;

void require(bool condition, const std::string& message)
{
    if (!condition) {
        throw std::runtime_error(message);
    }
}

std::string to_string(Enum value)
{
    std::ostringstream output;
    output << value;
    return output.str();
}

void check_api(const std::string& output_directory)
{
    static_assert(std::is_enum<Enum>::value, "Explicit must be a C++ enum");
    static_assert(std::is_same<std::underlying_type<Enum>::type, int>::value,
        "Explicit must retain the RTI Modern C++ enum representation");
    static_assert(static_cast<int>(Enum::NEGATIVE) == -2, "NEGATIVE must preserve its explicit value");
    static_assert(static_cast<int>(Enum::ZERO) == 0, "ZERO must preserve its explicit value");
    static_assert(static_cast<int>(Enum::GAP) == 7, "GAP must preserve its explicit value");
    static_assert(static_cast<int>(Enum::NEXT) == 8, "NEXT must increment from GAP");
    static_assert(!dds::topic::is_topic_type<Enum>::value,
        "The retained RTI Modern C++ enum is not a standalone topic type");
    require(rti::topic::default_enumerator<Enum>::value == Enum::NEGATIVE,
        "The default enumerator must be the first literal NEGATIVE");

    std::ofstream api(output_directory + "/api.txt", std::ios::out | std::ios::trunc);
    require(static_cast<bool>(api), "Could not create enum API report");
    api << "negative=" << static_cast<int>(Enum::NEGATIVE) << '\n';
    api << "zero=" << static_cast<int>(Enum::ZERO) << '\n';
    api << "gap=" << static_cast<int>(Enum::GAP) << '\n';
    api << "next=" << static_cast<int>(Enum::NEXT) << '\n';
    api << "default=" << static_cast<int>(rti::topic::default_enumerator<Enum>::value) << '\n';
    api << "topic-type=" << dds::topic::is_topic_type<Enum>::value << '\n';
    require(static_cast<bool>(api), "Could not finish enum API report");

    std::ofstream stream(output_directory + "/stream.txt", std::ios::out | std::ios::trunc);
    require(static_cast<bool>(stream), "Could not create stream report");
    stream << "negative=[" << to_string(Enum::NEGATIVE) << "]\n";
    stream << "zero=[" << to_string(Enum::ZERO) << "]\n";
    stream << "gap=[" << to_string(Enum::GAP) << "]\n";
    stream << "next=[" << to_string(Enum::NEXT) << "]\n";
    require(static_cast<bool>(stream), "Could not finish stream report");
}

void check_dynamic_type(const std::string& output_directory)
{
    using namespace dds::core::xtypes;
    const EnumType& type = rti::topic::dynamic_type<Enum>::get();
    require(type.name() == "CorpusEnumValues::Explicit", "Unexpected DynamicType name");
    require(type.member_count() == 4, "Expected four enum literals");
    require(type.extensibility_kind() == ExtensibilityKind::EXTENSIBLE,
        "Expected RTI's extensible enum profile");

    const char* names[] = {"NEGATIVE", "ZERO", "GAP", "NEXT"};
    const std::int32_t ordinals[] = {-2, 0, 7, 8};
    std::ofstream metadata(output_directory + "/metadata.txt", std::ios::out | std::ios::trunc);
    require(static_cast<bool>(metadata), "Could not create DynamicType report");
    metadata << "type-name=" << type.name() << '\n';
    metadata << "member-count=" << type.member_count() << '\n';
    metadata << "extensibility=" << type.extensibility_kind().underlying() << '\n';
    for (std::size_t index = 0; index < type.member_count(); ++index) {
        const auto& member = type.member(index);
        require(member.name() == names[index], "DynamicType enum member name/order differs from IDL");
        require(member.ordinal() == ordinals[index], "DynamicType enum ordinal differs from IDL");
        metadata << "member=" << index << '|' << member.name() << '|' << member.ordinal() << '\n';
    }
    require(static_cast<bool>(metadata), "Could not finish DynamicType report");
}
}

int main(int argc, char** argv)
{
    try {
        if (argc != 3 || std::string(argv[1]) != "contract") {
            throw std::runtime_error("Usage: peer contract <output-directory>");
        }
        check_api(argv[2]);
        check_dynamic_type(argv[2]);
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << '\n';
        return 1;
    }
}
