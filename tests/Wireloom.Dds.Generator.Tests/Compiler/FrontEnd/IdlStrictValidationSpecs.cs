using Wireloom;
using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.FrontEnd.Semantic.Tests;

public sealed class IdlStrictValidationSpecs
{
    public static TheoryData<string, string> KeywordDeclarations => new()
    {
        { "module Module { struct Sample { long number; }; };", "Module" },
        { "struct Struct { long number; };", "Struct" },
        { "valuetype Struct { public long number; };", "Struct" },
        { "enum Enum { First, Second };", "Enum" },
        { "enum Choice { @value(3) Long, Other };", "Long" },
        { "union Union switch(long) { case 1: long number; };", "Union" },
        { "union Choice switch(long) { case 1: long Long; };", "Long" },
        { "typedef long Long;", "Long" },
        { "typedef long Long[2];", "Long" },
        { "typedef sequence<long, 2> Long;", "Long" },
        { "const long Long = 1;", "Long" },
        { "struct Sample { @key long Long; };", "Long" }
    };

    [Theory]
    [MemberData(nameof(KeywordDeclarations))]
    public void DefaultModeWarnsAtTheIdentifierAndPreservesGeneration(string source, string identifier)
    {
        // Arrange
        var input = new IdlInput("keyword.idl", source);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var output = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);

