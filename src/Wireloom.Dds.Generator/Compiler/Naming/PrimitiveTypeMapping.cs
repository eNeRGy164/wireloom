namespace Wireloom.Compiler.Naming;

/// <summary>Provides the shared target mappings for one supported IDL primitive.</summary>
internal sealed class PrimitiveTypeMapping(
    string idlName,
    string managedType,
    string nativeStorageType,
    string dynamicType,
    string nativeDefaultLiteral,
    string? annotationTypeKind,
    string? annotationValueProperty,
    string? minimumLiteral = null,
    string? maximumLiteral = null,
    string? uncastNativeDefaultLiteral = null,
    string? annotationDefaultLiteral = null,
    bool nativeValueRequiresCast = true)
{
    private string IdlName { get; } = idlName;

    /// <summary>Gets the managed C# type.</summary>
    public string ManagedType { get; } = managedType;

    /// <summary>Gets the native storage type.</summary>
    public string NativeStorageType { get; } = nativeStorageType;

    /// <summary>Gets the RTI dynamic-type primitive name.</summary>
    public string DynamicType { get; } = dynamicType;

    /// <summary>Gets the native initialization literal for a zero/default value.</summary>
    public string NativeDefaultLiteral { get; } = nativeDefaultLiteral;

    /// <summary>Gets the RTI annotation type kind, when annotations support this primitive.</summary>
    public string? AnnotationTypeKind { get; } = annotationTypeKind;

    /// <summary>Gets the RTI annotation value property, when annotations support this primitive.</summary>
    public string? AnnotationValueProperty { get; } = annotationValueProperty;

    /// <summary>Gets the default minimum annotation literal, when one exists.</summary>
    public string? MinimumLiteral { get; } = minimumLiteral;

    /// <summary>Gets the default maximum annotation literal, when one exists.</summary>
    public string? MaximumLiteral { get; } = maximumLiteral;

    /// <summary>Gets the collection default literal when no native cast is required.</summary>
    public string UncastNativeDefaultLiteral => uncastNativeDefaultLiteral ?? NativeDefaultLiteral;

    /// <summary>Gets the annotation default literal for this primitive.</summary>
    public string AnnotationDefaultLiteral => annotationDefaultLiteral ?? NativeDefaultLiteral;

    /// <summary>Gets whether collection native initialization requires a managed-type cast.</summary>
    public bool NativeValueRequiresCast { get; } = nativeValueRequiresCast;

    /// <summary>Builds the managed expression used to read this primitive from native storage.</summary>
    public string FromNativeExpression(string source)
    {
        switch (IdlName)
        {
            case "boolean":
                return $"global::System.Convert.ToBoolean({source})";
            case "char":
                return $"NativeChar.FromUtf8({source})";
            case "wchar":
                return $"(char){source}";
            default:
                return source;
        }
    }

    /// <summary>Builds the native expression used to write this primitive from managed storage.</summary>
    public string ToNativeExpression(string source)
    {
        switch (IdlName)
        {
            case "boolean":
                return $"global::System.Convert.ToByte({source})";
            case "char":
                return $"NativeChar.ToUtf8({source})";
            case "wchar":
                return $"(short){source}";
            default:
                return source;
        }
    }

    /// <summary>Resolves one IDL primitive spelling to its shared target mapping.</summary>
    public static PrimitiveTypeMapping Resolve(string idlType)
    {
        var normalized = IdlNaming.NormalizeIdlType(idlType);

        switch (normalized)
        {
            case "short":
            case "int16":
                return new("short", "short", "short", "short", "(short)0", "Int16", "Int16Value", "short.MinValue", "short.MaxValue", "0", nativeValueRequiresCast: false);
            case "long":
            case "int32":
                return new("long", "int", "int", "int", "0", "Int32", "Int32Value", "int.MinValue", "int.MaxValue");
            case "long long":
            case "int64":
                return new("long long", "long", "long", "long", "0L", "Int64", "Int64Value", "long.MinValue", "long.MaxValue");
            case "unsigned short":
            case "uint16":
                return new("unsigned short", "ushort", "ushort", "ushort", "(ushort)0", "Uint16", "Uint16Value", "ushort.MinValue", "ushort.MaxValue");
            case "unsigned long":
            case "uint32":
                return new("unsigned long", "uint", "uint", "uint", "0U", "UInt32", "Uint32Value", "uint.MinValue", "uint.MaxValue");
            case "unsigned long long":
            case "uint64":
                return new("unsigned long long", "ulong", "ulong", "ulong", "0UL", "UInt64", "UInt64Value", "ulong.MinValue", "ulong.MaxValue");
            case "int8":
                return new("int8", "sbyte", "sbyte", "sbyte", "(sbyte)0", "Int8", "Int8Value", "sbyte.MinValue", "sbyte.MaxValue");
            case "uint8":
                return new("uint8", "byte", "byte", "byte", "(byte)0", "Uint8", "Uint8Value", "byte.MinValue", "byte.MaxValue");
            case "octet":
                return new("octet", "byte", "byte", "Octet", "(byte)0", "Octet", "OctetValue", "byte.MinValue", "byte.MaxValue");
            case "boolean":
                return new("boolean", "bool", "byte", "bool", "0", "Boolean", "BoolValue", annotationDefaultLiteral: "false");
            case "char":
                return new("char", "char", "byte", "char", "(byte)0", "Char8", "Char8Value", annotationDefaultLiteral: "'\\0'");
            case "wchar":
                return new("wchar", "char", "short", "DynamicTypeFactory.WideCharType", "(short)0", "Char16", "Char16Value", annotationDefaultLiteral: "'\\0'");
            case "float":
                return new("float", "float", "float", "float", "0.0F", "Float32", "Float32Value", "float.MinValue", "float.MaxValue", annotationDefaultLiteral: "0F", nativeValueRequiresCast: false);
            case "double":
                return new("double", "double", "double", "double", "0.0D", "Float64", "Float64Value", "double.MinValue", "double.MaxValue", annotationDefaultLiteral: "0D", nativeValueRequiresCast: false);
            case "long double":
                return new("long double", "LongDouble", "LongDouble", "LongDouble", "(LongDouble)0", null, null);
            default:
                throw new InvalidOperationException($"Unsupported primitive type: {idlType}");
        }
    }

    /// <summary>Determines whether an IDL spelling is a supported primitive.</summary>
    public static bool IsPrimitive(string idlType)
    {
        try
        {
            Resolve(idlType);
            return true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }
}
