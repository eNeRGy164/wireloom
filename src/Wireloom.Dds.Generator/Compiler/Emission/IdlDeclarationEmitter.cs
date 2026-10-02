using Wireloom.Compiler.Emission.Emitters;
using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.Emission;

/// <summary>Dispatches parsed IDL declarations to their specialized source emitters.</summary>
internal sealed class IdlDeclarationEmitter(
    CompilationContext compilation,
    IReadOnlyList<IdlDeclaration> declarations,
    EmissionResult result)
{
    /// <summary>Emits source documents for all validated declarations.</summary>
    public IReadOnlyList<GeneratedIdlSource> Emit()
    {
        foreach (var declaration in declarations)
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
                    result.AddRange(ClassEmitter.Emit(@class.Name, @class.Namespace, @class.Fields, @class.Extensibility, @class.SourceIdlFileName, @class.BaseType, @class.InheritedFields, @class.IsTopic));
                    break;

                case IdlUnionDeclaration union:
                    result.AddRange(UnionEmitter.Emit(union.Declaration, union.SourceIdlFileName));
                    break;
            }
        }

        return result.Sources;
    }
}
