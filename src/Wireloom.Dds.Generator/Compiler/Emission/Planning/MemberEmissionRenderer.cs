using System.Globalization;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Naming;

using static Wireloom.Compiler.Naming.IdlNaming;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Renders pure member-level summaries, defaults, and annotations.</summary>
internal sealed class MemberEmissionRenderer(MemberEmissionFacts facts, string? currentNamespace)
{
    /// <summary>Gets the member's escaped source name.</summary>
    private string EscapedName => EscapeIdentifier(facts.Name);

    /// <summary>Gets whether the member has an explicit default value.</summary>
    private bool HasExplicitDefault => facts.ValueMetadata?.DefaultValue is not null;

    /// <summary>Builds the documentation summary for a resolved bound.</summary>
    public string? BoundSummary
    {
        get
        {
            if (facts.Bound is not int bound)
            {
                return null;
            }

            if (facts.IsString)
            {
                return $"Its maximum length is <c>{bound}</c>.";
            }

            if (EmissionTypeProjector.HasSequenceType(facts.Type))
            {
                return $"Its maximum number of elements is <c>{bound}</c>.";
            }

            return $"Its DDS bound is <c>{bound}</c>.";
        }
    }

    /// <summary>Builds the documentation summary for value constraints.</summary>
    public string? ValueConstraintSummary
    {
        get
        {
            var constraints = new List<string>();
            var metadata = facts.ValueMetadata;

            if (metadata is { Minimum: { } minimum, Maximum: { } maximum })
            {
                constraints.Add($"Its value must be between <c>{FormatCSharpValue(facts.CSharpType.TrimEnd('?'), minimum)}</c> and <c>{FormatCSharpValue(facts.CSharpType.TrimEnd('?'), maximum)}</c>.");
            }
            else if (metadata?.Minimum is { } lower)
            {
                constraints.Add($"Its minimum value is <c>{FormatCSharpValue(facts.CSharpType.TrimEnd('?'), lower)}</c>.");
            }
            else if (metadata?.Maximum is { } upper)
            {
                constraints.Add($"Its maximum value is <c>{FormatCSharpValue(facts.CSharpType.TrimEnd('?'), upper)}</c>.");
            }

            if (metadata?.DefaultValue is { } defaultValue)
            {
                var defaultText = metadata.DefaultExpression ?? FormatCSharpValue(facts.CSharpType.TrimEnd('?'), defaultValue);
                constraints.Add($"Its default value is <c>{System.Security.SecurityElement.Escape(defaultText)}</c>.");
            }

            return constraints.Count == 0 ? null : string.Join(" ", constraints);
        }
    }

    /// <summary>Builds the managed property initializer for this member.</summary>
    public string? ManagedPropertyInitializer
    {
        get
        {
            if (HasExplicitDefault)
            {
                return null;
            }

            if (facts.CSharpType == "string")
            {
                return " = string.Empty;";
            }

            if (facts.Type.IsEnum && !facts.IsOptional)
            {
                var enumType = (EnumEmissionType)facts.ValueType;
                return $" = {TypeReference(facts.CSharpType, currentNamespace)}.{EscapeIdentifier(enumType.DefaultMemberName)};";
            }

            if (facts.IsSequence || facts.IsArray)
            {
                return facts.IsOptional ? null : " = null!;";
            }

            if (facts.IsAggregate)
            {
                return $" = new {TypeReference(facts.CSharpType, currentNamespace)}();";
            }

            return null;
        }
    }

    /// <summary>Builds the managed default initialization statement.</summary>
    public string? ManagedDefaultInitializationStatement(ManagedInitializationKind initialization)
    {
        if (HasExplicitDefault)
        {
            return $"{EscapedName} = {ManagedDefaultValue};";
        }

        return initialization switch
        {
            ManagedInitializationKind.Sequence => $"{EscapedName} = new Sequence<{TypeReference(facts.ElementCSharpType!, currentNamespace)}>();",
            ManagedInitializationKind.Array => $"{EscapedName} = new {TypeReference(facts.ElementCSharpType!, currentNamespace)}[{string.Join(", ", facts.Dimensions)}];",
            ManagedInitializationKind.Aggregate => $"{EscapedName} = new {TypeReference(facts.CSharpType, currentNamespace)}();",
            _ => null
        };
    }

