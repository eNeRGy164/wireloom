using Wireloom.Dds.Idl.Compiler.FrontEnd.Parsing;

namespace Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

/// <summary>Base type for declarations collected from an IDL source.</summary>
public abstract class IdlDeclaration
{
    private protected IdlDeclaration()
    {
    }

    /// <summary>Gets parsed declaration-level annotations retained for backend validation.</summary>
    internal IReadOnlyList<IdlAnnotationApplicationSyntax> AnnotationApplications { get; private set; } = [];

    /// <summary>Gets the parsed annotations attached to this declaration in source order.</summary>
    public IReadOnlyList<IdlAnnotation> Annotations { get; private set; } = [];

    /// <summary>Stores an immutable snapshot of the declaration-level annotation applications.</summary>
    internal void SetAnnotationApplications(IReadOnlyList<IdlAnnotationApplicationSyntax> applications)
    {
        AnnotationApplications = Array.AsReadOnly(applications.ToArray());
        Annotations = Array.AsReadOnly(applications.Select(IdlAnnotation.FromSyntax).ToArray());
    }
}

/// <summary>Represents a lexical IDL module scope and its scope-level annotations.</summary>
public sealed class IdlModuleDeclaration(string name, string? @namespace) : IdlDeclaration
{
    /// <summary>Gets the declared module name.</summary>
    public string Name { get; } = name;
    /// <summary>Gets the enclosing namespace, or null for the global scope.</summary>
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
public sealed class IdlConstantDeclaration(string name, string type, string expression, string? @namespace, string sourceIdlFileName, IdlScalarValue? value, IdlInput sourceInput, int sourceOffset, bool typeWasEscaped = false, bool expressionWasEscaped = false) : IdlDeclaration
{
    /// <summary>Gets the declared constant name.</summary>
    public string Name { get; } = name;
    /// <summary>Gets the declared IDL type spelling.</summary>
    public string Type { get; } = type;
    /// <summary>Gets the source expression used to define the constant.</summary>
    public string Expression { get; } = expression;
    /// <summary>Gets the enclosing namespace, or null for the global namespace.</summary>
    public string? Namespace { get; } = @namespace;
    /// <summary>Gets the source IDL file name that declares the constant.</summary>
    public string SourceIdlFileName { get; } = sourceIdlFileName;

    /// <summary>Gets the bound value, retaining its scalar category and enum or character identity.</summary>
    public IdlScalarValue? Value { get; private set; } = value;

    /// <summary>Gets the input containing the constant declaration.</summary>
    public IdlInput SourceInput { get; } = sourceInput;
    /// <summary>Gets the zero-based source offset of the constant declaration.</summary>
    public int SourceOffset { get; } = sourceOffset;
    internal bool TypeWasEscaped { get; } = typeWasEscaped;
    internal bool ExpressionWasEscaped { get; } = expressionWasEscaped;

    /// <summary>Applies the typed value established by semantic binding.</summary>
    internal void SetValue(IdlScalarValue? boundValue) => Value = boundValue;
}

/// <summary>Represents an IDL enum declaration and its source file.</summary>
public sealed class IdlEnumDeclaration(IdlEnum declaration, string sourceIdlFileName) : IdlDeclaration
{
    /// <summary>Gets the bound enum definition.</summary>
    public IdlEnum Declaration { get; } = declaration;
    /// <summary>Gets the source IDL file name that declares the enum.</summary>
    public string SourceIdlFileName { get; } = sourceIdlFileName;
}

/// <summary>Represents an IDL typedef declaration and its source file.</summary>
public sealed class IdlTypedefDeclaration(IdlTypedef declaration, string sourceIdlFileName) : IdlDeclaration
{
    /// <summary>Gets the bound typedef definition.</summary>
    public IdlTypedef Declaration { get; } = declaration;
    /// <summary>Gets the source IDL file name that declares the typedef.</summary>
    public string SourceIdlFileName { get; } = sourceIdlFileName;
}

/// <summary>Represents an IDL struct or topic declaration.</summary>
public sealed class IdlClassDeclaration(
    string name,
    string? @namespace,
    IReadOnlyList<IdlMember> fields,
    IdlExtensibilityKind extensibility,
    IdlInput sourceInput,
    string? baseType,
    int baseTypeOffset,
    bool isTopic,
    bool isValueType = false) : IdlDeclaration
{
    /// <summary>Gets the declared struct or topic name.</summary>
    public string Name { get; } = name;
    /// <summary>Gets the enclosing namespace, or null for the global namespace.</summary>
    public string? Namespace { get; } = @namespace;
    /// <summary>Gets the declared members in source order.</summary>
    public IReadOnlyList<IdlMember> Fields { get; } = fields;
    /// <summary>Gets the XTypes extensibility policy.</summary>
    public IdlExtensibilityKind Extensibility { get; } = extensibility;
    /// <summary>Gets the input containing the declaration.</summary>
    public IdlInput SourceInput { get; } = sourceInput;
    /// <summary>Gets the source IDL file name that declares this type.</summary>
    public string SourceIdlFileName { get; } = Path.GetFileName(sourceInput.Path);
    /// <summary>Gets the resolved base type name, if the declaration inherits from a type.</summary>
    public string? BaseType { get; private set; } = baseType;

    /// <summary>Gets the original source offset of the declared base type.</summary>
    public int BaseTypeOffset { get; } = baseTypeOffset;

    /// <summary>Gets the base-class members resolved during the binding phase.</summary>
    public IReadOnlyList<IdlMember> InheritedFields { get; private set; } = [];
    /// <summary>Gets whether the struct is marked as a DDS topic.</summary>
    public bool IsTopic { get; } = isTopic;
    /// <summary>Gets whether the declaration is an IDL valuetype.</summary>
    public bool IsValueType { get; } = isValueType;
    /// <summary>Applies the canonical base type name resolved during binding.</summary>
    internal void SetBaseType(string resolvedBaseType) => BaseType = resolvedBaseType;

    /// <summary>Applies the inherited member sequence resolved during binding.</summary>
    internal void SetInheritedFields(IReadOnlyList<IdlMember> inheritedFields) => InheritedFields = inheritedFields;

}

/// <summary>Represents an IDL union declaration and its source file.</summary>
public sealed class IdlUnionDeclaration(IdlUnion declaration, string sourceIdlFileName) : IdlDeclaration
{
    /// <summary>Gets the bound union definition.</summary>
    public IdlUnion Declaration { get; } = declaration;
    /// <summary>Gets the source IDL file name that declares the union.</summary>
    public string SourceIdlFileName { get; } = sourceIdlFileName;
}
