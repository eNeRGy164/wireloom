using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Dds.Generator.Tests;

public sealed class GeneratedIdlEmissionSpecs
{
    [Fact]
    [Trait("Corpus", "C003")]
    public void MapsIdlPrimitiveTypesToTheExpectedManagedSurface()
    {
        // Arrange
        var input = Input("01-full-widths.idl",
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

        // Act
        var output = Compile(input);

        // Assert
        output.ShouldContainInOrder(
            "namespace OracleP01",
            "public short legacyShort",
            "public int legacyLong",
            "public long legacyLongLong",
            "public ushort unsignedShort",
            "public uint unsignedLong",
            "public ulong unsignedLongLong",
            "public sbyte fixedInt8",
            "public byte octetValue",
            "public bool boolValue",
            "public char charValue",
            "public char wcharValue",
            "public float floatValue",
            "public double doubleValue",
            "public LongDouble longDoubleValue");
    }

    [Fact]
    [Trait("Corpus", "C004")]
    public void EscapesOnlyCSharpKeywords()
    {
        // Arrange
        var input = Input("02-names-constants.idl",
            "struct class { long namespace; long ordinary; };");

        // Act
        var output = Compile(input);

        // Assert
        output.ShouldContainInOrder(
            "public partial class @class",
            "public int @namespace",
            "public int ordinary");
        output.ShouldNotContain("@ordinary");
    }

    [Fact]
    [Trait("Corpus", "C001")]
    public void EmitsOneFileScopedDocumentForEachGeneratedClass()
    {
        // Arrange
        var input = Input("01-primitives.idl",
        "module OracleP01 { struct Primitive { long value; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents.Keys.ShouldBe(new[]
        {
            "OracleP01.Primitive.g.cs",
            "OracleP01.Implementation.PrimitiveUnmanaged.g.cs",
            "OracleP01.Implementation.PrimitivePlugin.g.cs",
            "OracleP01.PrimitiveSupport.g.cs"
        });

        var data = documents["OracleP01.Primitive.g.cs"].Source;
        data.ShouldContain("namespace OracleP01;");

        var unmanaged = documents["OracleP01.Implementation.PrimitiveUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("namespace OracleP01.Implementation;");

        var plugin = documents["OracleP01.Implementation.PrimitivePlugin.g.cs"].Source;
        plugin.ShouldContain("namespace OracleP01.Implementation;");

        var support = documents["OracleP01.PrimitiveSupport.g.cs"].Source;
        support.ShouldContain("namespace OracleP01;");

        foreach (var document in documents.Values)
        {
            document.Source.ShouldNotContain("namespace OracleP01\n{");
        }
    }

    [Fact]
    [Trait("Corpus", "C001")]
    public void EmitsSortedRuntimeImportsAndReadableTypeSupport()
    {
        // Arrange
        var input = Input("01-primitives.idl",
            "module OracleP01 { struct Primitive { long value; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var dataType = documents["OracleP01.Primitive.g.cs"].Source;
        dataType.ShouldContain("using Omg.Types;");
        dataType.ShouldContain("#nullable enable");
        dataType.ShouldNotContain("#nullable disable");

        var unmanagedType = documents["OracleP01.Implementation.PrimitiveUnmanaged.g.cs"].Source;
        unmanagedType.ShouldContain("public struct PrimitiveUnmanaged : INativeTopicType<Primitive>");
        unmanagedType.ShouldContain("public void FromNative(");
        unmanagedType.ShouldNotContain("global::System");

        var plugin = documents["OracleP01.Implementation.PrimitivePlugin.g.cs"].Source;
        plugin.ShouldContain("using Rti.Dds.Core;");
        plugin.ShouldContain("ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic)");
        plugin.ShouldNotContain("global::Rti.Dds.Core.ServiceEnvironment.Instance");
        plugin.ShouldContainInOrder(
            "using Omg.Types;",
            "using Omg.Types.Dynamic;",
            "using Rti.Dds.Core;",
            "using Rti.Dds.NativeInterface.TypePlugin;");
    }

    [Fact]
    [Trait("Corpus", "C017")]
    public void SupportsSequencesOfStructsAndComposedCollectionAliases()
    {
        // Arrange
        var input = Input("05-shapes.idl",
            """
            module Compositions {
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
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var data = documents["Compositions.CollectionShapes.g.cs"].Source;
        data.ShouldContainInOrder(
            "public ISequence<Item> unboundedItems { get; }",
            "public Items boundedItems { get; set; }",
            "public ItemArray aggregateArray { get; set; }",
            "public Row[] rows { get; set; }",
            "public ISequence<Row> sequenceOfArrays { get; }",
            "public ISequence<Rows> sequenceOfArrayAliases { get; }",
            "public LongSequence[] sequences { get; set; }",
            "new Sequence<Item>(other.unboundedItems.Select(element => new Item(element)))");
    }

    [Fact]
    [Trait("Corpus", "C044")]
    public void AcceptsTopicAnnotationsAndPreservesTheirMeaningInTheGeneratedDocumentation()
    {
        // Arrange
        var input = Input("annotations.idl",
            "module Annotations { @topic struct Sample { @key long id; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var sample = documents["Annotations.Sample.g.cs"].Source;
        sample.ShouldContain("It is marked as a DDS topic type.");

        var plugin = documents["Annotations.Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldContain("isKeyed: true");
    }
}
