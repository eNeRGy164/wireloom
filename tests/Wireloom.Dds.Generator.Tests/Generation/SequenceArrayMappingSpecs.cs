using Wireloom.Compiler.Emission;

namespace Wireloom.Generation.Tests;

/// <summary>Protects opt-in array semantics and unchanged default generation.</summary>
public sealed class SequenceArrayMappingSpecs
{
    /// <summary>Preserves the legacy output byte for byte when the default is selected explicitly.</summary>
    [Fact]
    public void RetainsDefaultMapping()
    {
        // Arrange
        const string source =
            """
            module Arrays { struct Sample { sequence<long> values[2]; }; };
            """;
        var implicitInput = new IdlInput("sample.idl", source);
        var explicitInput = new IdlInput("sample.idl", source, IdlSequenceArrayMapping.RtiCSharp);

        // Act
        var implicitSources = IdlCompiler.CompileSources([implicitInput], TestContext.Current.CancellationToken);
        var explicitSources = IdlCompiler.CompileSources([explicitInput], TestContext.Current.CancellationToken);

        // Assert
        implicitSources.Count.ShouldBe(explicitSources.Count);

        foreach (var entry in implicitSources)
        {
            explicitSources[entry.Key].Source.ShouldBe(entry.Value.Source);
        }
    }

    /// <summary>Plans native wrappers and anonymous metadata for both LTS profiles.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void PreservesSupportedElementKinds(bool use731)
    {
        // Arrange
        const string source =
            """
            module Arrays {
                struct Item { long value; };
                enum State { Ready, Done };
                typedef long Scalar;
                typedef sequence<long, 2> Numbers;
                typedef Scalar ScalarChain;
                struct Sample {
                    sequence<long> values[2];
                    sequence<string<8>, 3> names[2];
                    sequence<wstring<8>, 3> wideNames[2];
                    sequence<Item, 3> items[2];
                    sequence<State, 3> states[2];
                    sequence<Scalar, 3> scalars[2];
                    sequence<ScalarChain, 3> scalarChain[2];
                    sequence<Numbers, 3> nested[2];
                    @optional sequence<long, 3> matrix[2][3];
                    sequence<long, 3> ordinary;
                    long integers[2];
                    Numbers aliased[2];
                };
                struct Derived : Sample { long extra; };
            };
            """;
        var diagnostics = new List<IdlDiagnostic>();
        var profile = RtiApiProfile.Rtiddsgen470;
        if (use731)
        {
            profile = RtiApiProfile.Rtiddsgen431;
        }

        // Act
        var sources = IdlCompiler.CompileSourcesWithDiagnostics(
            [new IdlInput("sample.idl", source, IdlSequenceArrayMapping.Idl)], diagnostics,
            TestContext.Current.CancellationToken, profile);

        // Assert
        diagnostics.ShouldBeEmpty();
        var managed = sources["Arrays.Sample.g.cs"].Source;
        managed.ShouldContain("Sample_valuesSequence[] values");
        managed.ShouldContain("Sample_matrixSequence[,]? matrix");
        managed.ShouldContain("ISequence<int> ordinary");
        managed.ShouldContain("int[] integers");
        managed.ShouldContain("Numbers[] aliased");
        sources["Arrays.Sample_valuesSequence.g.cs"].Source.ShouldContain("Each element's representable values are in the inclusive range <c>int.MinValue</c> through <c>int.MaxValue</c>.");
        sources["Arrays.Sample_valuesSequence.g.cs"].Source.ShouldContain("For an unbounded IDL sequence");
        sources["Arrays.Sample_namesSequence.g.cs"].Source.ShouldNotContain("For an unbounded IDL sequence");
        var metadata = sources["Arrays.Implementation.SamplePlugin.g.cs"].Source;
        metadata.ShouldContain("CreateArrayWithAccessInfo<Sample_valuesSequenceUnmanaged>");
        metadata.ShouldContain("tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), 100), new uint[] { 2 }");
        metadata.ShouldContain("tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateWideString(8), 3)");
        metadata.ShouldNotContain("Sample_valuesSequenceSupport.Instance.GetDynamicTypeInternal");
        sources["Arrays.Implementation.Sample_valuesSequenceUnmanaged.g.cs"].Source.ShouldContain("NativeSeq Value");
        sources["Arrays.Implementation.Sample_namesSequenceUnmanaged.g.cs"].Source.ShouldContain("NativeStringSeq Value");
    }

