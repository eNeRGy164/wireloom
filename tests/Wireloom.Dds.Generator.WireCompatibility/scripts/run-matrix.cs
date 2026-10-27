#!/usr/bin/env dotnet
// Orchestrates the licensed wire-compatibility matrix inside the RTI container.
// Requires the .NET 10 SDK selected by global.json.
#:property NoWarn=CS7022%3BIL2026%3BIL3050
#:property RestorePackagesWithLockFile=false
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

const int TimeoutSeconds = 20;
var root = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE") ?? FindRoot();
var testRoot = Path.Combine(root, "tests", "Wireloom.Dds.Generator.WireCompatibility");
var scripts = Path.Combine(testRoot, "scripts");
var scratch = Path.Combine(Environment.GetEnvironmentVariable("RUNNER_TEMP") ?? "/tmp", "wireloom-wire-compatibility");
var output = Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_OUTPUT_DIR") ?? Path.Combine(root, "artifacts", "wire-compatibility");
var version = Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_RTI_VERSION") ?? "7.7.0";
var compileOnly = Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_CPP_COMPILE_ONLY") == "1";
var casesFilter = Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_CASE_FILTER");
var project = Path.Combine(testRoot, "Wireloom.Dds.Generator.WireCompatibility.csproj");
var rtiProject = Path.Combine(testRoot, "RtiOracleProbe", "RtiOracleProbe.csproj");
var manifest = Path.Combine(root, "docs", "corpus", "manifest.json");
var exclusions = Path.Combine(testRoot, $"wire-case-exclusions-{version}.json");
var oracleManifest = Path.Combine(root, "docs", "corpus", version, "rtiddsgen", "wire-oracles.json");
var nativeTemplate = Path.Combine(testRoot, "Native");
var nuget = Environment.GetEnvironmentVariable("NUGET_PACKAGES") ?? Path.Combine(scratch, "nuget-packages");
var toolsDll = Path.Combine(scratch, "matrix-tools.dll");
var buildRunnerDll = Path.Combine(scratch, "wire-case-build-runner.dll");
var exchangeRunnerDll = Path.Combine(scratch, "wire-exchange-runner.dll");
var scenariosPath = Path.Combine(scratch, "scenarios.tsv");
var caseBuildQueuePath = Path.Combine(scratch, "case-build-queue.tsv");
var caseBuildResultsPath = Path.Combine(scratch, "case-build-results.tsv");
var managedQueuePath = Path.Combine(scratch, "managed-build-queue.tsv");
var managedResultsPath = Path.Combine(scratch, "managed-build-results.tsv");
var caseBuildsPath = Path.Combine(scratch, "case-builds.tsv");
var cppResultsPath = Path.Combine(scratch, "cpp-compilation.tsv");
var elapsed = Stopwatch.StartNew();
var workflowGroupOpen = false;
var overallStatus = 0;
var expectedCount = 0;
var reportWritten = false;
var resultCode = 0;

