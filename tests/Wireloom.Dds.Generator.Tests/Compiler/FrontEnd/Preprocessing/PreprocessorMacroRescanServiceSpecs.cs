using System.Text;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

[Trait("Preprocessor", "PP008")]
[Trait("Preprocessor", "PP010")]
[Trait("Preprocessor", "PP020")]
[Trait("Preprocessor", "PP048")]
[Trait("Preprocessor", "PP050")]
[Trait("Preprocessor", "PP051")]
public sealed class PreprocessorMacroRescanServiceSpecs
{
    [Fact]
    public void MapsEachSuffixChunkToItsOriginalSourceOffset()
    {
        // Arrange
        var input = new IdlInput("rescan.idl", "");
        var sourceOffsets = Enumerable.Range(0, 32).Select(index => 100 + index).ToArray();
        var expansions = new List<(string Text, int Offset, int Depth, int[]? SourceOffsets)>();
        var service = CreateService(input, expansions);

        // Act
        var result = service.RescanReplacementAndSuffix(
            "FORWARD",
            "VALUE",
            ", __LINE__",
            [],
            depth: 2,
            tokenOffset: 30,
            suffixIndex: 4,
            sourceBaseOffset: 3,
            sourceOffsets);

        // Assert
        result.ShouldBe("VALUE, __LINE__");
        var suffixExpansions = expansions.Where(expansion => expansion.SourceOffsets is not null).ToArray();
        suffixExpansions.Select(expansion => expansion.Text).ShouldBe([", ", "__LINE__"]);
        suffixExpansions.Select(expansion => expansion.Offset).ShouldBe([7, 9]);
        foreach (var expansion in suffixExpansions)
        {
            expansion.SourceOffsets.ShouldBe(sourceOffsets);
        }
    }

    [Fact]
    public void ChecksOutputLengthAfterAddingTheExpandedSuffix()
    {
        // Arrange
        var input = new IdlInput("rescan.idl", "");
        var expansions = new List<(string Text, int Offset, int Depth, int[]? SourceOffsets)>();
        var checkedLengths = new List<int>();
        var service = CreateService(input, expansions, checkedLengths: checkedLengths);

        // Act
        var result = service.RescanReplacementAndSuffix(
            "FORWARD",
            "VALUE",
            ", NEXT",
            [],
            depth: 2,
            tokenOffset: 30,
            suffixIndex: 4,
            sourceBaseOffset: 3,
            sourceOffsets: null);

        // Assert
        result.ShouldBe("VALUE, NEXT");
        checkedLengths.ShouldBe([5, 11]);
    }

    [Fact]
    public void MapsSuffixChunksFromTheTokenOffsetWhenNoSourceMapExists()
    {
        // Arrange
        var input = new IdlInput("rescan.idl", "");
        var expansions = new List<(string Text, int Offset, int Depth, int[]? SourceOffsets)>();
        var service = CreateService(input, expansions);

        // Act
        var result = service.RescanReplacementAndSuffix(
            "FORWARD",
            "VALUE",
            ", __LINE__",
            [],
            depth: 2,
            tokenOffset: 30,
            suffixIndex: 4,
            sourceBaseOffset: 3,
            sourceOffsets: null);

        // Assert
        result.ShouldBe("VALUE, __LINE__");
        var suffixExpansions = expansions.Skip(1).ToArray();
        suffixExpansions.Select(expansion => expansion.Offset).ShouldBe([34, 36]);
        suffixExpansions.ShouldAllBe(expansion => expansion.SourceOffsets == null);
    }

    [Fact]
    public void BuildsCombinedOffsetsFromReplacementAndMappedInvocationSuffix()
    {
        // Arrange
        var input = new IdlInput("rescan.idl", "");
        var sourceOffsets = Enumerable.Range(0, 32).Select(index => 1000 + index * 2).ToArray();
        var expansions = new List<(string Text, int Offset, int Depth, int[]? SourceOffsets)>();
        var service = CreateService(input, expansions);

        // Act
        var result = service.RescanReplacementAndSuffix(
            "FORWARD",
            "CALL",
            "(x)",
            [],
            depth: 2,
            tokenOffset: 30,
            suffixIndex: 5,
            sourceBaseOffset: 2,
            sourceOffsets);

        // Assert
        result.ShouldBe("CALL(x)");
        var combinedExpansion = expansions.Single(expansion => expansion.SourceOffsets is not null);
        combinedExpansion.Text.ShouldBe("CALL(x)");
        combinedExpansion.Offset.ShouldBe(0);
        combinedExpansion.Depth.ShouldBe(3);
        combinedExpansion.SourceOffsets.ShouldBe(new[] { 30, 30, 30, 30, 1014, 1016, 1018 });
    }

