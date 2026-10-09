using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;
using Wireloom;
using Wireloom.Dds.Generator.Tests;

namespace Wireloom.Generation.Tests;

public sealed class GeneratedCollectionSpecs
{
    [Fact]
    public void WideStringSequencesUseWideNativeStorage()
    {
        // Arrange
        var input = Input("wide-string-sequence.idl", "module Sample { struct Value { sequence<wstring<8>, 3> values; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Sample.Implementation.ValueUnmanaged.g.cs"].Source.ShouldContain("private NativeWstringSeq values;");
    }

    [Fact]
    [Trait("Corpus", "C016")]
    [Trait("Corpus", "C017")]
    public void CollectionsEmitTypedBoundedAndMultidimensionalContracts()
    {
        // Arrange
        var input = Input("collections.idl",
            """
            module Collections {
                struct Item { long value; };
                typedef sequence<string<8>, 3> Texts;
                typedef sequence<Item, 2> Items;
                typedef long Grid[2][3];
                struct Sample {
                    sequence<long> values;
                    long matrix[2][3];
                    Texts texts;
                    Items items;
                    Grid grid;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Collections.Sample.g.cs"].Source;
        managed.ShouldContain("public ISequence<int> values");
        managed.ShouldContain("public Texts texts");
        managed.ShouldContain("public Items items");
        managed.ShouldContain("public Grid grid");
        managed.ShouldContain("Its maximum number of elements is <c>3</c>. Each narrow IDL string element is limited to <c>8</c> UTF-8 bytes.");
        managed.ShouldContain("For an unbounded IDL sequence, Wireloom currently generates an effective limit of 100 elements.");

        var texts = documents["Collections.Implementation.TextsUnmanaged.g.cs"].Source;
        texts.ShouldContain("private NativeStringSeq Value");
        texts.ShouldContain("maxStrLen: 8");
        texts.ShouldContain("Value.Destroy();");

        var items = documents["Collections.Implementation.ItemsUnmanaged.g.cs"].Source;
        items.ShouldContain("Value.Destroy<Item, ItemUnmanaged>(optionalsOnly);");

        var sampleUnmanaged = documents["Collections.Implementation.SampleUnmanaged.g.cs"].Source;
        sampleUnmanaged.ShouldContain("values.Initialize<int>");
        sampleUnmanaged.ShouldContain("grid.Initialize(allocatePointers, allocateMemory)");

        var sample = documents["Collections.Sample.g.cs"].Source;
        sample.ShouldContain("hash.Add(matrix[0, 0]);");
        sample.ShouldNotContain("hash.Add(matrix[0]);");

        var plugin = documents["Collections.Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldContain("CreateSequenceWithAccessInfo");
        plugin.ShouldContain("GridSupport.Instance.GetDynamicTypeInternal(isPublic)");

        var textsSupport = documents["Collections.TextsSupport.g.cs"].Source;
        textsSupport.ShouldContain("TypeSupport<Texts>");

        var itemsSupport = documents["Collections.ItemsSupport.g.cs"].Source;
        itemsSupport.ShouldContain("TypeSupport<Items>");

        var gridSupport = documents["Collections.GridSupport.g.cs"].Source;
        gridSupport.ShouldContain("TypeSupport<Grid>");
    }

    [Fact]
    public void NestedStringSequenceDocumentationDistinguishesExplicitAndEffectiveBounds()
    {
        // Arrange
        var input = Input(
            "nested-string-bounds.idl",
            "module NestedStrings { struct Sample { sequence<string, 2> unbounded; sequence<string<7>, 2> bounded; }; };");

        // Act
        var managed = CompileSources(input)["NestedStrings.Sample.g.cs"].Source;

        // Assert
        managed.ShouldContain("Each unbounded narrow IDL string element has an effective limit of <c>255</c> UTF-8 bytes.");
        managed.ShouldContain("Each narrow IDL string element is limited to <c>7</c> UTF-8 bytes.");
    }

    [Fact]
    [Trait("Corpus", "C039")]
    public void OptionalCollectionsEmitOptionalStorageMetadataAndDestroyOrder()
    {
        // Arrange
        var input = Input("optional.idl",
            """
            module Optional {
                @appendable struct Sample {
                    @optional sequence<long, 4> values;
                    @optional long grid[2][3];
                    @optional string<8> text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Optional.Sample.g.cs"].Source;
        managed.ShouldContain("[Optional]");
        managed.ShouldContain("public ISequence<int>? values");
        managed.ShouldContain("public int[,]? grid");
        managed.ShouldContain("public string? text");
        managed.ShouldContain("A <see langword=\"null\"/> value means absent; an empty collection is present with no elements.");

        var unmanaged = documents["Optional.Implementation.SampleUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("NativeOptionalSeq values");
        unmanaged.ShouldContain("NativeUnmanagedOptionalArray grid");
        unmanaged.ShouldContain("text.Destroy();");
        unmanaged.ShouldContainInOrder(
            "values.Destroy(optionalsOnly);",
            "grid.Destroy(optionalsOnly);",
            "text.Destroy();");

        var plugin = documents["Optional.Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldContain("isOptional: true");
        plugin.ShouldContain("id: 0");
        plugin.ShouldContain("id: 1");
        plugin.ShouldContain("id: 2");

        var support = documents["Optional.SampleSupport.g.cs"].Source;
        support.ShouldContain("TypeSupport<Sample>");
    }

    [Fact]
    [Trait("Corpus", "C114")]
    public void OptionalStringSequencesPreserveOptionalNativeSemantics()
    {
        // Arrange
        var input = Input(
            "optional-string-sequences.idl",
            """
            module OptionalStringSequences {
                @appendable struct Sample {
                    @optional sequence<string<16>, 4> narrowValues;
                    @optional sequence<wstring<16>, 4> wideValues;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var plugin = documents["OptionalStringSequences.Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldContain("dtf.CreateString(16)");
        plugin.ShouldContain("dtf.CreateWideString(16)");

        var unmanaged = documents["OptionalStringSequences.Implementation.SampleUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("using Omg.Types;");
        unmanaged.ShouldContain("private NativeOptionalStringSeq narrowValues;");
        unmanaged.ShouldContain("private NativeOptionalWstringSeq wideValues;");
        unmanaged.ShouldContain("narrowValues.FromNative(out ISequence<string> narrowValuesTemporary_);");
        unmanaged.ShouldContain("wideValues.FromNative(out ISequence<string> wideValuesTemporary_);");
        unmanaged.ShouldContain("narrowValues.ToNative(sample.narrowValues, 4, 16);");
        unmanaged.ShouldContain("wideValues.ToNative(sample.wideValues, 4, 16);");
        unmanaged.ShouldContain("narrowValues.Destroy();");
        unmanaged.ShouldContain("wideValues.Destroy();");
        unmanaged.ShouldNotContain("narrowValues.FromNative(sample.narrowValues);");
        unmanaged.ShouldNotContain("wideValues.FromNative(sample.wideValues);");
    }

    [Fact]
    public void QualifiesNamedCollectionElementsAgainstShadowingNamespaces()
    {
        // Arrange
        var input = Input(
            "collection-type-shadowing.idl",
            """
            module Shared {
                struct Item { long value; };
            };
            module Example {
                module Shared {
                    struct Item { long value; };
                };
                struct Sample { sequence<::Shared::Item> items; };
            };
            """);

        // Act
        var documents = CompileSources(input);
        var managed = documents["Example.Sample.g.cs"].Source;
        var plugin = documents["Example.Implementation.SamplePlugin.g.cs"].Source;
        var unmanaged = documents["Example.Implementation.SampleUnmanaged.g.cs"].Source;

        // Assert
        managed.ShouldContain("public ISequence<global::Shared.Item> items");
        plugin.ShouldContain("global::Shared.ItemSupport.Instance");
        unmanaged.ShouldContain("global::Shared.Implementation.ItemUnmanaged");
    }

    [Fact]
    public void QualifiesRootCollectionReferencesAgainstImplementationShadowing()
    {
        // Arrange
        var input = Input(
            "root-collection-shadowing.idl",
            "struct Item { long value; }; module Implementation { struct Item { long value; }; }; typedef sequence<Item, 2> Items;");

        // Act
        var documents = CompileSources(input);
        var plugin = documents["Implementation.ItemsPlugin.g.cs"].Source;
        var unmanaged = documents["Implementation.ItemsUnmanaged.g.cs"].Source;

        // Assert
        plugin.ShouldContain("InterpretedTypePlugin<global::Items, ItemsUnmanaged>");
        plugin.ShouldContain("global::ItemSupport.Instance");
        unmanaged.ShouldContain("INativeTopicType<global::Items>");
        unmanaged.ShouldContain("Value.Initialize<global::Item, ItemUnmanaged>");
    }

    [Fact]
    [Trait("Corpus", "C016")]
    public void PrimitiveArraysPreserveRankAndDimensionLogic()
    {
        // Arrange
        var input = Input(
                "primitive-array.idl",
                "module PrimitiveArray { struct Sample { long grid[2][3]; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["PrimitiveArray.Sample.g.cs"].Source;
        managed.ShouldContain("public int[,] grid");
        managed.ShouldContain("grid.Rank");
        managed.ShouldContain("GetLength(dimension)");

        var unmanaged = documents["PrimitiveArray.Implementation.SampleUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("NativeUnmanagedArray grid");
        unmanaged.ShouldContain("grid.Initialize<int>(dimension: 2 * 3, allocateMemory)");

        var samplePlugin = documents["PrimitiveArray.Implementation.SamplePlugin.g.cs"].Source;
        samplePlugin.ShouldContain("CreateArrayWithAccessInfo<int>");
    }

    [Fact]
    public void MultidimensionalArrayMembersNamedDimensionQualifyTheMemberInEqualityLambdas()
    {
        // Arrange
        var input = Input(
            "dimension-member-equality.idl",
            """
            module DimensionMemberEquality {
                struct Sample { long dimension[2][3]; };
            };
            """);

        // Act
        var managed = CompileSources(input)["DimensionMemberEquality.Sample.g.cs"].Source;

        // Assert
        managed.ShouldContain("this.dimension.GetLength(dimension)");
        managed.ShouldContain("global::System.Linq.Enumerable.Cast<int>(this.dimension)");
    }
}
