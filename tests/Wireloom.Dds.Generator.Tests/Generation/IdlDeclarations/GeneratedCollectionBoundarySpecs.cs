namespace Wireloom.Dds.Generator.Tests;

public sealed class GeneratedCollectionBoundarySpecs
{
    [Fact]
    [Trait("Corpus", "C016")]
    public void EmitsTheP05EmptyExactAndOverBoundAllocationContract()
    {
        var documents = IdlCompiler.CompileSources([
            CompilerTestSupport.Input(
                "C016-05-collections.idl",
                """
                module CorpusCollections {
                    struct Item { long id; string<16> label; };
                    typedef sequence<long, 4> BoundedLongs;
                    typedef long CoordinateGrid[2][3];
                    struct Sample {
                        long values[2][3];
                        sequence<long> unbounded;
                        BoundedLongs bounded;
                        sequence<Item, 2> items;
                        CoordinateGrid grid;
                    };
                };
                """)
        ], TestContext.Current.CancellationToken);

        var data = documents["CorpusCollections.Sample.g.cs"].Source;
        var native = documents["CorpusCollections.Implementation.SampleUnmanaged.g.cs"].Source;
        var plugin = documents["CorpusCollections.Implementation.SamplePlugin.g.cs"].Source;

        // Managed defaults establish the empty value and fixed-array shape.
        data.ShouldContain("values = new int[2, 3];");
        data.ShouldContain("unbounded = new Sequence<int>();");
        data.ShouldContain("public BoundedLongs bounded { get; set; } = new BoundedLongs();");
        data.ShouldContain("items = new Sequence<Item>();");
        data.ShouldContain("public CoordinateGrid grid { get; set; } = new CoordinateGrid();");

        // Native initialization carries the RTI boundary contract: exact values
        // are accepted and over-bound values are rejected by serialization.
        native.ShouldContain("values.Initialize<int>(dimension: 2 * 3, allocateMemory: allocateMemory);");
        native.ShouldContain("unbounded.Initialize<int>(max: 100, absoluteMax: 100, allocateMemory: allocateMemory);");
        native.ShouldContain("bounded.Initialize(allocatePointers, allocateMemory);");
        native.ShouldContain("items.Initialize<Item, ItemUnmanaged>(max: 2, absoluteMax: 2, allocateMemory: allocateMemory);");
        native.ShouldContain("grid.Initialize(allocatePointers, allocateMemory);");

        native.ShouldContain("unbounded.ToNative((Sequence<int>)sample.unbounded);");
        native.ShouldContain("items.ToNative<Item, ItemUnmanaged>(sample.items);");

        // DynamicType metadata must expose the same bounds and dimensions.
        plugin.ShouldContain("CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), new uint[] { 2, 3 })");
        plugin.ShouldContain("CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), 100)");
        plugin.ShouldContain("CreateSequenceWithAccessInfo(dtf, ItemSupport.Instance.GetDynamicTypeInternal(isPublic), 2)");
    }
}
