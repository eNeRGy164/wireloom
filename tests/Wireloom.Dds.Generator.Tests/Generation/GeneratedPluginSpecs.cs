using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Dds.Generator.Tests;

public sealed class GeneratedPluginSpecs
{
    [Fact]
    [Trait("Corpus", "C044")]
    public void PluginAnnotationsPreserveMemberIndexesAndPrimitiveMetadataOrder()
    {
        // Arrange
        var input = Input("plugin-annotations.idl",
            """
            module PluginAnnotations {
                @topic
                struct Sample {
                    @key long id;
                    string<8> text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var plugin = documents["PluginAnnotations.Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldContain("""internal SamplePlugin() : base("PluginAnnotations.Sample", isKeyed: true""");
        plugin.ShouldContain("""new StructMember("id", dtf.GetPrimitiveType<int>(), isKey: true, id: 0)""");
        plugin.ShouldContain("""new StructMember("text", dtf.CreateString(8), id: 1)""");
        plugin.ShouldContain("TypeKind.String");
        plugin.ShouldContain("StringValue = \"\"");
        plugin.ShouldContain("result.SetMemberAnnotations(1, annotations);");
        plugin.ShouldNotContain("result.SetMemberAnnotations(2, annotations);");
        plugin.ShouldContainInOrder(
            "new StructMember(\"id\"",
            "new StructMember(\"text\"",
            "new Annotations(",
            "TypeKind.String",
            "defaultValue:",
            "minValue:",
            "maxValue:",
            "result.SetMemberAnnotations(1, annotations);");
    }
}

