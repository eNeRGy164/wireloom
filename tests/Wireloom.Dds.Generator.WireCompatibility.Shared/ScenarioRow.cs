namespace Wireloom.Dds.Generator.WireCompatibility.Shared;

/// <summary>One completed or pending exchange in the canonical scheduler evidence.</summary>
/// <param name="CaseId">Corpus case identifier.</param>
/// <param name="WireloomType">Generated Wireloom managed type.</param>
/// <param name="RtiCSharpType">RTI managed control type.</param>
/// <param name="RtiCppType">Native control type.</param>
/// <param name="Writer">Writer peer label.</param>
/// <param name="WriterVersion">Writer runtime profile.</param>
/// <param name="Reader">Reader peer label.</param>
/// <param name="ReaderVersion">Reader runtime profile.</param>
/// <param name="Status">Recorded exchange outcome.</param>
/// <param name="Fixture">Named fixture.</param>
public sealed record ScenarioRow(string CaseId, string WireloomType, string RtiCSharpType, string RtiCppType, string Writer, string WriterVersion, string Reader, string ReaderVersion, string Status, string Fixture)
{
    /// <summary>Reads the stable tab-separated interchange format used by the wire matrix runner.</summary>
    /// <param name="line">A single tab-separated scenario row.</param>
    /// <returns>The parsed scenario.</returns>
    /// <exception cref="InvalidDataException">The row does not contain exactly ten fields.</exception>
    public static ScenarioRow Parse(string line)
    {
        ArgumentNullException.ThrowIfNull(line);

        var fields = line.Split('\t');
        if (fields.Length != 10)
        {
            throw new InvalidDataException("Scenario TSV row must contain 10 fields.");
        }

        return new ScenarioRow(fields[0], fields[1], fields[2], fields[3], fields[4], fields[5], fields[6], fields[7], fields[8], fields[9]);
    }

    /// <summary>Writes fields in the stable build/exchange worker protocol order.</summary>
    /// <returns>The scenario encoded as a tab-separated row.</returns>
    public string ToTsv() => string.Join('\t', CaseId, WireloomType, RtiCSharpType, RtiCppType, Writer, WriterVersion, Reader, ReaderVersion, Status, Fixture);
}
