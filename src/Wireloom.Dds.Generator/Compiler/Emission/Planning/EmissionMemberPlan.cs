using System.Globalization;
using JetBrains.Annotations;
using Wireloom.Compiler.Emission.Emitters;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Writers;
using Wireloom.Compiler.Naming;

using static Wireloom.Compiler.Naming.IdlNaming;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>
/// Per-member emission decisions shared by managed and native source emitters.
/// The plan retains the typed emission type and builds context-sensitive source
/// operations on demand, so names, namespaces, keysOnly, and recursive type
/// support are not frozen into reusable source fragments.
/// </summary>
[PublicAPI]
internal sealed partial class MemberEmissionPlan(IdlEmissionField field, string? currentNamespace, string? managedBackingFieldName = null)
{
    private readonly MemberEmissionFacts facts = new(field, currentNamespace);
    private readonly EmissionShape shape = field.Type.Shape;

    public IdlEmissionField Field { get; } = field;
    public string? CurrentNamespace => facts.CurrentNamespace;
    public string Name => facts.Name;
    public string EscapedName => EscapeIdentifier(Name);
    public EmissionTypePlan Type => facts.Type;
    public bool IsKey => facts.IsKey;
    public int? MemberId => facts.MemberId;
    public bool IsOptional => facts.IsOptional;
    public string CSharpType => facts.CSharpType;
    public int? Bound => facts.Bound;
    public string? SupportType => facts.SupportType;
    public EmissionTypePlan? ElementType => facts.ElementType;
    public string? ElementCSharpType => facts.ElementCSharpType;
    public string? ElementSupportType => facts.ElementSupportType;
    public IReadOnlyList<int> Dimensions => facts.Dimensions;
    public BigInteger? DefaultValue => facts.ValueMetadata?.DefaultValue;
    public BigInteger? MinimumValue => facts.ValueMetadata?.Minimum;
    public BigInteger? MaximumValue => facts.ValueMetadata?.Maximum;
    public string? DefaultExpression => facts.ValueMetadata?.DefaultExpression;
    public string? Unit => facts.ValueMetadata?.Unit;
    public bool IsExternal => facts.IsExternal;
    public bool IsMustUnderstand => facts.IsMustUnderstand;
    public string? MemberIdHashSource => facts.MemberIdHashSource;
    public bool UsesAutoIdHash => facts.UsesAutoIdHash;
    public bool HasExplicitDefault => DefaultValue is not null;
    public bool HasManagedRange => MinimumValue is not null || MaximumValue is not null;
    public string ManagedBackingFieldName => managedBackingFieldName ?? EscapeIdentifier("_" + Name);
    public bool IsString => facts.IsString;
    public bool IsSequence => facts.IsSequence;
    public bool IsArray => facts.IsArray;
    public bool IsSequenceArray => IsSequence && Dimensions.Count > 0;
    public bool IsArrayLoopLocalCollision
    {
        get
        {
            if (!IsArray || !Name.StartsWith("dimension", StringComparison.Ordinal))
            {
                return false;
            }

            var suffix = Name["dimension".Length..];
            if (suffix.Length == 0)
            {
                return Dimensions.Count > 0;
            }

            return int.TryParse(suffix, out var dimension)
                && dimension >= 0
                && dimension < Dimensions.Count;
        }
    }
    public bool IsStringSequence => facts.IsStringSequence;
    public bool IsAggregate => facts.IsAggregate;
    public bool IsUnion => facts.IsUnion;
    public bool HasAggregateElement => facts.HasAggregateElement;
    public bool HasSequenceElement => IsArray && ElementType is not null && EmissionTypeProjector.HasSequenceType(ElementType);
    public string? BoundSummary
    {
        get
        {
            if (Bound is not int bound)
            {
                return null;
            }

            if (IsString)
            {
                return $"Its maximum length is <c>{bound}</c>.";
            }

            if (EmissionTypeProjector.HasSequenceType(Type))
            {
                return $"Its maximum number of elements is <c>{bound}</c>.";
            }

            return $"Its DDS bound is <c>{bound}</c>.";
        }
    }

    public string? ValueConstraintSummary
    {
        get
        {
            var constraints = new List<string>();

            if (MinimumValue is { } minimum && MaximumValue is { } maximum)
            {
                constraints.Add($"Its value must be between <c>{FormatCSharpValue(CSharpType.TrimEnd('?'), minimum)}</c> and <c>{FormatCSharpValue(CSharpType.TrimEnd('?'), maximum)}</c>.");
            }
            else if (MinimumValue is { } lower)
            {
                constraints.Add($"Its minimum value is <c>{FormatCSharpValue(CSharpType.TrimEnd('?'), lower)}</c>.");
            }
            else if (MaximumValue is { } upper)
            {
                constraints.Add($"Its maximum value is <c>{FormatCSharpValue(CSharpType.TrimEnd('?'), upper)}</c>.");
            }

            if (DefaultValue is not null)
            {
                var defaultText = DefaultExpression ?? FormatCSharpValue(CSharpType.TrimEnd('?'), DefaultValue.Value);
                constraints.Add($"Its default value is <c>{System.Security.SecurityElement.Escape(defaultText)}</c>.");
            }

            return constraints.Count == 0 ? null : string.Join(" ", constraints);
        }
    }

