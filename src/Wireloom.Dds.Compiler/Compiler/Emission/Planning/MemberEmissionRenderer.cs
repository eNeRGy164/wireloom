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
    private bool HasExplicitDefault => facts.ValueMetadata?.HasDefault == true;

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
                var stringType = (StringEmissionType)facts.ValueType;
                return $"Its maximum length is <c>{bound}</c> {StringBoundUnit(stringType.IsWide)}.";
            }

            if (EmissionTypeProjector.HasSequenceType(facts.Type))
            {
                var summary = $"Its maximum number of elements is <c>{bound}</c>.";
                var sequenceType = UnwrapAliasAndOptional(facts.Type);
                if (sequenceType is SequenceEmissionType sequence)
                {
                    var elementConstraints = NestedCollectionConstraintSummary(sequence.Element);
                    if (elementConstraints is not null)
                    {
                        summary += $" {elementConstraints}";
                    }
                }

                return summary;
            }

            return $"Its DDS bound is <c>{bound}</c>.";
        }
    }

    internal static string? NestedCollectionConstraintSummary(EmissionTypePlan type)
    {
        type = UnwrapAliasAndOptional(type);

        if (type is StringEmissionType stringType)
        {
            var stringKind = stringType.IsWide ? "wide" : "narrow";
            var unit = StringBoundUnit(stringType.IsWide);
            return stringType.IsBounded
                ? $"Each {stringKind} IDL string element is limited to <c>{stringType.Bound}</c> {unit}."
                : $"Each unbounded {stringKind} IDL string element has an effective limit of <c>{stringType.Bound}</c> {unit}.";
        }

        if (type is SequenceEmissionType sequence)
        {
            var summary = $"Each nested sequence is limited to <c>{sequence.Bound}</c> elements.";
            var nestedConstraints = NestedCollectionConstraintSummary(sequence.Element);
            return nestedConstraints is null ? summary : $"{summary} {nestedConstraints}";
        }

        if (type is ArrayEmissionType array)
        {
            var summary = $"Each array element has dimensions {string.Join(" × ", array.Dimensions)}.";
            var nestedConstraints = NestedCollectionConstraintSummary(array.Element);
            return nestedConstraints is null ? summary : $"{summary} {nestedConstraints}";
        }

        if (type is PrimitiveEmissionType primitive)
        {
            var mapping = PrimitiveTypeMapping.Resolve(primitive.IdlName);
            if (mapping is { MinimumLiteral: { } minimumLiteral, MaximumLiteral: { } maximumLiteral, AnnotationTypeKind: "Float32" or "Float64" })
            {
                return $"Each element's finite values are in the inclusive range <c>{minimumLiteral}</c> through <c>{maximumLiteral}</c>. NaN and positive or negative infinity are also representable.";
            }

            if (mapping is { MinimumLiteral: { } integerMinimum, MaximumLiteral: { } integerMaximum, AnnotationTypeKind: { } typeKind }
                && IsIntegralAnnotation(typeKind))
            {
                return $"Each element's representable values are in the inclusive range <c>{integerMinimum}</c> through <c>{integerMaximum}</c>.";
            }
        }

        return null;
    }

    internal static string StringBoundUnit(bool isWide) => isWide ? "UTF-16 code units" : "UTF-8 bytes";

    private static EmissionTypePlan UnwrapAliasAndOptional(EmissionTypePlan type) => type switch
    {
        AliasEmissionType alias => UnwrapAliasAndOptional(alias.Target),
        OptionalEmissionType optional => UnwrapAliasAndOptional(optional.Target),
        _ => type
    };

    /// <summary>Builds the documentation summary for value constraints.</summary>
    public string? ValueConstraintSummary
    {
        get
        {
            var constraints = new List<string>();
            var metadata = facts.ValueMetadata;

            if (metadata is { Minimum: { } minimum, Maximum: { } maximum })
            {
                constraints.Add($"Valid values are in the inclusive range <c>{FormatCSharpValue(facts.CSharpType.TrimEnd('?'), minimum)}</c> through <c>{FormatCSharpValue(facts.CSharpType.TrimEnd('?'), maximum)}</c>.");
            }
            else if (metadata?.Minimum is { } lower)
            {
                constraints.Add($"Valid values are at least <c>{FormatCSharpValue(facts.CSharpType.TrimEnd('?'), lower)}</c>.");
            }
            else if (metadata?.Maximum is { } upper)
            {
                constraints.Add($"Valid values are no greater than <c>{FormatCSharpValue(facts.CSharpType.TrimEnd('?'), upper)}</c>.");
            }
            else if (facts.ValueType is PrimitiveEmissionType primitive)
            {
                var mapping = PrimitiveTypeMapping.Resolve(primitive.IdlName);
                if (mapping is { MinimumLiteral: { } minimumLiteral, MaximumLiteral: { } maximumLiteral, AnnotationTypeKind: "Float32" or "Float64" })
                {
                    constraints.Add($"Finite values are in the inclusive range <c>{minimumLiteral}</c> through <c>{maximumLiteral}</c>. NaN and positive or negative infinity are also representable.");
                }
                else if (mapping is { MinimumLiteral: { } integerMinimum, MaximumLiteral: { } integerMaximum, AnnotationTypeKind: { } typeKind }
                    && IsIntegralAnnotation(typeKind))
                {
                    constraints.Add($"Representable values are in the inclusive range <c>{integerMinimum}</c> through <c>{integerMaximum}</c>.");
                }
            }

            if (metadata?.HasDefault == true)
            {
                var defaultText = metadata.DefaultExpression ?? ManagedDefaultValue;
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
                var enumTypeName = enumType.SupportType ?? facts.CSharpType;
                var enumReference = ResolvedTypeReference(enumTypeName, currentNamespace: null);
                if (!enumReference.StartsWith("global::", StringComparison.Ordinal))
                {
                    enumReference = $"global::{enumReference}";
                }

                return $" = {enumReference}.{EscapeIdentifier(enumType.DefaultMemberName)};";
            }

            if (facts.IsSequence || facts.IsArray)
            {
                return facts.IsOptional ? null : " = null!;";
            }

            if (facts is { IsAggregate: true, IsOptional: false })
            {
                return " = new();";
            }

            return null;
        }
    }

    /// <summary>Builds the managed default initialization statement.</summary>
    public string? ManagedDefaultInitializationStatement(ManagedInitializationKind initialization) =>
        ManagedDefaultInitializationStatement(initialization, EscapedName);

    /// <summary>Builds the managed default initialization statement for a supplied target name.</summary>
    public string? ManagedDefaultInitializationStatement(ManagedInitializationKind initialization, string targetName)
    {
        if (HasExplicitDefault)
        {
            return $"{targetName} = {ManagedDefaultValue};";
        }

        return initialization switch
        {
            ManagedInitializationKind.Sequence => $"{targetName} = new Sequence<{TypeReference(facts.ElementCSharpType!, currentNamespace)}>();",
            ManagedInitializationKind.Array => $"{targetName} = new {TypeReference(facts.ElementCSharpType!, currentNamespace)}[{string.Join(", ", facts.Dimensions)}];",
            ManagedInitializationKind.Aggregate => $"{targetName} = new {TypeReference(facts.CSharpType, currentNamespace)}();",
            _ => null
        };
    }

    /// <summary>Builds the managed default value expression.</summary>
    public string ManagedDefaultValue => ScalarValue.ManagedDefaultLiteral!;

    private ScalarValueEmissionPlan ScalarValue { get; } = new(facts.ValueType, facts.ValueMetadata);

    /// <summary>Formats an IDL constant as a C# literal.</summary>
    public string FormatCSharpValue(string typeName, BigInteger value) => CSharpScalarValueFormatter.Format(typeName, value);

    /// <summary>Builds the annotation metadata for a supported primitive member.</summary>
    public (string TypeKind, string ValueProperty, string DefaultValue, string? Minimum, string? Maximum, string? Unit)? PrimitiveAnnotation() =>
        ScalarValue.Annotation;

    private static bool IsIntegralAnnotation(string typeKind) => typeKind is
        "Int16" or "Int32" or "Int64" or "Uint16" or "UInt32" or "UInt64" or "Int8" or "Uint8" or "Octet";
}
