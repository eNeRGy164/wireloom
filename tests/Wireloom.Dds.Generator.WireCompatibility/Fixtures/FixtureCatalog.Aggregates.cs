using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.WireCompatibility;

internal static partial class FixtureCatalog
{
    // Returns true when this group owns the case and has populated its fixture.
    private static bool TryPopulateAggregate(DynamicData sample, string caseId, string fixture)
    {
        if (caseId == "06-alias-composition")
        {
            SetFixtureValue(sample, "id", 1201);
            SetFixtureValue(sample, "values", new[] { 1.25f, -2.5f, 3.75f, 4.5f });
            return true;
        }

        if (caseId is "09-data-representation" or "09-allowed-data-representation")
        {
            SetFixtureValue(sample, "value", 1301);
            return true;
        }

        if (caseId == "05-shapes")
        {
            SetFixtureValue(sample, "unboundedItems[0].id", 701);
            SetFixtureValue(sample, "unboundedItems[0].label", "u701");
            SetFixtureValue(sample, "unboundedItems[1].id", 702);
            SetFixtureValue(sample, "unboundedItems[1].label", "u702");
            SetFixtureValue(sample, "boundedItems[0].id", 801);
            SetFixtureValue(sample, "boundedItems[0].label", "b801");
            SetFixtureValue(sample, "boundedItems[1].id", 802);
            SetFixtureValue(sample, "boundedItems[1].label", "b802");
            SetFixtureValue(sample, "nestedSequences[0][0]", 301);
            SetFixtureValue(sample, "nestedSequences[0][1]", 302);
            SetFixtureValue(sample, "nestedSequences[1][0]", 311);
            SetFixtureValue(sample, "nestedSequences[1][1]", 312);
            SetFixtureValue(sample, "nestedSequences[1][2]", 313);
            SetFixtureValue(sample, "rows[0][0]", 101);
            SetFixtureValue(sample, "rows[0][1]", 102);
            SetFixtureValue(sample, "rows[0][2]", 103);
            SetFixtureValue(sample, "rows[1][0]", 111);
            SetFixtureValue(sample, "rows[1][1]", 112);
            SetFixtureValue(sample, "rows[1][2]", 113);
            SetFixtureValue(sample, "sequenceOfArrays[0][0]", 201);
            SetFixtureValue(sample, "sequenceOfArrays[0][1]", 202);
            SetFixtureValue(sample, "sequenceOfArrays[0][2]", 203);
            SetFixtureValue(sample, "sequenceOfArrays[1][0]", 211);
            SetFixtureValue(sample, "sequenceOfArrays[1][1]", 212);
            SetFixtureValue(sample, "sequenceOfArrays[1][2]", 213);
            SetFixtureValue(sample, "sequenceOfArrayAliases[0][0][0]", 401);
            SetFixtureValue(sample, "sequenceOfArrayAliases[0][0][1]", 402);
            SetFixtureValue(sample, "sequenceOfArrayAliases[0][0][2]", 403);
            SetFixtureValue(sample, "sequenceOfArrayAliases[0][1][0]", 411);
            SetFixtureValue(sample, "sequenceOfArrayAliases[0][1][1]", 412);
            SetFixtureValue(sample, "sequenceOfArrayAliases[0][1][2]", 413);
            SetFixtureValue(sample, "sequenceOfArrayAliases[1][0][0]", 421);
            SetFixtureValue(sample, "sequenceOfArrayAliases[1][0][1]", 422);
            SetFixtureValue(sample, "sequenceOfArrayAliases[1][0][2]", 423);
            SetFixtureValue(sample, "sequenceOfArrayAliases[1][1][0]", 431);
            SetFixtureValue(sample, "sequenceOfArrayAliases[1][1][1]", 432);
            SetFixtureValue(sample, "sequenceOfArrayAliases[1][1][2]", 433);
            SetFixtureValue(sample, "names[0]", "name-1");
            SetFixtureValue(sample, "names[1]", "name-2");
            SetFixtureValue(sample, "names[2]", "name-3");
            return true;
        }

        if (caseId == "06-aggregates")
        {
            SetFixtureValue(sample, "base.base", 6101);
            SetFixtureValue(sample, "derived.base", 6201);
            SetFixtureValue(sample, "derived.derived", "derived-6201");
            return true;
        }

        return false;
    }
}
