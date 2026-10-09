using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Wireloom;
using Wireloom.Dds.Generator.Tests;

using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Generation.Tests;

public sealed class GeneratedAggregateSpecs
{
    [Fact]
    public void ExtensibilityKindsExplainTheirTypeEvolutionBehavior()
    {
        // Arrange
        var input = Input("extensibility-summary.idl",
            "module ExtensibilitySummary { @final struct FinalSample { long value; }; @appendable struct AppendableSample { long value; }; @mutable struct MutableSample { long value; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["ExtensibilitySummary.FinalSample.g.cs"].Source.ShouldContain("The type's members and layout cannot be extended compatibly.");
        documents["ExtensibilitySummary.AppendableSample.g.cs"].Source.ShouldContain("New members may be appended while preserving the existing member order for compatible type evolution.");
        documents["ExtensibilitySummary.MutableSample.g.cs"].Source.ShouldContain("Members may be identified and reordered by DDS member IDs during compatible type evolution.");
    }

    [Fact]
    public void NativeMemberAccessQualifiesOnlyParametersThatShadowFields()
    {
        // Arrange
        var input = Input(
            "native-shadowing.idl",
            "module Shadowing { struct Item { long value; }; struct Sample { string<8> optionalsOnly; Item allocatePointers; sequence<long, 2> allocateMemory; long value; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var native = documents["Shadowing.Implementation.SampleUnmanaged.g.cs"].Source;
        native.ShouldContain("this.optionalsOnly.Destroy();");
        native.ShouldContain("allocatePointers.Destroy(optionalsOnly);");
        native.ShouldContain("allocateMemory.Destroy(optionalsOnly);");
        native.ShouldContain("this.allocatePointers.Initialize(allocatePointers, allocateMemory);");
        native.ShouldContain("this.allocateMemory.Initialize<int>(max: 2, absoluteMax: 2, allocateMemory);");
        native.ShouldContain("value = sample.value;");
        native.ShouldContain("sample.value = value;");
        native.ShouldNotContain("this.value.FromNative");
        native.ShouldNotContain("this.value = sample.value;");
    }

    [Fact]
    [Trait("Corpus", "C019")]
    public void InheritedAggregatesPreserveConstructorAndDestroyOrder()
    {
        // Arrange
        var input = Input("inheritance.idl",
            """
            module Inheritance {
                struct Base { long state; };
                struct Context { string correlation; };
                struct Derived : Base { string value; Context traceContext; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Inheritance.Derived.g.cs"].Source;
        managed.ShouldContain("public partial class Derived : Base");
        managed.ShouldContain("public Derived(int state, string value, Context traceContext) : base(state)");

        var unmanaged = documents["Inheritance.Implementation.DerivedUnmanaged.g.cs"].Source;
        unmanaged.ShouldContainInOrder(
            "parent.Destroy(optionalsOnly);",
            "traceContext.Destroy(optionalsOnly);",
            "if (optionalsOnly)",
            "value.Destroy();");

        var plugin = documents["Inheritance.Implementation.DerivedPlugin.g.cs"].Source;
        plugin.ShouldContain("TypeKind.String");
        plugin.ShouldContain("SetMemberAnnotations(0");
    }

    [Fact]
    [Trait("Corpus", "C020")]
    public void ValueTypesPreserveInheritanceAndAggregateComposition()
    {
        // Arrange
        var input = Input("valuetypes.idl",
            """
            module ValueTypes {
                valuetype BaseValue {
                    public long baseValue;
                };
                valuetype DerivedValue : BaseValue {
                    public string<16> name;
                };
                struct Holder { DerivedValue value; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var @base = documents["ValueTypes.BaseValue.g.cs"].Source;
        @base.ShouldContain("public partial class BaseValue");
        @base.ShouldContain("public int baseValue");

        var derived = documents["ValueTypes.DerivedValue.g.cs"].Source;
        derived.ShouldContain("public partial class DerivedValue : BaseValue");
        derived.ShouldContain("public DerivedValue(int baseValue, string name) : base(baseValue)");
        derived.ShouldContain("[Bound(16)]");

        var holder = documents["ValueTypes.Implementation.HolderUnmanaged.g.cs"].Source;
        holder.ShouldContain("private DerivedValueUnmanaged value");
        holder.ShouldContain("value.FromNative(sample.value, keysOnly: false);");
        holder.ShouldContain("value.ToNative(sample.value, keysOnly: false);");
    }

    [Fact]
    [Trait("Corpus", "C012")]
    [Trait("Corpus", "C013")]
    public void AggregateAndNestedAliasesPreserveTypedDelegationContracts()
    {
        // Arrange
        var input = Input("aliases.idl",
            """
            module Aliases {
                struct Item { long value; };
                typedef Item ItemAlias;
                typedef sequence<ItemAlias, 2> Items;
                typedef sequence<Items, 2> Nested;
                struct Holder { ItemAlias item; Nested values; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var itemAliasUnmanaged = documents["Aliases.Implementation.ItemAliasUnmanaged.g.cs"].Source;
        itemAliasUnmanaged.ShouldContain("Value.Destroy(optionalsOnly);");

        var itemsUnmanaged = documents["Aliases.Implementation.ItemsUnmanaged.g.cs"].Source;
        itemsUnmanaged.ShouldContain("Value.Destroy<ItemAlias, ItemAliasUnmanaged>(optionalsOnly);");

        var nestedUnmanaged = documents["Aliases.Implementation.NestedUnmanaged.g.cs"].Source;
        nestedUnmanaged.ShouldContain("Value.Destroy<Items, ItemsUnmanaged>(optionalsOnly);");

        var holder = documents["Aliases.Implementation.HolderUnmanaged.g.cs"].Source;
        holder.ShouldContain("item.Destroy(optionalsOnly);");
        holder.ShouldContain("values.Destroy(optionalsOnly);");

        var itemAlias = documents["Aliases.ItemAlias.g.cs"].Source;
        itemAlias.ShouldContain("public Item Value");

        var itemAliasPlugin = documents["Aliases.Implementation.ItemAliasPlugin.g.cs"].Source;
        itemAliasPlugin.ShouldContain("CreateAliasWithAccessInfo<ItemAliasUnmanaged>");

        var itemAliasSupport = documents["Aliases.ItemAliasSupport.g.cs"].Source;
        itemAliasSupport.ShouldContain("TypeSupport<ItemAlias>");
    }

    [Fact]
    [Trait("Corpus", "C030")]
    [Trait("Corpus", "C044")]
    public void KeyedTopicsPreserveKeyOnlyCopyAndMetadata()
    {
        // Arrange
        var input = Input("keyed.idl",
            """
            module Keyed {
                @topic struct Sample { @key long id; @key long tenant; string text; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Keyed.Sample.g.cs"].Source;
        managed.ShouldContain("[Key]");
        managed.ShouldContain("public int id");
        managed.ShouldContain("public string text");

        var unmanaged = documents["Keyed.Implementation.SampleUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("The operation copies values into <paramref name=\"sample\"/>. When <paramref name=\"keysOnly\"/> is true, only key members are copied.");
        unmanaged.ShouldContain("The operation copies values from <paramref name=\"sample\"/> into native storage. When <paramref name=\"keysOnly\"/> is true, only key members are copied.");
        unmanaged.ShouldNotContain("For an unkeyed type");
        unmanaged.ShouldContain("sample.id = id;\n        sample.tenant = tenant;");
        unmanaged.ShouldContainInOrder(
            "sample.id = id;",
            "sample.tenant = tenant;",
            "if (keysOnly)",
            "sample.text = text.FromNative();");

        var plugin = documents["Keyed.Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldContain("isKeyed: true");
        plugin.ShouldContain("isKey: true, id: 0");
        plugin.ShouldContain("id: 1");

        var sampleSupport = documents["Keyed.SampleSupport.g.cs"].Source;
        sampleSupport.ShouldContain("TypeSupport<Sample>");
    }

    [Fact]
    [Trait("Corpus", "C016")]
    [Trait("Corpus", "C019")]
    public void AggregateResourcesPreserveMemberAndDestroyOrder()
    {
        // Arrange
        var input = Input("aggregate-resources.idl",
            """
            module AggregateResources {
                struct Sample {
                    long first;
                    string text;
                    sequence<long, 2> values;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["AggregateResources.Sample.g.cs"].Source;
        managed.ShouldContain("public int first");
        managed.ShouldContain("public string text");
        managed.ShouldContain("public ISequence<int> values");
        managed.ShouldContain("public Sample(Sample? other)");

        var unmanaged = documents["AggregateResources.Implementation.SampleUnmanaged.g.cs"].Source;
        unmanaged.ShouldContainInOrder(
            "text.Destroy();",
            "values.Destroy(optionalsOnly);");

        var plugin = documents["AggregateResources.Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldContain("dtf.CreateString");
        plugin.ShouldContain("CreateSequenceWithAccessInfo");

        var sampleSupport = documents["AggregateResources.SampleSupport.g.cs"].Source;
        sampleSupport.ShouldContain("TypeSupport<Sample>");
    }

    [Fact]
    public void AggregateArrayCopyQualifiesTargetsThatShadowTheCopyParameter()
    {
        // Arrange
        var input = Input("aggregate-array-shadowing.idl",
            "module AggregateArrayShadowing { struct Item { long value; }; struct Sample { Item other[2]; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["AggregateArrayShadowing.Sample.g.cs"].Source;
        managed.ShouldContain("this.other[dimension0] = new Item(other.other[dimension0]);");
    }

    [Fact]
    public void AggregateArrayNamesMatchingDimensionLocalsQualifyManagedStorage()
    {
        // Arrange
        var input = Input(
            "dimension-array-shadowing.idl",
            "module DimensionArrayShadowing { struct Item { long value; }; struct Sample { Item dimension0[2]; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["DimensionArrayShadowing.Sample.g.cs"].Source;
        managed.ShouldContain("this.dimension0[dimension0] = new Item();");
        managed.ShouldContain("this.dimension0[dimension0] = new Item(other.dimension0[dimension0]);");
    }

    [Fact]
    public void OptionalAggregateCollectionsUseTypedNativeConversions()
    {
        // Arrange
        var input = Input("optional-aggregate-collections.idl",
            """
            module OptionalAggregateCollections {
                struct Item { long value; };
                struct Sample {
                    @optional Item items[2];
                    @optional sequence<Item, 4> values;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var unmanaged = documents["OptionalAggregateCollections.Implementation.SampleUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("items.FromNative<Item, ItemUnmanaged>(out Item[] itemsTemporary_, keysOnly: false, dimensions: new int[] { 2 });");
        unmanaged.ShouldContain("values.FromNative<Item, ItemUnmanaged>(out ISequence<Item> valuesTemporary_, keysOnly: false);");
        unmanaged.ShouldContain("items.ToNative<Item, ItemUnmanaged>(sample.items, keysOnly: false, dimension: 2);");
        unmanaged.ShouldContain("values.ToNative<Item, ItemUnmanaged>(sample.values, 4);");
        unmanaged.ShouldContain("values.Initialize<Item, ItemUnmanaged>();");
    }

    [Fact]
    public void OptionalAggregateMembersPreservePresenceAndSupportNestedAliases()
    {
        // Arrange
        var input = Input("optional-aggregate-members.idl",
            """
            module OptionalAggregateMembers {
                struct Payload { long value; };
                typedef Payload PayloadAlias;
                typedef PayloadAlias PayloadAliasChain;
                union Choice switch(long) { case 0: long number; case 1: @optional Payload payload; };
                typedef Choice ChoiceAlias;
                typedef ChoiceAlias ChoiceAliasChain;
                struct Sample {
                    @optional Payload payload;
                    @optional PayloadAliasChain nestedPayload;
                    @optional ChoiceAliasChain choice;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["OptionalAggregateMembers.Sample.g.cs"].Source;
        var choice = documents["OptionalAggregateMembers.Choice.g.cs"].Source;
        var unmanaged = documents["OptionalAggregateMembers.Implementation.SampleUnmanaged.g.cs"].Source;
        var plugin = documents["OptionalAggregateMembers.Implementation.SamplePlugin.g.cs"].Source;

        managed.ShouldContain("public Payload? payload { get; set; }");
        managed.ShouldContain("public Payload? nestedPayload { get; set; }");
        managed.ShouldContain("public Choice? choice { get; set; }");
        choice.ShouldContain("public Payload payload");
        choice.ShouldNotContain("[Optional]");
        managed.ShouldContain("other.payload is null ? null : new");
        managed.ShouldContain("other.nestedPayload is null ? null : new");
        managed.ShouldContain("other.choice is null ? null : new");

        unmanaged.ShouldContain("private NativeManagedOptional payload;");
        unmanaged.ShouldContain("private NativeManagedOptional nestedPayload;");
        unmanaged.ShouldContain("private NativeManagedOptional choice;");
        unmanaged.ShouldContain("payload.FromNative<");
        unmanaged.ShouldContain("out var payloadTemporary_");
        unmanaged.ShouldContain("payload.FromNative<Payload, Implementation.PayloadUnmanaged>(out var payloadTemporary_);\n        sample.payload = payloadTemporary_;\n\n        nestedPayload.FromNative<");
        unmanaged.ShouldContain("nestedPayload.FromNative<");
        unmanaged.ShouldContain("out var nestedPayloadTemporary_");
        unmanaged.ShouldContain("choice.FromNative<");
        unmanaged.ShouldContain("out var choiceTemporary_");
        unmanaged.ShouldContain("payload.ToNative<");
        unmanaged.ShouldContain("sample.payload");
        unmanaged.ShouldContain("nestedPayload.ToNative<");
        unmanaged.ShouldContain("sample.nestedPayload");
        unmanaged.ShouldContain("choice.ToNative<");
        unmanaged.ShouldContain("sample.choice");
        unmanaged.ShouldContain("payload.Destroy<");
        unmanaged.ShouldContain("nestedPayload.Destroy<");
        unmanaged.ShouldContain("choice.Destroy<");
        plugin.ShouldContain("new StructMember(\"payload\"");
        plugin.ShouldContain("new StructMember(\"nestedPayload\"");
        plugin.ShouldContain("new StructMember(\"choice\"");
        plugin.ShouldContain("new StructMember(\"nestedPayload\", PayloadAliasChainSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 1)");
        plugin.ShouldContain("new StructMember(\"choice\", ChoiceAliasChainSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 2)");
        plugin.ShouldContain("isOptional: true");
    }

    [Fact]
    [Trait("Corpus", "C021")]
    public void AggregateMembersNamedSampleQualifyNativeStorageWhenForwarding()
    {
        // Arrange
        var input = Input("alias-composition.idl",
            "module AggregateComposition { struct Sample { long value; }; struct Named { Sample sample; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var unmanaged = documents["AggregateComposition.Implementation.NamedUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("this.sample.FromNative(sample.sample, keysOnly: false);");
        unmanaged.ShouldContain("this.sample.ToNative(sample.sample, keysOnly: false);");
    }

    [Fact]
    public void KeyedAggregateMembersForwardKeysOnlyWhileQualifyingShadowedNativeStorage()
    {
        // Arrange
        var input = Input("keyed-aggregate-forwarding.idl",
            "module AggregateComposition { struct Item { long value; }; @topic struct Named { @key Item sample; long payload; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var unmanaged = documents["AggregateComposition.Implementation.NamedUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("this.sample.FromNative(sample.sample, keysOnly);");
        unmanaged.ShouldContain("this.sample.ToNative(sample.sample, keysOnly);");
    }

    [Fact]
    public void MembersNamedSampleQualifyNativeStorageAcrossConversionShapesAndKeyPasses()
    {
        // Arrange
        var input = Input("sample-shadowing.idl",
            """
            module SampleShadowing {
                struct Item { long value; };
                struct Primitive { long sample; };
                struct Text { string sample; };
                struct Sequence { sequence<long, 2> sample; };
                struct Array { long sample[2]; };
                struct Aggregate { Item sample; };
                @topic struct Keyed { @key long id; long keysOnly; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var primitive = documents["SampleShadowing.Implementation.PrimitiveUnmanaged.g.cs"].Source;
        primitive.ShouldContainInOrder(
            "sample.sample = this.sample;",
            "this.sample = sample.sample;");

        var text = documents["SampleShadowing.Implementation.TextUnmanaged.g.cs"].Source;
        text.ShouldContainInOrder(
            "sample.sample = this.sample.FromNative();",
            "this.sample.ToNative(sample.sample,");

        var sequence = documents["SampleShadowing.Implementation.SequenceUnmanaged.g.cs"].Source;
        sequence.ShouldContainInOrder(
            "this.sample.FromNative((Sequence<int>)sample.sample);",
            "this.sample.ToNative((Sequence<int>)sample.sample);");

        var array = documents["SampleShadowing.Implementation.ArrayUnmanaged.g.cs"].Source;
        array.ShouldContainInOrder(
            "this.sample.FromNative(sample.sample, dimension: 2);",
            "this.sample.ToNative<int>(sample.sample, dimension: 2);");

        var aggregate = documents["SampleShadowing.Implementation.AggregateUnmanaged.g.cs"].Source;
        aggregate.ShouldContainInOrder(
            "this.sample.FromNative(sample.sample, keysOnly: false);",
            "this.sample.ToNative(sample.sample, keysOnly: false);");

        var keyed = documents["SampleShadowing.Implementation.KeyedUnmanaged.g.cs"].Source;
        keyed.ShouldContainInOrder(
            "sample.id = id;",
            "if (keysOnly)",
            "sample.keysOnly = this.keysOnly;",
            "id = sample.id;",
            "this.keysOnly = sample.keysOnly;");
    }

    [Fact]
    [Trait("Corpus", "C035")]
    public void AppendableAggregatesEmitExtensibilityAcrossArtifacts()
    {
        // Arrange
        var input = Input(
                "appendable.idl",
                "module Appendable { @appendable struct Sample { string text; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Appendable.Sample.g.cs"].Source;
        managed.ShouldContain("It is marked as <c>extensible</c>");
        managed.ShouldContain("public string text");

        var sampleUnmanaged = documents["Appendable.Implementation.SampleUnmanaged.g.cs"].Source;
        sampleUnmanaged.ShouldContain("public void Destroy(bool optionalsOnly)");

        var samplePlugin = documents["Appendable.Implementation.SamplePlugin.g.cs"].Source;
        samplePlugin.ShouldContain("ExtensibilityKind.Extensible");

        var sampleSupport = documents["Appendable.SampleSupport.g.cs"].Source;
        sampleSupport.ShouldContain("TypeSupport<Sample>");
    }

    [Fact]
    [Trait("Corpus", "C022")]
    public void ConstructorsDistinguishEmptyManagedBodiesFromDiscriminatorInitialization()
    {
        // Arrange
        var input = Input("constructor-shapes.idl",
            """
            module ConstructorShapes {
                struct Empty {};
                union Choice switch(long) {
                    default: long value;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var emptySource = documents["ConstructorShapes.Empty.g.cs"].Source;

        var empty = FindClass(emptySource, "Empty");

        var emptyConstructor = empty.Members
            .OfType<ConstructorDeclarationSyntax>()
            .Single(c => c.ParameterList.Parameters.Count == 0);
        emptyConstructor.Body!.Statements.ShouldBeEmpty();

        var choiceSource = documents["ConstructorShapes.Choice.g.cs"].Source;

        var choice = FindClass(choiceSource, "Choice");

        var choiceConstructor = choice.Members
            .OfType<ConstructorDeclarationSyntax>()
            .Single(c => c.ParameterList.Parameters.Count == 0);
        choiceConstructor.Body!.Statements.Count.ShouldBe(1);
        choiceConstructor.Body.Statements[0].ToString().ShouldBe("Discriminator = DefaultDiscriminator;");
    }

    [Fact]
    [Trait("Corpus", "C019")]
    public void GetHashCodePreservesBaseAndMemberContributionOrder()
    {
        // Arrange
        var input = Input("hash-order.idl",
            """
            module HashOrder {
                struct Base { long first; };
                struct Derived : Base { string second; long third; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var @base = documents["HashOrder.Base.g.cs"].Source;
        @base.ShouldContainInOrder(
            "hash.Add(first);",
            "return hash.ToHashCode();");

        var derived = documents["HashOrder.Derived.g.cs"].Source;
        derived.ShouldContainInOrder(
            "hash.Add(base.GetHashCode());",
            "hash.Add(second);",
            "hash.Add(third);",
            "return hash.ToHashCode();");
    }

    private static ClassDeclarationSyntax FindClass(string source, string name) =>
        CSharpSyntaxTree.ParseText(source)
            .GetRoot()
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .Single(cd => cd.Identifier.ValueText == name);

}
