namespace Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

/// <summary>Describes the XTypes extensibility policy of an IDL declaration.</summary>
public enum IdlExtensibilityKind
{
    /// <summary>Members can be added, removed, and reordered by member ID.</summary>
    Extensible,
    /// <summary>Members can be appended while preserving existing member order.</summary>
    Appendable,
    /// <summary>The declaration's members and layout cannot be extended.</summary>
    Final,
    /// <summary>Members are identified and ordered by member ID.</summary>
    Mutable
}
