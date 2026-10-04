using FsCheck;
using FsCheck.Fluent;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Wireloom.Dds.Generator.Tests;

public sealed class FsCheckIdlCompilerFuzzingSpecs
{
    [Fact]
    [Trait("Category", "Fuzzing")]
    public void MutatedIdlTextDoesNotEscapeThePublicCompilerErrorBoundary()
    {
        // Arrange
        var property = Prop.ForAll(
            Arb.From(StructuredInputGenerator),
            DoesNotEscapePublicCompilerErrorBoundary);
        var configuration = Config.QuickThrowOnFailure.WithMaxTest(GetMaxTestCases());

        // Act
        Check.One(configuration, property);

        // Assert
        // Check.One throws when FsCheck finds a counterexample.
    }

    [Fact]
    [Trait("Category", "Fuzzing")]
    public void MutatedIdlTextAndBuildMetadataDoNotEscapeTheRoslynGeneratorBoundary()
    {
        // Arrange
        var property = Prop.ForAll(
            Arb.From(RoslynInputGenerator),
            DoesNotEscapeRoslynGeneratorBoundary);
        var configuration = Config.QuickThrowOnFailure.WithMaxTest(GetMaxTestCases());

        // Act
        Check.One(configuration, property);

        // Assert
        // Check.One throws when FsCheck finds a counterexample.
    }

    private static bool DoesNotEscapePublicCompilerErrorBoundary(string source)
    {
        try
        {
            IdlCompiler.CompileSources(
                [new IdlInput("fscheck.idl", source, strict: source.Length % 2 == 0)],
                CancellationToken.None);
        }
        catch (IdlException)
        {
            // Expected for malformed or unsupported IDL.
        }

        return true;
    }

    private static bool DoesNotEscapeRoslynGeneratorBoundary(RoslynInput input)
    {
        var parseOptions = new CSharpParseOptions(LanguageVersion.CSharp12);
        var compilation = CSharpCompilation.Create(
            "FsCheckGeneratorInput",
            [CSharpSyntaxTree.ParseText("namespace Input; public sealed class Marker { }", parseOptions)],
            RuntimeReferences(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var driver = CSharpGeneratorDriver.Create(
            [new Wireloom.Generator().AsSourceGenerator()],
            [new TestAdditionalText("fscheck.idl", input.Source)],
            parseOptions,
            new TestAnalyzerConfigOptionsProvider(input.Metadata));

        var runDriver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var output, out _);
        var runResult = runDriver.GetRunResult();
        return runResult.Results.All(result => result.Exception is null)
            && output.GetDiagnostics().All(diagnostic => diagnostic.Severity != DiagnosticSeverity.Error);
    }

    private static Gen<string> BuildMutationGenerator(Gen<string> source) =>
        source.SelectMany(text =>
            Gen.Choose(0, 2).SelectMany(operation =>
                Gen.Choose(0, text.Length).SelectMany(position =>
                    Gen.Elements(MutationCharacters).Select(character =>
                        Mutate(text, operation, position, character)))));

    private static string Mutate(string text, int operation, int position, char character)
    {
        if (operation == 0 && text.Length > 0)
        {
            return text.Remove(Math.Min(position, text.Length - 1), 1);
        }

        if (operation == 1)
        {
            return text.Insert(Math.Min(position, text.Length), character.ToString());
        }

        if (text.Length == 0)
        {
            return character.ToString();
        }

        if (position == text.Length)
        {
            return text + character;
        }

        return text[..position] + character + text[(position + 1)..];
    }

    private static int GetMaxTestCases() =>
        int.TryParse(Environment.GetEnvironmentVariable("WIRELOOM_FSCHECK_TESTS"), out var testCases)
            ? Math.Clamp(testCases, 1, 100_000)
            : 1_000;

    private static Gen<RoslynInput> RoslynInputGenerator =>
        StructuredInputGenerator.Select(source => new RoslynInput(
            source,
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Generate"] = "true",
                ["Strict"] = source.Length % 2 == 0 ? "true" : "false",
                ["Defines"] = source.Length % 3 == 0 ? "FEATURE_A; FEATURE_B" : string.Empty,
                ["Undefines"] = source.Length % 5 == 0 ? "FEATURE_C" : string.Empty,
                ["IncludeDirectories"] = source.Length % 7 == 0 ? "idl;idl|includes" : string.Empty
            }));

    private static Gen<string> StructuredInputGenerator =>
        Gen.Frequency(
            (1, Gen.Elements(Seeds)),
            (8, BuildMutationGenerator(Gen.Elements(Seeds))),
            (4, BuildMutationGenerator(BuildMutationGenerator(Gen.Elements(Seeds)))));

    private static readonly char[] MutationCharacters =
    [
        ' ', '\t', '\n', '\r', ';', ',', ':', '.', '#', '@', '(', ')', '[', ']', '{', '}', '<', '>',
        '+', '-', '/', '*', '=', '&', '|', '!', '?', '0', '1', '7', '9', '_', 'a', 'A', 'Z', '"', '\'', '\\'
    ];

    private static readonly string[] Seeds =
    [
        "module Sample { struct Value { long value; }; };",
        "module Sample { struct Value { boolean flag; char letter; wchar wide; float ratio; double precision; }; };",
        "module Sample { struct Value { sequence<long, 4> values; string<16> name; long numbers[2]; }; };",
        "module Sample { typedef sequence<long, 4> Values; struct Value { Values values; }; };",
        "module Outer { module Inner { struct Value { long value; }; }; };",
        "module Sample { enum State { Idle, Running, Stopped }; struct Value { State state; }; };",
        "module Sample { enum State { Idle, Running, Stopped }; union Choice switch(State) { case Idle: long value; default: string<16> text; }; };",
        "module Sample { union Choice switch(long) { case 0: long value; case 1: string<16> text; default: boolean flag; }; };",
        """
#define VALUE 7
#if VALUE
const long Constant = VALUE;
#else
const long Other = 1;
#endif
""",
        """
#define ADD(a, b) a + b
const long Constant = ADD(1, 2);
""",
        "@appendable struct Value { long value; };",
        "struct Value { @key long id; @optional string<32> name; };",
        "struct Value { @id(3) long value; @hashid long hashed; };",
        "module Sample { struct Base { long base; }; struct Value : Base { long value; }; };",
        "module Sample { struct Value { sequence<string<16>, 4> values; }; };"
    ];

    private static readonly Lazy<IReadOnlyList<MetadataReference>> RuntimeReferenceCache = new(CreateRuntimeReferences);

    private static IReadOnlyList<MetadataReference> RuntimeReferences() => RuntimeReferenceCache.Value;

    private static IReadOnlyList<MetadataReference> CreateRuntimeReferences()
    {
        var paths = (string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") ?? string.Empty;
        var runtime = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "Rti.ConnextDds.dll"));
        var references = paths
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Where(path => !string.Equals(Path.GetFullPath(path), runtime, StringComparison.OrdinalIgnoreCase))
            .Select(path => MetadataReference.CreateFromFile(path))
            .ToList();

        File.Exists(runtime).ShouldBeTrue($"Expected RTI runtime at {runtime}");
        references.Add(MetadataReference.CreateFromFile(runtime));

        return references;
    }

    private sealed record RoslynInput(string Source, IReadOnlyDictionary<string, string> Metadata);

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
            const string prefix = "build_metadata.AdditionalFiles.";
            var metadataName = key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                ? key[prefix.Length..]
                : key;

            return values.TryGetValue(metadataName, out value!);
        }
    }
}