try
{
    if (args.SequenceEqual(["--verify-harness"]))
    {
        VerifyFinalizationPolicy();
        return 0;
    }
    if (version is not ("7.3.1" or "7.7.0"))
        throw new ArgumentException("The wire runner supports exact RTI 7.3.1 and 7.7.0 only.");

    InitializeRtiEnvironment();
    var license = Environment.GetEnvironmentVariable("RTI_LICENSE_FILE");
    var nddsHome = Environment.GetEnvironmentVariable("NDDSHOME");
    if (string.IsNullOrWhiteSpace(license) || !File.Exists(license))
        throw new ArgumentException("RTI_LICENSE_FILE must name a readable RTI license file.");
    if (string.IsNullOrWhiteSpace(nddsHome) || !Directory.Exists(Path.Combine(nddsHome, "include")) ||
        !Directory.Exists(Path.Combine(nddsHome, "resource", "cmake")))
        throw new ArgumentException("NDDSHOME must point to the selected RTI development libraries and CMake modules.");

    if (Directory.Exists(scratch)) Directory.Delete(scratch, recursive: true);
    Directory.CreateDirectory(scratch);
    Directory.CreateDirectory(Path.Combine(output, "logs"));
    foreach (var file in Directory.EnumerateFiles(Path.Combine(output, "logs"), "*.log")) File.Delete(file);

    var scenarioRows = new List<string>();
    var caseBuildRows = new List<string>();
    var nativeBuildRows = new List<string>();
    var cppRows = new List<string> { "case_id\tstatus\tdetail" };
    var managedStatuses = new Dictionary<string, (string Status, string Detail)>(StringComparer.Ordinal);
    File.WriteAllText(scenariosPath, string.Empty);
    File.WriteAllText(caseBuildQueuePath, string.Empty);
    File.WriteAllText(managedQueuePath, string.Empty);
    File.WriteAllText(cppResultsPath, "case_id\tstatus\tdetail" + Environment.NewLine);
    File.WriteAllText(Path.Combine(output, "results.json"), $$"""{"rtiVersion":"{{version}}","scenarios":[]}""");
    StartGroup("Prepare wire compatibility matrix");

    await BuildUtility(Path.Combine(scripts, "matrix-tools.cs"));
    if (!compileOnly) await BuildUtility(Path.Combine(scripts, "wire-exchange-runner.cs"));
    await BuildUtility(Path.Combine(scripts, "wire-case-build-runner.cs"));

    var cases = Lines(await Matrix("list-cases", manifest, exclusions));
    var rtiOnlyCases = Lines(await Matrix("list-rti-only-cases", exclusions));
    cases.AddRange(rtiOnlyCases);
    if (cases.Count == 0) throw new InvalidOperationException("Case discovery returned no wire-testable cases.");
    if (!string.IsNullOrWhiteSpace(casesFilter))
    {
        var requested = casesFilter.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var invalid = requested.FirstOrDefault(item => !cases.Contains(item, StringComparer.Ordinal));
        if (invalid is not null) throw new ArgumentException($"Unknown or inapplicable corpus case for RTI {version}: {invalid}");
        cases = requested.ToList();
    }

    if (!compileOnly)
    {
        var planOutput = await Matrix("create-plan", manifest, exclusions, version,
            string.IsNullOrEmpty(casesFilter) ? "-" : casesFilter,
            Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_SHARD_INDEX") ?? "0",
            Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_SHARD_COUNT") ?? "1", output);
        File.Copy(Path.Combine(output, "scenarios.tsv"), scenariosPath, overwrite: true);
        scenarioRows.AddRange(File.ReadAllLines(scenariosPath));
        cases = File.ReadAllLines(Path.Combine(output, "selected-cases.txt")).Where(line => line.Length > 0).ToList();
        expectedCount = scenarioRows.Count(line => line.Length > 0);
        Console.WriteLine($"Shard {Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_SHARD_INDEX") ?? "0"}/{Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_SHARD_COUNT") ?? "1"}: {cases.Count} cases, {expectedCount} scenario exchanges planned.");
    }
    else
    {
        Console.WriteLine($"RTI Connext {version}: compiling C++ peers for {cases.Count} cases (wire exchanges disabled).");
        File.WriteAllText(scenariosPath, string.Empty);
    }
    EndGroup();

    await Run("dotnet", ["restore", project, $"-p:WireCompatibilityRtiVersion={version}", $"-p:RtiConnextDdsVersion={version}", $"-p:RestorePackagesPath={nuget}", "--locked-mode", "--verbosity", "quiet"]);
    if (!compileOnly)
        await Run("dotnet", ["restore", rtiProject, $"-p:WireCompatibilityRtiVersion={version}", $"-p:RtiConnextDdsVersion={version}", $"-p:RestorePackagesPath={nuget}", "--locked-mode", "--verbosity", "quiet"]);

    var bootstrap = cases.FirstOrDefault(item => !rtiOnlyCases.Contains(item, StringComparer.Ordinal));
    if (bootstrap is not null)
    {
        var first = await CaseFields(bootstrap);
        var dir = Path.Combine(scratch, bootstrap);
        var bootstrapLog = Path.Combine(dir, "wireloom-csharp-build.log");
        Directory.CreateDirectory(dir);
        var bootstrapArgs = new List<string> { "build", project, "--no-restore", "--configuration", "Release", "-t:Rebuild",
            $"-p:WireCompatibilityCase={bootstrap}", $"-p:WireCompatibilityRtiVersion={version}", $"-p:RtiConnextDdsVersion={version}",
            $"-p:RestorePackagesPath={nuget}", $"-p:OutputPath={Path.Combine(dir, "wireloom-bin")}/",
            $"-p:WireCompatibilityIdl={Path.Combine(root, "docs", "corpus", first.Idl)}", $"-p:DdsIdlSequenceArrayMapping={first.Mapping}",
            $"-p:WireCompatibilityGeneratedOutputPath={Path.Combine(dir, "wireloom-generated")}" };
        if (bootstrap == "05-array-of-sequences") bootstrapArgs.Add("-p:WarningsNotAsErrors=DDSG0105");
        bootstrapArgs.AddRange(["--verbosity", "quiet"]);
        var result = await Run("dotnet", bootstrapArgs, bootstrapLog, allowFailure: true);
        managedStatuses[bootstrap] = (result.ExitCode == 0 ? "compiled" : "build-failed", result.ExitCode == 0 ? Path.Combine(dir, "wireloom-bin") : bootstrapLog);
        if (result.ExitCode != 0) overallStatus = 1;

        foreach (var caseId in cases.Where(item => item != bootstrap && !rtiOnlyCases.Contains(item, StringComparer.Ordinal)))
        {
            var fields = await CaseFields(caseId);
            var caseDir = Path.Combine(scratch, caseId);
            Directory.CreateDirectory(Path.Combine(caseDir, "wireloom-bin"));
            Directory.CreateDirectory(Path.Combine(caseDir, "wireloom-obj"));
            var row = string.Join('\t', caseId, Path.Combine(root, "docs", "corpus", fields.Idl),
                string.IsNullOrEmpty(fields.Mapping) ? "-" : fields.Mapping,
                Path.Combine(caseDir, "wireloom-bin"), Path.Combine(caseDir, "wireloom-generated"), Path.Combine(caseDir, "wireloom-obj"),
                Path.Combine(caseDir, "wireloom-csharp-build.log"), caseId == "05-array-of-sequences" ? "DDSG0105" : "-");
            File.AppendAllText(managedQueuePath, row + Environment.NewLine);
        }
    }

    if (File.Exists(managedQueuePath) && new FileInfo(managedQueuePath).Length > 0)
    {
        var runner = await Run("dotnet", [buildRunnerDll, "managed", "--builds", managedQueuePath, "--results", managedResultsPath,
            "--project", project, "--rti-version", version, "--restore-packages", nuget,
            "--project-extensions", Path.Combine(testRoot, "obj"), "--max-concurrency",
            Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_CSHARP_BUILD_CONCURRENCY") ?? "4"], allowFailure: true);
        if (runner.ExitCode > 1) throw new InvalidOperationException($"The C# managed peer build runner failed with exit code {runner.ExitCode}.");
        foreach (var row in ReadTsv(managedResultsPath))
        {
            managedStatuses[row[0]] = (row[1], row[2]);
            if (row[1] != "compiled") overallStatus = 1;
        }
        if (runner.ExitCode != 0) overallStatus = 1;
    }
    Console.WriteLine($"WIRE_MATRIX_PHASE managed-peer-build queued={ReadTsv(managedQueuePath).Count} concurrency={Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_CSHARP_BUILD_CONCURRENCY") ?? "4"} elapsed={elapsed.Elapsed.TotalSeconds:F0}s");

    for (var index = 0; index < cases.Count; index++)
    {
        var caseId = cases[index];
        StartGroup($"Case {index + 1}/{cases.Count}: {caseId}");
        Console.WriteLine($"WIRE_MATRIX_CASE {index + 1}/{cases.Count} case={caseId} phase=peer-prepare completed-scenarios=0/{expectedCount} elapsed={elapsed.Elapsed.TotalSeconds:F0}s");
        var caseDir = Path.Combine(scratch, caseId);
        var nativeDir = Path.Combine(caseDir, "native");
        var wireloomBin = Path.Combine(caseDir, "wireloom-bin");
        Directory.CreateDirectory(nativeDir);
        Directory.CreateDirectory(wireloomBin);
        var pairings = Lines(await Matrix("case-profile-pairings", manifest, caseId, version));
        var needsRtiCSharp = !compileOnly && pairings.Any(value => value.Contains("RTI C#", StringComparison.Ordinal));
        var wireloomAvailable = !rtiOnlyCases.Contains(caseId, StringComparer.Ordinal) && managedStatuses.GetValueOrDefault(caseId).Status == "compiled";
        var wireloomFailure = $"Wireloom C# build failed for {caseId}";
        var typeName = "-";
        var cppType = "CorpusOptionalAggregate::Holder";
        var idlRelative = "idl/features/09-optional-aggregate-member.idl";
        var defines = new List<string>();
        var generated = Path.Combine(caseDir, "wireloom-generated");
        if (!rtiOnlyCases.Contains(caseId, StringComparer.Ordinal))
        {
            if (!wireloomAvailable) ReportBuildFailure(wireloomFailure, managedStatuses.GetValueOrDefault(caseId).Detail);
            else await Matrix("generation-provenance", manifest, caseId, generated, version, Path.Combine(output, "logs", caseId + "-generation.json"));
            var registry = Path.Combine(caseDir, "case.json");
            await Matrix("discover-cases", manifest, generated, registry, caseId);
            var info = Lines(await Matrix("case-info", registry, manifest, caseId));
            typeName = info.ElementAtOrDefault(0) ?? "-";
            cppType = info.ElementAtOrDefault(1) ?? "-";
            idlRelative = info.ElementAtOrDefault(2) ?? throw new InvalidDataException($"Missing IDL path for {caseId}.");
            defines = info.Skip(3).Where(value => value.Length > 0).ToList();
        }

        var idlPath = Path.Combine(root, "docs", "corpus", idlRelative.Replace('/', Path.DirectorySeparatorChar));
        var wrapperName = caseId switch { "14-autoid-scope" => "14-autoid-scope-topic.idl", "14-enum-constant" => "14-enum-constant-topic.idl", _ => caseId + ".idl" };
        var wrapper = Path.Combine(testRoot, "IdlWrappers", wrapperName);
        if (File.Exists(wrapper)) idlPath = wrapper;

        var rtiAvailable = false;
        var rtiTypeName = "-";
        var rtiPeer = Path.Combine(caseDir, "rti-bin", "WireCompatibility.RtiOracleProbe.dll");
        if (needsRtiCSharp)
            (rtiAvailable, rtiTypeName) = await PrepareRtiCSharp(caseId, typeName, defines, caseDir, rtiPeer);

        var oracleSource = await Matrix("case-wire-oracle-source", oracleManifest, caseId, "rtiCpp");
        var cppOracle = oracleSource.Trim();
        if (caseId == "07-union-wchar-label") File.Copy(idlPath, Path.Combine(nativeDir, "idl.idl"), true);
        else if (caseId == "10-flat-data-binding") WriteAdaptedIdl(idlPath, Path.Combine(nativeDir, "idl.idl"), "@language_binding\\(FLAT_DATA\\)", "");
        else if ((await Matrix("case-field", manifest, caseId, "rti-cpp-idl-adaptations")).Contains(version + ":numeric-char-labels", StringComparison.Ordinal))
            WriteAdaptedIdl(idlPath, Path.Combine(nativeDir, "idl.idl"), @"case '\0':", "case 0:", "case '~':", "case 126:");
        else File.Copy(idlPath, Path.Combine(nativeDir, "idl.idl"), true);

        if (cppOracle.Length > 0)
        {
            var oracleDir = Path.Combine(root, "docs", "corpus", version, "rtiddsgen", cppOracle.Replace('/', Path.DirectorySeparatorChar));
            StageCppOracle(caseId, oracleDir, nativeDir, root);
            File.WriteAllText(Path.Combine(output, "logs", caseId + "-rti-cpp-oracle.log"),
                $"Using checked-in RTI C++ oracle ({cppOracle}; selection and adjustments are recorded in {Path.GetRelativePath(root, oracleManifest).Replace('\\', '/')}).{Environment.NewLine}");
        }
        else if (caseId != "07-union-wchar-label")
        {
            cppRows.Add($"{caseId}\tmissing-captured-oracle\t{oracleManifest}"); overallStatus = 1; EndGroup(); continue;
        }

        var cmakeOptions = CMakeOptions(caseId, version, cppType, nativeDir, cppOracle.Length == 0);
        var nativeMain = File.ReadAllText(Path.Combine(nativeTemplate, "main.cpp.in"))
            .Replace("@IDL_BASE@", "idl", StringComparison.Ordinal).Replace("@CPP_TYPE@", cmakeOptions.CppType, StringComparison.Ordinal);
        File.WriteAllText(Path.Combine(nativeDir, "main.cpp"), nativeMain);
        File.Copy(Path.Combine(nativeTemplate, "typed-fixture-peer.hpp.in"), Path.Combine(nativeDir, "typed-fixture-peer.hpp"), true);
        File.Copy(Path.Combine(nativeTemplate, "peer-qos.hpp"), Path.Combine(nativeDir, "peer-qos.hpp"), true);
        File.Copy(Path.Combine(nativeTemplate, "CMakeLists.txt"), Path.Combine(nativeDir, "CMakeLists.txt"), true);
        var cmakeArgs = new List<string> { "-S", nativeDir, "-B", Path.Combine(nativeDir, "build"), "-DCMAKE_MODULE_PATH=" + Path.Combine(Environment.GetEnvironmentVariable("NDDSHOME")!, "resource", "cmake"), "-DCONNEXTDDS_ARCH=" + (version == "7.3.1" ? "x64Linux4gcc7.3.0" : "x64Linux4gcc8.5.0") };
        cmakeArgs.AddRange(cmakeOptions.Options);
        var cmake = await Run("cmake", cmakeArgs, Path.Combine(caseDir, "cmake-configure.log"), allowFailure: true);
        if (cmake.ExitCode != 0) { cppRows.Add($"{caseId}\tcmake-configure-failed\tsee {caseDir}/cmake-configure.log"); overallStatus = 1; EndGroup(); continue; }
        nativeBuildRows.Add(string.Join('\t', caseId, Path.Combine(nativeDir, "build"), Path.Combine(caseDir, "cmake-build.log"), Path.Combine(nativeDir, "build", "wire-peer")));
        var metadata = string.Join('\t', caseId, typeName, rtiTypeName, cppType, caseDir, Path.Combine(wireloomBin, $"WireCompatibility.{caseId}.dll"), rtiPeer,
            wireloomAvailable ? "1" : "0", wireloomFailure, rtiAvailable ? "1" : "0", $"RTI C# peer is unavailable for {caseId}");
        File.WriteAllText(Path.Combine(caseDir, "build-metadata.tsv"), metadata);
        if (caseId.StartsWith("14-", StringComparison.Ordinal))
            foreach (var fixture in Lines(await Matrix("case-fixtures", caseId, version)))
            {
                await VerifyManagedFixture("wireloom-cs", Path.Combine(wireloomBin, $"WireCompatibility.{caseId}.dll"), typeName, caseId, fixture, caseDir);
                if (rtiAvailable) await VerifyManagedFixture("rti-cs", rtiPeer, rtiTypeName, caseId, fixture, caseDir);
            }
        EndGroup();
    }

    if (nativeBuildRows.Count > 0)
    {
        File.WriteAllLines(caseBuildQueuePath, nativeBuildRows);
        StartGroup("Build native peers concurrently");
        Console.WriteLine($"WIRE_MATRIX_PHASE native-peer-build queued={nativeBuildRows.Count} concurrency={Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_CASE_BUILD_CONCURRENCY") ?? "2"} elapsed={elapsed.Elapsed.TotalSeconds:F0}s");
        var build = await Run("dotnet", [buildRunnerDll, "--builds", caseBuildQueuePath, "--results", caseBuildResultsPath,
            "--max-concurrency", Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_CASE_BUILD_CONCURRENCY") ?? "2", "--jobs-per-build", "2"], allowFailure: true);
        if (build.ExitCode > 1) throw new InvalidOperationException($"The C# native peer build runner failed with exit code {build.ExitCode}.");
        foreach (var row in ReadTsv(caseBuildResultsPath))
        {
            var caseDir = Path.Combine(scratch, row[0]);
            if (row[1] == "compiled")
            {
                var metadata = File.ReadAllText(Path.Combine(caseDir, "build-metadata.tsv")).Split('\t');
                cppRows.Add($"{row[0]}\tcompiled\t{Path.Combine(caseDir, "native", "build", "wire-peer")}");
                caseBuildRows.Add(string.Join('\t', metadata[0], metadata[1], metadata[2], metadata[3], caseDir, metadata[5], metadata[6], Path.Combine(caseDir, "native", "build", "wire-peer"), metadata[7], metadata[8], metadata[9], metadata[10]));
            }
            else { overallStatus = 1; cppRows.Add($"{row[0]}\tcompile-failed\tsee {row[2]}"); }
        }
        if (build.ExitCode != 0) overallStatus = 1;
        EndGroup();
    }

    File.WriteAllLines(cppResultsPath, cppRows);
    File.WriteAllLines(caseBuildsPath, caseBuildRows);
    Console.WriteLine($"WIRE_MATRIX_PHASE peer-preparation elapsed={elapsed.Elapsed.TotalSeconds:F0}s wireloom-cases={cases.Count} native-peers={caseBuildRows.Count}");
    if (!compileOnly && caseBuildRows.Count > 0)
    {
        Console.WriteLine($"WIRE_MATRIX_PHASE exchange-run built-cases={caseBuildRows.Count} planned-exchanges={expectedCount} elapsed={elapsed.Elapsed.TotalSeconds:F0}s");
        var run = await Run("dotnet", [exchangeRunnerDll, "--scenarios", scenariosPath, "--case-builds", caseBuildsPath, "--rti-version", version,
            "--output-directory", output, "--scratch", scratch, "--timeout-seconds", TimeoutSeconds.ToString(CultureInfo.InvariantCulture),
            "--max-concurrency", Environment.GetEnvironmentVariable("WIRE_COMPATIBILITY_MAX_CONCURRENCY") ?? "16"], allowFailure: true);
        if (run.ExitCode == 1) overallStatus = 1;
        else if (run.ExitCode != 0) throw new InvalidOperationException($"The C# exchange runner failed with exit code {run.ExitCode}.");
    }

    if (compileOnly)
    {
        File.Copy(scenariosPath, Path.Combine(output, "scenarios.tsv"), true);
        File.Copy(cppResultsPath, Path.Combine(output, "cpp-compilation.tsv"), true);
        var compiled = cppRows.Count(row => row.Split('\t').ElementAtOrDefault(1) == "compiled");
        Console.WriteLine($"RTI C++ compile sweep: {compiled} compiled, {cppRows.Count - 1 - compiled} failed; see {output}/cpp-compilation.tsv.");
        await WriteReport();
        reportWritten = true;
        resultCode = FinalizationOutcome.Resolve(0, 0, overallStatus, compileOnly).ExitCode;
    }
    else
    {
        var scenarioLines = File.ReadAllLines(scenariosPath).Where(line => line.Length > 0).ToArray();
        var passed = scenarioLines.Count(line => line.Split('\t').ElementAtOrDefault(8) == "PASS");
        var failed = scenarioLines.Count(line => line.Split('\t').ElementAtOrDefault(8) == "FAIL");
        if (scenarioLines.Length != expectedCount) { Console.Error.WriteLine($"Expected {expectedCount} scenarios but recorded {scenarioLines.Length}."); overallStatus = 1; }
        Console.WriteLine(overallStatus == 0 ? $"Matrix completed without scenario or peer-build issues in {elapsed.Elapsed.TotalSeconds:F0}s." : $"Matrix completed with scenario or peer-build issues in {elapsed.Elapsed.TotalSeconds:F0}s; see the generated report.");
        await WriteReport();
        reportWritten = true;
        var outcome = FinalizationOutcome.Resolve(0, 0, overallStatus, compileOnly);
        if (outcome.Annotation == "warning") WorkflowCommand("warning", "Wire compatibility matrix completed with issues", $"{passed} of {expectedCount} scenario exchanges passed; {failed} failed; see the generated report.");
        else WorkflowCommand("notice", "Wire compatibility matrix passed", $"{passed} of {expectedCount} scenario exchanges passed in {elapsed.Elapsed.TotalSeconds:F0}s.");
        resultCode = outcome.ExitCode;
    }
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex is ArgumentException ? ex.Message : $"Wire matrix setup failed: {ex.Message}");
    WorkflowCommand("error", "Wire compatibility matrix setup failed", ex.Message);
    resultCode = FinalizationOutcome.Resolve(2, 0, overallStatus, compileOnly).ExitCode;
}

EndGroup();
if (!reportWritten)
{
    try
    {
        await WriteReport();
        reportWritten = true;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to generate the wire-compatibility report: {ex.Message}");
        WorkflowCommand("error", "Wire compatibility report generation failed", ex.Message);
        resultCode = FinalizationOutcome.Resolve(resultCode, 1, overallStatus, compileOnly).ExitCode;
    }
}
return resultCode;

async Task WriteReport()
{
    if (!Directory.Exists(output) || !File.Exists(scenariosPath)) return;
    File.Copy(scenariosPath, Path.Combine(output, "scenarios.tsv"), true);
    if (compileOnly)
    {
        if (File.Exists(cppResultsPath)) File.Copy(cppResultsPath, Path.Combine(output, "cpp-compilation.tsv"), true);
        return;
    }
    if (File.Exists(toolsDll))
        await Run("dotnet", [toolsDll, "render-report", version, expectedCount.ToString(CultureInfo.InvariantCulture), scenariosPath,
            Path.Combine(output, "results.json"), Path.Combine(output, "results.md"), Path.Combine(output, "logs"), exclusions, manifest], allowFailure: true);
}

async Task BuildUtility(string source)
{
    var result = await Run("dotnet", ["build", source, "--output", scratch, "--verbosity", "quiet"], allowFailure: true);
    if (result.ExitCode != 0) throw new InvalidOperationException($"Failed to build {Path.GetFileName(source)}.");
}

async Task<(bool Available, string TypeName)> PrepareRtiCSharp(string caseId, string typeName, List<string> defines, string caseDir, string peerPath)
{
    var oracle = (await Matrix("case-wire-oracle-source", oracleManifest, caseId, "rtiCSharp")).Trim();
    if (oracle.Length == 0) { overallStatus = 1; Console.Error.WriteLine($"No checked-in RTI C# wire oracle is available for {caseId}; refusing to generate during wire execution."); return (false, "-"); }
    var sourceDir = Path.Combine(root, "docs", "corpus", version, "rtiddsgen", oracle.Replace('/', Path.DirectorySeparatorChar));
    var generatedDir = Path.Combine(caseDir, "rti-csharp-generated");
    Directory.CreateDirectory(generatedDir);
    CopyDirectory(sourceDir, generatedDir);
    File.WriteAllText(Path.Combine(output, "logs", caseId + "-rti-csharp-oracle.log"),
        $"Using checked-in RTI C# oracle ({oracle}; selection and adjustments are recorded in {Path.GetRelativePath(root, oracleManifest).Replace('\\', '/')}).{Environment.NewLine}");
    var registry = Path.Combine(caseDir, "rti-oracle.json");
    var discover = await Run("dotnet", [toolsDll, "discover-rti", manifest, caseId, typeName, generatedDir, registry], allowFailure: true);
    if (discover.ExitCode != 0) { overallStatus = 1; return (false, "-"); }
    using var json = JsonDocument.Parse(File.ReadAllText(registry));
    var rtiType = json.RootElement.GetProperty("rtiCSharpType").GetString() ?? "-";
    var rtiBin = Path.GetDirectoryName(peerPath)!;
    Directory.CreateDirectory(rtiBin);
    var build = await Run("dotnet", ["build", rtiProject, "--no-restore", "--configuration", "Release", "--verbosity", "quiet",
        $"-p:RtiOracleSourceDir={generatedDir}", $"-p:WireCompatibilityCase={caseId}", $"-p:BaseIntermediateOutputPath={Path.Combine(caseDir, "rti-obj")}/",
        $"-p:MSBuildProjectExtensionsPath={Path.Combine(testRoot, "RtiOracleProbe", "obj")}/", $"-p:RestorePackagesPath={nuget}", $"-p:OutputPath={rtiBin}/"],
        Path.Combine(caseDir, "rti-csharp-build.log"), allowFailure: true);
    if (build.ExitCode != 0) { overallStatus = 1; ReportBuildFailure($"RTI-generated C# peer failed to compile for {caseId}", Path.Combine(caseDir, "rti-csharp-build.log")); return (false, rtiType); }
    Console.WriteLine($"RTI-generated C# peer compiled: {rtiType}");
    return (true, rtiType);
}

async Task VerifyManagedFixture(string label, string dll, string type, string caseId, string fixture, string caseDir)
{
    if (type == "-" || !File.Exists(dll)) return;
    var args = new List<string> { dll, "--case", caseId, "--type", type, "--topic", "fixture-validation-" + caseId, "--runtime-version", version, "--role", "writer", "--fixture", fixture, "--fixture-only", "true" };
    var result = await Run("dotnet", args, Path.Combine(caseDir, $"fixture-verify-{label}-{fixture}.log"), timeout: TimeSpan.FromSeconds(TimeoutSeconds + 5), allowFailure: true);
    if (result.ExitCode != 0) overallStatus = 1; else Console.WriteLine($"{label} fixture verified: {fixture}");
}

async Task<(string Idl, string Mapping)> CaseFields(string caseId)
{
    var idl = (await Matrix("case-field", manifest, caseId, "idl")).Trim();
    var mapping = (await Matrix("case-field", manifest, caseId, "sequence-array-mapping")).Trim();
    return (idl, mapping);
}

async Task<string> Matrix(params string[] arguments) => (await Run("dotnet", [toolsDll, .. arguments])).Output.TrimEnd('\r', '\n');

async Task<ProcessResult> Run(string executable, IEnumerable<string> arguments, string? logPath = null, TimeSpan? timeout = null, bool allowFailure = false)
{
    var start = new ProcessStartInfo(executable) { WorkingDirectory = root, RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false };
    foreach (var argument in arguments) start.ArgumentList.Add(argument);
    using var process = Process.Start(start) ?? throw new InvalidOperationException($"Could not start {executable}.");
    var stdout = process.StandardOutput.ReadToEndAsync();
    var stderr = process.StandardError.ReadToEndAsync();
    using var cts = new CancellationTokenSource(timeout ?? TimeSpan.FromMinutes(30));
    try { await process.WaitForExitAsync(cts.Token); }
    catch (OperationCanceledException) { process.Kill(entireProcessTree: true); throw new TimeoutException($"{executable} exceeded its timeout."); }
    var result = new ProcessResult(process.ExitCode, await stdout, await stderr);
    if (logPath is not null) await File.WriteAllTextAsync(logPath, result.Output + result.Error);
    if (result.Output.Length > 0) Console.Write(result.Output);
    if (result.ExitCode != 0 && !allowFailure)
    {
        Console.Error.Write(result.Error);
        throw new InvalidOperationException($"{executable} failed with exit code {result.ExitCode}.");
    }
    return result;
}

void InitializeRtiEnvironment()
{
    if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("NDDSHOME"))) return;
    var bash = new ProcessStartInfo("bash") { WorkingDirectory = root, RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false };
    bash.ArgumentList.Add("-lc");
    bash.ArgumentList.Add("eval \"$(rtienv -l \"$RTI_LICENSE_FILE\")\" && env -0");
    using var process = Process.Start(bash) ?? throw new InvalidOperationException("Could not initialize the RTI environment.");
    var stdout = process.StandardOutput.ReadToEnd();
    var stderr = process.StandardError.ReadToEnd();
    process.WaitForExit();
    if (process.ExitCode != 0) throw new InvalidOperationException("RTI environment setup failed: " + stderr.Trim());
    foreach (var entry in stdout.Split('\0', StringSplitOptions.RemoveEmptyEntries))
    {
        var separator = entry.IndexOf('=');
        if (separator > 0) Environment.SetEnvironmentVariable(entry[..separator], entry[(separator + 1)..]);
    }
}

