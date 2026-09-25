namespace Wireloom.Dds.Generator.Tests;

public sealed class GeneratedIdlEmissionSpecs
{
    [Fact]
    [Trait("Corpus", "C003")]
    public void MapsIdlPrimitiveTypesToTheExpectedManagedSurface()
    {
        var input = CompilerTestSupport.Input(
            "C003-01-full-widths.idl",
            """
            module OracleP01 {
                struct Primitives {
                    short legacyShort;
                    long legacyLong;
                    long long legacyLongLong;
                    unsigned short unsignedShort;
                    unsigned long unsignedLong;
                    unsigned long long unsignedLongLong;
                    int8 fixedInt8;
                    uint64 fixedUint64;
                    octet octetValue;
                    boolean boolValue;
                    char charValue;
                    wchar wcharValue;
                    float floatValue;
                    double doubleValue;
                    long double longDoubleValue;
                };
            };
            """
        );

        var output = CompilerTestSupport.Compile(input);

        output.ShouldContain("namespace OracleP01");
        output.ShouldContain("public short legacyShort");
        output.ShouldContain("public int legacyLong");
        output.ShouldContain("public long legacyLongLong");
        output.ShouldContain("public ushort unsignedShort");
        output.ShouldContain("public uint unsignedLong");
        output.ShouldContain("public ulong unsignedLongLong");
        output.ShouldContain("public sbyte fixedInt8");
        output.ShouldContain("public byte octetValue");
        output.ShouldContain("public bool boolValue");
        output.ShouldContain("public char charValue");
        output.ShouldContain("public char wcharValue");
        output.ShouldContain("public float floatValue");
        output.ShouldContain("public double doubleValue");
        output.ShouldContain("public LongDouble longDoubleValue");
    }

    [Fact]
    [Trait("Corpus", "C004")]
    public void EscapesOnlyCSharpKeywords()
    {
        var output = CompilerTestSupport.Compile(
            CompilerTestSupport.Input("C004-02-names-constants.idl", "struct class { long namespace; long ordinary; };"));

        output.ShouldContain("public partial class @class");
        output.ShouldContain("public int @namespace");
        output.ShouldContain("public int ordinary");
        output.ShouldNotContain("@ordinary");
    }

    [Fact]
    [Trait("Corpus", "C001")]
    public void EmitsOneFileScopedDocumentForEachGeneratedClass()
    {
        var documents = IdlCompiler.CompileSources(
            [CompilerTestSupport.Input("C001-01-primitives.idl", "module OracleP01 { struct Primitive { long value; }; };")],
            TestContext.Current.CancellationToken);

        documents.Keys.ShouldBe(new[]
        {
            "OracleP01.Primitive.g.cs",
            "OracleP01.Implementation.PrimitiveUnmanaged.g.cs",
            "OracleP01.Implementation.PrimitivePlugin.g.cs",
            "OracleP01.PrimitiveSupport.g.cs"
        });
        documents["OracleP01.Primitive.g.cs"].Source.ShouldContain("namespace OracleP01;");
        documents["OracleP01.Implementation.PrimitiveUnmanaged.g.cs"].Source.ShouldContain("namespace OracleP01.Implementation;");
        documents["OracleP01.Implementation.PrimitivePlugin.g.cs"].Source.ShouldContain("namespace OracleP01.Implementation;");
        documents["OracleP01.PrimitiveSupport.g.cs"].Source.ShouldContain("namespace OracleP01;");
        foreach (var document in documents.Values)
        {
            document.Source.ShouldNotContain("namespace OracleP01\n{");
        }
    }

    [Fact]
    [Trait("Corpus", "C001")]
    public void EmitsSortedRuntimeImportsAndReadableTypeSupport()
    {
        var documents = IdlCompiler.CompileSources(
        [CompilerTestSupport.Input("C001-01-primitives.idl", "module OracleP01 { struct Primitive { long value; }; };")],
        TestContext.Current.CancellationToken);

        var dataType = documents["OracleP01.Primitive.g.cs"];
        var unmanagedType = documents["OracleP01.Implementation.PrimitiveUnmanaged.g.cs"];
        var plugin = documents["OracleP01.Implementation.PrimitivePlugin.g.cs"];

        dataType.Source.ShouldContain("using Omg.Types;");
        dataType.Source.ShouldContain("#nullable enable");
        dataType.Source.ShouldNotContain("#nullable disable");
        unmanagedType.Source.ShouldContain("public struct PrimitiveUnmanaged : INativeTopicType<Primitive>");
        unmanagedType.Source.ShouldContain("public void FromNative(");
        unmanagedType.Source.ShouldNotContain("global::System");
        plugin.Source.ShouldContain("using Rti.Dds.Core;");
        plugin.Source.ShouldContain("ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic)");
        plugin.Source.ShouldNotContain("global::Rti.Dds.Core.ServiceEnvironment.Instance");
        plugin.Source.IndexOf("using Omg.Types;", StringComparison.Ordinal)
            .ShouldBeLessThan(plugin.Source.IndexOf("using Omg.Types.Dynamic;", StringComparison.Ordinal));
        plugin.Source.IndexOf("using Rti.Dds.Core;", StringComparison.Ordinal)
            .ShouldBeLessThan(plugin.Source.IndexOf("using Rti.Dds.NativeInterface.TypePlugin;", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Corpus", "C017")]
    public void SupportsSequencesOfStructsAndComposedCollectionAliases()
    {
        var documents = IdlCompiler.CompileSources([
            CompilerTestSupport.Input(
                "C017-05-shapes.idl",
                """
                module P05Compositions {
                    struct Item { long id; string<12> label; };
                    typedef long Row[3];
                    typedef Item ItemArray[2];
                    typedef sequence<long, 4> LongSequence;
                    typedef sequence<Row, 2> Rows;
                    typedef sequence<Item, 3> Items;
                    struct CollectionShapes {
                        sequence<Item> unboundedItems;
                        Items boundedItems;
                        ItemArray aggregateArray;
                        Row rows[2];
                        sequence<Row, 2> sequenceOfArrays;
                        sequence<Rows, 2> sequenceOfArrayAliases;
                        LongSequence sequences[2];
                    };
                };
                """
            )
        ], TestContext.Current.CancellationToken);

        var data = documents["P05Compositions.CollectionShapes.g.cs"].Source;
        data.ShouldContain("public ISequence<Item> unboundedItems { get; }");
        data.ShouldContain("public Items boundedItems { get; set; }");
        data.ShouldContain("public ItemArray aggregateArray { get; set; }");
        data.ShouldContain("public Row[] rows { get; set; }");
        data.ShouldContain("public ISequence<Row> sequenceOfArrays { get; }");
        data.ShouldContain("public ISequence<Rows> sequenceOfArrayAliases { get; }");
        data.ShouldContain("public LongSequence[] sequences { get; set; }");
        data.ShouldContain("new Sequence<Item>(other.unboundedItems.Select(element => new Item(element)))");
    }

    [Fact]
    [Trait("Corpus", "C044")]
    public void AcceptsTopicAnnotationsAndPreservesTheirMeaningInTheGeneratedDocumentation()
    {
        var documents = IdlCompiler.CompileSources([
            CompilerTestSupport.Input("C044-annotations.idl", "module CorpusAnnotations { @topic struct Sample { @key long id; }; };")
        ], TestContext.Current.CancellationToken);

        var sample = documents["CorpusAnnotations.Sample.g.cs"].Source;
        sample.ShouldContain("It is marked as a DDS topic type.");
        documents["CorpusAnnotations.Implementation.SamplePlugin.g.cs"].Source
            .ShouldContain("isKeyed: true");
    }
}
