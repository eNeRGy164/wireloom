using Wireloom.Compiler.FrontEnd.Parsing;
using Wireloom.Compiler.FrontEnd.Symbols;
using static Wireloom.Compiler.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Semantic.Tests;

/// <summary>Verifies annotation literals and expressions at parser and binding boundaries.</summary>
public sealed class AnnotationValueSpecs
{
    [Theory]
    [InlineData("\"value)\"")]
    [InlineData("\"value{\"")]
    [InlineData("\"value}after\"")]
    [InlineData("\"escaped\\\"})\"")]
    public void PreservesQuotedDelimitersInMemberAndTypedefDefaults(string literal)
    {
        // Arrange
        var input = Input("quoted-default.idl",
            $$"""
            module Example {
                @default(value={{literal}}) typedef string<32> Text;
                struct Sample {
                    @default({{literal}}) string<32> text;
                    Text inherited;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Example.Sample.g.cs"].Source.ShouldContain($"text = {literal};");
        documents["Example.Text.g.cs"].Source.ShouldContain($"Value = {literal};");
        documents["Example.Implementation.SampleUnmanaged.g.cs"].Source.ShouldContain($"text.ToNative({literal}");
    }

    [Fact]
    public void DecodesHexadecimalOctalAndUnicodeEscapesInStringDefaults()
    {
        // Arrange
        var input = Input("string-default-escapes.idl",
            "struct Sample { @default(L\"\\x41\\101\\u03A9\") wstring<8> text; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Sample.g.cs"].Source.ShouldContain("text = \"AAΩ\";");
        documents["Implementation.SamplePlugin.g.cs"].Source.ShouldContain("StringValue = \"AAΩ\"");
    }

    [Fact]
    public void DecodesStandardControlQuoteAndBackslashStringEscapes()
    {
        // Arrange
        var input = Input("standard-string-escapes.idl",
            "struct Sample { @default(\"\\a\\b\\f\\n\\r\\t\\v\\\\\\'\\\"\\?\") string<32> text; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Sample.g.cs"].Source.ShouldContain("text = \"\\a\\b\\f\\n\\r\\t\\v\\\\'\\\"?\";");
    }

    [Theory]
    [InlineData("'\\a'", false, 7)]
    [InlineData("'A'", false, 65)]
    [InlineData("L'a'", true, 97)]
    [InlineData("'\\b'", false, 8)]
    [InlineData("'\\f'", false, 12)]
    [InlineData("'\\v'", false, 11)]
    [InlineData("'\\?'", false, 63)]
    [InlineData("'\\\\'", false, 92)]
    [InlineData("'\\\''", false, 39)]
    [InlineData("'\\\"'", false, 34)]
    [InlineData("'\\x4'", false, 4)]
    [InlineData("'\\x41'", false, 65)]
    [InlineData("'\\x00'", false, 0)]
    [InlineData("'\\1'", false, 1)]
    [InlineData("'\\12'", false, 10)]
    [InlineData("'\\101'", false, 65)]
    [InlineData("L'\\777'", true, 511)]
    [InlineData("'\\000'", false, 0)]
    [InlineData("L'\\uA'", true, 10)]
    [InlineData("L'\\u03A9'", true, 937)]
    public void DecodesIdlCharacterEscapes(string literal, bool allowWideLiteral, int expected)
    {
        // Arrange

        // Act
        var parsed = IdlCharacterLiteralParser.TryParse(literal, allowWideLiteral, out var value);

        // Assert
        parsed.ShouldBeTrue();
        value.ShouldBe(expected);
    }

    [Theory]
    [InlineData("'\\x'")]
    [InlineData("'\\U00000041'")]
    [InlineData("'\\u0041'")]
    [InlineData("L'a'")]
    [InlineData("'\\x100'")]
    public void RejectsMalformedOrWideOnlyNarrowCharacterEscapes(string literal)
    {
        // Arrange

        // Act
        var parsed = IdlCharacterLiteralParser.TryParse(literal, allowWideLiteral: false, out _);

        // Assert
        parsed.ShouldBeFalse();
    }

    [Theory]
    [InlineData("@default(\"\\0\") string<4> text;", "NUL character")]
    [InlineData("@default(\"\\000\") string<4> text;", "NUL character")]
    [InlineData("@default(\"\\x00\") string<4> text;", "NUL character")]
    [InlineData("@default(L\"\\u0000\") wstring<4> text;", "NUL character")]
    [InlineData("@default(\"\\u0041\") string<4> text;", "only valid in a wide string")]
    [InlineData("@default(\"\\x\") string<4> text;", "Invalid hexadecimal escape")]
    [InlineData("@default(\"\\U00000041\") string<4> text;", "Unsupported escape sequence")]
    public void RejectsInvalidIdlStringEscapes(string member, string expectedMessage)
    {
        // Arrange
        var input = Input("invalid-string-default-escape.idl", $"struct Sample {{ {member} }};");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain(expectedMessage);
    }

    [Fact]
    public void RejectsLiteralNulCharactersInStringDefaults()
    {
        // Arrange
        var input = Input("literal-nul-string-default.idl",
            "struct Sample { @default(\"" + '\0' + "\") string<4> text; };");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("NUL character");
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    public void PreservesNamedBooleanAnnotationValues(string literal, bool expected)
    {
        // Arrange
        var input = Input("named-booleans.idl",
            $$"""
            struct Sample {
                @key(value={{literal}}) long keyed;
                @optional(value={{literal}}) long optional;
                @external(value={{literal}}) long external;
                @must_understand(value={{literal}}) long understood;
            };
            """);
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), CancellationToken.None);

        // Act
        parser.Parse(input.Text, input, baseOffset: 0, currentNamespace: null);
        parser.Bind();
        parser.Validate(strict: false);

        // Assert
        var fields = parser.Declarations.OfType<IdlClassDeclaration>().Single().Fields;
        fields.Single(field => field.Name == "keyed").Metadata.IsKey.ShouldBe(expected);
        fields.Single(field => field.Name == "optional").Metadata.IsOptional.ShouldBe(expected);
        fields.Single(field => field.Name == "external").Metadata.IsExternal.ShouldBe(expected);
        fields.Single(field => field.Name == "understood").Metadata.IsMustUnderstand.ShouldBe(expected);
    }

    [Fact]
    public void ResolvesScopedConstantMemberIdsBeforeHashPolicyAndValidation()
    {
        // Arrange
        var input = Input("constant-member-ids.idl",
            """
            module Example {
                const long MemberId = 7;
                @autoid(HASH) struct Sample {
                    @id(value=(MemberId + 1)) long first;
                    @id(::Example::MemberId) long second;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var plugin = documents["Example.Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldContain("new StructMember(\"first\", dtf.GetPrimitiveType<int>(), id: 8)");
        plugin.ShouldContain("new StructMember(\"second\", dtf.GetPrimitiveType<int>(), id: 7)");
    }

    [Fact]
    public void ResolvesChainedEnumConstantsInDefaultsAndUnionLabels()
    {
        // Arrange
        var input = Input("chained-enum-constants.idl",
            """
            module Example {
                enum Kind { FIRST, SECOND };
                const Kind Initial = Kind::SECOND;
                const Kind Selected = Initial;
                struct Sample { @default(Selected) Kind value; };
                union Choice switch(Kind) { case Selected: long second; case FIRST: long first; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Example.Selected.g.cs"].Source.ShouldContain("const Kind Value = Kind.SECOND;");
        documents["Example.Sample.g.cs"].Source.ShouldContain("value = (Kind)(1);");
        documents["Example.Choice.g.cs"].Source.ShouldContain("Kind.SECOND");
    }

    [Fact]
    public void ResolvesNamedBooleanConstantsInDefaultsAfterBinding()
    {
        // Arrange
        var input = Input("boolean-constant-default.idl",
            "typedef boolean Flag; struct Sample { @default(Selected) boolean value; }; const Flag Enabled = TRUE; const boolean Selected = Enabled;");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Sample.g.cs"].Source.ShouldContain("value = true;");
        documents["Implementation.SamplePlugin.g.cs"].Source.ShouldContain("BoolValue = true");
    }

    [Fact]
    public void RejectsEnumDefaultQualifiedByAnotherEnumType()
    {
        // Arrange
        var input = Input("wrong-qualified-enum-default.idl",
            "enum First { Entry }; enum Second { Entry }; struct Sample { @default(First::Entry) Second value; };");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Unknown default enum value");
    }

    [Theory]
    [InlineData("@id(Missing) long first;", "Unknown integral constant")]
    [InlineData("@id(2147483648) long first;", "invalid @id")]
    [InlineData("@id(MemberId) @id(1) long first;", "Duplicate or invalid @id")]
    [InlineData("@id(MemberId) @hashid(\"stable\") long first;", "Duplicate or conflicting")]
    [InlineData("@hashid(\"stable\") @id(MemberId) long first;", "Duplicate or invalid @id")]
    [InlineData("@id(MemberId) long first; @id(7) long second;", "Duplicate member ID: 7")]
    [InlineData("@id((MemberId + 1)) long first; @id(8) long second;", "Duplicate member ID: 8")]
    public void RejectsInvalidOrConflictingConstantMemberIds(string members, string message)
    {
        // Arrange
        var input = Input("invalid-constant-member-id.idl",
            $$"""
            const long MemberId = 7;
            struct Sample { {{members}} };
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain(message);
    }

    [Fact]
    public void RejectsEnumConstantAliasesFromAnotherEnum()
    {
        // Arrange
        var input = Input("foreign-enum-constant-alias.idl",
            """
            enum First { ONE };
            enum Second { TWO };
            const First Initial = ONE;
            const Second Invalid = Initial;
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("does not belong to Second");
    }
}
