using System.Security.Cryptography;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Wireloom.Roslyn;

namespace Wireloom;

/// <summary>
/// Generates C# source from <c>DdsIdl</c> roots and their tracked include
/// closures.
/// </summary>
[Generator]
public sealed class Generator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var inputs = context.AdditionalTextsProvider
            .Where(file => file.Path.EndsWith(".idl", StringComparison.OrdinalIgnoreCase))
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Select(GeneratorInputFactory.Create)
            .Collect()
            .Combine(context.CompilationProvider);

        context.RegisterSourceOutput(inputs, (production, compilationAndInputs) =>
        {
            var files = compilationAndInputs.Left;
            var compilation = compilationAndInputs.Right;

            if (!files.Any(file => file.Input.Generate))
            {
                return;
            }

            if (compilation is CSharpCompilation { LanguageVersion: < LanguageVersion.CSharp12 } csharpCompilation)
            {
                production.ReportDiagnostic(Diagnostic.Create(GeneratorDiagnostics.LanguageVersionError, Location.None, csharpCompilation.LanguageVersion));

                return;
            }

            if (!RuntimeReferenceGuard.TryGetProfile(compilation, out var profile))
            {
                production.ReportDiagnostic(Diagnostic.Create(GeneratorDiagnostics.RuntimeReferenceError, Location.None));
                return;
            }

            var invalidBooleanSetting = files.FirstOrDefault(file => file.InvalidBooleanSettingName is not null);
            if (invalidBooleanSetting is not null)
            {
                ReportInvalidBooleanSetting(production, invalidBooleanSetting);
                return;
            }

            var result = IdlCompiler.Compile(
                files.Select(file => file.Input).ToArray(),
                profile,
                static bytes => MD5.HashData(bytes),
                production.CancellationToken);
            GeneratorDiagnostics.Report(production, result.Diagnostics);

            if (!result.Success)
            {
                return;
            }

            foreach (var output in result.Documents)
            {
                production.AddSource(output.HintName, SourceText.From(output.Source, Encoding.UTF8));
            }
        });
    }

    private static void ReportInvalidBooleanSetting(SourceProductionContext production, GeneratorInput input)
    {
        var text = SourceText.From(input.Input.Text);
        var span = new TextSpan(0, 0);

        production.ReportDiagnostic(Diagnostic.Create(GeneratorDiagnostics.GenerationError,
            Location.Create(input.Input.Path, span, text.Lines.GetLinePositionSpan(span)),
            $"{input.InvalidBooleanSettingName} must be 'true' or 'false'."));
    }
}
