using System.Globalization;
using Microsoft.CodeAnalysis.CSharp;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.FrontEnd.Symbols;

using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Parsing.Tests;

public sealed class IdlConstantSpecs
{
    [Fact]
    public void EvaluatesAbsoluteConstantNamesFromTheGlobalScope()
    {
        // Arrange
        var input = Input("absolute-constant-evaluation.idl", string.Empty);
        var symbols = new IdlSymbolTable();
        symbols.AddConstant("Label", new IdlConstantDeclaration(
            "Label", "long", "1", null, input.Path, new IdlScalarValue.Integral(1), input, 0));
        symbols.AddConstant("N.Label", new IdlConstantDeclaration(
            "Label", "long", "2", "N", input.Path, new IdlScalarValue.Integral(2), input, 0));

        // Act
        symbols.TryResolveConstant("::Label", "N", out var resolved).ShouldBeTrue();

        var value = IdlConstantExpressionEvaluator.Evaluate("::Label", symbols, "N");

        // Assert
        resolved.Namespace.ShouldBeNull();
        value.ShouldBe(new System.Numerics.BigInteger(1));
    }

    [Fact]
    public void RejectsMalformedSingleColonConstantNames()
    {
        // Arrange
        var input = Input("malformed-constant-name.idl", string.Empty);
        var symbols = new IdlSymbolTable();
        symbols.AddConstant("Label", new IdlConstantDeclaration(
            "Label", "long", "1", null, input.Path, new IdlScalarValue.Integral(1), input, 0));

        // Act
        var exception = Should.Throw<FormatException>(() =>
            IdlConstantExpressionEvaluator.Evaluate(":Label", symbols, null));

        // Assert
        exception.Message.ShouldContain("Invalid constant name");
    }

