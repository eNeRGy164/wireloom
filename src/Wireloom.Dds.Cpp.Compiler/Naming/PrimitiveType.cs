using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

namespace Wireloom.Dds.Cpp.Naming;

/// <summary>Maps IDL scalar types to Modern C++ storage and RTI DynamicType expressions.</summary>
internal sealed record PrimitiveType(string StorageType, string MetadataExpression, string DefaultValue, bool IsOctet = false)
{
    private static readonly Dictionary<string, PrimitiveType> Mappings = new(StringComparer.Ordinal)
        {
            ["short"] = new("::std::int16_t", "::std::int16_t", "0"),
            ["long"] = new("::std::int32_t", "::std::int32_t", "0"),
            ["long long"] = new("::std::int64_t", "::std::int64_t", "0"),
            ["unsigned short"] = new("::std::uint16_t", "::std::uint16_t", "0"),
            ["unsigned long"] = new("::std::uint32_t", "::std::uint32_t", "0"),
            ["unsigned long long"] = new("::std::uint64_t", "::std::uint64_t", "0"),
            ["int8"] = new("::std::int8_t", "::std::int8_t", "0"),
            ["int16"] = new("::std::int16_t", "::std::int16_t", "0"),
            ["int32"] = new("::std::int32_t", "::std::int32_t", "0"),
            ["int64"] = new("::std::int64_t", "::std::int64_t", "0"),
            ["uint8"] = new("::std::uint8_t", "::std::uint8_t", "0"),
            ["uint16"] = new("::std::uint16_t", "::std::uint16_t", "0"),
            ["uint32"] = new("::std::uint32_t", "::std::uint32_t", "0"),
            ["uint64"] = new("::std::uint64_t", "::std::uint64_t", "0"),
            ["octet"] = new("::std::uint8_t", "::rti::core::xtypes::octet_tag_t", "0", IsOctet: true),
            ["boolean"] = new("bool", "bool", "false"),
            ["char"] = new("char", "char", "'\\0'"),
            ["wchar"] = new("wchar_t", "wchar_t", "L'\\0'"),
            ["float"] = new("float", "float", "0.0F"),
            ["double"] = new("double", "double", "0.0"),
            ["long double"] = new("::rti::core::LongDouble", "::rti::core::LongDouble", "::rti::core::LongDouble{}"),
        };

    /// <summary>Resolves a bound IDL primitive or returns false when it is outside this backend's scalar profile.</summary>
    internal static bool TryResolve(IdlType type, out PrimitiveType mapping)
    {
        if (type is IdlType.Primitive primitive && Mappings.TryGetValue(primitive.Name, out var resolved))
        {
            mapping = resolved;
            return true;
        }

        mapping = null!;
        return false;
    }
}
