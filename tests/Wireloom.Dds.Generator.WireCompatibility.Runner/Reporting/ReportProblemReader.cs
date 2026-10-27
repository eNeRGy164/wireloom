namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Extracts observed failure and not-run reasons from sanitized scenario logs.</summary>
internal static class ReportProblemReader
{
    /// <summary>Reads the established detail/status/exit-code fallbacks for one directed exchange.</summary>
    internal static string Read(string id, string fixture, string writer, string reader, string status, string directory)
    {
        var peerIds = new Dictionary<string, string>
        {
            ["Wireloom C#"] = "wireloom-cs",
            ["RTI C#"] = "rti-cs",
            ["RTI C++"] = "rti-cpp",
        };

        var log = Path.Combine(directory, $"{id}-{fixture}-{peerIds[writer]}-{peerIds[reader]}.log");
        if (!File.Exists(log))
        {
            return "scenario log is missing";
        }

        var lines = File.ReadAllLines(log);
        var notRun = lines.FirstOrDefault(l => l.StartsWith("NOT_RUN_DETAIL ", StringComparison.Ordinal));
        if (notRun is not null)
        {
            return notRun["NOT_RUN_DETAIL ".Length..];
        }

        if (status == "NOT_RUN")
        {
            return "scenario did not run";
        }

        var observations = new List<string>();
        foreach (var line in lines)
        {
            if (line.StartsWith("FAIL_DETAIL ", StringComparison.Ordinal))
            {
                observations.Add(line["FAIL_DETAIL ".Length..].TrimEnd('.'));
            }
            else if (line.StartsWith("FAIL ", StringComparison.Ordinal))
            {
                observations.Add(line["FAIL ".Length..].TrimEnd('.'));
            }
        }

        var endpointFields = new Dictionary<string, string>();

        var endpointLine = lines.FirstOrDefault(line => line.StartsWith("ENDPOINT_STATUS ", StringComparison.Ordinal));
        if (endpointLine is not null)
        {
            foreach (var field in endpointLine.Split(' ').Skip(1))
            {
                var parts = field.Split('=', 2);
                if (parts.Length == 2)
                {
                    endpointFields.Add(parts[0], parts[1]);
                }
            }
        }

        foreach (var (Name, Peer) in new[] { (Name: "writer", Peer: writer), (Name: "reader", Peer: reader) })
        {
            if (endpointFields.GetValueOrDefault($"{Name}_exit_code") == "139")
            {
                observations.Add($"{Peer} {Name} terminated with exit 139 (SIGSEGV)");
            }
        }

        if (observations.Count > 0)
        {
            return string.Join("; ", observations.Distinct());
        }

        if (lines.Any(l => l.StartsWith("RTI_DIAGNOSTIC ", StringComparison.Ordinal) && l.Contains("different type kinds", StringComparison.Ordinal)))
        {
            return "RTI discovery rejected the type: different member type kinds";
        }

        foreach (var (Name, Peer) in new[] { (Name: "writer", Peer: writer), (Name: "reader", Peer: reader) })
        {
            if (endpointFields.TryGetValue($"{Name}_exit_code", out var code) && code != "0")
            {
                return $"{Peer} {Name} exited with code {code}";
            }
        }

        return "endpoint failed; see sanitized scenario log";
    }
}
