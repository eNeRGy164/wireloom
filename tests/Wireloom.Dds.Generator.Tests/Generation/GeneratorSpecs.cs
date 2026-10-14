using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Wireloom.Generation.Tests;

public sealed class GeneratorSpecs
{
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
            ? $"module Matrix {{ union Choice switch(boolean) {{ case FALSE: long whenFalse; case TRUE: long whenTrue; }}; }};"
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

    private static GeneratorRunResult Run(
        string input,
        LanguageVersion languageVersion,
        IReadOnlyDictionary<string, string> metadata,
        bool includeRuntime,
        DocumentationMode documentationMode = DocumentationMode.Parse,
        string? source = null,
        string assemblyName = "GeneratorInput")
    {
        var parseOptions = new CSharpParseOptions(languageVersion, documentationMode: documentationMode);
        var compilation = CSharpCompilation.Create(
            assemblyName,
            [CSharpSyntaxTree.ParseText(source ?? "namespace Input; public sealed class Marker { }", parseOptions)],
            includeRuntime ? RuntimeReferences() : FrameworkReferences(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver.Create(
            [new Generator().AsSourceGenerator()],
            [new TestAdditionalText("sample.idl", input)],
            parseOptions,
            new TestAnalyzerConfigOptionsProvider(metadata));

        driver.RunGeneratorsAndUpdateCompilation(compilation, out var output, out var generatorDiagnostics);
        var runDiagnostics = driver.GetRunResult().Results
            .SelectMany(result => result.Diagnostics.IsDefault ? [] : result.Diagnostics)
            .ToImmutableArray();

        return new GeneratorRunResult(output,
        [
            .. generatorDiagnostics,
            .. runDiagnostics,
            .. output.GetDiagnostics().Where(d => d.Id.StartsWith("DDSG", StringComparison.Ordinal)),
        ]);
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

    private sealed record GeneratorRunResult(Compilation Output, ImmutableArray<Diagnostic> Diagnostics);

    private sealed class TestAdditionalText(string path, string text) : AdditionalText
    {
        public override string Path { get; } = path;

        public override SourceText? GetText(CancellationToken cancellationToken = default) => SourceText.From(text);
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
