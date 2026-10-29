#!/usr/bin/env dotnet
// Checks wire-matrix finalization and fixture selection without RTI peers or a license.
#:property NoWarn=CS7022%3BIL2026%3BIL3050
#:property RestorePackagesWithLockFile=false
using System.Diagnostics;

var repositoryRoot = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE") ?? FindRepositoryRoot();
var scripts = Path.Combine(repositoryRoot, "tests", "Wireloom.Dds.Generator.WireCompatibility", "scripts");
var scratch = Path.Combine(Path.GetTempPath(), "wireloom-harness-" + Guid.NewGuid().ToString("N"));
var matrixTools = Path.Combine(scratch, "matrix-tools.dll");

try
{
    Directory.CreateDirectory(scratch);
    await Run("dotnet", ["build", Path.Combine(scripts, "matrix-tools.cs"), "--output", scratch, "--verbosity", "quiet"]);
    await Run("dotnet", ["run", "--file", Path.Combine(scripts, "run-matrix.cs"), "--", "--verify-harness"]);

    VerifyFixtures("7.3.1", new Dictionary<string, string[]>(StringComparer.Ordinal)
    {
        ["05-array-of-sequences-idl"] = ["sequence-empty", "sequence-unequal", "sequence-mixed-empty", "sequence-boundary"],
        ["14-extensibility-full"] = ["non-default"],
        ["06-aggregates"] = ["non-default", "direct-recursive-sequence"],
        ["04-unbounded-string-aliases"] = ["string-empty", "string-over-default-bound"],
        ["05-unbounded-sequence-aliases"] = ["sequence-empty", "sequence-multiple"],
        ["14-autoid-scope"] = ["scope-nested"],
        ["14-annotation-arguments"] = ["default", "non-default"],
        ["14-member-declarators"] = ["non-default"],
        ["14-union-label-expressions"] = ["symbolic", "arithmetic", "hexadecimal"],
        ["14-union-scoped-enum"] = ["enum-selected", "enum-first"],
        ["14-union-array-branch"] = ["array-branch", "scalar-branch"],
        ["14-string-expression-bound"] = ["string-boundary"],
        ["14-typed-defaults"] = ["default", "non-default"],
        ["14-enum-constant"] = ["constant-and-label"],
    });

    VerifyFixtures("7.7.0", new Dictionary<string, string[]>(StringComparer.Ordinal)
    {
        ["14-typed-defaults"] = ["default", "non-default", "sparse-omitted"],
    });
    Console.WriteLine("C163–C172 fixture selection passed: 10 cases, 17 fixtures; sparse C171 is RTI 7.7-only.");
    return 0;
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception.Message);
    return 1;
}
finally
{
    if (Directory.Exists(scratch)) Directory.Delete(scratch, recursive: true);
}

void VerifyFixtures(string version, IReadOnlyDictionary<string, string[]> expectations)
{
    foreach (var (caseId, expected) in expectations)
    {
        var output = RunSync("dotnet", [matrixTools, "case-fixtures", caseId, version]);
        var actual = output.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (!actual.SequenceEqual(expected, StringComparer.Ordinal))
            throw new InvalidDataException($"Fixture selection for {caseId} on RTI {version} differed. Expected [{string.Join(", ", expected)}], got [{string.Join(", ", actual)}].");
    }

    if (version == "7.3.1") Console.WriteLine("Sequence-array fixture selection passed: 4 fixtures.");
}

async Task Run(string executable, IEnumerable<string> arguments)
{
    var result = await RunProcess(executable, arguments);
    if (result.ExitCode != 0) throw new InvalidOperationException($"{executable} failed: {result.Error.Trim()}");
    Console.Write(result.Output);
}

string RunSync(string executable, IEnumerable<string> arguments)
{
    var start = CreateStartInfo(executable, arguments);
    using var process = Process.Start(start) ?? throw new InvalidOperationException($"Could not start {executable}.");
    var output = process.StandardOutput.ReadToEnd();
    var error = process.StandardError.ReadToEnd();
    process.WaitForExit();
    if (process.ExitCode != 0) throw new InvalidOperationException($"{executable} failed: {error.Trim()}");
    return output;
}

async Task<ProcessResult> RunProcess(string executable, IEnumerable<string> arguments)
{
    var start = CreateStartInfo(executable, arguments);
    using var process = Process.Start(start) ?? throw new InvalidOperationException($"Could not start {executable}.");
    var output = process.StandardOutput.ReadToEndAsync();
    var error = process.StandardError.ReadToEndAsync();
    await process.WaitForExitAsync();
    return new ProcessResult(process.ExitCode, await output, await error);
}

ProcessStartInfo CreateStartInfo(string executable, IEnumerable<string> arguments)
{
    var start = new ProcessStartInfo(executable)
    {
        WorkingDirectory = repositoryRoot,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
    };
    foreach (var argument in arguments) start.ArgumentList.Add(argument);
    return start;
}

string FindRepositoryRoot()
{
    var directory = new DirectoryInfo(Environment.CurrentDirectory);
    while (directory is not null && !File.Exists(Path.Combine(directory.FullName, ".git"))) directory = directory.Parent;
    return directory?.FullName ?? throw new DirectoryNotFoundException("Could not locate the repository root.");
}

record ProcessResult(int ExitCode, string Output, string Error);
