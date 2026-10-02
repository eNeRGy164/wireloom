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
                    ConstantEmitter.Emit(compilation, result, constant, constant.SourceIdlFileName);
                    break;

                case IdlEnumDeclaration @enum:
                    EnumEmitter.Emit(result, @enum.Declaration, @enum.SourceIdlFileName);
                    break;

                case IdlTypedefDeclaration typedef:
                    CollectionAliasEmitter.Emit(compilation, result, typedef.Declaration, typedef.SourceIdlFileName);
                    break;

                case IdlClassDeclaration @class:
                    ClassEmitter.Emit(result, @class.Name, @class.Namespace, @class.Fields, @class.Extensibility, @class.SourceIdlFileName, @class.BaseType, @class.InheritedFields, @class.IsTopic);
                    break;

                case IdlUnionDeclaration union:
                    UnionEmitter.Emit(result, union.Declaration, union.SourceIdlFileName);
                    break;
            }
        }

        return result.Sources;
    }
}
