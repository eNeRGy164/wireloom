using Wireloom.Dds.Cpp.Naming;

namespace Wireloom.Dds.Cpp.Emission.Model;

/// <summary>Represents a validated member storage type and its default expression.</summary>
internal abstract record MemberTypePlan(string StorageType, string DefaultValue)
{
    /// <summary>Gets whether this member contains a sequence at any collection depth.</summary>
    internal virtual bool ContainsSequence => false;

    /// <summary>Gets whether this member contains a fixed array at any collection depth.</summary>
    internal virtual bool ContainsArray => false;

    /// <summary>Gets whether this member contains a string at any collection depth.</summary>
    internal virtual bool ContainsString => false;

    /// <summary>Gets whether this member contains a wide string at any collection depth.</summary>
    internal virtual bool ContainsWideString => false;

    /// <summary>Gets whether this member requires collection lifecycle support.</summary>
    internal bool ContainsCollection => ContainsSequence || ContainsArray;

    /// <summary>Gets the value-constructor parameter type used by the RTI-generated C++ profile.</summary>
    internal string ConstructorParameterType => this switch
    {
        Primitive { Mapping.StorageType: "::rti::core::LongDouble" } => $"const {StorageType}&",
        Primitive => StorageType,
        _ => $"const {StorageType}&",
    };

    /// <summary>Represents a scalar IDL primitive.</summary>
    internal sealed record Primitive : MemberTypePlan
    {
        internal Primitive(PrimitiveType mapping, string initializer)
            : base(mapping.StorageType, initializer) => Mapping = mapping;

        internal PrimitiveType Mapping { get; }
    }

    /// <summary>Represents an IDL string stored using the Modern C++ string type.</summary>
    internal sealed record String(int MetadataBound, bool IsWide)
        : MemberTypePlan(IsWide ? "::std::wstring" : "::std::string", "{}")
    {
        internal override bool ContainsString => true;

        internal override bool ContainsWideString => IsWide;
    }

    /// <summary>Represents an IDL enum that supplies an RTI DynamicType.</summary>
    internal sealed record Enumeration : MemberTypePlan
    {
        internal Enumeration(string qualifiedName, string defaultMember)
            : base($"::{qualifiedName}", $"::{qualifiedName}::{defaultMember}") => QualifiedName = qualifiedName;

        internal string QualifiedName { get; }
    }

    /// <summary>Represents an IDL typedef storage spelling with metadata resolved through its target.</summary>
    internal sealed record Alias(MemberTypePlan Target, string AliasStorageType)
        : MemberTypePlan(AliasStorageType, Target.DefaultValue)
    {
        internal override bool ContainsSequence => Target.ContainsSequence;

        internal override bool ContainsArray => Target.ContainsArray;

        internal override bool ContainsString => Target.ContainsString;

        internal override bool ContainsWideString => Target.ContainsWideString;
    }

    /// <summary>Represents an IDL struct stored inline as a nested aggregate.</summary>
    internal sealed record Struct : MemberTypePlan
    {
        internal Struct(string qualifiedName, string? storageTypeName = null)
            : base(storageTypeName ?? $"::{qualifiedName}", "{}") => QualifiedName = qualifiedName;

        internal string QualifiedName { get; }
    }

    /// <summary>Represents a canonical IDL union stored inline as a value.</summary>
    internal sealed record Union : MemberTypePlan
    {
        internal Union(string qualifiedName, string? storageTypeName = null)
            : base(storageTypeName ?? $"::{qualifiedName}", "{}") => QualifiedName = qualifiedName;

        internal string QualifiedName { get; }
    }

    /// <summary>Represents a bounded or unbounded IDL sequence using RTI's C++ sequence type.</summary>
    internal sealed record Sequence(MemberTypePlan ElementType, int Bound, bool IsUnbounded = false)
        : MemberTypePlan(
            IsUnbounded
                ? $"::omg::types::sequence< {ElementType.StorageType} >"
                : $"::omg::types::bounded_sequence< {ElementType.StorageType}, {Bound} >",
            "{}")
    {
        internal override bool ContainsSequence => true;

        internal override bool ContainsArray => ElementType.ContainsArray;

        internal override bool ContainsString => ElementType.ContainsString;

        internal override bool ContainsWideString => ElementType.ContainsWideString;
    }

    /// <summary>Represents a fixed IDL array using RTI's C++ array alias.</summary>
    internal sealed record Array(MemberTypePlan ElementType, IReadOnlyList<int> Dimensions)
        : MemberTypePlan(
            FormatArrayStorageType(ElementType.StorageType, Dimensions),
            FormatArrayDefaultValue(ElementType, Dimensions))
    {
        internal override bool ContainsSequence => ElementType.ContainsSequence;

        internal override bool ContainsArray => true;

        internal override bool ContainsString => ElementType.ContainsString;

        internal override bool ContainsWideString => ElementType.ContainsWideString;
    }

    private static string FormatArrayStorageType(string elementStorageType, IReadOnlyList<int> dimensions)
    {
        var storageType = elementStorageType;
        for (var index = dimensions.Count - 1; index >= 0; index--)
        {
            storageType = $"::dds::core::array< {storageType}, {dimensions[index]} >";
        }

        return storageType;
    }

    private static string FormatArrayDefaultValue(MemberTypePlan elementType, IReadOnlyList<int> dimensions)
    {
        if (!IsEnumeration(elementType))
        {
            return "{}";
        }

        return FormatArrayDimension(elementType.DefaultValue, dimensions, 0, 1);
    }

    private static bool IsEnumeration(MemberTypePlan type) => type switch
    {
        Enumeration => true,
        Alias alias => IsEnumeration(alias.Target),
        _ => false,
    };

    private static string FormatArrayDimension(
        string elementDefaultValue,
        IReadOnlyList<int> dimensions,
        int dimensionIndex,
        int indentationLevel)
    {
        var indentation = new string(' ', indentationLevel * 4);
        var childIndentation = new string(' ', (indentationLevel + 1) * 4);
        var values = Enumerable.Range(0, dimensions[dimensionIndex])
            .Select(_ => dimensionIndex == dimensions.Count - 1
                ? childIndentation + elementDefaultValue
                : childIndentation + FormatArrayDimension(
                    elementDefaultValue,
                    dimensions,
                    dimensionIndex + 1,
                    indentationLevel + 1));
        return "{\n" + string.Join(",\n", values) + "\n" + indentation + "}";
    }
}
