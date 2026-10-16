#nullable enable annotations

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Describes the corpus case, generated type, topic, fixture, and process role.</summary>
internal sealed record PeerOptions(string CaseId, string TypeName, string TopicName, string RuntimeVersion, string Fixture, PeerRole Role)
{
    /// <summary>Reads the option/value pairs supplied by the matrix runner.</summary>
    public static PeerOptions Parse(string[] args)
    {
        var values = new Dictionary<string, string>(StringComparer.Ordinal);

        for (var index = 0; index < args.Length; index += 2)
        {
            if (index + 1 >= args.Length || !args[index].StartsWith("--", StringComparison.Ordinal))
            {
                throw new ArgumentException("Expected option/value pairs.");
            }

            values.Add(args[index][2..], args[index + 1]);
        }

        var role = values["role"] switch
        {
            "writer" => PeerRole.Writer,
            "reader" => PeerRole.Reader,
            _ => throw new ArgumentException("Role must be writer or reader.")
        };

        return new PeerOptions(
            values["case"],
            values["type"],
            values["topic"],
            values["runtime-version"],
            values.GetValueOrDefault("fixture", "default"),
            role);
    }
}

/// <summary>Identifies which side of a single exchange this process runs.</summary>
internal enum PeerRole
{
    Writer,
    Reader
}
