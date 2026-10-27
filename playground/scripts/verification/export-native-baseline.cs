#:project ../../../src/Wireloom.Dds.Compiler/Wireloom.Dds.Compiler.csproj
#:property RestorePackagesWithLockFile=false

using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wireloom;

if (args.Length != 2)
{
    throw new ArgumentException(
        "Usage: dotnet run --file playground/scripts/verification/export-native-baseline.cs -- <cases.json> <output.json>");
}

var casesPath = Path.GetFullPath(args[0]);
var outputPath = Path.GetFullPath(args[1]);
var cases = JsonSerializer.Deserialize(
    File.ReadAllText(casesPath),
    NativeBaselineJsonContext.Default.CaseDefinitionArray)
    ?? throw new InvalidDataException("The case file must contain a JSON array.");
var baseline = new List<BaselineEntry>();
var seenCaseIds = new HashSet<string>(StringComparer.Ordinal);

foreach (var testCase in cases)
{
    var caseId = testCase.Id;
    if (!seenCaseIds.Add(caseId))
    {
        throw new InvalidDataException($"Case id '{caseId}' is duplicated.");
    }

    var files = testCase.Files
        .Select(file => new IdlInput(file.Path, file.Source,
            file.Options?.SequenceArrayMapping == "Idl" ? IdlSequenceArrayMapping.Idl : IdlSequenceArrayMapping.RtiCSharp,
            generate: file.IsGenerationRoot, strict: testCase.Options?.Strict ?? false,
            defines: testCase.Options?.Defines, undefines: testCase.Options?.Undefines,
            includeDirectories: testCase.Options?.IncludeDirectories?.Order(StringComparer.Ordinal).ToArray(),
            sequenceSize: file.Options?.SequenceSize ?? 100, stringSize: file.Options?.StringSize ?? 255,
            enableEscapeChar: testCase.Options?.EnableEscapeChar ?? false))
        .ToArray();

    if (files.Length == 0 || !files.Any(file => file.Generate))
    {
        throw new InvalidDataException($"Case '{caseId}' must include at least one generation root.");
    }

    foreach (var profile in new[]
    {
        (Name: "7.3.1", Value: IdlCompilerProfile.Rti731),
        (Name: "7.7.0", Value: IdlCompilerProfile.Rti770),
    })
    {
        var result = IdlCompiler.Compile(
            files,
            profile.Value,
            static bytes => MD5.HashData(bytes),
            CancellationToken.None);
        if (!testCase.ExpectedSuccess.TryGetValue(profile.Name, out var expectedSuccess))
        {
            throw new InvalidDataException($"Case '{caseId}' has no expected result for profile {profile.Name}.");
        }

        if (result.Success != expectedSuccess)
        {
            throw new InvalidDataException(
                $"Case '{caseId}' profile {profile.Name} expected success={expectedSuccess}, actual={result.Success}. "
                + string.Join("; ", result.Diagnostics.Select(diagnostic => $"{diagnostic.Id} at {diagnostic.Offset}: {diagnostic.Message}")));
        }

        baseline.Add(new BaselineEntry(
            caseId,
            profile.Name,
            result.Success,
            result.Documents
                .OrderBy(document => document.HintName, StringComparer.Ordinal)
                .Select(document => new BaselineDocument(document.HintName, document.Source))
                .ToArray(),
            result.Diagnostics
                .OrderBy(diagnostic => diagnostic.Path, StringComparer.Ordinal)
                .ThenBy(diagnostic => diagnostic.Offset)
                .ThenBy(diagnostic => diagnostic.Id, StringComparer.Ordinal)
                .Select(diagnostic => new BaselineDiagnostic(
                    diagnostic.Id,
                    diagnostic.Severity.ToString(),
                    diagnostic.Message,
                    diagnostic.Path,
                    diagnostic.Offset,
                    0))
                .ToArray()));
    }
}

Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
File.WriteAllText(outputPath, JsonSerializer.Serialize(
    baseline.ToArray(),
    NativeBaselineJsonContext.Default.BaselineEntryArray));

