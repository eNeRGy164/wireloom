
using static Wireloom.Compiler.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Parsing.Tests;

public sealed class IdlAnnotationCatalogSpecs
{
    [Theory]
    [InlineData("struct Sample { @key(1) long value; };")]
    [InlineData("struct Sample { @unit(12) long value; };")]
    [InlineData("@language_binding(\"CSharp\") struct Sample { long value; };")]
    public void CatalogEnforcesDeclaredParameterKinds(string source)
    {
        // Arrange
        var input = Input("annotation-parameter-kind.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("must be a");
    }

    [Fact]
    public void CatalogValidatesReorderedNamedArgumentsWithoutChangingTheirValues()
    {
        // Arrange
        var application = new IdlAnnotationApplicationSyntax("range", "@range(max=10,min=0)", 0, 23);

        // Act
        IdlAnnotationCatalog.ValidateUniqueNamedArguments(application);
        var arguments = application.ParseArguments()
            .ToDictionary(argument => argument.Name!, argument => argument.Expression, StringComparer.Ordinal);

        // Assert
        arguments["min"].ShouldBe("0");
        arguments["max"].ShouldBe("10");
    }

    [Fact]
    public void CatalogRejectsAnUnknownNamedArgumentForAKnownAnnotation()
    {
        // Arrange
        var application = new IdlAnnotationApplicationSyntax("key", "@key(enabled=true)", 0, 20);

        // Act
        var exception = Should.Throw<FormatException>(() => IdlAnnotationCatalog.ValidateUniqueNamedArguments(application));

        // Assert
        exception.Message.ShouldContain("Unknown argument 'enabled'");
    }

    [Fact]
    public void CatalogLeavesUnknownAnnotationArgumentsOpaque()
    {
        // Arrange
        var application = new IdlAnnotationApplicationSyntax("vendor_custom", "@vendor_custom(payload=(x, \"y,z\"))", 0, 38);

        // Act
        Should.NotThrow(() => IdlAnnotationCatalog.ValidateUniqueNamedArguments(application));

        // Assert
        IdlAnnotationCatalog.IsKnown(application.Name).ShouldBeFalse();
    }

    [Fact]
    public void CatalogRejectsKnownApplicationsOnUnsupportedTargets()
    {
        // Arrange
        var application = new IdlAnnotationApplicationSyntax("default", "@default(1)", 0, 11);

        // Act
        var exception = Should.Throw<FormatException>(() =>
            IdlAnnotationCatalog.ValidateTarget(application, IdlAnnotationTarget.EnumLiteral));

        // Assert
        exception.Message.ShouldContain("not supported on this target");
    }

    [Theory]
    [InlineData("@id long value;")]
    [InlineData("@id<(3) long value;")]
    public void MalformedKnownApplicationsStayInsideTheCompilerErrorBoundary(string member)
    {
        // Arrange
        var input = Input("malformed-known-annotation.idl", $"struct Sample {{ {member} }};");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("@id");
    }

    [Fact]
    public void CatalogSeparatesRecognizedUnsupportedAnnotationsFromUnknownOnes()
    {
        // Arrange
        const string unsupported = "position";
        const string unknown = "vendor_custom";

        // Act
        var unsupportedKnown = IdlAnnotationCatalog.IsKnown(unsupported);
        var unsupportedStatus = IdlAnnotationCatalog.IsRecognizedUnsupported(unsupported);
        var unknownKnown = IdlAnnotationCatalog.IsKnown(unknown);

        // Assert
        unsupportedKnown.ShouldBeTrue();
        unsupportedStatus.ShouldBeTrue();
        unknownKnown.ShouldBeFalse();
    }

    [Theory]
    [InlineData("autoid", (int)IdlAnnotationTarget.Declaration, "Duplicate @autoid")]
    [InlineData("default", (int)IdlAnnotationTarget.Typedef, "Duplicate @default")]
    [InlineData("default_literal", (int)IdlAnnotationTarget.EnumLiteral, "Malformed or duplicate @default_literal")]
    [InlineData("value", (int)IdlAnnotationTarget.EnumLiteral, "Duplicate @value")]
    [InlineData("key", (int)IdlAnnotationTarget.Member, "Duplicate @key")]
    [InlineData("optional", (int)IdlAnnotationTarget.Member, "Duplicate @optional")]
    [InlineData("id", (int)IdlAnnotationTarget.Member, "Duplicate or invalid @id")]
    [InlineData("hashid", (int)IdlAnnotationTarget.Member, "Duplicate or conflicting @id/@hashid")]
    [InlineData("min", (int)IdlAnnotationTarget.Member, "Duplicate @min or @range")]
    [InlineData("max", (int)IdlAnnotationTarget.Member, "Duplicate @max or @range")]
    [InlineData("range", (int)IdlAnnotationTarget.Member, "Duplicate @min, @max, or @range")]
    [InlineData("external", (int)IdlAnnotationTarget.Member, "Duplicate @external")]
    [InlineData("must_understand", (int)IdlAnnotationTarget.Member, "Duplicate @must_understand")]
    [InlineData("unit", (int)IdlAnnotationTarget.Member, "Duplicate @unit")]
    [InlineData("extensibility", (int)IdlAnnotationTarget.Declaration, "Duplicate extensibility")]
    [InlineData("final", (int)IdlAnnotationTarget.Declaration, "Duplicate extensibility")]
    [InlineData("appendable", (int)IdlAnnotationTarget.Declaration, "Duplicate extensibility")]
    [InlineData("mutable", (int)IdlAnnotationTarget.Declaration, "Duplicate extensibility")]
    public void CompositionPolicyRejectsRepeatedKnownApplicationsWithStableDiagnostics(
        string name,
        int target,
        string message)
    {
        // Arrange
        var applications = new[]
        {
            new IdlAnnotationApplicationSyntax(name, $"@{name}", 0, name.Length + 1),
            new IdlAnnotationApplicationSyntax(name, $"@{name}", name.Length + 1, name.Length + 1)
        };

        // Act
        var violation = IdlAnnotationCatalog.FindCompositionViolation(applications, (IdlAnnotationTarget)target);

        // Assert
        violation.ShouldNotBeNull();
        violation.Value.Application.Offset.ShouldBe(name.Length + 1);
        violation.Value.Message.ShouldContain(message);
    }

    [Theory]
    [InlineData("id", "hashid", (int)IdlAnnotationTarget.Member, "Duplicate or conflicting @id/@hashid")]
    [InlineData("hashid", "id", (int)IdlAnnotationTarget.Member, "Duplicate or invalid @id")]
    [InlineData("min", "range", (int)IdlAnnotationTarget.Member, "Duplicate @min, @max, or @range")]
    [InlineData("range", "min", (int)IdlAnnotationTarget.Member, "Duplicate @min, @max, or @range")]
    [InlineData("max", "range", (int)IdlAnnotationTarget.Member, "Duplicate @min, @max, or @range")]
    [InlineData("range", "max", (int)IdlAnnotationTarget.Member, "Duplicate @min, @max, or @range")]
    [InlineData("final", "extensibility", (int)IdlAnnotationTarget.Declaration, "Duplicate or conflicting extensibility")]
    [InlineData("final", "appendable", (int)IdlAnnotationTarget.Declaration, "Duplicate or conflicting extensibility")]
    [InlineData("appendable", "mutable", (int)IdlAnnotationTarget.Declaration, "Duplicate or conflicting extensibility")]
    [InlineData("mutable", "final", (int)IdlAnnotationTarget.Declaration, "Duplicate or conflicting extensibility")]
    public void CompositionPolicyRejectsCataloguedConflictsInEitherOrder(
        string first,
        string second,
        int target,
        string message)
    {
        // Arrange
        var applications = new[]
        {
            new IdlAnnotationApplicationSyntax(first, $"@{first}", 0, first.Length + 1),
            new IdlAnnotationApplicationSyntax(second, $"@{second}", first.Length + 1, second.Length + 1)
        };

        // Act
        var violation = IdlAnnotationCatalog.FindCompositionViolation(applications, (IdlAnnotationTarget)target);

        // Assert
        violation.ShouldNotBeNull();
        violation.Value.Message.ShouldContain(message);
    }

    [Fact]
    public void CompositionPolicyAllowsAnnotationsDeclaredRepeatable()
    {
        // Arrange
        var applications = new[]
        {
            new IdlAnnotationApplicationSyntax("nested", "@nested", 0, 7),
            new IdlAnnotationApplicationSyntax("nested", "@nested", 8, 7)
        };

        // Act
        var violation = IdlAnnotationCatalog.FindCompositionViolation(applications, IdlAnnotationTarget.Declaration);

        // Assert
        violation.ShouldBeNull();
    }

    [Theory]
    [InlineData("@range(min=0, 1)", "cannot mix named and positional")]
    [InlineData("@unit(=\"meters\")", "empty argument name")]
    [InlineData("@range(0,1)", "requires named arguments")]
    [InlineData("@key(true,false)", "accepts at most 1 positional")]
    [InlineData("@range(min=0)", "requires argument 'max'")]
    [InlineData("@autoid(INVALID)", "must be one of")]
    [InlineData("@unit(prefix \"meters\")", "must be a stringliteral value")]
    [InlineData("@language_binding(123)", "must be a identifier value")]
    [InlineData("@key(1)", "must be a boolean value")]
    public void ArgumentShapeAndDeclaredKindsRejectInvalidKnownArguments(string text, string message)
    {
        // Arrange
        var application = IdlSyntaxReader.ReadAnnotationApplications(text).Single();

        // Act
        var exception = Should.Throw<FormatException>(() => IdlAnnotationCatalog.ValidateUniqueNamedArguments(application));

        // Assert
        exception.Message.ShouldContain(message);
    }

    [Theory]
    [InlineData("@unit(\"one\")")]
    [InlineData("@unit(\"one\" \"two\")")]
    [InlineData("@unit(L\"wide\")")]
    [InlineData("@key(TRUE)")]
    [InlineData("@key(::Scope::Enabled)")]
    [InlineData("@language_binding(::Scope::CSharp)")]
    public void ArgumentShapeAcceptsDeclaredLiteralAndNameForms(string text)
    {
        // Arrange
        var application = IdlSyntaxReader.ReadAnnotationApplications(text).Single();

        // Act/Assert
        Should.NotThrow(() => IdlAnnotationCatalog.ValidateUniqueNamedArguments(application));
    }

    [Fact]
    public void KnownApplicationWithUnclosedArgumentsReportsMalformedSyntax()
    {
        // Arrange
        var application = new IdlAnnotationApplicationSyntax("key", "@key(true", 0, 9);

        // Act
        var exception = Should.Throw<FormatException>(() => IdlAnnotationCatalog.ValidateUniqueNamedArguments(application));

        // Assert
        exception.Message.ShouldContain("Malformed arguments");
    }

    [Fact]
    public void DuplicateNamedArgumentsAreRejectedBeforeShapeValidation()
    {
        // Arrange
        var application = new IdlAnnotationApplicationSyntax("range", "@range(min=0,min=1,max=2)", 0, 26);

        // Act
        var exception = Should.Throw<FormatException>(() => IdlAnnotationCatalog.ValidateUniqueNamedArguments(application));

        // Assert
        exception.Message.ShouldContain("Duplicate annotation argument 'min'");
    }
}
