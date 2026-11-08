namespace Wireloom.Compiler.Tests;

/// <summary>Protects the unbounded option and its declaring-file semantics.</summary>
public sealed class UnboundedSupportSpecs
{
    /// <summary>Preserves declaring-file string policies through sequence and array aliases.</summary>
    [Theory]
    [InlineData(false, "string", "String")]
    [InlineData(true, "string", "String")]
    [InlineData(false, "wstring", "WideString")]
    [InlineData(true, "wstring", "WideString")]
    public void KeepsStringCollectionAliasPolicyIndependentOfConsumer(bool unboundedSupport, string idlType, string dynamicType)
    {
        // Arrange
        var root = new IdlInput("root.idl",
            "#include \"aliases.idl\"\nstruct Sample { Texts values; TextArray arrayValues; };",
            stringSize: 19, unboundedSupport: !unboundedSupport);
        var aliases = new IdlInput("aliases.idl",
            $"typedef sequence<{idlType}> Texts; typedef {idlType} TextArray[2];",
            generate: false, stringSize: 7, unboundedSupport: unboundedSupport);
        var expectedBound = unboundedSupport ? $"{dynamicType}Type.Unbounded" : "7";
        var expectedType = unboundedSupport ? $"dtf.Create{dynamicType}()" : $"dtf.Create{dynamicType}(7)";

        // Act
        var sources = CompilerTestSupport.CompileSources([root, aliases], TestContext.Current.CancellationToken);

        // Assert
        foreach (var alias in new[] { "Texts", "TextArray" })
        {
            sources[$"Implementation.{alias}Plugin.g.cs"].Source.ShouldContain(expectedType);
            sources[$"Implementation.{alias}Unmanaged.g.cs"].Source.ShouldContain($"maxStrLen: {expectedBound}");
        }
    }