Console.WriteLine($"Exported {baseline.Count} native baselines for {seenCaseIds.Count} cases to {outputPath}.");

/// <summary>Describes one input file in a browser-shaped verification case.</summary>
/// <param name="Path">The normalized virtual input path.</param>
/// <param name="Source">The complete IDL source.</param>
/// <param name="IsGenerationRoot">Whether this file is a generation root.</param>
/// <param name="Options">Optional declaring-file settings.</param>
internal sealed record CaseFile(string Path, string Source, bool IsGenerationRoot, CaseFileOptions? Options = null);

/// <summary>Declaring-file settings for a native verification fixture.</summary>
/// <param name="SequenceArrayMapping">The direct sequence-array mapping.</param>
/// <param name="SequenceSize">The effective unbounded sequence size.</param>
/// <param name="StringSize">The effective unbounded string size.</param>
internal sealed record CaseFileOptions(string SequenceArrayMapping = "RtiCSharp", int SequenceSize = 100, int StringSize = 255);

/// <summary>Batch settings for a native verification fixture.</summary>
/// <param name="Defines">External definitions.</param>
/// <param name="Undefines">Symbols removed after definitions.</param>
/// <param name="IncludeDirectories">Virtual include search directories.</param>
/// <param name="Strict">Batch strictness.</param>
/// <param name="EnableEscapeChar">Batch escaped-identifier setting.</param>
internal sealed record CaseOptions(string[] Defines, string[] Undefines, string[] IncludeDirectories, bool Strict, bool EnableEscapeChar);

/// <summary>Describes one browser-shaped input case and expected native outcomes.</summary>
/// <param name="Id">The stable verification case identifier.</param>
/// <param name="ExpectedSuccess">Expected success by RTI profile name.</param>
/// <param name="Files">All roots and include-only files in this workspace.</param>
/// <param name="Options">Optional batch settings.</param>
internal sealed record CaseDefinition(
    string Id,
    IReadOnlyDictionary<string, bool> ExpectedSuccess,
    CaseFile[] Files,
    CaseOptions? Options = null);

/// <summary>Contains the compiler result for one case and profile.</summary>
/// <param name="CaseId">The source case identifier.</param>
/// <param name="Profile">The RTI profile label.</param>
/// <param name="Success">Whether compilation succeeded.</param>
/// <param name="Documents">Every generated document, sorted by name.</param>
/// <param name="Diagnostics">Every diagnostic in deterministic source order.</param>
internal sealed record BaselineEntry(
    string CaseId,
    string Profile,
    bool Success,
    BaselineDocument[] Documents,
    BaselineDiagnostic[] Diagnostics);

/// <summary>Contains one generated source document.</summary>
/// <param name="Name">The compiler document hint name.</param>
/// <param name="Source">The exact generated C# text.</param>
internal sealed record BaselineDocument(string Name, string Source);

/// <summary>Contains one source-located compiler diagnostic.</summary>
/// <param name="Id">The compiler diagnostic identifier.</param>
/// <param name="Severity">The compiler diagnostic severity.</param>
/// <param name="Message">The diagnostic message.</param>
/// <param name="FilePath">The diagnostic virtual input path.</param>
/// <param name="StartOffset">The UTF-16 source offset.</param>
/// <param name="Length">Zero because the current compiler API provides no span length.</param>
internal sealed record BaselineDiagnostic(
    string Id,
    string Severity,
    string Message,
    string FilePath,
    int StartOffset,
    int Length);

/// <summary>Provides trimmed, source-generated JSON metadata for the exporter.</summary>
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, WriteIndented = true)]
[JsonSerializable(typeof(CaseDefinition[]))]
[JsonSerializable(typeof(BaselineEntry[]))]
internal sealed partial class NativeBaselineJsonContext : JsonSerializerContext;
