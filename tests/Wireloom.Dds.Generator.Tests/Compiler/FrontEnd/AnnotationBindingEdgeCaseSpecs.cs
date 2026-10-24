using Wireloom;

using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Parsing.Tests;

public sealed class AnnotationBindingEdgeCaseSpecs
{
    [Fact]
    public void RejectsKnownAnnotationsOnInvalidTargetsAtTheirSourceLocation()
    {
        // Arrange
        const string source = "@unit(\"meters\") struct Sample { long value; };";
        var input = Input("known-annotation-invalid-target.idl", source);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSourcesWithDiagnostics(
            [input], diagnostics, TestContext.Current.CancellationToken));

        // Assert
        exception.Message.ShouldContain("not supported on this target");
        exception.Offset.ShouldBe(source.IndexOf("@unit", StringComparison.Ordinal));
        diagnostics.ShouldBeEmpty();
    }

    [Fact]
    public void ReportsRecognizedUnsupportedAnnotationsDifferentlyFromUnknownAnnotations()
    {
        // Arrange
        const string unsupportedSource = "struct Sample { @position(1) long value; };";
        var unsupportedInput = Input("recognized-unsupported-annotation.idl", unsupportedSource);
        var unsupportedDiagnostics = new List<IdlDiagnostic>();

        // Act
        var unsupportedException = Should.Throw<IdlException>(() => IdlCompiler.CompileSourcesWithDiagnostics(
            [unsupportedInput], unsupportedDiagnostics, TestContext.Current.CancellationToken));

        // Assert
        var unsupportedDiagnostic = unsupportedDiagnostics.Single(diagnostic => diagnostic.Id == "DDSG0102");
        unsupportedDiagnostics.ShouldNotContain(diagnostic => diagnostic.Id == "DDSG0101");
        unsupportedDiagnostic.Input.ShouldBe(unsupportedInput);
        unsupportedDiagnostic.Offset.ShouldBe(unsupportedSource.IndexOf("@position", StringComparison.Ordinal));
        unsupportedException.Offset.ShouldBe(unsupportedDiagnostic.Offset);
        unsupportedDiagnostic.Message.ShouldContain("recognized but unsupported");
        unsupportedException.Message.ShouldContain("not supported in this context");

        // Arrange
        const string unknownSource = "@Vendor::custom(payload=(1 + (2 * 3))) struct Custom { long value; };";
        var unknownInput = Input("unknown-annotation-warning.idl", unknownSource);
        var unknownDiagnostics = new List<IdlDiagnostic>();

        // Act
        var documents = IdlCompiler.CompileSourcesWithDiagnostics(
            [unknownInput], unknownDiagnostics, TestContext.Current.CancellationToken);

        // Assert
        documents.ShouldContainKey("Custom.g.cs");
        var unknownDiagnostic = unknownDiagnostics.Single(diagnostic => diagnostic.Id == "DDSG0101");
        unknownDiagnostics.Count(diagnostic => diagnostic.Id == "DDSG0101").ShouldBe(1);
        unknownDiagnostics.ShouldNotContain(diagnostic => diagnostic.Id == "DDSG0102");
        unknownDiagnostic.Input.ShouldBe(unknownInput);
        unknownDiagnostic.Offset.ShouldBe(unknownSource.IndexOf("@Vendor::", StringComparison.Ordinal));
    }

    [Fact]
    public void RejectsCyclicBooleanConstantsUsedByBooleanMemberAnnotations()
    {
        // Arrange
        const string source = "const boolean First = Second; const boolean Second = First; struct Sample { @key(First) long value; };";
        var input = Input("cyclic-boolean-annotation-constant.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Input.ShouldBe(input);
        exception.Message.ShouldContain("Cyclic constant reference");
        exception.Offset.ShouldBeInRange(0, source.Length - 1);
    }

    [Theory]
    [InlineData("struct Sample { @min(TRUE) long value; };", "Unknown integral constant: TRUE")]
    [InlineData("struct Sample { @min(1.5) long value; };", "Unexpected token in constant expression: .5")]
    [InlineData("struct Sample { @min(0) @max(2) @default(3) long value; };", "outside its declared range")]
    public void ReportsNumericExpressionTypeAndRangeErrors(string source, string expectedMessage)
    {
        // Arrange
        var input = Input("numeric-annotation-expression-error.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Input.ShouldBe(input);
        exception.Message.ShouldContain(expectedMessage);
        exception.Offset.ShouldBeInRange(0, source.Length - 1);
    }

    [Fact]
    public void MapsUnknownAnnotationDiagnosticsBackToIncludedSource()
    {
        // Arrange
        var root = Input("root.idl", "#include \"annotations.idl\"");
        const string includedSource = "@Vendor::included(payload=(1 + 2)) struct Included { long value; };";
        var included = Input("annotations.idl", includedSource, generate: false);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        IdlCompiler.CompileSourcesWithDiagnostics([root, included], diagnostics, TestContext.Current.CancellationToken);

        // Assert
        var diagnostic = diagnostics.ShouldHaveSingleItem();
        diagnostic.Id.ShouldBe("DDSG0101");
        diagnostic.Input.ShouldBe(included);
        diagnostic.Offset.ShouldBe(includedSource.IndexOf("@Vendor::", StringComparison.Ordinal));
    }

    [Fact]
    public void MapsExpandedAnnotationErrorsBackToTheMacroInvocation()
    {
        // Arrange
        const string source = "#define MEMBER_ANNOTATION @default(7)\nMEMBER_ANNOTATION struct Sample { long value; };";
        var input = Input("macro-expanded-annotation-target.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Input.ShouldBe(input);
        exception.Message.ShouldContain("only supported on typedef declarations and members");
        exception.Offset.ShouldBe(source.LastIndexOf("MEMBER_ANNOTATION", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("@default(\"a=b\") string value;")]
    [InlineData("@default('=') char value;")]
    public void KeepsEqualsSignsInsidePositionalLiterals(string member)
    {
        // Arrange
        var input = Input("equals-in-annotation-literal.idl", $"struct Sample {{ {member} }};");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents.ShouldContainKey("Sample.g.cs");
        documents["Sample.g.cs"].Source.ShouldContain("value =");
    }

    [Fact]
    public void PromotesUnaryComplementWidthFromItsResolvedConstantOperand()
    {
        // Arrange
        var input = Input(
            "wide-complement.idl",
            "const unsigned long long Wide = 0; const long Result = (~Wide >> 40) & 255;");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Result.g.cs"].Source.ShouldContain("public const int Value = 255;");
    }

    [Fact]
    public void ConcatenatesAdjacentHashIdStringLiteralsBeforeHashing()
    {
        // Arrange
        const string source = "struct Sample { @hashid(\"abcd\") long direct; @hashid(\"ab\" \"cd\") long adjacent; };";
        var input = Input("adjacent-hashid-strings.idl", source);
        var parser = new IdlDeclarationParser(new Wireloom.Compiler.FrontEnd.Symbols.IdlSymbolTable(), TestContext.Current.CancellationToken);

        // Act
        parser.Parse(source, input, 0, currentNamespace: null);
        var members = parser.Declarations
            .OfType<Wireloom.Compiler.FrontEnd.Semantic.IdlClassDeclaration>()
            .Single()
            .Fields
            .ToDictionary(field => field.Name, StringComparer.Ordinal);

        // Assert
        members["direct"].Metadata.MemberIdHashSource.ShouldBe("abcd");
        members["adjacent"].Metadata.MemberIdHashSource.ShouldBe("abcd");
        members["adjacent"].Metadata.MemberId.ShouldBe(members["direct"].Metadata.MemberId);
    }

    [Fact]
    public void ResolvesFloatingConstantAndScopeNamesBeginningWithE()
    {
        // Arrange
        const string source = "const double Epsilon = 0.1; const double Result = Epsilon; module Example { const double Base = 0.2; const double ResultScoped = Example::Base; }; struct Sample { @default(Epsilon) double value; };";
        var input = Input("floating-identifier-leading-e.idl", source);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Result.g.cs"].Source.ShouldContain("public const double Value = 0.1;");
        documents["Example.ResultScoped.g.cs"].Source.ShouldContain("public const double Value = 0.2;");
        documents["Sample.g.cs"].Source.ShouldContain("value = 0.1;");
    }
}
