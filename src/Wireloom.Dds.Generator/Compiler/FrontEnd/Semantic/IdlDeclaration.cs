using Wireloom.Compiler.FrontEnd.Parsing;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Base type for declarations collected from an IDL source.</summary>
internal abstract class IdlDeclaration
{
    /// <summary>Gets captured declaration-level applications retained on their semantic target.</summary>
    internal IReadOnlyList<IdlAnnotationApplicationSyntax> AnnotationApplications { get; private set; } = [];

    /// <summary>Retains the declaration-level applications that were consumed before target parsing.</summary>
    internal void SetAnnotationApplications(IReadOnlyList<IdlAnnotationApplicationSyntax> applications) =>
        AnnotationApplications = applications;
}

/// <summary>Represents a lexical IDL module scope and its scope-level annotations.</summary>
internal sealed class IdlModuleDeclaration(string name, string? @namespace) : IdlDeclaration
{
    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;

    /// <summary>Resolves the member-ID policy inherited by declarations in this module scope.</summary>
    internal bool ResolveMemberIdHashPolicy(bool inheritedPolicy)
    {
        var application = AnnotationApplications
            .SingleOrDefault(candidate => candidate.Name.Equals("autoid", StringComparison.OrdinalIgnoreCase));
        if (application is null)
        {
            return inheritedPolicy;
        }

        var value = application.ParseArguments().SingleOrDefault()?.Expression ?? "HASH";
        return string.Equals(value, "HASH", StringComparison.OrdinalIgnoreCase);
    }
}

/// <summary>Represents an IDL constant declaration and its evaluated value.</summary>
internal sealed class IdlConstantDeclaration(string name, string type, string expression, string? @namespace, string sourceIdlFileName, IdlScalarValue? value, IdlInput sourceInput, int sourceOffset, bool typeWasEscaped = false, bool expressionWasEscaped = false) : IdlDeclaration
{
    public string Name { get; } = name;
    public string Type { get; } = type;
    public string Expression { get; } = expression;
    public string? Namespace { get; } = @namespace;
    public string SourceIdlFileName { get; } = sourceIdlFileName;

    /// <summary>Gets the bound value, retaining its scalar category and enum or character identity.</summary>
    internal IdlScalarValue? Value { get; private set; } = value;

    internal IdlInput SourceInput { get; } = sourceInput;
    internal int SourceOffset { get; } = sourceOffset;
    internal bool TypeWasEscaped { get; } = typeWasEscaped;
    internal bool ExpressionWasEscaped { get; } = expressionWasEscaped;

    /// <summary>Applies the typed value established by semantic binding.</summary>
    internal void SetValue(IdlScalarValue? boundValue) => Value = boundValue;
}

/// <summary>Represents an IDL enum declaration and its source file.</summary>
internal sealed class IdlEnumDeclaration(IdlEnum declaration, string sourceIdlFileName) : IdlDeclaration
{
    public IdlEnum Declaration { get; } = declaration;
    public string SourceIdlFileName { get; } = sourceIdlFileName;
}

/// <summary>Represents an IDL typedef declaration and its source file.</summary>
internal sealed class IdlTypedefDeclaration(IdlTypedef declaration, string sourceIdlFileName) : IdlDeclaration
{
    public IdlTypedef Declaration { get; } = declaration;
    public string SourceIdlFileName { get; } = sourceIdlFileName;
}

/// <summary>Represents an IDL struct or topic declaration.</summary>
internal sealed class IdlClassDeclaration(
    string name,
    string? @namespace,
    IReadOnlyList<IdlMember> fields,
    IdlExtensibilityKind extensibility,
    IdlInput sourceInput,
    string? baseType,
    int baseTypeOffset,
    bool isTopic) : IdlDeclaration
{
    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    public IReadOnlyList<IdlMember> Fields { get; } = fields;
    public IdlExtensibilityKind Extensibility { get; } = extensibility;
    public IdlInput SourceInput { get; } = sourceInput;
    public string SourceIdlFileName { get; } = Path.GetFileName(sourceInput.Path);
    public string? BaseType { get; private set; } = baseType;

    /// <summary>Gets the original source offset of the declared base type.</summary>
    public int BaseTypeOffset { get; } = baseTypeOffset;

    /// <summary>Gets the base-class members resolved during the binding phase.</summary>
    public IReadOnlyList<IdlMember> InheritedFields { get; private set; } = [];
    public bool IsTopic { get; } = isTopic;
    /// <summary>Applies the canonical base type name resolved during binding.</summary>
    internal void SetBaseType(string resolvedBaseType) => BaseType = resolvedBaseType;

    /// <summary>Applies the inherited member sequence resolved during binding.</summary>
    internal void SetInheritedFields(IReadOnlyList<IdlMember> inheritedFields) => InheritedFields = inheritedFields;

}

/// <summary>Represents an IDL union declaration and its source file.</summary>
internal sealed class IdlUnionDeclaration(IdlUnion declaration, string sourceIdlFileName) : IdlDeclaration
{
    public IdlUnion Declaration { get; } = declaration;
    public string SourceIdlFileName { get; } = sourceIdlFileName;
}
