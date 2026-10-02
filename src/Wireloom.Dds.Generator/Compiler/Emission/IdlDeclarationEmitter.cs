using Wireloom.Compiler.Emission.Emitters;
using Wireloom.Compiler.FrontEnd.Parsing;
using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.Emission;

/// <summary>Dispatches parsed IDL declarations to their specialized source emitters.</summary>
internal sealed class IdlDeclarationEmitter(
    CompilationContext compilation,
    IdlDeclarationParser parser,
    bool strict,
    EmissionResult result)
{
    /// <summary>Emits source documents for all declarations collected by the parser.</summary>
    public IReadOnlyList<GeneratedIdlSource> Emit()
    {
        foreach (var declaration in parser.Declarations)
        {
            switch (declaration)
            {
                case IdlConstantDeclaration constant:
                    result.AddRange(ConstantEmitter.Emit(compilation, constant, constant.SourceIdlFileName));
                    break;

                case IdlEnumDeclaration @enum:
                    result.AddRange(EnumEmitter.Emit(@enum.Declaration, @enum.SourceIdlFileName));
                    break;

                case IdlTypedefDeclaration typedef:
                    result.AddRange(CollectionAliasEmitter.Emit(compilation, typedef.Declaration, typedef.SourceIdlFileName));
                    break;

                case IdlClassDeclaration @class:
                    var inheritedFields = GetInheritedFields(@class);

                    if (strict && @class.BaseType is not null && @class.Fields.Any(field => field.Metadata.IsKey))
                    {
                        throw new IdlException(@class.SourceInput, 0, "struct/valuetype derived from a struct/valuetype can not contain @key fields. This check is only enforced when using strict validation.");
                    }

                    result.AddRange(ClassEmitter.Emit(@class.Name, @class.Namespace, @class.Fields, @class.Extensibility, @class.SourceIdlFileName, @class.BaseType, inheritedFields, @class.IsTopic));
                    break;

                case IdlUnionDeclaration union:
                    result.AddRange(UnionEmitter.Emit(union.Declaration, union.SourceIdlFileName));
                    break;
            }
        }

        return result.Sources;
    }

    private IReadOnlyList<IdlMember> GetInheritedFields(IdlClassDeclaration declaration) =>
        GetInheritedFields(declaration, []);

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
