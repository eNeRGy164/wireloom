using System.Text.Json.Nodes;
using DirectedPeerPair = Wireloom.Dds.Generator.WireCompatibility.Shared.DirectedPeerPair;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Resolves ordered peer availability and directed fixture pairings for a runtime profile.</summary>
internal static class PeerPairingPolicy
{
    /// <summary>Returns configured or default peers after exact-profile exclusions.</summary>
    internal static string[] GetPeers(JsonNode definition, string? version = null)
    {
        ArgumentNullException.ThrowIfNull(definition);

        var peers = definition["wireMatrix"]?["peers"] is JsonArray configuredPeers
            ? configuredPeers.Select(peer => (string)peer!).ToList()
            : [PeerNames.WireloomCSharp, PeerNames.RtiCSharp, PeerNames.RtiCpp];

        foreach (var exclusion in definition["wireMatrix"]?["excludedPeersByRtiVersion"]?[version ?? ""]?.AsObject() ?? [])
        {
            peers.Remove(exclusion.Key);
        }

        return [.. peers];
    }

    /// <summary>Applies a directed fixture override or expands the ordered Cartesian peer product.</summary>
    internal static DirectedPeerPair[] GetPairings(JsonNode definition, string? fixture = null, string? version = null)
    {
        var peers = GetPeers(definition, version);

        if (fixture is null || definition["wireMatrix"]?["fixturePairings"]?[fixture] is not JsonArray configuredPairings)
        {
            return [.. peers.SelectMany(writer => peers.Select(reader => new DirectedPeerPair(writer, reader)))];
        }

        var pairings = configuredPairings.Select(value =>
        {
            var endpoints = ((string)value!).Split('|');
            if (endpoints.Length != 2 || !peers.Contains(endpoints[0]) || !peers.Contains(endpoints[1]))
            {
                throw new InvalidDataException($"Invalid peer pairing for {fixture}.");
            }

            return new DirectedPeerPair(endpoints[0], endpoints[1]);
        }).ToArray();

        if (pairings.Length == 0 || pairings.Distinct().Count() != pairings.Length)
        {
            throw new InvalidDataException($"Fixture pairings for {fixture} must be non-empty and unique.");
        }

        return pairings;
    }
}
