using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Dds.Generator.Tests;

public sealed class GeneratedIncludeSpecs
{
    [Fact]
    [Trait("Corpus", "C048")]
    public void IncludedTypesProduceDeterministicSupportAndConsumerArtifacts()
    {
        // Arrange
        var inputs = new List<IdlInput> {
            new IdlInput("includes/common.idl", "struct IncludedType { long value; };", generate: false),
                Input("main.idl",
                    """
                    #include "includes/common.idl"
                    module IncludedConsumer {
                        struct Sample { IncludedType item; long value; };
                    };
                    """)
        };

        // Act
        var documents = CompileSources(inputs);

        // Assert
        documents.ShouldContainKey("IncludedType.g.cs");
        documents.ShouldContainKey("IncludedTypeSupport.g.cs");
        documents.ShouldContainKey("Implementation.IncludedTypePlugin.g.cs");
        documents.ShouldContainKey("Implementation.IncludedTypeUnmanaged.g.cs");
        documents.ShouldContainKey("IncludedConsumer.Sample.g.cs");

        var includedType = documents["IncludedType.g.cs"].Source;
        includedType.ShouldContain("public partial class IncludedType");

        var sample = documents["IncludedConsumer.Sample.g.cs"].Source;
        sample.ShouldContain("public IncludedType item");

        var includedTypePlugin = documents["Implementation.IncludedTypePlugin.g.cs"].Source;
        includedTypePlugin.ShouldContain("SetMemberAnnotations(0, annotations);");
    }
}

