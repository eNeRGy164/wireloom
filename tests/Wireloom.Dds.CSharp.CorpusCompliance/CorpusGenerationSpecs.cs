using System.Security.Cryptography;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Wireloom.Dds.CSharp.Compiler;
using Wireloom.Dds.Idl;

namespace Wireloom.Dds.CSharp.CorpusCompliance;

/// <summary>Checks generation, exact-runtime compilation, and source export for the selected corpus profile.</summary>
[Trait("CorpusSuite", "Generation")]
public sealed class CorpusGenerationSpecs
{
    private static readonly IReadOnlyList<MetadataReference> RuntimeReferences = CreateRuntimeReferences();

    /// <summary>Gets every inventory entry, including negative probes and integration roots.</summary>
    public static IEnumerable<object[]> CorpusCases() => CorpusRepository.Cases
        .Select(corpusCase => new object[] { corpusCase });

    /// <summary>Requires expected generation results and compilation against the selected exact runtime.</summary>
    [Theory]
    [MemberData(nameof(CorpusCases))]
    public void ChecksGenerationAgainstItsRuntime(CorpusCase corpusCase)
    {
        // Arrange
        var profile = CorpusProfile.Selected;
        var inputs = CorpusRepository.BuildInputs(corpusCase);

        // Act
        var result = IdlCompiler.Compile(inputs, profile.CompilerProfile, MD5.HashData, TestContext.Current.CancellationToken);

        // Assert
        if (!corpusCase.ExpectedToCompile)
        {
            result.Success.ShouldBeFalse($"{profile.Version}: {corpusCase.Id} should be rejected.");
            result.Documents.ShouldBeEmpty();

            if (profile.UnsupportedCases.TryGetValue(corpusCase.Id, out var feature))
            {
                result.Diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0004"
                    && diagnostic.Message.Contains(feature, StringComparison.Ordinal));
            }

            return;
        }

        result.Success.ShouldBeTrue(string.Join(Environment.NewLine, result.Diagnostics.Select(diagnostic => diagnostic.Message)));
        AssertCompiles(corpusCase, result);
    }

    /// <summary>Exports accepted sources after compiling each case against the selected exact runtime.</summary>
    [Fact]
    public void ExportAcceptedCorpusSources()
    {
        // Arrange
        var exportRoot = Environment.GetEnvironmentVariable("CORPUS_GENERATOR_EXPORT_ROOT");
        if (string.IsNullOrWhiteSpace(exportRoot))
        {
            Assert.Skip("Set CORPUS_GENERATOR_EXPORT_ROOT to export sources.");
        }

        var profile = CorpusProfile.Selected;
        var root = Path.GetFullPath(exportRoot, CorpusRepository.RepositoryRoot);

        // Act
        foreach (var corpusCase in CorpusRepository.Cases.Where(corpusCase => corpusCase.ExpectedToCompile))
        {
            var result = IdlCompiler.Compile(CorpusRepository.BuildInputs(corpusCase), profile.CompilerProfile,
                MD5.HashData, TestContext.Current.CancellationToken);

            // Assert
            result.Success.ShouldBeTrue(string.Join(Environment.NewLine, result.Diagnostics.Select(diagnostic => diagnostic.Message)));
            AssertCompiles(corpusCase, result);

            if (result.Documents.Count == 0)
            {
                continue;
            }

            var caseRoot = Path.Combine(root, corpusCase.SourceKind, corpusCase.Id);
            Directory.CreateDirectory(caseRoot);

            foreach (var source in result.Documents)
            {
                var fileName = source.HintName.EndsWith(".g.cs", StringComparison.Ordinal)
                    ? source.HintName
                    : $"{source.HintName}.g.cs";

                var outputPath = Path.Combine(caseRoot, fileName);
                if (File.Exists(outputPath)
                    && string.Equals(File.ReadAllText(outputPath), source.Source, StringComparison.Ordinal))
                {
                    continue;
                }

                File.WriteAllText(outputPath, source.Source);
            }
        }
    }

    /// <summary>Requires successful C# 12 compilation against the selected exact runtime.</summary>
    private static void AssertCompiles(CorpusCase corpusCase, IdlCompilationResult<GeneratedIdlSource> result)
    {
        var trees = result.Documents.Select(document => CSharpSyntaxTree.ParseText(document.Source,
            new CSharpParseOptions(LanguageVersion.CSharp12), document.HintName));
        var compilation = CSharpCompilation.Create("Corpus_" + corpusCase.Id.Replace('-', '_'), trees,
            RuntimeReferences, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
        using var assembly = new MemoryStream();
        var emitted = compilation.Emit(assembly, cancellationToken: TestContext.Current.CancellationToken);
        emitted.Success.ShouldBeTrue(string.Join(Environment.NewLine, emitted.Diagnostics));
    }

    /// <summary>Loads the selected exact RTI assembly as a compilation metadata reference.</summary>
    private static IReadOnlyList<MetadataReference> CreateRuntimeReferences()
    {
        var platformPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator);
        var runtimePath = Path.Combine(AppContext.BaseDirectory, "RuntimeReferences", CorpusProfile.Selected.Version, "Rti.ConnextDds.dll");
        return platformPaths.Select(path => MetadataReference.CreateFromFile(path))
            .Append(MetadataReference.CreateFromFile(runtimePath)).ToArray();
    }
}
