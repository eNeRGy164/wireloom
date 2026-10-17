// Black-box partition/merge checks without an RTI toolchain or license.
#:property NoWarn=CS7022%3BIL2026%3BIL3050
#:property RestorePackagesWithLockFile=false
using System.Diagnostics;
using System.Text.Json.Nodes;

if (args.Length != 1)
{
    throw new ArgumentException("Usage: verify-shards.cs <built-matrix-tools.dll>");
}
var tool = Path.GetFullPath(args[0]);
var repositoryRoot = FindRepositoryRoot(tool);
var rtiVersion = Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_RTI_VERSION") ?? "7.7.0";
var temporaryRoot = Path.Combine(repositoryRoot, "tmp");
var root = Path.Combine(temporaryRoot, "wireloom-shards-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(root);
var checks = 0;
try
{
    // Arrange / Act / Assert: applicability changes selection before type-support discovery.
    var manifestPath = Path.Combine(repositoryRoot, "docs", "corpus", "manifest.json");
    var previousVersion = Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_RTI_VERSION");
    try
    {
        foreach (var version in new[] { "7.3.1", "7.7.0" })
        {
            Environment.SetEnvironmentVariable("WIRE_COMPATIBILITY_RTI_VERSION", version);
            var registryPath = Path.Combine(repositoryRoot, "tests", "Wireloom.Dds.Generator.WireCompatibility", $"wire-case-exclusions-{version}.json");
            var selected = Invoke(true, "list-cases", manifestPath, registryPath).Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            Check(selected.Length == (version == "7.3.1" ? 76 : 77), "Only applicable positive cases are selected.");
            Check(selected.Contains("09-autoid-hash") == (version == "7.7.0"), "The must-understand fixture is available only on 7.7.0.");
            var generatedRoot = Path.Combine(repositoryRoot, "docs", "corpus", version, "generator");
            var discovery = Invoke(version == "7.7.0", "discover-cases", manifestPath, generatedRoot, Path.Combine(root, "discovered.json"), "09-autoid-hash");
            if (version == "7.3.1")
            {
                Check(discovery.Contains("not a wire-testable", StringComparison.Ordinal), "Inapplicable cases are rejected before type discovery.");
            }
        }
    }
    finally
    {
        Environment.SetEnvironmentVariable("WIRE_COMPATIBILITY_RTI_VERSION", previousVersion);
    }

    // Arrange: unequal case sizes, all nine pairings, and an RTI-only case.
    var peers = new[] { "Wireloom C#", "RTI C#", "RTI C++" };
    var cases = new Dictionary<string, string[]>
    {
        ["01-boundaries"] = ["non-default"],
        ["01-full-widths"] = ["non-default"],
        ["09-defaults-ranges"] = ["non-default"],
        ["05-array-of-sequences"] = ["default"],
        ["05-collections"] = ["sequence-empty", "sequence-single", "sequence-multiple"],
        ["07-union-char-numeric"] = ["union-127", "union-default"],
        ["07-union-char-numeric-255"] = ["union-255", "union-default"],
        ["07-union-char-boundaries"] = ["union-minimum", "union-maximum", "union-default"],
        ["07-union-ulong"] = ["union-minimum", "union-maximum", "union-default"],
        ["09-optional-aggregate-member"] = ["optional-absent", "optional-present"],
        ["09-optional-recursive-array"] = ["optional-absent", "optional-present", "optional-nested"]
    };
    var pairings = peers.SelectMany(writer => peers.Select(reader => (Writer: writer, Reader: reader))).ToArray();
    var rows = new List<string[]>();
    foreach (var item in cases)
    {
        foreach (var fixture in item.Value)
        {
            foreach (var (writer, reader) in pairings)
            {
                if (item.Key == "09-optional-aggregate-member" && (writer == peers[0] || reader == peers[0]))
                {
                    continue;
                }

                var status = "PASS";
                if (item.Key == "05-array-of-sequences"
                    && ((writer == peers[0] && reader == peers[2]) || (writer == peers[2] && reader == peers[0])))
                {
                    status = "FAIL";
                }
                else if (item.Key == "07-union-ulong" && (writer == peers[1] || reader == peers[1]))
                {
                    status = "FAIL";
                }

                rows.Add([item.Key, "Managed.Type", "Oracle.Type", "::Native::Type", writer, rtiVersion, reader, rtiVersion, status, fixture]);
            }
        }
    }
    var seed = Path.Combine(root, "all.tsv");
    File.WriteAllLines(seed, rows.Select(row => string.Join('\t', row)));
    var exclusions = Path.Combine(root, "exclusions.json");
    File.WriteAllText(exclusions, """
        {
          "notImplementedCases": [{"id":"09-optional-aggregate-member","diagnostic":"TEST","reason":"RTI-only control","issue":"https://github.com/eNeRGy164/wireloom/issues/15"}],
          "notApplicableCases": [{"id":"09-autoid-hash","reason":"RTI feature unavailable in this test profile"}],
          "expectedFailures": [{"id":"05-array-of-sequences","writer":"Wireloom C#","reader":"RTI C++","reason":"registered failure"}]
        }
        """);
    var source = Path.Combine(root, "shards");
    for (var index = 0; index < 4; index++)
    {
        var directory = Path.Combine(source, $"shard-{index}");
        Invoke(true, "partition-shard", rtiVersion, index.ToString(), "4", seed, directory);
        var logs = Path.Combine(directory, "logs");
        Directory.CreateDirectory(logs);
        foreach (var row in File.ReadAllLines(Path.Combine(directory, "scenarios.tsv")).Select(line => line.Split('\t')))
        {
            var peerIds = new Dictionary<string, string> { [peers[0]] = "wireloom-cs", [peers[1]] = "rti-cs", [peers[2]] = "rti-cpp" };
            var logText = "FAIL_DETAIL retained symptom\n";
            if (row[8] == "NOT_RUN")
            {
                logText = "NOT_RUN_DETAIL interrupted exchange\n";
            }
            else if (row[0] == "07-union-ulong" && row[8] == "FAIL")
            {
                logText = "FAIL_DETAIL RTI-generated C# peer failed to compile; CS0221\n";
            }

            File.WriteAllText(Path.Combine(logs, $"{row[0]}-{row[9]}-{peerIds[row[4]]}-{peerIds[row[6]]}.log"),
                logText);
        }
        var count = File.ReadAllLines(Path.Combine(directory, "scenarios.tsv")).Length.ToString();
        Invoke(true, "render-report", rtiVersion, count, Path.Combine(directory, "scenarios.tsv"),
            Path.Combine(directory, "results.json"), Path.Combine(directory, "results.md"), logs, exclusions,
            Path.Combine(repositoryRoot, "docs", "corpus", "manifest.json"));
    }

    // Act: compare the merged report with an unsharded rendering of the same rows/logs.
    var merged = Path.Combine(root, "merged");
    Invoke(true, "merge-shards", rtiVersion, "4", source, merged, exclusions);
    var baseline = Path.Combine(root, "baseline.json");
    Invoke(true, "render-report", rtiVersion, rows.Count.ToString(), seed, baseline,
        Path.Combine(root, "baseline.md"), Path.Combine(merged, "logs"), exclusions,
        Path.Combine(repositoryRoot, "docs", "corpus", "manifest.json"));

    // Assert: payload/report evidence survives, including registry entries and failure reasons.
    Check(JsonNode.DeepEquals(Read(baseline), Read(Path.Combine(merged, "results.json"))), "Combined JSON equals the unsharded report.");
    Check((int)Read(baseline)["notApplicableCount"]! == 1, "Unavailable features are recorded separately from failures.");
    Check(Read(baseline)["scenarios"]!.AsArray().All(item => (string?)item!["case"] != "09-autoid-hash"), "Unavailable features add no exchanges to the denominator.");
    Check(File.ReadAllText(Path.Combine(merged, "results.md")).Contains("Not applicable", StringComparison.Ordinal), "The Markdown report explains unavailable features.");
    var unavailableSeed = Path.Combine(root, "unavailable.tsv");
    var unavailableRow = rows[0].ToArray();
    unavailableRow[0] = "09-autoid-hash";
    File.WriteAllText(unavailableSeed, string.Join('\t', unavailableRow));
    var unavailableResult = Invoke(false, "render-report", rtiVersion, "1", unavailableSeed,
        Path.Combine(root, "unavailable.json"), Path.Combine(root, "unavailable.md"),
        Path.Combine(merged, "logs"), exclusions, manifestPath);
    Check(unavailableResult.Contains("not applicable", StringComparison.Ordinal), "Reports reject an exchange for an unavailable feature instead of counting it.");
    // Assert: endpoint notes describe the selected release, without leaking another profile's defect rationale.
    var otherVersion = rtiVersion == "7.3.1" ? "7.7.0" : "7.3.1";
    foreach (var scenario in Read(baseline)["scenarios"]!.AsArray())
    {
        foreach (var role in new[] { "writer", "reader" })
        {
            var adaptation = (string?)scenario![role]!["adaptation"] ?? "";
            Check(!adaptation.Contains(otherVersion, StringComparison.Ordinal),
                "Endpoint adaptations do not attribute another release's defects to the selected profile.");
        }
    }

    // Assert: typed scalar adaptations appear on managed endpoints, without attributing them to C++.
    foreach (var caseId in new[] { "01-boundaries", "01-full-widths", "09-defaults-ranges" })
    {
        var scalarScenarios = Read(baseline)["scenarios"]!.AsArray()
            .Where(item => (string?)item!["case"] == caseId).ToArray();
        Check(scalarScenarios.Length == 9, "All scalar adaptation pairings remain in the report.");
        foreach (var scenario in scalarScenarios)
        {
            foreach (var role in new[] { "writer", "reader" })
            {
                var peer = scenario![role]!;
                var adaptation = (string?)peer["adaptation"] ?? "";
                Check(adaptation.Contains("Typed", StringComparison.Ordinal) == ((string?)peer["peer"] != "RTI C++"),
                    "Only managed endpoints report typed scalar fixture construction.");
            }
        }
    }
    var scalarMarkdown = File.ReadAllText(Path.Combine(merged, "results.md"));
    Check(scalarMarkdown.Contains("leave color untouched", StringComparison.Ordinal), "The report explains that the enum constructor default is preserved.");
    Check(scalarMarkdown.Contains("negative int8 values", StringComparison.Ordinal), "The report explains the signed-int8 adaptation.");
    var numericScenarios = Read(baseline)["scenarios"]!.AsArray()
        .Where(item => (string?)item!["case"] is "07-union-char-numeric" or "07-union-char-numeric-255").ToArray();
    Check(numericScenarios.Length == 36 && numericScenarios.All(item =>
        new[] { "writer", "reader" }.Where(role => (string?)item![role]!["peer"] == "RTI C++")
            .All(role => ((string?)item![role]!["adaptation"] ?? "").Contains("65/13703", StringComparison.Ordinal))),
        "Numeric character reports disclose the matching C++ default discriminator and payload.");
    Check(scalarMarkdown.Contains("127/13701 or 255/13702", StringComparison.Ordinal)
        && scalarMarkdown.Contains("65/13703", StringComparison.Ordinal),
        "Markdown reports retain the exact numeric character fixture expectations.");

    var recursiveScenarios = Read(baseline)["scenarios"]!.AsArray()
        .Where(item => (string?)item!["case"] == "09-optional-recursive-array").ToArray();
    Check(recursiveScenarios.Length == 27, "All recursive array exchanges are retained.");
    foreach (var scenario in recursiveScenarios)
    {
        foreach (var role in new[] { "writer", "reader" })
        {
            var peer = scenario![role]!;
            var adaptation = (string?)peer["adaptation"] ?? "";
            Check(adaptation.Contains("pool_buffer_max_size=4096", StringComparison.Ordinal),
                "Every recursive peer reports the writer fast-pool threshold.");
            Check(adaptation.Contains("Bootstrap", StringComparison.Ordinal) == ((string?)peer["peer"] == "RTI C#"),
                "Only the RTI C# reference reports a recursive support bootstrap.");
        }
    }

    Check(scalarMarkdown.Contains("pool_buffer_max_size=4096", StringComparison.Ordinal),
        "The Markdown report discloses the recursive writer memory adaptation.");
    Check(Read(baseline)["scenarios"]!.AsArray()
        .Where(item => (string?)item!["case"] != "09-optional-recursive-array")
        .All(item => new[] { "writer", "reader" }.All(role =>
            !((string?)item![role]!["adaptation"] ?? "").Contains("pool_buffer_max_size", StringComparison.Ordinal))),
        "The writer memory adaptation is scoped to recursive-array scenarios.");

    Check(File.ReadAllText(Path.Combine(root, "baseline.md")) == File.ReadAllText(Path.Combine(merged, "results.md")), "Combined Markdown equals the unsharded report.");
    var unsignedScenarios = Read(Path.Combine(merged, "results.json"))["scenarios"]!.AsArray()
        .Where(item => (string?)item!["case"] == "07-union-ulong").ToArray();
    Check(unsignedScenarios.Count(item => (string?)item!["status"] == "FAIL"
        && ((string?)item["observedProblem"])!.Contains("CS0221", StringComparison.Ordinal)) == 15,
        "All RTI C# build-blocked pairings retain failure status and compiler evidence.");
    Check(unsignedScenarios.Count(item => (string?)item!["status"] == "PASS") == 12,
        "Wireloom and C++ controls remain passing when the RTI C# peer cannot compile.");
    var markdownLines = File.ReadAllLines(Path.Combine(merged, "results.md"));
    var compilerRows = markdownLines.Where(line => line.Contains("CS0221", StringComparison.Ordinal)).ToArray();
    Check(compilerRows.Length == 3 && compilerRows.All(line => line.Split("CS0221").Length == 2
        && pairings.Where(pair => pair.Writer == peers[1] || pair.Reader == peers[1])
            .All(pair => line.Contains($"{pair.Writer} → {pair.Reader}", StringComparison.Ordinal))),
        "Each row shows one compiler message and identifies all five affected pairings.");
    var expectedRow = markdownLines.Single(line => line.StartsWith("| `05-array-of-sequences.idl`", StringComparison.Ordinal));
    Check(expectedRow.Split("retained symptom").Length == 3 && expectedRow.Contains("**Expected:** registered failure", StringComparison.Ordinal),
        "Matching observed messages with different expected-failure classifications remain separate.");
    var shardRows = Enumerable.Range(0, 4).Select(index => File.ReadAllLines(Path.Combine(source, $"shard-{index}", "scenarios.tsv"))).ToArray();
    Check(shardRows.Sum(items => items.Length) == rows.Count, "Every exchange is retained exactly once.");
    var caseSets = shardRows.Select(items => items.Select(line => line.Split('\t')[0]).ToHashSet()).ToArray();
    Check(caseSets.Sum(items => items.Count) == cases.Count && caseSets.SelectMany(items => items).Distinct().Count() == cases.Count, "Whole cases belong to exactly one shard.");
    Check(shardRows.Max(items => items.Length) - shardRows.Min(items => items.Length) <= rows.GroupBy(row => row[0]).Max(group => group.Count()), "Exchange counts are balanced within one indivisible case.");
    var repeat = Path.Combine(root, "repeat");
    Invoke(true, "partition-shard", rtiVersion, "0", "4", seed, repeat);
    Check(JsonNode.DeepEquals(Read(Path.Combine(source, "shard-0", "plan.json")), Read(Path.Combine(repeat, "plan.json"))), "Independent plans are deterministic.");

    Reject("missing-shard", directory => Directory.Move(Path.Combine(directory, "shard-1"), Path.Combine(root, "removed-shard")));
    Reject("duplicate-index", directory => ChangeJson(Path.Combine(directory, "shard-1", "shard.json"), node => node["shardIndex"] = 0));
    Reject("different-plan", directory => ChangeJson(Path.Combine(directory, "shard-1", "plan.json"), node => node["expectedScenarioCount"] = 999));
    Reject("wrong-version", directory => ChangeJson(Path.Combine(directory, "shard-1", "shard.json"), node => node["rtiVersion"] = rtiVersion == "7.7.0" ? "7.3.1" : "7.7.0"));
    Reject("missing-exchange", directory => ChangeRows(directory, lines => lines.Skip(1).ToArray()));
    Reject("unexecuted-exchange", directory => ChangeRows(directory, lines =>
    {
        var fields = lines[0].Split('\t');
        fields[8] = "NOT_RUN";
        return [string.Join('\t', fields), .. lines.Skip(1)];
    }));
    Reject("duplicate-exchange", directory => ChangeRows(directory, lines => lines.Skip(1).Append(lines[1]).ToArray()));
    Reject("unexpected-fixture", directory => ChangeRows(directory, lines => [lines[0] + "-unknown", .. lines.Skip(1)]));
    Reject("invalid-status", directory => ChangeRows(directory, lines => [lines[0].Replace("\tPASS\t", "\tUNKNOWN\t"), .. lines.Skip(1)]));
    Reject("wrong-report-count", directory => ChangeJson(Path.Combine(directory, "shard-0", "results.json"), node => node["scenarioCount"] = 999));
    Invoke(false, "partition-shard", "7.7.0", "4", "4", seed, Path.Combine(root, "invalid-index"));
    checks++;
    Invoke(false, "partition-shard", "7.7.0", "0", "0", seed, Path.Combine(root, "invalid-count"));
    checks++;
    var duplicateSeed = Path.Combine(root, "duplicate.tsv");
    File.WriteAllLines(duplicateSeed, rows.Select(row => string.Join('\t', row)).Append(string.Join('\t', rows[0])));
    Invoke(false, "partition-shard", "7.7.0", "0", "4", duplicateSeed, Path.Combine(root, "duplicate-plan"));
    checks++;
    // Exercise guarded production-style post-generation edits against both retained RTI toolchains.
    foreach (var version in new[] { "7.3.1", "7.7.0" })
    {
        var referenceExclusions = Read(Path.Combine(repositoryRoot, "tests",
            "Wireloom.Dds.Generator.WireCompatibility", $"wire-case-exclusions-{version}.json"));
        Check(referenceExclusions["expectedFailures"]!.AsArray().All(item =>
            (string?)item!["id"] is not ("07-union-char-numeric" or "07-union-char-numeric-255")),
            "Passing adapted character cases retain no obsolete expected-failure entries.");
        foreach (var caseId in new[] { "07-union-char-numeric", "07-union-char-numeric-255" })
        {
            var referenceRoot = Path.Combine(repositoryRoot, "docs", "corpus", version, "rtiddsgen");
            var originalDirectory = Path.Combine(referenceRoot, "csharp", "features", caseId);
            var patchDirectory = Path.Combine(root, $"reference-{version}-{caseId}");
            Directory.CreateDirectory(patchDirectory);
            var originalSample = File.ReadAllText(Path.Combine(originalDirectory, $"{caseId}.cs"));
            var originalPlugin = File.ReadAllText(Path.Combine(originalDirectory, $"{caseId}Plugin.cs"));
            var samplePath = Path.Combine(patchDirectory, $"{caseId}.cs");
            var pluginPath = Path.Combine(patchDirectory, $"{caseId}Plugin.cs");
            File.WriteAllText(samplePath, originalSample);
            File.WriteAllText(pluginPath, originalPlugin);
            Invoke(true, "adapt-rti-characters", "csharp", caseId, patchDirectory);
            var adaptedSample = File.ReadAllText(samplePath);
            var adaptedPlugin = File.ReadAllText(pluginPath);
            var label = caseId.EndsWith("-255", StringComparison.Ordinal) ? "255" : "127";
            Check(adaptedSample.Contains($"Discriminator = (char){label};", StringComparison.Ordinal)
                && adaptedPlugin.Contains("sample.Setfallback(fallback, NativeChar.FromUtf8(_d));", StringComparison.Ordinal),
                "C# reference corrections compile numeric chars and preserve the incoming fallback discriminator.");
            Check(adaptedPlugin.Contains("new int[] { -1 }", StringComparison.Ordinal) == (label == "255"),
                "Only high character metadata uses signed-byte normalization.");
            Invoke(true, "adapt-rti-characters", "csharp", caseId, patchDirectory);
            Check(adaptedSample == File.ReadAllText(samplePath) && adaptedPlugin == File.ReadAllText(pluginPath),
                "Reapplying the reference correction is idempotent.");
            Check(originalSample == File.ReadAllText(Path.Combine(originalDirectory, $"{caseId}.cs"))
                && originalPlugin == File.ReadAllText(Path.Combine(originalDirectory, $"{caseId}Plugin.cs")),
                "Retained RTI captures are untouched.");

            File.WriteAllText(samplePath, originalSample);
            File.WriteAllText(pluginPath, originalPlugin.Replace("sample.fallback = fallback;", "/* unexpected generated shape */", StringComparison.Ordinal));
            var invalidPlugin = File.ReadAllText(pluginPath);
            Invoke(false, "adapt-rti-characters", "csharp", caseId, patchDirectory);
            Check(originalSample == File.ReadAllText(samplePath) && invalidPlugin == File.ReadAllText(pluginPath),
                "Unexpected generated code fails before either C# file is changed.");
        }

        var cppCase = "07-union-char-numeric-255";
        var cppDirectory = Path.Combine(root, $"cpp-reference-{version}");
        Directory.CreateDirectory(cppDirectory);
        var cppPath = Path.Combine(cppDirectory, "idl.cxx");
        var originalCpp = File.ReadAllText(Path.Combine(repositoryRoot, "docs", "corpus", version,
            "rtiddsgen", "cpp", "features", cppCase, $"{cppCase}.cxx"));
        File.WriteAllText(cppPath, originalCpp);
        Invoke(true, "adapt-rti-characters", "cpp", cppCase, cppDirectory);
        var adaptedCpp = File.ReadAllText(cppPath);
        Check(adaptedCpp == originalCpp.Replace("static_cast<int>(255)", "static_cast<int>(-1)", StringComparison.Ordinal),
            "The C++ adaptation changes only the native TypeCode label.");
        Invoke(true, "adapt-rti-characters", "cpp", cppCase, cppDirectory);
        Check(adaptedCpp == File.ReadAllText(cppPath), "The C++ metadata correction is idempotent.");
        File.WriteAllText(cppPath, originalCpp.Replace("static_cast<int>(255)", "unexpected-label", StringComparison.Ordinal));
        var invalidCpp = File.ReadAllText(cppPath);
        Invoke(false, "adapt-rti-characters", "cpp", cppCase, cppDirectory);
        Check(invalidCpp == File.ReadAllText(cppPath), "An unexpected C++ label fails without changing source.");
    }
    Invoke(false, "adapt-rti-characters", "csharp", "unrelated-case", root);
    Invoke(false, "adapt-rti-characters", "cpp", "07-union-char-numeric", root);
    Invoke(false, "adapt-rti-characters", "unknown-language", "07-union-char-numeric-255", root);
    checks += 3;
    Check(numericScenarios.All(item => new[] { "writer", "reader" }
        .Where(role => (string?)item![role]!["peer"] == "RTI C#")
        .All(role => ((string?)item![role]!["adaptation"] ?? "").Contains("Post-generation RTI C#", StringComparison.Ordinal))),
        "Every RTI C# numeric-character endpoint discloses its corrected generated binding.");
    Check(scalarMarkdown.Contains("internal TypeCode label from 255 to signed-byte -1", StringComparison.Ordinal)
        && scalarMarkdown.Contains("Setfallback", StringComparison.Ordinal),
        "Markdown reports disclose the C++ label and C# discriminator-preservation corrections.");
    Console.WriteLine($"Shard verification passed: {checks} checks, {rows.Count} exchanges, no RTI license required.");

    void Reject(string name, Action<string> mutate)
    {
        var directory = Path.Combine(root, name);
        CopyDirectory(source, directory);
        mutate(directory);
        Invoke(false, "merge-shards", rtiVersion, "4", directory, Path.Combine(root, name + "-output"), exclusions);
        checks++;
    }
}

finally
{
    // Only remove the unique temporary directory created by this verification.
    if (!Path.GetFullPath(root).StartsWith(Path.GetFullPath(temporaryRoot) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
    {
        throw new InvalidOperationException("Verification output is outside the temporary directory.");
    }
    Directory.Delete(root, recursive: true);
}

static string FindRepositoryRoot(string path)
{
    // CI builds the utility under RUNNER_TEMP, outside the checkout.
    foreach (var startPath in new[] { Environment.CurrentDirectory, Path.GetDirectoryName(Path.GetFullPath(path))! })
    {
        var directory = new DirectoryInfo(startPath);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "docs", "corpus", "manifest.json")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }
    }

    throw new DirectoryNotFoundException("Could not locate the repository corpus manifest.");
}
string Invoke(bool succeeds, params string[] command)
{
    var start = new ProcessStartInfo("dotnet") { RedirectStandardOutput = true, RedirectStandardError = true };
    start.ArgumentList.Add(tool);
    foreach (var argument in command)
    {
        start.ArgumentList.Add(argument);
    }
    using var process = Process.Start(start)!;
    var output = process.StandardOutput.ReadToEndAsync();
    var error = process.StandardError.ReadToEndAsync();
    if (!process.WaitForExit(30000))
    {
        process.Kill(entireProcessTree: true);
        throw new TimeoutException("Shard utility exceeded 30 seconds.");
    }
    Task.WaitAll(output, error);
    if ((process.ExitCode == 0) != succeeds)
    {
        throw new InvalidOperationException($"{command[0]} exit {process.ExitCode}: {output.Result} {error.Result}");
    }

    return output.Result + error.Result;
}

void Check(bool condition, string description)
{
    if (!condition)
    {
        throw new InvalidOperationException(description);
    }
    checks++;
}

JsonNode Read(string path) => JsonNode.Parse(File.ReadAllText(path))!;

void ChangeJson(string path, Action<JsonNode> change)
{
    var node = Read(path);
    change(node);
    File.WriteAllText(path, node.ToJsonString());
}

void ChangeRows(string directory, Func<string[], string[]> change)
{
    var path = Path.Combine(directory, "shard-0", "scenarios.tsv");
    File.WriteAllLines(path, change(File.ReadAllLines(path)));
}

void CopyDirectory(string source, string destination)
{
    Directory.CreateDirectory(destination);
    foreach (var file in Directory.GetFiles(source))
    {
        File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));
    }
    foreach (var directory in Directory.GetDirectories(source))
    {
        CopyDirectory(directory, Path.Combine(destination, Path.GetFileName(directory)));
    }
}
