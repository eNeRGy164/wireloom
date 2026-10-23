using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.FrontEnd.Symbols;

using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Parsing.Tests;

/// <summary>Checks annotation composition independently of the order of parser handlers.</summary>
public sealed class AnnotationOrderingReviewSpecs
{
    [Theory]
    [InlineData("@custom @default(7) typedef long Count;")]
    [InlineData("@default(7) @custom typedef long Count;")]
    [InlineData("@default(7) @Vendor::custom(value=(1 + (2 * 3)), text=\"a),b\") typedef long Count;")]
    [InlineData("struct Sample { @custom @default(7) long value; };")]
    [InlineData("struct Sample { @default(7) @Vendor::custom(value=(1 + 2)) long value; };")]
    public void IgnoresCompleteUnknownApplicationsAmongSupportedAnnotations(string source)
    {
        // Arrange
        var input = Input("annotation-order.idl", source);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var documents = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);

        // Assert
        documents.ShouldNotBeEmpty();
        diagnostics.Count(diagnostic => diagnostic.Id == "DDSG0101").ShouldBe(1);
        var expectedOffset = source.Contains("@Vendor::", StringComparison.Ordinal)
            ? source.IndexOf("@Vendor::", StringComparison.Ordinal)
            : source.IndexOf("@custom", StringComparison.Ordinal);
        diagnostics.Single(diagnostic => diagnostic.Id == "DDSG0101").Offset.ShouldBe(expectedOffset);
    }

    [Theory]
    [InlineData("", false)]
    [InlineData("long value", false)]
    [InlineData("@", false)]
    [InlineData("@invalid:", false)]
    [InlineData("@Vendor::", false)]
    [InlineData("@custom(", false)]
    [InlineData("@custom(\"unterminated)", false)]
    [InlineData("@custom", true)]
    [InlineData("@custom  ", true)]
    [InlineData("@::Vendor::custom(value=(1 + (2)), text=\"),\") ", true)]
    public void ReadsCompleteAnnotationSyntaxWithoutInterpretingUnknownArguments(string source, bool expected)
    {
        // Arrange/Act
        var success = IdlSyntaxReader.TryReadAnnotationApplication(source, out _, out var length);

        // Assert
        success.ShouldBe(expected);
        if (success)
        {
            length.ShouldBe(source.Length);
        }
    }

    [Theory]
    [InlineData("@range(min=0,min=1,max=2) long value;")]
    [InlineData("@default(value=1,value=2) long value;")]
    public void RejectsRepeatedNamedArgumentsForKnownAnnotations(string member)
    {
        // Arrange
        var input = Input("duplicate-annotation-argument.idl", $"struct Sample {{ {member} }};");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Duplicate annotation argument");
    }

    [Fact]
    public void LeavesRepeatedNamedArgumentsInUnknownApplicationsOpaque()
    {
        // Arrange
        var input = Input("unknown-duplicate-arguments.idl", "@Vendor::custom(value=1,value=(2 + 3)) struct Sample { long value; };");
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var documents = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);

        // Assert
        documents.ShouldNotBeEmpty();
        diagnostics.Count(diagnostic => diagnostic.Id == "DDSG0101").ShouldBe(1);
    }

    [Fact]
    public void CapturesApplicationsInSourceOrderWithOffsetsAndBalancedText()
    {
        // Arrange
        const string source = " @key @Vendor::custom(value=(1 + (2 * 3)), text=\"a),b\") @default(7) long value;";

        // Act
        var applications = IdlSyntaxReader.ReadAnnotationApplications(source);

        // Assert
        applications.Select(application => application.Name).ShouldBe(["key", "Vendor::custom", "default"]);
        applications.Select(application => application.Offset).ShouldBe([1, source.IndexOf("@Vendor::", StringComparison.Ordinal), source.IndexOf("@default", StringComparison.Ordinal)]);
        applications[1].Text.ShouldContain("text=\"a),b\"");
    }

    [Fact]
    public void ParsesKnownArgumentsAsOrderedNodesWithExpressionSpans()
    {
        // Arrange
        const string source = "@range(max=(10 + (2 * 3)), min=1)";
        var application = IdlSyntaxReader.ReadAnnotationApplications(source).Single();

        // Act
        var arguments = application.ParseArguments();

        // Assert
        arguments.Select(argument => argument.Name).ShouldBe(["max", "min"]);
        arguments.Select(argument => argument.Expression).ShouldBe(["(10 + (2 * 3))", "1"]);
        source.Substring(arguments[0].RelativeOffset, arguments[0].Expression.Length).ShouldBe(arguments[0].Expression);
        source.Substring(arguments[1].RelativeOffset, arguments[1].Expression.Length).ShouldBe(arguments[1].Expression);
    }

    [Theory]
    [InlineData("@default(5) @range(max=10,min=0) @Vendor::custom(value=(1 + 2)) @unit(value=\"meters\")")]
    [InlineData("@unit(value=\"meters\") @Vendor::custom(value=(1 + 2)) @range(min=0,max=10) @default(5)")]
    [InlineData("@range(min=0,max=10) @default(5) @unit(value=\"meters\") @Vendor::custom(value=(1 + 2))")]
    public void MixedKnownAndUnknownApplicationsProduceTheSamePublicOutput(string annotations)
    {
        // Arrange
        var source = $"struct Sample {{ {annotations} long value; }};";
        var input = Input("annotation-composition.idl", source);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var documents = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);
        var generated = string.Join("\n", documents.Values.Select(document => document.Source));

        // Assert
        generated.ShouldContain("5");
        generated.ShouldContain("meters");
        diagnostics.Count(diagnostic => diagnostic.Id == "DDSG0101").ShouldBe(1);
    }

    [Theory]
    [InlineData("@key @id(1) long keyed; @Vendor::one(value=(1 + 2), text=\"x,y\") @optional(value=true) @id(value=2) long optional; @default(5) @range(max=10,min=0) @unit(value=\"meters\") @Vendor::two(a=(2,3)) long measured; @hashid(value=\"stable\") long hashed; @external @must_understand long flags; @resolve_name(true) long resolved; @min(1) @Vendor::three(expr=(1 << 1)) long lowerBound; @Vendor::four(flag=true) @max(9) long upperBound;")]
    [InlineData("@id(1) @key long keyed; @id(value=2) @Vendor::one(value=(1 + 2), text=\"x,y\") @optional(value=true) long optional; @unit(value=\"meters\") @Vendor::two(a=(2,3)) @range(min=0,max=10) @default(5) long measured; @hashid(value=\"stable\") long hashed; @must_understand @external long flags; @resolve_name(true) long resolved; @Vendor::three(expr=(1 << 1)) @min(1) long lowerBound; @max(9) @Vendor::four(flag=true) long upperBound;")]
    public void ComposesEverySupportedMemberAnnotationWithUnknownApplicationsInMixedOrder(string members)
    {
        // Arrange
        var input = Input("catalog-annotation-composition.idl", $"struct Sample {{ {members} }};");
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var documents = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);
        var generated = string.Join("\n", documents.Values.Select(document => document.Source));

        // Assert
        documents.ShouldContainKey("Sample.g.cs");
        generated.ShouldContain("keyed");
        generated.ShouldContain("optional");
        generated.ShouldContain("isKey: true");
        generated.ShouldContain("isOptional: true");
        generated.ShouldContain("id: 1");
        generated.ShouldContain("id: 2");
        generated.ShouldContain("measured");
        generated.ShouldContain("meters");
        generated.ShouldContain("stable");
        generated.ShouldContain("isMustUnderstand: true");
        generated.ShouldContain("5");
        generated.ShouldContain("lowerBound");
        generated.ShouldContain("upperBound");
        diagnostics.Count(diagnostic => diagnostic.Id == "DDSG0101").ShouldBe(4);
    }

    [Fact]
    public void ComposesBuiltInAnnotationsAcrossTargetsWithUnknownApplicationsInterleaved()
    {
        // Arrange
        const string source = """
            @default_nested
            @Vendor::before(value=(1 + 2))
            @language_binding(CSharp)
            @transfer_mode(SHMEM_REF)
            @data_representation(XCDR2)
            @allowed_data_representation(XCDR2)
            @topic
            @autoid(SEQUENTIAL)
            @final
            @nested
            struct Sample {
                @Vendor::member(value=(1 + (2 * 3)))
                @key
                @id(1)
                @must_understand
                @default(1)
                @range(max=2,min=0)
                @unit("meters")
                @resolve_name(true)
                long measured;
                @Vendor::hash(text="stable") @hashid("stable") long hashed;
                @Vendor::external(value=(1 + 2)) @unit("external-only") @external long externalValue;
                @min(0) long lowerBound;
                @Vendor::max(value=(1 + 1)) @max(2) long upperBound;
            };
            enum Status { @Vendor::enum(text="a,b") @value(1) @default_literal Ready };
            union Choice switch(long) { case 0: @Vendor::branch(value=(1,2)) @optional long selected; };
            """;
        var input = Input("built-in-annotation-catalog-composition.idl", source);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var documents = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);
        var generated = string.Join("\n", documents.Values.Select(document => document.Source));

        // Assert
        documents.ShouldContainKey("Sample.g.cs");
        documents.ShouldContainKey("Status.g.cs");
        documents.ShouldContainKey("Choice.g.cs");
        generated.ShouldContain("isKey: true");
        generated.ShouldContain("isMustUnderstand: true");
        generated.ShouldContain("measured");
        generated.ShouldContain("meters");
        generated.ShouldNotContain("external-only");
        generated.ShouldContain("stable");
        generated.ShouldContain("lowerBound");
        generated.ShouldContain("upperBound");
        generated.ShouldContain("Ready");
        generated.ShouldContain("selected");
        diagnostics.Count(diagnostic => diagnostic.Id == "DDSG0101").ShouldBe(7);
    }

    [Fact]
    public void ParsesArrayAliasOnDefaultUnionBranch()
    {
        // Arrange
        const string source = """
            module RuntimeEnumArrayDefaults {
                enum Color { GREEN, @default_literal RED, BLUE };
                typedef Color ColorAlias;
                union Palette switch(long) {
                    case 1: long other;
                    default: ColorAlias colors[2];
                };
            };
            """;

        // Act
        var documents = IdlCompiler.CompileSources(
            [Input("union-array-alias.idl", source)],
            TestContext.Current.CancellationToken);

        // Assert
        documents.ShouldContainKey("RuntimeEnumArrayDefaults.Palette.g.cs");
        documents["RuntimeEnumArrayDefaults.Palette.g.cs"].Source.ShouldContain("colors");
    }

    [Theory]
    [InlineData("@min(1) @default(FALSE) boolean value;")]
    [InlineData("@max(0) @default(TRUE) boolean value;")]
    [InlineData("@min(66) @default('A') char value;")]
    [InlineData("@max(64) @default(L'A') wchar value;")]
    [InlineData("@default('A') typedef char Letter; struct Inner { @min(66) Letter value; };")]
    public void RejectsTypedDefaultsOutsideDeclaredRanges(string member)
    {
        // Arrange
        var source = member.StartsWith("@default", StringComparison.Ordinal)
            ? member
            : $"struct Sample {{ {member} }};";
        var input = Input("typed-default-range.idl", source);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("outside its declared range");
    }

    [Fact]
    public void ResolvesForwardAndScopedBooleanMemberAnnotationArguments()
    {
        // Arrange
        const string source = "module Scope { struct Sample { @key(Enabled) long keyed; @optional(::Scope::Disabled) long ordinary; }; const boolean Enabled = TRUE; const boolean Disabled = FALSE; };";
        var input = Input("boolean-annotation-constants.idl", source);

        // Act
        var documents = IdlCompiler.CompileSources([input], TestContext.Current.CancellationToken);
        var generated = string.Join("\n", documents.Values.Select(document => document.Source));

        // Assert
        generated.ShouldContain("isKey: true");
        generated.ShouldNotContain("isOptional: true");
    }

    [Theory]
    [InlineData("@final @Vendor::custom(value=(1 + (2 * 3))) struct Sample { long value; };")]
    [InlineData("@Vendor::custom(text=\"a),b\") @extensibility(value=FINAL) struct Sample { long value; };")]
    public void PreservesTypePrefixSemanticsAroundUnknownAnnotations(string declaration)
    {
        // Arrange
        var input = Input("type-prefix-annotation-order.idl", declaration);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var documents = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);

        // Assert
        documents.ShouldContainKey("Sample.g.cs");
        diagnostics.Count(diagnostic => diagnostic.Id == "DDSG0101").ShouldBe(1);
    }

    [Fact]
    public void KeepsUnknownEnumLiteralAnnotationsOpaqueAroundKnownAnnotations()
    {
        // Arrange
        const string source = "enum Status { @Vendor::before(text=\"a,b\") @value(value=7) Ready, @default_literal @Vendor::after(value=(1 + 2)) Unknown };";
        var input = Input("enum-literal-annotation-order.idl", source);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var documents = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);
        var generated = string.Join("\n", documents.Values.Select(document => document.Source));

        // Assert
        generated.ShouldContain("Ready");
        generated.ShouldContain("Unknown");
        diagnostics.Count(diagnostic => diagnostic.Id == "DDSG0101").ShouldBe(2);
    }

    [Fact]
    public void KeepsUnknownUnionBranchAnnotationsOpaqueAroundOptional()
    {
        // Arrange
        const string source = "union Choice switch(long) { case 0: @Vendor::before(value=(1 + 2)) @optional long selected; default: @optional @Vendor::after(text=\"a,b\") long fallback; };";
        var input = Input("union-branch-annotation-order.idl", source);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var documents = IdlCompiler.CompileSourcesWithDiagnostics([input], diagnostics, TestContext.Current.CancellationToken);
        var generated = string.Join("\n", documents.Values.Select(document => document.Source));

        // Assert
        generated.ShouldContain("selected");
        generated.ShouldContain("fallback");
        diagnostics.Count(diagnostic => diagnostic.Id == "DDSG0101").ShouldBe(2);
    }

    [Fact]
    public void AppliesMemberAnnotationsAroundOpaqueApplicationsToTheCorrectDeclarator()
    {
        // Arrange
        const string source = "struct Sample { @Vendor::before(value=(1,2)) @key @Vendor::after(text=\"a,b\") long first, second; };";
        var input = Input("annotation-applications-retained.idl", source);
        var diagnostics = new List<IdlDiagnostic>();
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), TestContext.Current.CancellationToken, diagnostics);

        // Act
        parser.Parse(source, input, 0, currentNamespace: null);
        var declaration = parser.Declarations.OfType<IdlClassDeclaration>().Single();

        // Assert
        declaration.Fields.Select(field => field.Name).ShouldBe(["first", "second"]);
        declaration.Fields[0].Metadata.IsKey.ShouldBeFalse();
        declaration.Fields[1].Metadata.IsKey.ShouldBeTrue();
        diagnostics.Count(diagnostic => diagnostic.Id == "DDSG0101").ShouldBe(2);
    }

    [Fact]
    public void AppliesEnumAndUnionAnnotationsAroundOpaqueApplications()
    {
        // Arrange
        const string source = "enum Status { @Vendor::enum(value=1) @value(7) Ready }; union Choice switch(long) { case 0: @Vendor::branch(text=\"x,y\") @optional long selected; };";
        var diagnostics = new List<IdlDiagnostic>();
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), TestContext.Current.CancellationToken, diagnostics);

        // Act
        parser.Parse(source, Input("annotation-targets-retained.idl", source), 0, currentNamespace: null);
        var enumMember = parser.Declarations.OfType<IdlEnumDeclaration>().Single().Declaration.Members.Single();
        var unionBranch = parser.Declarations.OfType<IdlUnionDeclaration>().Single().Declaration.Branches.Single();

        // Assert
        enumMember.Value.ShouldBe(7);
        unionBranch.Field.Metadata.IsOptional.ShouldBeFalse();
        diagnostics.Count(diagnostic => diagnostic.Id == "DDSG0101").ShouldBe(2);
    }

    [Fact]
    public void AppliesTypePrefixAnnotationsAroundOpaqueApplications()
    {
        // Arrange
        const string source = "@final @Vendor::custom(value=(1 + 2)) struct Sample { long value; };";
        var diagnostics = new List<IdlDiagnostic>();
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), TestContext.Current.CancellationToken, diagnostics);

        // Act
        parser.Parse(source, Input("type-prefix-applications-retained.idl", source), 0, currentNamespace: null);
        var declaration = parser.Declarations.OfType<IdlClassDeclaration>().Single();

        // Assert
        declaration.Extensibility.ShouldBe(IdlExtensibilityKind.Final);
        diagnostics.Count(diagnostic => diagnostic.Id == "DDSG0101").ShouldBe(1);
    }

    [Fact]
    public void LocatesMalformedEnumAnnotationValuesAtTheirExpression()
    {
        // Arrange
        const string source = "enum Status { @value(1 + 2) Ready };";
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), TestContext.Current.CancellationToken);

        // Act
        var exception = Should.Throw<IdlException>(() => parser.Parse(
            source,
            Input("malformed-enum-value.idl", source),
            0,
            currentNamespace: null));

        // Assert
        exception.Message.ShouldContain("@value requires one signed decimal value");
        exception.Offset.ShouldBe(source.IndexOf("1 + 2", StringComparison.Ordinal));
    }

    [Fact]
    public void RejectsNonBooleanConstantsInBooleanMemberAnnotations()
    {
        // Arrange
        var input = Input("integer-key-constant.idl", "const long Enabled = 1; struct Sample { @key(Enabled) long value; };");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Invalid boolean argument for @key");
    }
}
