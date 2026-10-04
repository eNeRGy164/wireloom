using System.Globalization;
using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;
using Wireloom;

namespace Wireloom.Compiler.FrontEnd.Parsing.Tests;

public sealed class IdlConstantSpecs
{
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
        precise.ShouldContain("public const double Value = 2.5D;");

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
    public void ReplacesPreprocessedConstantReferencesExactlyOnce()
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
        branchValue.ShouldContain("public const int Value = IncludedConstant.Value + 1;");
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
    public void PreservesResolvedConstantQualificationAgainstShadowingNamespaces()
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
        result.ShouldContain("public const int Value = global::Shared.Base.Value;");
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
        uses.ShouldContain("public const int Value = A.Value + 1;");
    }

    [Fact]
    public void PreservesGlobalQualificationForAbsoluteConstantReferences()
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
        pick.ShouldContain("public const int Value = global::Label.Value;");
        pick.ShouldNotContain("public const int Value = Label.Value;");

        var nestedPick = documents["N.NestedPick.g.cs"].Source;
        nestedPick.ShouldContain("public const int Value = global::N.Label.Value;");
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
    [InlineData("double", "1--2.0")]
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
        var input = Input("constant-string-repeated-operator.idl", "const string Text = \"a++b--c\";");

        // Act
        var source = CompileSources(input)["Text.g.cs"].Source;

        // Assert
        source.ShouldContain("public const string Value = \"a++b--c\";");
    }
}