    public ManagedInitializationKind ManagedInitialization => MemberEmissionPolicies.GetManagedInitialization(
        shape.Kind,
        IsOptional,
        IsSequence,
        IsArray,
        IsAggregate);

    public NativeDestroyKind DestroyKind => MemberEmissionPolicies.GetDestroyKind(
        IsAggregate,
        IsSequence,
        IsArray,
        IsString,
        IsOptionalScalar);

    public bool HasTypeSupport => Bound is null || EmissionTypeProjector.HasSequenceType(Type) || IsString;

    /// <summary>Determines whether this member recursively refers to the containing runtime type.</summary>
    public bool IsRecursive(string runtimeTypeName) =>
        IsSequence && string.Equals(ElementCSharpType, runtimeTypeName, StringComparison.Ordinal);

    public string ManagedPropertyAccessors => IsSequence && !IsOptional ? " { get; }" : " { get; set; }";
    public string ManagedPropertySummaryVerb => IsSequence && !IsOptional ? "Gets" : "Gets or sets";

    public string? ManagedPropertyInitializer
    {
        get
        {
            if (HasExplicitDefault)
            {
                return null;
            }

            if (CSharpType == "string")
            {
                return " = string.Empty;";
            }

            if (Type.IsEnum)
            {
                return $" = ({TypeReference(CSharpType, currentNamespace)}){EnumDefaultValue};";
            }

            if (IsSequence || IsArray)
            {
                return IsOptional ? null : " = null!;";
            }

            if (IsAggregate)
            {
                return $" = new {TypeReference(CSharpType, currentNamespace)}();";
            }

            return null;
        }
    }

    public string? ManagedDefaultInitializationStatement => ManagedInitialization switch
    {
        _ when HasExplicitDefault => $"{EscapedName} = {ManagedDefaultValue};",
        ManagedInitializationKind.Sequence => $"{EscapedName} = new Sequence<{TypeReference(ElementCSharpType!, currentNamespace)}>();",
        ManagedInitializationKind.Array => $"{EscapedName} = new {TypeReference(ElementCSharpType!, currentNamespace)}[{string.Join(", ", Dimensions)}];",
        ManagedInitializationKind.Aggregate => $"{EscapedName} = new {TypeReference(CSharpType, currentNamespace)}();",
        _ => null
    };

    public string ManagedDefaultValue => ValueType switch
    {
        EnumEmissionType => $"({TypeReference(CSharpType, currentNamespace)}){DefaultValue!.Value.ToString(CultureInfo.InvariantCulture)}",
        _ => FormatCSharpValue(CSharpType.TrimEnd('?'), DefaultValue!.Value)
    };

    /// <summary>Formats an IDL constant value as a C# literal for the requested type.</summary>
    public string FormatCSharpValue(string typeName, BigInteger value) => typeName switch
    {
        "long" when value == long.MinValue => "long.MinValue",
        "long" => $"{value.ToString(CultureInfo.InvariantCulture)}L",
        "ulong" when value == ulong.MaxValue => "ulong.MaxValue",
        "ulong" => $"{value.ToString(CultureInfo.InvariantCulture)}UL",
        "uint" => $"{value.ToString(CultureInfo.InvariantCulture)}U",
        "short" => $"(short){value.ToString(CultureInfo.InvariantCulture)}",
        "ushort" => $"(ushort){value.ToString(CultureInfo.InvariantCulture)}",
        "sbyte" => $"(sbyte){value.ToString(CultureInfo.InvariantCulture)}",
        "byte" => $"(byte){value.ToString(CultureInfo.InvariantCulture)}",
        _ => value.ToString(CultureInfo.InvariantCulture)
    };


    /// <summary>Resolves the native storage type for this member.</summary>
    public string NativeStorageTypeFor(string? namespaceOverride)
    {
        var namespaceName = namespaceOverride ?? currentNamespace;

        switch (shape.Kind)
        {
            case EmissionShapeKind.Sequence:
                if (IsStringSequence && IsSequenceArray)
                {
                    return "NativeStringSeq";
                }

                return IsOptional ? "NativeOptionalSeq" : "NativeSeq";
            case EmissionShapeKind.Array:
                if (IsOptional)
                {
                    return HasAggregateElement ? "NativeManagedOptionalArray" : "NativeUnmanagedOptionalArray";
                }

                return HasAggregateElement ? "NativeManagedArray" : "NativeUnmanagedArray";
        }

        if (IsOptionalScalar)
        {
            return "NativeUnmanagedOptional";
        }

        if (IsAggregate)
        {
            return GetReferencedUnmanagedType(namespaceName);
        }

        return BuildScalarNativeStorageType(namespaceName);
    }

