using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Wireloom.Generation.Tests;

public sealed class GeneratorSpecs
{
    [Theory]
    [InlineData("7.3.1", IdlCompilerProfile.Rti731)]
    [InlineData("7.7.0", IdlCompilerProfile.Rti770)]
    public void RoslynAdapterMatchesPublicCompilerForWarningAndGeneratedDocuments(
        string runtimeVersion,
        IdlCompilerProfile profile)
    {
        // Arrange
        const string source = "struct Sample { long Context; };";

        // Act
        var (compilerResult, adapterResult) = CompileThroughBothBoundaries(source, profile, runtimeVersion);

        // Assert
        compilerResult.Success.ShouldBeTrue();
        adapterResult.GeneratedSources.ShouldNotBeEmpty();
        AssertEquivalentResults(compilerResult, adapterResult);
    }

    [Theory]
    [InlineData("7.3.1", IdlCompilerProfile.Rti731)]
    [InlineData("7.7.0", IdlCompilerProfile.Rti770)]
    public void RoslynAdapterMatchesPublicCompilerForFatalDiagnostic(
        string runtimeVersion,
        IdlCompilerProfile profile)
    {
        // Arrange
        const string source = "#include \"missing.idl\"\n";

        // Act
        var (compilerResult, adapterResult) = CompileThroughBothBoundaries(source, profile, runtimeVersion);

        // Assert
        compilerResult.Success.ShouldBeFalse();
        adapterResult.GeneratedSources.ShouldBeEmpty();
        AssertEquivalentResults(compilerResult, adapterResult);
    }

    [Theory]
    [InlineData("const long long Wide = 7; typedef long Count; const Count Narrow = Wide;")]
    [InlineData("const double Wide = 1.5; typedef float Ratio; const Ratio Narrow = Wide;")]
    [InlineData("const unsigned long Wide = 7; const short Narrow = Wide + 1;")]
    [InlineData("const long long Wide = 7; const int8 Narrow = Wide;")]
    [InlineData("const long long Wide = 7; const uint16 Narrow = Wide;")]
    [InlineData("const unsigned long long Wide = 7; typedef octet Byte; const Byte Narrow = Wide;")]
    [InlineData("const long long Wide = 2147483647; const long Narrow = Wide + 1 - 1;")]
    [InlineData("@default(L'\\uFFFF') typedef wchar High; typedef High HighChain; struct Sample { @default(L'\\u8000') wchar value; HighChain inherited; }; ")]
    public void CompilesBoundConstantConversionsAndFullWidthCharacterDefaults(string input)
    {
        // Arrange
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ShouldBeEmpty();
    }

    [Theory]
    [InlineData("\n", false)]
    [InlineData("\r\n", false)]
    [InlineData("\r", true)]
    [InlineData("\u0085", true)]
    [InlineData("\u2028", true)]
    [InlineData("\u2029", true)]
    [InlineData("\t", true)]
    [InlineData("\\n", true)]
    public void HandlesUnitControlCharactersWithoutGeneratingInvalidCSharp(string separator, bool generatesSources)
    {
        // Arrange
        var input = $$"""
            @language_binding(PLAIN)
            @transfer_mode(INBAND)
            module Sample {
                @autoid(HASH) @appendable struct Value {
                    @key @hashid("stable_id")long id;
                    @min(0) @max(100) @default(50) long count;
                    @range(min = -32, max = 31) @unit("mete{{separator}}s") long distance;
                    @resolve_name(false) string<16> name;
                    @external string<16> externalName;
                };
            };
            """;
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ShouldBeEmpty();

        if (!generatesSources)
        {
            result.Diagnostics.ShouldHaveSingleItem().Id.ShouldBe("DDSG0001");
            result.Output.SyntaxTrees.Count().ShouldBe(1);
            return;
        }

        result.Diagnostics.ShouldBeEmpty();
        var annotationTokens = result.Output.SyntaxTrees
            .SelectMany(tree => tree.GetRoot().DescendantTokens())
            .Where(token => token.IsKind(SyntaxKind.StringLiteralToken))
            .ToArray();
        var expectedUnit = separator switch
        {
            "\\n" => "mete\ns",
            "\\r" => "mete\rs",
            _ => $"mete{separator}s"
        };
        annotationTokens.ShouldContain(token => token.ValueText == expectedUnit);
    }

