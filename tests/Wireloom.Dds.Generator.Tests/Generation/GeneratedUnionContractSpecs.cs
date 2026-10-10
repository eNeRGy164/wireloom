using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;
using Wireloom;
using Wireloom.Dds.Generator.Tests;

namespace Wireloom.Generation.Tests;

public sealed class GeneratedUnionContractSpecs
{
    [Fact]
    [Trait("Corpus", "C025")]
    public void MultiLabelUnionsPreserveLabelsAndNativeDiscriminatorBehavior()
    {
        // Arrange
        var input = Input("multi-label.idl",
            """
            module MultiLabel {
                union Choice switch(long) {
                    case 1: case 5: long number;
                    default: string text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["MultiLabel.Choice.g.cs"].Source;
        managed.ShouldContain("1 or 5 => global::System.HashCode.Combine(Discriminator, number)");
        managed.ShouldContain("public void Setnumber(int value, int discriminator)");
        managed.ShouldContain("Discriminator != 1 && Discriminator != 5");
        managed.ShouldContain("Assigning the <c>number</c> property selects the first label listed in its documentation.");
        managed.ShouldContain("An unmatched discriminator selects the default branch <see cref=\"text\"/>.");
        managed.ShouldContain("falls back to the default branch <see cref=\"text\"/>.");
        managed.ShouldContain("The concrete value of the active branch, including the default branch <see cref=\"text\"/> when no explicit label matches.");
        managed.ShouldContain("<exception cref=\"global::System.InvalidOperationException\">The discriminator selects an explicit branch.</exception>");
        managed.ShouldContain("Reading this property while another branch is active throws <see cref=\"global::System.InvalidOperationException\"/>.");
        managed.ShouldContain("choice.Setnumber(default!, 1);");
        managed.ShouldContain("var activeValue = choice.number;");
        managed.ShouldContain("activeValue = choice.number;");
        managed.ShouldContain("Reading choice.text here throws InvalidOperationException because that branch is inactive.");
        managed.ShouldContain("<exception cref=\"global::System.ArgumentException\">");

        var unmanaged = documents["MultiLabel.Implementation.ChoiceUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("sample.Setnumber(number, _discriminator);");
        unmanaged.ShouldNotContain("sample.Setnumber(sample.number = number, _discriminator);");

        var choicePlugin = documents["MultiLabel.Implementation.ChoicePlugin.g.cs"].Source;
        choicePlugin.ShouldContain("new int[] { 1, 5 }");

        var choiceSupport = documents["MultiLabel.ChoiceSupport.g.cs"].Source;
        choiceSupport.ShouldContain("TypeSupport<Choice>");
    }

    [Fact]
    public void CharacterUnionLabelsAreEscapedInXmlDocumentation()
    {
        // Arrange
        var input = Input("character-label.idl",
            """
            module CharacterLabel {
                union Choice switch(char) {
                    case '&': case '+': long symbol;
                    default: long other;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["CharacterLabel.Choice.g.cs"].Source;
        managed.ShouldContain("<c>'&amp;', '+'</c>");
        managed.ShouldContain("choice.Setsymbol(default!, '&amp;');");
        managed.ShouldNotContain("<c>'&'</c>");
    }

    [Fact]
    [Trait("Corpus", "C022")]
    [Trait("Corpus", "C024")]
    [Trait("Corpus", "C029")]
    public void ResourceUnionsDestroyEveryBranchAndAliasDelegates()
    {
        // Arrange
        var input = Input("resource-union.idl",
            """
            module ResourceUnion {
                struct Payload { long value; };
                union Choice switch(long) {
                    case 1: string text;
                    case 2: Payload payload;
                    case 3: sequence<long, 2> values;
                };
                typedef Choice ChoiceAlias;
                typedef ChoiceAlias ChoiceAlias2;
                struct Holder { ChoiceAlias2 value; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var union = documents["ResourceUnion.Implementation.ChoiceUnmanaged.g.cs"].Source;
        union.ShouldContainInOrder(
            "if (optionalsOnly)",
            "return;",
            "text.Destroy();",
            "payload.Destroy(optionalsOnly);",
            "values.Destroy(optionalsOnly);");

        var choiceAliasUnmanaged = documents["ResourceUnion.Implementation.ChoiceAliasUnmanaged.g.cs"].Source;
        choiceAliasUnmanaged.ShouldContain("Value.Destroy(optionalsOnly);");

        var choiceAlias2Unmanaged = documents["ResourceUnion.Implementation.ChoiceAlias2Unmanaged.g.cs"].Source;
        choiceAlias2Unmanaged.ShouldContain("Value.Destroy(optionalsOnly);");

        var holderUnmanaged = documents["ResourceUnion.Implementation.HolderUnmanaged.g.cs"].Source;
        holderUnmanaged.ShouldContain("value.Destroy(optionalsOnly);");

        var managed = documents["ResourceUnion.Choice.g.cs"].Source;
        managed.ShouldContain("public string text");
        managed.ShouldContain("public Payload payload");
        managed.ShouldContain("public ISequence<int> values");

        var plugin = documents["ResourceUnion.Implementation.ChoicePlugin.g.cs"].Source;
        plugin.ShouldContain("new UnionMember(\"text\"");
        plugin.ShouldContain("new UnionMember(\"payload\"");
        plugin.ShouldContain("new UnionMember(\"values\"");

        var choiceSupport = documents["ResourceUnion.ChoiceSupport.g.cs"].Source;
        choiceSupport.ShouldContain("TypeSupport<Choice>");
    }

    [Fact]
    [Trait("Corpus", "C023")]
    public void EnumDiscriminatorsUseEnumSupportMetadata()
    {
        // Arrange
        var input = Input("enum-union.idl",
            """
            module EnumUnion {
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
        var managed = documents["EnumUnion.Choice.g.cs"].Source;
        managed.ShouldContain("public Kind Discriminator");
        managed.ShouldContain("public const Kind DefaultDiscriminator = 0");

        var unmanaged = documents["EnumUnion.Implementation.ChoiceUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("private global::EnumUnion.Kind _discriminator");
        unmanaged.ShouldContain("public void FromNative");

        var plugin = documents["EnumUnion.Implementation.ChoicePlugin.g.cs"].Source;
        plugin.ShouldContain("WithDiscriminator(KindSupport.Instance.GetDynamicTypeInternal(isPublic)");

        var support = documents["EnumUnion.ChoiceSupport.g.cs"].Source;
        support.ShouldContain("TypeSupport<Choice>");
    }

    [Fact]
    [Trait("Corpus", "C027")]
    public void CharDiscriminatorsPreserveManagedAndNativeRepresentations()
    {
        // Arrange
        var input = Input("primitive-unions.idl",
            """
            module CharUnion {
                union Choice switch(char) {
                    case 'a': long letter;
                    case 'z': string text;
                    default: boolean other;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["CharUnion.Choice.g.cs"].Source;
        managed.ShouldContain("public char Discriminator");
        managed.ShouldContain("public const char DefaultDiscriminator = '\\0'");
        managed.ShouldContain("Discriminator != 'a'");
        managed.ShouldContain("Discriminator != 'z'");

        var unmanaged = documents["CharUnion.Implementation.ChoiceUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("private byte _discriminator");
        unmanaged.ShouldContain("switch (NativeChar.FromUtf8(_discriminator))");
        unmanaged.ShouldContain("_discriminator = NativeChar.ToUtf8(sample.Discriminator);");
        unmanaged.ShouldContain("_discriminator = NativeChar.ToUtf8(Choice.DefaultDiscriminator);");

        var plugin = documents["CharUnion.Implementation.ChoicePlugin.g.cs"].Source;
        plugin.ShouldContain("WithDiscriminator(dtf.GetPrimitiveType<char>())");
        plugin.ShouldContain("new int[] { 97 }");
        plugin.ShouldContain("new int[] { 122 }");
    }

    [Fact]
    public void WideCharacterDiscriminatorsPreserveNativeStorageAndConversions()
    {
        // Arrange
        var input = Input("wide-character-union.idl",
            """
            module WideCharacterUnion {
                union Choice switch(wchar) {
                    case 97: long letter;
                    default: string text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var unmanaged = documents["WideCharacterUnion.Implementation.ChoiceUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("private short _discriminator");
        unmanaged.ShouldContain("switch ((char)_discriminator)");
        unmanaged.ShouldContain("_discriminator = (short)sample.Discriminator;");
        unmanaged.ShouldContain("_discriminator = (short)Choice.DefaultDiscriminator;");
        var plugin = documents["WideCharacterUnion.Implementation.ChoicePlugin.g.cs"].Source;
        plugin.ShouldContain("WithDiscriminator(dtf.GetPrimitiveType<DynamicTypeFactory.WideCharType>())");
    }

    [Fact]
    public void OctetDiscriminatorsUseTheDdsPrimitiveDynamicType()
    {
        // Arrange
        var input = Input("octet-union.idl",
            """
            module OctetUnion {
                union Choice switch(octet) {
                    case 0: long zero;
                    default: string other;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var plugin = documents["OctetUnion.Implementation.ChoicePlugin.g.cs"].Source;
        plugin.ShouldContain("WithDiscriminator(dtf.GetPrimitiveType<Octet>())");
        plugin.ShouldNotContain("WithDiscriminator(dtf.GetPrimitiveType<byte>())");
    }

    [Fact]
    public void DefaultOnlyUnionsGenerateUnconditionalBranchAccess()
    {
        // Arrange
        var input = Input("default-only-union.idl", "union Choice switch(long) { default: long value; };");

        // Act
        var managed = CompileSources(input)["Choice.g.cs"].Source;

        // Assert
        managed.ShouldContain("An unmatched discriminator selects the default branch <see cref=\"value\"/>.");
        managed.ShouldContain("falls back to the default branch <see cref=\"value\"/>.");
        managed.ShouldNotContain("When neither discriminator selects a branch, equality depends on the discriminator alone.");
        managed.ShouldContain("if (false)");
        managed.ShouldNotContain("if ()");
    }

    [Fact]
    public void UnmatchedDiscriminatorsWithoutDefaultBranchesAreDocumentedAsInactive()
    {
        // Arrange
        var input = Input("unmatched-union.idl", "union Choice switch(long) { case 1: long value; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Choice.g.cs"].Source;
        managed.ShouldContain("If it matches no declared label, no branch is active.");
        managed.ShouldContain("If the default discriminator matches no declared label, no branch is active.");
        managed.ShouldContain("When neither discriminator selects a branch, equality depends on the discriminator alone.");

        var unmanaged = documents["Implementation.ChoiceUnmanaged.g.cs"].Source;
        unmanaged.ShouldNotContain("default:");

        var fromNativeStart = unmanaged.IndexOf("public void FromNative(", StringComparison.Ordinal);
        var toNativeStart = unmanaged.IndexOf("public void ToNative(", StringComparison.Ordinal);
        fromNativeStart.ShouldBeGreaterThanOrEqualTo(0);
        toNativeStart.ShouldBeGreaterThanOrEqualTo(0);
        var fromNative = unmanaged[fromNativeStart..toNativeStart];
        var toNative = unmanaged[toNativeStart..];
        fromNative.ShouldContain("break;\n        }\n    }");
        toNative.ShouldContain("break;\n        }\n    }");
    }

    [Fact]
    [Trait("Corpus", "C028")]
    public void BooleanDiscriminatorsPreserveManagedAndNativeRepresentations()
    {
        // Arrange
        var input = Input("primitive-unions.idl",
            """
            module BooleanUnion {
                union Choice switch(boolean) {
                    case TRUE: long enabled;
                    case FALSE: sequence<long, 2> disabled;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["BooleanUnion.Choice.g.cs"].Source;
        managed.ShouldContain("public bool Discriminator");
        managed.ShouldContain("public const bool DefaultDiscriminator = false");
        managed.ShouldContain("public object? Get()");
        managed.ShouldContain("!Discriminator");
        managed.ShouldNotContain("Discriminator != true");
        managed.ShouldNotContain("Discriminator != false");
        managed.ShouldNotContain("_ => null");
        managed.ShouldNotContain("_ => global::System.HashCode.Combine(Discriminator)");
        managed.ShouldNotContain("_ => true");
        managed.ShouldNotContain("default:");

        var unmanaged = documents["BooleanUnion.Implementation.ChoiceUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("private byte _discriminator");
        unmanaged.ShouldContain("switch (global::System.Convert.ToBoolean(_discriminator))");
        unmanaged.ShouldContain("_discriminator = global::System.Convert.ToByte(sample.Discriminator);");
        unmanaged.ShouldContain("_discriminator = global::System.Convert.ToByte(Choice.DefaultDiscriminator);");
        unmanaged.ShouldContain("if (sample.Discriminator != global::System.Convert.ToBoolean(_discriminator))");
        unmanaged.ShouldNotContain("default:");

        var plugin = documents["BooleanUnion.Implementation.ChoicePlugin.g.cs"].Source;
        plugin.ShouldContain("WithDiscriminator(dtf.GetPrimitiveType<bool>())");
        plugin.ShouldContain("new int[] { 1 }");
        plugin.ShouldContain("new int[] { 0 }");
    }

    [Fact]
    [Trait("Corpus", "C022")]
    public void DefaultUnionsEmitFallbackAndResourceCleanup()
    {
        // Arrange
        var input = Input("default-union.idl",
            """
            module DefaultUnion {
                union Choice switch(long) {
                    case 1: long number;
                    default: string text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["DefaultUnion.Choice.g.cs"].Source;
        managed.ShouldContain("DefaultDiscriminator");
        managed.ShouldContain("global::System.HashCode.Combine(Discriminator, text)");
        managed.ShouldContain("_ => text");

        var unmanaged = documents["DefaultUnion.Implementation.ChoiceUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("public void Destroy(bool optionalsOnly)");
        unmanaged.ShouldContain("text.Destroy();");

        var choicePlugin = documents["DefaultUnion.Implementation.ChoicePlugin.g.cs"].Source;
        choicePlugin.ShouldContain("UnionMember.DefaultLabel");

        var choiceSupport = documents["DefaultUnion.ChoiceSupport.g.cs"].Source;
        choiceSupport.ShouldContain("TypeSupport<Choice>");
    }

    [Fact]
    [Trait("Corpus", "C022")]
    [Trait("Corpus", "C029")]
    public void DestroyPreservesOptionalGuardAndDelegationOrder()
    {
        // Arrange
        var input = Input("destroy-order.idl",
            """
            module DestroyOrder {
                struct Payload { long value; };
                union Choice switch(long) {
                    case 1: string text;
                    case 2: Payload payload;
                };
                typedef Choice ChoiceAlias;
                typedef ChoiceAlias ChoiceAlias2;
                struct Holder { ChoiceAlias2 value; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var union = documents["DestroyOrder.Implementation.ChoiceUnmanaged.g.cs"].Source;
        union.ShouldContainInOrder(
            "if (optionalsOnly)",
            "return;",
            "text.Destroy();",
            "payload.Destroy(optionalsOnly);");

        var alias = documents["DestroyOrder.Implementation.ChoiceAliasUnmanaged.g.cs"].Source;
        alias.ShouldNotContain("if (optionalsOnly)");
        alias.ShouldContainInOrder(
            "public void Destroy(bool optionalsOnly)",
            "Value.Destroy(optionalsOnly);");

        var alias2 = documents["DestroyOrder.Implementation.ChoiceAlias2Unmanaged.g.cs"].Source;
        alias2.ShouldNotContain("if (optionalsOnly)");
        alias2.ShouldContain("Value.Destroy(optionalsOnly);");

        var holder = documents["DestroyOrder.Implementation.HolderUnmanaged.g.cs"].Source;
        holder.ShouldNotContain("if (optionalsOnly)");
        holder.ShouldContain("value.Destroy(optionalsOnly);");
    }
}
