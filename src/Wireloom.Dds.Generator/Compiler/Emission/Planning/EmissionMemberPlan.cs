using System.Globalization;
using JetBrains.Annotations;
using Wireloom.Compiler.Emission.Emitters;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Writers;

using static Wireloom.Compiler.Naming.IdlNaming;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Classifies the projected shape of a member value.</summary>
internal enum FieldEmissionShape
{
    Primitive,
    String,
    Enum,
    Struct,
    Alias,
    Sequence,
    Array
}

/// <summary>Identifies the managed initialization strategy for a member.</summary>
internal enum ManagedInitializationKind
{
    None,
    Sequence,
    Array,
    Aggregate
}

/// <summary>Identifies the native cleanup strategy for a member.</summary>
internal enum NativeDestroyKind
{
    None,
    Nested,
    Collection,
    String,
    OptionalPrimitive
}

/// <summary>
/// Per-member emission decisions shared by managed and native source emitters.
/// The plan retains the typed emission type and builds context-sensitive source
/// operations on demand, so names, namespaces, keysOnly, and recursive type
/// support are not frozen into reusable source fragments.
/// </summary>
[PublicAPI]
internal sealed partial class MemberEmissionPlan(IdlEmissionField field, string? currentNamespace)
{
    private readonly FieldEmissionShape shape = GetShape(field.Type);

    public IdlEmissionField Field { get; } = field;
    public string? CurrentNamespace => currentNamespace;
    public string Name => Field.Name;
    public string EscapedName => EscapeIdentifier(Name);
    public EmissionTypePlan Type => Field.Type;
    public bool IsKey => Field.IsKey;
    public int? MemberId => Field.MemberId;
    public bool IsOptional => Field.IsOptional;
    public string CSharpType => Field.CSharpType;
    public int? Bound => Field.Bound;
    public string? SupportType => Field.SupportType;
    public EmissionTypePlan? ElementType => Field.ElementType;
    public string? ElementCSharpType => Field.ElementCSharpType;
    public string? ElementSupportType => Field.ElementSupportType;
    public IReadOnlyList<int> Dimensions => Field.Dimensions;
    public BigInteger? DefaultValue => Field.ValueMetadata?.DefaultValue;
    public BigInteger? MinimumValue => Field.ValueMetadata?.Minimum;
    public BigInteger? MaximumValue => Field.ValueMetadata?.Maximum;
    public string? DefaultExpression => Field.ValueMetadata?.DefaultExpression;
    public string? Unit => Field.ValueMetadata?.Unit;
    public bool IsExternal => Field.IsExternal;
    public bool IsMustUnderstand => Field.IsMustUnderstand;
    public string? MemberIdHashSource => Field.MemberIdHashSource;
    public bool UsesAutoIdHash => Field.UsesAutoIdHash;
    public bool HasExplicitDefault => DefaultValue is not null;
    public bool HasManagedRange => MinimumValue is not null || MaximumValue is not null;
    public string ManagedBackingFieldName => "_" + EscapedName;
    public bool IsString => ValueType is StringEmissionType;
    public bool IsSequence => shape == FieldEmissionShape.Sequence;
    public bool IsArray => shape == FieldEmissionShape.Array;
    public bool IsSequenceArray => IsSequence && Dimensions.Count > 0;
    public bool IsStringSequence => IsSequence && ElementType is StringEmissionType;
    public bool IsAggregate => Type.IsAggregate;
    public bool IsUnion => Type.IsUnion;
    public bool HasAggregateElement => ElementType?.IsAggregate == true;
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

    public ManagedInitializationKind ManagedInitialization => shape switch
    {
        _ when IsOptional && (IsSequence || IsArray) => ManagedInitializationKind.None,
        FieldEmissionShape.Sequence => ManagedInitializationKind.Sequence,
        FieldEmissionShape.Array => ManagedInitializationKind.Array,
        _ when IsAggregate => ManagedInitializationKind.Aggregate,
        _ => ManagedInitializationKind.None
    };

    public NativeDestroyKind DestroyKind
    {
        get
        {
            if (IsAggregate && !IsSequence && !IsArray)
            {
                return NativeDestroyKind.Nested;
            }

            if (IsSequence || IsArray)
            {
                return NativeDestroyKind.Collection;
            }

            if (IsString)
            {
                return NativeDestroyKind.String;
            }

            if (IsOptionalScalar)
            {
                return NativeDestroyKind.OptionalPrimitive;
            }

            return NativeDestroyKind.None;
        }
    }

    public bool HasTypeSupport => Bound is null || EmissionTypeProjector.HasSequenceType(Type) || IsString;

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