    [Fact]
    public void GeneratesSourcesWhenRuntimeAndMetadataAreAvailable()
    {
        // Arrange
        var input = """module Sample { struct Value { long value; }; };""";
        var metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Generate"] = "true",
            ["Strict"] = "true",
            ["Defines"] = "FEATURE_A; FEATURE_B",
            ["Undefines"] = "FEATURE_C",
            ["IncludeDirectories"] = "idl;idl/includes"
        };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeTrue();
        result.Output.SyntaxTrees.Count().ShouldBeGreaterThan(1);
    }

    [Fact]
    public void CompilesTypedDefaultsAfterBindingForwardAliasesAndConstants()
    {
        // Arrange
        const string input =
            """
            module TypedDefaults {
                const long Selected = 7;
                enum Kind { First, Second };
                const Kind Chosen = Second;
                @default(Selected) typedef Later Base;
                typedef long Later;
                @default(Chosen) typedef Kind EnumValue;
                @default(TRUE) typedef boolean Enabled;
                @default(FALSE) typedef boolean Disabled;
                @default('\n') typedef char LineFeed;
                @default(L'\u03a9') typedef wchar Omega;
                @default(L"wide") typedef wstring<8> WideText;
                struct Sample {
                    Base number;
                    EnumValue kind;
                    Enabled enabled;
                    Disabled disabled;
                    LineFeed lineFeed;
                    Omega omega;
                    WideText text;
                };
            };
            """;
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);
        var generatedSources = string.Concat(result.Output.SyntaxTrees.Select(tree => tree.GetText().ToString()));

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ShouldBeEmpty();
        generatedSources.ShouldContain("Value = 7;");
        generatedSources.ShouldContain("Value = (Kind)(1);");
        generatedSources.ShouldContain("Value = true;");
        generatedSources.ShouldContain("Value = false;");
        generatedSources.ShouldContain("Value = '\\n';");
        generatedSources.ShouldContain("Value = 'Ω';");
        generatedSources.ShouldContain("Value = \"wide\";");
        generatedSources.ShouldContain("enabled = true;");
        generatedSources.ShouldContain("disabled = false;");
        generatedSources.ShouldContain("lineFeed = '\\n';");
        generatedSources.ShouldContain("omega = 'Ω';");
        generatedSources.ShouldContain("text = \"wide\";");
    }

    [Fact]
    public void SkipsPrerequisiteChecksWhenNoIdlRootIsMarkedForGeneration()
    {
        // Arrange
        var input = "module Sample { struct Value { long value; }; };";
        var metadata = new Dictionary<string, string>
        {
            ["Generate"] = "false",
            ["Strict"] = "true"
        };

        // Act
        var result = Run(input, LanguageVersion.CSharp11, metadata, includeRuntime: false);

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void CompilesBooleanUnionWithNonScalarBranch()
    {
        // Arrange
        var input =
            """
            module BooleanUnion {
                union Choice switch(boolean) {
                    case TRUE: long enabled;
                    case FALSE: sequence<long, 2> disabled;
                };
            };
            """;
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ShouldBeEmpty();
    }

    [Fact]
    public void ReportsMissingRuntimeReferenceBeforeParsingIdl()
    {
        // Arrange
        var input = """module Sample { struct Value { long value; }; };""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: false);

        // Assert
        result.Diagnostics.ShouldContain(d => d.Id == "DDSG0003");
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Theory]
    [InlineData("7.3.1.0", "byte", false, true)]
    [InlineData("7.6.1.0", "byte", false, true)]
    [InlineData("7.7.0.0", "Octet", true, false)]
    [InlineData("7.7.0.1", "Octet", true, false)]
    [InlineData("8.0.0.0", "Octet", true, false)]
    public void SelectsTheNearestRtiApiProfileFromTheResolvedAssemblyVersion(string version, string octetDynamicType, bool includesAnnotations, bool hasLegacyComplianceMask)
    {
        // Arrange
        const string input = "module Sample { enum Color { Red, Green }; struct Value { octet count; long value; }; };";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: false, runtimeVersion: version);

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        var plugin = result.Output.SyntaxTrees
            .Select(tree => tree.GetText().ToString())
            .Single(source => source.Contains("class ValuePlugin", StringComparison.Ordinal));
        plugin.ShouldContain($"dtf.GetPrimitiveType<{octetDynamicType}>()");
        plugin.Contains("AnnotationParameterValue", StringComparison.Ordinal).ShouldBe(includesAnnotations);
        plugin.Contains("xTypesComplianceMask = 0x0000068C;", StringComparison.Ordinal).ShouldBe(hasLegacyComplianceMask);
    }

    [Theory]
    [InlineData("7.3.1.0", "7.3.1", "4.3.1")]
    [InlineData("7.6.1.0", "7.3.1", "4.3.1")]
    [InlineData("7.7.0.0", "7.7.0", "4.7.0")]
    [InlineData("7.7.0.1", "7.7.0", "4.7.0")]
    [InlineData("8.0.0.0", "7.7.0", "4.7.0")]
    public void NamesEmissionProfilesByRtiddsgenVersion(string runtimeVersion, string expectedRtiVersion, string expectedRtiddsgenVersion)
    {
        // Arrange
        var version = Version.Parse(runtimeVersion);

        // Act
        var profile = IdlCompiler.GetProfileForRuntimeVersion(version);

        // Assert
        profile.ShouldBe(expectedRtiVersion == "7.3.1" ? IdlCompilerProfile.Rti731 : IdlCompilerProfile.Rti770);
        (profile == IdlCompilerProfile.Rti731 ? "4.3.1" : "4.7.0").ShouldBe(expectedRtiddsgenVersion);
    }

    [Fact]
    public void RejectsResolvedRuntimeVersionsBelowTheMinimum()
    {
        // Arrange
        const string input = "module Sample { struct Value { long value; }; };";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: false, runtimeVersion: "7.3.0.0");

        // Assert
        result.Diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0003");
        result.Output.SyntaxTrees.Any(tree => tree.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void ReportsProfileDiagnosticAndEmitsNoSourcesForMustUnderstandOnRtiddsgen431()
    {
        // Arrange
        const string input = "struct Sample { @must_understand long value; };";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: false, runtimeVersion: "7.3.1.0");

        // Assert
        result.Diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0004" && diagnostic.Severity == DiagnosticSeverity.Error);
        result.Output.SyntaxTrees.Any(tree => tree.GetText().ToString().Contains("class Sample", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void RegeneratesOutputWhenTheResolvedRtiReferenceChanges()
    {
        // Arrange
        const string input = "module Sample { struct Value { octet count; long value; }; };";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };
        var parseOptions = new CSharpParseOptions(LanguageVersion.CSharp12);
        var source = new TestAdditionalText("sample.idl", input);
        var runtime731 = CreateRuntimeReference("7.3.1.0");
        var runtime770 = CreateRuntimeReference("7.7.0.0");
        var baseReferences = FrameworkReferences().ToArray();
        var compilation731 = CSharpCompilation.Create(
            "GeneratorInput",
            [CSharpSyntaxTree.ParseText("namespace Input; public sealed class Marker { }", parseOptions, cancellationToken: TestContext.Current.CancellationToken)],
            [.. baseReferences, runtime731],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            [new Generator().AsSourceGenerator()],
            [source],
            parseOptions,
            new TestAnalyzerConfigOptionsProvider(metadata));

        // Act
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation731, out var output731, out _, TestContext.Current.CancellationToken);
        var compilation770 = CSharpCompilation.Create(
            "GeneratorInput",
            [CSharpSyntaxTree.ParseText("namespace Input; public sealed class Marker { }", parseOptions, cancellationToken: TestContext.Current.CancellationToken)],
            [.. baseReferences, runtime770],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        driver.RunGeneratorsAndUpdateCompilation(compilation770, out var output770, out _, TestContext.Current.CancellationToken);

        // Assert
        var source731 = output731.SyntaxTrees.Select(tree => tree.GetText().ToString())
            .Single(text => text.Contains("class ValuePlugin", StringComparison.Ordinal));
        var source770 = output770.SyntaxTrees.Select(tree => tree.GetText().ToString())
            .Single(text => text.Contains("class ValuePlugin", StringComparison.Ordinal));
        source731.ShouldContain("GetPrimitiveType<byte>()");
        source731.ShouldNotContain("AnnotationParameterValue");
        source770.ShouldContain("GetPrimitiveType<Octet>()");
        source770.ShouldContain("AnnotationParameterValue");
    }

    [Theory]
    [InlineData("7.3.1.0", "DefaultDiscriminator = 5;")]
    [InlineData("7.7.0.0", "DefaultDiscriminator = 0;")]
    public void UsesTheSelectedProfilesUnionConstructorDiscriminatorPolicy(string version, string expected)
    {
        // Arrange
        const string input = "module Sample { union Choice switch(long) { case 5: long high; case -2: long low; }; };";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: false, runtimeVersion: version);

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        var union = result.Output.SyntaxTrees
            .Select(tree => tree.GetText().ToString())
            .Single(source => source.Contains("DefaultDiscriminator =", StringComparison.Ordinal));
        union.ShouldContain(expected);
    }

    [Fact]
    public void ReportsUnsupportedLanguageVersion()
    {
        // Arrange
        var input = """module Sample { struct Value { long value; }; };""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp11, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldContain(d => d.Id == "DDSG0002");
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void ReportsInvalidIdlWithTheAdditionalFileLocation()
    {
        // Arrange
        var input = """module Sample { struct Value { long value; }""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0001");
        diagnostic.GetMessage().ShouldNotBeNullOrWhiteSpace();
        var lineSpan = diagnostic.Location.GetLineSpan();
        lineSpan.Path.ShouldBe("sample.idl");
        lineSpan.StartLinePosition.Line.ShouldBe(0);
    }

    [Fact]
    public void DoesNotEmitInvalidCSharpForMalformedUnionBranches()
    {
        // Arrange
        var input = "module Sample { enum State { Idle, Running, Stopped }; union Choice switch(State) { case Idle: lo'g value; default: string<16> text; }; };";
        var metadata = new Dictionary<string, string>
        {
            ["Generate"] = "true",
            ["Strict"] = "true",
            ["Defines"] = "FEATURE_A; FEATURE_B"
        };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldContain(d => d.Id == "DDSG0001");
        result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ShouldBeEmpty();
    }

    [Fact]
    public void DoesNotEmitInvalidCSharpForDuplicateUnionDiscriminatorLabels()
    {
        // Arrange
        var input = "module Sample { union Choice switch(long) { case 0: long value; case 0: string<16> text; default: boolean flag; }; };";
        var metadata = new Dictionary<string, string>
        {
            ["Generate"] = "true",
            ["Strict"] = "true"
        };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldContain(d => d.Id == "DDSG0001");
        result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ShouldBeEmpty();
    }

    [Fact]
    public void DoesNotEmitInvalidCSharpForStrictBoundedStringSequences()
    {
        // Arrange
        var input = "module Sample { struct Value { sequence<string<16>, 4> values; }; };";
        var metadata = new Dictionary<string, string>
        {
            ["Generate"] = "true",
            ["Strict"] = "true"
        };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ShouldBeEmpty();
    }

    [Fact]
    public void DoesNotEmitInvalidCSharpForDirectlyRecursiveStructMembers()
    {
        // Arrange
        var input = "module Sample { typedef sequence<long, 4> Values; struct Value { Value values; }; };";
        var metadata = new Dictionary<string, string>
        {
            ["Generate"] = "true",
            ["Strict"] = "true"
        };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldContain(d => d.Id == "DDSG0001");
        result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ShouldBeEmpty();
    }

    [Fact]
    [Trait("Preprocessor", "PP020")]
    public void DoesNotEmitInvalidCSharpForMacroExpandedAdjacentUnarySigns()
    {
        // Arrange
        var input = "#define ADD(a, b) + + b\nconst long Constant = ADD(1, 2);";
        var metadata = new Dictionary<string, string>
        {
            ["Generate"] = "true",
            ["Strict"] = "true"
        };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ShouldBeEmpty();
    }

    [Fact]
    public void ReportsUnknownAnnotationWarningAndContinuesGeneration()
    {
        // Arrange
        var input = """
        module Sample {
            @custom_unknown
            struct Value { long value; };
        };
        """;
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0101");
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
        diagnostic.GetMessage().ShouldBe("Annotation '@custom_unknown' is not recognized and will be ignored.");
        var lineSpan = diagnostic.Location.GetLineSpan();
        lineSpan.Path.ShouldBe("sample.idl");
        lineSpan.StartLinePosition.Line.ShouldBe(1);
        lineSpan.StartLinePosition.Character.ShouldBe(4);
        diagnostic.Location.SourceSpan.Start.ShouldBe(input.IndexOf("@custom_unknown", StringComparison.Ordinal));

        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeTrue();
    }

    [Fact]
    public void ReportsUnsupportedAnnotationWarningBeforeContextualError()
    {
        // Arrange
        var input = """module Sample { @position(1) struct Value { long value; }; };""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0102");
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
        diagnostic.GetMessage().ShouldBe("Annotation 'position' is recognized but unsupported and will be ignored.");
        diagnostic.Location.GetLineSpan().Path.ShouldBe("sample.idl");

        result.Diagnostics.ShouldContain(d => d.Id == "DDSG0001");
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void ReportsAndIgnoresNonDdsInterface()
    {
        // Arrange
        var input = """module Sample { interface Service { void ping(); }; };""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0103");
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
        diagnostic.GetMessage().ShouldBe("The interface 'Service' is ignored because it is not a DDS service.");
        diagnostic.Location.GetLineSpan().Path.ShouldBe("sample.idl");

        result.Diagnostics.ShouldNotContain(d => d.Id == "DDSG0001");
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Service", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void ReportsAndIgnoresDdsServiceInterface()
    {
        // Arrange
        var input = """module Sample { @service("DDS") interface Service { void ping(); }; };""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0103");
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
        diagnostic.GetMessage().ShouldBe("The DDS service interface 'Service' is ignored because service interfaces are not emitted for C#.");
        diagnostic.Location.GetLineSpan().Path.ShouldBe("sample.idl");
        diagnostic.Location.SourceSpan.Start.ShouldBe(input.IndexOf("@service", StringComparison.Ordinal));

        result.Diagnostics.ShouldNotContain(d => d.Id == "DDSG0001");
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Service", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void ReportsUnsupportedDdsServiceAnnotationOnNonInterface()
    {
        // Arrange
        var input = """module Sample { @service("DDS") struct Value { long value; }; };""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0102");
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
        diagnostic.GetMessage().ShouldBe("Annotation 'service' is recognized but unsupported and will be ignored.");
        diagnostic.Location.GetLineSpan().Path.ShouldBe("sample.idl");
        diagnostic.Location.SourceSpan.Start.ShouldBe(input.IndexOf("@service", StringComparison.Ordinal));

        result.Diagnostics.ShouldContain(d => d.Id == "DDSG0001");
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void RejectsNonDdsServiceAnnotation()
    {
        // Arrange
        var input = """module Sample { @service("Other") interface Service { void ping(); }; };""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0102");
        diagnostic.GetMessage().ShouldBe("Annotation 'service' is recognized but unsupported and will be ignored.");
        diagnostic.Location.SourceSpan.Start.ShouldBe(input.IndexOf("@service", StringComparison.Ordinal));
        result.Diagnostics.ShouldContain(d => d.Id == "DDSG0001");
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Service", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Theory]
    [InlineData("@service(\"DDS\") @custom")]
    [InlineData("@custom @service(\"DDS\")")]
    public void ReportsAndIgnoresDdsServiceInterfaceWithUnknownAnnotation(string annotations)
    {
        // Arrange
        var input = $"module Sample {{ {annotations} interface Service {{ void ping(); }}; }};";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0103");
        diagnostic.GetMessage().ShouldBe("The DDS service interface 'Service' is ignored because service interfaces are not emitted for C#.");
        diagnostic.Location.SourceSpan.Start.ShouldBe(input.IndexOf("@service", StringComparison.Ordinal));

        var annotationWarning = result.Diagnostics.Single(d => d.Id == "DDSG0101");
        annotationWarning.GetMessage().ShouldBe("Annotation '@custom' is not recognized and will be ignored.");
        result.Diagnostics.ShouldNotContain(d => d.Id == "DDSG0001");
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Service", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void ReportsMacroArityWarningAndContinuesExpansion()
    {
        // Arrange
        var input = """
        #define CORPUS_PAIR(a, b) a
        const long Constant = CORPUS_PAIR(1);
        """;
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0104");
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
        diagnostic.GetMessage().ShouldBe("Function-like macro 'CORPUS_PAIR' was invoked with the wrong number of arguments; expansion will continue.");
        diagnostic.Location.GetLineSpan().Path.ShouldBe("sample.idl");

        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("public const int Value = 1;", StringComparison.Ordinal)).ShouldBeTrue();
    }

    [Fact]
    [Trait("Preprocessor", "PP040")]
    public void ReportsPreprocessorWarningThroughTheRoslynAdapter()
    {
        // Arrange
        const string input = """
        #warning prefer the supported IDL form
        const long Value = 1;
        """;
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0106");
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
        diagnostic.GetMessage().ShouldBe("Preprocessor warning: prefer the supported IDL form");
        diagnostic.Location.GetLineSpan().Path.ShouldBe("sample.idl");
    }

    [Fact]
    [Trait("Preprocessor", "PP042")]
    public void ReportsPreprocessorMessageThroughTheRoslynAdapter()
    {
        // Arrange
        const string input = """
        #pragma message("build note")
        const long Value = 1;
        """;
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.Single(d => d.Id == "DDSG0107");
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Info);
        diagnostic.GetMessage().ShouldBe("Preprocessor message: message(\"build note\")");
        diagnostic.Location.GetLineSpan().Path.ShouldBe("sample.idl");
    }

    [Fact]
    public void ReportsDirectArrayOfSequencesWarning()
    {
        // Arrange
        var input = """module Sample { struct Value { sequence<long> values[2]; sequence<string<16>, 3> names[2]; }; };""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "true" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.Count(d => d.Id == "DDSG0105").ShouldBe(2);
        result.Diagnostics.ShouldAllBe(d => d.Id != "DDSG0001");
        result.Diagnostics.Where(d => d.Id == "DDSG0105")
            .ShouldAllBe(d => d.Severity == DiagnosticSeverity.Warning && d.Location.GetLineSpan().Path == "sample.idl");
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeTrue();
    }

    /// <summary>Applies project defaults and per-file overrides without changing ordinary sequences.</summary>
    [Theory]
    [InlineData("Idl", "", true)]
    [InlineData("Idl", "RtiCSharp", false)]
    [InlineData("RtiCSharp", "Idl", true)]
    [InlineData("", " idl ", true)]
    [InlineData("", "", false)]
    public void SelectsSequenceArrayMapping(string projectValue, string fileValue, bool preservesArray)
    {
        // Arrange
        const string source = """
            struct Sample { sequence<long> values[2]; sequence<long> ordinary; };
            """;
        var metadata = new Dictionary<string, string>
        {
            ["Generate"] = "true",
            ["build_property.DdsIdlSequenceArrayMapping"] = projectValue,
            ["SequenceArrayMapping"] = fileValue
        };

        // Act
        var result = Run(source, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Output.GetDiagnostics(TestContext.Current.CancellationToken).Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
        var generated = string.Join(Environment.NewLine, result.Output.SyntaxTrees.Select(tree => tree.ToString()));
        generated.ShouldContain("ISequence<int> ordinary");
        if (preservesArray)
        {
            result.Diagnostics.ShouldBeEmpty();
            generated.ShouldContain("Sample_valuesSequence[] values");
        }
        else
        {
            result.Diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0105");
            generated.ShouldContain("ISequence<int> values");
        }
    }

    /// <summary>Applies escaped-identifier normalization from the project property.</summary>
    [Fact]
    public void EnablesEscapeCharFromProjectProperty()
    {
        // Arrange
        const string source = "module _Escaped { struct _Sample { long _event; }; };";
        var metadata = new Dictionary<string, string>
        {
            ["Generate"] = "true",
            ["build_property.DdsIdlEnableEscapeChar"] = "true"
        };

        // Act
        var result = Run(source, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        var generated = string.Join(Environment.NewLine, result.Output.SyntaxTrees.Select(tree => tree.ToString()));
        generated.ShouldContain("namespace Escaped");
        generated.ShouldContain("class Sample");
        generated.ShouldContain("public int @event");
        generated.ShouldContain("new StructMember(\"event\"");
    }

    /// <summary>Ignores item metadata for this project-wide setting.</summary>
    [Fact]
    public void DoesNotAllowPerFileEscapeCharOverride()
    {
        // Arrange
        const string source = "module _Escaped { struct _Sample { long _event; }; };";
        var metadata = new Dictionary<string, string>
        {
            ["Generate"] = "true",
            ["EnableEscapeChar"] = "true"
        };

        // Act
        var result = Run(source, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        var generated = string.Join(Environment.NewLine, result.Output.SyntaxTrees.Select(tree => tree.ToString()));
        generated.ShouldContain("namespace _Escaped");
        generated.ShouldContain("class _Sample");
        generated.ShouldContain("public int _event");
    }

    /// <summary>Rejects project escape-character values that are not boolean.</summary>
    [Fact]
    public void RejectsInvalidEscapeCharProjectProperty()
    {
        // Arrange
        var metadata = new Dictionary<string, string>
        {
            ["Generate"] = "true",
            ["build_property.DdsIdlEnableEscapeChar"] = "enabled"
        };

        // Act
        var result = Run("struct _Sample { long value; };", LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0001"
            && diagnostic.GetMessage().Contains("DdsIdlEnableEscapeChar", StringComparison.Ordinal)
            && diagnostic.Location.GetLineSpan().Path == "sample.idl");
        string.Join(Environment.NewLine, result.Output.SyntaxTrees.Select(tree => tree.ToString()))
            .ShouldNotContain("class _Sample");
    }

    /// <summary>Uses project effective limits unless the declaring file overrides them.</summary>
    [Fact]
    public void AppliesProjectAndFileUnboundedSizeSettings()
    {
        // Arrange
        const string source =
            """
            module Sizes {
                typedef sequence<long> Numbers;
                typedef string Text;
                struct Sample {
                    sequence<long> values;
                    string narrow;
                    wstring wide;
                    sequence<string> names;
                    sequence<long, 2> explicitlyBounded;
                    string<4> explicitlyBoundedText;
                };
            };
            """;
        var projectSettings = new Dictionary<string, string>
        {
            ["Generate"] = "true",
            ["build_property.DdsIdlSequenceSize"] = "7",
            ["build_property.DdsIdlStringSize"] = "19"
        };
        var fileSettings = new Dictionary<string, string>(projectSettings)
        {
            ["SequenceSize"] = "3",
            ["StringSize"] = "5"
        };

        // Act
        var projectResult = Run(source, LanguageVersion.CSharp12, projectSettings, includeRuntime: true);
        var fileResult = Run(source, LanguageVersion.CSharp12, fileSettings, includeRuntime: true);

        // Assert
        projectResult.Diagnostics.ShouldBeEmpty();
        fileResult.Diagnostics.ShouldBeEmpty();

        var projectGenerated = string.Join(Environment.NewLine, projectResult.Output.SyntaxTrees.Select(tree => tree.ToString()));
        projectGenerated.ShouldContain("effective limit of 7 elements");
        projectGenerated.ShouldContain("effective limit of 19 UTF-8 bytes");
        projectGenerated.ShouldContain("effective limit of 19 UTF-16 code units");
        projectGenerated.ShouldContain("CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), 7)");
        projectGenerated.ShouldContain("CreateString(19)");
        projectGenerated.ShouldContain("CreateWideString(19)");
        projectGenerated.ShouldContain("[Bound(2)]");
        projectGenerated.ShouldContain("[Bound(4)]");

        var fileGenerated = string.Join(Environment.NewLine, fileResult.Output.SyntaxTrees.Select(tree => tree.ToString()));
        fileGenerated.ShouldContain("effective limit of 3 elements");
        fileGenerated.ShouldContain("effective limit of 5 UTF-8 bytes");
        fileGenerated.ShouldContain("effective limit of 5 UTF-16 code units");
        fileGenerated.ShouldContain("CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), 3)");
        fileGenerated.ShouldContain("CreateString(5)");
        fileGenerated.ShouldContain("CreateWideString(5)");
    }

    /// <summary>Accepts the largest positive Int32 size representable by the generated APIs.</summary>
    [Fact]
    public void AcceptsInt32MaximumAsFiniteSize()
    {
        // Arrange
        var metadata = new Dictionary<string, string>
        {
            ["Generate"] = "true",
            ["build_property.DdsIdlSequenceSize"] = int.MaxValue.ToString(System.Globalization.CultureInfo.InvariantCulture),
            ["build_property.DdsIdlStringSize"] = int.MaxValue.ToString(System.Globalization.CultureInfo.InvariantCulture)
        };

        // Act
        var result = Run("struct Sample { sequence<long> values; string text; };", LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldBeEmpty();

        var generated = string.Join(Environment.NewLine, result.Output.SyntaxTrees.Select(tree => tree.ToString()));
        generated.ShouldContain($"effective limit of {int.MaxValue} elements");
        generated.ShouldContain($"effective limit of {int.MaxValue} UTF-8 bytes");
    }

    /// <summary>Rejects invalid project and file-level effective size settings.</summary>
    [Theory]
    [InlineData("build_property.DdsIdlSequenceSize", "0", "SequenceSize")]
    [InlineData("build_property.DdsIdlStringSize", "2147483648", "StringSize")]
    [InlineData("SequenceSize", "not-a-number", "SequenceSize")]
    [InlineData("StringSize", "-1", "StringSize")]
    public void RejectsInvalidUnboundedSizeSettings(string setting, string value, string expectedName)
    {
        // Arrange
        var metadata = new Dictionary<string, string>
        {
            ["Generate"] = "true",
            [setting] = value
        };

        // Act
        var result = Run("struct Sample { sequence<long> values; };", LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0001"
            && diagnostic.GetMessage().Contains(expectedName, StringComparison.Ordinal)
            && diagnostic.Location.GetLineSpan().Path == "sample.idl");
    }

    /// <summary>Reports invalid build settings at the input and emits no sources.</summary>
    [Theory]
    [InlineData("SequenceArrayMapping")]
    [InlineData("build_property.DdsIdlSequenceArrayMapping")]
    public void RejectsInvalidSequenceArrayMapping(string setting)
    {
        // Arrange
        var metadata = new Dictionary<string, string> { ["Generate"] = "true", [setting] = "unknown" };

        // Act
        var result = Run("struct Sample { long value; };", LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0001"
            && diagnostic.GetMessage().Contains("SequenceArrayMapping", StringComparison.Ordinal)
            && diagnostic.Location.GetLineSpan().Path == "sample.idl");
        result.Output.SyntaxTrees.Count().ShouldBe(1);
    }

    [Fact]
    public void HonorsGenerateFalseAdditionalFileMetadata()
    {
        // Arrange
        var input = """module Sample { struct Value { long value; }; };""";
        var metadata = new Dictionary<string, string> { ["Generate"] = "false" };

        // Act
        var result = Run(input, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldBeEmpty();
        result.Output.SyntaxTrees.Any(t => t.GetText().ToString().Contains("class Value", StringComparison.Ordinal)).ShouldBeFalse();
    }

    [Fact]
    public void GeneratedXmlDocumentationHasNoMalformedOrKeywordParameterWarnings()
    {
        // Arrange
        const string input = "module Keywords { struct Sample { long event; long base; }; union Choice switch(long) { case 1: case 5: long number; default: string text; }; };";

        // Act
        var result = Run(
            input,
            LanguageVersion.CSharp12,
            new Dictionary<string, string> { ["Generate"] = "true" },
            includeRuntime: true,
            documentationMode: DocumentationMode.Diagnose,
            source: "using Keywords; namespace Input; public sealed class Marker { public void Run() { var choice = new Choice(); choice.number = default!; choice.Setnumber(default!, 1); } }");
        var documentationWarnings = result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(diagnostic => diagnostic.Id is "CS1570" or "CS1572" or "CS1573" or "CS1584" or "CS0419")
            .ToArray();

        // Assert
        documentationWarnings.ShouldBeEmpty();
    }

    [Fact]
    public void GeneratedUsageExamplesCompileAgainstTheirGeneratedTypes()
    {
        // Arrange
        const string input = "module Examples { struct Sample { sequence<long, 4> values; @optional string title; }; union Choice switch(long) { case 1: case 5: long number; default: string text; }; };";
        const string source = "using Examples; namespace Input; public sealed class Marker { public void Run() { var sample = new Sample(); sample.values.Add(default!); sample.values.RemoveAt(sample.values.Count - 1); sample.title = null; var original = new Sample(); var copy = new Sample(original); var text = SampleSupport.Instance.ToString(copy); var dynamicType = SampleSupport.Instance.DynamicType; var serializer = SampleSupport.Instance.CreateSerializer(); var choice = new Choice(); choice.number = 42; var activeValue = choice.number; choice.Setnumber(42, 5); activeValue = choice.number; } }";

        // Act
        var result = Run(
            input,
            LanguageVersion.CSharp12,
            new Dictionary<string, string> { ["Generate"] = "true" },
            includeRuntime: true,
            documentationMode: DocumentationMode.Diagnose,
            source: source);
        var errors = result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ToArray();
        var documentationWarnings = result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(diagnostic => diagnostic.Id is "CS1570" or "CS1572" or "CS1573" or "CS1584" or "CS0419")
            .ToArray();

        // Assert
        errors.ShouldBeEmpty();
        documentationWarnings.ShouldBeEmpty();
    }

    [Fact]
    public void UnsignedLongUnionLabelsCompileAndRoundTripThroughRtiSerializer()
    {
        // Arrange
        const string input = "module UnsignedLabels { union Choice switch(unsigned long) { case 2147483648: long aboveSignedMax; case 4294967295: long unsignedMax; default: long fallback; }; };";
        var result = Run(
            input,
            LanguageVersion.CSharp12,
            new Dictionary<string, string> { ["Generate"] = "true" },
            includeRuntime: true);
        var errors = result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ToArray();

        // Act
        using var generatedAssemblyStream = new MemoryStream();
        result.Output.Emit(generatedAssemblyStream, cancellationToken: TestContext.Current.CancellationToken).Success.ShouldBeTrue();
        generatedAssemblyStream.Position = 0;
        var generatedAssembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(generatedAssemblyStream, null);
        var choiceType = generatedAssembly.GetType("UnsignedLabels.Choice", throwOnError: true)!;
        var supportType = generatedAssembly.GetType("UnsignedLabels.ChoiceSupport", throwOnError: true)!;
        var support = supportType.GetProperty("Instance")!.GetValue(null)!;
        var createSerializer = supportType.GetMethod("CreateSerializer")!;
        var serializerArguments = createSerializer.GetParameters()
            .Select(parameter => parameter.DefaultValue)
            .ToArray();
        var serializer = createSerializer.Invoke(support, serializerArguments)!;

        try
        {
            var sample = Activator.CreateInstance(choiceType)!;
            choiceType.GetProperty("aboveSignedMax")!.SetValue(sample, 121);
            var serializerInterface = serializer.GetType().GetInterfaces()
                .Single(type => type.IsGenericType && type.GetGenericTypeDefinition().Name == "ISerializer`1");
            var serialize = serializerInterface.GetMethod("Serialize", [choiceType])!;
            var deserialize = serializerInterface.GetMethod("Deserialize", [typeof(byte[])])!;
            var bytes = (byte[])serialize.Invoke(serializer, [sample])!;
            var roundTripped = deserialize.Invoke(serializer, [bytes])!;

            // Assert
            errors.ShouldBeEmpty();
            choiceType.GetProperty("Discriminator")!.GetValue(roundTripped).ShouldBe(2147483648U);
            choiceType.GetProperty("aboveSignedMax")!.GetValue(roundTripped).ShouldBe(121);

            choiceType.GetProperty("unsignedMax")!.SetValue(sample, 122);
            bytes = (byte[])serialize.Invoke(serializer, [sample])!;
            roundTripped = deserialize.Invoke(serializer, [bytes])!;
            choiceType.GetProperty("Discriminator")!.GetValue(roundTripped).ShouldBe(uint.MaxValue);
            choiceType.GetProperty("unsignedMax")!.GetValue(roundTripped).ShouldBe(122);
        }
        finally
        {
            ((IDisposable)serializer).Dispose();
        }
    }

    [Theory]
    [InlineData("boolean", "FALSE", "False")]
    [InlineData("boolean", "TRUE", "True")]
    [InlineData("char", "'\\0'", "\0")]
    [InlineData("char", "'a'", "a")]
    [InlineData("char", "'\u007f'", "\u007f")]
    [InlineData("char", "'\u0080'", "\u0080")]
    [InlineData("char", "'\u00fe'", "\u00fe")]
    [InlineData("char", "'ÿ'", "ÿ")]
    [InlineData("wchar", "L'a'", "a")]
    [InlineData("Kind", "Minimum", "Minimum")]
    [InlineData("Kind", "Middle", "Middle")]
    [InlineData("Kind", "Maximum", "Maximum")]
    [InlineData("octet", "255", "255")]
    [InlineData("octet", "0", "0")]
    [InlineData("short", "0", "0")]
    [InlineData("short", "-32768", "-32768")]
    [InlineData("short", "32767", "32767")]
    [InlineData("unsigned short", "0", "0")]
    [InlineData("unsigned short", "65535", "65535")]
    [InlineData("long", "-2147483648", "-2147483648")]
    [InlineData("long", "0", "0")]
    [InlineData("long", "2147483647", "2147483647")]
    [InlineData("unsigned long", "0", "0")]
    [InlineData("unsigned long", "2147483648", "2147483648")]
    [InlineData("unsigned long", "4294967295", "4294967295")]
    [InlineData("int8", "0", "0")]
    [InlineData("int8", "-128", "-128")]
    [InlineData("int8", "127", "127")]
    [InlineData("uint8", "0", "0")]
    [InlineData("uint8", "255", "255")]
    [InlineData("int16", "-32768", "-32768")]
    [InlineData("int16", "0", "0")]
    [InlineData("int16", "32767", "32767")]
    [InlineData("uint16", "0", "0")]
    [InlineData("uint16", "65535", "65535")]
    [InlineData("int32", "-2147483648", "-2147483648")]
    [InlineData("int32", "0", "0")]
    [InlineData("int32", "2147483647", "2147483647")]
    [InlineData("uint32", "0", "0")]
    [InlineData("uint32", "2147483648", "2147483648")]
    [InlineData("uint32", "4294967295", "4294967295")]
    public void RtiAcceptedPrimitiveDiscriminatorsCompileAndExposeRoundTripBehavior(
        string discriminatorType,
        string label,
        string expectedDiscriminator)
    {
        // Arrange
        var assemblyName = $"UnionDiscriminator_{Convert.ToHexString(Encoding.UTF8.GetBytes($"{discriminatorType}_{label}"))}";
        var enumDeclaration = discriminatorType == "Kind" ? "enum Kind { Minimum, Middle, Maximum }; " : string.Empty;
        var isBoolean = discriminatorType == "boolean";
        var input = isBoolean
            ? "module Matrix { union Choice switch(boolean) { case FALSE: long whenFalse; case TRUE: long whenTrue; }; };"
            : $"module Matrix {{ {enumDeclaration}union Choice switch({discriminatorType}) {{ case {label}: long value; default: long fallback; }}; }};";
        var result = Run(
            input,
            LanguageVersion.CSharp12,
            new Dictionary<string, string> { ["Generate"] = "true" },
            includeRuntime: true,
            assemblyName: assemblyName);
        var errors = result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ToArray();
        errors.ShouldBeEmpty();

        // Act
        using var generatedAssemblyStream = new MemoryStream();
        result.Output.Emit(generatedAssemblyStream, cancellationToken: TestContext.Current.CancellationToken).Success.ShouldBeTrue();
        generatedAssemblyStream.Position = 0;
        var generatedAssembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(generatedAssemblyStream, null);
        var choiceType = generatedAssembly.GetType("Matrix.Choice", throwOnError: true)!;
        var supportType = generatedAssembly.GetType("Matrix.ChoiceSupport", throwOnError: true)!;
        var support = supportType.GetProperty("Instance")!.GetValue(null)!;
        var createSerializer = supportType.GetMethod("CreateSerializer")!;
        var serializerArguments = createSerializer.GetParameters()
            .Select(parameter => parameter.DefaultValue)
            .ToArray();
        var serializer = createSerializer.Invoke(support, serializerArguments)!;

        try
        {
            var sample = Activator.CreateInstance(choiceType)!;
            var valueMember = isBoolean ? (label == "FALSE" ? "whenFalse" : "whenTrue") : "value";
            choiceType.GetProperty(valueMember)!.SetValue(sample, 42);
            var serializerInterface = serializer.GetType().GetInterfaces()
                .Single(type => type.IsGenericType && type.GetGenericTypeDefinition().Name == "ISerializer`1");
            var serialize = serializerInterface.GetMethod("Serialize", [choiceType])!;
            var deserialize = serializerInterface.GetMethod("Deserialize", [typeof(byte[])])!;
            var bytes = (byte[])serialize.Invoke(serializer, [sample])!;
            var roundTripped = deserialize.Invoke(serializer, [bytes])!;

            // Assert
            Convert.ToString(choiceType.GetProperty("Discriminator")!.GetValue(roundTripped), System.Globalization.CultureInfo.InvariantCulture)
                .ShouldBe(expectedDiscriminator);
            choiceType.GetProperty(valueMember)!.GetValue(roundTripped).ShouldBe(42);

            if (discriminatorType == "char" && expectedDiscriminator == "þ")
            {
                var dynamicType = (Rti.Types.Dynamic.UnionType)supportType.GetProperty("DynamicType")!.GetValue(support)!;
                dynamicType.Members.Single(member => member.Name == valueMember).Labels.ShouldBe([-2]);
            }

            if (!isBoolean)
            {
                var defaultSample = Activator.CreateInstance(choiceType)!;
                choiceType.GetProperty("fallback")!.SetValue(defaultSample, 43);
                var defaultDiscriminator = choiceType.GetProperty("Discriminator")!.GetValue(defaultSample);
                bytes = (byte[])serialize.Invoke(serializer, [defaultSample])!;
                roundTripped = deserialize.Invoke(serializer, [bytes])!;
                choiceType.GetProperty("Discriminator")!.GetValue(roundTripped).ShouldBe(defaultDiscriminator);
                choiceType.GetProperty("fallback")!.GetValue(roundTripped).ShouldBe(43);
            }
        }
        finally
        {
            ((IDisposable)serializer).Dispose();
        }
    }

    [Fact]
    public void ArrayTypedefAtNativeDimensionLimitCompilesEveryGeneratedDocument()
    {
        // Arrange
        const string input = "module Arrays { typedef long Grid[2147483647]; };";

        // Act
        var result = Run(
            input,
            LanguageVersion.CSharp12,
            new Dictionary<string, string> { ["Generate"] = "true" },
            includeRuntime: true,
            documentationMode: DocumentationMode.Diagnose);
        var errors = result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ToArray();

        // Assert
        errors.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void IdentifierDiagnosticsRespectStrictModeBeforeAddingSources(bool strict)
    {
        // Arrange
        const string source = "struct Struct { long number; };";
        var metadata = new Dictionary<string, string>
        {
            ["Generate"] = "true",
            ["Strict"] = strict.ToString()
        };

        // Act
        var result = Run(source, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Location.SourceSpan.Start.ShouldBe(source.IndexOf("Struct", StringComparison.Ordinal));
        if (strict)
        {
            diagnostic.Id.ShouldBe("DDSG0001");
            diagnostic.Severity.ShouldBe(DiagnosticSeverity.Error);
            result.Output.SyntaxTrees.Count().ShouldBe(1);
        }
        else
        {
            diagnostic.Id.ShouldBe("DDSG0108");
            diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
            result.Output.SyntaxTrees.Count().ShouldBeGreaterThan(1);
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void InterfaceBodyValidationUsesSourceLocationsBeforeEmission(bool strict)
    {
        // Arrange
        const string source = "@service(\"DDS\") interface I { void Context(); }; struct Sample { long number; };";
        var metadata = new Dictionary<string, string>
        {
            ["Generate"] = "true",
            ["Strict"] = strict.ToString()
        };

        // Act
        var result = Run(source, LanguageVersion.CSharp12, metadata, includeRuntime: true);

        // Assert
        result.Diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0103");
        var naming = result.Diagnostics.Single(diagnostic => diagnostic.Id != "DDSG0103");
        naming.Location.SourceSpan.Start.ShouldBe(source.IndexOf("Context", StringComparison.Ordinal));
        if (strict)
        {
            naming.Id.ShouldBe("DDSG0001");
            naming.Severity.ShouldBe(DiagnosticSeverity.Error);
            result.Output.SyntaxTrees.Count().ShouldBe(1);
        }
        else
        {
            naming.Id.ShouldBe("DDSG0108");
            naming.Severity.ShouldBe(DiagnosticSeverity.Warning);
            result.Output.SyntaxTrees.Count().ShouldBeGreaterThan(1);
        }
    }

    [Fact]
    public void DefaultAggregateArrayUnionBranchInitializesAndCopiesElementsIndependently()
    {
        // Arrange
        const string input =
            """
            module DefaultAggregateArray {
                struct Item { long value; };
                union Choice switch(long) {
                    case 1: long number;
                    default: Item items[2];
                };
            };
            """;
        var result = Run(
            input,
            LanguageVersion.CSharp12,
            new Dictionary<string, string> { ["Generate"] = "true" },
            includeRuntime: true,
            assemblyName: "DefaultAggregateArrayUnionBranch");
        var errors = result.Output.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ToArray();
        errors.ShouldBeEmpty();

        // Act
        using var generatedAssemblyStream = new MemoryStream();
        var emitResult = result.Output.Emit(generatedAssemblyStream, cancellationToken: TestContext.Current.CancellationToken);
        emitResult.Success.ShouldBeTrue(string.Join(Environment.NewLine, emitResult.Diagnostics));
        generatedAssemblyStream.Position = 0;
        var generatedAssembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(generatedAssemblyStream, null);
        var choiceType = generatedAssembly.GetType("DefaultAggregateArray.Choice", throwOnError: true)!;
        var itemType = generatedAssembly.GetType("DefaultAggregateArray.Item", throwOnError: true)!;
        var valueProperty = itemType.GetProperty("value")!;
        var itemsProperty = choiceType.GetProperty("items")!;
        var choice = Activator.CreateInstance(choiceType)!;
        var items = (Array)itemsProperty.GetValue(choice)!;
        items.Length.ShouldBe(2);
        var originalItem = items.GetValue(0)!;
        originalItem.ShouldNotBeNull();
        valueProperty.SetValue(originalItem, 17);

        var copy = Activator.CreateInstance(choiceType, choice)!;
        var copiedItems = (Array)itemsProperty.GetValue(copy)!;
        var copiedItem = copiedItems.GetValue(0)!;

        // Assert
        copiedItems.ShouldNotBeSameAs(items);
        copiedItem.ShouldNotBeSameAs(originalItem);
        valueProperty.GetValue(copiedItem).ShouldBe(17);

        valueProperty.SetValue(originalItem, 23);
        valueProperty.GetValue(copiedItem).ShouldBe(17);
    }

    private static (IdlCompilationResult Compiler, GeneratorRunResult Adapter) CompileThroughBothBoundaries(
        string source,
        IdlCompilerProfile profile,
        string runtimeVersion)
    {
        var compilerResult = IdlCompiler.Compile(
            [new IdlInput("sample.idl", source)],
            profile,
            TestContext.Current.CancellationToken);
        var adapterResult = Run(
            source,
            LanguageVersion.CSharp12,
            new Dictionary<string, string> { ["Generate"] = "true" },
            includeRuntime: false,
            runtimeVersion: runtimeVersion);

        return (compilerResult, adapterResult);
    }

    private static void AssertEquivalentResults(IdlCompilationResult compilerResult, GeneratorRunResult adapterResult)
    {
        var compilerDocuments = compilerResult.Documents
            .Select(document => (document.HintName, document.Source))
            .ToArray();
        var adapterDocuments = adapterResult.GeneratedSources
            .Select(document => (document.HintName, document.SourceText.ToString()))
            .ToArray();
        adapterDocuments.ShouldBe(compilerDocuments);

        var compilerDiagnostics = compilerResult.Diagnostics
            .Select(diagnostic => (diagnostic.Id, diagnostic.Severity, diagnostic.Message, diagnostic.Path, diagnostic.Offset))
            .ToArray();
        var adapterDiagnostics = adapterResult.Diagnostics
            .Where(diagnostic => diagnostic.Id.StartsWith("DDSG", StringComparison.Ordinal))
            .Select(diagnostic => (
                diagnostic.Id,
                diagnostic.Severity switch
                {
                    DiagnosticSeverity.Error => IdlDiagnosticSeverity.Error,
                    DiagnosticSeverity.Warning => IdlDiagnosticSeverity.Warning,
                    DiagnosticSeverity.Info => IdlDiagnosticSeverity.Info,
                    _ => throw new ArgumentOutOfRangeException(nameof(diagnostic.Severity), diagnostic.Severity, null)
                },
                diagnostic.GetMessage(),
                diagnostic.Location.GetLineSpan().Path,
                diagnostic.Location.SourceSpan.Start))
            .ToArray();
        adapterDiagnostics.ShouldBe(compilerDiagnostics);
    }

    private static GeneratorRunResult Run(
        string input,
        LanguageVersion languageVersion,
        IReadOnlyDictionary<string, string> metadata,
        bool includeRuntime,
        DocumentationMode documentationMode = DocumentationMode.Parse,
        string? source = null,
        string assemblyName = "GeneratorInput",
        string? runtimeVersion = null)
    {
        var parseOptions = new CSharpParseOptions(languageVersion, documentationMode: documentationMode);
        var compilation = CSharpCompilation.Create(
            assemblyName,
            [CSharpSyntaxTree.ParseText(source ?? "namespace Input; public sealed class Marker { }", parseOptions)],
            runtimeVersion is not null
                ? FrameworkReferences().Append(CreateRuntimeReference(runtimeVersion))
                : includeRuntime ? RuntimeReferences() : FrameworkReferences(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver.Create(
            [new Generator().AsSourceGenerator()],
            [new TestAdditionalText("sample.idl", input)],
            parseOptions,
            new TestAnalyzerConfigOptionsProvider(metadata));

        var updatedDriver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var output, out var generatorDiagnostics);
        var runDiagnostics = driver.GetRunResult().Results
            .SelectMany(result => result.Diagnostics.IsDefault ? [] : result.Diagnostics)
            .ToImmutableArray();
        var generatedSources = updatedDriver.GetRunResult().Results
            .SelectMany(result => result.GeneratedSources.IsDefault ? [] : result.GeneratedSources)
            .ToImmutableArray();

        return new GeneratorRunResult(output,
        [
            .. generatorDiagnostics,
            .. runDiagnostics,
            .. output.GetDiagnostics().Where(d => d.Id.StartsWith("DDSG", StringComparison.Ordinal)),
        ], generatedSources);
    }

    private static IEnumerable<MetadataReference> FrameworkReferences()
    {
        var paths = (string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") ?? string.Empty;

        return paths
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Where(path => !string.Equals(Path.GetFileName(path), "Rti.ConnextDds.dll", StringComparison.OrdinalIgnoreCase))
            .Select(path => MetadataReference.CreateFromFile(path));
    }

    private static IEnumerable<MetadataReference> RuntimeReferences()
    {
        var references = FrameworkReferences().ToList();
        var runtime = Path.Combine(AppContext.BaseDirectory, "Rti.ConnextDds.dll");

        File.Exists(runtime).ShouldBeTrue($"Expected RTI runtime at {runtime}");

        references.Add(MetadataReference.CreateFromFile(runtime));

        return references;
    }

    private static MetadataReference CreateRuntimeReference(string version)
    {
        var assembly = CSharpCompilation.Create(
            "Rti.ConnextDds",
            [CSharpSyntaxTree.ParseText($"[assembly: System.Reflection.AssemblyVersion(\"{version}\")]")],
            FrameworkReferences(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var emitResult = assembly.Emit(stream);
        emitResult.Success.ShouldBeTrue(string.Join(Environment.NewLine, emitResult.Diagnostics));

        return MetadataReference.CreateFromImage(stream.ToArray());
    }

    private sealed record GeneratorRunResult(
        Compilation Output,
        ImmutableArray<Diagnostic> Diagnostics,
        ImmutableArray<GeneratedSourceResult> GeneratedSources);

    private sealed class TestAdditionalText(string path, string text) : AdditionalText
    {
        public override string Path { get; } = path;

        public override SourceText GetText(CancellationToken cancellationToken = default) => SourceText.From(text);
    }

    private sealed class TestAnalyzerConfigOptionsProvider(IReadOnlyDictionary<string, string> values) : AnalyzerConfigOptionsProvider
    {
        private readonly TestAnalyzerConfigOptions options = new(values);

        public override AnalyzerConfigOptions GlobalOptions => options;

        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => options;

        public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) => options;
    }

    private sealed class TestAnalyzerConfigOptions(IReadOnlyDictionary<string, string> values) : AnalyzerConfigOptions
    {
        public override bool TryGetValue(string key, out string value)
        {
            string metadataName;

            if (key.StartsWith("build_metadata.AdditionalFiles.", StringComparison.OrdinalIgnoreCase))
            {
                metadataName = key["build_metadata.AdditionalFiles.".Length..];
            }
            else
            {
                metadataName = key;
            }

            return values.TryGetValue(metadataName, out value!);
        }
    }
}
