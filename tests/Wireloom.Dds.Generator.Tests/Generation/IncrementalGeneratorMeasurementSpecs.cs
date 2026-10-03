using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Wireloom.Generation.Tests;

/// <summary>Measures current Roslyn incremental-generator invalidation behavior.</summary>
public sealed class IncrementalGeneratorMeasurementSpecs(ITestOutputHelper output)
{
    /// <summary>Records cold, unchanged, and changed-input incremental runs.</summary>
    [Fact]
    public void RecordsCurrentIncrementalInvalidationBaseline()
    {
        // Arrange
        var measurements = new[]
        {
            MeasureColdRun(),
            MeasureUnchangedRerun(),
            MeasureRootChange(),
            MeasureIncludedChange(),
            MeasureUnrelatedInputChange(),
            MeasureMultipleRoots()
        };

        // Act
        foreach (var measurement in measurements)
        {
            output.WriteLine(measurement.Format());
        }

        // Assert
        measurements.ShouldAllBe(measurement => measurement.GeneratedSourceCount > 0);
        measurements.ShouldAllBe(measurement => measurement.StepCount > 0);
        measurements.Single(measurement => measurement.Name == "unchanged-rerun")
            .Reasons.ShouldContain(reason => reason.Key == IncrementalStepRunReason.Cached && reason.Value > 0);
    }

    private static Measurement MeasureColdRun()
    {
        var scenario = CreateScenario();
        return Run("cold-run", scenario.Driver, scenario.Compilation).Measurement;
    }

    private static Measurement MeasureUnchangedRerun()
    {
        var scenario = CreateScenario();
        var first = Run("cold-run", scenario.Driver, scenario.Compilation);
        return Run("unchanged-rerun", first.Driver, scenario.Compilation).Measurement;
    }

    private static Measurement MeasureRootChange()
    {
        var scenario = CreateScenario();
        var first = Run("cold-run", scenario.Driver, scenario.Compilation);
        var changedRoot = new TestAdditionalText("root.idl", scenario.RootText.Replace("Root", "ChangedRoot", StringComparison.Ordinal));
        var driver = first.Driver.ReplaceAdditionalText(scenario.Root, changedRoot);
        return Run("root-change", driver, scenario.Compilation).Measurement;
    }

    private static Measurement MeasureIncludedChange()
    {
        var scenario = CreateScenario();
        var first = Run("cold-run", scenario.Driver, scenario.Compilation);
        var changedIncluded = new TestAdditionalText("shared.idl", "struct Shared { long changed; };");
        var driver = first.Driver.ReplaceAdditionalText(scenario.Included, changedIncluded);
        return Run("included-change", driver, scenario.Compilation).Measurement;
    }

    private static Measurement MeasureUnrelatedInputChange()
    {
        var scenario = CreateScenario(includeUnrelatedInput: true);
        var first = Run("cold-run", scenario.Driver, scenario.Compilation);
        var changedUnrelated = new TestAdditionalText("notes.txt", "unrelated changed");
        var driver = first.Driver.ReplaceAdditionalText(scenario.Unrelated!, changedUnrelated);
        return Run("unrelated-input-change", driver, scenario.Compilation).Measurement;
    }

    private static Measurement MeasureMultipleRoots()
    {
        var scenario = CreateScenario(includeSecondRoot: true);
        var first = Run("cold-run", scenario.Driver, scenario.Compilation);
        var secondRoot = scenario.SecondRoot!;
        var changedSecondRoot = new TestAdditionalText("second.idl", "module Second { struct Changed { long value; }; };");
        var driver = first.Driver.ReplaceAdditionalText(secondRoot, changedSecondRoot);
        return Run("multiple-roots-second-root-change", driver, scenario.Compilation).Measurement;
    }

    private static Scenario CreateScenario(bool includeUnrelatedInput = false, bool includeSecondRoot = false)
    {
        var root = new TestAdditionalText(
            "root.idl",
            "#include \"shared.idl\"\nmodule Sample { struct Root { Shared value; }; }; ");
        var included = new TestAdditionalText("shared.idl", "struct Shared { long value; };");
        var additionalTexts = new List<AdditionalText> { root, included };
        var options = new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            [root.Path] = new Dictionary<string, string> { ["Generate"] = "true" },
            [included.Path] = new Dictionary<string, string> { ["Generate"] = "false" }
        };

        TestAdditionalText? unrelated = null;
        if (includeUnrelatedInput)
        {
            unrelated = new TestAdditionalText("notes.txt", "unrelated");
            additionalTexts.Add(unrelated);
        }

        TestAdditionalText? secondRoot = null;
        if (includeSecondRoot)
        {
            secondRoot = new TestAdditionalText("second.idl", "module Second { struct Value { long value; }; };");
            additionalTexts.Add(secondRoot);
            options[secondRoot.Path] = new Dictionary<string, string> { ["Generate"] = "true" };
        }