    public string NativeStorageTypeFor(string? namespaceOverride)
    {
        var namespaceName = namespaceOverride ?? currentNamespace;

        switch (shape)
        {
            case FieldEmissionShape.Sequence:
                if (IsStringSequence && IsSequenceArray)
                {
                    return "NativeStringSeq";
                }

                return IsOptional ? "NativeOptionalSeq" : "NativeSeq";
            case FieldEmissionShape.Array:
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
        return ValueType switch
        {
            StringEmissionType { IsWide: true } => "NativeWstring",
            StringEmissionType => "NativeString",
            PrimitiveEmissionType { IdlName: "boolean" or "char" } => "byte",
            PrimitiveEmissionType { IdlName: "wchar" } => "short",
            _ => TypeReference(CSharpType, namespaceName)
        };
    }


    public string BuildCopyExpression(string sourcePrefix = "other.")
    {
        var source = sourcePrefix + EscapedName;

        if (IsSequence)
        {
            var elementType = TypeReference(ElementCSharpType!, currentNamespace);
            var elements = HasAggregateElement ? $".Select(element => new {elementType}(element))" : string.Empty;
            if (IsOptional)
            {
                return $"{source} is null ? null! : new Sequence<{elementType}>({source}{elements})";
            }

            return $"new Sequence<{elementType}>({source}{elements})";
        }

        if (IsArray)
        {
            var copy = $"({TypeReference(CSharpType, currentNamespace)}){source}.Clone()";
            return IsOptional ? $"{source} is null ? null! : {copy}" : copy;
        }

        if (IsAggregate)
        {
            return $"new {TypeReference(CSharpType, currentNamespace)}({source})";
        }

        return source;
    }

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

    public string HashValue(string targetPrefix = "")
    {
        if (IsOptional && IsSequence)
        {
            return $"{targetPrefix}{EscapedName}?.Count ?? -1";
        }

        if (IsOptional && IsArray)
        {
            return $"{targetPrefix}{EscapedName} is null ? -1 : {targetPrefix}{EscapedName}[0]";
        }

        var suffix = shape switch
        {
            FieldEmissionShape.Array => "[0]",
            FieldEmissionShape.Sequence => ".Count",
            _ => string.Empty
        };

        return targetPrefix + EscapedName + suffix;
    }

    public string EqualityExpression(string otherPrefix = "other.", string thisPrefix = "")
    {
        if (IsArray)
        {
            string arrayEquality;
            if (Dimensions.Count == 1)
            {
                arrayEquality = $"{thisPrefix}{EscapedName}.SequenceEqual({otherPrefix}{EscapedName})";
            }
            else
            {
                arrayEquality = $"{thisPrefix}{EscapedName}.Rank == {otherPrefix}{EscapedName}.Rank && Enumerable.Range(0, {thisPrefix}{EscapedName}.Rank).All(dimension => {thisPrefix}{EscapedName}.GetLength(dimension) == {otherPrefix}{EscapedName}.GetLength(dimension)) && {thisPrefix}{EscapedName}.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>().SequenceEqual({otherPrefix}{EscapedName}.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>())";
            }

            if (IsOptional)
            {
                return $"(ReferenceEquals({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName}) || ({thisPrefix}{EscapedName} is not null && {otherPrefix}{EscapedName} is not null && {arrayEquality}))";
            }

            return arrayEquality;
        }

        if (IsOptional && IsSequence)
        {
            return $"(ReferenceEquals({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName}) || ({thisPrefix}{EscapedName} is not null && {otherPrefix}{EscapedName} is not null && {thisPrefix}{EscapedName}.SequenceEqual({otherPrefix}{EscapedName})))";
        }

        if (IsArray && HasAggregateElement)
        {
            return $"{thisPrefix}{EscapedName}.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>().SequenceEqual({otherPrefix}{EscapedName}.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>())";
        }

        if (IsArray || IsSequence)
        {
            return $"{thisPrefix}{EscapedName}.SequenceEqual({otherPrefix}{EscapedName})";
        }

        if (IsOptional)
        {
            return $"Equals({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName})";
        }

        return $"{thisPrefix}{EscapedName}.Equals({otherPrefix}{EscapedName})";
    }

    public (string TypeKind, string ValueProperty, string DefaultValue, string? Minimum, string? Maximum, string? Unit)? PrimitiveAnnotation() =>
        ValueType switch
        {
            StringEmissionType { IsWide: true } => ("WideString", "WideStringValue", "\"\"", null, null, null),
            StringEmissionType => ("String", "StringValue", "\"\"", null, null, null),
            EnumEmissionType enumType => ("Enumeration", "EnumValue", HasExplicitDefault ? DefaultValue!.Value.ToString(CultureInfo.InvariantCulture) : enumType.DefaultValue.ToString(CultureInfo.InvariantCulture), null, null, UnitLiteral),
            PrimitiveEmissionType primitive => primitive.IdlName switch
            {
                "short" or "int16" => ("Int16", "Int16Value", HasExplicitDefault ? FormatCSharpValue("short", DefaultValue!.Value) : "(short)0", MinimumValue is null ? "short.MinValue" : FormatCSharpValue("short", MinimumValue.Value), MaximumValue is null ? "short.MaxValue" : FormatCSharpValue("short", MaximumValue.Value), UnitLiteral),
                "long" or "int32" => ("Int32", "Int32Value", HasExplicitDefault ? FormatCSharpValue("int", DefaultValue!.Value) : "0", MinimumValue is null ? "int.MinValue" : FormatCSharpValue("int", MinimumValue.Value), MaximumValue is null ? "int.MaxValue" : FormatCSharpValue("int", MaximumValue.Value), UnitLiteral),
                "long long" or "int64" => ("Int64", "Int64Value", HasExplicitDefault ? FormatCSharpValue("long", DefaultValue!.Value) : "0L", MinimumValue is null ? "long.MinValue" : FormatCSharpValue("long", MinimumValue.Value), MaximumValue is null ? "long.MaxValue" : FormatCSharpValue("long", MaximumValue.Value), UnitLiteral),
                "unsigned short" or "uint16" => ("Uint16", "Uint16Value", HasExplicitDefault ? FormatCSharpValue("ushort", DefaultValue!.Value) : "(ushort)0", MinimumValue is null ? "ushort.MinValue" : FormatCSharpValue("ushort", MinimumValue.Value), MaximumValue is null ? "ushort.MaxValue" : FormatCSharpValue("ushort", MaximumValue.Value), UnitLiteral),
                "unsigned long" or "uint32" => ("UInt32", "Uint32Value", HasExplicitDefault ? FormatCSharpValue("uint", DefaultValue!.Value) : "0U", MinimumValue is null ? "uint.MinValue" : FormatCSharpValue("uint", MinimumValue.Value), MaximumValue is null ? "uint.MaxValue" : FormatCSharpValue("uint", MaximumValue.Value), UnitLiteral),
                "unsigned long long" or "uint64" => ("UInt64", "Uint64Value", HasExplicitDefault ? FormatCSharpValue("ulong", DefaultValue!.Value) : "0UL", MinimumValue is null ? "ulong.MinValue" : FormatCSharpValue("ulong", MinimumValue.Value), MaximumValue is null ? "ulong.MaxValue" : FormatCSharpValue("ulong", MaximumValue.Value), UnitLiteral),
                "int8" => ("Int8", "Int8Value", HasExplicitDefault ? FormatCSharpValue("sbyte", DefaultValue!.Value) : "(sbyte)0", MinimumValue is null ? "sbyte.MinValue" : FormatCSharpValue("sbyte", MinimumValue.Value), MaximumValue is null ? "sbyte.MaxValue" : FormatCSharpValue("sbyte", MaximumValue.Value), UnitLiteral),
                "uint8" => ("Uint8", "Uint8Value", HasExplicitDefault ? FormatCSharpValue("byte", DefaultValue!.Value) : "(byte)0", MinimumValue is null ? "byte.MinValue" : FormatCSharpValue("byte", MinimumValue.Value), MaximumValue is null ? "byte.MaxValue" : FormatCSharpValue("byte", MaximumValue.Value), UnitLiteral),
                "octet" => ("Octet", "OctetValue", HasExplicitDefault ? FormatCSharpValue("byte", DefaultValue!.Value) : "(byte)0", MinimumValue is null ? "byte.MinValue" : FormatCSharpValue("byte", MinimumValue.Value), MaximumValue is null ? "byte.MaxValue" : FormatCSharpValue("byte", MaximumValue.Value), UnitLiteral),
                "boolean" => ("Boolean", "BoolValue", "false", null, null, UnitLiteral),
                "char" => ("Char8", "Char8Value", "'\\0'", null, null, UnitLiteral),
                "wchar" => ("Char16", "Char16Value", "'\\0'", null, null, UnitLiteral),
                "float" => ("Float32", "Float32Value", "0.0F", "float.MinValue", "float.MaxValue", UnitLiteral),
                "double" => ("Float64", "Float64Value", "0.0D", "double.MinValue", "double.MaxValue", UnitLiteral),
                _ => null
            },
            _ => null
        };

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

    private static FieldEmissionShape GetShape(EmissionTypePlan type) =>
        EmissionTypeProjector.UnwrapOptionalEmissionType(type) switch
        {
            PrimitiveEmissionType => FieldEmissionShape.Primitive,
            StringEmissionType => FieldEmissionShape.String,
            EnumEmissionType => FieldEmissionShape.Enum,
            StructEmissionType or UnionEmissionType => FieldEmissionShape.Struct,
            AliasEmissionType => FieldEmissionShape.Alias,
            SequenceEmissionType => FieldEmissionShape.Sequence,
            ArrayEmissionType => FieldEmissionShape.Array,
            _ => throw new InvalidOperationException("Unknown emission type plan.")
        };
}
