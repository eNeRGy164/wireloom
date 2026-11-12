namespace Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

/// <summary>Represents raw typedef facts retained until semantic type resolution.</summary>
public sealed class IdlTypedef(string name, string? @namespace, string target, string? elementType, int? bound, IReadOnlyList<int>? dimensions = null, int? stringBound = null, bool isWideString = false, int effectiveSequenceBound = 100, int effectiveStringBound = 255, bool targetWasEscaped = false, bool elementTypeWasEscaped = false, bool isUnboundedSequence = false, bool isUnboundedString = false, bool isUnboundedStringElement = false)
{
    /// <summary>Gets the alias's declared IDL name.</summary>
    public string Name { get; } = name;
    /// <summary>Gets the enclosing namespace, or null for the global namespace.</summary>
    public string? Namespace { get; } = @namespace;
    /// <summary>Gets the IDL spelling of the alias target.</summary>
    public string Target { get; } = target;
    /// <summary>Gets whether the target spelling was escaped as an identifier.</summary>
    public bool TargetWasEscaped { get; } = targetWasEscaped;
    /// <summary>Gets whether the collection element spelling was escaped as an identifier.</summary>
    public bool ElementTypeWasEscaped { get; } = elementTypeWasEscaped;
    /// <summary>Gets whether the alias target is a sequence.</summary>
    public bool IsSequence => !TargetWasEscaped && Target == "sequence";
    /// <summary>Gets whether the alias target is an array.</summary>
    public bool IsArray => !TargetWasEscaped && Target == "array";
    /// <summary>Gets whether the alias target is a sequence or array.</summary>
    public bool IsCollection => IsSequence || IsArray;
    /// <summary>Gets the collection element's IDL type spelling.</summary>
    public string? ElementType { get; } = elementType;
    /// <summary>Gets whether a collection element is a narrow or wide string.</summary>
    public bool ElementTypeIsString => ElementType is not null
        && IdlBuiltinTypeSyntax.TryParseStringType(ElementType, out _, out _);
    /// <summary>Gets whether a collection element is a wide string.</summary>
    public bool ElementTypeIsWideString => ElementType is not null
        && IdlBuiltinTypeSyntax.TryParseStringType(ElementType, out var isWide, out _) && isWide;
    /// <summary>Gets the declared collection bound, if any.</summary>
    public int? Bound { get; private set; } = bound;
    /// <summary>Gets the declared sequence bound or its configured fallback.</summary>
    public int EffectiveSequenceBound { get; private set; } = bound ?? effectiveSequenceBound;
    /// <summary>Gets whether the sequence uses explicit unbounded semantics.</summary>
    public bool IsUnboundedSequence { get; } = isUnboundedSequence;
    /// <summary>Gets the array dimensions in source order.</summary>
    public IReadOnlyList<int> Dimensions { get; } = dimensions ?? [];
    /// <summary>Gets whether the alias target is a string type.</summary>
    public bool IsString => !TargetWasEscaped && Target is ("string" or "wstring");
    /// <summary>Gets whether the alias's string target is wide.</summary>
    public bool IsWideString { get; } = isWideString;
    /// <summary>Gets the declared string bound or its configured fallback.</summary>
    public int StringBound { get; private set; } = stringBound ?? effectiveStringBound;
    /// <summary>Gets whether the string target has an explicit bound.</summary>
    public bool IsStringBounded { get; private set; } = stringBound is not null;
    /// <summary>Gets whether the string target uses explicit unbounded semantics.</summary>
    public bool IsUnboundedString { get; } = isUnboundedString;
    /// <summary>Gets whether string elements use explicit unbounded semantics.</summary>
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
