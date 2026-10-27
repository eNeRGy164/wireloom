namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Checks oracle mode precedence, fallback availability, and per-run/profile manifest snapshots.</summary>
internal static class WireOracleCatalogVerification
{
    /// <summary>Exercises oracle selection without building utilities or launching peer processes.</summary>
    internal static void Run(MatrixOptions options)
    {
        // Arrange: construction must not load the manifest before run initialization.
        var root = Path.Combine(options.ScratchDirectory, "oracle-catalog-verification");
        var manifest = Path.Combine(root, "wire-oracles.json");
        var catalog = new WireOracleCatalog(manifest);
        Directory.CreateDirectory(root);
        File.WriteAllText(manifest, """
            { "cases": {
              "Case": { "rtiCSharp": { "mode": "captured", "sourceDirectory": "csharp/override-not-present" } },
              "adjusted": { "rtiCpp": { "mode": "adjusted", "sourceDirectory": "cpp/wire-adjusted/adjusted" } },
              "empty": { "rtiCSharp": { "mode": "captured", "sourceDirectory": "" } },
              "missing-path": { "rtiCSharp": { "mode": "captured" } },
              "manual": { "rtiCSharp": { "mode": "manual" }, "rtiCpp": { "mode": "manual" } },
              "exception": { "rtiCSharp": { "mode": "known-exception" } },
              "unknown-mode": { "rtiCpp": { "mode": "unknown" } },
              "missing-mode": { "rtiCpp": {} },
              "null-entry": { "rtiCSharp": null }
            } }
            """);
        foreach (var language in new[] { "csharp", "cpp" })
        {
            foreach (var caseId in new[] { "fallback", "manual", "exception", "unknown-mode", "missing-mode", "null-entry", "adjusted" })
            {
                Directory.CreateDirectory(Path.Combine(root, language, "features", caseId));
            }
        }

        File.WriteAllText(Path.Combine(root, "cpp", "features", "file-only"), "Not a directory.");

        // Act / Assert
        Check(catalog.FindSource("Case", "rtiCSharp") == "csharp/override-not-present", "An explicit capture must retain its configured path even if absent on disk.");
        Check(catalog.FindSource("case", "rtiCSharp") is null, "Manifest case identity must remain ordinal.");
        Check(catalog.FindSource("adjusted", "rtiCpp") == "cpp/wire-adjusted/adjusted", "An adjusted capture must override an existing conventional directory.");
        Check(catalog.FindSource("adjusted", "rtiCSharp") == "csharp/features/adjusted", "Each peer must resolve independently.");
        Check(catalog.FindSource("empty", "rtiCSharp") == "" && catalog.FindSource("missing-path", "rtiCSharp") == "", "Explicit empty/missing capture paths must preserve the standalone blank-line contract.");
        Check(catalog.FindSource("fallback", "rtiCSharp") == "csharp/features/fallback" && catalog.FindSource("fallback", "rtiCpp") == "cpp/features/fallback", "Absent entries must use existing language-specific conventional directories.");
        Check(catalog.FindSource("null-entry", "rtiCSharp") == "csharp/features/null-entry", "An explicit null entry must retain conventional fallback.");
        Check(catalog.FindSource("manual", "rtiCSharp") is null && catalog.FindSource("manual", "rtiCpp") is null, "Manual entries must suppress fallback for both peers.");
        Check(catalog.FindSource("exception", "rtiCSharp") is null && catalog.FindSource("unknown-mode", "rtiCpp") is null && catalog.FindSource("missing-mode", "rtiCpp") is null, "Every explicit non-capture mode must suppress fallback.");
        Check(catalog.FindSource("absent", "rtiCSharp") is null && catalog.FindSource("file-only", "rtiCpp") is null, "Unavailable directories and ordinary files must not become captures.");
        CheckRejected<ArgumentException>(() => catalog.FindSource("fallback", "unknown"), "Unknown fallback peers must remain errors.");

        // Arrange / Act / Assert: metadata is a snapshot, while fallback directory availability is checked on lookup.
        File.Delete(manifest);
        Check(catalog.FindSource("Case", "rtiCSharp") == "csharp/override-not-present", "The run must retain its loaded oracle manifest without rereading the file.");
        Directory.CreateDirectory(Path.Combine(root, "csharp", "features", "late-capture"));
        Check(catalog.FindSource("late-capture", "rtiCSharp") == "csharp/features/late-capture", "Fallback lookup must still check current directory availability.");
        File.WriteAllText(manifest, """{ "cases": {} }""");
        Check(new WireOracleCatalog(manifest).FindSource("Case", "rtiCSharp") is null, "A new run must see its own manifest snapshot.");
        var otherProfile = Path.Combine(root, "other-profile", "wire-oracles.json");
        Directory.CreateDirectory(Path.GetDirectoryName(otherProfile)!);
        File.WriteAllText(otherProfile, """{ "cases": { "Case": { "rtiCSharp": { "mode": "captured", "sourceDirectory": "csharp/other-profile" } } } }""");
        Check(new WireOracleCatalog(otherProfile).FindSource("Case", "rtiCSharp") == "csharp/other-profile" &&
            catalog.FindSource("Case", "rtiCSharp") == "csharp/override-not-present", "Profile-specific catalogs must not share selection state.");
        Console.WriteLine("Wire oracle selection passed: captured/adjusted precedence, explicit exclusions, per-peer fallback, missing paths, and per-run/profile snapshots; no utility subprocess required.");
    }

    private static void CheckRejected<TException>(Action action, string message) where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }

        throw new InvalidOperationException(message);
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