    /// <summary>Builds the managed default value expression.</summary>
    public string ManagedDefaultValue => facts.ValueType switch
    {
        EnumEmissionType => $"({TypeReference(facts.CSharpType, currentNamespace)})({facts.ValueMetadata!.DefaultValue!.Value.ToString(CultureInfo.InvariantCulture)})",
        _ => FormatCSharpValue(facts.CSharpType.TrimEnd('?'), facts.ValueMetadata!.DefaultValue!.Value)
    };

    /// <summary>Formats an IDL constant as a C# literal.</summary>
    public string FormatCSharpValue(string typeName, BigInteger value) => typeName switch
    {
        "long" when value == long.MinValue => "long.MinValue",
        "long" => $"{value.ToString(CultureInfo.InvariantCulture)}L",
        "ulong" when value == ulong.MaxValue => "ulong.MaxValue",
        "ulong" => $"{value.ToString(CultureInfo.InvariantCulture)}UL",
        "uint" => $"{value.ToString(CultureInfo.InvariantCulture)}U",
        _ => value.ToString(CultureInfo.InvariantCulture)
    };

    /// <summary>Builds the annotation metadata for a supported primitive member.</summary>
    public (string TypeKind, string ValueProperty, string DefaultValue, string? Minimum, string? Maximum, string? Unit)? PrimitiveAnnotation()
    {
        switch (facts.ValueType)
        {
            case StringEmissionType { IsWide: true }:
                return ("WideString", "WideStringValue", "\"\"", null, null, null);
            case StringEmissionType:
                return ("String", "StringValue", "\"\"", null, null, null);
            case EnumEmissionType enumType:
                var enumDefault = enumType.DefaultValue.ToString(CultureInfo.InvariantCulture);
                if (HasExplicitDefault)
                {
                    enumDefault = facts.ValueMetadata!.DefaultValue!.Value.ToString(CultureInfo.InvariantCulture);
                }

                return ("Enumeration", "EnumValue", enumDefault, null, null, UnitLiteral);
            case PrimitiveEmissionType primitive:
                return PrimitiveAnnotation(primitive);
            default:
                return null;
        }
    }

    private (string TypeKind, string ValueProperty, string DefaultValue, string? Minimum, string? Maximum, string? Unit)? PrimitiveAnnotation(PrimitiveEmissionType primitive)
    {
        var mapping = PrimitiveTypeMapping.Resolve(primitive.IdlName);
        if (mapping.AnnotationTypeKind is null || mapping.AnnotationValueProperty is null)
        {
            return null;
        }

        var defaultValue = mapping.NativeDefaultLiteral;
        if (mapping.ManagedType != mapping.NativeStorageType)
        {
            defaultValue = mapping.AnnotationDefaultLiteral;
        }

        if (HasExplicitDefault && IsIntegralAnnotation(mapping.AnnotationTypeKind))
        {
            defaultValue = FormatCSharpValue(mapping.ManagedType, facts.ValueMetadata!.DefaultValue!.Value);
        }

        var minimum = mapping.MinimumLiteral;
        if (facts.ValueMetadata?.Minimum is { } minimumValue)
        {
            minimum = FormatCSharpValue(mapping.ManagedType, minimumValue);
        }

        var maximum = mapping.MaximumLiteral;
        if (facts.ValueMetadata?.Maximum is { } maximumValue)
        {
            maximum = FormatCSharpValue(mapping.ManagedType, maximumValue);
        }

        return (mapping.AnnotationTypeKind, mapping.AnnotationValueProperty, defaultValue, minimum, maximum, UnitLiteral);
    }

    private string? UnitLiteral => facts.ValueMetadata?.Unit is null
        ? null
        : $"\"{facts.ValueMetadata.Unit.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";

    private static bool IsIntegralAnnotation(string typeKind)
    {
        switch (typeKind)
        {
            case "Int16":
            case "Int32":
            case "Int64":
            case "Uint16":
            case "UInt32":
            case "UInt64":
            case "Int8":
            case "Uint8":
            case "Octet":
                return true;
            default:
                return false;
        }
    }
}
