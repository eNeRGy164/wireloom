using static Wireloom.Compiler.Tests.CompilerTestSupport;

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
        var exception = Should.Throw<IdlException>(() => Wireloom.Compiler.Tests.CompilerTestSupport.CompileSourcesWithDiagnostics(
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
        var unsupportedException = Should.Throw<IdlException>(() => Wireloom.Compiler.Tests.CompilerTestSupport.CompileSourcesWithDiagnostics(
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
        var documents = Wireloom.Compiler.Tests.CompilerTestSupport.CompileSourcesWithDiagnostics(
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
        Wireloom.Compiler.Tests.CompilerTestSupport.CompileSourcesWithDiagnostics([root, included], diagnostics, TestContext.Current.CancellationToken);

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
        var parser = new IdlDeclarationParser(new Symbols.IdlSymbolTable(), TestContext.Current.CancellationToken, Compiler.Tests.CompilerTestSupport.ComputeMd5);

        // Act
        parser.Parse(source, input, 0, currentNamespace: null);
        var members = parser.Declarations
            .OfType<Semantic.IdlClassDeclaration>()
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

    [Fact]
    public void EvaluatesIntegerOperandTypesLocallyForEachSubexpression()
    {
        // Arrange
        const string source = "const unsigned long Mask = 0; const long A = (~0 / 2) + Mask; const long B = Mask + (~0 / 2);";
        var input = Input("integer-subexpression-promotion.idl", source);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["A.g.cs"].Source.ShouldContain("Value = 0;");
        documents["B.g.cs"].Source.ShouldContain("Value = 0;");
    }

    [Fact]
    public void PreservesWideLiteralPrecisionUntilTheFinalDeclaredRangeCheck()
    {
        // Arrange
        var shifted = Input("wide-literal-shift.idl", "const long Result = 0x100000000 >> 32;");
        var outOfRange = Input("wide-literal-overflow.idl", "const long Result = 4294967296 | 0;");

        // Act
        var documents = CompileSources(shifted);
        var exception = Should.Throw<IdlException>(() => CompileSources(outOfRange));

        // Assert
        documents["Result.g.cs"].Source.ShouldContain("Value = 1;");
        exception.Input.ShouldBe(outOfRange);
        exception.Message.ShouldContain("outside its representable range");
    }

    [Fact]
    public void PromotesEqualRankOperandsBySignednessInEitherOrder()
    {
        // Arrange
        const string source = "const long long Signed = 1; const unsigned long long Unsigned = 2; const long long Left = Signed + Unsigned; const long long Right = Unsigned + Signed;";
        var input = Input("equal-rank-signedness-promotion.idl", source);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Left.g.cs"].Source.ShouldContain("Value = 3L;");
        documents["Right.g.cs"].Source.ShouldContain("Value = 3L;");
    }

    [Fact]
    public void PromotesAliasedIntegralConstantsUsingTheirResolvedDeclaredTypes()
    {
        // Arrange
        const string source = "typedef unsigned long long Wide; const Wide Unsigned = 2; const long long Signed = 1; const long long Result = Signed + Unsigned;";
        var input = Input("aliased-integer-expression-type.idl", source);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Result.g.cs"].Source.ShouldContain("Value = 3L;");
    }

    [Theory]
    [InlineData("@min(1.0) float value;")]
    [InlineData("@max(1.0) double value;")]
    [InlineData("@range(min=0.0,max=1.0) float value;")]
    public void ReportsFloatingPointRangesThroughTheIdlDiagnosticBoundary(string member)
    {
        // Arrange
        const string sourcePrefix = "struct Sample { ";
        var source = $"{sourcePrefix}{member} }};";
        var input = Input("floating-point-range.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Input.ShouldBe(input);
        exception.Offset.ShouldBeGreaterThanOrEqualTo(sourcePrefix.Length);
        exception.Message.ShouldContain("not supported for floating-point members");
    }

    [Fact]
    public void EvaluatesLiteralMemberIdsWithTheSharedIntegerGrammar()
    {
        // Arrange
        const string source = "struct Sample { @id(010) long octal; @id(10) long decimal; @id(0xA) long hexadecimal; };";
        var parser = new IdlDeclarationParser(new Symbols.IdlSymbolTable(), TestContext.Current.CancellationToken, Compiler.Tests.CompilerTestSupport.ComputeMd5);

        // Act
        parser.Parse(source, Input("member-id-integer-literals.idl", source), 0, currentNamespace: null);
        parser.Bind();
        var members = parser.Declarations
            .OfType<Semantic.IdlClassDeclaration>()
            .Single()
            .Fields
            .ToDictionary(member => member.Name, StringComparer.Ordinal);

        // Assert
        members["octal"].Metadata.MemberId.ShouldBe(8);
        members["decimal"].Metadata.MemberId.ShouldBe(10);
        members["hexadecimal"].Metadata.MemberId.ShouldBe(10);
    }

    [Theory]
    [InlineData("@unit(\"me\" \"ters\")", "meters")]
    [InlineData("@unit(L\"meters\")", "meters")]
    [InlineData("@unit(\"line\\nfeed\")", "line\nfeed")]
    [InlineData("@unit(\"line\\rfeed\")", "line\rfeed")]
    public void DecodesUnitLiteralSequencesBeforeStoringMetadata(string annotation, string expectedUnit)
    {
        // Arrange
        var source = $"struct Sample {{ {annotation} long value; }};";
        var parser = new IdlDeclarationParser(new Symbols.IdlSymbolTable(), TestContext.Current.CancellationToken, Compiler.Tests.CompilerTestSupport.ComputeMd5);

        // Act
        parser.Parse(source, Input("decoded-unit-annotation.idl", source), 0, currentNamespace: null);
        parser.Bind();
        var member = parser.Declarations
            .OfType<Semantic.IdlClassDeclaration>()
            .Single()
            .Fields
            .Single();

        // Assert
        member.Metadata.ValueMetadata!.Unit.ShouldBe(expectedUnit);
    }

    [Fact]
    public void RejectsNumericExpressionDispatchForUnsupportedScalarTypes()
    {
        // Arrange
        var symbols = new Symbols.IdlSymbolTable();

        // Act
        var exception = Should.Throw<FormatException>(() =>
            Semantic.IdlTypedExpressionEvaluator.Evaluate("1", "boolean", symbols, currentNamespace: null));

        // Assert
        exception.Message.ShouldContain("expressions of type 'boolean' are not supported");
    }

    [Theory]
    [InlineData("TRUE", true)]
    [InlineData("FALSE", false)]
    public void ResolvesBooleanAnnotationLiteralsDirectly(string expression, bool expected)
    {
        // Arrange
        var resolver = new Semantic.IdlValueResolver(
            new Symbols.IdlSymbolTable());

        // Act
        var actual = resolver.ResolveBooleanAnnotation(Input("boolean-literal.idl", ""), 0, null, expression, "key");

        // Assert
        actual.ShouldBe(expected);
    }
}
