using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Wireloom.Roslyn;

/// <summary>Defines and reports diagnostics raised by the Roslyn generator adapter.</summary>
internal static class GeneratorDiagnostics
{
    internal static readonly DiagnosticDescriptor GenerationError = new("DDSG0001", "IDL generation failed", "{0}", "DDS Source Generator", DiagnosticSeverity.Error, true);
    internal static readonly DiagnosticDescriptor LanguageVersionError = new("DDSG0002", "C# 12 or later is required", "The DDS source generator requires C# 12 or later; the project uses C# {0}", "DDS Source Generator", DiagnosticSeverity.Error, true);
    internal static readonly DiagnosticDescriptor RuntimeReferenceError = new("DDSG0003", "Compatible RTI runtime not found", $"A resolved Rti.ConnextDds reference with version {RuntimeReferenceGuard.MinimumSupportedRtiVersion} or later is required", "DDS Source Generator", DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor unknownAnnotationWarning = new("DDSG0101", "Unknown IDL annotation", "{0}", "DDS Source Generator", DiagnosticSeverity.Warning, true);
    private static readonly DiagnosticDescriptor unsupportedAnnotationWarning = new("DDSG0102", "Unsupported IDL annotation", "{0}", "DDS Source Generator", DiagnosticSeverity.Warning, true);
    private static readonly DiagnosticDescriptor ignoredInterfaceWarning = new("DDSG0103", "IDL interface ignored", "{0}", "DDS Source Generator", DiagnosticSeverity.Warning, true);
    private static readonly DiagnosticDescriptor macroArityWarning = new("DDSG0104", "Macro argument count mismatch", "{0}", "DDS Source Generator", DiagnosticSeverity.Warning, true);
    private static readonly DiagnosticDescriptor arrayOfSequenceWarning = new("DDSG0105", "Array of sequences may not preserve IDL semantics", "{0}", "DDS Source Generator", DiagnosticSeverity.Warning, true);
    private static readonly DiagnosticDescriptor preprocessorWarning = new("DDSG0106", "Preprocessor warning", "{0}", "DDS Source Generator", DiagnosticSeverity.Warning, true);
    private static readonly DiagnosticDescriptor preprocessorMessage = new("DDSG0107", "Preprocessor message", "{0}", "DDS Source Generator", DiagnosticSeverity.Info, true);

    internal static void Report(SourceProductionContext production, IEnumerable<IdlDiagnostic> diagnostics)
    {
        foreach (var diagnostic in diagnostics)
        {
            var text = SourceText.From(diagnostic.Input.Text);
            var span = new TextSpan(Math.Min(diagnostic.Offset, text.Length), 0);
            var descriptor = diagnostic.Id switch
            {
                "DDSG0102" => unsupportedAnnotationWarning,
                "DDSG0103" => ignoredInterfaceWarning,
                "DDSG0104" => macroArityWarning,
                "DDSG0105" => arrayOfSequenceWarning,
                "DDSG0106" => preprocessorWarning,
                "DDSG0107" => preprocessorMessage,
                _ => unknownAnnotationWarning
            };

            production.ReportDiagnostic(Diagnostic.Create(descriptor, Location.Create(diagnostic.Input.Path, span, text.Lines.GetLinePositionSpan(span)), diagnostic.Message));
        }
    }
}
