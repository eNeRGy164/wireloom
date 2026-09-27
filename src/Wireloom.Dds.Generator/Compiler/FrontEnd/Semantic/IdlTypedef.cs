namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Represents raw typedef facts retained until semantic type resolution.</summary>
internal sealed class IdlTypedef(string name, string? @namespace, string target, string? elementType, int? bound, IReadOnlyList<int>? dimensions = null, int? stringBound = null, bool isWideString = false)
{
    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    public string Target { get; } = target;
    public bool IsSequence => Target == "sequence";
    public bool IsArray => Target == "array";
    public bool IsCollection => IsSequence || IsArray;
    public string? ElementType { get; } = elementType;
    public int? Bound { get; } = bound;
    public IReadOnlyList<int> Dimensions { get; } = dimensions ?? [];
    public bool IsString => Target is "string" or "wstring";
    public bool IsWideString { get; } = isWideString;
    public int StringBound { get; } = stringBound ?? 255;
}
