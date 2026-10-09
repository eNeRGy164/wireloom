using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class IdlInputGraphCycleSpecs
{
    [Fact]
    [Trait("Corpus", "C077")]
    [Trait("Corpus", "C078")]
    [Trait("Corpus", "C079")]
    [Trait("Preprocessor", "PP037")]
    public void ReportsCyclicIncludes()
    {
        // Arrange
        var first = Input("a.idl",
            """
            #include "b.idl"
            """);
        var second = Input("b.idl",
            """
            #include "a.idl"
            """,
            generate: false);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(first, second));

        // Assert
        exception.Message.ShouldContain("Cyclic include");
    }

    [Fact]
    [Trait("Preprocessor", "PP034")]
    [Trait("Preprocessor", "PP036")]
    public void AllowsIncludeCyclesThatAreStoppedByIncludeGuards()
    {
        // Arrange
        var first = Input("a.idl",
            """
            #ifndef A_IDL
            #define A_IDL
            #include "b.idl"
            struct First { long value; };
            #endif
            """);
        var second = Input("b.idl",
            """
            #ifndef B_IDL
            #define B_IDL
            #include "a.idl"
            struct Second { long value; };
            #endif
            """,
            generate: false);

        // Act
        var output = Compile(first, second);

        // Assert
        output.ShouldContain("public partial class First");
        output.ShouldContain("public partial class Second");
    }

    [Fact]
    [Trait("Preprocessor", "PP035")]
    public void AllowsSelfIncludesWhenPragmaOncePrecedesTheInclude()
    {
        // Arrange
        var root = Input("self.idl",
            """
            #pragma once
            #include "self.idl"
            struct SelfIncluded { long value; };
            """);

        // Act
        var output = Compile(root);

        // Assert
        output.ShouldContain("public partial class SelfIncluded");
    }

    [Fact]
    [Trait("Preprocessor", "PP049")]
    public void ReportsIncludeDepthOverflowSeparatelyFromCycles()
    {
        // Arrange
        var inputs = Enumerable.Range(0, 130)
            .Select(index => Input(
                $"include-{index}.idl",
                index == 129
                    ? """struct Last { long value; };"""
                    : $$"""
                      #include "include-{{index + 1}}.idl"
                      """,
                index == 0))
            .ToList();

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(inputs));

        // Assert
        exception.Message.ShouldContain("maximum depth");
        exception.Message.ShouldNotContain("Cyclic include");
    }

    [Fact]
    [Trait("Preprocessor", "PP049")]
    public void AllowsAnIncludeChainAtTheMaximumSupportedDepth()
    {
        // Arrange
        const int maximumDepth = 128;
        var inputs = Enumerable.Range(0, maximumDepth + 1)
            .Select(index => Input(
                $"depth-{index}.idl",
                index == maximumDepth
                    ? "struct Last { long value; };"
                    : $"#include \"depth-{index + 1}.idl\""))
            .ToArray();
        var pathComparer = Path.DirectorySeparatorChar == '\\'
            ? StringComparer.OrdinalIgnoreCase
            : StringComparer.Ordinal;
        var files = inputs.ToDictionary(input => Path.GetFullPath(input.Path), pathComparer);
        var graph = new IdlInputGraph(files, [], [], [], CancellationToken.None);
        graph.BeginRoot();
        var parsedInputs = 0;

        // Act
        graph.Visit(inputs[0], (_, _, _, _, _) => parsedInputs++);

        // Assert
        parsedInputs.ShouldBe(maximumDepth + 1);
    }

    [Fact]
    [Trait("Preprocessor", "PP034")]
    public void DeduplicatesRootOutputsUsingPlatformPathCasingRules()
    {
        // Arrange
        var graph = new IdlInputGraph(new Dictionary<string, IdlInput>(), [], [], [], CancellationToken.None);
        graph.BeginRoot();
        var lowerCasePath = Input("case.idl", "struct Sample { long value; };");
        var upperCasePath = Input("CASE.IDL", "struct Sample { long value; };");
        var parsedInputs = 0;

        // Act
        graph.Visit(lowerCasePath, (_, _, _, _, _) => parsedInputs++);
        graph.Visit(upperCasePath, (_, _, _, _, _) => parsedInputs++);

        // Assert
        parsedInputs.ShouldBe(Path.DirectorySeparatorChar == '\\' ? 1 : 2);
    }

    [Fact]
    [Trait("Preprocessor", "PP037")]
    [Trait("Preprocessor", "PP049")]
    public void RemovesVisitedPathsFromTheActiveSetAfterSuccessfulVisits()
    {
        // Arrange
        const int maximumDepth = 128;
        var shared = Input("shared.idl", "struct Shared { long value; };");
        var chain = Enumerable.Range(0, maximumDepth + 1)
            .Select(index => Input(
                $"cleanup-{index}.idl",
                index == maximumDepth
                    ? "#include \"shared.idl\""
                    : $"#include \"cleanup-{index + 1}.idl\""))
            .ToArray();
        var pathComparer = Path.DirectorySeparatorChar == '\\'
            ? StringComparer.OrdinalIgnoreCase
            : StringComparer.Ordinal;
        var files = chain.Append(shared).ToDictionary(input => Path.GetFullPath(input.Path), pathComparer);
        var graph = new IdlInputGraph(files, [], [], [], CancellationToken.None);
        graph.BeginRoot();
        var parse = (string _, IdlInput _, int _, string? _, IReadOnlyList<SourceOriginSpan> _) => { };
        graph.Visit(shared, parse);

        // Act
        var exception = Should.Throw<IdlException>(() => graph.Visit(chain[0], parse));

        // Assert
        exception.Message.ShouldContain("maximum depth");
        exception.Message.ShouldNotContain("Cyclic include");
    }
}
