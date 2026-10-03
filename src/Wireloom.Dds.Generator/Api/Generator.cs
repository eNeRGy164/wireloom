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

            if (!files.Any(file => file.Generate))
            {
                return;
            }

            if (compilation is CSharpCompilation { LanguageVersion: < LanguageVersion.CSharp12 } csharpCompilation)
            {
                production.ReportDiagnostic(Diagnostic.Create(GeneratorDiagnostics.LanguageVersionError, Location.None, csharpCompilation.LanguageVersion));

                return;
            }

            if (!RuntimeReferenceGuard.HasCompatibleRuntime(compilation))
            {
                production.ReportDiagnostic(Diagnostic.Create(GeneratorDiagnostics.RuntimeReferenceError, Location.None));
                return;
            }

            var diagnostics = new List<IdlDiagnostic>();

            try
            {
                var outputs = IdlCompiler.CompileSourcesWithDiagnostics([.. files], diagnostics, production.CancellationToken);
                GeneratorDiagnostics.Report(production, diagnostics);

                foreach (var output in outputs.Values)
                {
                    production.AddSource(output.HintName, SourceText.From(output.Source, Encoding.UTF8));
                }
            }
            catch (IdlException exception)
            {
                GeneratorDiagnostics.Report(production, diagnostics);
                var text = SourceText.From(exception.Input.Text);
                var span = new TextSpan(Math.Min(exception.Offset, text.Length), 0);

                production.ReportDiagnostic(Diagnostic.Create(GeneratorDiagnostics.GenerationError, Location.Create(exception.Input.Path, span, text.Lines.GetLinePositionSpan(span)), exception.Message));
            }
        });
    }

}
