using ReportFields = System.Collections.Generic.Dictionary<string, object?>;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Describes actual endpoint implementations and checked-in test adaptations.</summary>
internal static class ReportEndpointPolicy
{
    private static readonly Dictionary<string, (string Implementation, string Language)> PeerData = new()
        {
            ["Wireloom C#"] = ("Wireloom", "C#"),
            ["RTI C#"] = ("RTI", "C#"),
            ["RTI C++"] = ("RTI", "C++"),
        };

    /// <summary>Builds endpoint evidence using the established adaptation rules.</summary>
    internal static ReportFields Create(string id, string fixture, string peer, string peerVersion)
    {
        var adaptation = Adaptation(id, fixture, peer, peerVersion);
        var (Implementation, Language) = PeerData[peer];
        var endpoint = new ReportFields
        {
            ["peer"] = peer,
            ["implementation"] = Implementation,
            ["language"] = Language,
            ["rtiVersion"] = peerVersion,
            ["mode"] = EndpointMode(peer, adaptation),
        };

        if (adaptation is not null)
        {
            endpoint["adaptation"] = adaptation;
        }

        return endpoint;
    }

    private static string EndpointMode(string peer, string? adaptation)
    {
        if (adaptation is not null)
        {
            return "custom-test-adaptation";
        }

        if (peer == "Wireloom C#")
        {
            return "wireloom-generated";
        }

        return "rti-default";
    }

    private static string? Adaptation(string id, string fixture, string peer, string peerVersion)
    {
        if (peer == "RTI C#" && (id is "07-union-uint32" or "07-union-ulong"))
        {
            return "Checked-in adjusted RTI C# oracle: use an unchecked 32-bit conversion for the maximum union label.";
        }

        if (peer == "RTI C#" && id == "07-union-char-boundaries")
        {
            return "Checked-in adjusted RTI C# oracle: restore the IDL ASCII maximum label U+007E in place of the captured U+00FF literal.";
        }

        if ((id is "14-autoid-scope" or "14-enum-constant") && peer == "RTI C#")
        {
            return peerVersion == "7.3.1"
                ? "RTI 7.3.1 C# include staging: pass byte-identical wrapper/canonical/dependency IDL copies through an isolated scratch tree to satisfy its basename include resolution; preserve source and staged hashes."
                : "RTI C# include staging: reuse generated root and canonical dependency sources after expected names and unique output locations are confirmed; preserve raw copies and hashes.";
        }

        if (id == "14-autoid-scope" && peer == "RTI C++")
        {
            return "RTI C++ include staging: enumerate recursive dependencies; reuse unique generated outputs at their exact relative paths or stage headers at IDL-relative paths after byte comparison, retaining generated copies and hashes.";
        }

        if (id == "10-resolve-name-wstring-aliases" && peer == "RTI C#")
        {
            return "Post-generation namespace correction: qualify both unmanaged alias types with the IDL module.";
        }

        if (id == "10-resolve-name-wstring-aliases" && peer == "RTI C++")
        {
            return "Post-generation TypeCode correction: use the generated, module-qualified alias-tag types for both wstring members.";
        }

        if (id == "09-optional-recursive-array")
        {
            var adaptation = "Disable TypeObject/TypeCode propagation.";
            adaptation += " Writer dds.data_writer.history.memory_manager.fast_pool.pool_buffer_max_size=4096 bytes; larger samples allocate separately.";

            if (peerVersion == "7.3.1")
            {
                adaptation += " Avoids RTI 7.3.1 preallocating about 64 GiB.";
            }

            if (peer == "RTI C#")
            {
                adaptation += "\nBootstrap plugin before Support.Instance to avoid recursive initialization NullReferenceException.";
            }
            else if (peer == "RTI C++")
            {
                adaptation += "\nForward the visited-node chain during TypeCode initialization.";
            }

            return adaptation;
        }

        if (id == "06-alias-composition" && peer == "RTI C#")
        {
            return "Qualify the conversion receiver to fix shadowed member access.";
        }

        if (id is "07-union-char-numeric" or "07-union-char-numeric-255")
        {
            if (peer == "RTI C#")
            {
                var adaptation = "Post-generation RTI C# correction: add char casts; preserve fallback discriminator with Setfallback.";

                if (id == "07-union-char-numeric-255")
                {
                    adaptation += " UnionMember label: 255 → -1; public 255 and wire 0xFF unchanged.";
                }

                return adaptation;
            }

            if (peer == "RTI C++" && id == "07-union-char-numeric-255")
            {
                return "Compile with -funsigned-char for label 255. TypeCode label: 255 → -1; public 255, wire 0xFF, and payload 13702 unchanged. Fixes zero-payload serialization and type matching.";
            }
        }

        if (peer != "RTI C++")
        {
            return null;
        }

        if (id == "09-optional-string-sequences" && fixture is ("optional-wide-only" or "optional-multiple"))
        {
            return "DynamicData C API element setters bypass the typed optional wide-string sequence serializer.";
        }

        if (id == "06-aggregates" && peerVersion == "7.3.1")
        {
            return "Qualify the generated Recursive TypeCode reference with its IDL module.";
        }

        if (id == "07-union-wchar-label")
        {
            return "Manual DynamicType reference; rtiddsgen rejects the wchar discriminator.";
        }

        if (id == "10-flat-data-binding")
        {
            return "Plain C++ reference: remove @language_binding(FLAT_DATA); canonical extensibility is incompatible with FlatData. FlatData layout is unqualified.";
        }

        return null;
    }
}
