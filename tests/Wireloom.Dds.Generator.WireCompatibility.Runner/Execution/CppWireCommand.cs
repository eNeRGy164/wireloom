using System.Security.Cryptography;
using System.Text.Json;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Runs native C++ corpus wire scenarios using a staged generator package.</summary>
internal static class CppWireCommand
{
    private const string Corpus = "docs/corpus";
    private static readonly JsonSerializerOptions EvidenceJson = new()
    {
        WriteIndented = true
    };

    /// <summary>Builds and runs the selected licensed C++ corpus fixture.</summary>
    internal static async Task<int> RunAsync(string[] arguments)
    {
        switch (arguments)
        {
            case ["wire", var evidencePath, var packagePrefix]:
                return await RunWireAsync(new CppWireCase("10-annotations", "CppSpike", "cpp-peer"),
                    Path.GetFullPath(evidencePath), Path.GetFullPath(packagePrefix));
            case ["01-primitives", "wire", var evidencePath, var packagePrefix]:
                return await RunWireAsync(new CppWireCase("01-primitives", "CppFirstCase", "cpp-first-case"),
                    Path.GetFullPath(evidencePath), Path.GetFullPath(packagePrefix));
            case ["10-annotations", "wire", var evidencePath, var packagePrefix]:
                return await RunWireAsync(new CppWireCase("10-annotations", "CppSpike", "cpp-peer"),
                    Path.GetFullPath(evidencePath), Path.GetFullPath(packagePrefix));
            default:
                throw new ArgumentException("Use --cpp-wire <01-primitives|10-annotations> wire <evidence-directory> <package-prefix>.");
        }
    }

    private static async Task<int> RunWireAsync(CppWireCase wireCase, string evidencePath, string packagePrefix)
    {
        var repository = Directory.GetCurrentDirectory();
        var idl = $"{Corpus}/idl/features/{wireCase.Id}.idl";
        var native = $"tests/Wireloom.Dds.Generator.WireCompatibility/Native/{wireCase.NativeDirectory}";
        var retained = $"{Corpus}/7.7.0/generator/cpp/features/{wireCase.Id}";
        var oracle = $"{Corpus}/7.7.0/rtiddsgen/cpp/features/{wireCase.Id}";
        if (!File.Exists(Path.Combine(repository, idl)))
        {
            throw new InvalidOperationException("Run C++ wire verification from the repository root.");
        }
        if (Directory.Exists(evidencePath) && Directory.EnumerateFileSystemEntries(evidencePath).Any())
        {
            throw new InvalidOperationException("C++ wire verification requires an empty evidence directory.");
        }
        Directory.CreateDirectory(evidencePath);

        new RtiEnvironment(repository).Initialize();
        var nddsHome = Environment.GetEnvironmentVariable("NDDSHOME")!;
        var cppGenerator = Path.Combine(packagePrefix, "tools", "linux-x64", "Wireloom.Dds.Cpp");
        var runner = typeof(CppWireCommand).Assembly.Location;
        var nativePath = Path.Combine(repository, native);
        var buildPath = Path.Combine(evidencePath, "build");
        var processes = new ProcessRunner(repository);
        var extractedConfig = Path.Combine(packagePrefix, "build", "cmake");
        var packageArgument = File.Exists(Path.Combine(extractedConfig, "WireloomCppConfig.cmake"))
            ? $"-DWireloomCpp_DIR={extractedConfig}"
            : $"-DCMAKE_PREFIX_PATH={packagePrefix}";
        await processes.RunAsync("cmake",
            ["-S", nativePath, "-B", buildPath, packageArgument,
             $"-DCMAKE_MODULE_PATH={Path.Combine(nddsHome, "resource", "cmake")}", "-DCONNEXTDDS_ARCH=x64Linux4gcc8.5.0"],
            Path.Combine(evidencePath, "cmake-configure.log"));
        await processes.RunAsync("cmake", ["--build", buildPath, "--parallel", "2"], Path.Combine(evidencePath, "cmake-build.log"));

        var generated = (await File.ReadAllTextAsync(Path.Combine(buildPath, "wireloom-generated-dir.txt"))).TrimEnd();
        var hashes = new SortedDictionary<string, string>(StringComparer.Ordinal)
        {
            [idl] = Hash(Path.Combine(repository, idl)),
            [native + "/peer.cpp"] = Hash(Path.Combine(nativePath, "peer.cpp")),
            [native + "/CMakeLists.txt"] = Hash(Path.Combine(nativePath, "CMakeLists.txt")),
            [native + "/../peer-qos.hpp"] = Hash(Path.Combine(nativePath, "../peer-qos.hpp")),
            ["runner-assembly"] = Hash(runner),
            ["cpp-generator-native-executable"] = Hash(cppGenerator),
        };
        foreach (var suffix in new[] { ".hpp", ".cxx", "Plugin.hpp", "Plugin.cxx" })
        {
            var name = wireCase.Id + suffix;
            var freshHash = Hash(Path.Combine(generated, name));
            if (freshHash != Hash(Path.Combine(repository, retained, name)))
            {
                throw new InvalidOperationException($"Fresh generation differs from the retained C++ output for {name}.");
            }
            hashes[retained + "/" + name] = freshHash;
            hashes[oracle + "/" + name] = Hash(Path.Combine(repository, oracle, name));
        }
        WriteJson(evidencePath, "source-hashes.json", hashes);

        if (wireCase.Id == "01-primitives")
        {
            await CompareContractsAsync(processes, buildPath, evidencePath, wireCase.ExecutableSuffix);
        }

        var runId = Guid.NewGuid().ToString("N");
        var outcomes = new List<ScenarioOutcome>();
        foreach (var representation in new[] { "xcdr1", "xcdr2" })
        {
            foreach (var (writer, reader) in new[] { ("rti", "rti"), ("wireloom", "rti"), ("rti", "wireloom") })
            {
                var name = ScenarioName(representation, writer, reader);
                var scenario = Path.Combine(evidencePath, name);
                Directory.CreateDirectory(scenario);
                var topic = $"WireloomCpp_{runId}_{outcomes.Count}";
                var read = RunPeerAsync(processes, buildPath, wireCase.ExecutableSuffix, reader, "reader", representation, topic, scenario);
                var write = RunPeerAsync(processes, buildPath, wireCase.ExecutableSuffix, writer, "writer", representation, topic, scenario);
                var exits = await Task.WhenAll(read, write);
                var outcome = new ScenarioOutcome(representation, writer, reader, exits[1], exits[0]);
                outcomes.Add(outcome);
                WriteJson(evidencePath, "results.json", outcomes);
                Console.WriteLine($"{outcome.Name}: {(outcome.Passed ? "PASS" : "FAIL")}");
            }
        }

        return outcomes.All(outcome => outcome.Passed) ? 0 : 1;
    }

