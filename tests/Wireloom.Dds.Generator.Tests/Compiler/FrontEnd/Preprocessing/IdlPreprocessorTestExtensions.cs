namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

internal static class IdlPreprocessorTestExtensions
{
    public static string Process(
        this IdlPreprocessor preprocessor,
        IdlInput input,
        Action<string, bool, int> include,
        ICollection<IdlDiagnostic>? diagnosticSink = null) =>
        preprocessor.ProcessWithMetadata(input, include, diagnosticSink).Text;
}
