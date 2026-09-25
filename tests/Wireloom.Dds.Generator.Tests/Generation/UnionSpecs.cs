namespace Wireloom.Dds.Generator.Tests;

public sealed class UnionSpecs
{
    [Fact]
    public void EmitsUnionManagedNativePluginAndTypeSupportDocuments()
    {
        var documents = IdlCompiler.CompileSources([CompilerTestSupport.Input("union.idl",
            "module Example { union Choice switch(long) { case 1: case 5: long number; case 2: string text; default: boolean flag; }; };" )], TestContext.Current.CancellationToken);

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

        documents["Example.Implementation.ChoicePlugin.g.cs"].Source.ShouldContain("BuildUnion()");
        documents["Example.Implementation.ChoicePlugin.g.cs"].Source.ShouldContain("UnionMember.DefaultLabel");
        documents["Example.ChoiceSupport.g.cs"].Source.ShouldContain("TypeSupport<Choice>");
    }

    [Fact]
    public void EmitsEnumDiscriminatorAndUnionWithoutDefault()
    {
        var documents = IdlCompiler.CompileSources([CompilerTestSupport.Input("enum-union.idl", """
            module Example {
                enum Kind { Number, Text };
                union Choice switch(Kind) {
                    case Number: long number;
                    case Text: string text;
                };
            };
            """)], TestContext.Current.CancellationToken);

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

        documents["Example.Implementation.ChoicePlugin.g.cs"].Source
            .ShouldContain("WithDiscriminator(KindSupport.Instance.GetDynamicTypeInternal(isPublic))");
    }

    [Fact]
    public void UnionGeneratedBehaviorSelectsBranchesAndCopiesValues()
    {
        var documents = IdlCompiler.CompileSources([CompilerTestSupport.Input("behavior.idl", """
            module Example {
                union Choice switch(long) {
                    case 1: long number;
                    default: string text;
                };
            };
            """)], TestContext.Current.CancellationToken);
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
    public void AppendableEnumAndUnionUseExtensibleTypeSupport()
    {
        var documents = IdlCompiler.CompileSources([CompilerTestSupport.Input("appendable.idl", """
            module Example {
                @appendable enum Kind { Number, Text };
                @appendable union Choice switch(Kind) {
                    case Number: long number;
                    case Text: string text;
                };
            };
            """)], TestContext.Current.CancellationToken);

        documents["Example.Implementation.KindPlugin.g.cs"].Source
            .ShouldContain("WithExtensibility(ExtensibilityKind.Extensible)");
        documents["Example.Implementation.ChoicePlugin.g.cs"].Source
            .ShouldContain("WithExtensibility(ExtensibilityKind.Extensible)");
    }

    [Fact]
    public void NestedAppendableUnionIsAccepted()
    {
        var documents = IdlCompiler.CompileSources([CompilerTestSupport.Input("nested-union.idl", """
            module Example {
                enum Kind { Number, Text };
                @nested @appendable union Choice switch(Kind) {
                    case Number: long number;
                    case Text: string text;
                };
            };
            """)], TestContext.Current.CancellationToken);

        documents["Example.Choice.g.cs"].Source.ShouldContain("public partial class Choice");
    }
}