    [Fact]
    public void BuildsCombinedOffsetsFromTokenPositionWithoutASourceMap()
    {
        // Arrange
        var input = new IdlInput("rescan.idl", "");
        var expansions = new List<(string Text, int Offset, int Depth, int[]? SourceOffsets)>();
        var service = CreateService(input, expansions);

        // Act
        var result = service.RescanReplacementAndSuffix(
            "FORWARD",
            "CALL",
            "(x)",
            [],
            depth: 2,
            tokenOffset: 30,
            suffixIndex: 5,
            sourceBaseOffset: 2,
            sourceOffsets: null);

        // Assert
        result.ShouldBe("CALL(x)");
        var combinedExpansion = expansions.Single(expansion => expansion.SourceOffsets is not null);
        combinedExpansion.SourceOffsets.ShouldBe(new[] { 30, 30, 30, 30, 35, 36, 37 });
    }

    [Fact]
    public void AllowsJoinedOutputAtTheLimitAndRejectsOneCharacterOver()
    {
        // Arrange
        var input = new IdlInput("rescan.idl", "");
        var suffix = "(x)";
        var service = new PreprocessorMacroRescanService(
            new PreprocessorMacroTable(),
            new PreprocessorMacroTokenService(),
            CancellationToken.None,
            () => input,
            (text, _, _, _, _) => text,
            (output, text, _, _) => output.Append(text),
            (_, _) => { });
        var maximumReplacement = new string('A', PreprocessorLimits.MaximumOutputLength - suffix.Length);
        var oversizedReplacement = new string('A', maximumReplacement.Length + 1);

        // Act
        var maximumResult = service.RescanReplacementAndSuffix(
            "FORWARD",
            maximumReplacement,
            suffix,
            [],
            depth: 0,
            tokenOffset: 0,
            suffixIndex: 0,
            sourceBaseOffset: 0,
            sourceOffsets: null);
        var exception = Should.Throw<IdlException>(() => service.RescanReplacementAndSuffix(
            "FORWARD",
            oversizedReplacement,
            suffix,
            [],
            depth: 0,
            tokenOffset: 0,
            suffixIndex: 0,
            sourceBaseOffset: 0,
            sourceOffsets: null));

        // Assert
        maximumResult.Length.ShouldBe(PreprocessorLimits.MaximumOutputLength);
        exception.Message.ShouldContain("Preprocessor output exceeds the");
    }

    [Fact]
    public void KeepsHasIncludeInvocationTogetherWhenScanningSuffix()
    {
        // Arrange
        var input = new IdlInput("rescan.idl", "");
        var expansions = new List<(string Text, int Offset, int Depth, int[]? SourceOffsets)>();
        var service = CreateService(input, expansions);

        // Act
        var result = service.RescanReplacementAndSuffix(
            "FORWARD",
            "VALUE",
            "__has_include(\"common.idl\") NEXT",
            [],
            depth: 1,
            tokenOffset: 20,
            suffixIndex: 6,
            sourceBaseOffset: 0,
            sourceOffsets: null);

        // Assert
        result.ShouldBe("VALUE__has_include(\"common.idl\") NEXT");
        expansions.Skip(1).Select(expansion => expansion.Text)
            .ShouldBe(["__has_include(\"common.idl\")", " ", "NEXT"]);
    }