    /// <summary>Only changes declarations in opted-in files, independent of include order.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void KeepsSharedIncludeMappingIndependentOfRoot(bool reverse)
    {
        // Arrange
        var enabled = new IdlInput("enabled.idl",
            """
            #include "shared.idl"
            struct Enabled { sequence<long> values[2]; };
            """,
            IdlSequenceArrayMapping.Idl);
        var disabled = new IdlInput("disabled.idl",
            """
            #include "shared.idl"
            struct Disabled { sequence<long> values[2]; };
            """);
        var shared = new IdlInput("shared.idl",
            """
            struct Shared { sequence<long> values[2]; };
            """,
            generate: false);
        var inputs = new List<IdlInput> { enabled, disabled, shared };
        if (reverse)
        {
            inputs.Reverse();
        }

        // Act
        var sources = IdlCompiler.CompileSources(inputs, TestContext.Current.CancellationToken);

        // Assert
        sources["Enabled.g.cs"].Source.ShouldContain("Enabled_valuesSequence[] values");
        sources["Disabled.g.cs"].Source.ShouldContain("ISequence<int> values");
        sources["Shared.g.cs"].Source.ShouldContain("ISequence<int> values");
    }

    /// <summary>Reserves all companion identities when choosing deterministic wrapper names.</summary>
    [Fact]
    public void AvoidsUserTypeAndSupportCollisions()
    {
        // Arrange
        const string source = """
            struct Sample_valuesSequence { long value; };
            struct Sample_valuesSequence_Support { long value; };
            struct Sample { sequence<long> values[2]; };
            """;

        // Act
        var sources = IdlCompiler.CompileSources([new IdlInput("sample.idl", source, IdlSequenceArrayMapping.Idl)], TestContext.Current.CancellationToken);

        // Assert
        sources["Sample.g.cs"].Source.ShouldContain("Sample_valuesSequence__[] values");
        sources.ContainsKey("Sample_valuesSequence__.g.cs").ShouldBeTrue();
    }

    /// <summary>Rejects invalid programmatic mapping values before emission.</summary>
    [Fact]
    public void RejectsUnknownMapping()
    {
        // Arrange
        var input = new IdlInput("sample.idl", "struct Sample { long value; };", (IdlSequenceArrayMapping)42);

        // Act
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSources([input], TestContext.Current.CancellationToken));

        // Assert
        exception.Input.ShouldBeSameAs(input);
        exception.Message.ShouldContain("SequenceArrayMapping");
    }

    /// <summary>Rejects recursive opt-in arrays before emitting unsafe plugin initialization.</summary>
    [Theory]
    [InlineData("struct Node { sequence<Node> children[2]; };")]
    [InlineData("typedef Node Alias; struct Node { sequence<Alias> children[2]; };")]
    [InlineData("struct Child { sequence<Node> parents; }; struct Node { sequence<Child> children[2]; };")]
    [InlineData("typedef Node Nodes[2]; struct Child { Nodes parents; }; struct Node { sequence<Child> children[2]; };")]
    [InlineData("union Child switch(long) { case 0: sequence<Node> parents; }; struct Node { sequence<Child> children[2]; };")]
    public void RejectsRecursiveSequenceArrays(string source)
    {
        // Arrange
        var input = new IdlInput("recursive.idl", source, IdlSequenceArrayMapping.Idl);

        // Act
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSources([input], TestContext.Current.CancellationToken));

        // Assert
        exception.Input.ShouldBeSameAs(input);
        exception.Message.ShouldContain("recursive array elements");
    }

    /// <summary>Terminates recursive element graph inspection when it does not reach the owning type.</summary>
    [Fact]
    public void AllowsIndependentRecursiveElementGraphs()
    {
        // Arrange
        const string source =
            """
            struct Leaf { sequence<Leaf> children; };
            union Choice switch(long) { case 0: long value; case 1: sequence<Choice> children; };
            struct Sample { sequence<Leaf> values[2]; sequence<Choice> choices[2]; };
            """;

        // Act
        var sources = IdlCompiler.CompileSources([new IdlInput("sample.idl", source, IdlSequenceArrayMapping.Idl)], TestContext.Current.CancellationToken);

        // Assert
        sources["Sample.g.cs"].Source.ShouldContain("Sample_valuesSequence[] values");
        sources["Sample.g.cs"].Source.ShouldContain("Sample_choicesSequence[] choices");
    }
}
