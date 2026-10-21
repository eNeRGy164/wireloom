namespace Wireloom.Compiler.FrontEnd.Semantic.Tests;

public sealed class IdlInterfaceValidationSpecs
{
    public static TheoryData<string> ReservedNames => new()
    {
        "void context();",
        "void invoke(in long context);",
        "attribute long context;",
        "const long context = 1;",
        "typedef long context;",
        "typedef long context[2];",
        "enum context { First };",
        "enum Choice { context };",
        "struct context { long number; };",
        "exception context { long number; };",
        "union context switch(long) { case 0: long number; };",
        "valuetype V { factory context(); };",
        "valuetype V { factory create(in long context); };",
        "struct Item { long context; };",
        "exception Failure { long context; };",
        "union Variant switch(long) { case 0: long context; };"
    };

    [Theory]
    [MemberData(nameof(ReservedNames))]
    public void ExactKeywordsInInterfaceDeclarationsFailInBothModes(string body)
    {
        // Arrange
        var source = $"interface I {{ {body} }};";

        // Act
        var exceptions = new[] { false, true }.Select(strict =>
            Should.Throw<IdlException>(() => IdlCompiler.CompileSources([new IdlInput("interface.idl", source, strict: strict)]))).ToArray();

        // Assert
        foreach (var exception in exceptions)
        {
            exception.Offset.ShouldBe(source.IndexOf("context", StringComparison.Ordinal));
            exception.Message.ShouldContain("cannot be used as an identifier");
        }
    }

    [Theory]
    [MemberData(nameof(ReservedNames))]
    public void CaseVariantKeywordsUseTheExistingWarningAndStrictPolicy(string body)
    {
        // Arrange
        var source = $"@service(\"DDS\") interface I {{ {body.Replace("context", "Context", StringComparison.Ordinal)} }};";
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var output = IdlCompiler.CompileSourcesWithDiagnostics([new IdlInput("interface.idl", source)], diagnostics, TestContext.Current.CancellationToken);
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSources([new IdlInput("interface.idl", source, strict: true)]));

