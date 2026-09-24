using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Wireloom;

/// <summary>
/// Generates C# source from <c>DdsIdl</c> roots and their tracked include
/// closures.
/// </summary>
[Generator]
public sealed class Generator : IIncrementalGenerator
{
    private static readonly DiagnosticDescriptor GenerationError = new(
        "DDSG0001",
        "IDL generation failed",
        "{0}",
        "DDS Source Generator",
        DiagnosticSeverity.Error,
        true);

    private static readonly DiagnosticDescriptor LanguageVersionError = new(
        "DDSG0002",
        "C# 12 or later is required",
        "The DDS source generator requires C# 12 or later; the project uses C# {0}",
        "DDS Source Generator",
        DiagnosticSeverity.Error,
        true);

    private static readonly DiagnosticDescriptor RuntimeReferenceError = new(
        "DDSG0003",
        "Compatible RTI runtime not found",
        "A resolved Rti.ConnextDds reference with version 7.7.0 or later is required",
        "DDS Source Generator",
        DiagnosticSeverity.Error,
        true);

    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var inputs = context.AdditionalTextsProvider
            .Where(file => file.Path.EndsWith(".idl", StringComparison.OrdinalIgnoreCase))
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Select(CreateInput)
            .Collect()
            .Combine(context.CompilationProvider);

        context.RegisterSourceOutput(inputs, (production, compilationAndInputs) =>
        {
            var files = compilationAndInputs.Left;
            var compilation = compilationAndInputs.Right;

            if (compilation is CSharpCompilation csharpCompilation && csharpCompilation.LanguageVersion < LanguageVersion.CSharp12)
            {
                production.ReportDiagnostic(Diagnostic.Create(LanguageVersionError, Location.None, csharpCompilation.LanguageVersion));

                return;
            }

            if (!HasCompatibleRuntime(compilation))
            {
                production.ReportDiagnostic(Diagnostic.Create(RuntimeReferenceError, Location.None));
                return;
            }

            try
            {
                var outputs = IdlCompiler.CompileSources([.. files], production.CancellationToken);
                foreach (var output in outputs.Values)
                {
                    production.AddSource(output.HintName, SourceText.From(output.Source, Encoding.UTF8));
                }
            }
            catch (IdlException exception)
            {
                var text = SourceText.From(exception.Input.Text);
                var span = new TextSpan(Math.Min(exception.Offset, text.Length), 0);

                production.ReportDiagnostic(Diagnostic.Create(GenerationError, Location.Create(exception.Input.Path, span, text.Lines.GetLinePositionSpan(span)), exception.Message));
            }
        });
    }

    private static IdlInput CreateInput((AdditionalText Left, AnalyzerConfigOptionsProvider Right) inputAndOptions, CancellationToken cancellationToken)
    {
        var input = inputAndOptions.Left;

        inputAndOptions.Right
            .GetOptions(input)
            .TryGetValue("build_metadata.AdditionalFiles.Generate", out var generate);

        inputAndOptions.Right
            .GetOptions(input)
            .TryGetValue("build_metadata.AdditionalFiles.Strict", out var strict);

        inputAndOptions.Right
            .GetOptions(input)
            .TryGetValue("build_metadata.AdditionalFiles.Defines", out var defines);

        inputAndOptions.Right
            .GetOptions(input)
            .TryGetValue("build_metadata.AdditionalFiles.Undefines", out var undefines);

        inputAndOptions.Right
            .GetOptions(input)
            .TryGetValue("build_metadata.AdditionalFiles.IncludeDirectories", out var includeDirectories);

        return new(
            input.Path,
            input.GetText(cancellationToken)?.ToString() ?? string.Empty,
            !string.Equals(generate, "false", StringComparison.OrdinalIgnoreCase),
            string.Equals(strict, "true", StringComparison.OrdinalIgnoreCase),
            ParseSymbols(defines),
            ParseSymbols(undefines),
            ParseSymbols(includeDirectories));
    }

    private static List<string> ParseSymbols(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? []
            : [.. value!.Split([';'], StringSplitOptions.RemoveEmptyEntries)
                .Select(symbol => symbol.Trim())
                .Where(symbol => symbol.Length != 0)];

    private static bool HasCompatibleRuntime(Compilation compilation)
    {
        foreach (var reference in compilation.References)
        {
            var assembly = compilation.GetAssemblyOrModuleSymbol(reference) as IAssemblySymbol;
            if (assembly?.Identity.Name == "Rti.ConnextDds" && assembly.Identity.Version >= new Version(7, 7, 0))
            {
                return true;
            }
        }

        return false;
    }
}
