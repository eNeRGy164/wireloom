namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Base type for declarations collected from an IDL source.</summary>
internal abstract class IdlDeclaration;

/// <summary>Represents an IDL constant declaration and its evaluated value.</summary>
internal sealed class IdlConstantDeclaration(string name, string type, string expression, string? @namespace, string sourceIdlFileName, BigInteger? integerValue, IdlInput sourceInput, int sourceOffset) : IdlDeclaration
{
    public string Name { get; } = name;
    public string Type { get; } = type;
    public string Expression { get; } = expression;
    public string? Namespace { get; } = @namespace;
    public string SourceIdlFileName { get; } = sourceIdlFileName;
    public BigInteger? IntegerValue { get; private set; } = integerValue;
    internal IdlInput SourceInput { get; } = sourceInput;
    internal int SourceOffset { get; } = sourceOffset;

    /// <summary>Applies the value evaluated after all constants have been registered.</summary>
    internal void SetIntegerValue(BigInteger? value) => IntegerValue = value;
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