void StageCppOracle(string caseId, string source, string destination, string repo)
{
    foreach (var file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories).Where(file => Path.GetExtension(file) is ".cxx" or ".hpp"))
    {
        var relative = Path.GetRelativePath(source, file);
        var name = Path.GetFileName(relative);
        string staged;
        if (!source.Contains("wire-wrapped", StringComparison.Ordinal) && DirectoryNameOrDot(relative) == ".")
            staged = RenameOracle(name, caseId);
        else if (relative is "idl.cxx" or "idl.hpp" or "idlPlugin.cxx" or "idlPlugin.hpp") staged = RenameOracle(name, caseId);
        else if (DirectoryNameOrDot(relative) == ".")
        {
            var stem = Path.GetFileNameWithoutExtension(name);
            if (stem.EndsWith("Plugin", StringComparison.Ordinal)) stem = stem[..^"Plugin".Length];
            staged = File.Exists(Path.Combine(repo, "docs", "corpus", "idl", "features", stem + ".idl")) ? Path.Combine("features", name) :
                File.Exists(Path.Combine(repo, "docs", "corpus", "idl", "includes", stem + ".idl")) ? Path.Combine("includes", name) : name;
        }
        else staged = relative;
        var target = Path.Combine(destination, staged);
        Directory.CreateDirectory(Path.GetDirectoryName(target)!);
        File.Copy(file, target, true);
    }
    if (!source.Contains("wire-wrapped", StringComparison.Ordinal) && caseId != "05-array-of-sequences-idl")
        foreach (var file in Directory.EnumerateFiles(destination, "*", SearchOption.TopDirectoryOnly).Where(file => Path.GetExtension(file) is ".cxx" or ".hpp"))
        {
            var text = File.ReadAllText(file).Replace($"\"{caseId}Plugin.hpp\"", "\"idlPlugin.hpp\"", StringComparison.Ordinal).Replace($"\"{caseId}.hpp\"", "\"idl.hpp\"", StringComparison.Ordinal);
            File.WriteAllText(file, text);
        }
}

