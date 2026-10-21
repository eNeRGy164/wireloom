using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;
using Wireloom;
using Wireloom.Dds.Generator.Tests;
using Wireloom.Compiler.Emission;

namespace Wireloom.Generation.Tests;

public sealed class GeneratedPluginSpecs
{
    /// <summary>Uses a free primitive discriminator for legacy fallback constructors and the type default for current constructors.</summary>
    [Theory]
    [InlineData("7.3.1", "long", "case 0: case 1: long selected; default: long fallback;", "", "2")]
    [InlineData("7.3.1", "Kind", "case 0: case 1: long selected; default: long fallback;", "typedef long Kind;", "2")]
    [InlineData("7.3.1", "long", "case 5: long selected; default: long fallback;", "", "0")]
    [InlineData("7.3.1", "long", "default: long fallback;", "", "0")]
    [InlineData("7.3.1", "boolean", "case FALSE: long selected; default: long fallback;", "", "true")]
    [InlineData("7.3.1", "boolean", "case TRUE: long selected; default: long fallback;", "", "false")]
    [InlineData("7.3.1", "char", "case '\\0': long selected; default: long fallback;", "", "(char)1")]
    [InlineData("7.3.1", "long", "case 5: long first; case 1: long second;", "", "5")]
    [InlineData("7.7.0", "long", "case 0: case 1: long selected; default: long fallback;", "", "0")]
    [InlineData("7.7.0", "boolean", "case FALSE: long selected; default: long fallback;", "", "false")]
    public void PrimitiveUnionConstructorsUseTheSelectedProfilesDefault(string version, string discriminator, string branches, string alias, string expected)
    {
        // Arrange
        var input = Input("primitive-union-default.idl", $$"""
            {{alias}}
            union Choice switch({{discriminator}}) { {{branches}} };
            """);
        var profile = RtiApiProfile.ForVersion(Version.Parse(version));

        // Act
        var documents = IdlCompiler.CompileSourcesWithDiagnostics([input], null, TestContext.Current.CancellationToken, profile);

        // Assert
        documents["Choice.g.cs"].Source.ShouldContain($"DefaultDiscriminator = {expected};");
        documents["Choice.g.cs"].Source.ShouldContain("Discriminator = DefaultDiscriminator;");
        documents["Implementation.ChoiceUnmanaged.g.cs"].Source.ShouldContain("Choice.DefaultDiscriminator");
    }

    /// <summary>Retains the enum constructor defaults observed with RTI 4.3.1, including default-only and aliased discriminators.</summary>
    [Theory]
    [InlineData("7.3.1", "", "default: long fallback;", "", 5)]
    [InlineData("7.3.1", "", "case C: long selected; default: long fallback;", "", 5)]
    [InlineData("7.3.1", "@default_literal", "case C: long selected; default: long fallback;", "", 5)]
    [InlineData("7.3.1", "@default_literal", "default: long fallback;", "typedef E Discriminator;", 5)]
    [InlineData("7.3.1", "", "case C: long selected;", "", 12)]
    [InlineData("7.7.0", "@default_literal", "default: long fallback;", "", 8)]
    [InlineData("7.7.0", "@default_literal", "case C: long selected; default: long fallback;", "typedef E Discriminator;", 8)]
    public void EnumUnionConstructorsUseTheSelectedProfilesDeclarationDefault(string version, string defaultAnnotation, string branches, string alias, int expected)
    {
        // Arrange
        var discriminator = alias.Length == 0 ? "E" : "Discriminator";
        var input = Input("enum-union-default.idl", $$"""
            enum E { A=5, {{defaultAnnotation}} B=8, C=12 };
            {{alias}}
            union Choice switch({{discriminator}}) { {{branches}} };
            """);
        var profile = RtiApiProfile.ForVersion(Version.Parse(version));

        // Act
        var documents = IdlCompiler.CompileSourcesWithDiagnostics([input], null, TestContext.Current.CancellationToken, profile);

        // Assert
        documents["Choice.g.cs"].Source.ShouldContain($"DefaultDiscriminator = (E){expected};");
        documents["Choice.g.cs"].Source.ShouldContain("Discriminator = DefaultDiscriminator;");
    }

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

    [Theory]
    [InlineData("7.3.1")]
    [InlineData("7.7.0")]
    [Trait("Corpus", "C040")]
    public void AutoIdHashAndHashIdAnnotationsProduceXTypesMemberIds(string version)
    {
        // Arrange
        var input = Input("autoid-hash.idl",
            """
            module AutoIdHash {
                @autoid(HASH) @appendable struct Sample {
                    @hashid("stable_id") long id;
                    @hashid string<16> text;
                };
            };
            """);

        // Act
        var documents = IdlCompiler.CompileSourcesWithDiagnostics([input], null, TestContext.Current.CancellationToken, RtiApiProfile.ForVersion(Version.Parse(version)));

        // Assert
        var managed = documents["AutoIdHash.Sample.g.cs"].Source;
        managed.ShouldContain("Its DDS member ID is generated from the hash of <c>stable_id</c> through <c>@hashid</c>.");
        managed.ShouldNotContain("MustUnderstand");
        managed.ShouldContain("Its DDS member ID is generated from the hash of <c>text</c> through <c>@hashid</c>.");

        var plugin = documents["AutoIdHash.Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldContain("new StructMember(\"id\", dtf.GetPrimitiveType<int>(), id: 75475440)");
        plugin.ShouldContain("new StructMember(\"text\", dtf.CreateString(16), id: 206680604)");
    }

    [Fact]
    [Trait("Corpus", "C142")]
    public void MustUnderstandIsRejectedByRtiddsgen431ProfileAndAcceptedByRtiddsgen470Profile()
    {
        // Arrange
        var input = Input("must-understand-profile.idl", "struct Sample { @must_understand long value; };");
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var oldProfileOutputs = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken, RtiApiProfile.Rtiddsgen431);
        var currentProfileOutputs = IdlCompiler.CompileSourcesWithDiagnostics([input], null, TestContext.Current.CancellationToken, RtiApiProfile.Rtiddsgen470);

        // Assert
        oldProfileOutputs.ShouldBeEmpty();
        diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0004");
        currentProfileOutputs.Values.Single(output => output.HintName.Contains("SamplePlugin", StringComparison.Ordinal)).Source.ShouldContain("isMustUnderstand: true");
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
                @appendable struct Derived : Base { Context traceContext; };
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