    private string BuildScalarNativeStorageType(string? namespaceName)
    {
        if (ValueType is PrimitiveEmissionType primitive)
        {
            return PrimitiveTypeMapping.Resolve(primitive.IdlName).NativeStorageType;
        }

        if (ValueType is StringEmissionType stringType)
        {
            return stringType.IsWide ? "NativeWstring" : "NativeString";
        }

        return TypeReference(CSharpType, namespaceName);
    }


    /// <summary>Builds the managed copy expression for this member.</summary>
    public string BuildCopyExpression(string sourcePrefix = "other.")
    {
        var source = sourcePrefix + EscapedName;

        if (IsSequence)
        {
            var elementType = TypeReference(ElementCSharpType!, currentNamespace);
            var copiedSequence = HasAggregateElement
                ? $"global::System.Linq.Enumerable.Select({source}, element => new {elementType}(element))"
                : source;
            if (IsOptional)
            {
                return $"{source} is null ? null! : new Sequence<{elementType}>({copiedSequence})";
            }

            return $"new Sequence<{elementType}>({copiedSequence})";
        }

        if (IsArray)
        {
            var copy = $"({TypeReference(CSharpType.TrimEnd('?'), currentNamespace)}){source}.Clone()";
            return IsOptional ? $"{source} is null ? null! : {copy}" : copy;
        }

        if (IsAggregate)
        {
            return $"new {TypeReference(CSharpType, currentNamespace)}({source})";
        }

        return source;
    }

    /// <summary>Builds the managed union copy expression for this member.</summary>
    public string BuildUnionCopyExpression(string sourcePrefix = "other.")
    {
        var source = sourcePrefix + EscapedName;

        if (IsSequence)
        {
            return $"new Sequence<{TypeReference(ElementCSharpType!, currentNamespace)}>({source})";
        }

        if (IsAggregate)
        {
            return $"new {TypeReference(CSharpType, currentNamespace)}({source})";
        }

        return source;
    }

    /// <summary>Emits initialization code for an aggregate array member.</summary>
    public void EmitAggregateArrayInitialization(GeneratedSourceWriter writer, string target, bool hasFollowingStatements)
    {
        if (!IsArray || !HasAggregateElement)
        {
            return;
        }

        writer.BlankLine();
        var indices = ArraySourceEmitter.OpenLoops(writer, Dimensions);
        writer.WriteLine($"{target}{ArraySourceEmitter.IndexExpression(indices)} = new {TypeReference(ElementCSharpType!, currentNamespace)}();");

        ArraySourceEmitter.CloseLoops(writer, indices.Count);

        if (hasFollowingStatements)
        {
            writer.BlankLine();
        }
    }

    /// <summary>Emits copy code for an aggregate array member.</summary>
    public void EmitAggregateArrayCopy(GeneratedSourceWriter writer, string target, bool hasFollowingStatements)
    {
        if (!IsArray || !HasAggregateElement)
        {
            return;
        }

        writer.BlankLine();

        if (IsOptional)
        {
            writer.OpenBlock($"if (other.{EscapedName} is not null)");
        }

        var indices = ArraySourceEmitter.OpenLoops(writer, Dimensions);
        writer.WriteLine($"{target}{ArraySourceEmitter.IndexExpression(indices)} = new {TypeReference(ElementCSharpType!, currentNamespace)}(other.{EscapedName}{ArraySourceEmitter.IndexExpression(indices)});");

        ArraySourceEmitter.CloseLoops(writer, indices.Count);

        if (IsOptional)
        {
            writer.CloseBlock();
        }

        if (hasFollowingStatements)
        {
            writer.BlankLine();
        }
    }

    /// <summary>Builds the hash expression for this member.</summary>
    public string HashValue(string targetPrefix = "")
    {
        if (IsOptional && IsSequence)
        {
            return $"{targetPrefix}{EscapedName}?.Count ?? -1";
        }

        if (IsOptional && IsArray)
        {
            return $"{targetPrefix}{EscapedName} is null ? -1 : {targetPrefix}{EscapedName}{ArraySourceEmitter.IndexExpression(ZeroIndices())}";
        }

        var suffix = shape.Kind switch
        {
            EmissionShapeKind.Array => ArraySourceEmitter.IndexExpression(ZeroIndices()),
            EmissionShapeKind.Sequence => ".Count",
            _ => string.Empty
        };

        return targetPrefix + EscapedName + suffix;
    }

