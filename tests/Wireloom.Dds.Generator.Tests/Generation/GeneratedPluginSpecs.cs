using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Dds.Generator.Tests;

public sealed class GeneratedPluginSpecs
{
    [Fact]
    [Trait("Corpus", "C042")]
    public void MemberDefaultsAndRangesReachManagedNativeAndDynamicTypeOutputs()
    {
        // Arrange
        var input = Input("defaults-ranges.idl",
            """
            module DefaultsRanges {
                enum Color { GREEN, @default_literal RED, BLUE };
                struct Sample {
                    @min(0) @max(100) @default(50) long value;
                    @range(min = -32, max = 31) long ranged;
                    @default(BLUE) Color color;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["DefaultsRanges.Sample.g.cs"].Source;
        managed.ShouldContainInOrder(
            "private int _value;",
            "private int _ranged;",
            "public int value");
        managed.ShouldContain("ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);");
        managed.ShouldContain("ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 100);");
        managed.ShouldContain("ArgumentOutOfRangeException.ThrowIfLessThan(value, -32);");
        managed.ShouldContain("ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 31);");
        managed.ShouldContain("Its value must be between <c>0</c> and <c>100</c>. Its default value is <c>50</c>.");
        managed.ShouldContain("Its value must be between <c>-32</c> and <c>31</c>.");
        managed.ShouldContain("Its default value is <c>BLUE</c>.");
        managed.ShouldContainInOrder(
            "public Sample()",
            "value = 50;",
            "color = (Color)2;");
        managed.ShouldContainInOrder(
            "this._value = value;",
            "this._ranged = ranged;",
            "this.color = color;");
        managed.ShouldContainInOrder(
            "_value = other._value;",
            "_ranged = other._ranged;");

        var unmanaged = documents["DefaultsRanges.Implementation.SampleUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("value = 50;");
        unmanaged.ShouldContain("color = (Color)2;");

        var plugin = documents["DefaultsRanges.Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldContain("Int32Value = 50");
        plugin.ShouldContain("Int32Value = 0");
        plugin.ShouldContain("Int32Value = 100");
        plugin.ShouldContain("Int32Value = -32");
        plugin.ShouldContain("Int32Value = 31");
        plugin.ShouldContain("EnumValue = 2");
    }

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

