using Wireloom;
using Wireloom.Compiler.FrontEnd.Preprocessing;
using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Generation.Tests;

/// <summary>Checks project-wide leading-underscore IDL name normalization.</summary>
public sealed class EnableEscapeCharSpecs
{
    [Fact]
    public void KeepsLeadingUnderscoresWhenEscapeCharacterIsDisabled()
    {
        // Arrange
        var input = new IdlInput("legacy.idl", "module _Legacy { struct _Sample { long _event; }; };");

        // Act
        var generated = IdlCompiler.CompileSources([input], TestContext.Current.CancellationToken);

        // Assert
        generated["_Legacy._Sample.g.cs"].Source.ShouldContain("namespace _Legacy");
        generated["_Legacy._Sample.g.cs"].Source.ShouldContain("class _Sample");
        generated["_Legacy._Sample.g.cs"].Source.ShouldContain("public int _event");
    }

    [Fact]
    public void NormalizesDeclarationsReferencesMetadataAndHashIds()
    {
        // Arrange
        const string source = """
            module _Escaped {
                const long _COUNT = 2;
                typedef long _Scalar;
                typedef long _Array[_COUNT];
                enum _Color { _RED = 0, _GREEN = 1 };
                @autoid(HASH)
                struct _Record {
                    _Scalar _value;
                    _Array _numbers;
                    _Color _color;
                    long _event;
                    long __double;
                };
                union _Choice switch(_Color) {
                    case _RED: long _number;
                    default: _Scalar _other;
                };
            };
            """;
        var input = new IdlInput("escaped.idl", source, enableEscapeChar: true);

        // Act
        var generated = IdlCompiler.CompileSources([input], TestContext.Current.CancellationToken);

        // Assert
        var generatedText = string.Join(Environment.NewLine, generated.Values.Select(source => source.Source));
        generatedText.ShouldContain("namespace Escaped");
        generatedText.ShouldContain("public partial class Record");
        generatedText.ShouldContain("public int @event");
        generatedText.ShouldContain("public int _double");
        generatedText.ShouldContain("public enum Color");
        generatedText.ShouldContain("RED = 0");
        generatedText.ShouldContain("class Choice");
        generatedText.ShouldContain("new int[2]");
        generated["Escaped.Implementation.RecordPlugin.g.cs"].Source.ShouldContain(
            $"new StructMember(\"event\", dtf.GetPrimitiveType<int>(), id: {IdlMemberIdHash.Compute("event")})");
        generated["Escaped.Implementation.RecordPlugin.g.cs"].Source.ShouldContain("new StructMember(\"value\"");
        generated["Escaped.Implementation.ChoicePlugin.g.cs"].Source.ShouldContain("new UnionMember(\"number\"");
    }

    [Fact]
    public void ResolvesEscapedNamesThatMatchBuiltinTypesAsDeclaredTypes()
    {
        // Arrange
        const string source = """
            struct _long { long value; };
            struct _string { long value; };
            typedef _long _LongAlias;
            typedef sequence<_long> _LongSequence;
            typedef _long _LongArray[2];
            struct Sample {
                _long nested;
                _string textType;
                _LongAlias aliasedValue;
                _LongSequence items;
                sequence<_long> directItems;
                _LongArray array;
                _long values[2];
            };
            union Choice switch(long) {
                case 0: _long nested;
                default: _string textType;
            };
            """;
        var input = new IdlInput("escaped-builtin-types.idl", source, enableEscapeChar: true);

        // Act
        var generated = IdlCompiler.CompileSources([input], TestContext.Current.CancellationToken);
        var generatedText = string.Join(Environment.NewLine, generated.Values.Select(document => document.Source));
        var sampleSource = generated["Sample.g.cs"].Source;
        var unionSource = generated["Choice.g.cs"].Source;

        // Assert
        generatedText.ShouldContain("class @long");
        sampleSource.ShouldContain("public @long nested");
        sampleSource.ShouldContain("public @string textType");
        sampleSource.ShouldContain("public @long aliasedValue");
        sampleSource.ShouldContain("public LongSequence items");
        sampleSource.ShouldContain("public @long[] values");
        unionSource.ShouldContain("@long");
        unionSource.ShouldContain("@string");
    }

    [Fact]
    public void ResolvesEscapedBooleanConstantReferencesBeforeBooleanLiterals()
    {
        // Arrange
        const string source = """
            const boolean _TRUE = FALSE;
            const boolean Alias = _TRUE;
            struct Sample {
                @key(_TRUE) boolean escapedConstantKey;
                @key(TRUE) boolean literalKey;
                @default(_TRUE) boolean defaulted;
            };
            """;
        var input = new IdlInput("escaped-boolean-constant.idl", source, enableEscapeChar: true);

        // Act
        var generated = IdlCompiler.CompileSources([input], TestContext.Current.CancellationToken);
        var generatedText = string.Join(Environment.NewLine, generated.Values.Select(document => document.Source));
        var plugin = generated.Values.Single(document => document.Source.Contains("new StructMember(\"escapedConstantKey\"", StringComparison.Ordinal)).Source;
        var alias = generated.Values.Single(document => document.Source.Contains("class Alias", StringComparison.Ordinal)).Source;

        // Assert
        generatedText.ShouldContain("public const bool Value = false;");
        alias.ShouldContain("public const bool Value = false;");
        plugin.ShouldContain("new StructMember(\"escapedConstantKey\"");
        plugin.ShouldNotContain("new StructMember(\"escapedConstantKey\", dtf.GetPrimitiveType<bool>(), isKey: true");
        plugin.ShouldContain("new StructMember(\"literalKey\", dtf.GetPrimitiveType<bool>(), isKey: true, id: 1)");
        plugin.ShouldContain("isKeyed: true");
        plugin.ShouldContain("BoolValue = false");
    }

