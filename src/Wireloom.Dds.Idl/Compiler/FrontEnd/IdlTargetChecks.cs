using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

namespace Wireloom.Dds.Idl.Compiler.FrontEnd;

/// <summary>Describes the IDL declaration at a target validation site.</summary>
internal enum IdlDeclarationKind
{
    Constant,
    Enum,
    Struct,
    Typedef,
    Union
}

/// <summary>Runs optional target checks at the original source locations and in analysis order.</summary>
internal interface IdlTargetChecks
{
    /// <summary>Validates target options for an input before its path is registered.</summary>
    public void ValidateInput(IdlInput input);

    /// <summary>Reports target restrictions for a syntactically recognized member type.</summary>
    public void ObserveMemberType(IdlInput input, int offset, string name, IdlType type, ICollection<IdlDiagnostic>? diagnostics);

    /// <summary>Reports a declaration that the front end parses for validation but does not bind.</summary>
    public void ReportInterface(IdlInput input, int offset, string name, bool isDdsService, ICollection<IdlDiagnostic>? diagnostics);

    /// <summary>Validates the target identity of a declared name in deferred source order.</summary>
    public void ValidateIdentity(IdlInput input, int offset, string name);

    /// <summary>Validates target companion identities for one declaration.</summary>
    public void ValidateCompanions(IdlInput input, int offset, string name, string? currentNamespace, IdlDeclarationKind kind);

    /// <summary>Validates a declaration name against members added by the target.</summary>
    public void ValidateDeclarationName(IdlInput input, int offset, string name, IdlDeclarationKind kind);

    /// <summary>Validates aggregate member names against members added by the target.</summary>
    public void ValidateStructNames(IdlInput input, int offset, string name, IReadOnlyList<IdlMember> fields, string? baseType);

    /// <summary>Validates branch names against members added by the target.</summary>
    public void ValidateUnionNames(IdlInput input, int offset, string name, IReadOnlyList<IdlUnionBranch> branches);

    /// <summary>Validates target restrictions across the complete bound declaration graph.</summary>
    public void ValidateDeclarations(IReadOnlyList<IdlDeclaration> declarations);
}
