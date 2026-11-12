using System.Text.Json.Nodes;
using DirectedPeerPair = Wireloom.Dds.Generator.WireCompatibility.Shared.DirectedPeerPair;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Resolves ordered peer availability and directed fixture pairings for a runtime profile.</summary>
internal static class PeerPairingPolicy
{
    /// <summary>Returns configured or default peers after exact-profile exclusions.</summary>
    internal static string[] GetPeers(JsonNode definition, string? version = null, bool includeWireloomCpp = false)
    {
        ArgumentNullException.ThrowIfNull(definition);

        var peers = definition["wireMatrix"]?["peers"] is JsonArray configuredPeers
            ? configuredPeers.Select(peer => (string)peer!).ToList()
            : [PeerNames.WireloomCSharp, PeerNames.RtiCSharp, PeerNames.RtiCpp];

        foreach (var exclusion in definition["wireMatrix"]?["excludedPeers"]?.AsObject() ?? [])
        {
            peers.Remove(exclusion.Key);
        }

        foreach (var exclusion in definition["wireMatrix"]?["excludedPeersByRtiVersion"]?[version ?? ""]?.AsObject() ?? [])
        {
            peers.Remove(exclusion.Key);
        }

        var excludedPeers = definition["wireMatrix"]?["excludedPeers"]?.AsObject();
        var profileExcludedPeers = definition["wireMatrix"]?["excludedPeersByRtiVersion"]?[version ?? ""]?.AsObject();
        if (includeWireloomCpp
            && excludedPeers?.ContainsKey(PeerNames.WireloomCpp) != true
            && profileExcludedPeers?.ContainsKey(PeerNames.WireloomCpp) != true
            && !peers.Contains(PeerNames.WireloomCpp, StringComparer.Ordinal))
        {
            peers.Add(PeerNames.WireloomCpp);
        }

        return [.. peers];
    }

    /// <summary>Expands the ordered Cartesian product for a full C++ matrix; otherwise applies an exact fixture override.</summary>
    internal static DirectedPeerPair[] GetPairings(JsonNode definition, string? fixture = null, string? version = null,
        bool includeWireloomCpp = false, IReadOnlyList<DirectedPeerPair>? qualifiedWireloomCppPairings = null)
    {
        var fixtureExclusions = fixture is null
            ? null
            : definition["wireMatrix"]?["excludedPeersByFixtureByRtiVersion"]?[version ?? ""]?[fixture]?.AsObject()
                ?? definition["wireMatrix"]?["excludedPeersByFixture"]?[fixture]?.AsObject();
        var peers = GetPeers(definition, version, includeWireloomCpp)
            .Where(peer => fixtureExclusions?.ContainsKey(peer) != true)
            .ToArray();
        includeWireloomCpp &= peers.Contains(PeerNames.WireloomCpp, StringComparer.Ordinal);

        var configuredPairings = fixture is null
            ? null
            : definition["wireMatrix"]?["fixturePairingsByRtiVersion"]?[version ?? ""]?[fixture]?.AsArray()
                ?? definition["wireMatrix"]?["fixturePairings"]?[fixture]?.AsArray();
        var parsedPairings = configuredPairings?.Select(value =>
        {
            var endpoints = ((string)value!).Split('|');
            if (endpoints.Length != 2 || !peers.Contains(endpoints[0]) || !peers.Contains(endpoints[1]))
            {
                throw new InvalidDataException($"Invalid peer pairing for {fixture}.");
            }

            return new DirectedPeerPair(endpoints[0], endpoints[1]);
        }).ToArray();
        if (parsedPairings is { } configured && (configured.Length == 0 || configured.Distinct().Count() != configured.Length))
        {
            throw new InvalidDataException($"Fixture pairings for {fixture} must be non-empty and unique.");
        }

        var result = new List<DirectedPeerPair>();
        if (parsedPairings is not null)
        {
            // A fixture-specific list is an exact documented exception to the
            // profile's default full matrix, including Wireloom C++ pairings.
            result.AddRange(parsedPairings);
        }
        else
        {
            result.AddRange(peers.SelectMany(writer => peers.Select(reader => new DirectedPeerPair(writer, reader))));
        }

        if (includeWireloomCpp && parsedPairings is null)
        {
            foreach (var otherPeer in peers.Where(peer => peer != PeerNames.WireloomCpp))
            {
                result.Add(new DirectedPeerPair(PeerNames.WireloomCpp, otherPeer));
                result.Add(new DirectedPeerPair(otherPeer, PeerNames.WireloomCpp));
            }
            result.Add(new DirectedPeerPair(PeerNames.WireloomCpp, PeerNames.WireloomCpp));
        }

        if (qualifiedWireloomCppPairings is not null)
        {
            var availablePeers = GetPeers(definition, version, includeWireloomCpp: true)
                .Where(peer => fixtureExclusions?.ContainsKey(peer) != true)
                .ToHashSet(StringComparer.Ordinal);
            foreach (var pairing in qualifiedWireloomCppPairings)
            {
                if (!availablePeers.Contains(pairing.Writer) || !availablePeers.Contains(pairing.Reader)
                    || pairing.Writer != PeerNames.WireloomCpp && pairing.Reader != PeerNames.WireloomCpp)
                {
                    throw new InvalidDataException($"Invalid qualified Wireloom C++ pairing for {fixture}.");
                }

                result.Add(pairing);
            }
        }

        return [.. result.Distinct()];
    }
}
