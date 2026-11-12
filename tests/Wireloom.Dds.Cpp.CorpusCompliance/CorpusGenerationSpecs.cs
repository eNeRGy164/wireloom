using System.Security.Cryptography;
using Wireloom.Dds.Idl;

namespace Wireloom.Dds.Cpp.CorpusCompliance;

/// <summary>Checks the existing supported Modern C++ generation corpus independently of CLI I/O.</summary>
[Trait("CorpusSuite", "Generation")]
public sealed class CorpusGenerationSpecs
{
    /// <summary>Generates the selected supported corpus inputs and existing keyed regression.</summary>
    [Theory]
    [InlineData("01-primitives")]
    [InlineData("01-boundaries")]
    [InlineData("01-int8-boundaries")]
    [InlineData("01-full-widths")]
    [InlineData("02-names-constants")]
    [InlineData("02-formatting")]
    [InlineData("02-multiple")]
    [InlineData("02-scopes")]
    [InlineData("02-constant-expressions")]
    [InlineData("03-enum-values-explicit")]
    [InlineData("03-enum-values-prefix")]
    [InlineData("03-alias-aggregate")]
    [InlineData("03-enums-aliases")]
    [InlineData("03-alias-collections")]
    [InlineData("04-strings")]
    [InlineData("04-boundaries")]
    [InlineData("04-unbounded-string-aliases")]
    [InlineData("04-string-alias-chains")]
    [InlineData("05-collections")]
    [InlineData("05-shapes")]
    [InlineData("05-array-of-sequences")]
    [InlineData("05-array-of-sequences-idl")]
    [InlineData("11-conditionals")]
    [InlineData("11-macros")]
    [InlineData("11-macro-operators")]
    [InlineData("11-preprocessor-advanced")]
    [InlineData("10-annotations")]
    [InlineData("06-aggregates")]
    [InlineData("06-valuetypes")]
    [InlineData("06-alias-composition")]
    [InlineData("07-unions")]
    [InlineData("07-union-enum")]
    [InlineData("07-union-scoped")]
    [InlineData("07-union-multilabel")]
    [InlineData("07-union-short")]
    [InlineData("07-union-octet")]
    [InlineData("07-union-ushort")]
    [InlineData("07-union-ulong")]
    [InlineData("07-union-int8")]
    [InlineData("07-union-uint8")]
    [InlineData("07-union-int16")]
    [InlineData("07-union-uint16")]
    [InlineData("07-union-int32")]
    [InlineData("07-union-uint32")]
    [InlineData("07-union-long")]
    [InlineData("07-union-string-sequence-bounded-direct")]
    [InlineData("07-union-string-sequence-aliases")]
    [InlineData("07-union-char")]
    [InlineData("07-union-char-boundaries")]
    [InlineData("07-union-boolean")]
    [InlineData("07-union-aliases")]
    [InlineData("08-key-nested")]
    [InlineData("08-key-inherited")]
    [InlineData("08-key-boundaries")]
    [InlineData("08-key-union")]
    [InlineData("09-extensibility")]
    [InlineData("10-default-literal-enum-array")]
    [InlineData("10-resolve-name-wstring-aliases")]
    [InlineData("14-extensibility-full")]
    [InlineData("14-annotation-arguments")]
    [InlineData("14-member-declarators")]
    [InlineData("14-union-label-expressions")]
    [InlineData("14-enum-constant")]
    [InlineData("09-evolution-optional")]
    [InlineData("09-optional-collections")]
    [InlineData("09-optional-aggregate-member")]
    [InlineData("09-optional-string-sequences")]
    [InlineData("09-id-gaps")]
    public void GeneratesExistingCorpusInput(string name)
    {
        // Arrange
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", name + ".idl"));

        // Act
        var result = CppCompiler.Compile(
            new IdlCompilationRequest([new IdlInput(name + ".idl", source)], MD5.HashData),
            CppCompilerProfile.Rti770);

        // Assert
        result.Success.ShouldBeTrue(string.Join(Environment.NewLine, result.Diagnostics.Select(diagnostic => diagnostic.Message)));
        result.Diagnostics.ShouldBeEmpty();
        result.Documents.Select(document => document.FileName).Order()
            .ShouldBe(new[] { name + ".cxx", name + ".hpp", name + "Plugin.cxx", name + "Plugin.hpp" }.Order());
        foreach (var document in result.Documents)
        {
            var lines = document.Source.Split('\n');
            document.Source.ShouldNotContain("\r");
            lines.Where(line => !IsLongDoxygenSeeUrl(line)).Max(line => line.Length).ShouldBeLessThanOrEqualTo(120);
            lines.Where(IsLongDoxygenSeeUrl).ShouldAllBe(line => line.Length <= 320);
        }
    }

    private static bool IsLongDoxygenSeeUrl(string line) =>
        line.TrimStart().StartsWith("/// @see [", StringComparison.Ordinal)
        && line.Contains("https://community.rti.com/static/documentation/connext-dds/", StringComparison.Ordinal);
}
