namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Represents raw typedef facts retained until semantic type resolution.</summary>
internal sealed class IdlTypedef(string name, string? @namespace, string target, string? elementType, int? bound, IReadOnlyList<int>? dimensions = null, int? stringBound = null, bool isWideString = false, int effectiveSequenceBound = 100, int effectiveStringBound = 255, bool targetWasEscaped = false, bool elementTypeWasEscaped = false, bool isUnboundedSequence = false, bool isUnboundedString = false, bool isUnboundedStringElement = false)
{
    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    public string Target { get; } = target;
    public bool TargetWasEscaped { get; } = targetWasEscaped;
    public bool ElementTypeWasEscaped { get; } = elementTypeWasEscaped;
    public bool IsSequence => !TargetWasEscaped && Target == "sequence";
    public bool IsArray => !TargetWasEscaped && Target == "array";
    public bool IsCollection => IsSequence || IsArray;
    public string? ElementType { get; } = elementType;
    public int? Bound { get; private set; } = bound;
    public int EffectiveSequenceBound { get; private set; } = bound ?? effectiveSequenceBound;
    public bool IsUnboundedSequence { get; } = isUnboundedSequence;
    public IReadOnlyList<int> Dimensions { get; } = dimensions ?? [];
    public bool IsString => !TargetWasEscaped && Target is ("string" or "wstring");
    public bool IsWideString { get; } = isWideString;
    public int StringBound { get; private set; } = stringBound ?? effectiveStringBound;
    public bool IsStringBounded { get; private set; } = stringBound is not null;
    public bool IsUnboundedString { get; } = isUnboundedString;
    public bool IsUnboundedStringElement { get; } = isUnboundedStringElement;
    /// <summary>Gets the typed default resolved for this alias.</summary>
    public IdlMemberValueMetadata? ValueMetadata { get; private set; }

    /// <summary>Applies a sequence bound resolved after parsing.</summary>
    internal void SetBound(int value)
    {
        Bound = value;
        EffectiveSequenceBound = value;
    }

    /// <summary>Applies a string bound resolved after parsing.</summary>
    internal void SetStringBound(int value)
    {
        StringBound = value;
        IsStringBounded = true;
    }

    /// <summary>Applies the typed default after binding the alias target.</summary>
    internal void SetValueMetadata(IdlMemberValueMetadata? valueMetadata) => ValueMetadata = valueMetadata;
}
