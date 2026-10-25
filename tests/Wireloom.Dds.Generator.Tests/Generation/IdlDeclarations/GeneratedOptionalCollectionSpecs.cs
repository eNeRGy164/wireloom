using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;
using Wireloom;
using Wireloom.Dds.Generator.Tests;

namespace Wireloom.Generation.IdlDeclarations.Tests;

[Trait("Corpus", "C039")]
public sealed class GeneratedOptionalCollectionSpecs
{
    [Theory]
    [InlineData("struct Node { @optional Node children[2]; };", "Node[]?", "2")]
    [InlineData("struct Node { @optional Node children[2][3]; };", "Node[,]?", "2, 3")]
    [InlineData("typedef Node First; typedef First Second; struct Node { @optional Second children[2]; };", "Node[]?", "2")]
    public void EmitsRecursiveOptionalArraySupport(string source, string propertyType, string dimensions)
    {
        // Arrange
        var input = Input("optional-recursive-array.idl", $"module RecursiveArrays {{ {source} }};");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["RecursiveArrays.Node.g.cs"].Source.ShouldContain($"public {propertyType} children {{ get; set; }}");
        documents["RecursiveArrays.Implementation.NodeUnmanaged.g.cs"].Source.ShouldContain("private NativeManagedOptionalArray children;");
        var plugin = documents["RecursiveArrays.Implementation.NodePlugin.g.cs"].Source;
        plugin.ShouldContain("isRecursiveType = true");
        plugin.ShouldContain("CreateArrayWithAccessInfo<NodeUnmanaged>");
        plugin.ShouldContain("NodeSupport.GetOrCreateInstanceImpl().GetDynamicTypeInternal(isPublic)");
        plugin.ShouldContain("\n\n        isInitialized = true;\n\n        var members");
        plugin.ShouldNotContain("NodeSupport.Instance.GetDynamicTypeInternal(isPublic)");
        plugin.ShouldContain("\n\n        isInitialized = false;");
        plugin.ShouldContain("\n                .WithExtensibility(ExtensibilityKind.Extensible)");
        plugin.ShouldContain($"[{dimensions}]");
        plugin.ShouldNotContain("CreateSequenceWithAccessInfo");
        var managed = documents["RecursiveArrays.Node.g.cs"].Source;
        managed.ShouldContain("if (children is not null)");
        managed.ShouldContain("hash.Add(children[");
        managed.ShouldContain("hash.Add(-1);");
        managed.ShouldNotContain("children is null ? -1");
        var support = documents["RecursiveArrays.NodeSupport.g.cs"].Source;
        support.ShouldContain("internal static NodeSupport GetOrCreateInstanceImpl()");
        support.ShouldContain("Instance ??= ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NodeSupport, Node>();");
    }

    [Fact]
    public void ForwardsKeysOnlyForOptionalArraysOfKeyedAggregates()
    {
        // Arrange
        var input = Input("optional-keyed-array.idl",
            """
            module OptionalKeyedArrays {
                struct Child { @key long id; long value; };
                struct Parent { @optional Child children[2]; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var native = documents["OptionalKeyedArrays.Implementation.ParentUnmanaged.g.cs"].Source;
        native.ShouldContain("out Child[] childrenTemporary_, keysOnly, dimensions: [2]");
        native.ShouldContain("sample.children, keysOnly, dimension: 2");
        native.ShouldContain("dimension: 2, optionalsOnly);");
    }

    [Fact]
    public void EmitsOptionalSequenceArrayAndStringSurfaces()
    {
        // Arrange
        var input = Input("09-optional-collections.idl",
            """
            module OracleOptionalCollections {
                @appendable struct Message {
                    @id(1) @optional sequence<long, 4> values;
                    @id(2) @optional long matrix[2][3];
                    @id(3) @optional string<8> text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var source = documents["OracleOptionalCollections.Message.g.cs"].Source;
        source.ShouldContain("[Optional]\n    [Bound(4)]\n    public ISequence<int>? values { get; set; }");
        source.ShouldContain("sample.values = new Sequence&lt;int&gt;();");
        source.ShouldContain("sample.values.Add(default!);");
        source.ShouldContain("[Optional]\n    public int[,]? matrix { get; set; }");
        source.ShouldContain("[Optional]\n    [Bound(8)]\n    public string? text { get; set; }");
        source.ShouldNotContain("values = new Sequence<int>();");
        source.ShouldNotContain("matrix = new int[2, 3]");
    }

    [Fact]
    public void EmitsOptionalCollectionNativeStorageConversionAndMetadata()
    {
        // Arrange
        var input = Input("09-optional-collections.idl",
            """
            module OracleOptionalCollections {
                @appendable struct Message {
                    @id(1) @optional sequence<long, 4> values;
                    @id(2) @optional long matrix[2][3];
                    @id(3) @optional string<8> text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var unmanaged = documents["OracleOptionalCollections.Implementation.MessageUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("private NativeOptionalSeq values;");
        unmanaged.ShouldContain("private NativeUnmanagedOptionalArray matrix;");
        unmanaged.ShouldContain("values.Initialize();");
        unmanaged.ShouldContain("values.FromNative(out Sequence<int> valuesTemporary_);");
        unmanaged.ShouldContain("values.ToNative((Sequence<int>)sample.values!, 4);");
        unmanaged.ShouldContain("matrix.FromNative(out int[,] matrixTemporary_, dimensions: [2, 3]);");
        unmanaged.ShouldContain("matrix.ToNative(sample.matrix, 2 * 3);");
        unmanaged.ShouldContain("text.ToNativeOptional(sample.text, 8);");
        unmanaged.ShouldContain("values.Destroy(optionalsOnly);\n        matrix.Destroy(optionalsOnly);\n        text.Destroy();");
        unmanaged.ShouldNotContain("text.Initialize(size: 8");

        var plugin = documents["OracleOptionalCollections.Implementation.MessagePlugin.g.cs"].Source;
        plugin.ShouldContain("new StructMember(\"values\", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), 4), isOptional: true, id: 1)");
        plugin.ShouldContain("new StructMember(\"matrix\", tsf.CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), [2, 3]), isOptional: true, id: 2)");
        plugin.ShouldContain("new StructMember(\"text\", dtf.CreateString(8), isOptional: true, id: 3)");
    }

    [Fact]
    public void KeepsOptionalCollectionsNullSafeForCopiesEqualityAndHashing()
    {
        // Arrange
        var input = Input("09-optional-collections.idl",
            """
            module OracleOptionalCollections {
                @appendable struct Message {
                    @optional sequence<long, 4> values;
                    @optional long matrix[2][3];
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var source = documents["OracleOptionalCollections.Message.g.cs"].Source;
        source.ShouldContain("values is null ? null! : new Sequence<int>(other.values)");
        source.ShouldContain("matrix = ((int[,]?)other.matrix?.Clone())!;");
        source.ShouldContain("values?.Count ?? -1");
        source.ShouldContain("matrix is null ? -1 : matrix[0, 0]");
        source.ShouldContain("global::System.Object.ReferenceEquals(values, other.values)");
        source.ShouldContain("global::System.Object.ReferenceEquals(matrix, other.matrix)");
    }
}