        // Assert
        output.ShouldBeEmpty();
        diagnostics.Select(item => item.Id).ShouldBe(["DDSG0103", "DDSG0108"]);
        diagnostics[1].Offset.ShouldBe(source.IndexOf("Context", StringComparison.Ordinal));
        exception.Offset.ShouldBe(diagnostics[1].Offset);
    }

    [Theory]
    [InlineData("void ping(); void Ping();", "Ping")]
    [InlineData("void send(in long number, out long Number);", "Number")]
    [InlineData("attribute long number, Number;", "Number")]
    [InlineData("typedef long Item; const long item = 1;", "item")]
    [InlineData("struct Item { long number; long Number; };", "Number")]
    [InlineData("union Variant switch(long) { case 0: long number; case 1: long Number; };", "Number")]
    public void CaseOnlyCollisionsRespectInterfaceAndParameterScopes(string body, string laterName)
    {
        // Arrange
        var source = $"@service(\"DDS\") interface I {{ {body} }};";
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        IdlCompiler.CompileSourcesWithDiagnostics([new IdlInput("interface.idl", source)], diagnostics, TestContext.Current.CancellationToken);
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSources([new IdlInput("interface.idl", source, strict: true)]));

        // Assert
        diagnostics.Select(item => item.Id).ShouldBe(["DDSG0103", "DDSG0108"]);
        exception.Offset.ShouldBe(source.IndexOf(laterName, StringComparison.Ordinal));
        diagnostics[1].Offset.ShouldBe(exception.Offset);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DdsServiceAndOrdinaryInterfacesUseTheSameBodyValidation(bool isDdsService)
    {
        // Arrange
        var source = "interface I { void context(); };";
        if (isDdsService)
        {
            source = "@service(\"DDS\") " + source;
        }

        var input = new IdlInput("interface.idl", source, strict: true);

        // Act
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSources([input]));

        // Assert
        exception.Offset.ShouldBe(source.IndexOf("context", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("void ping(in long number); void pong(in long Number);")]
    [InlineData("struct First { long number; }; struct Second { long Number; };")]
    [InlineData("@note(value = (1 + (2))) void ping(@note(\"context\") in long number);")]
    [InlineData("const long LIMIT = 1'024; typedef long Values[LIMIT];")]
    [InlineData("typedef sequence<sequence<long, 2>, 3> Values; void send(in Values values);")]
    [InlineData("typedef sequence<long, 8 >> 1> Values; typedef sequence<long, 1 << 2> More;")]
    [InlineData("const wstring TEXT = L\"context } \\\"\"; const char BRACE = '}';")]
    [InlineData("oneway void ping(in unsigned long long number, in long double fraction, out long long result);")]
    [InlineData("void send(in unsigned short number);")]
    [InlineData("struct Item; struct Derived : Item { private long number; public string label; };")]
    [InlineData("struct Item; struct Item { long number; }; struct Item;")]
    [InlineData("enum Choice { @value(1) First, Second = (1 + 1), }; union Variant switch(Choice) { case First: case Second: long number; };")]
    [InlineData("void ping() context(\"context\", \"{ [ ( }\");")]
    [InlineData("typedef struct Item { long number; } Alias;")]
    [InlineData("valuetype V { factory create(); }; struct Sample { long number; };")]
    [InlineData("valuetype V { public long number; factory create(in long initialNumber, in string label); }; struct Sample { long number; };")]
    [InlineData("valuetype V { factory create() raises (Failure); }; struct Sample { long number; };")]
    [InlineData("typedef sequence<long, (1 + 2)> Values;")]
    [InlineData("typedef sequence<sequence<long>> Values;")]
    [InlineData("@notes::note(values = {1, 2}) void ping();")]
    public void ReferencesExpressionsLiteralsAndIndependentScopesAreNotDeclarations(string body)
    {
        // Arrange
        var diagnostics = new List<IdlDiagnostic>();
        var input = new IdlInput("interface.idl", $"interface I {{ {body} }};", strict: true);

        // Act
        var output = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);

        // Assert
        output.ShouldBeEmpty();
        diagnostics.ShouldHaveSingleItem().Id.ShouldBe("DDSG0103");
    }

    [Theory]
    [InlineData("void ping(")]
    [InlineData("long 42();")]
    [InlineData("void ping() raises;")]
    [InlineData("typedef sequence<long, 2 Value;")]
    [InlineData("attribute long count another;")]
    [InlineData("enum Choice { 42 };")]
    [InlineData("@note([)) void ping();")]
    [InlineData("struct Item { const long LIMIT = 1 };")]
    [InlineData("union Variant switch(long) { case 0 };")]
    public void MalformedMemberStructureDoesNotSilentlySkipDeclaredNames(string body)
    {
        // Arrange
        var input = new IdlInput("interface.idl", $"interface I {{ {body} }};");

        // Act
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSources([input]));

        // Assert
        exception.Message.ShouldContain("interface");
        exception.Offset.ShouldBeInRange(0, input.Text.Length);
    }

    [Theory]
    [InlineData("interface I {", "Unterminated interface")]
    [InlineData("interface I { void ping(); }", "must end with a semicolon")]
    public void InterfaceBoundariesFailWithSourceLocatedErrors(string source, string message)
    {
        // Arrange
        var input = new IdlInput("interface.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSources([input]));

        // Assert
        exception.Input.ShouldBe(input);
        exception.Message.ShouldContain(message);
    }

    [Theory]
    [InlineData("void Context();")]
    [InlineData("void ping(); void Ping();")]
    [InlineData("void ping(); void ping();")]
    [InlineData("void send(in long number, out long Number);")]
    public void OrdinaryInterfaceBodiesRetainRtiAcceptanceForSpecificationOnlyViolations(string body)
    {
        // Arrange
        var input = new IdlInput("interface.idl", $"interface I {{ {body} }};", strict: true);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var output = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);

        // Assert
        output.ShouldBeEmpty();
        diagnostics.ShouldHaveSingleItem().Id.ShouldBe("DDSG0103");
    }

    [Theory]
    [InlineData(64, true)]
    [InlineData(65, false)]
    public void NestedBodyTypesHaveABoundedParsingDepth(int depth, bool accepted)
    {
        // Arrange
        var body = "long number;";
        for (var index = depth - 1; index >= 0; index--)
        {
            body = $"struct Item{index} {{ {body} }} value{index};";
        }

        // The outer constructed declaration has no field declarator.
        body = body[..body.LastIndexOf(" value0", StringComparison.Ordinal)] + ";";
        var input = new IdlInput("nested.idl", $"interface I {{ {body} }};");

        // Act
        var exception = Record.Exception(() => IdlCompiler.CompileSources([input], TestContext.Current.CancellationToken));

        // Assert
        if (accepted)
        {
            exception.ShouldBeNull();
        }
        else
        {
            exception.ShouldBeOfType<IdlException>().Message.ShouldContain("nesting depth of 64");
        }
    }

    [Fact]
    public void IncludedAndMacroExpandedOperationNamesKeepOriginalLocations()
    {
        // Arrange
        var root = new IdlInput("root.idl", "#include \"service.idl\"", strict: true);
        var service = new IdlInput("service.idl", """
            #define NAME Context
            @service("DDS") interface I { void NAME(); };
            """, generate: false);

        // Act
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSources([root, service]));

        // Assert
        exception.Input.ShouldBe(service);
        exception.Offset.ShouldBe(service.Text.IndexOf("NAME()", StringComparison.Ordinal));
    }
}
