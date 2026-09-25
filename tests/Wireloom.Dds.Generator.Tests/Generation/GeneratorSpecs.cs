using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Wireloom.Dds.Generator.Tests;

public sealed class GeneratorSpecs
{
    [Fact]
    public void GeneratesSourcesWhenRuntimeAndMetadataAreAvailable()
    {
        var result = Run("module Sample { struct Value { long value; }; };", LanguageVersion.CSharp12,
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Generate"] = "true",
                ["Strict"] = "true",
                ["Defines"] = "FEATURE_A; FEATURE_B",
                ["Undefines"] = "FEATURE_C",
                ["IncludeDirectories"] = "idl;idl/includes"
            }, includeRuntime: true);

        result.Diagnostics.ShouldBeEmpty();
        result.Output.SyntaxTrees.Any(tree => tree.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeTrue();
        result.Output.SyntaxTrees.Count().ShouldBeGreaterThan(1);
    }

    [Fact]
    public void ReportsMissingRuntimeReferenceBeforeParsingIdl()
    {
        var result = Run("module Sample { struct Value { long value; }; };", LanguageVersion.CSharp12,
            new Dictionary<string, string>(), includeRuntime: false);

        result.Diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0003");
        result.Output.SyntaxTrees.Any(tree => tree.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void ReportsUnsupportedLanguageVersion()
    {
        var result = Run("module Sample { struct Value { long value; }; };", LanguageVersion.CSharp11,
            new Dictionary<string, string>(), includeRuntime: true);

        result.Diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0002");
        result.Output.SyntaxTrees.Any(tree => tree.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void ReportsInvalidIdlWithTheAdditionalFileLocation()
    {
        var result = Run("module Sample { struct Value { long value; }", LanguageVersion.CSharp12,
            new Dictionary<string, string>(), includeRuntime: true);

        var diagnostic = result.Diagnostics.Single(diagnostic => diagnostic.Id == "DDSG0001");
        diagnostic.GetMessage().ShouldNotBeNullOrWhiteSpace();
        diagnostic.Location.GetLineSpan().Path.ShouldBe("sample.idl");
    }

    [Fact]
    public void HonorsGenerateFalseAdditionalFileMetadata()
    {
        var result = Run("module Sample { struct Value { long value; }; };", LanguageVersion.CSharp12,
            new Dictionary<string, string> { ["Generate"] = "false" }, includeRuntime: true);

        result.Diagnostics.ShouldBeEmpty();
        result.Output.SyntaxTrees.Any(tree => tree.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeFalse();
    }

    private static GeneratorRunResult Run(string idl, LanguageVersion languageVersion,
        IReadOnlyDictionary<string, string> metadata, bool includeRuntime)
    {
        var parseOptions = new CSharpParseOptions(languageVersion);
        var compilation = CSharpCompilation.Create(
            "GeneratorInput",
            [CSharpSyntaxTree.ParseText("namespace Input; public sealed class Marker { }", parseOptions)],
            includeRuntime ? RuntimeReferences() : FrameworkReferences(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver.Create(
            [new global::Wireloom.Generator().AsSourceGenerator()],
            [new TestAdditionalText("sample.idl", idl)],
            parseOptions,
            new TestAnalyzerConfigOptionsProvider(metadata));

        driver.RunGeneratorsAndUpdateCompilation(compilation, out var output, out var generatorDiagnostics);
        var runDiagnostics = driver.GetRunResult().Results
            .SelectMany(result => result.Diagnostics.IsDefault ? [] : result.Diagnostics)
            .ToImmutableArray();
        return new GeneratorRunResult(output, generatorDiagnostics
            .Concat(runDiagnostics)
            .Concat(output.GetDiagnostics().Where(diagnostic => diagnostic.Id.StartsWith("DDSG", StringComparison.Ordinal)))
            .ToImmutableArray());
    }

    private static IEnumerable<MetadataReference> FrameworkReferences()
    {
        var paths = (string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") ?? string.Empty;
        return paths.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Where(path => !string.Equals(Path.GetFileName(path), "Rti.ConnextDds.dll", StringComparison.OrdinalIgnoreCase))
            .Select(path => MetadataReference.CreateFromFile(path));
    }

    private static IEnumerable<MetadataReference> RuntimeReferences()
    {
        var references = FrameworkReferences().ToList();
        var runtime = Path.Combine(AppContext.BaseDirectory, "Rti.ConnextDds.dll");
        File.Exists(runtime).ShouldBeTrue($"Expected RTI runtime at {runtime}");
        references.Add(MetadataReference.CreateFromFile(runtime));
        return references;
    }

    private sealed record GeneratorRunResult(Compilation Output, ImmutableArray<Diagnostic> Diagnostics);

    private sealed class TestAdditionalText(string path, string text) : AdditionalText
    {
        public override string Path { get; } = path;

        public override SourceText? GetText(CancellationToken cancellationToken = default) => SourceText.From(text);
    }

    private sealed class TestAnalyzerConfigOptionsProvider(IReadOnlyDictionary<string, string> values) : AnalyzerConfigOptionsProvider
    {
        private readonly TestAnalyzerConfigOptions options = new(values);

        public override AnalyzerConfigOptions GlobalOptions => options;

        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => options;

        public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) => options;
    }

    private sealed class TestAnalyzerConfigOptions(IReadOnlyDictionary<string, string> values) : AnalyzerConfigOptions
    {
        public override bool TryGetValue(string key, out string value)
        {
            var metadataName = key.StartsWith("build_metadata.AdditionalFiles.", StringComparison.OrdinalIgnoreCase)
                ? key["build_metadata.AdditionalFiles.".Length..]
                : key;
            return values.TryGetValue(metadataName, out value!);
        }
    }
}
