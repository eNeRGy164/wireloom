namespace Wireloom.Compiler.Tests;

internal static class CompilerTestSupport
{
    public static IdlInput Input(string name, string content, bool generate = true) =>
        new(name, content, generate);

    public static string Compile(params List<IdlInput> inputs) =>
        string.Concat(CompileSources(inputs).Values.Select(s => s.Source));

    public static Dictionary<string, GeneratedIdlSource> CompileSources(params List<IdlInput> inputs) =>
        CompileSourcesWithDiagnostics(inputs, null, TestContext.Current.CancellationToken,
            Emission.RtiApiProfile.Rtiddsgen470);

    public static Dictionary<string, GeneratedIdlSource> CompileSources(
        IReadOnlyList<IdlInput> inputs,
        CancellationToken cancellationToken) =>
        CompileSourcesWithDiagnostics(inputs, null, cancellationToken,
            Emission.RtiApiProfile.Rtiddsgen470);

    public static Dictionary<string, GeneratedIdlSource> CompileSourcesWithDiagnostics(
        IReadOnlyList<IdlInput> inputs,
        ICollection<IdlDiagnostic>? diagnostics,
        CancellationToken cancellationToken,
        Emission.RtiApiProfile? profile = null)
    {
        var selectedProfile = profile == Emission.RtiApiProfile.Rtiddsgen431
            ? IdlCompilerProfile.Rti731
            : IdlCompilerProfile.Rti770;
        var result = IdlCompiler.Compile(inputs, selectedProfile, cancellationToken);
        var fatal = result.Diagnostics.FirstOrDefault(diagnostic =>
            diagnostic is { Severity: IdlDiagnosticSeverity.Error, Id: "DDSG0001" });
        if (diagnostics is not null)
        {
            foreach (var diagnostic in result.Diagnostics.Where(diagnostic =>
                         diagnostic.Severity != IdlDiagnosticSeverity.Error || diagnostic.Id != "DDSG0001"))
            {
                diagnostics.Add(diagnostic);
            }
        }

        if (fatal is not null)
        {
            throw new IdlException(fatal.Input, fatal.Offset, fatal.Message);
        }

        return result.Documents.ToDictionary(source => source.HintName, StringComparer.Ordinal);
    }
}
