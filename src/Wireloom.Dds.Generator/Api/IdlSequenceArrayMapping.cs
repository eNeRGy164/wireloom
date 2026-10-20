namespace Wireloom;

/// <summary>Selects the generated mapping for direct IDL arrays of sequences.</summary>
public enum IdlSequenceArrayMapping
{
    /// <summary>Preserves RTI C# compatibility by flattening the outer array and reporting DDSG0105.</summary>
    RtiCSharp,

    /// <summary>Preserves the IDL array and its independently sized sequence elements.</summary>
    Idl
}
