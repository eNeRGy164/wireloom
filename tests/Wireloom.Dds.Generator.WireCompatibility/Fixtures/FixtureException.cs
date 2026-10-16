namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Reports a fixture that cannot be constructed for the selected case.</summary>
internal sealed class FixtureException(string message) : Exception(message);
