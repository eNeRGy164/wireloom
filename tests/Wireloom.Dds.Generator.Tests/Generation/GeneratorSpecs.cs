using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Wireloom.Generation.Tests;

public sealed class GeneratorSpecs
{
    [Fact]
    public void GeneratesSourcesWhenRuntimeAndMetadataAreAvailable()
    {
        // Arrange
        var input = """module Sample { struct Value { long value; }; };""";
        var metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Generate"] = "true",
            ["Strict"] = "true",
            ["Defines"] = "FEATURE_A; FEATURE_B",
            ["Undefines"] = "FEATURE_C",
            ["IncludeDirectories"] = "idl;idl/includes"
        };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeTrue();
        result.Output.SyntaxTrees.Count().ShouldBeGreaterThan(1);
    }

    [Fact]
    public void SkipsPrerequisiteChecksWhenNoIdlRootIsMarkedForGeneration()
    {
        // Arrange
        var input = "module Sample { struct Value { long value; }; };";
        var metadata = new Dictionary<string, string>();

        // Act
        var result = Run(input, LanguageVersion.CSharp11, metadata, includeRuntime: false);

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void CompilesBooleanUnionWithNonScalarBranch()
    {
        // Arrange
        var input =
            """
            module BooleanUnion {
                union Choice switch(boolean) {
                    case TRUE: long enabled;
                    case FALSE: sequence<long, 2> disabled;
                };
            };
            """;
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ShouldBeEmpty();
    }

    [Fact]
    public void ReportsMissingRuntimeReferenceBeforeParsingIdl()
    {
        // Arrange
        var input = """module Sample { struct Value { long value; }; };""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: false);

        // Assert
        result.Diagnostics.ShouldContain(d => d.Id == "DDSG0003");
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void ReportsUnsupportedLanguageVersion()
    {
        // Arrange
        var input = """module Sample { struct Value { long value; }; };""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp11, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldContain(d => d.Id == "DDSG0002");
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void ReportsInvalidIdlWithTheAdditionalFileLocation()
    {
        // Arrange
        var input = """module Sample { struct Value { long value; }""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0001");
        diagnostic.GetMessage().ShouldNotBeNullOrWhiteSpace();
        var lineSpan = diagnostic.Location.GetLineSpan();
        lineSpan.Path.ShouldBe("sample.idl");
        lineSpan.StartLinePosition.Line.ShouldBe(0);
    }

    [Fact]
    public void ReportsUnknownAnnotationWarningAndContinuesGeneration()
    {
        // Arrange
        var input = """
        module Sample {
            @custom_unknown
            struct Value { long value; };
        };
        """;
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0101");
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
        diagnostic.GetMessage().ShouldBe("Annotation '@custom_unknown' is not recognized and will be ignored.");
        var lineSpan = diagnostic.Location.GetLineSpan();
        lineSpan.Path.ShouldBe("sample.idl");
        lineSpan.StartLinePosition.Line.ShouldBe(1);
        lineSpan.StartLinePosition.Character.ShouldBe(4);
        diagnostic.Location.SourceSpan.Start.ShouldBe(input.IndexOf("@custom_unknown", StringComparison.Ordinal));

        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeTrue();
    }

    [Fact]
    public void ReportsUnsupportedAnnotationWarningBeforeContextualError()
    {
        // Arrange
        var input = """module Sample { @position(1) struct Value { long value; }; };""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0102");
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
        diagnostic.GetMessage().ShouldBe("Annotation 'position' is recognized but unsupported and will be ignored.");
        diagnostic.Location.GetLineSpan().Path.ShouldBe("sample.idl");

        result.Diagnostics.ShouldContain(d => d.Id == "DDSG0001");
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void ReportsAndIgnoresNonDdsInterface()
    {
        // Arrange
        var input = """module Sample { interface Service { void ping(); }; };""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0103");
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
        diagnostic.GetMessage().ShouldBe("The interface 'Service' is ignored because it is not a DDS service.");
        diagnostic.Location.GetLineSpan().Path.ShouldBe("sample.idl");

        result.Diagnostics.ShouldNotContain(d => d.Id == "DDSG0001");
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Service", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void ReportsMacroArityWarningAndContinuesExpansion()
    {
        // Arrange
        var input = """
        #define CORPUS_PAIR(a, b) a
        const long Constant = CORPUS_PAIR(1);
        """;
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0104");
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
        diagnostic.GetMessage().ShouldBe("Function-like macro 'CORPUS_PAIR' was invoked with the wrong number of arguments; expansion will continue.");
        diagnostic.Location.GetLineSpan().Path.ShouldBe("sample.idl");

        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("public const int Value = 1;", StringComparison.Ordinal)).ShouldBeTrue();
    }

    [Fact]
    [Trait("Preprocessor", "PP040")]
    public void ReportsPreprocessorWarningThroughTheRoslynAdapter()
    {
        // Arrange
        const string input = """
        #warning prefer the supported IDL form
        const long Value = 1;
        """;
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0106");
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
        diagnostic.GetMessage().ShouldBe("Preprocessor warning: prefer the supported IDL form");
        diagnostic.Location.GetLineSpan().Path.ShouldBe("sample.idl");
    }

    [Fact]
    [Trait("Preprocessor", "PP042")]
    public void ReportsPreprocessorMessageThroughTheRoslynAdapter()
    {
        // Arrange
        const string input = """
        #pragma message("build note")
        const long Value = 1;
        """;
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0107");
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Info);
        diagnostic.GetMessage().ShouldBe("Preprocessor message: message(\"build note\")");
        diagnostic.Location.GetLineSpan().Path.ShouldBe("sample.idl");
    }

    [Fact]
    public void ReportsDirectArrayOfSequencesWarning()
    {
        // Arrange
        var input = """module Sample { struct Value { sequence<long> values[2]; sequence<string<16>, 3> names[2]; }; };""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.Count(d => d.Id == "DDSG0105").ShouldBe(2);
        result.Diagnostics.ShouldAllBe(d => d.Id != "DDSG0001");
        result.Diagnostics.Where(d => d.Id == "DDSG0105")
            .ShouldAllBe(d => d.Severity == DiagnosticSeverity.Warning && d.Location.GetLineSpan().Path == "sample.idl");
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeTrue();
    }

    [Fact]
    public void HonorsGenerateFalseAdditionalFileMetadata()
    {
        // Arrange
        var input = """module Sample { struct Value { long value; }; };""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "false" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeFalse();
    }

    private static GeneratorRunResult Run(string input, LanguageVersion languageVersion, IReadOnlyDictionary<string, string> metadata, bool includeRuntime)
    {
        var parseOptions = new CSharpParseOptions(languageVersion);
        var compilation = CSharpCompilation.Create(
            "GeneratorInput",
            [CSharpSyntaxTree.ParseText("namespace Input; public sealed class Marker { }", parseOptions)],
            includeRuntime ? RuntimeReferences() : FrameworkReferences(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver.Create(
            [new Generator().AsSourceGenerator()],
            [new TestAdditionalText("sample.idl", input)],
            parseOptions,
            new TestAnalyzerConfigOptionsProvider(metadata));

        driver.RunGeneratorsAndUpdateCompilation(compilation, out var output, out var generatorDiagnostics);
        var runDiagnostics = driver.GetRunResult().Results
            .SelectMany(result => result.Diagnostics.IsDefault ? [] : result.Diagnostics)
            .ToImmutableArray();

        return new GeneratorRunResult(output,
        [
            .. generatorDiagnostics,
            .. runDiagnostics,
            .. output.GetDiagnostics().Where(d => d.Id.StartsWith("DDSG", StringComparison.Ordinal)),
        ]);
    }

    private static IEnumerable<MetadataReference> FrameworkReferences()
    {
        var paths = (string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") ?? string.Empty;

        return paths
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
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
            string metadataName;

            if (key.StartsWith("build_metadata.AdditionalFiles.", StringComparison.OrdinalIgnoreCase))
            {
                metadataName = key["build_metadata.AdditionalFiles.".Length..];
            }
            else
            {
                metadataName = key;
            }

            return values.TryGetValue(metadataName, out value!);
        }
    }
}