        var parseOptions = new CSharpParseOptions(LanguageVersion.CSharp12);
        var compilation = CSharpCompilation.Create(
            "IncrementalMeasurement",
            [CSharpSyntaxTree.ParseText("namespace Input; public sealed class Marker { }", parseOptions)],
            RuntimeReferences(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var driver = CSharpGeneratorDriver.Create(
            [new Generator().AsSourceGenerator()],
            additionalTexts,
            parseOptions,
            new TestAnalyzerConfigOptionsProvider(options),
            new GeneratorDriverOptions(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true, baseDirectory: null));

        return new Scenario(driver, compilation, root, included, unrelated, secondRoot, root.GetText()!.ToString());
    }

    private static (GeneratorDriver Driver, Measurement Measurement) Run(string name, GeneratorDriver driver, Compilation compilation)
    {
        var stopwatch = Stopwatch.StartNew();
        driver = driver.RunGenerators(compilation);
        stopwatch.Stop();

        var result = driver.GetRunResult().Results.Single();
        var generatorElapsed = driver.GetTimingInfo().GeneratorTimes.Single().ElapsedTime;
        var reasons = result.TrackedSteps
            .SelectMany(step => step.Value)
            .SelectMany(step => step.Outputs)
            .GroupBy(output => output.Item2)
            .ToDictionary(group => group.Key, group => group.Count());
        var stepDetails = result.TrackedSteps
            .SelectMany(step => step.Value)
            .Select(step => $"{step.Name}:{string.Join(',', step.Outputs.GroupBy(output => output.Item2).OrderBy(group => group.Key).Select(group => $"{group.Key}={group.Count()}"))}")
            .ToArray();

        return (driver, new Measurement(name, stopwatch.Elapsed, generatorElapsed, result.GeneratedSources.Length, result.TrackedSteps.Sum(step => step.Value.Length), reasons, stepDetails));
    }

    private static IEnumerable<MetadataReference> RuntimeReferences()
    {
        var paths = (string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") ?? string.Empty;
        var references = paths
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Where(path => !string.Equals(Path.GetFileName(path), "Rti.ConnextDds.dll", StringComparison.OrdinalIgnoreCase))
            .Select(path => MetadataReference.CreateFromFile(path))
            .ToList();
        references.Add(MetadataReference.CreateFromFile(Path.Combine(AppContext.BaseDirectory, "Rti.ConnextDds.dll")));
        return references;
    }

    private sealed record Measurement(
        string Name,
        TimeSpan DriverElapsed,
        TimeSpan GeneratorElapsed,
        int GeneratedSourceCount,
        int StepCount,
        IReadOnlyDictionary<IncrementalStepRunReason, int> Reasons,
        IReadOnlyList<string> StepDetails)
    {
        public string Format() =>
            $"{Name}: driverMs={DriverElapsed.TotalMilliseconds:F2}; generatorMs={GeneratorElapsed.TotalMilliseconds:F2}; generated={GeneratedSourceCount}; steps={StepCount}; reasons={string.Join(',', Reasons.OrderBy(reason => reason.Key).Select(reason => $"{reason.Key}:{reason.Value}"))}; details={string.Join('|', StepDetails)}";
    }

    private sealed record Scenario(
        GeneratorDriver Driver,
        Compilation Compilation,
        TestAdditionalText Root,
        TestAdditionalText Included,
        TestAdditionalText? Unrelated,
        TestAdditionalText? SecondRoot,
        string RootText);

    private sealed class TestAdditionalText(string path, string text) : AdditionalText
    {
        public override string Path { get; } = path;

        public override SourceText? GetText(CancellationToken cancellationToken = default) => SourceText.From(text);
    }

    private sealed class TestAnalyzerConfigOptionsProvider(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> values) : AnalyzerConfigOptionsProvider
    {
        private readonly TestAnalyzerConfigOptions empty = new(new Dictionary<string, string>());

        public override AnalyzerConfigOptions GlobalOptions => empty;

        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => empty;

        public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) =>
            new TestAnalyzerConfigOptions(values.TryGetValue(textFile.Path, out var options) ? options : new Dictionary<string, string>());
    }

    private sealed class TestAnalyzerConfigOptions(IReadOnlyDictionary<string, string> values) : AnalyzerConfigOptions
    {
        public override bool TryGetValue(string key, out string value)
        {
            var metadataName = key.StartsWith("build_metadata.AdditionalFiles.", StringComparison.OrdinalIgnoreCase)
                ? key["build_metadata.AdditionalFiles.".Length..]
                : key;
            return values.TryGetValue(metadataName, out value!);
        }
    }
}