(string CppType, List<string> Options) CMakeOptions(string caseId, string rti, string cppType, string nativeDir, bool noOracle)
{
    var options = new List<string>();
    if (rti == "7.3.1") options.Add("-DWIRELOOM_RTI_7_3_1=ON");
    var ccache = FindExecutable("ccache");
    if (ccache is not null) options.Add("-DCMAKE_CXX_COMPILER_LAUNCHER=" + ccache);
    var type = cppType;
    if (caseId == "07-union-wchar-label") { type = "dds::core::xtypes::DynamicData"; options.Add("-DWIRELOOM_DYNAMIC_WCHAR_UNION=ON"); }
    var flags = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["09-optional-string-sequences"]="WIRELOOM_TYPED_STRING_SEQUENCE_FIXTURE", ["04-strings"]="WIRELOOM_TYPED_WIDE_STRINGS_FIXTURE",
        ["04-unbounded-string-aliases"]="WIRELOOM_TYPED_UNBOUNDED_STRING_ALIASES", ["10-resolve-name-wstring-aliases"]="WIRELOOM_TYPED_RESOLVE_NAME_WSTRING_ALIASES",
        ["10-default-literal-enum-array"]="WIRELOOM_TYPED_DEFAULT_LITERAL_ENUM_ARRAY", ["07-union-char-numeric"]="WIRELOOM_TYPED_NUMERIC_CHAR_UNION",
        ["07-union-char-numeric-255"]="WIRELOOM_TYPED_NUMERIC_CHAR_UNION", ["04-string-alias-chains"]="WIRELOOM_TYPED_STRING_ALIAS_CHAINS",
        ["05-unbounded-sequence-aliases"]="WIRELOOM_TYPED_UNBOUNDED_SEQUENCE_ALIASES", ["05-array-of-sequences-idl"]="WIRELOOM_TYPED_SEQUENCE_ARRAYS",
        ["07-union-string-sequence-aliases"]="WIRELOOM_TYPED_UNION_STRING_SEQUENCE_ALIASES", ["07-union-string-sequence-bounded-direct"]="WIRELOOM_TYPED_UNION_STRING_SEQUENCE_BOUNDED",
        ["14-extensibility-full"]="WIRELOOM_TYPED_C163_EXTENSIBILITY", ["14-autoid-scope"]="WIRELOOM_TYPED_C164_AUTOID_SCOPE",
        ["14-annotation-arguments"]="WIRELOOM_TYPED_C165_ANNOTATION_ARGUMENTS", ["14-member-declarators"]="WIRELOOM_TYPED_C166_MEMBER_DECLARATORS",
        ["14-union-label-expressions"]="WIRELOOM_TYPED_C167_UNION_LABELS", ["14-union-scoped-enum"]="WIRELOOM_TYPED_C168_SCOPED_ENUM",
        ["14-union-array-branch"]="WIRELOOM_TYPED_C169_UNION_ARRAY", ["14-string-expression-bound"]="WIRELOOM_TYPED_C170_STRING_BOUND",
        ["14-typed-defaults"]="WIRELOOM_TYPED_C171_DEFAULTS", ["14-enum-constant"]="WIRELOOM_TYPED_C172_ENUM_CONSTANT"
    };
    if (flags.TryGetValue(caseId, out var flag)) options.Add("-D" + flag + "=ON");
    if (caseId == "07-union-char-numeric-255") options.Add("-DWIRELOOM_UNSIGNED_CHAR_NUMERIC_UNION=ON");
    if (caseId == "04-boundaries") options.Add("-DWIRELOOM_TYPED_STRING_BOUNDARIES_FIXTURE=ON");
    if (caseId == "09-optional-aggregate-member") options.Add("-DWIRELOOM_TYPED_OPTIONAL_AGGREGATE_FIXTURE=ON");
    if (caseId == "09-optional-recursive-array") options.Add("-DWIRELOOM_TYPED_OPTIONAL_RECURSIVE_ARRAY_FIXTURE=ON");
    if (caseId == "06-aggregates") File.Copy(Path.Combine(nativeTemplate, "recursive-sequence-peer.cpp"), Path.Combine(nativeDir, "recursive-sequence-peer.cpp"), true);
    if (caseId == "09-optional-recursive-array" && noOracle)
    {
        var cpp = Path.Combine(nativeDir, "idl.cxx");
        var source = File.ReadAllText(cpp);
        if (!source.Contains("Node_g_tc_children_array._data._typeCode", StringComparison.Ordinal))
            throw new InvalidDataException("RTI generated recursive array TypeCode initializer changed; update its guarded workaround.");
        EditCpp(cpp, text => Regex.Replace(text,
            @"(Node_g_tc_children_array\._data\._typeCode[^\r\n]*?)::get\(\)\.native\(\)", "$1::get(&node).native()"));
        if (rti == "7.3.1") EditCpp(cpp, text => text.Replace("dynamic_type< Node>", "dynamic_type< ::CorpusOptionalRecursiveArray::Node>", StringComparison.Ordinal));
    }
    if (caseId == "06-aggregates" && rti == "7.3.1" && noOracle)
        EditCpp(Path.Combine(nativeDir, "idl.cxx"), text => text.Replace("dynamic_type< Recursive>", "dynamic_type< ::CorpusAggregates::Recursive>", StringComparison.Ordinal));
    if (caseId == "10-resolve-name-wstring-aliases" && noOracle)
    {
        var cpp = File.ReadAllText(Path.Combine(nativeDir, "idl.cxx"));
        if (Count(cpp, "dynamic_type< BoundedWideText>") != 1 || Count(cpp, "dynamic_type< BoundedWideTextAlias>") != 1)
            throw new InvalidDataException($"RTI C++ generated TypeCode references changed for {caseId}; update the guarded alias-tag workaround.");
        EditCpp(Path.Combine(nativeDir, "idl.cxx"), text => text
            .Replace("dynamic_type< BoundedWideText>", "dynamic_type< ::CorpusResolveNameWstringAliases::BoundedWideText_AliasTag_t>", StringComparison.Ordinal)
            .Replace("dynamic_type< BoundedWideTextAlias>", "dynamic_type< ::CorpusResolveNameWstringAliases::BoundedWideTextAlias_AliasTag_t>", StringComparison.Ordinal));
    }
    if (caseId == "07-union-char-numeric-255" && noOracle)
        RunSync("dotnet", [toolsDll, "adapt-rti-characters", "cpp", caseId, nativeDir]);
    return (type, options);
}