    [Fact]
    public void NormalizesPreprocessorExpandedIdentifiersAndLeavesStringLiteralsAlone()
    {
        // Arrange
        const string source = """
            #define MODULE _MacroModule
            #define TYPE _Scalar
            #define MEMBER _value
            module MODULE {
                const long _COUNT = 2;
                typedef long TYPE;
                typedef long _Array[_COUNT];
                struct _Record { TYPE MEMBER; string<16> _text; };
                const string _TEXT = "_keep_this";
            };
            """;
        var input = new IdlInput("macro-escaped.idl", source, enableEscapeChar: true);

        // Act
        var generated = IdlCompiler.CompileSources([input], TestContext.Current.CancellationToken);

        // Assert
        var generatedText = string.Join(Environment.NewLine, generated.Values.Select(document => document.Source));
        generatedText.ShouldContain("namespace MacroModule");
        generatedText.ShouldContain("public int value");
        generatedText.ShouldContain("public string text");
        generatedText.ShouldContain("new int[2]");
        generatedText.ShouldContain("\"_keep_this\"");
    }

    [Fact]
    public void AllowsEscapedKeywordsInStrictMode()
    {
        // Arrange
        var input = new IdlInput(
            "escaped-keyword.idl",
            "struct _event { long _sequence; };",
            strict: true,
            enableEscapeChar: true);

        // Act
        var generated = IdlCompiler.CompileSources([input], TestContext.Current.CancellationToken);

        // Assert
        var generatedText = string.Join(Environment.NewLine, generated.Values.Select(document => document.Source));
        generatedText.ShouldContain("class @event");
        generatedText.ShouldContain("public int sequence");
    }

    [Fact]
    public void RejectsNamesThatCollideAfterNormalizationAtOriginalSourceOffset()
    {
        // Arrange
        const string source = "struct Sample { long _ordinary; long ordinary; };";
        var input = new IdlInput("collision.idl", source, enableEscapeChar: true);

        // Act
        var exception = Should.Throw<IdlException>(() =>
            IdlCompiler.CompileSources([input], TestContext.Current.CancellationToken));

        // Assert
        exception.Offset.ShouldBe(source.LastIndexOf("long ordinary", StringComparison.Ordinal));
        exception.Message.ShouldContain("duplicate");
        exception.Message.ShouldContain("ordinary");
    }

    [Fact]
    public void RejectsTypeNamesThatCollideAfterNormalization()
    {
        // Arrange
        const string source = "struct _Sample { long first; }; struct Sample { long second; };";
        var input = new IdlInput("type-collision.idl", source, enableEscapeChar: true);

        // Act
        var exception = Should.Throw<IdlException>(() =>
            IdlCompiler.CompileSources([input], TestContext.Current.CancellationToken));

        // Assert
        exception.Input.ShouldBe(input);
        exception.Message.ShouldContain("Sample");
    }

    [Fact]
    public void RejectsAnIdentifierThatNormalizesToAnEmptyName()
    {
        // Arrange
        const string source = "struct _ { long value; };";
        var input = new IdlInput("empty-name.idl", source, enableEscapeChar: true);

        // Act
        var exception = Should.Throw<IdlException>(() =>
            IdlCompiler.CompileSources([input], TestContext.Current.CancellationToken));

        // Assert
        exception.Offset.ShouldBe(source.IndexOf('_'));
        exception.Message.ShouldContain("leaves an empty name");
    }

    [Fact]
    public void RequiresOneEscapeCharacterSettingAcrossGenerationRoots()
    {
        // Arrange
        var inputs = new List<IdlInput>
        {
            new("first.idl", "struct First { long value; };", enableEscapeChar: true),
            new("second.idl", "struct Second { long value; };")
        };

        // Act
        var exception = Should.Throw<IdlException>(() =>
            IdlCompiler.CompileSources(inputs, TestContext.Current.CancellationToken));

        // Assert
        exception.Message.ShouldContain("same value for every generation root");
    }

    [Fact]
    public void NormalizesTextWithoutSourceOriginSpans()
    {
        // Arrange
        var input = new IdlInput("originless.idl", "x", enableEscapeChar: true);
        var preprocessed = new PreprocessedIdl("x", hasPragmaOnce: false, sourceOrigins: []);

        // Act
        var normalized = IdlEscapeCharNormalizer.Normalize(preprocessed, input, TestContext.Current.CancellationToken);

        // Assert
        normalized.Text.ShouldBe("x");
        normalized.SourceOrigins.ShouldContain(origin => origin.SourceStart == 0);
    }

    [Fact]
    public void MapsOffsetsOutsideSourceOriginSpansToTheLastOrigin()
    {
        // Arrange
        var input = new IdlInput("gapped-origins.idl", "x", enableEscapeChar: true);
        var preprocessed = new PreprocessedIdl("x", hasPragmaOnce: false, sourceOrigins: [new SourceOriginSpan(2, 1, 10, 1)]);

        // Act
        var normalized = IdlEscapeCharNormalizer.Normalize(preprocessed, input, TestContext.Current.CancellationToken);

        // Assert
        normalized.SourceOrigins.ShouldContain(origin => origin.SourceStart == 10);
    }
}
