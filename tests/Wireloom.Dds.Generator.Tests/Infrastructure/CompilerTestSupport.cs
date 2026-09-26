namespace Wireloom.Dds.Generator.Tests;

internal static class CompilerTestSupport
{
    public static IdlInput Input(string name, string content, bool generate = true) =>
        new(name, content, generate);

    public static string Compile(params List<IdlInput> inputs) =>
        IdlCompiler.Compile(inputs, TestContext.Current.CancellationToken);

    public static Dictionary<string, GeneratedIdlSource> CompileSources(params List<IdlInput> inputs) =>
        IdlCompiler.CompileSources(inputs, TestContext.Current.CancellationToken);
}
