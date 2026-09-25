namespace Wireloom;

using static EmissionTypeProjector;
using static IdlCompiler;

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

internal enum ManagedInitializationKind
{
    None,
    Sequence,
    Array,
    Aggregate
}

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
internal sealed partial class MemberEmissionPlan(IdlEmissionField field, string? namespaceName)
{
    private readonly string? currentNamespace = namespaceName;
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
    public bool IsString => ValueType is StringEmissionType;
    public bool IsSequence => shape == FieldEmissionShape.Sequence;
    public bool IsArray => shape == FieldEmissionShape.Array;
    public bool IsAggregate => Type.IsAggregate;
    public bool HasAggregateElement => ElementType?.IsAggregate == true;
    public string? BoundSummary => Bound is not int bound
        ? null
        : IsString
            ? $"Its maximum length is <c>{bound}</c>."
            : HasSequenceType(Type)
                ? $"Its maximum number of elements is <c>{bound}</c>."
                : $"Its DDS bound is <c>{bound}</c>.";
    public ManagedInitializationKind ManagedInitialization => shape switch
    {
        _ when IsOptional && (IsSequence || IsArray) => ManagedInitializationKind.None,
        FieldEmissionShape.Sequence => ManagedInitializationKind.Sequence,
        FieldEmissionShape.Array => ManagedInitializationKind.Array,
        _ when IsAggregate => ManagedInitializationKind.Aggregate,
        _ => ManagedInitializationKind.None
    };

    public NativeDestroyKind DestroyKind =>
        IsAggregate && !IsSequence && !IsArray
            ? NativeDestroyKind.Nested
            : IsSequence || IsArray
                ? NativeDestroyKind.Collection
                : IsString
                    ? NativeDestroyKind.String
                    : IsOptionalScalar
                        ? NativeDestroyKind.OptionalPrimitive
                        : NativeDestroyKind.None;

    public bool HasTypeSupport => Bound is null || HasSequenceType(Type) || IsString;

    public bool IsRecursive(string runtimeTypeName) =>
        IsSequence && string.Equals(ElementCSharpType, runtimeTypeName, StringComparison.Ordinal);

    public string ManagedPropertyAccessors => IsSequence && !IsOptional ? " { get; }" : " { get; set; }";
    public string ManagedPropertySummaryVerb => IsSequence && !IsOptional ? "Gets" : "Gets or sets";

