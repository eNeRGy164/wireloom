using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;
using Wireloom;
using Wireloom.Dds.Generator.Tests;

namespace Wireloom.Generation.Tests;

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

        var sampleUnmanaged = documents["IncludedConsumer.Implementation.SampleUnmanaged.g.cs"].Source;
        sampleUnmanaged.ShouldContain("global::Implementation.IncludedTypeUnmanaged item;");
        sampleUnmanaged.ShouldNotContain("private IncludedTypeUnmanaged item;");

        var includedTypePlugin = documents["Implementation.IncludedTypePlugin.g.cs"].Source;
        includedTypePlugin.ShouldContain("SetMemberAnnotations(0, annotations);");
    }
}
