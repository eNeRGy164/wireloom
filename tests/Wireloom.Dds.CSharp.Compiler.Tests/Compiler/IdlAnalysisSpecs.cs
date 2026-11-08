using Wireloom.Compiler.FrontEnd;
using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.Tests;

/// <summary>Protects the language-independent model shared by the two generators.</summary>
public sealed class IdlAnalysisSpecs
{
    /// <summary>Retains explicit module, declaration, and member annotations at their source offsets.</summary>
    [Fact]
    public void RetainsAnnotationApplicationsForBackendValidation()
    {
        // Arrange
        var source = """
            @autoid(SEQUENTIAL) module CorpusAnnotations {
                @data_representation(XCDR2) @topic struct Sample {
                    @key long id;
                    @min(0) @default(3) long value;
                };
            };
            """;
        var input = new IdlInput("annotations.idl", source);

        // Act
        var analysis = IdlAnalysis.Analyze([input], CompilerTestSupport.ComputeMd5,
            TestContext.Current.CancellationToken);

        // Assert
        var module = analysis.Declarations.OfType<IdlModuleDeclaration>().ShouldHaveSingleItem();
        var sample = analysis.Declarations.OfType<IdlClassDeclaration>().ShouldHaveSingleItem();
        module.AnnotationApplications.Select(application => application.Name).ShouldBe(["autoid"]);
        sample.AnnotationApplications.Select(application => application.Name).ShouldBe(["data_representation", "topic"]);
        sample.Fields[0].AnnotationApplications.Select(application => application.Name).ShouldBe(["key"]);
        sample.Fields[1].AnnotationApplications.Select(application => application.Name).ShouldBe(["min", "default"]);
        var annotations = module.AnnotationApplications.Concat(sample.AnnotationApplications)
            .Concat(sample.Fields.SelectMany(field => field.AnnotationApplications));
        foreach (var annotation in annotations)
        {
            annotation.Offset.ShouldBe(source.IndexOf($"@{annotation.Name}", StringComparison.Ordinal));
            source.Substring(annotation.Offset, annotation.Length).ShouldBe(annotation.Text);
        }
    }

    /// <summary>Retains declarations that conflict only with C# companion or member names.</summary>
    [Fact]
    public void KeepsIdlNamesIndependentOfTheCSharpGeneratedSurface()
    {
        // Arrange
        var input = new IdlInput("names.idl",
            """
            struct Sample { long Destroy; };
            struct SampleSupport { long value; };
            """);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var analysis = IdlAnalysis.Analyze([input], CompilerTestSupport.ComputeMd5,
            TestContext.Current.CancellationToken, diagnostics);

        // Assert
        analysis.Declarations.OfType<IdlClassDeclaration>().Select(declaration => declaration.Name)
            .ShouldBe(["Sample", "SampleSupport"]);
        diagnostics.ShouldBeEmpty();
        var csharp = IdlCompiler.Compile([input], IdlCompilerProfile.Rti770, CompilerTestSupport.ComputeMd5,
            TestContext.Current.CancellationToken);
        csharp.Success.ShouldBeFalse();
        csharp.Diagnostics.ShouldContain(diagnostic => diagnostic.Message.Contains("generated", StringComparison.Ordinal));
    }

    /// <summary>Preserves both collection layers even when the caller uses the C# compatibility mapping.</summary>
    [Fact]
    public void PreservesDirectSequenceArrayMeaningWithoutCSharpWarnings()
    {
        // Arrange
        var input = new IdlInput("collections.idl", "struct Sample { sequence<long, 4> values[2]; };");
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var analysis = IdlAnalysis.Analyze([input], CompilerTestSupport.ComputeMd5,
            TestContext.Current.CancellationToken, diagnostics);

        // Assert
        var sample = analysis.Declarations.OfType<IdlClassDeclaration>().ShouldHaveSingleItem();
        var sequence = sample.Fields.ShouldHaveSingleItem().Type.ShouldBeOfType<IdlType.Sequence>();
        sequence.Dimensions.ShouldBe([2]);
        sequence.Bound.ShouldBe(4);
        diagnostics.ShouldBeEmpty();
        var csharp = IdlCompiler.Compile([input], IdlCompilerProfile.Rti770, CompilerTestSupport.ComputeMd5,
            TestContext.Current.CancellationToken);
        csharp.Success.ShouldBeTrue();
        csharp.Diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0105");
    }

    /// <summary>Leaves RTI reference-generator restrictions to the backend.</summary>
    [Fact]
    public void BindsRecursiveSequenceAliasesWithoutApplyingTheCSharpReferenceRestriction()
    {
        // Arrange
        var input = new IdlInput("recursive.idl",
            """
            struct Node;
            typedef sequence<Node> Children;
            struct Node { Children children; };
            """,
            unboundedSupport: true);

        // Act
        var analysis = IdlAnalysis.Analyze([input], CompilerTestSupport.ComputeMd5,
            TestContext.Current.CancellationToken);

        // Assert
        analysis.Declarations.OfType<IdlClassDeclaration>().ShouldHaveSingleItem().Fields.ShouldHaveSingleItem()
            .Type.ShouldBeOfType<IdlType.Alias>().Target.ShouldBeOfType<IdlType.Sequence>()
            .Element.ShouldBeOfType<IdlType.Struct>().QualifiedName.ShouldBe("Node");
        var csharp = IdlCompiler.Compile([input], IdlCompilerProfile.Rti770, CompilerTestSupport.ComputeMd5,
            TestContext.Current.CancellationToken);
        csharp.Success.ShouldBeFalse();
        csharp.Diagnostics.ShouldContain(diagnostic => diagnostic.Message.Contains("Recursive collection typedef", StringComparison.Ordinal));
    }
}

