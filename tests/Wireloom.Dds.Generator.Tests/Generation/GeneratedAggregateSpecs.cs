using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Dds.Generator.Tests;

public sealed class GeneratedAggregateSpecs
{
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
                @topic struct Sample { @key long id; string text; };
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
        unmanaged.ShouldContainInOrder(
            "sample.id = id;",
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