    public string? ManagedPropertyInitializer
    {
        get
        {
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
        ManagedInitializationKind.Sequence => $"{EscapedName} = new Sequence<{TypeReference(ElementCSharpType!, currentNamespace)}>();",
        ManagedInitializationKind.Array => $"{EscapedName} = new {TypeReference(ElementCSharpType!, currentNamespace)}[{string.Join(", ", Dimensions)}];",
        ManagedInitializationKind.Aggregate => $"{EscapedName} = new {TypeReference(CSharpType, currentNamespace)}();",
        _ => null
    };


    public string NativeStorageTypeFor(string? namespaceOverride)
    {
        var namespaceName = namespaceOverride ?? currentNamespace;

        switch (shape)
        {
            case FieldEmissionShape.Sequence:
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
            StringEmissionType stringType when stringType.IsWide => "NativeWstring",
            StringEmissionType => "NativeString",
            PrimitiveEmissionType primitive when primitive.IdlName is "boolean" or "char" => "byte",
            PrimitiveEmissionType primitive when primitive.IdlName == "wchar" => "short",
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
            return $"{targetPrefix}{EscapedName}?.Length ?? -1";
        }

        var suffix = shape switch
        {
            FieldEmissionShape.Array => ".Length",
            FieldEmissionShape.Sequence => ".Count",
            _ => string.Empty
        };

        return targetPrefix + EscapedName + suffix;
    }

    public string EqualityExpression(string otherPrefix = "other.", string thisPrefix = "")
    {
        if (IsOptional && IsArray)
        {
            return $"(ReferenceEquals({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName}) || ({thisPrefix}{EscapedName} is not null && {otherPrefix}{EscapedName} is not null && {thisPrefix}{EscapedName}.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>().SequenceEqual({otherPrefix}{EscapedName}.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>()" + ")))";
        }

        if (IsOptional && IsSequence)
        {
            return $"(ReferenceEquals({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName}) || ({thisPrefix}{EscapedName} is not null && {otherPrefix}{EscapedName} is not null && {thisPrefix}{EscapedName}.SequenceEqual({otherPrefix}{EscapedName})))";
        }

        if (IsArray)
        {
            return $"{thisPrefix}{EscapedName}.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>().SequenceEqual({otherPrefix}{EscapedName}.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>())";
        }

        if (IsSequence)
        {
            return $"{thisPrefix}{EscapedName}.SequenceEqual({otherPrefix}{EscapedName})";
        }

        if (IsOptional)
        {
            return $"Equals({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName})";
        }

        return $"{thisPrefix}{EscapedName}.Equals({otherPrefix}{EscapedName})";
    }

    public (string TypeKind, string ValueProperty, string DefaultValue, string? Minimum, string? Maximum)? PrimitiveAnnotation() =>
        ValueType switch
        {
            StringEmissionType stringType when stringType.IsWide => ("WideString", "WideStringValue", "\"\"", null, null),
            StringEmissionType => ("String", "StringValue", "\"\"", null, null),
            EnumEmissionType enumType => ("Enumeration", "EnumValue", enumType.DefaultValue.ToString(), null, null),
            PrimitiveEmissionType primitive => primitive.IdlName switch
            {
                "short" or "int16" => ("Int16", "Int16Value", "(short)0", "short.MinValue", "short.MaxValue"),
                "long" or "int32" => ("Int32", "Int32Value", "0", "int.MinValue", "int.MaxValue"),
                "long long" or "int64" => ("Int64", "Int64Value", "0L", "long.MinValue", "long.MaxValue"),
                "unsigned short" or "uint16" => ("Uint16", "Uint16Value", "(ushort)0", "ushort.MinValue", "ushort.MaxValue"),
                "unsigned long" or "uint32" => ("UInt32", "Uint32Value", "0u", "uint.MinValue", "uint.MaxValue"),
                "unsigned long long" or "uint64" => ("UInt64", "Uint64Value", "0uL", "ulong.MinValue", "ulong.MaxValue"),
                "int8" => ("Int8", "Int8Value", "(sbyte)0", "sbyte.MinValue", "sbyte.MaxValue"),
                "uint8" => ("Uint8", "Uint8Value", "(byte)0", "byte.MinValue", "byte.MaxValue"),
                "octet" => ("Octet", "OctetValue", "(byte)0", "byte.MinValue", "byte.MaxValue"),
                "boolean" => ("Boolean", "BoolValue", "false", null, null),
                "char" => ("Char8", "Char8Value", "'\\0'", null, null),
                "wchar" => ("Char16", "Char16Value", "'\\0'", null, null),
                "float" => ("Float32", "Float32Value", "0f", "float.MinValue", "float.MaxValue"),
                "double" => ("Float64", "Float64Value", "0d", "double.MinValue", "double.MaxValue"),
                _ => null
            },
            _ => null
        };

    private bool IsOptionalScalar => IsOptional && !IsSequence && !IsArray && !IsString && !IsAggregate;

    private EmissionTypePlan ValueType => UnwrapValueEmissionType(Type);

    private int EnumDefaultValue => ValueType is EnumEmissionType enumType ? enumType.DefaultValue : 0;

    private string GetReferencedUnmanagedType(string? namespaceOverride) =>
        GetUnmanagedType(CSharpType, namespaceOverride);

    private string ElementUnmanagedType(string? namespaceOverride = null)
    {
        var typeName = ElementSupportType ?? ElementCSharpType!;

        return GetUnmanagedType(typeName, namespaceOverride ?? currentNamespace);
    }

    private string NullableValueType() => CSharpType.TrimEnd('?');

    private static FieldEmissionShape GetShape(EmissionTypePlan type) =>
        UnwrapOptionalEmissionType(type) switch
        {
            PrimitiveEmissionType => FieldEmissionShape.Primitive,
            StringEmissionType => FieldEmissionShape.String,
            EnumEmissionType => FieldEmissionShape.Enum,
            StructEmissionType => FieldEmissionShape.Struct,
            AliasEmissionType => FieldEmissionShape.Alias,
            SequenceEmissionType => FieldEmissionShape.Sequence,
            ArrayEmissionType => FieldEmissionShape.Array,
            _ => throw new InvalidOperationException("Unknown emission type plan.")
        };
}