    [Fact]
    public void KeepsPrefixedStringLiteralTogetherWhenScanningSuffix()
    {
        // Arrange
        var input = new IdlInput("rescan.idl", "");
        var macros = new PreprocessorMacroTable();
        macros["u8"] = new PreprocessorMacro(null, "replaced", variadic: false);
        var expansions = new List<(string Text, int Offset, int Depth, int[]? SourceOffsets)>();
        var service = CreateService(input, expansions, macros);

        // Act
        var result = service.RescanReplacementAndSuffix(
            "FORWARD",
            "VALUE",
            "u8\"INNER\" NEXT",
            [],
            depth: 1,
            tokenOffset: 20,
            suffixIndex: 6,
            sourceBaseOffset: 0,
            sourceOffsets: null);

        // Assert
        result.ShouldBe("VALUEu8\"INNER\" NEXT");
        expansions.Skip(1).Select(expansion => expansion.Text)
            .ShouldBe(["u8\"INNER\"", " ", "NEXT"]);
    }

    [Fact]
    public void DoesNotRescanMacrosInsideOrdinaryStringLiteralsAfterUnknownIdentifiers()
    {
        // Arrange
        var input = new IdlInput("rescan.idl", "");
        var macros = new PreprocessorMacroTable
        {
            ["INNER"] = new PreprocessorMacro(null, "replaced", variadic: false)
        };
        var expansions = new List<(string Text, int Offset, int Depth, int[]? SourceOffsets)>();
        var service = CreateService(input, expansions, macros);

        // Act
        var result = service.RescanReplacementAndSuffix(
            "FORWARD",
            "VALUE",
            "UNKNOWN \"INNER\" NEXT",
            [],
            depth: 1,
            tokenOffset: 20,
            suffixIndex: 6,
            sourceBaseOffset: 0,
            sourceOffsets: null);

        // Assert
        result.ShouldBe("VALUEUNKNOWN \"INNER\" NEXT");
        expansions.Skip(1).Select(expansion => expansion.Text)
            .ShouldBe(["UNKNOWN ", "\"INNER\"", " ", "NEXT"]);
    }

    [Fact]
    public void ScansUnknownSuffixIdentifierUpToTheNextPossibleExpansion()
    {
        // Arrange
        var input = new IdlInput("rescan.idl", "");
        var expansions = new List<(string Text, int Offset, int Depth, int[]? SourceOffsets)>();
        var service = CreateService(input, expansions);

        // Act
        var result = service.RescanReplacementAndSuffix(
            "FORWARD",
            "VALUE",
            "UNKNOWN NEXT",
            [],
            depth: 1,
            tokenOffset: 20,
            suffixIndex: 6,
            sourceBaseOffset: 0,
            sourceOffsets: null);

        // Assert
        result.ShouldBe("VALUEUNKNOWN NEXT");
        expansions.Skip(1).Select(expansion => expansion.Text).ShouldBe(["UNKNOWN ", "NEXT"]);
    }

    [Fact]
    public void SkipsWhitespaceBeforeReadingFunctionMacroSuffixArguments()
    {
        // Arrange
        var input = new IdlInput("rescan.idl", "");
        var macros = new PreprocessorMacroTable();
        macros["CALL"] = new PreprocessorMacro(["value"], "value", variadic: false);
        var expansions = new List<(string Text, int Offset, int Depth, int[]? SourceOffsets)>();
        var service = CreateService(input, expansions, macros);

        // Act
        var result = service.RescanReplacementAndSuffix(
            "FORWARD",
            "VALUE",
            "CALL \t(ITEM) NEXT",
            [],
            depth: 1,
            tokenOffset: 20,
            suffixIndex: 6,
            sourceBaseOffset: 0,
            sourceOffsets: null);

        // Assert
        result.ShouldBe("VALUECALL \t(ITEM) NEXT");
        expansions.Skip(1).Select(expansion => expansion.Text)
            .ShouldBe(["CALL \t(ITEM)", " ", "NEXT"]);
    }

    private static PreprocessorMacroRescanService CreateService(
        IdlInput input,
        ICollection<(string Text, int Offset, int Depth, int[]? SourceOffsets)> expansions,
        PreprocessorMacroTable? macros = null,
        ICollection<int>? checkedLengths = null) =>
        new(
            macros ?? new PreprocessorMacroTable(),
            new PreprocessorMacroTokenService(),
            CancellationToken.None,
            () => input,
            (text, _, depth, offset, sourceOffsets) =>
            {
                expansions.Add((text, offset, depth, sourceOffsets?.ToArray()));
                return text;
            },
            (output, text, _, _) =>
            {
                output.Append(text);
            },
            (length, _) => checkedLengths?.Add(length));
}