    /// <summary>Emits RTI unbounded metadata while preserving explicit IDL bounds.</summary>
    [Fact]
    public void EmitsUnboundedSequenceAndStringSemantics()
    {
        // Arrange
        const string source =
            """
            struct Item { long value; };
            struct Sample {
                sequence<long> values;
                string text;
                wstring wideText;
                string texts[2];
                sequence<Item> items;
                @optional sequence<long> optionalValues;
                @optional sequence<string> optionalTexts;
                @optional sequence<Item> optionalItems;
                sequence<string<8> > boundedElements;
                sequence<string > unboundedElements;
                sequence<long, 4> boundedValues;
            };
            """;
        var input = new IdlInput("sample.idl", source, unboundedSupport: true, sequenceSize: 7, stringSize: 9);

        // Act
        var sources = CompilerTestSupport.CompileSources([input], TestContext.Current.CancellationToken);

        // Assert
        var managed = sources["Sample.g.cs"].Source;
        managed.ShouldContain("[Bound(-1)]\n    public string text");
        managed.ShouldContain("[Bound(-1)]\n    public string wideText");
        managed.ShouldContain("[Bound(4)]");
        var plugin = sources["Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldContain("SequenceType.Unbounded");
        plugin.ShouldContain("dtf.CreateString()");
        plugin.ShouldContain("dtf.CreateWideString()");
        plugin.ShouldContain("tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(), SequenceType.Unbounded)");
        plugin.ShouldNotContain("using var dtString");
        plugin.ShouldNotContain("using var dtWString");
        plugin.ShouldContain("CreateString(8)");
        plugin.ShouldContain("CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), 4)");
        plugin.ShouldContain("using Rti.Types.Dynamic;");
        var native = sources["Implementation.SampleUnmanaged.g.cs"].Source;
        native.ShouldContain("using Rti.Types.Dynamic;");
        native.ShouldContain("values.Initialize<int>(max: 0, absoluteMax: SequenceType.Unbounded");
        native.ShouldContain("text.Initialize(size: 0");
        native.ShouldContain("texts.Initialize(dimension: 2, maxStrLen: StringType.Unbounded, allocateMemory: allocateMemory);");
        native.ShouldContain("items.Initialize<Item, global::Implementation.ItemUnmanaged>(max: 0, absoluteMax: SequenceType.Unbounded, allocateMemory);");
        native.ShouldContain("texts.ToNative(sample.texts, dimension: 2, maxStrLen: StringType.Unbounded);");
        native.ShouldContain("optionalValues.ToNative((Sequence<int>)sample.optionalValues!, SequenceType.Unbounded);");
        native.ShouldContain("optionalTexts.ToNative(sample.optionalTexts, SequenceType.Unbounded, -1);");
        native.ShouldContain("optionalItems.ToNative<Item, global::Implementation.ItemUnmanaged>(sample.optionalItems, SequenceType.Unbounded);");
        native.ShouldContain("text.ToNative(sample.text, StringType.Unbounded)");
        native.ShouldContain("wideText.ToNative(sample.wideText, WideStringType.Unbounded)");
        native.ShouldNotContain("Rti.Types.Dynamic.StringType.Unbounded");
        native.ShouldNotContain("Rti.Types.Dynamic.WideStringType.Unbounded");
    }

    /// <summary>Leaves finite fallbacks in effect when the option is omitted.</summary>
    [Fact]
    public void KeepsFiniteDefaultsWhenDisabled()
    {
        // Arrange
        const string source = "struct Sample { sequence<long> values; string text; };";

        // Act
        var sources = CompilerTestSupport.CompileSources(
            [new IdlInput("sample.idl", source, sequenceSize: 7, stringSize: 9)],
            TestContext.Current.CancellationToken);

        // Assert
        var plugin = sources["Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldNotContain("SequenceType.Unbounded");
        plugin.ShouldNotContain("StringType.Unbounded");
        var native = sources["Implementation.SampleUnmanaged.g.cs"].Source;
        native.ShouldNotContain("using Rti.Types.Dynamic;");
        native.ShouldContain("values.Initialize<int>(max: 7, absoluteMax: 7");
        native.ShouldContain("text.ToNative(sample.text, 9)");
    }

    /// <summary>Uses the configured finite capacity for recursive sequences unless unbounded support is enabled.</summary>
    [Theory]
    [InlineData(false, "100")]
    [InlineData(true, "SequenceType.Unbounded")]
    public void EmitsRecursiveSequenceUsingSelectedBoundPolicy(bool unboundedSupport, string expectedBound)
    {
        // Arrange
        const string source = "struct Node { long value; sequence<Node> children; };";
        var input = new IdlInput("recursive-sequence.idl", source, unboundedSupport: unboundedSupport);

        // Act
        var sources = CompilerTestSupport.CompileSources([input], TestContext.Current.CancellationToken);

        // Assert
        var plugin = sources["Implementation.NodePlugin.g.cs"].Source;
        plugin.ShouldContain("CreateSequenceWithAccessInfo");
        plugin.ShouldContain(expectedBound);

        var unmanaged = sources["Implementation.NodeUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("max: 0");
        unmanaged.ShouldContain($"absoluteMax: {expectedBound}");
    }

    /// <summary>Uses unbounded native capacity for a typedef that points into a recursive sequence component.</summary>
    [Fact]
    public void EmitsRecursiveSequenceAliasWithUnboundedNativeCapacity()
    {
        // Arrange
        const string source = "struct Node { sequence<Node> children; }; typedef sequence<Node> Nodes;";
        var input = new IdlInput("recursive-sequence-alias.idl", source, unboundedSupport: true);

        // Act
        var sources = CompilerTestSupport.CompileSources([input], TestContext.Current.CancellationToken);

        // Assert
        sources["Implementation.NodesUnmanaged.g.cs"].Source.ShouldContain(
            "Value.Initialize<global::Node, NodeUnmanaged>(max: 0, absoluteMax: SequenceType.Unbounded, allocateMemory);");
    }

    /// <summary>Uses the input's policy for declarations reached through includes.</summary>
    [Fact]
    public void KeepsIncludedFilePolicyIndependentOfRoot()
    {
        // Arrange
        var root = new IdlInput("root.idl", "#include \"shared.idl\"\nstruct Root { sequence<long> values; };", unboundedSupport: false);
        var shared = new IdlInput("shared.idl", "struct Shared { sequence<long> values; };", generate: false, unboundedSupport: true);

        // Act
        var sources = CompilerTestSupport.CompileSources([root, shared], TestContext.Current.CancellationToken);

        // Assert
        sources["Implementation.RootPlugin.g.cs"].Source.ShouldNotContain("SequenceType.Unbounded");
        sources["Implementation.SharedPlugin.g.cs"].Source.ShouldContain("SequenceType.Unbounded");
    }

    /// <summary>Applies the declaring file's policy to typedef sequences and strings.</summary>
    [Fact]
    public void AppliesPolicyToCollectionAliases()
    {
        // Arrange
        const string source =
            """
            typedef sequence<long> Numbers;
            typedef string Text;
            struct Sample { Numbers values; Text text; };
            """;

        // Act
        var sources = CompilerTestSupport.CompileSources(
            [new IdlInput("aliases.idl", source, unboundedSupport: true)],
            TestContext.Current.CancellationToken);

        // Assert
        sources["Implementation.NumbersPlugin.g.cs"].Source.ShouldContain("SequenceType.Unbounded");
        sources["Implementation.NumbersUnmanaged.g.cs"].Source.ShouldContain("absoluteMax: SequenceType.Unbounded");
        sources["Text.g.cs"].Source.ShouldContain("[Bound(-1)]");
        sources["Implementation.TextPlugin.g.cs"].Source.ShouldContain("dtf.CreateString()");
        sources["Implementation.TextPlugin.g.cs"].Source.ShouldContain("var dtString = dtf.CreateString();");
        sources["Implementation.TextPlugin.g.cs"].Source.ShouldNotContain("using var dtString");
        sources["Implementation.TextUnmanaged.g.cs"].Source.ShouldContain("StringType.Unbounded");
    }
}