    private IReadOnlyList<string> ZeroIndices() => Enumerable.Repeat("0", Dimensions.Count).ToArray();

    /// <summary>Builds the equality expression for this member.</summary>
    public string EqualityExpression(string otherPrefix = "other.", string thisPrefix = "")
    {
        if (IsArray)
        {
            string arrayEquality;
            if (Dimensions.Count == 1)
            {
                arrayEquality = $"global::System.Linq.Enumerable.SequenceEqual({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName})";
            }
            else
            {
                arrayEquality = $"{thisPrefix}{EscapedName}.Rank == {otherPrefix}{EscapedName}.Rank && global::System.Linq.Enumerable.All(global::System.Linq.Enumerable.Range(0, {thisPrefix}{EscapedName}.Rank), dimension => {thisPrefix}{EscapedName}.GetLength(dimension) == {otherPrefix}{EscapedName}.GetLength(dimension)) && global::System.Linq.Enumerable.SequenceEqual(global::System.Linq.Enumerable.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>({thisPrefix}{EscapedName}), global::System.Linq.Enumerable.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>({otherPrefix}{EscapedName}))";
            }

            if (IsOptional)
            {
                return $"(global::System.Object.ReferenceEquals({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName}) || ({thisPrefix}{EscapedName} is not null && {otherPrefix}{EscapedName} is not null && {arrayEquality}))";
            }

            return arrayEquality;
        }

        if (IsOptional && IsSequence)
        {
            return $"(global::System.Object.ReferenceEquals({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName}) || ({thisPrefix}{EscapedName} is not null && {otherPrefix}{EscapedName} is not null && global::System.Linq.Enumerable.SequenceEqual({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName})))";
        }

        if (IsArray && HasAggregateElement)
        {
            return $"global::System.Linq.Enumerable.SequenceEqual(global::System.Linq.Enumerable.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>({thisPrefix}{EscapedName}), global::System.Linq.Enumerable.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>({otherPrefix}{EscapedName}))";
        }

        if (IsArray || IsSequence)
        {
            return $"global::System.Linq.Enumerable.SequenceEqual({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName})";
        }

        if (IsOptional)
        {
            return $"Equals({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName})";
        }

        return $"{thisPrefix}{EscapedName}.Equals({otherPrefix}{EscapedName})";
    }

    /// <summary>Builds the annotation metadata for a supported primitive member.</summary>
    public (string TypeKind, string ValueProperty, string DefaultValue, string? Minimum, string? Maximum, string? Unit)? PrimitiveAnnotation()
    {
        switch (ValueType)
        {
            case StringEmissionType { IsWide: true }:
                return ("WideString", "WideStringValue", "\"\"", null, null, null);
            case StringEmissionType:
                return ("String", "StringValue", "\"\"", null, null, null);
            case EnumEmissionType enumType:
                var enumDefault = enumType.DefaultValue.ToString(CultureInfo.InvariantCulture);
                if (HasExplicitDefault)
                {
                    enumDefault = DefaultValue!.Value.ToString(CultureInfo.InvariantCulture);
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
        if (HasExplicitDefault && IsIntegralAnnotation(mapping.AnnotationTypeKind))
        {
            defaultValue = FormatCSharpValue(mapping.ManagedType, DefaultValue!.Value);
        }

        var minimum = mapping.MinimumLiteral;
        if (MinimumValue is not null)
        {
            minimum = FormatCSharpValue(mapping.ManagedType, MinimumValue.Value);
        }

        var maximum = mapping.MaximumLiteral;
        if (MaximumValue is not null)
        {
            maximum = FormatCSharpValue(mapping.ManagedType, MaximumValue.Value);
        }

        return (mapping.AnnotationTypeKind, mapping.AnnotationValueProperty, defaultValue, minimum, maximum, UnitLiteral);
    }

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

    private string? UnitLiteral => Unit is null
        ? null
        : $"\"{Unit.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";

    private bool IsOptionalScalar => IsOptional && !IsSequence && !IsArray && !IsString && !IsAggregate;

    private EmissionTypePlan ValueType => EmissionTypeProjector.UnwrapValueEmissionType(Type);

    private int EnumDefaultValue => ValueType is EnumEmissionType enumType ? enumType.DefaultValue : 0;

    private string GetReferencedUnmanagedType(string? namespaceOverride) =>
        EmissionSupport.GetUnmanagedType(CSharpType, namespaceOverride);

    private string ElementUnmanagedType(string? namespaceOverride = null)
    {
        var typeName = ElementSupportType ?? ElementCSharpType!;

        return EmissionSupport.GetUnmanagedType(typeName, namespaceOverride ?? currentNamespace);
    }

    private string NullableValueType() => CSharpType.TrimEnd('?');

}
