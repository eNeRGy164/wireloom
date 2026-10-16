using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.WireCompatibility;

internal static partial class FixtureCatalog
{
    private static void PopulateCollection(DynamicData sample, string caseId)
    {
        if (caseId != "05-collections")
        {
            return;
        }

        SetFixtureValue(sample, "values[0,0]", 101);
        SetFixtureValue(sample, "values[0,1]", 102);
        SetFixtureValue(sample, "values[0,2]", 103);
        SetFixtureValue(sample, "values[1,0]", 201);
        SetFixtureValue(sample, "values[1,1]", 202);
        SetFixtureValue(sample, "values[1,2]", 203);
        SetFixtureValue(sample, "unbounded", new[] { 301, 302 });
        SetFixtureValue(sample, "bounded", new[] { 401, 402 });
        SetFixtureValue(sample, "items[0].id", 701);
        SetFixtureValue(sample, "items[0].label", "item-701");
        SetFixtureValue(sample, "items[1].id", 702);
        SetFixtureValue(sample, "items[1].label", "item-702");
        SetFixtureValue(sample, "grid[0,0]", 501);
        SetFixtureValue(sample, "grid[0,1]", 502);
        SetFixtureValue(sample, "grid[0,2]", 503);
        SetFixtureValue(sample, "grid[1,0]", 601);
        SetFixtureValue(sample, "grid[1,1]", 602);
        SetFixtureValue(sample, "grid[1,2]", 603);

        return;
    }
}
