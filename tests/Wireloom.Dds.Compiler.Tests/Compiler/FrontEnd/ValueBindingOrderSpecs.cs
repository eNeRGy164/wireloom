using static Wireloom.Compiler.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Parsing.Tests;

public sealed class ValueBindingOrderSpecs
{
    [Theory]
    [InlineData("const wchar Wide = L'A'; struct Sample { @default(Wide) char value; };")]
    [InlineData("const char Narrow = 'A'; struct Sample { @default(Narrow) wchar value; };")]
    [InlineData("struct Sample { @default(65) char value; };")]
    [InlineData("@default(65) typedef wchar Character;")]
    [InlineData("@default('A') typedef wchar Character;")]
    public void CharacterDefaultsRequireTheirDeclaredCharacterType(string source)
    {
        // Arrange
        var input = Input("character-default-type.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("default");
    }

    [Fact]
    public void ExpandsMacrosDecodesTokensConcatenatesAndChecksTheFinalBound()
    {
        // Arrange
        var input = Input("adjacent-literals.idl", """
            #define FIRST "\x4"
            #define SECOND "1"
            typedef string<2> Text;
            const Text Joined = FIRST SECOND;
            @default(Joined) typedef Text InitialText;
            struct Sample { InitialText inherited; @default("A" "B") Text direct; };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Joined.g.cs"].Source.ShouldContain("Value = \"\\u00041\";");
        documents["Sample.g.cs"].Source.ShouldContain("direct = \"AB\";");
        documents["Sample.g.cs"].Source.ShouldContain("inherited = \"\\u00041\";");
    }

    [Fact]
    public void BindsForwardCharacterAliasesAndAdjacentWideStringDefaults()
    {
        // Arrange
        var input = Input("forward-values.idl", """
            struct Sample { @default(Letter) Character value; @default(L"\u03a9" L"B") wstring<2> text; };
            typedef wchar Character;
            const Character Letter = L'A';
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Sample.g.cs"].Source.ShouldContain("value = 'A';");
        documents["Sample.g.cs"].Source.ShouldContain("text = \"ΩB\";");
    }

    [Theory]
    [InlineData("@default(\"A\" \"B\" \"C\") string<2> value;")]
    [InlineData("@default(\"A\" + \"B\") string value;")]
    [InlineData("@default(\"A\" trailing) string value;")]
    [InlineData("@default(\"A\" L\"B\") string value;")]
    public void ValidatesConcatenatedLengthAndRejectsNonliteralStringTokens(string member)
    {
        // Arrange
        var input = Input("invalid-concatenation.idl", $"struct Sample {{ {member} }};");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldNotBeEmpty();
    }
}
