using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Dds.Generator.Tests;

public sealed class GeneratedCollectionSpecs
{
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

        var texts = documents["Collections.Implementation.TextsUnmanaged.g.cs"].Source;
        texts.ShouldContain("private NativeStringSeq Value");
        texts.ShouldContain("maxStrLen: 8");
        texts.ShouldContain("Value.Destroy();");

        var items = documents["Collections.Implementation.ItemsUnmanaged.g.cs"].Source;
        items.ShouldContain("Value.Destroy<Item, ItemUnmanaged>(optionalsOnly);");

        var sampleUnmanaged = documents["Collections.Implementation.SampleUnmanaged.g.cs"].Source;
        sampleUnmanaged.ShouldContain("values.Initialize<int>");
        sampleUnmanaged.ShouldContain("grid.Initialize(allocatePointers, allocateMemory)");

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
        managed.ShouldContain("public ISequence<int> values");
        managed.ShouldContain("public int[,] grid");
        managed.ShouldContain("public string? text");

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
        unmanaged.ShouldContain("grid.Initialize<int>(dimension: 2 * 3, allocateMemory: allocateMemory)");

        var samplePlugin = documents["PrimitiveArray.Implementation.SamplePlugin.g.cs"].Source;
        samplePlugin.ShouldContain("CreateArrayWithAccessInfo<int>");
    }
}

