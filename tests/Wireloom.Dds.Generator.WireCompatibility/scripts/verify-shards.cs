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
var root = Path.Combine(Path.GetTempPath(), "wireloom-shards-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(root);
var checks = 0;
try
{
    // Arrange: unequal case sizes, all nine pairings, and an RTI-only case.
    var peers = new[] { "Wireloom C#", "RTI C#", "RTI C++" };
    var cases = new Dictionary<string, string[]>
    {
        ["05-array-of-sequences"] = ["default"],
        ["05-collections"] = ["sequence-empty", "sequence-single", "sequence-multiple"],
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

                rows.Add([item.Key, "Managed.Type", "Oracle.Type", "::Native::Type", writer, "7.7.0", reader, "7.7.0", status, fixture]);
            }
        }
    }
    var seed = Path.Combine(root, "all.tsv");
    File.WriteAllLines(seed, rows.Select(row => string.Join('\t', row)));
    var exclusions = Path.Combine(root, "exclusions.json");
    File.WriteAllText(exclusions, """
        {
          "notImplementedCases": [{"id":"09-optional-aggregate-member","diagnostic":"TEST","reason":"RTI-only control","issue":"https://github.com/eNeRGy164/wireloom/issues/15"}],
          "expectedFailures": [{"id":"05-array-of-sequences","writer":"Wireloom C#","reader":"RTI C++","reason":"registered failure"}]
        }
        """);
    var source = Path.Combine(root, "shards");
    for (var index = 0; index < 4; index++)
    {
        var directory = Path.Combine(source, $"shard-{index}");
        Invoke(true, "partition-shard", "7.7.0", index.ToString(), "4", seed, directory);
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
        Invoke(true, "render-report", "7.7.0", count, Path.Combine(directory, "scenarios.tsv"),
            Path.Combine(directory, "results.json"), Path.Combine(directory, "results.md"), logs, exclusions);
    }

    // Act: compare the merged report with an unsharded rendering of the same rows/logs.
    var merged = Path.Combine(root, "merged");
    Invoke(true, "merge-shards", "7.7.0", "4", source, merged, exclusions);
    var baseline = Path.Combine(root, "baseline.json");
    Invoke(true, "render-report", "7.7.0", rows.Count.ToString(), seed, baseline,
        Path.Combine(root, "baseline.md"), Path.Combine(merged, "logs"), exclusions);

    // Assert: payload/report evidence survives, including registry entries and failure reasons.
    Check(JsonNode.DeepEquals(Read(baseline), Read(Path.Combine(merged, "results.json"))), "Combined JSON equals the unsharded report.");
    var recursiveScenarios = Read(baseline)["scenarios"]!.AsArray()
        .Where(item => (string?)item!["case"] == "09-optional-recursive-array").ToArray();
    Check(recursiveScenarios.Length == 27, "All recursive array exchanges are retained.");
    foreach (var scenario in recursiveScenarios)
    {
        foreach (var role in new[] { "writer", "reader" })
        {
            var peer = scenario![role]!;
            var adaptation = (string?)peer["adaptation"] ?? "";
            Check(adaptation.Contains("Bootstrap", StringComparison.Ordinal) == ((string?)peer["peer"] == "RTI C#"),
                "Only the RTI C# reference reports a recursive support bootstrap.");
        }
    }

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
    Invoke(true, "partition-shard", "7.7.0", "0", "4", seed, repeat);
    Check(JsonNode.DeepEquals(Read(Path.Combine(source, "shard-0", "plan.json")), Read(Path.Combine(repeat, "plan.json"))), "Independent plans are deterministic.");

    Reject("missing-shard", directory => Directory.Move(Path.Combine(directory, "shard-1"), Path.Combine(root, "removed-shard")));
    Reject("duplicate-index", directory => ChangeJson(Path.Combine(directory, "shard-1", "shard.json"), node => node["shardIndex"] = 0));
    Reject("different-plan", directory => ChangeJson(Path.Combine(directory, "shard-1", "plan.json"), node => node["expectedScenarioCount"] = 999));
    Reject("wrong-version", directory => ChangeJson(Path.Combine(directory, "shard-1", "shard.json"), node => node["rtiVersion"] = "7.3.1"));
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
    Console.WriteLine($"Shard verification passed: {checks} checks, {rows.Count} exchanges, no RTI license required.");

    void Reject(string name, Action<string> mutate)
    {
        var directory = Path.Combine(root, name);
        CopyDirectory(source, directory);
        mutate(directory);
        Invoke(false, "merge-shards", "7.7.0", "4", directory, Path.Combine(root, name + "-output"), exclusions);
        checks++;
    }
}
finally
{
    // Only remove the unique temporary directory created by this verification.
    if (!Path.GetFullPath(root).StartsWith(Path.GetFullPath(Path.GetTempPath()), StringComparison.OrdinalIgnoreCase))
    {
        throw new InvalidOperationException("Verification output is outside the temporary directory.");
    }
    Directory.Delete(root, recursive: true);
}

void Invoke(bool succeeds, params string[] command)
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
