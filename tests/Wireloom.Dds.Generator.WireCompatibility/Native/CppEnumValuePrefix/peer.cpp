#include "03-enum-values-prefix.hpp"

#include <fstream>
#include <iostream>
#include <sstream>
#include <stdexcept>
#include <string>
#include <type_traits>

#include <dds/core/xtypes/EnumType.hpp>

namespace
{
using Enum = CorpusEnumValuePrefix::Annotated;

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

void check_enum_api(const std::string& output_directory)
{
    static_assert(std::is_enum<Enum>::value, "Annotated must be a C++ enum");
    static_assert(std::is_same<std::underlying_type<Enum>::type, int>::value,
        "Annotated must retain the RTI Modern C++ enum representation");
    static_assert(static_cast<int>(Enum::FIRST) == 7, "FIRST must preserve @value(7)");
    static_assert(static_cast<int>(Enum::SECOND) == 8, "SECOND must increment from FIRST");
    static_assert(!dds::topic::is_topic_type<Enum>::value,
        "The retained RTI Modern C++ enum is not a standalone topic type");

    require(rti::topic::default_enumerator<Enum>::value == Enum::FIRST,
        "The default enumerator must be FIRST");

    std::ofstream api(output_directory + "/api.txt", std::ios::out | std::ios::trunc);
    require(static_cast<bool>(api), "Could not create the enum API report");
    api << "first=" << static_cast<int>(Enum::FIRST) << '\n';
    api << "second=" << static_cast<int>(Enum::SECOND) << '\n';
    api << "default=" << static_cast<int>(rti::topic::default_enumerator<Enum>::value) << '\n';
    api << "topic-type=" << dds::topic::is_topic_type<Enum>::value << '\n';
    require(static_cast<bool>(api), "Could not finish the enum API report");

    std::ofstream stream(output_directory + "/stream.txt", std::ios::out | std::ios::trunc);
    require(static_cast<bool>(stream), "Could not create the stream operator report");
    stream << "first=[" << to_string(Enum::FIRST) << "]\n";
    stream << "second=[" << to_string(Enum::SECOND) << "]\n";
    require(static_cast<bool>(stream), "Could not finish the stream operator report");
}

void check_dynamic_type(const std::string& output_directory)
{
    using namespace dds::core::xtypes;
    const EnumType& type = rti::topic::dynamic_type<Enum>::get();
    require(type.name() == "CorpusEnumValuePrefix::Annotated", "Unexpected DynamicType name");
    require(type.member_count() == 2, "Expected two enum literals");
    require(type.extensibility_kind() == ExtensibilityKind::EXTENSIBLE,
        "Expected RTI's extensible enum profile");

    const char* names[] = {"FIRST", "SECOND"};
    const std::int32_t ordinals[] = {7, 8};
    std::ofstream metadata(output_directory + "/metadata.txt", std::ios::out | std::ios::trunc);
    require(static_cast<bool>(metadata), "Could not create the metadata report");
    metadata << "type-name=" << type.name() << '\n';
    metadata << "member-count=" << type.member_count() << '\n';
        metadata << "extensibility=" << type.extensibility_kind().underlying() << '\n';
    for (std::size_t index = 0; index < type.member_count(); ++index) {
        const auto& member = type.member(index);
        require(member.name() == names[index], "DynamicType enum member name/order differs from IDL");
        require(member.ordinal() == ordinals[index], "DynamicType enum ordinal differs from IDL");
        metadata << "member=" << index << '|' << member.name() << '|' << member.ordinal() << '\n';
    }
    require(static_cast<bool>(metadata), "Could not finish the metadata report");
}
}

int main(int argc, char** argv)
{
    try {
        if (argc != 3 || std::string(argv[1]) != "contract") {
            throw std::runtime_error("Usage: peer contract <output-directory>");
        }
        check_enum_api(argv[2]);
        check_dynamic_type(argv[2]);
        return 0;
    } catch (const std::exception& error) {
        std::cerr << error.what() << '\n';
        return 1;
    }
}