    [Fact]
    [Trait("Corpus", "C004")]
    public void EmitsTypedIntegralConstantsIncludingSigned64Extrema()
    {
        // Arrange
        var input = Input("02-names-constants.idl",
            """
            module Constants {
                const short shortValue = -32768;
                const unsigned long unsignedValue = 4294967295;
                const long longValue = -2147483648;
                const long long longLongMin = -9223372036854775807 - 1;
                const long long longLongMax = 9223372036854775807;
                const int64 int64Max = 9223372036854775807;
                const uint64 uint64Max = 18446744073709551615;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var shortValue = documents["Constants.shortValue.g.cs"].Source;
        shortValue.ShouldContain("public const short Value = -32768;");

        var unsignedValue = documents["Constants.unsignedValue.g.cs"].Source;
        unsignedValue.ShouldContain("public const uint Value = 4294967295U;");

        var longLongMin = documents["Constants.longLongMin.g.cs"].Source;
        longLongMin.ShouldContain("public const long Value = long.MinValue;");

        var longLongMax = documents["Constants.longLongMax.g.cs"].Source;
        longLongMax.ShouldContain("public const long Value = 9223372036854775807L;");

        var uint64Max = documents["Constants.uint64Max.g.cs"].Source;
        uint64Max.ShouldContain("public const ulong Value = ulong.MaxValue;");
    }

    [Fact]
    public void EmitsBooleanFloatingPointAndUnsignedConstantTypes()
    {
        // Arrange
        var input = Input("constant-types.idl",
            """
            module Constants {
                const boolean Flag = TRUE;
                const float Ratio = 1.5;
                const double Precise = 2.5;
                const unsigned long long Large = 42;
                const short Small = 7;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var flag = documents["Constants.Flag.g.cs"].Source;
        flag.ShouldContain("public const bool Value = true;");

        var ratio = documents["Constants.Ratio.g.cs"].Source;
        ratio.ShouldContain("public const float Value = 1.5F;");

        var precise = documents["Constants.Precise.g.cs"].Source;
        precise.ShouldContain("public const double Value = 2.5;");

        var large = documents["Constants.Large.g.cs"].Source;
        large.ShouldContain("public const ulong Value = 42UL;");

        var small = documents["Constants.Small.g.cs"].Source;
        small.ShouldContain("public const short Value = 7;");
    }

    [Fact]
    [Trait("Corpus", "C006")]
    public void ResolvesConstantExpressionsInCollectionAndStringBounds()
    {
        // Arrange
        var input = Input("02-constant-expressions.idl",
            """
            module Constants {
                const long Bound = (2 + 1) << 1;
                typedef sequence<long, Bound> Values;
                typedef long Samples[Bound - 1];
                struct Sample {
                    string<Bound> name;
                    long values[Bound];
                    Values sequenceValues;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var samplePlugin = documents["Constants.Implementation.SamplePlugin.g.cs"].Source;
        samplePlugin.ShouldContain("CreateString(6)");
        samplePlugin.ShouldContain("new uint[] { 6 }");

        var valuesPlugin = documents["Constants.Implementation.ValuesPlugin.g.cs"].Source;
        valuesPlugin.ShouldContain("CreateSequenceWithAccessInfo(dtf");

        var samplesPlugin = documents["Constants.Implementation.SamplesPlugin.g.cs"].Source;
        samplesPlugin.ShouldContain("new uint[] { 5 }");
    }

    [Fact]
    public void ResolvesHexadecimalBoundsAndMemberDefaultsAsUnsignedValues()
    {
        // Arrange
        var input = Input("hex-bound-and-default.idl",
            """
            const long Limit = 0xFF;
            struct Sample {
                string<Limit> text;
                @default(Limit) long number;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents.Values.Select(document => document.Source).ShouldContain(source => source.Contains("CreateString(255)", StringComparison.Ordinal));
        documents.Values.Select(document => document.Source).ShouldContain(source => source.Contains("number = 255;", StringComparison.Ordinal));
    }

    [Fact]
    public void ResolvesMemberDefaultsFromConstantsDeclaredEarlierInTheFile()
    {
        // Arrange
        var input = Input("member-default-constant.idl",
            """
            const long Selected = 255;
            struct Sample { @default(Selected) long number; };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents.Values.Select(document => document.Source).ShouldContain(source => source.Contains("number = 255;", StringComparison.Ordinal));
    }

    [Fact]
    public void InheritedTypedefDefaultsKeepTheDeclaringScopeAcrossAliasChains()
    {
        // Arrange
        var input = Input("scoped-inherited-typedef-default.idl",
            """
            module Defaults {
                const long Selected = 5;
                @default(Selected) typedef long Base;
                typedef Base Chained;
            };
            module Consumer {
                const long Selected = 9;
                struct Sample { Defaults::Chained value; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents.Values.Select(document => document.Source)
            .ShouldContain(source => source.Contains("value = 5;", StringComparison.Ordinal));
        documents["Defaults.Implementation.ChainedPlugin.g.cs"].Source.ShouldContain("Int32Value = 5");
    }

    [Fact]
    public void InheritedStringDefaultsFlowThroughTypedefAliases()
    {
        // Arrange
        var input = Input("inherited-string-default.idl",
            "@default(\"text\") typedef string<8> BaseText; typedef BaseText Text; struct Sample { Text value; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Sample.g.cs"].Source.ShouldContain("value = \"text\";");
        documents["Implementation.TextPlugin.g.cs"].Source.ShouldContain("StringValue = \"text\"");
    }

    [Fact]
    public void ResolvesNamedStringConstantsInTypedefDefaults()
    {
        // Arrange
        var input = Input("named-string-constant-default.idl",
            """
            module Defaults {
                const string Text = "text";
                @default(Text) typedef string<8> TextAlias;
            };
            module Consumer {
                struct Sample { Defaults::TextAlias value; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents.Values.Select(document => document.Source)
            .ShouldContain(source => source.Contains("value = \"text\";", StringComparison.Ordinal));
        documents["Defaults.Implementation.TextAliasPlugin.g.cs"].Source.ShouldContain("StringValue = \"text\"");
    }

    [Fact]
    public void RejectsCyclicStringConstantDefaults()
    {
        // Arrange
        var input = Input("cyclic-string-constant-default.idl",
            "const string First = Second; const string Second = First; @default(First) typedef string<8> Text;");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Cyclic string constant reference");
    }

    [Fact]
    public void RejectsNamedStringDefaultsThatExceedTheEffectiveBound()
    {
        // Arrange
        var input = Input("named-string-constant-bound.idl",
            "const string Text = \"long\"; @default(Text) typedef string<3> Alias;");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("String default exceeds the effective bound of 3");
    }

    [Fact]
    public void RejectsNamedStringDefaultsWithAnIncompatibleWidth()
    {
        // Arrange
        var input = Input("named-wide-string-constant-default.idl",
            "const wstring Text = L\"wide\"; @default(Text) typedef string<8> Alias;");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("incompatible narrow or wide string type");
    }

    [Fact]
    public void EvaluatesFloatingPointDefaultExpressionsWithArithmeticPrecedence()
    {
        // Arrange
        var input = Input("floating-default-expression.idl",
            "typedef double Ratio; struct Sample { @default(1.5 + 2.0 * (Scale) - -0.5 / 2.0 + +1e+1) double value; }; const Ratio Scale = 1.25;");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Sample.g.cs"].Source.ShouldContain("value = 14.25;");
        documents["Implementation.SamplePlugin.g.cs"].Source.ShouldContain("Float64Value = 14.25");
    }

    [Theory]
    [InlineData("1.0 / 0.0", "Invalid double default expression")]
    [InlineData("1e308 * 1e308", "Invalid double default expression")]
    [InlineData("1.0 +", "Invalid double default expression")]
    [InlineData("1.0 2.0", "Invalid double default expression")]
    [InlineData("2", "Invalid double default expression")]
    [InlineData("1.0 + 2", "Invalid double default expression")]
    public void RejectsInvalidFloatingDefaultExpressions(string expression, string expectedMessage)
    {
        // Arrange
        var input = Input("invalid-floating-default.idl", $"struct Sample {{ @default({expression}) double value; }};");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain(expectedMessage);
    }

    [Fact]
    public void RejectsUnclosedParenthesesInFloatingExpressions()
    {
        // Arrange
        var symbols = new IdlSymbolTable();

        // Act
        var exception = Should.Throw<FormatException>(() => FloatingConstantExpressionEvaluator.Evaluate("(2.0", symbols, currentNamespace: null));

        // Assert
        exception.Message.ShouldContain("Missing closing parenthesis");
    }

    [Fact]
    public void RejectsCyclicFloatingDefaultConstants()
    {
        // Arrange
        var input = Input("cyclic-floating-default.idl",
            "const double Loop = Loop; struct Sample { @default(Loop) double value; };");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Invalid double constant expression");
    }

    [Fact]
    public void RejectsFloatingDefaultConstantWithAnIntegralIdlType()
    {
        // Arrange
        var input = Input("wrong-floating-constant-type.idl",
            "const long Scale = 2; struct Sample { @default(Scale) double value; };");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Invalid double default expression");
    }

    [Fact]
    public void ResolvesCharacterTypedConstantsInCharacterDefaults()
    {
        // Arrange
        var input = Input("character-constant-default.idl",
            "typedef char LetterType; struct Sample { @default(Letter) char value; }; const LetterType Letter = '\\x41';");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Sample.g.cs"].Source.ShouldContain("value = 'A';");
    }

    [Fact]
    public void BindsReferencesBetweenCharacterConstantsForDefaults()
    {
        // Arrange
        var input = Input("character-constant-alias-default.idl",
            "typedef char Letter; const Letter Original = 'A'; const Letter Alias = Original; struct Sample { @default(Alias) Letter value; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Sample.g.cs"].Source.ShouldContain("value = 'A';");
        documents["Alias.g.cs"].Source.ShouldContain("Original.Value");
    }

    [Fact]
    public void RejectsCyclicCharacterConstantReferences()
    {
        // Arrange
        var input = Input("cyclic-character-constant.idl",
            "const char First = Second; const char Second = First; struct Sample { @default(First) char value; };");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Cyclic constant reference");
    }

    [Fact]
    public void RejectsCyclicBooleanConstantReferencesInUnionLabels()
    {
        // Arrange
        var input = Input("cyclic-boolean-constant.idl",
            "const boolean First = Second; const boolean Second = First; union Choice switch(boolean) { case First: long value; default: long other; };");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Cyclic constant reference");
    }

    [Theory]
    [InlineData("@default(1) typedef long double Extended;")]
    [InlineData("struct Sample { @default(1) long double value; };")]
    [InlineData("struct Sample { @min(0) long double value; };")]
    public void RejectsLongDoubleNumericConstraintsWithAnIdlDiagnostic(string declaration)
    {
        // Arrange
        var input = Input("long-double-numeric-constraint.idl", declaration);

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("are not supported for long double");
    }

    [Fact]
    public void PreservesReferencesBetweenCharacterTypedConstants()
    {
        // Arrange
        var input = Input("character-constant-alias.idl",
            "const char Original = '\\a'; const char Alias = Original;");

        // Act
        var source = CompileSources(input)["Alias.g.cs"].Source;

        // Assert
        source.ShouldContain("Original.Value");
        CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken)
            .GetDiagnostics(TestContext.Current.CancellationToken)
            .ShouldBeEmpty();
    }

    [Fact]
    public void EmitsWideCharacterConstantsAsValidCSharpCharacterLiterals()
    {
        // Arrange
        var input = Input("wide-character-constant.idl",
            "const wchar Omega = L'\\u03A9'; const char Alert = '\\a';");

        // Act
        var documents = CompileSources(input);
        var wideSource = documents["Omega.g.cs"].Source;
        var narrowSource = documents["Alert.g.cs"].Source;
        var wideDeclaration = wideSource.Split('\n').Single(line => line.Contains("Value = ", StringComparison.Ordinal));

        // Assert
        wideDeclaration.ShouldNotContain("L'");
        wideSource.ShouldContain("public const char Value =");
        var wideSyntax = CSharpSyntaxTree.ParseText(wideSource, cancellationToken: TestContext.Current.CancellationToken);
        wideSyntax.GetDiagnostics(TestContext.Current.CancellationToken).ShouldBeEmpty();
        wideSyntax.GetRoot(TestContext.Current.CancellationToken)
            .DescendantTokens()
            .Where(token => token.RawKind == (int)SyntaxKind.CharacterLiteralToken)
            .Select(token => token.Value)
            .ShouldContain('Ω');
        narrowSource.ShouldContain("public const char Value =");
        var narrowSyntax = CSharpSyntaxTree.ParseText(narrowSource, cancellationToken: TestContext.Current.CancellationToken);
        narrowSyntax.GetDiagnostics(TestContext.Current.CancellationToken).ShouldBeEmpty();
        narrowSyntax.GetRoot(TestContext.Current.CancellationToken)
            .DescendantTokens()
            .Where(token => token.RawKind == (int)SyntaxKind.CharacterLiteralToken)
            .Select(token => token.Value)
            .ShouldContain('\a');
    }

    [Fact]
    public void RejectsEnumTypedConstantsInCharacterDefaults()
    {
        // Arrange
        var input = Input("enum-character-default.idl",
            "enum Kind { First }; const Kind Selected = First; struct Sample { @default(Selected) char value; };");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Character default constant 'Selected' has type Kind, not char.");
    }

    [Fact]
    public void ResolvesNamedFalseBooleanConstantsInMemberDefaults()
    {
        // Arrange
        var input = Input("false-boolean-constant-default.idl",
            "const boolean Disabled = FALSE; struct Sample { @default(Disabled) boolean enabled; };");

        // Act
        var source = CompileSources(input)["Sample.g.cs"].Source;

        // Assert
        source.ShouldContain("enabled = false;");
    }

    [Theory]
    [InlineData("const long Number = 1;", "Number")]
    [InlineData("const boolean Broken = 1;", "Broken")]
    public void RejectsBooleanDefaultsFromNonBooleanOrInvalidConstants(string declaration, string defaultExpression)
    {
        // Arrange
        var input = Input("invalid-boolean-constant-default.idl",
            $"{declaration} struct Sample {{ @default({defaultExpression}) boolean enabled; }};");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Invalid boolean");
    }

    [Fact]
    public void RejectsCyclicBooleanConstantsUsedAsMemberDefaults()
    {
        // Arrange
        var input = Input("cyclic-boolean-default.idl",
            "const boolean First = Second; const boolean Second = First; struct Sample { @default(First) boolean value; };");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Cyclic constant reference");
    }

    [Fact]
    public void ResolvesAbsoluteNamesInMemberDefaultExpressions()
    {
        // Arrange
        var input = Input("absolute-member-default.idl",
            "const long Selected = 7; struct Sample { @default(::Selected) long value; };");

        // Act
        var source = CompileSources(input)["Sample.g.cs"].Source;

        // Assert
        source.ShouldContain("value = 7;");
    }

    [Fact]
    public void ResolvesBooleanRangeMetadataThroughIntegralBounds()
    {
        // Arrange
        var input = Input("boolean-range.idl", "struct Sample { @min(0) @max(1) boolean value; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Sample.g.cs"].Source.ShouldContain("value");
    }

    [Fact]
    public void RejectsUnquotedStringMemberDefaults()
    {
        // Arrange
        var input = Input("unquoted-string-default.idl", "const long Selected = 1; struct Sample { @default(Selected) string value; };");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("String default must be a quoted string literal");
    }

    [Fact]
    public void DefersValueMetadataUntilForwardReferencedTypedefsAreBound()
    {
        // Arrange
        var input = Input("forward-typedef-default.idl",
            "struct Sample { @default(7) Later value; }; typedef long Later;");

        // Act
        var source = CompileSources(input)["Sample.g.cs"].Source;

        // Assert
        source.ShouldContain("value = 7;");
    }

    [Fact]
    public void UnionBranchesInheritTypedefDefaultsAfterBinding()
    {
        // Arrange
        var input = Input("union-typedef-default.idl",
            "@default(7) typedef long Seven; union Choice switch(long) { case 0: Seven value; }; ");

        // Act
        var source = CompileSources(input)["Choice.g.cs"].Source;

        // Assert
        source.ShouldContain("value = 7;");
    }

    [Fact]
    public void FloatConstantReferencesUseTheDeclaredSinglePrecisionValue()
    {
        // Arrange
        var input = Input("float-constant-precision.idl",
            "const double D = F; const float F = 16777217.0; struct Sample { @default(F) double value; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["F.g.cs"].Source.ShouldContain("public const float Value = 16777216F;");
        documents["D.g.cs"].Source.ShouldContain("public const double Value = 16777216;");
        documents["Sample.g.cs"].Source.ShouldContain("value = 16777216;");
    }

    [Fact]
    public void RejectsIntegerConstantReferencesInFloatingPointExpressions()
    {
        // Arrange
        var input = Input("mixed-numeric-constant-expression.idl", "const long Integer = 2; const double Value = Integer + 1.0;");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Invalid double constant expression");
    }

    [Fact]
    public void RejectsArrayDeclarationsWithoutDimensionsInTheTypeParser()
    {
        // Arrange
        var input = Input("missing-array-dimensions.idl", string.Empty);
        var context = new IdlParseContext(new IdlSymbolTable(), CancellationToken.None, diagnostics: null);

        // Act
        var exception = Should.Throw<IdlException>(() => context.TypeParser.ParseDimensions(input, 0, string.Empty, currentNamespace: null));

        // Assert
        exception.Message.ShouldContain("Array declaration must specify at least one dimension");
    }

    [Fact]
    public void ResolvesTypedefConstantTypesBeforeBindingAndEmittingValues()
    {
        // Arrange
        var input = Input("typedef-constant-type.idl",
            "typedef long Count; const Count Selected = 7; struct Sample { @default(Selected) long value; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Selected.g.cs"].Source.ShouldContain("public const int Value = 7;");
        documents["Sample.g.cs"].Source.ShouldContain("value = 7;");
    }

    [Fact]
    public void RejectsIntegralConstantUsedAsCharacterDefault()
    {
        // Arrange
        var input = Input("wrong-character-constant-default.idl",
            "const long Number = 65; struct Sample { @default(Number) char value; };");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("not char");
    }

    [Fact]
    public void ResolvesArithmeticExpressionsInStringBounds()
    {
        // Arrange
        var input = Input("string-expression-bound.idl",
            """
            const long Limit = 2;
            struct Sample { string<Limit + 1> text; };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents.Values.Select(document => document.Source).ShouldContain(source => source.Contains("CreateString(3)", StringComparison.Ordinal));
    }

    [Fact]
    public void ResolvesShiftExpressionsInBalancedStringBoundsAcrossTypeContexts()
    {
        // Arrange
        var input = Input("string-shift-expression-bounds.idl",
            """
            typedef string<(1 << 3)> NarrowAlias;
            typedef sequence<wstring<(1 << 2)>, 2> WideSequence;
            struct Sample {
                string<(1 << 3)> narrow;
                wstring<(1 << 2)> wide;
                sequence<string<(1 << 3)>, 2> values;
            };
            union Choice switch(long) {
                case 0: string<(1 << 3)> narrow;
                default: wstring<(1 << 2)> wide;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents.Values.Select(document => document.Source).ShouldContain(source => source.Contains("CreateString(8)", StringComparison.Ordinal));
        documents.Values.Select(document => document.Source).ShouldContain(source => source.Contains("CreateWideString(4)", StringComparison.Ordinal));
        documents["Choice.g.cs"].Source.ShouldContain("[Bound(8)]");
        documents["Choice.g.cs"].Source.ShouldContain("[Bound(4)]");
    }

    [Fact]
    public void EvaluatesHighBitHexadecimalLiteralsAndUnaryNegation()
    {
        // Arrange
        var symbols = new IdlSymbolTable();

        // Act
        var positive = IdlConstantExpressionEvaluator.Evaluate("0xFF", symbols, currentNamespace: null);
        var negative = IdlConstantExpressionEvaluator.Evaluate("-0xFF", symbols, currentNamespace: null);

        // Assert
        positive.ShouldBe(new System.Numerics.BigInteger(255));
        negative.ShouldBe(new System.Numerics.BigInteger(-255));
    }

    [Fact]
    public void EvaluatesLeadingZeroIntegerLiteralsAsOctal()
    {
        // Arrange
        var input = Input("octal-constant.idl", "const long Octal = 014; const long Decimal = 12;");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Octal.g.cs"].Source.ShouldContain("public const int Value = 12;");
        documents["Decimal.g.cs"].Source.ShouldContain("public const int Value = 12;");
    }

    [Fact]
    public void RejectsNonOctalDigitsInLeadingZeroIntegerLiterals()
    {
        // Arrange
        var input = Input("invalid-octal-constant.idl", "const long Invalid = 019;");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Octal integer literals may contain only digits 0 through 7");
    }

    [Fact]
    public void PreservesWideOperandTypeWhenEvaluatingNarrowIntegerConstants()
    {
        // Arrange
        var input = Input(
            "wide-operand-in-narrow-expression.idl",
            "const unsigned long long Wide = 4294967296; const long Narrow = Wide | 0;");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("outside its representable range");
    }

    [Fact]
    public void SignedLongLongOutranksUnsignedLongInIntegerExpressionPromotion()
    {
        // Arrange
        var input = Input(
            "signed-long-long-promotion.idl",
            "const long long Signed = -1; const unsigned long Unsigned = 1; const long long Result = Signed | Unsigned;");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Result.g.cs"].Source.ShouldContain("public const long Value = -1L;");
    }

    [Fact]
    public void UnsignedLongLongOutranksSignedLongLongInIntegerExpressionPromotion()
    {
        // Arrange
        var input = Input(
            "unsigned-long-long-promotion.idl",
            "const long long Signed = -1; const unsigned long long Unsigned = 0; const long long Result = Signed | Unsigned;");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("outside its representable range");
    }

    [Theory]
    [InlineData("0x")]
    [InlineData("0X")]
    public void RejectsHexadecimalLiteralsWithoutDigits(string expression)
    {
        // Arrange
        var symbols = new IdlSymbolTable();

        // Act
        var exception = Should.Throw<FormatException>(() => IdlConstantExpressionEvaluator.Evaluate(expression, symbols, currentNamespace: null));

        // Assert
        exception.Message.ShouldContain("Hexadecimal literal must contain");
    }

    [Fact]
    public void EmitsEnumTypedConstantsAndResolvesThemInDefaultsAndUnionLabels()
    {
        // Arrange
        var input = Input("enum-valued-constant.idl",
            """
            module Example {
                enum Kind { FIRST, SECOND };
                const Kind Selected = SECOND;
                struct Sample { @default(Selected) Kind value; };
                union Choice switch(Kind) { case Selected: long value; case FIRST: long first; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Example.Selected.g.cs"].Source.ShouldContain("const Kind Value = Kind.SECOND;");
        documents["Example.Choice.g.cs"].Source.ShouldContain("Kind.SECOND");
        documents["Example.Sample.g.cs"].Source.ShouldContain("value = (Kind)(1);");
    }

    [Fact]
    public void ResolvesForwardReferencedEnumConstantChains()
    {
        // Arrange
        var input = Input("forward-enum-constant-chain.idl",
            "enum E { A }; const E X = Y; const E Y = A;");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["X.g.cs"].Source.ShouldContain("const E Value = E.A;");
        documents["Y.g.cs"].Source.ShouldContain("const E Value = E.A;");
    }

    [Fact]
    public void RejectsCyclicEnumConstantChains()
    {
        // Arrange
        var input = Input("cyclic-enum-constant-chain.idl",
            "enum E { A }; const E X = Y; const E Y = X;");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Cyclic constant reference");
    }

    [Fact]
    public void RejectsEnumConstantsThatNameMembersOfAnotherEnum()
    {
        // Arrange
        var input = Input("enum-constant-wrong-type.idl",
            "enum First { FIRST }; enum Second { SECOND }; const First Selected = Second::SECOND;");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("does not belong to First");
    }

    [Fact]
    public void RejectsUnknownEnumConstantMembers()
    {
        // Arrange
        var input = Input("enum-constant-unknown-member.idl",
            "enum Kind { FIRST }; const Kind Selected = MISSING;");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Unknown enum constant value: MISSING");
    }

    [Fact]
    public void RejectsIntegralConstantsOutsideTheirIdlRange()
    {
        // Arrange
        var input = Input("constant-range.idl", """const long long TooLarge = 9223372036854775808;""");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("outside its representable range");
    }

    [Fact]
    public void ResolvesQualifiedEnumDefaultsAndConstantsThroughTypedefAliases()
    {
        // Arrange
        var input = Input("qualified-enum-typedef-default.idl",
            "enum Kind { First, Second }; typedef Kind KindAlias; const KindAlias Selected = KindAlias::Second; struct Sample { @default(KindAlias::Second) KindAlias value; @default(Selected) KindAlias selected; };");

        // Act
        var documents = CompileSources(input);
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), CancellationToken.None);
        parser.Parse(input.Text, input, baseOffset: 0, currentNamespace: null);
        parser.Bind();
        parser.Validate(strict: false);

        // Assert
        var fields = parser.Declarations.OfType<IdlClassDeclaration>().Single().Fields;
        fields.Select(field => ((IdlScalarValue.Enumeration)field.Metadata.ValueMetadata!.DefaultValue!).Value).ShouldAllBe(value => value == 1);
        documents["Selected.g.cs"].Source.ShouldContain("const Kind Value = Kind.SECOND;");
    }

    [Fact]
    public void EvaluatesBitwiseUnaryArithmeticAndQualifiedExpressions()
    {
        // Arrange
        var input = Input("constant-operators.idl",
            """
            module Constants {
                const long long Base = 7;
                const long long Arithmetic = (Base * 3 / 2) % 5;
                const long long Bits = 1 | 2 ^ 3 & 1;
                const long long Shift = 8 >> 2;
                const long long Complement = ~1;
                const long long Hex = 0x10;
                const long long Qualified = ::Constants::Base;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var arithmetic = documents["Constants.Arithmetic.g.cs"].Source;
        arithmetic.ShouldContain("public const long Value = 0L;");

        var bits = documents["Constants.Bits.g.cs"].Source;
        bits.ShouldContain("public const long Value = 3L;");

        var shift = documents["Constants.Shift.g.cs"].Source;
        shift.ShouldContain("public const long Value = 2L;");

        var complement = documents["Constants.Complement.g.cs"].Source;
        complement.ShouldContain("public const long Value = -2L;");

        var hex = documents["Constants.Hex.g.cs"].Source;
        hex.ShouldContain("public const long Value = 16L;");

        var qualified = documents["Constants.Qualified.g.cs"].Source;
        qualified.ShouldContain("public const long Value = 7L;");
    }

    [Fact]
    [Trait("Corpus", "C048")]
    [Trait("Preprocessor", "PP007")]
    [Trait("Preprocessor", "PP031")]
    public void EmitsTheBoundPreprocessedConstantValue()
    {
        // Arrange
        var common = new IdlInput(
            "includes/common.idl",
            "const long IncludedConstant = 7;",
            generate: false);
        var root = new IdlInput(
            "11-preprocessing.idl",
            """
            #include "includes/common.idl"
            #define CORPUS_VALUE(x) ((x) + 1)
            const long BranchValue = CORPUS_VALUE(IncludedConstant);
            """,
            defines: ["CORPUS_CAPTURE_DEFINE"]);

        // Act
        var documents = CompileSources(common, root);

        // Assert
        var branchValue = documents["BranchValue.g.cs"].Source;
        branchValue.ShouldContain("public const int Value = 8;");
        branchValue.ShouldNotContain("((IncludedConstant.Value) + 1)");
        branchValue.ShouldNotContain("IncludedConstant.Value.Value");
    }

    [Fact]
    public void ReplacesUnqualifiedConstantsUsingTheInnermostIdlScope()
    {
        // Arrange
        var input = Input("constant-scope.idl",
            """
            const string A = "root"; module N { const string A = "nested"; const string Pick = A; const string Root = ::A; };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var pick = documents["N.Pick.g.cs"].Source;
        pick.ShouldContain("public const string Value = A.Value;");
        pick.ShouldNotContain("global::A.Value");

        var root = documents["N.Root.g.cs"].Source;
        root.ShouldContain("public const string Value = global::A.Value;");
    }

    [Fact]
    public void BindsConstantReferencesAgainstShadowingNamespaces()
    {
        // Arrange
        var input = Input("constant-shadowing.idl",
            """
            module Shared {
                const long Base = 1;
            };
            module Example {
                module Shared {
                    struct Item { long value; };
                };
                const long Result = Shared::Base;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var result = documents["Example.Result.g.cs"].Source;
        result.ShouldContain("public const int Value = 1;");
    }

    [Fact]
    public void DoesNotReplaceConstantNamesInsideStringOrCharacterLiterals()
    {
        // Arrange
        var input = Input("constant-literals.idl",
            """
            module Constants {
                const long A = 7;
                const char Marker = 'A';
                const string Text = "A";
                const long Uses = A + 1;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var marker = documents["Constants.Marker.g.cs"].Source;
        marker.ShouldContain("public const char Value = 'A';");

        var text = documents["Constants.Text.g.cs"].Source;
        text.ShouldContain("public const string Value = \"A\";");

        var uses = documents["Constants.Uses.g.cs"].Source;
        uses.ShouldContain("public const int Value = 8;");
    }

    [Fact]
    public void BindsAbsoluteConstantReferencesInTheirDeclaredScopes()
    {
        // Arrange
        var input = Input("absolute-constant.idl",
            """
            const long Label = 1;
            module N {
                const long Label = 2;
                const long Pick = ::Label;
                const long NestedPick = ::N::Label;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var pick = documents["N.Pick.g.cs"].Source;
        pick.ShouldContain("public const int Value = 1;");
        pick.ShouldNotContain("public const int Value = Label.Value;");

        var nestedPick = documents["N.NestedPick.g.cs"].Source;
        nestedPick.ShouldContain("public const int Value = 2;");
    }

    [Fact]
    public void RejectsDivisionByZeroInConstantExpressions()
    {
        // Arrange
        var input = Input("constant-division.idl", """const long Constant = 1 / 0;""");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Division by zero");
    }

    [Fact]
    public void RejectsRemainderByZeroInConstantExpressions()
    {
        // Arrange
        var input = Input("constant-remainder-by-zero.idl", "const long Constant = 1 % 0;");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Division by zero");
    }

    [Fact]
    public void RejectsNegativeConstantExpressionShiftCounts()
    {
        // Arrange
        var input = Input("constant-shift.idl", """const long Constant = 1 << -1;""");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Shift count is outside");
    }

    [Fact]
    public void AcceptsZeroCountForLeftAndRightConstantExpressionShifts()
    {
        // Arrange
        var input = Input("constant-zero-shift.idl",
            """
            module Constants {
                const long Left = 8 << 0;
                const long Right = 8 >> 0;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Constants.Left.g.cs"].Source.ShouldContain("public const int Value = 8;");
        documents["Constants.Right.g.cs"].Source.ShouldContain("public const int Value = 8;");
    }

    [Fact]
    public void RejectsShiftCountsOutsideTheOMGConstantExpressionRange()
    {
        // Arrange
        var input = Input(
            "constant-maximum-shift.idl",
            "const long Constant = 0 << 64;");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Shift count is outside");
    }

    [Fact]
    public void UsesZeroFillForRightShiftsInIntegerConstantExpressions()
    {
        // Arrange
        var input = Input("constant-zero-fill-shift.idl", "const long Constant = -2 >> 1;");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Constant.g.cs"].Source.ShouldContain("public const int Value = 2147483647;");
    }

    [Fact]
    public void RejectsUnknownIntegralConstants()
    {
        // Arrange
        var input = Input("constant-name.idl", """const long Constant = Missing;""");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Unknown integral constant");
    }

    [Fact]
    public void RejectsMalformedConstantExpressions()
    {
        // Arrange
        var input = Input("constant-token.idl", """const long Constant = 1 + );""");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Expected an integer literal");
    }

    [Fact]
    public void RejectsUnterminatedParenthesizedConstantExpressions()
    {
        // Arrange
        var input = Input("constant-parenthesis.idl", "const long Constant = (1 + 2;");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Expected ')' in constant expression");
    }

    [Theory]
    [InlineData("++2")]
    [InlineData("+ +2")]
    [InlineData("--2")]
    [InlineData("- -2")]
    public void EmitsEvaluatedLiteralForRepeatedUnaryOperators(string expression)
    {
        // Arrange
        var input = Input("constant-repeated-operator.idl", $"const long Constant = {expression};");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Constant.g.cs"].Source.ShouldContain("public const int Value = 2;");
    }

    [Fact]
    public void FormatsRepeatedOperatorIntegerLiteralsUsingInvariantCulture()
    {
        // Arrange
        var input = Input("constant-repeated-operator-culture.idl", "const long Constant = -- -2;");
        var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        culture.NumberFormat.NegativeSign = "~";
        var previousCulture = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = culture;

            // Act
            var source = CompileSources(input)["Constant.g.cs"].Source;

            // Assert
            source.ShouldContain("public const int Value = -2;");
            source.ShouldNotContain("Value = ~2");
        }
        finally
        {
            CultureInfo.CurrentCulture = previousCulture;
        }
    }

    [Theory]
    [InlineData("double", "++2.0")]
    [InlineData("double", "1.0--2.0")]
    [InlineData("float", "- -2.0")]
    public void NormalizesRepeatedUnaryOperatorsForFloatingPointConstants(string type, string expression)
    {
        // Arrange
        var input = Input("constant-floating-repeated-operator.idl", $"const {type} Constant = {expression};");

        // Act
        var source = CompileSources(input)["Constant.g.cs"].Source;

        // Assert
        source.ShouldNotContain("++");
        source.ShouldNotContain("--");
    }

    [Fact]
    public void PreservesRepeatedOperatorsInsideStringConstants()
    {
        // Arrange
        var input = Input("constant-string-repeated-operator.idl", """const string Text = "a++\"b--c";""");

        // Act
        var source = CompileSources(input)["Text.g.cs"].Source;

        // Assert
        source.ShouldContain("""public const string Value = "a++\"b--c";""");
    }
}