void WriteAdaptedIdl(string source, string target, params string[] replacements)
{
    var content = File.ReadAllText(source);
    for (var i = 0; i < replacements.Length; i += 2)
        content = replacements[i].StartsWith("@", StringComparison.Ordinal)
            ? Regex.Replace(content, replacements[i][1..], replacements[i + 1])
            : content.Replace(replacements[i], replacements[i + 1], StringComparison.Ordinal);
    File.WriteAllText(target, content);
}

void ReportBuildFailure(string label, string? path)
{
    Console.Error.WriteLine($"{label}; see {path}");
    if (path is null || !File.Exists(path)) return;
    var match = Regex.Match(File.ReadAllText(path), @": error (CS[0-9]+: .*) \[.*$", RegexOptions.Multiline);
    if (match.Success) Console.Error.WriteLine(match.Groups[1].Value);
}

void VerifyFinalizationPolicy()
{
    Check(FinalizationOutcome.Resolve(7, 0, 1, false) == new FinalizationOutcome(7, "error"), "A late setup failure remains a failure.");
    Check(FinalizationOutcome.Resolve(0, 0, 1, false) == new FinalizationOutcome(0, "warning"), "A handled peer failure is reported without failing the workflow.");
    Check(FinalizationOutcome.Resolve(7, 1, 1, false) == new FinalizationOutcome(1, "error"), "A report failure remains visible and fails the workflow.");
    Check(FinalizationOutcome.Resolve(0, 0, 1, true) == new FinalizationOutcome(1, "error"), "A C++ compile sweep fails when a peer fails to compile.");
    Console.WriteLine("Harness finalization passed: 4 checks, no RTI license required.");
}

