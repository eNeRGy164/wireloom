namespace Wireloom.Compiler.Semantics;

/// <summary>Base type for declarations collected from an IDL source.</summary>
internal abstract class IdlDeclaration;

/// <summary>Represents an IDL constant declaration and its evaluated value.</summary>
internal sealed class IdlConstantDeclaration(string name, string type, string expression, string? @namespace, string sourceIdlFileName, BigInteger? integerValue) : IdlDeclaration
{
    public string Name { get; } = name;
    public string Type { get; } = type;
    public string Expression { get; } = expression;
    public string? Namespace { get; } = @namespace;
    public string SourceIdlFileName { get; } = sourceIdlFileName;
    public BigInteger? IntegerValue { get; } = integerValue;
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
internal sealed class IdlClassDeclaration(string name, string? @namespace, IReadOnlyList<IdlMember> fields, IdlExtensibilityKind extensibility, IdlInput sourceInput, string? baseType, bool isTopic) : IdlDeclaration
{
    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    public IReadOnlyList<IdlMember> Fields { get; } = fields;
    public IdlExtensibilityKind Extensibility { get; } = extensibility;
    public IdlInput SourceInput { get; } = sourceInput;
    public string SourceIdlFileName { get; } = Path.GetFileName(sourceInput.Path);
    public string? BaseType { get; } = baseType;
    public bool IsTopic { get; } = isTopic;
}

/// <summary>Represents an IDL union declaration and its source file.</summary>
internal sealed class IdlUnionDeclaration(IdlUnion declaration, string sourceIdlFileName) : IdlDeclaration
{
    public IdlUnion Declaration { get; } = declaration;
    public string SourceIdlFileName { get; } = sourceIdlFileName;
}
