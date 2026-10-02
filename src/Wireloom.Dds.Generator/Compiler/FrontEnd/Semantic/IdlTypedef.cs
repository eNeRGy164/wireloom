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
    public int? Bound { get; private set; } = bound;
    public IReadOnlyList<int> Dimensions { get; } = dimensions ?? [];
    public bool IsString => Target is "string" or "wstring";
    public bool IsWideString { get; } = isWideString;
    public int StringBound { get; private set; } = stringBound ?? 255;

    /// <summary>Applies a sequence bound resolved after parsing.</summary>
    internal void SetBound(int value) => Bound = value;

    /// <summary>Applies a string bound resolved after parsing.</summary>
    internal void SetStringBound(int value) => StringBound = value;

}
