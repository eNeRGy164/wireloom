using System.Linq;
using Wireloom;
using Shouldly;

namespace Wireloom.Dds.Generator.Tests;

[Trait("Corpus", "C039")]
public sealed class GeneratedOptionalCollectionSpecs
{
    [Fact]
    public void EmitsOptionalSequenceArrayAndStringSurfaces()
    {
        var documents = IdlCompiler.CompileSources([
            CompilerTestSupport.Input(
                "C039-09-optional-collections.idl",
                """
                module OracleOptionalCollections {
                    @appendable struct Message {
                        @id(1) @optional sequence<long, 4> values;
                        @id(2) @optional long matrix[2][3];
                        @id(3) @optional string<8> text;
                    };
                };
                """
            )
        ], TestContext.Current.CancellationToken);

        var source = documents["OracleOptionalCollections.Message.g.cs"].Source;

        source.ShouldContain("[Optional]\n    [Bound(4)]\n    public ISequence<int> values { get; set; }");
        source.ShouldContain("[Optional]\n    public int[,] matrix { get; set; }");
        source.ShouldContain("[Optional]\n    [Bound(8)]\n    public string? text { get; set; }");
        source.ShouldNotContain("values = new Sequence<int>();");
        source.ShouldNotContain("matrix = new int[2, 3]");
    }

    [Fact]
    public void EmitsOptionalCollectionNativeStorageConversionAndMetadata()
    {
        var documents = IdlCompiler.CompileSources([
            CompilerTestSupport.Input(
                "C039-09-optional-collections.idl",
                """
                module OracleOptionalCollections {
                    @appendable struct Message {
                        @id(1) @optional sequence<long, 4> values;
                        @id(2) @optional long matrix[2][3];
                        @id(3) @optional string<8> text;
                    };
                };
                """
            )
        ], TestContext.Current.CancellationToken);

        var unmanaged = documents["OracleOptionalCollections.Implementation.MessageUnmanaged.g.cs"].Source;
        var plugin = documents["OracleOptionalCollections.Implementation.MessagePlugin.g.cs"].Source;

        unmanaged.ShouldContain("private NativeOptionalSeq values;");
        unmanaged.ShouldContain("private NativeUnmanagedOptionalArray matrix;");
        unmanaged.ShouldContain("values.Initialize();");
        unmanaged.ShouldContain("values.FromNative<int>(out Sequence<int> valuesTemporary_);");
        unmanaged.ShouldContain("values.ToNative<int>((Sequence<int>)sample.values, 4);");
        unmanaged.ShouldContain("matrix.FromNative<int>(out int[,] matrixTemporary_, dimensions: new int[] { 2, 3 });");
        unmanaged.ShouldContain("matrix.ToNative<int>(sample.matrix, dimension: 2 * 3);");
        unmanaged.ShouldContain("text.ToNativeOptional(sample.text, 8);");
        unmanaged.ShouldContain("values.Destroy(optionalsOnly);\n        matrix.Destroy(optionalsOnly);\n        text.Destroy();");
        unmanaged.ShouldNotContain("text.Initialize(size: 8");
        plugin.ShouldContain("new StructMember(\"values\", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), 4), isOptional: true, id: 1)");
        plugin.ShouldContain("new StructMember(\"matrix\", tsf.CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), new uint[] { 2, 3 }), isOptional: true, id: 2)");
        plugin.ShouldContain("new StructMember(\"text\", dtf.CreateString(8), isOptional: true, id: 3)");
    }

    [Fact]
    public void KeepsOptionalCollectionsNullSafeForCopiesEqualityAndHashing()
    {
        var source = IdlCompiler.CompileSources([
            CompilerTestSupport.Input(
                "C039-09-optional-collections.idl",
                """
                module OracleOptionalCollections {
                    @appendable struct Message {
                        @optional sequence<long, 4> values;
                        @optional long matrix[2][3];
                    };
                };
                """
            )
        ], TestContext.Current.CancellationToken)["OracleOptionalCollections.Message.g.cs"].Source;

        source.ShouldContain("values is null ? null! : new Sequence<int>(other.values)");
        source.ShouldContain("matrix is null ? null! : (int[,])other.matrix.Clone()");
        source.ShouldContain("values?.Count ?? -1");
        source.ShouldContain("matrix?.Length ?? -1");
        source.ShouldContain("ReferenceEquals(values, other.values)");
        source.ShouldContain("ReferenceEquals(matrix, other.matrix)");
    }
}
