namespace Wireloom.Compiler.Emission.Model;

/// <summary>Resolved facts shared by every emitter for one IDL typedef.</summary>
internal sealed class CollectionAliasEmissionPlan(
    string elementType,
    string elementIdlType,
    bool isString,
    bool isPrimitive,
    bool isEnum,
    bool isAggregate,
    bool isUnion,
    bool nativeValueRequiresCast)
{
    public string ElementType { get; } = elementType;

    public string ElementIdlType { get; } = elementIdlType;

    public bool IsString { get; } = isString;

    public bool IsPrimitive { get; } = isPrimitive;

    public bool IsEnum { get; } = isEnum;

    public bool IsAggregate { get; } = isAggregate;

    public bool IsUnion { get; } = isUnion;

    public bool NativeValueRequiresCast { get; } = nativeValueRequiresCast;

    public bool CollectionElementIsAggregate { get; } = isAggregate && !isString;

    public bool IsWideString { get; } = elementIdlType.StartsWith("wstring", StringComparison.Ordinal);

    public int StringBound { get; } = ParseStringBound(elementIdlType);

    private static int ParseStringBound(string typeName)
    {
        var open = typeName.IndexOf('<');
        return open < 0 || !int.TryParse(typeName[(open + 1)..^1].Trim(), out var bound)
            ? 255
            : bound;
    }
}
