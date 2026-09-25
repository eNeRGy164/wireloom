namespace Wireloom;

using static IdlCompiler;

internal sealed partial class CompilationState
{
    private readonly IdlDeclarationParser parser;
    private readonly IdlSymbolTable symbols;
    private readonly List<GeneratedIdlSource> sources = [];
    private readonly bool strict;

    internal CompilationState(
        IdlDeclarationParser parser,
        IdlSymbolTable symbols,
        bool strict)
    {
        this.parser = parser;
        this.symbols = symbols;
        this.strict = strict;
    }

    public IReadOnlyList<GeneratedIdlSource> Sources => sources;

    public IEnumerable<IdlConstantDeclaration> Constants => symbols.Constants;

    public void AddSource(GeneratedIdlSource source) => sources.Add(source);

    public bool IsEnum(string typeName, string? currentNamespace) =>
        symbols.ContainsEnum(ResolveTypeName(typeName, currentNamespace));

    public void EmitDeclarations()
    {
        foreach (var declaration in parser.Declarations)
        {
            switch (declaration)
            {
                case IdlConstantDeclaration constant:
                    ConstantEmitter.Emit(this, constant, constant.SourceIdlFileName);
                    break;

                case IdlEnumDeclaration @enum:
                    EnumEmitter.Emit(this, @enum.Declaration, @enum.SourceIdlFileName);
                    break;

                case IdlTypedefDeclaration typedef:
                    CollectionAliasEmitter.Emit(this, typedef.Declaration, typedef.SourceIdlFileName);
                    break;

                case IdlClassDeclaration @class:
                    var inheritedFields = GetInheritedFields(@class);

                    if (strict && @class.BaseType is not null && @class.Fields.Any(field => field.Metadata.IsKey))
                    {
                        throw new IdlException(@class.SourceInput, 0, "struct/valuetype derived from a struct/valuetype can not contain @key fields. This check is only enforced when using strict validation.");
                    }

                    ClassEmitter.Emit(this, @class.Name, @class.Namespace, @class.Fields, @class.Extensibility, @class.SourceIdlFileName, @class.BaseType, inheritedFields);
                    break;

                case IdlUnionDeclaration union:
                    UnionEmitter.Emit(this, union.Declaration, union.SourceIdlFileName);
                    break;
            }
        }
    }

    private IReadOnlyList<IdlMember> GetInheritedFields(IdlClassDeclaration declaration)
            => GetInheritedFields(declaration, []);

    private IReadOnlyList<IdlMember> GetInheritedFields(IdlClassDeclaration declaration, HashSet<IdlClassDeclaration> active)
    {
        if (declaration.BaseType is null)
        {
            return [];
        }

        if (!active.Add(declaration))
        {
            throw new IdlException(new IdlInput(declaration.SourceIdlFileName, string.Empty, false), 0, $"Cyclic struct inheritance detected: {declaration.Name}");
        }

        var qualifiedBase = declaration.BaseType.Replace("@", string.Empty);
        if (!parser.TryGetClass(qualifiedBase, out var baseDeclaration))
        {
            throw new IdlException(new IdlInput(declaration.SourceIdlFileName, string.Empty, false), 0, $"Unknown struct base type: {declaration.BaseType}");
        }

        return [.. GetInheritedFields(baseDeclaration, active), .. baseDeclaration.Fields];
    }
}
