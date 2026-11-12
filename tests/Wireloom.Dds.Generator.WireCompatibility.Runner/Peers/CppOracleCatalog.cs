using System.Security.Cryptography;
using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Selects retained RTI C++ generated output only when its provenance hashes verify.</summary>
/// <param name="manifestPath">The profile's rtiddsgen C++ capture manifest.</param>
/// <param name="repositoryRoot">The checkout containing canonical IDL and retained output.</param>
internal sealed class CppOracleCatalog(string manifestPath, string repositoryRoot)
{
    private static readonly System.Text.RegularExpressions.Regex TopicTypePattern = new(
        @"struct\s+topic_type_name\s*<\s*([^>]+)\s*>", System.Text.RegularExpressions.RegexOptions.CultureInvariant);

    private readonly Lazy<JsonNode> manifest = new(() => JsonNode.Parse(File.ReadAllText(manifestPath))!);
    private readonly string cppRoot = Path.GetDirectoryName(Path.GetFullPath(manifestPath))!;
    private readonly string profileRoot = Directory.GetParent(Path.GetDirectoryName(Path.GetFullPath(manifestPath))!)!.FullName;
    private readonly string repository = Path.GetFullPath(repositoryRoot);

    /// <summary>Returns a retained generated source directory after validating the source and every output hash.</summary>
    internal string? FindSource(string caseId, CaseMetadata metadata, string expectedVersion)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(caseId);
        if (!File.Exists(manifestPath))
        {
            return null;
        }

        var entry = manifest.Value["cases"]?.AsArray()
            .SingleOrDefault(item => string.Equals((string?)item?["id"], caseId, StringComparison.Ordinal));
        if (entry is null || (string?)entry["status"] != "generated" || (int?)entry["exitCode"] != 0)
        {
            return null;
        }

        if ((string?)manifest.Value["rtiVersion"] != expectedVersion
            || metadata.RtiCppIdlAdaptations.Count > 0)
        {
            return null;
        }

