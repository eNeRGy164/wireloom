using Wireloom;
using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Semantic.Tests;

public sealed class EnumValueResolutionSpecs
{
    [Fact]
    public void RejectsAnAbsoluteLiteralNameForAnEnumDefinedInANestedScope()
    {
        // Arrange
        var input = Input("wrong-absolute-enum-member.idl", """
            module Nested { enum Kind { Shared }; @default(::Shared) typedef Kind Alias; };
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Unknown default enum value");
    }

    [Fact]
    public void ResolvesAliasQualifiedUnionLabelsThroughMultipleTypedefs()
    {
        // Arrange
        var input = Input("enum-alias-label.idl", """
            enum Kind { First, Second };
            typedef Kind Alias;
            typedef Alias OuterAlias;
            union Choice switch(OuterAlias) { case OuterAlias::Second: long selected; default: long other; };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Choice.g.cs"].Source.ShouldContain("Kind.Second");
    }

    [Fact]
    public void PreservesAbsoluteEnumIdentityAcrossConstantsDefaultsAndUnionLabels()
    {
        // Arrange
        var input = Input("absolute-enum-values.idl", """
            enum Kind { Root, Shared };
            module Nested {
                enum Kind { Local, Shared };
                const ::Kind Selected = ::Kind::Shared;
                @default(::Kind::Shared) typedef ::Kind Alias;
                union Choice switch(::Kind) { case ::Kind::Shared: long selected; default: long other; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Nested.Choice.g.cs"].Source.ShouldContain("global::Kind.Shared");
        documents["Nested.Selected.g.cs"].Source.ShouldContain("global::Kind.Shared");
    }

    [Fact]
    public void RejectsUnionLabelsQualifiedByAnAliasToAnotherEnum()
    {
        // Arrange
        var input = Input("wrong-enum-alias-label.idl", """
            enum FirstKind { One, Shared };
            enum OtherKind { Two, Shared };
            typedef OtherKind OtherAlias;
            union Choice switch(FirstKind) { case OtherAlias::Shared: long selected; };
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Unknown union discriminator label");
    }
}
