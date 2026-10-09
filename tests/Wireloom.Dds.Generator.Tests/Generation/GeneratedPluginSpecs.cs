using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;
using Wireloom;
using Wireloom.Dds.Generator.Tests;

namespace Wireloom.Generation.Tests;

public sealed class GeneratedPluginSpecs
{
    [Fact]
    public void MaximumOnlyRangesAreDocumentedAndValidatedByTheGeneratedProperty()
    {
        // Arrange
        var input = Input("maximum-only-range.idl",
            "module MaximumOnlyRange { struct Sample { @max(10) long value; }; };");

        // Act
        var managed = CompileSources(input)["MaximumOnlyRange.Sample.g.cs"].Source;

        // Assert
        managed.ShouldContain("Valid values are no greater than <c>10</c>.");
        managed.ShouldContain("<exception cref=\"global::System.ArgumentOutOfRangeException\">The assigned value is greater than 10.</exception>");
        managed.ShouldContain("ThrowIfGreaterThan(value, 10);");
        managed.ShouldContain("this.value = value;");
    }

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
        managed.ShouldContain("global::System.ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);");
        managed.ShouldContain("global::System.ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 100);");
        managed.ShouldContain("global::System.ArgumentOutOfRangeException.ThrowIfLessThan(value, -32);");
        managed.ShouldContain("global::System.ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 31);");
        managed.ShouldContain("Valid values are in the inclusive range <c>0</c> through <c>100</c>. Its default value is <c>50</c>.");
        managed.ShouldContain("Valid values are in the inclusive range <c>-32</c> through <c>31</c>.");
        managed.ShouldContain("Its default value is <c>BLUE</c>.");
        managed.ShouldContainInOrder(
            "public Sample()",
            "value = 50;",
            "color = (Color)(2);");
        managed.ShouldContainInOrder(
            "this.value = value;",
            "this.ranged = ranged;",
            "this.color = color;");
        managed.ShouldContain("<exception cref=\"global::System.ArgumentOutOfRangeException\">A supplied member value is outside its declared range.</exception>");
        managed.ShouldContainInOrder(
            "_value = other._value;",
            "_ranged = other._ranged;");

        var unmanaged = documents["DefaultsRanges.Implementation.SampleUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("value = 50;");
        unmanaged.ShouldContain("color = (Color)(2);");

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

    [Fact]
    [Trait("Corpus", "C046")]
    public void RtiGenerationAnnotationsReachTheExpectedDynamicTypeMetadata()
    {
        // Arrange
        var input = Input("annotations-rti.idl",
            """
            module RtiAnnotations {
                @language_binding(PLAIN)
                @transfer_mode(INBAND)
                @topic struct Sample {
                    @key long id;
                    @range(min = -32, max = 31) @unit("meters") long value;
                    @resolve_name(false) string<16> text;
                    @external string<16> externalText;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["RtiAnnotations.Sample.g.cs"].Source;
        managed.ShouldContain("global::System.ArgumentOutOfRangeException.ThrowIfLessThan(value, -32);");
        managed.ShouldContain("global::System.ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 31);");

        var plugin = documents["RtiAnnotations.Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldContain("new StructMember(\"value\", dtf.GetPrimitiveType<int>(), id: 1)");
        plugin.ShouldContain("unit: \"meters\"");
        plugin.ShouldContain("result.SetMemberAnnotations(0, annotations);");
        plugin.ShouldContain("result.SetMemberAnnotations(1, annotations);");
        plugin.ShouldContain("result.SetMemberAnnotations(2, annotations);");
        plugin.ShouldNotContain("result.SetMemberAnnotations(3, annotations);");
    }

    [Fact]
    [Trait("Corpus", "C040")]
    public void AutoIdHashAndHashIdAnnotationsProduceXTypesMemberIds()
    {
        // Arrange
        var input = Input("autoid-hash.idl",
            """
            module AutoIdHash {
                @autoid(HASH) @appendable struct Sample {
                    @must_understand @hashid("stable_id") long id;
                    @hashid string<16> text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["AutoIdHash.Sample.g.cs"].Source;
        managed.ShouldContain("Its DDS member ID is generated from the hash of <c>stable_id</c> through <c>@hashid</c>.");
        managed.ShouldContain("A DDS reader that does not understand this member cannot safely read the sample.");
        managed.ShouldContain("Its DDS member ID is generated from the hash of <c>text</c> through <c>@hashid</c>.");

        var plugin = documents["AutoIdHash.Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldContain("new StructMember(\"id\", dtf.GetPrimitiveType<int>(), isMustUnderstand: true, id: 75475440)");
        plugin.ShouldContain("new StructMember(\"text\", dtf.CreateString(16), id: 206680604)");
    }

    [Fact]
    [Trait("Corpus", "C040")]
    public void AutoIdHashDocumentsImplicitMemberIdsAsAutoGenerated()
    {
        // Arrange
        var input = Input(
            "C040-autoid-hash-implicit-member.idl",
            "@autoid(HASH) struct Sample { long value; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Sample.g.cs"].Source.ShouldContain("Its DDS member ID is generated from the hash of <c>value</c> through <c>@autoid(HASH)</c>.");
        documents["Implementation.SamplePlugin.g.cs"].Source.ShouldContain("new StructMember(\"value\", dtf.GetPrimitiveType<int>(), id:");
    }

    [Fact]
    [Trait("Corpus", "C043")]
    public void AllowedDataRepresentationAnnotationDoesNotChangeGeneratedContractShape()
    {
        // Arrange
        var input = Input("allowed-data-representation.idl",
            """
            module AllowedDataRepresentation {
                @allowed_data_representation(XCDR2) @appendable struct Sample {
                    @id(1) long value;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["AllowedDataRepresentation.Sample.g.cs"].Source;
        managed.ShouldContain("public partial class Sample");
        managed.ShouldContain("public int value");

        var plugin = documents["AllowedDataRepresentation.Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldContain("ExtensibilityKind.Extensible");
        plugin.ShouldContain("new StructMember(\"value\", dtf.GetPrimitiveType<int>(), id: 1)");
    }

    [Fact]
    [Trait("Corpus", "C110")]
    public void AliasInheritancePreservesScalarAliasAnnotationsAndDerivedParent()
    {
        var input = Input("alias-inheritance.idl",
            """
            @default_nested
            module AliasInheritance {
                typedef string<16> Producer;
                @appendable struct Context { @optional Producer producer; };
                @appendable struct Base { long baseValue; };
                @appendable struct Derived : Base { Context context; };
            };
            """);

        var documents = CompileSources(input);

        var producerPlugin = documents["AliasInheritance.Implementation.ProducerPlugin.g.cs"].Source;
        producerPlugin.ShouldContain("using var dtString = dtf.CreateString(16);");
        producerPlugin.ShouldContain("\"Producer\", dtString);");
        producerPlugin.ShouldContain("TypeKind.String,");
        producerPlugin.ShouldContain("defaultValue: new AnnotationParameterValue { StringValue = \"\" },");
        producerPlugin.ShouldContain("aliasType.SetAnnotations(annotations);");

        var contextPlugin = documents["AliasInheritance.Implementation.ContextPlugin.g.cs"].Source;
        contextPlugin.ShouldContain("ProducerSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true");

        var derivedPlugin = documents["AliasInheritance.Implementation.DerivedPlugin.g.cs"].Source;
        derivedPlugin.ShouldContain(".WithParent((StructType) BaseSupport.Instance.GetDynamicTypeInternal(isPublic))");
    }
}
