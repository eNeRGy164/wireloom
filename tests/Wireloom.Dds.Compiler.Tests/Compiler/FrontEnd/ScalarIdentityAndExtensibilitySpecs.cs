using static Wireloom.Compiler.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Parsing.Tests;

public sealed class ScalarIdentityAndExtensibilitySpecs
{
    [Theory]
    [InlineData("boolean", "TRUE", "TRUE")]
    [InlineData("boolean", "FALSE", "FALSE")]
    [InlineData("char", "'A'", "65")]
    [InlineData("wchar", "L'Ω'", "937")]
    public void NamedDiscriminatorLabelsKeepTheirScalarCategory(string type, string literal, string expectedLabel)
    {
        // Arrange
        var input = Input("typed-labels.idl",
            $$"""
            const {{type}} Selected = {{literal}};
            union Choice switch({{type}}) { case Selected: long value; default: long other; };
            """);
        var parser = new IdlDeclarationParser(new Symbols.IdlSymbolTable(), CancellationToken.None, Compiler.Tests.CompilerTestSupport.ComputeMd5);
        parser.Parse(input.Text, input, baseOffset: 0, currentNamespace: null);

        // Act
        parser.Bind();

        // Assert
        var choice = parser.Declarations.OfType<Semantic.IdlUnionDeclaration>().Single();
        choice.Declaration.Branches[0].Labels.ShouldBe([expectedLabel]);
    }

    [Theory]
    [InlineData("const boolean Enabled = TRUE; typedef string<Enabled> Text;")]
    [InlineData("const char Size = 'A'; struct Sample { long values[Size]; };")]
    [InlineData("typedef boolean Flag; const Flag Enabled = TRUE; struct Sample { @id(Enabled) long value; };")]
    [InlineData("const char Number = 'A'; const long Selected = Number + 1;")]
    [InlineData("const wchar Number = L'A'; struct Sample { @default(Number) long value; };")]
    [InlineData("const wstring Text = \"narrow\";")]
    [InlineData("struct Sample { @default(\"narrow\") wstring<8> text; };")]
    [InlineData("@default(L\"A\" \"B\") typedef wstring Text;")]
    [InlineData("const wstring Text = L\"A\" \"B\";")]
    [InlineData("const boolean Enabled = TRUE; struct Sample { @min(Enabled) long value; };")]
    [InlineData("const boolean Enabled = TRUE; typedef sequence<long, Enabled> Values;")]
    [InlineData("enum Kind { First, Second }; const Kind Size = Kind::Second; typedef string<Size> Text;")]
    [InlineData("const char Letter = 'A'; union Sample switch(long) { case Letter: long value; };")]
    [InlineData("const wchar Letter = L'A'; union Sample switch(char) { case Letter: long value; };")]
    [InlineData("union Sample switch(char) { case L'A': long value; };")]
    [InlineData("@final @mutable struct Sample { long value; };")]
    [InlineData("@appendable @extensibility(APPENDABLE) union Sample switch(long) { case 0: long value; };")]
    [InlineData("@extensibility(FINAL) @final enum Kind { First };")]
    [InlineData("@final @autoid enum Kind { First };")]
    [InlineData("@topic enum Kind { First };")]
    [InlineData("@extensibility(MUTABLE) enum Kind { First };")]
    public void RejectsValuesAndAnnotationsWithIncompatibleSemantics(string source)
    {
        // Arrange
        var input = Input("typed-values.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldNotBeEmpty();
    }

    [Theory]
    [InlineData("FINAL", "final")]
    [InlineData("APPENDABLE", "appendable")]
    public void EnumExtensibilityIsIndependentOfAnnotationSpelling(string fullValue, string shortName)
    {
        // Arrange
        var shortForm = Input("enum-extensibility.idl", $"@{shortName} enum Kind {{ First, Second }};");
        var fullForm = Input("enum-extensibility.idl", $"@extensibility({fullValue}) enum Kind {{ First, Second }};");

        // Act
        var expected = Compile(shortForm);
        var actual = Compile(fullForm);

        // Assert
        actual.ShouldBe(expected);
        var kind = fullValue == "FINAL" ? "Final" : "Extensible";
        actual.ShouldContain($"WithExtensibility(ExtensibilityKind.{kind})");
    }

    [Fact]
    public void TypedConstantsStillWorkInMatchingDiscriminatorAndDefaultContexts()
    {
        // Arrange
        var input = Input("typed-contexts.idl", """
            typedef boolean Flag;
            const Flag Enabled = TRUE;
            typedef wchar Character;
            const Character Letter = L'A';
            union Switch switch(boolean) { case Enabled: long value; default: long other; };
            union CharacterSwitch switch(wchar) { case Letter: long value; default: long other; };
            struct Sample { @default(Enabled) Flag flag; @default(Letter) Character character; };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Sample.g.cs"].Source.ShouldContain("flag = true;");
        documents["Sample.g.cs"].Source.ShouldContain("character = 'A';");
    }
}
