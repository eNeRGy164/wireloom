using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.FrontEnd.Symbols;

using static Wireloom.Compiler.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Parsing.Tests;

public sealed class ReviewAnnotationCompositionSpecs
{
    [Theory]
    [InlineData("key", "false")]
    [InlineData("key", "true")]
    [InlineData("optional", "false")]
    [InlineData("optional", "true")]
    [InlineData("external", "false")]
    [InlineData("external", "true")]
    [InlineData("must_understand", "false")]
    [InlineData("must_understand", "true")]
    public void BooleanAnnotationArgumentsHaveTheSameMeaningInEitherCase(string annotation, string value)
    {
        // Arrange
        var lower = Input("annotations.idl", $"struct Sample {{ @{annotation}({value}) long value; }};");
        var upper = Input("annotations.idl", $"struct Sample {{ @{annotation}({value.ToUpperInvariant()}) long value; }};");

        // Act
        var expected = Compile(lower);
        var actual = Compile(upper);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData("@topic @autoid(HASH) @autoid(SEQUENTIAL) struct Sample { long value; };")]
    [InlineData("@autoid(HASH) @topic @autoid(SEQUENTIAL) struct Sample { long value; };")]
    [InlineData("@appendable @autoid(HASH) @autoid(SEQUENTIAL) union Sample switch(long) { case 0: long value; };")]
    public void RejectsDuplicateAutoIdThroughoutMixedPrefixLists(string source)
    {
        // Arrange
        var input = Input("duplicate-autoid.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Duplicate @autoid");
    }

    [Theory]
    [InlineData("@appendable", "union Sample switch(long) { case 0: long value; };")]
    [InlineData("@nested(FALSE) @mutable", "struct Sample { long value; };")]
    public void AutoIdComposesWithOtherAggregateAnnotations(string prefix, string declaration)
    {
        // Arrange
        var first = Input("composition.idl", $"@autoid(HASH) {prefix} {declaration}");
        var last = Input("composition.idl", $"{prefix} @autoid(HASH) {declaration}");

        // Act
        var expected = Compile(first);
        var actual = Compile(last);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void BindsNamedCatalogArgumentsForDeclarationAnnotations()
    {
        // Arrange
        const string source = "@autoid(value=SEQUENTIAL) struct Sample { long first; long second; }; @default(value=7) typedef long Count; struct Holder { Count value; };";
        var input = Input("named-declaration-annotation-arguments.idl", source);

        // Act
        var documents = CompileSources(input);
        var generated = string.Join("\n", documents.Values.Select(document => document.Source));

        // Assert
        documents.ShouldContainKey("Sample.g.cs");
        documents.ShouldContainKey("Holder.g.cs");
        generated.ShouldContain("first");
        generated.ShouldContain("second");
        generated.ShouldContain("7");
    }

    [Fact]
    public void RetainsDeclarationApplicationsOnTheirSemanticTargets()
    {
        // Arrange
        const string source = "@autoid(value=SEQUENTIAL) struct Sample { long value; }; @default(value=7) typedef long Count; @autoid(HASH) union Choice switch(long) { case 0: long value; }; @autoid(SEQUENTIAL) module Scope { struct Nested { long value; }; };";
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), TestContext.Current.CancellationToken, Compiler.Tests.CompilerTestSupport.ComputeMd5);

        // Act
        parser.Parse(source, Input("targeted-annotations.idl", source), 0, null);

        // Assert
        var structure = parser.Declarations.OfType<IdlClassDeclaration>().Single(declaration => declaration.Name == "Sample");
        var typedef = parser.Declarations.OfType<IdlTypedefDeclaration>().Single();
        structure.AnnotationApplications.Select(application => application.Name).ShouldBe(["autoid"]);
        typedef.AnnotationApplications.Select(application => application.Name).ShouldBe(["default"]);
        typedef.AnnotationApplications.Single().ParseArguments().Single().Expression.ShouldBe("7");
        parser.Declarations.OfType<IdlUnionDeclaration>().Single().AnnotationApplications.Select(application => application.Name).ShouldBe(["autoid"]);
        parser.Declarations.OfType<IdlModuleDeclaration>().Single().AnnotationApplications.Select(application => application.Name).ShouldBe(["autoid"]);
    }

    [Fact]
    public void BindsTypedefDefaultFromItsTargetOwnedApplication()
    {
        // Arrange
        const string source = "const string Text = \"long\"; @default(Text) typedef string<3> Alias;";
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), TestContext.Current.CancellationToken, Compiler.Tests.CompilerTestSupport.ComputeMd5);
        var input = Input("typedef-default-binding.idl", source);
        parser.Parse(source, input, 0, null);

        // Act
        parser.Declarations.OfType<IdlTypedefDeclaration>().Single().AnnotationApplications.Count.ShouldBe(1);
        var exception = Should.Throw<IdlException>(parser.Bind);

        // Assert
        exception.Message.ShouldContain("String default exceeds the effective bound of 3");
    }
}
