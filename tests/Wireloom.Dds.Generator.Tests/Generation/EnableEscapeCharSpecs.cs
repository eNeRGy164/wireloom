using Wireloom;
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
}