    private static async Task CompareContractsAsync(ProcessRunner processes, string buildPath, string evidencePath,
        string executableSuffix)
    {
        var reports = Path.Combine(evidencePath, "contracts");
        var rti = Path.Combine(reports, "rti");
        var wireloom = Path.Combine(reports, "wireloom");
        foreach (var (implementation, directory) in new[] { ("rti", rti), ("wireloom", wireloom) })
        {
            Directory.CreateDirectory(directory);
            await processes.RunAsync(Path.Combine(buildPath, $"{implementation}-{executableSuffix}"),
                ["contract", directory], Path.Combine(reports, $"{implementation}.log"),
                TimeSpan.FromSeconds(30), sanitizeOutput: StripLicenseBanner);
        }

        var expected = Directory.GetFiles(rti).Select(Path.GetFileName).Order(StringComparer.Ordinal).ToArray();
        var actual = Directory.GetFiles(wireloom).Select(Path.GetFileName).Order(StringComparer.Ordinal);
        if (expected.Length != 12 || !expected.SequenceEqual(actual, StringComparer.Ordinal))
        {
            throw new InvalidOperationException("Expected matching metadata, consumer state, and ten CDR reports.");
        }

        var hashes = new SortedDictionary<string, string>(StringComparer.Ordinal);
        foreach (var file in expected)
        {
            var expectedHash = Hash(Path.Combine(rti, file!));
            if (expectedHash != Hash(Path.Combine(wireloom, file!)))
            {
                throw new InvalidOperationException($"C++ contract report differs from RTI: {file}.");
            }
            hashes[file!] = expectedHash;
        }
        WriteJson(evidencePath, "contract-hashes.json", hashes);
    }

    private static async Task<int> RunPeerAsync(ProcessRunner processes, string buildPath, string executableSuffix, string implementation,
        string role, string representation, string topic, string scenario)
    {
        try
        {
            var result = await processes.RunAsync(Path.Combine(buildPath, $"{implementation}-{executableSuffix}"),
                [role, representation, topic, scenario], Path.Combine(scenario, role + ".log"),
                TimeSpan.FromSeconds(95), allowFailure: true, sanitizeOutput: StripLicenseBanner);
            return result.ExitCode;
        }
        catch (TimeoutException exception)
        {
            await File.WriteAllTextAsync(Path.Combine(scenario, role + ".log"), exception.Message);
            return 124;
        }
    }

    private static string StripLicenseBanner(string output) => string.Join('\n', output.Split('\n').Where(line =>
        !line.Contains("[RTI LICENSE]", StringComparison.Ordinal)
        && !line.StartsWith("Expires on ", StringComparison.Ordinal)
        && !line.StartsWith("Please contact support@rti.com ", StringComparison.Ordinal)));

    private static string Hash(string path) => Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(path)));

    private static void WriteJson<T>(string directory, string name, T value) =>
        File.WriteAllText(Path.Combine(directory, name), JsonSerializer.Serialize(value, EvidenceJson));

    private static string ScenarioName(string representation, string writer, string reader) =>
        $"{representation}-{writer}-to-{reader}";

    private sealed record CppWireCase(string Id, string NativeDirectory, string ExecutableSuffix);

    private sealed record ScenarioOutcome(string Representation, string Writer, string Reader,
        int WriterExitCode, int ReaderExitCode)
    {
        public string Name => ScenarioName(Representation, Writer, Reader);

        public bool Passed => WriterExitCode == 0 && ReaderExitCode == 0;
    }
}
