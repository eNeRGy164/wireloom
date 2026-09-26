using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Dds.Generator.Tests;

public sealed class UnionSpecs
{
    [Fact]
    public void EmitsUnionManagedNativePluginAndTypeSupportDocuments()
    {
        // Arrange
        var input = Input("union.idl",
            "module Example { union Choice switch(long) { case 1: case 5: long number; case 2: string text; default: boolean flag; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents.Keys.ShouldBe([
            "Example.Choice.g.cs",
            "Example.Implementation.ChoiceUnmanaged.g.cs",
            "Example.Implementation.ChoicePlugin.g.cs",
            "Example.ChoiceSupport.g.cs"]);

        var managed = documents["Example.Choice.g.cs"].Source;
        managed.ShouldContain("public int number");
        managed.ShouldContain("public string text");
        managed.ShouldContain("public bool flag");
        managed.ShouldContain("Setflag");
        managed.ShouldContain("case 1:");
        managed.ShouldContain("case 5:");
        managed.ShouldContain("throw new InvalidOperationException");

        var native = documents["Example.Implementation.ChoiceUnmanaged.g.cs"].Source;
        native.ShouldContain("FromNative");
        native.ShouldContain("ToNative");
        native.ShouldContain("Initialize");
        native.ShouldContain("Destroy");

        var plugin = documents["Example.Implementation.ChoicePlugin.g.cs"].Source;
        plugin.ShouldContain("BuildUnion()");
        plugin.ShouldContain("UnionMember.DefaultLabel");