        // Assert
        output.ShouldNotBeEmpty();
        var warning = diagnostics.ShouldHaveSingleItem();
        warning.Id.ShouldBe("DDSG0108");
        warning.Input.ShouldBe(input);
        warning.Offset.ShouldBe(source.IndexOf(identifier, StringComparison.Ordinal));
        warning.Message.ShouldContain($"'{identifier}'");
    }

    [Theory]
    [MemberData(nameof(KeywordDeclarations))]
    public void StrictModeRejectsAtTheIdentifierWithoutAddingThePermissiveWarning(string source, string identifier)
    {
        // Arrange
        var input = new IdlInput("keyword.idl", source, strict: true);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken));

        // Assert
        exception.Offset.ShouldBe(source.IndexOf(identifier, StringComparison.Ordinal));
        exception.Input.ShouldBe(input);
        diagnostics.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("struct Alpha { long first; }; struct alpha { long second; };", "alpha")]
    [InlineData("struct Sample { long number; long Number; };", "Number")]
    [InlineData("union Choice switch(long) { case 1: long number; case 2: long Number; };", "Number")]
    [InlineData("enum First { Value }; enum Second { value };", "value")]
    [InlineData("module Scope { struct First { long number; }; }; module scope { struct Second { long number; }; };", "scope")]
    public void CaseOnlyCollisionsUseTheSameModePolicy(string source, string laterName)
    {
        // Arrange
        var diagnostics = new List<IdlDiagnostic>();
        var input = new IdlInput("collision.idl", source);

        // Act
        var output = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSources([new IdlInput(input.Path, source, strict: true)]));

        // Assert
        output.ShouldNotBeEmpty();
        var warning = diagnostics.ShouldHaveSingleItem();
        warning.Offset.ShouldBe(source.IndexOf(laterName, StringComparison.Ordinal));
        warning.Message.ShouldContain("in scope");
        exception.Offset.ShouldBe(warning.Offset);
    }

    [Theory]
    [InlineData("struct _Struct { long event; };")]
    [InlineData("module Scope { struct First { long number; }; }; module Scope { struct Second { long number; }; };")]
    [InlineData("module Left { struct Sample { long number; }; }; module Right { struct Sample { long number; }; };")]
    [InlineData("struct Sample { long sample; }; struct Other { long sample; };")]
    [InlineData("struct class { long event; };")]
    public void LegalSpellingAndIndependentScopesRemainAccepted(string source)
    {
        // Arrange
        var diagnostics = new List<IdlDiagnostic>();
        var input = new IdlInput("legal.idl", source, strict: true);

        // Act
        var output = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);

        // Assert
        output.ShouldNotBeEmpty();
        diagnostics.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ExactKeywordsAreRejectedInEitherMode(bool strict)
    {
        // Arrange
        var input = new IdlInput("exact.idl", "struct struct { long number; };", strict: strict);

        // Act
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSources([input]));

        // Assert
        exception.Offset.ShouldBe(7);
        exception.Message.ShouldContain("cannot be used as an identifier");
    }

    [Fact]
    public void StrictnessComesOnlyFromGeneratingRootsAndAppliesToTheWholeBatch()
    {
        // Arrange
        var permissive = new IdlInput("first.idl", "struct Struct { long number; };");
        var strict = new IdlInput("second.idl", "struct Other { long number; };", strict: true);
        var tracked = new IdlInput("unrelated.idl", "struct Unused { long number; };", generate: false, strict: true);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var output = IdlCompiler.CompileSourcesWithDiagnostics([permissive, tracked], diagnostics, TestContext.Current.CancellationToken);
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSources([permissive, strict]));

        // Assert
        output.ShouldNotBeEmpty();
        diagnostics.ShouldHaveSingleItem().Id.ShouldBe("DDSG0108");
        exception.Input.ShouldBe(permissive);
    }

    [Fact]
    public void IncludedAndMacroIdentifiersKeepTheirOriginalLocations()
    {
        // Arrange
        var root = new IdlInput("root.idl", "#include \"child.idl\"", strict: true);
        var child = new IdlInput("child.idl", "#define NAME Struct\nstruct NAME { long number; };", generate: false);

        // Act
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSources([root, child]));

        // Assert
        exception.Input.ShouldBe(child);
        exception.Offset.ShouldBe(child.Text.IndexOf("NAME {", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("interface Interface { void call(); };")]
    [InlineData("@service(\"DDS\") interface Interface { void call(); };")]
    public void IgnoredInterfaceNamesStillParticipateInIdentifierValidation(string source)
    {
        // Arrange
        var input = new IdlInput("interface.idl", source, strict: true);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken));

        // Assert
        exception.Offset.ShouldBe(source.IndexOf("Interface", StringComparison.Ordinal));
        diagnostics.ShouldHaveSingleItem().Id.ShouldBe("DDSG0103");
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void ValidatedInterfaceBodiesDoNotChangeGeneratedOutput(bool strict, bool isDdsService)
    {
        // Arrange
        var annotation = string.Empty;
        if (isDdsService)
        {
            annotation = "@service(\"DDS\")";
        }

        var input = new IdlInput("interface.idl", $$"""
            {{annotation}} interface I {
                const long LIMIT = 2;
                typedef sequence<string<16>, 2> Names, MoreNames;
                enum Choice { First, Second };
                struct Item { long number; };
                exception Failure { string message; };
                union Variant switch(long) { case 0: long number; default: string label; };
                readonly attribute long count;
                attribute string<16> label getraises(Failure) setraises(Failure);
                void ping(in long count) raises(Failure) context("context", "}");
                void pong(out ::I::Names names, inout Item item);
            };
            struct Sample { long number; };
            """, strict: strict);
        var control = new IdlInput(input.Path, "struct Sample { long number; };", strict: strict);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var output = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);
        var expected = IdlCompiler.CompileSources([control], TestContext.Current.CancellationToken);

        // Assert
        diagnostics.ShouldHaveSingleItem().Id.ShouldBe("DDSG0103");
        output.Keys.Order().ShouldBe(expected.Keys.Order());
        foreach (var source in output)
        {
            source.Value.Source.ShouldBe(expected[source.Key].Source);
        }
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void IgnoringAnInterfaceBodyDoesNotExemptItsName(bool strict, bool isDdsService)
    {
        // Arrange
        var source = "interface context { void ping(); };";
        if (isDdsService)
        {
            source = "@service(\"DDS\") " + source;
        }

        var input = new IdlInput("interface.idl", source, strict: strict);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var exception = Should.Throw<IdlException>(() => IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken));

        // Assert
        exception.Offset.ShouldBe(source.IndexOf("context", StringComparison.Ordinal));
        exception.Message.ShouldContain("cannot be used as an identifier");
        diagnostics.ShouldHaveSingleItem().Id.ShouldBe("DDSG0103");
    }

    [Fact]
    public void SharedIncludesReportOneWarningForOneParsedOccurrence()
    {
        // Arrange
        var first = new IdlInput("first.idl", "#include \"shared.idl\"");
        var second = new IdlInput("second.idl", "#include \"shared.idl\"");
        var shared = new IdlInput("shared.idl", "struct Struct { long number; };", generate: false);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var output = IdlCompiler.CompileSourcesWithDiagnostics([second, shared, first], diagnostics, TestContext.Current.CancellationToken);

        // Assert
        output.ShouldNotBeEmpty();
        diagnostics.ShouldHaveSingleItem().Input.ShouldBe(shared);
    }

    [Fact]
    public void InheritedCaseOnlyHidingRetainsTheRtiAcceptedBehavior()
    {
        // Arrange
        var input = new IdlInput("inherited.idl", "struct Parent { long field; }; struct Child : Parent { long Field; };", strict: true);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var output = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);

        // Assert
        output.ShouldNotBeEmpty();
        diagnostics.ShouldBeEmpty();
    }

    [Fact]
    public void IdentifierRulesObserveCancellationWithoutDependingOnStrictness()
    {
        // Arrange
        var input = new IdlInput("input.idl", "Struct");
        var occurrences = new[] { new IdlIdentifierOccurrence("Struct", "", IdlIdentifierScopeKind.Namespace, input, 0) };
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        // Act
        var exception = Record.Exception(() => IdlIdentifierValidator.FindViolations(occurrences, cancellation.Token).ToArray());

        // Assert
        exception.ShouldBeOfType<OperationCanceledException>();
    }

    [Theory]
    [InlineData("ReservedKeyword", false, true)]
    [InlineData("ReservedKeyword", true, true)]
    [InlineData("KeywordCaseCollision", false, false)]
    [InlineData("KeywordCaseCollision", true, true)]
    [InlineData("IdentifierCaseCollision", false, false)]
    [InlineData("IdentifierCaseCollision", true, true)]
    [InlineData("DerivedKey", false, false)]
    [InlineData("DerivedKey", true, true)]
    public void ADiscardedDiagnosticSinkNeverDisablesErrors(string ruleName, bool strict, bool rejects)
    {
        // Arrange
        var input = new IdlInput("policy.idl", "Name");
        var violation = new IdlSpecificationViolation(Enum.Parse<IdlSpecificationRule>(ruleName), input, 0, "Name", "name", "Scope");

        // Act
        var exception = Record.Exception(() => IdlValidationPolicy.Apply(violation, strict, diagnostics: null));

        // Assert
        (exception is IdlException).ShouldBe(rejects);
    }
}
