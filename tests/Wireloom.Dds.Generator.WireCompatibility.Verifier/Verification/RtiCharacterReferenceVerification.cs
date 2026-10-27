using System.Text.Json.Nodes;

namespace Wireloom.Dds.Generator.WireCompatibility.Verifier.Verification;

/// <summary>Checks guarded RTI C# and C++ reference-binding corrections.</summary>
internal static class RtiCharacterReferenceVerification
{
    internal static void Run(VerificationContext context)
    {
        var repositoryRoot = context.RepositoryRoot;
        var root = context.Root;

        void Check(bool condition, string description) => context.Check(condition, description);
        void Invoke(bool succeeds, params string[] command) => context.Invoke(succeeds, command);
        JsonNode ReadJson(string path) => VerificationContext.ReadJson(path);

        foreach (var version in new[] { "7.3.1", "7.7.0" })
        {
            var referenceExclusions = ReadJson(Path.Combine(repositoryRoot, "tests", "Wireloom.Dds.Generator.WireCompatibility", $"wire-case-exclusions-{version}.json"));
            Check(referenceExclusions["expectedFailures"] is null,
                "Profile exclusions contain no expected-failure registry.");

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

                Invoke(succeeds: true, "adapt-rti-characters", "csharp", caseId, patchDirectory);

                var adaptedSample = File.ReadAllText(samplePath);
                var adaptedPlugin = File.ReadAllText(pluginPath);
                var label = caseId.EndsWith("-255", StringComparison.Ordinal) ? "255" : "127";
                Check(adaptedSample.Contains($"Discriminator = (char){label};", StringComparison.Ordinal)
                    && adaptedPlugin.Contains("sample.Setfallback(fallback, NativeChar.FromUtf8(_d));", StringComparison.Ordinal),
                    "C# reference corrections compile numeric chars and preserve the incoming fallback discriminator.");

                Check(adaptedPlugin.Contains("new int[] { -1 }", StringComparison.Ordinal) == (label == "255"),
                    "Only high character metadata uses signed-byte normalization.");

                Invoke(succeeds: true, "adapt-rti-characters", "csharp", caseId, patchDirectory);
                Check(adaptedSample == File.ReadAllText(samplePath) && adaptedPlugin == File.ReadAllText(pluginPath),
                    "Reapplying the reference correction is idempotent.");

                Check(originalSample == File.ReadAllText(Path.Combine(originalDirectory, $"{caseId}.cs"))
                    && originalPlugin == File.ReadAllText(Path.Combine(originalDirectory, $"{caseId}Plugin.cs")),
                    "Retained RTI captures are untouched.");

                File.WriteAllText(samplePath, originalSample);
                File.WriteAllText(pluginPath, originalPlugin.Replace("sample.fallback = fallback;", "/* unexpected generated shape */", StringComparison.Ordinal));

                var invalidPlugin = File.ReadAllText(pluginPath);
                Invoke(succeeds: false, "adapt-rti-characters", "csharp", caseId, patchDirectory);
                Check(originalSample == File.ReadAllText(samplePath) && invalidPlugin == File.ReadAllText(pluginPath), "Unexpected generated code fails before either C# file is changed.");
            }

            var cppCase = "07-union-char-numeric-255";
            var cppDirectory = Path.Combine(root, $"cpp-reference-{version}");
            Directory.CreateDirectory(cppDirectory);

            var cppPath = Path.Combine(cppDirectory, "idl.cxx");
            var originalCpp = File.ReadAllText(Path.Combine(repositoryRoot, "docs", "corpus", version,
                "rtiddsgen", "cpp", "features", cppCase, $"{cppCase}.cxx"));

            File.WriteAllText(cppPath, originalCpp);

            Invoke(succeeds: true, "adapt-rti-characters", "cpp", cppCase, cppDirectory);

            var adaptedCpp = File.ReadAllText(cppPath);
            Check(adaptedCpp == originalCpp.Replace("static_cast<int>(255)", "static_cast<int>(-1)", StringComparison.Ordinal),
                "The C++ adaptation changes only the native TypeCode label.");

            Invoke(succeeds: true, "adapt-rti-characters", "cpp", cppCase, cppDirectory);
            Check(adaptedCpp == File.ReadAllText(cppPath), "The C++ metadata correction is idempotent.");

            File.WriteAllText(cppPath, originalCpp.Replace("static_cast<int>(255)", "unexpected-label", StringComparison.Ordinal));

            var invalidCpp = File.ReadAllText(cppPath);
            Invoke(succeeds: false, "adapt-rti-characters", "cpp", cppCase, cppDirectory);
            Check(invalidCpp == File.ReadAllText(cppPath), "An unexpected C++ label fails without changing source.");
        }
    }
}