void Check(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}

void EditCpp(string path, Func<string, string> edit) => File.WriteAllText(path, edit(File.ReadAllText(path)));
void RunSync(string exe, IEnumerable<string> argv) { var p = new ProcessStartInfo(exe) { WorkingDirectory = root }; foreach (var arg in argv) p.ArgumentList.Add(arg); using var proc = Process.Start(p)!; proc.WaitForExit(); if (proc.ExitCode != 0) throw new InvalidOperationException($"{exe} failed with exit code {proc.ExitCode}."); }
void StartGroup(string name) { if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true" && !workflowGroupOpen) { Console.WriteLine("::group::" + Escape(name)); workflowGroupOpen = true; } }
void EndGroup() { if (workflowGroupOpen) { Console.WriteLine("::endgroup::"); workflowGroupOpen = false; } }
void WorkflowCommand(string level, string title, string message) { if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true") Console.WriteLine($"::{level} title={Escape(title)}::{Escape(message)}"); }
string Escape(string value) => value.Replace("%", "%25").Replace("\r", "%0D").Replace("\n", "%0A");
string FindRoot() { var dir = new DirectoryInfo(Environment.CurrentDirectory); while (dir is not null && !File.Exists(Path.Combine(dir.FullName, ".git"))) dir = dir.Parent; return dir?.FullName ?? throw new DirectoryNotFoundException("Could not find the Git repository root."); }
string? FindExecutable(string name) { var path = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator).Select(dir => Path.Combine(dir, name)).FirstOrDefault(File.Exists); return path; }
string DirectoryNameOrDot(string path) => string.IsNullOrEmpty(Path.GetDirectoryName(path)) ? "." : Path.GetDirectoryName(path)!;
int Count(string text, string value) => Regex.Matches(text, Regex.Escape(value)).Count;
List<string> Lines(string value) => value.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(line => line.TrimEnd('\r')).ToList();
List<string[]> ReadTsv(string path) => File.Exists(path) ? File.ReadAllLines(path).Where(line => line.Length > 0).Select(line => line.Split('\t')).ToList() : [];
string RenameOracle(string name, string caseId) => name switch { var x when x == caseId + ".cxx" => "idl.cxx", var x when x == caseId + ".hpp" => "idl.hpp", var x when x == caseId + "Plugin.cxx" => "idlPlugin.cxx", var x when x == caseId + "Plugin.hpp" => "idlPlugin.hpp", _ => name };
void CopyDirectory(string source, string destination) { foreach (var dir in Directory.EnumerateDirectories(source, "*", SearchOption.AllDirectories)) Directory.CreateDirectory(Path.Combine(destination, Path.GetRelativePath(source, dir))); foreach (var file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories)) { var target = Path.Combine(destination, Path.GetRelativePath(source, file)); Directory.CreateDirectory(Path.GetDirectoryName(target)!); File.Copy(file, target, true); } }
record ProcessResult(int ExitCode, string Output, string Error);

record FinalizationOutcome(int ExitCode, string Annotation)
{
    internal static FinalizationOutcome Resolve(int runStatus, int reportStatus, int overallStatus, bool compileOnly)
    {
        if (reportStatus != 0) return new FinalizationOutcome(1, "error");
        if (runStatus != 0) return new FinalizationOutcome(runStatus, "error");
        if (compileOnly && overallStatus != 0) return new FinalizationOutcome(1, "error");
        return overallStatus == 0 ? new FinalizationOutcome(0, "notice") : new FinalizationOutcome(0, "warning");
    }
}
