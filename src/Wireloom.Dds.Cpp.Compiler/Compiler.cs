using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;
using Wireloom.Dds.Cpp.Emission;
using Wireloom.Dds.Cpp.Emission.Planning;

namespace Wireloom.Dds.Cpp;

/// <summary>Compiles in-memory IDL for the supported Modern C++ profile.</summary>
internal static class Compiler
{
    /// <summary>Analyzes IDL inputs and emits C++ files when compilation succeeds.</summary>
    /// <param name="inputs">The generation root and its available IDL includes.</param>
    /// <param name="computeMd5">The provider for hashed IDL member identifiers.</param>
    /// <param name="cancellationToken">The token used to cancel IDL analysis.</param>
    /// <param name="diagnostics">The collection that receives source diagnostics.</param>
    /// <param name="profile">The RTI C++ API profile used by the generated code.</param>
    /// <returns>The generated files, or an empty collection when compilation fails.</returns>
    internal static IReadOnlyList<GeneratedCppFile> Compile(
        IReadOnlyList<IdlInput> inputs,
        Func<byte[], byte[]> computeMd5,
        ICollection<IdlDiagnostic> diagnostics,
        RtiApiProfile? profile = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bound = IdlAnalyzer.Analyze(inputs, computeMd5, cancellationToken, diagnostics);
            if (diagnostics.Any(diagnostic => diagnostic.Severity == IdlDiagnosticSeverity.Error))
            {
                return [];
            }

            return Generate(bound.Declarations, inputs.Single(i => i.Generate), diagnostics, profile);
        }
        catch (IdlException exception)
        {
            diagnostics.Add(new IdlDiagnostic("DDSG0001", IdlDiagnosticSeverity.Error, exception.Input, exception.Offset, exception.Message));
            return [];
        }
    }

    /// <summary>Generates C++ declarations and RTI type support from bound IDL declarations.</summary>
    internal static IReadOnlyList<GeneratedCppFile> Generate(IReadOnlyList<IdlDeclaration> declarations, IdlInput input, ICollection<IdlDiagnostic> diagnostics, RtiApiProfile? profile = null)
    {
        var plan = GenerationPlanner.Create(declarations, input, diagnostics, profile ?? RtiApiProfile.Rti770);
        if (plan is null)
        {
            return [];
        }

        return
        [
            new GeneratedCppFile($"{plan.FileStem}.hpp", NormalizeLineEndings(HeaderEmitter.Emit(plan))),
            new GeneratedCppFile($"{plan.FileStem}.cxx", NormalizeLineEndings(ValueEmitter.Emit(plan))),
            new GeneratedCppFile($"{plan.FileStem}Plugin.hpp", NormalizeLineEndings(PluginHeaderEmitter.Emit(plan))),
            new GeneratedCppFile($"{plan.FileStem}Plugin.cxx", NormalizeLineEndings(PluginEmitter.Emit(plan))),
        ];
    }

    /// <summary>Normalizes generated source to LF so host platforms produce identical document bytes.</summary>
    private static string NormalizeLineEndings(string source) => source.ReplaceLineEndings("\n");
}
