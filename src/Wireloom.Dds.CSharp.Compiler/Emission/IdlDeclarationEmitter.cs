using Wireloom.Dds.CSharp.Compiler.Emission.Emitters;
using Wireloom.Dds.CSharp.Compiler.Emission.Planning;
using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

namespace Wireloom.Dds.CSharp.Compiler.Emission;

/// <summary>Dispatches parsed IDL declarations to their specialized source emitters.</summary>
internal sealed class IdlDeclarationEmitter(CompilationContext compilation, IReadOnlyList<IdlDeclaration> declarations, EmissionResult result, RtiApiProfile profile, SequenceArrayEmissionPlan sequenceArrays)
{
    private readonly string[] flatDataModules = declarations
        .OfType<IdlModuleDeclaration>()
        .Where(module => module.Annotations.Any(annotation =>
            annotation.Name.Equals("language_binding", StringComparison.OrdinalIgnoreCase)
            && annotation.Arguments.Any(argument => argument.Expression.Equals("FLAT_DATA", StringComparison.OrdinalIgnoreCase))))
        .Select(module => module.Namespace is null ? module.Name : $"{module.Namespace}.{module.Name}")
        .ToArray();

    /// <summary>Emits source documents for all validated declarations.</summary>
    public IReadOnlyList<GeneratedIdlSource> Emit()
    {
        var recursiveTypes = new RecursiveTypeGraph(declarations);

        foreach (var element in sequenceArrays.Elements)
        {
            result.AddRange(CollectionAliasEmitter.Emit(element.Declaration, element.Plan, element.SourceIdlFileName, profile));
        }

        foreach (var declaration in declarations)
        {
            switch (declaration)
            {
                case IdlConstantDeclaration constant:
                    result.AddRange(ConstantEmitter.Emit(compilation, constant, constant.SourceIdlFileName, profile));
                    break;

                case IdlEnumDeclaration @enum:
                    result.AddRange(EnumEmitter.Emit(@enum.Declaration, @enum.SourceIdlFileName, profile));
                    break;

                case IdlTypedefDeclaration typedef:
                    result.AddRange(CollectionAliasEmitter.Emit(compilation, typedef.Declaration, typedef.SourceIdlFileName, profile, recursiveTypes));
                    break;

                case IdlClassDeclaration @class:
                    result.AddRange(ClassEmitter.Emit(@class.Name, @class.Namespace, @class.Fields,
                        GetRtiExtensibility(@class), @class.SourceIdlFileName, @class.BaseType,
                        @class.InheritedFields, @class.IsTopic, profile, sequenceArrays, recursiveTypes));
                    break;

                case IdlUnionDeclaration union:
                    result.AddRange(UnionEmitter.Emit(union.Declaration, union.SourceIdlFileName, profile, sequenceArrays));
                    break;
            }
        }

        return result.Sources;
    }

    private IdlExtensibilityKind GetRtiExtensibility(IdlClassDeclaration declaration)
    {
        if (declaration.Extensibility == IdlExtensibilityKind.Final
            && !profile.SupportsFinalExtensibilityWithFlatData
            && flatDataModules.Any(module => declaration.Namespace == module
                || declaration.Namespace?.StartsWith(module + ".", StringComparison.Ordinal) == true))
        {
            // The retained RTI 6.1.2 and 7.3.1 C# generators emit Extensible
            // metadata for final types inside a FlatData-bound module.
            return IdlExtensibilityKind.Extensible;
        }

        return declaration.Extensibility;
    }
}