        var support = documents["Example.ChoiceSupport.g.cs"].Source;
        support.ShouldContain("TypeSupport<Choice>");
    }

    [Fact]
    public void MultiLabelUnionAccessorsAndFromNativePreserveTheNativeDiscriminator()
    {
        // Arrange
        var input = Input("multi-label-union.idl",
            """
            module Example {
                union Choice switch(long) {
                    case 1: case 5: long number;
                    default: boolean flag;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Example.Choice.g.cs"].Source;
        managed.ShouldContain("Discriminator != 1 && Discriminator != 5");
        managed.ShouldContain("public void Setnumber(int value, int discriminator)");

        var native = documents["Example.Implementation.ChoiceUnmanaged.g.cs"].Source;
        native.ShouldContain("sample.Setnumber(number, _discriminator);");
        native.ShouldNotContain("sample.Setnumber(sample.number = number, _discriminator);");
    }

    [Fact]
    public void UnionDestroyReleasesAllResourceBearingBranchesAndHolderDelegatesDirectly()
    {
        // Arrange
        var input = Input("union-destroy.idl",
            """
            module Example {
                struct Payload { long value; };
                union Choice switch(long) {
                    case 1: string text;
                    case 2: Payload payload;
                };
                typedef Choice ChoiceAlias;
                struct Holder { ChoiceAlias value; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var union = documents["Example.Implementation.ChoiceUnmanaged.g.cs"].Source;
        union.ShouldContain("text.Destroy();");
        union.ShouldContain("payload.Destroy(optionalsOnly);");

        var choiceAlias = documents["Example.Implementation.ChoiceAliasUnmanaged.g.cs"].Source;
        choiceAlias.ShouldContain("Value.Destroy(optionalsOnly);");
        choiceAlias.ShouldNotContain("if (optionalsOnly)");

        var holder = documents["Example.Implementation.HolderUnmanaged.g.cs"].Source;
        holder.ShouldContain("value.Destroy(optionalsOnly);");
        holder.ShouldNotContain("if (optionalsOnly)");
    }

    [Fact]
    public void EmitsEnumDiscriminatorAndUnionWithoutDefault()
    {
        // Arrange
        var input = Input("enum-union.idl",
            """
            module Example {
                enum Kind { Number, Text };
                union Choice switch(Kind) {
                    case Number: long number;
                    case Text: string text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Example.Choice.g.cs"].Source;
        managed.ShouldContain("Discriminator { get; private set; }");
        managed.ShouldContain("public Kind Discriminator { get; private set; }");
        managed.ShouldContain("public const Kind DefaultDiscriminator = 0;");
        managed.ShouldContain("Discriminator != Kind.Number");
        managed.ShouldContain("Discriminator = Kind.Number;");
        managed.ShouldNotContain("global::Example.Kind");
        managed.ShouldNotContain("Settext");
        managed.ShouldContain("number");
        managed.ShouldContain("text");

        var plugin = documents["Example.Implementation.ChoicePlugin.g.cs"].Source;
        plugin.ShouldContain("WithDiscriminator(KindSupport.Instance.GetDynamicTypeInternal(isPublic))");
    }

    [Fact]
    public void UnionGeneratedBehaviorSelectsBranchesAndCopiesValues()
    {
        // Arrange
        var input = Input("behavior.idl",
            """
            module Example {
                union Choice switch(long) {
                    case 1: long number;
                    default: string text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Example.Choice.g.cs"].Source;

        managed.ShouldContain("Discriminator = 1;");
        managed.ShouldContain("Discriminator = discriminator;");
        managed.ShouldContain("return _number;");
        managed.ShouldContain("return _text;");
        managed.ShouldContain("public Choice(Choice? other)");
        managed.ShouldContain("if (Discriminator != 1)\n            {");
        managed.ShouldContain("Discriminator = other.Discriminator;\n\n");
        managed.ShouldContain("switch (Discriminator)");
        managed.ShouldNotContain("throw new InvalidOperationException(\"number not selected\");\n\n            return _number;");
    }

    [Fact]
    public void UnionConstructorDoesNotInitializeInactiveAggregateBranches()
    {
        // Arrange
        var input = Input("struct-union.idl",
            """
            module Example {
                struct Payload { long value; };
                union Choice switch(long) {
                    case 10: Payload payload;
                    case 11: sequence<long, 4> values;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Example.Choice.g.cs"].Source;
        var constructorStart = managed.IndexOf("public Choice()", StringComparison.Ordinal);
        constructorStart.ShouldBeGreaterThanOrEqualTo(0);
        var constructor = managed[constructorStart..managed.IndexOf("public Choice(Choice? other)", constructorStart, StringComparison.Ordinal)];

        constructor.ShouldContain("Discriminator = DefaultDiscriminator;");
        constructor.ShouldNotContain("new global::Example.Payload()");
        constructor.ShouldNotContain("new Sequence<long>");
    }

    [Fact]
    public void UnionFromNativeInitializesActiveAggregateBranchesWhenTheDiscriminatorChanges()
    {
        // Arrange
        var input = Input("union-from-native.idl",
            """
            module Example {
                struct Payload { long value; };
                union Choice switch(long) {
                    case 10: Payload payload;
                    case 11: sequence<long, 4> values;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var native = documents["Example.Implementation.ChoiceUnmanaged.g.cs"].Source;

        native.ShouldContain("if (sample.Discriminator != _discriminator)");
        native.ShouldContain("sample.payload = new ");
        native.ShouldContain("payload.FromNative(sample.payload, keysOnly: false);");
        native.ShouldContain("sample.values = new Sequence<int>();");
        native.ShouldContain("values.FromNative((Sequence<int>)sample.values);");
    }

    [Fact]
    public void AppendableEnumAndUnionUseExtensibleTypeSupport()
    {
        // Arrange
        var input = Input("appendable.idl",
            """
            module Example {
                @appendable enum Kind { Number, Text };
                @appendable union Choice switch(Kind) {
                    case Number: long number;
                    case Text: string text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var kindPlugin = documents["Example.Implementation.KindPlugin.g.cs"].Source;
        kindPlugin.ShouldContain("WithExtensibility(ExtensibilityKind.Extensible)");

        var choicePlugin = documents["Example.Implementation.ChoicePlugin.g.cs"].Source;
        choicePlugin.ShouldContain("WithExtensibility(ExtensibilityKind.Extensible)");
    }

    [Fact]
    public void NestedAppendableUnionIsAccepted()
    {
        // Arrange
        var input = Input("nested-union.idl",
            """
            module Example {
                enum Kind { Number, Text };
                @nested @appendable union Choice switch(Kind) {
                    case Number: long number;
                    case Text: string text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var choice = documents["Example.Choice.g.cs"].Source;
        choice.ShouldContain("public partial class Choice");
    }
}
