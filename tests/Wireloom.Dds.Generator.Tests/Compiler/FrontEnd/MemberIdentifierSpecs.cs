using Wireloom;
using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Semantic.Tests;

/// <summary>Verifies member-ID assignment independently of declaration indexes.</summary>
public sealed class MemberIdentifierSpecs
{
    [Theory]
    [InlineData("@autoid(HASH) union Choice switch(long) { case 0: long hashed; default: long other; };", "Implementation.ChoicePlugin.g.cs")]
    [InlineData("@autoid(HASH) module Scope { union Choice switch(long) { case 0: long hashed; default: long other; }; };", "Scope.Implementation.ChoicePlugin.g.cs")]
    public void AppliesHashIdsToUnionBranches(string declaration, string document)
    {
        // Arrange
        var input = Input("union-hash-ids.idl", declaration);

        // Act
        var source = CompileSources(input)[document].Source;

        // Assert
        var member = source.Split('\n').Single(line => line.Contains("new UnionMember(\"hashed\"", StringComparison.Ordinal));
        member.ShouldContain("id: 24944607");
    }

    [Theory]
    [InlineData("const long Number = 1;")]
    [InlineData("typedef long Number;")]
    [InlineData("enum Kind { One };")]
    [InlineData("interface Service { void ping(); };")]
    public void RejectsAutoIdOnDeclarationsWithoutMemberIds(string declaration)
    {
        // Arrange
        var input = Input("invalid-autoid-context.idl", $"@autoid(HASH) {declaration}");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("@autoid");
    }

    [Fact]
    public void SequentialUnionPolicyOverridesAnEnclosingHashPolicy()
    {
        // Arrange
        var input = Input("union-autoid-override.idl", """
            @autoid(HASH) module Scope {
                @autoid(SEQUENTIAL) union Choice switch(long) { case 0: long hashed; };
            };
            union Sibling switch(long) { case 0: long hashed; };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Scope.Implementation.ChoicePlugin.g.cs"].Source.ShouldContain("id: 1)");
        documents["Implementation.SiblingPlugin.g.cs"].Source.ShouldContain("id: 1)");
    }

    [Theory]
    [InlineData("@autoid(HASH)")]
    [InlineData("@autoid(HASH) @autoid(SEQUENTIAL) struct Sample { long value; };")]
    public void RejectsAutoIdWithoutOneFollowingApplicableDeclaration(string declaration)
    {
        // Arrange
        var input = Input("invalid-autoid-application.idl", declaration);

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("@autoid");
    }

    [Fact]
    public void ContinuesSequentialIdsAfterExplicitIdOnFinalDeclarator()
    {
        // Arrange
        var input = Input("declarators.idl",
            """
            struct Sample { @key @id(4) long first, second; long numbers[2], matrix[2][3]; };
            """);

        // Act
        var plugin = CompileSources(input)["Implementation.SamplePlugin.g.cs"].Source;

        // Assert
        plugin.ShouldContain("new StructMember(\"first\", dtf.GetPrimitiveType<int>(), id: 0)");
        plugin.ShouldContain("new StructMember(\"second\", dtf.GetPrimitiveType<int>(), isKey: true, id: 4)");
        plugin.ShouldContain("new uint[] { 2 }), id: 5)");
        plugin.ShouldContain("new uint[] { 2, 3 }), id: 6)");
    }

    [Fact]
    public void ContinuesIdsAcrossInheritanceAfterConstantsAndExplicitResets()
    {
        // Arrange
        var input = Input("inherited-ids.idl",
            """
            const long MemberId = 7;
            struct Derived : Base { long fourth; @id(12) long fifth; long sixth; };
            struct Base { @id(MemberId) long first; @id(2) long second; long third; };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var parent = documents["Implementation.BasePlugin.g.cs"].Source;
        parent.ShouldContain("new StructMember(\"third\", dtf.GetPrimitiveType<int>(), id: 3)");
        var derived = documents["Implementation.DerivedPlugin.g.cs"].Source;
        derived.ShouldContain("new StructMember(\"fourth\", dtf.GetPrimitiveType<int>(), id: 4)");
        derived.ShouldContain("new StructMember(\"sixth\", dtf.GetPrimitiveType<int>(), id: 13)");
    }

    [Fact]
    public void HashIdsDoNotAdvanceTheSequentialCounter()
    {
        // Arrange
        var input = Input("hashed-ids.idl",
            """
            struct Sample { @hashid("stable") long hashed; long first; @id(7) long explicitId; @hashid("other") long other; long next; };
            """);

        // Act
        var plugin = CompileSources(input)["Implementation.SamplePlugin.g.cs"].Source;

        // Assert
        plugin.ShouldContain("new StructMember(\"hashed\", dtf.GetPrimitiveType<int>(), id: 61804532)");
        plugin.ShouldContain("new StructMember(\"first\", dtf.GetPrimitiveType<int>(), id: 0)");
        plugin.ShouldContain("new StructMember(\"next\", dtf.GetPrimitiveType<int>(), id: 8)");
    }

    [Fact]
    public void ExplicitNumericIdUnderHashPolicyDoesNotBecomeAHashAnnotation()
    {
        // Arrange
        var input = Input("explicit-hash-policy.idl",
            """
            @autoid(HASH) struct Sample { @id(7) long explicitId; long hashed; };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Implementation.SamplePlugin.g.cs"].Source.ShouldContain("new StructMember(\"explicitId\", dtf.GetPrimitiveType<int>(), id: 7)");
        documents["Sample.g.cs"].Source.ShouldNotContain("hash of <c>explicitId</c>");
    }

    [Theory]
    [InlineData("struct Sample { long first; @id(0) long second; };", "Duplicate member ID: 0")]
    [InlineData("struct Base { @id(7) long first; }; struct Sample : Base { @id(7) long second; };", "Duplicate member ID: 7")]
    [InlineData("struct Sample { @id(2147483647) long first; long second; };", "Sequential member ID exceeds Int32")]
    public void RejectsResolvedIdCollisionsAndOverflowBeforeEmission(string text, string message)
    {
        // Arrange
        var input = Input("invalid-member-ids.idl", text);

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain(message);
    }
}