        var canonicalIdl = (string?)entry["idl"]
            ?? throw new InvalidDataException($"The RTI C++ oracle entry for {caseId} has no IDL path.");
        if (!string.Equals(canonicalIdl.Replace('/', Path.DirectorySeparatorChar), metadata.Idl.Replace('/', Path.DirectorySeparatorChar),
            StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        var expectedInputHash = (string?)entry["sourceSha256"];
        var generatedInputHash = (string?)entry["generatedInputSha256"];
        var adaptations = entry["adaptations"]?.AsArray().Select(item => (string?)item).ToArray() ?? [];
        if (string.IsNullOrWhiteSpace(expectedInputHash) || string.IsNullOrWhiteSpace(generatedInputHash))
        {
            return null;
        }

        var idlPath = ResolveUnder(repository, Path.Combine("docs", "corpus", canonicalIdl.Replace('/', Path.DirectorySeparatorChar)));
        if (!File.Exists(idlPath) || !HashFile(idlPath).Equals(expectedInputHash, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidDataException($"The RTI C++ oracle input hash for {caseId} does not match {canonicalIdl}.");
        }
        var generatedIdl = File.ReadAllText(idlPath);
        if (adaptations.Length > 0)
        {
            if (caseId != "10-flat-data-binding" || expectedVersion is not ("6.1.2" or "7.3.1")
                || adaptations.Length != 1
                || !adaptations[0]!.Contains("@language_binding(FLAT_DATA)", StringComparison.Ordinal))
            {
                return null;
            }

            generatedIdl = System.Text.RegularExpressions.Regex.Replace(
                generatedIdl,
                @"(?m)^.*@language_binding\(FLAT_DATA\).*\r?\n",
                string.Empty);
        }

        if (!HashText(generatedIdl).Equals(generatedInputHash, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidDataException($"The RTI C++ generated input hash for {caseId} does not match its recorded adaptation.");
        }
        if (generatedIdl.Contains("#include", StringComparison.Ordinal))
        {
            return null;
        }

        var codegenOptions = entry["options"]?.AsArray().Select(item => (string?)item).ToArray() ?? [];
        if (!ContainsInOrder(codegenOptions, metadata.RtiCppCodegenOptions))
        {
            return null;
        }

        var sourceDirectory = (string?)entry["sourceDirectory"]
            ?? DeriveSourceDirectoryFromLegacyEntry(entry, caseId);
        if (sourceDirectory is null)
        {
            return null;
        }

        var isLegacyEntry = entry["sourceDirectory"] is null;
        var legacyCategory = (string?)entry["category"];
        var expectedSourceDirectory = isLegacyEntry
            ? $"{legacyCategory}/{caseId}"
            : $"features/{caseId}";
        if (!string.Equals(sourceDirectory.Replace('\\', '/'), expectedSourceDirectory, StringComparison.Ordinal))
        {
            return null;
        }

        var wrapperName = caseId switch
        {
            "14-autoid-scope" => "14-autoid-scope-topic.idl",
            "14-enum-constant" => "14-enum-constant-topic.idl",
            _ => caseId + ".idl",
        };
        var wrapperPath = Path.Combine(repository, "tests", "Wireloom.Dds.Generator.WireCompatibility", "IdlWrappers", wrapperName);
        if (File.Exists(wrapperPath))
        {
            return null;
        }

        var outputFiles = entry["output"]?["files"]?.AsArray();
        if (outputFiles is null)
        {
            return FindNamedFileSource(entry, caseId, sourceDirectory);
        }

        var expectedFileNames = ExpectedGeneratedFileNames(caseId);
        if (isLegacyEntry && !HasExactLegacyGeneratedFiles(entry["generatedFiles"], expectedFileNames))
        {
            return null;
        }

        var selectedFiles = new List<string>();
        foreach (var outputFile in outputFiles)
        {
            if (outputFile is null)
            {
                throw new InvalidDataException($"The RTI C++ oracle entry for {caseId} contains an empty output entry.");
            }

            var relativePath = (string?)outputFile["path"]
                ?? throw new InvalidDataException($"The RTI C++ oracle entry for {caseId} contains an output without a path.");
            if (!relativePath.StartsWith("cpp/", StringComparison.Ordinal))
            {
                continue;
            }

            var outputPath = ResolveUnder(profileRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
            var expectedHash = (string?)outputFile["sha256"];
            if (string.IsNullOrWhiteSpace(expectedHash) || !File.Exists(outputPath)
                || !HashFile(outputPath).Equals(expectedHash, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException($"The RTI C++ oracle output hash for {caseId} does not match {relativePath}.");
            }

            selectedFiles.Add(outputPath);
        }

        var sourcePath = ResolveUnder(cppRoot, sourceDirectory.Replace('/', Path.DirectorySeparatorChar));
        if (selectedFiles.Count == 0
            || selectedFiles.Any(path => !path.StartsWith(sourcePath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            || (isLegacyEntry && (selectedFiles.Count != expectedFileNames.Count
                || selectedFiles.Any(path => !string.Equals(Path.GetDirectoryName(path), sourcePath, StringComparison.OrdinalIgnoreCase))
                || !selectedFiles.Select(path => Path.GetFileName(path)).ToHashSet(StringComparer.Ordinal).SetEquals(expectedFileNames))))
        {
            throw new InvalidDataException($"The RTI C++ oracle output list for {caseId} does not identify files in its source directory.");
        }

        return Path.GetRelativePath(profileRoot, sourcePath).Replace('\\', '/');
    }

    private string? FindNamedFileSource(JsonNode entry, string caseId, string sourceDirectory)
    {
        var expectedFiles = ExpectedGeneratedFileNames(caseId);
        if (!HasExactLegacyGeneratedFiles(entry["generatedFiles"], expectedFiles))
        {
            return null;
        }

        var sourcePath = ResolveUnder(cppRoot, sourceDirectory.Replace('/', Path.DirectorySeparatorChar));
        foreach (var name in expectedFiles)
        {
            if (!File.Exists(Path.Combine(sourcePath, name)))
            {
                return null;
            }
        }

        return Path.GetRelativePath(profileRoot, sourcePath).Replace('\\', '/');
    }

    /// <summary>Finds the single topic type declared by the verified retained RTI C++ oracle.</summary>
    internal string FindTopicType(string caseId, CaseMetadata metadata, string expectedVersion)
    {
        var sourceDirectory = FindSource(caseId, metadata, expectedVersion)
            ?? throw new InvalidDataException($"No verified RTI C++ oracle is available to resolve the topic type for {caseId}.");
        var sourcePath = ResolveUnder(profileRoot, sourceDirectory.Replace('/', Path.DirectorySeparatorChar));
        var types = Directory.EnumerateFiles(sourcePath, "*.hpp", SearchOption.AllDirectories)
            .SelectMany(path => TopicTypePattern.Matches(File.ReadAllText(path))
                .Select(match => match.Groups[1].Value.Trim().TrimStart(':')))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        if (types.Length != 1)
        {
            throw new InvalidDataException($"The verified RTI C++ oracle for {caseId} must declare exactly one topic type; found {types.Length}.");
        }

        return "::" + types[0];
    }

    private static string ResolveUnder(string root, string relativePath)
    {
        var fullRoot = Path.GetFullPath(root).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        var fullPath = Path.GetFullPath(Path.Combine(root, relativePath));
        if (!fullPath.StartsWith(fullRoot, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidDataException($"RTI C++ oracle path escapes its configured root: {relativePath}.");
        }

        return fullPath;
    }

    private static string HashFile(string path) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();

    private static string HashText(string text) => Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(text))).ToLowerInvariant();

    private static string? DeriveSourceDirectoryFromLegacyEntry(JsonNode entry, string caseId)
    {
        var category = (string?)entry["category"];
        var generatedFiles = entry["generatedFiles"]?.AsArray();
        if (!IsSafePathSegment(category) || !IsSafePathSegment(caseId) || generatedFiles is null
            || !HasExactLegacyGeneratedFiles(generatedFiles, ExpectedGeneratedFileNames(caseId)))
        {
            return null;
        }

        return $"{category}/{caseId}";
    }

    private static bool IsSafePathSegment(string? value) =>
        !string.IsNullOrWhiteSpace(value)
        && System.Text.RegularExpressions.Regex.IsMatch(value,
            @"\A[a-z0-9][a-z0-9-]*\z", System.Text.RegularExpressions.RegexOptions.CultureInvariant);

    private static HashSet<string> ExpectedGeneratedFileNames(string caseId) =>
    [
        caseId + ".cxx",
        caseId + ".hpp",
        caseId + "Plugin.cxx",
        caseId + "Plugin.hpp",
    ];

    private static bool HasExactLegacyGeneratedFiles(JsonNode? generatedFiles, HashSet<string> expectedFileNames)
    {
        if (generatedFiles is not JsonArray files || files.Count != expectedFileNames.Count)
        {
            return false;
        }

        var actualFileNames = new HashSet<string>(StringComparer.Ordinal);
        foreach (var file in files)
        {
            var name = (string?)file;
            if (string.IsNullOrWhiteSpace(name)
                || !string.Equals(name, Path.GetFileName(name), StringComparison.Ordinal)
                || !actualFileNames.Add(name))
            {
                return false;
            }
        }

        return actualFileNames.SetEquals(expectedFileNames);
    }

    private static bool ContainsInOrder(IReadOnlyList<string?> options, IReadOnlyList<string> required)
    {
        var requiredIndex = 0;
        foreach (var option in options)
        {
            if (requiredIndex < required.Count && string.Equals(option, required[requiredIndex], StringComparison.Ordinal))
            {
                requiredIndex++;
            }
        }

        return requiredIndex == required.Count;
    }
}
