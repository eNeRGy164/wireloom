using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Dds.Generator.Tests;

public sealed class GeneratedTypedefSpecs
{
    [Fact]
    public void EmitsPrimitiveAndAggregateCollectionAliasesWithMatchingNativeContracts()
    {
        // Arrange
        var input = Input("collection-aliases.idl",
            """
            module CollectionAliases {
                struct Item { long value; };
                typedef sequence<long, 3> Values;
                typedef sequence<Item, 2> Items;
                typedef long Matrix[2][3];
                typedef Item ItemArray[2];
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var values = documents["CollectionAliases.Values.g.cs"].Source;
        values.ShouldContain("public ISequence<int> Value { get; } = null!;");

        var valuesPlugin = documents["CollectionAliases.Implementation.ValuesPlugin.g.cs"].Source;
        valuesPlugin.ShouldContain("CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), 3)");

        var valuesNative = documents["CollectionAliases.Implementation.ValuesUnmanaged.g.cs"].Source;
        valuesNative.ShouldContain("private NativeSeq Value;");
        valuesNative.ShouldNotContain("NativeManagedArray");
        valuesNative.ShouldContain("Value.Initialize<int>(max: 3, absoluteMax: 3, allocateMemory: allocateMemory);");
        valuesNative.ShouldContain("Value.FromNative((Sequence<int>)sample.Value);");
        valuesNative.ShouldContain("Value.ToNative((Sequence<int>)sample.Value);");

        var items = documents["CollectionAliases.Items.g.cs"].Source;
        items.ShouldContain("public ISequence<Item> Value { get; } = null!;");

        var itemsPlugin = documents["CollectionAliases.Implementation.ItemsPlugin.g.cs"].Source;
        itemsPlugin.ShouldContain("CreateSequenceWithAccessInfo(dtf, ItemSupport.Instance.GetDynamicTypeInternal(isPublic), 2)");

        var itemsNative = documents["CollectionAliases.Implementation.ItemsUnmanaged.g.cs"].Source;
        itemsNative.ShouldNotContain("Value.Initialize<int>");
        itemsNative.ShouldContain("Value.Initialize<Item, ItemUnmanaged>(max: 2, absoluteMax: 2, allocateMemory: allocateMemory);");
        itemsNative.ShouldContain("Value.FromNative<Item, ItemUnmanaged>((Sequence<Item>)sample.Value);");
        itemsNative.ShouldContain("Value.ToNative<Item, ItemUnmanaged>((Sequence<Item>)sample.Value);");

        var matrix = documents["CollectionAliases.Matrix.g.cs"].Source;
        matrix.ShouldContain("public int[,] Value { get; set; } = new int[2, 3];");

        var matrixPlugin = documents["CollectionAliases.Implementation.MatrixPlugin.g.cs"].Source;
        matrixPlugin.ShouldContain("CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), new uint[] { 2, 3 })");

        var matrixNative = documents["CollectionAliases.Implementation.MatrixUnmanaged.g.cs"].Source;
        matrixNative.ShouldContain("private NativeUnmanagedArray Value;");
        matrixNative.ShouldNotContain("private NativeManagedArray Value;");
        matrixNative.ShouldContain("Value.Initialize<int>(dimension: 2 * 3, allocateMemory: allocateMemory);");
        matrixNative.ShouldContain("Value.FromNative(sample.Value, dimension: 2 * 3);");
        matrixNative.ShouldContain("Value.ToNative<int>(sample.Value, dimension: 2 * 3);");

        var itemArray = documents["CollectionAliases.ItemArray.g.cs"].Source;
        itemArray.ShouldContain("public Item[] Value { get; set; } = new Item[2];");
        itemArray.ShouldContain("for (var dimension0 = 0; dimension0 < 2; dimension0++)");
        itemArray.ShouldContain("Value[dimension0] = new Item();");

        var itemArrayPlugin = documents["CollectionAliases.Implementation.ItemArrayPlugin.g.cs"].Source;
        itemArrayPlugin.ShouldContain("CreateArrayWithAccessInfo<ItemUnmanaged>(dtf, ItemSupport.Instance.GetDynamicTypeInternal(isPublic), new uint[] { 2 })");

        var itemArrayNative = documents["CollectionAliases.Implementation.ItemArrayUnmanaged.g.cs"].Source;
        itemArrayNative.ShouldContain("private NativeManagedArray Value;");
        itemArrayNative.ShouldNotContain("private NativeUnmanagedArray Value;");
        itemArrayNative.ShouldContain("Value.Destroy<Item, ItemUnmanaged>(dimension: 2, optionalsOnly: optionalsOnly);");
        itemArrayNative.ShouldContain("Value.Initialize<Item, ItemUnmanaged>(dimension: 2, allocatePointers: allocatePointers, allocateMemory: allocateMemory);");
        itemArrayNative.ShouldContain("Value.FromNative<Item, ItemUnmanaged>(sample.Value, keysOnly: false, dimension: 2);");
        itemArrayNative.ShouldContain("Value.ToNative<Item, ItemUnmanaged>(sample.Value, keysOnly: false, dimension: 2);");
    }

    [Fact]
    public void EmitsNarrowAndWideStringAliasContracts()
    {
        // Arrange
        var input = Input("string-aliases.idl",
            """
            module StringAliases {
                typedef string<8> Text;
                typedef wstring<4> WideText;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var text = documents["StringAliases.Text.g.cs"].Source;
        text.ShouldContain("[Bound(8)]");
        text.ShouldContain("public string Value { get; set; } = string.Empty;");

        var textPlugin = documents["StringAliases.Implementation.TextPlugin.g.cs"].Source;
        textPlugin.ShouldContain("dtf.CreateString(8)");

        var textNative = documents["StringAliases.Implementation.TextUnmanaged.g.cs"].Source;
        textNative.ShouldContain("private NativeString Value;");
        textNative.ShouldNotContain("NativeWstring");
        textNative.ShouldContain("Value.Initialize(size: 8, allocateMemory: allocateMemory);");
        textNative.ShouldContain("sample.Value = Value.FromNative();");
        textNative.ShouldContain("Value.ToNative(sample.Value, 8);");

        var wideText = documents["StringAliases.WideText.g.cs"].Source;
        wideText.ShouldContain("[Bound(4)]");
        wideText.ShouldContain("public string Value { get; set; } = string.Empty;");

        var wideTextPlugin = documents["StringAliases.Implementation.WideTextPlugin.g.cs"].Source;
        wideTextPlugin.ShouldContain("dtf.CreateWideString(4)");

        var wideTextNative = documents["StringAliases.Implementation.WideTextUnmanaged.g.cs"].Source;
        wideTextNative.ShouldContain("private NativeWstring Value;");
        wideTextNative.ShouldNotContain("NativeString");
        wideTextNative.ShouldContain("Value.Initialize(size: 4, allocateMemory: allocateMemory);");
        wideTextNative.ShouldContain("sample.Value = Value.FromNative();");
        wideTextNative.ShouldContain("Value.ToNative(sample.Value, 4);");
    }

    [Fact]
    public void EmitsBoundedStringSequenceAliasContracts()
    {
        // Arrange
        var input = Input("string-sequence-aliases.idl",
            """
            module StringSequenceAliases {
                typedef sequence<string<8>, 3> Texts;
                typedef sequence<wstring<4>, 2> WideTexts;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var textsNative = documents["StringSequenceAliases.Implementation.TextsUnmanaged.g.cs"].Source;
        textsNative.ShouldContain("private NativeStringSeq Value;");
        textsNative.ShouldContain("Value.Initialize(max: 3, absoluteMax: 3, maxStrLen: 8, allocateMemory: allocateMemory);");
        textsNative.ShouldContain("Value.FromNative(sample.Value);");
        textsNative.ShouldContain("Value.ToNative(sample.Value, 8);");
        textsNative.ShouldContain("if (optionalsOnly)");
        textsNative.ShouldContain("Value.Destroy();");
        textsNative.ShouldNotContain("Value.Destroy(optionalsOnly)");

        var wideTextsNative = documents["StringSequenceAliases.Implementation.WideTextsUnmanaged.g.cs"].Source;
        wideTextsNative.ShouldContain("private NativeWstringSeq Value;");
        wideTextsNative.ShouldContain("Value.Initialize(max: 2, absoluteMax: 2, maxStrLen: 4, allocateMemory: allocateMemory);");
        wideTextsNative.ShouldContain("Value.FromNative(sample.Value);");
        wideTextsNative.ShouldContain("Value.ToNative(sample.Value, 4);");
        wideTextsNative.ShouldContain("if (optionalsOnly)");
        wideTextsNative.ShouldContain("Value.Destroy();");
        wideTextsNative.ShouldNotContain("Value.Destroy(optionalsOnly)");
    }

    [Fact]
    public void EmitsTypedDestroyContractsForAggregateAliasesAndNestedSequences()
    {
        // Arrange
        var input = Input("destroy-aliases.idl",
            """
            module DestroyAliases {
                struct Item { long value; };
                typedef Item ItemAlias;
                typedef sequence<ItemAlias, 2> Items;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var itemAlias = documents["DestroyAliases.Implementation.ItemAliasUnmanaged.g.cs"].Source;
        itemAlias.ShouldContain("if (optionalsOnly)");
        itemAlias.ShouldContain("Value.Destroy(optionalsOnly);");

        var items = documents["DestroyAliases.Implementation.ItemsUnmanaged.g.cs"].Source;
        items.ShouldContain("if (optionalsOnly)");
        items.ShouldContain("Value.Destroy<ItemAlias, ItemAliasUnmanaged>(optionalsOnly);");
    }
}
