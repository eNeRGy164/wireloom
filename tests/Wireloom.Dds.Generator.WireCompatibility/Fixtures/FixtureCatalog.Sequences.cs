using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.WireCompatibility;

internal static partial class FixtureCatalog
{
    // Returns true when this group owns the case and has populated its fixture.
    private static bool TryPopulateSequence(DynamicData sample, string caseId, string fixture)
    {
        if (caseId == "02-scopes" && fixture.StartsWith("sequence-", StringComparison.Ordinal))
        {
            SetFixtureValue(sample, "event", 501);
            SetFixtureValue(sample, "relative.x", 502);
            SetFixtureValue(sample, "relative.y", 503);
            SetFixtureValue(sample, "absolute.x", 504);
            SetFixtureValue(sample, "absolute.y", 505);
            SetFixtureValue(sample, "values", fixture == "sequence-empty"
                ? []
                : new[] { 506 });
            return true;
        }

        if (caseId == "02-constant-expressions" && fixture.StartsWith("sequence-", StringComparison.Ordinal))
        {
            SetFixtureValue(sample, "values", fixture == "sequence-empty"
                ? []
                : new[] { 601 });
            for (var index = 0; index < 5; index++)
            {
                SetFixtureValue(sample, $"array[{index}]", 611 + index);
            }
            return true;
        }

        if (caseId == "03-alias-collections" && fixture.StartsWith("sequence-", StringComparison.Ordinal))
        {
            if (fixture == "sequence-single")
            {
                SetFixtureValue(sample, "values[0][0]", 1101);
            }
            return true;
        }

        if (caseId == "05-collections" && fixture.StartsWith("sequence-", StringComparison.Ordinal))
        {
            SetFixtureValue(sample, "unbounded", fixture == "sequence-empty"
                ? []
                : new[] { 301 });
            SetFixtureValue(sample, "bounded", fixture == "sequence-empty"
                ? []
                : new[] { 401 });
            if (fixture == "sequence-single")
            {
                SetFixtureValue(sample, "items[0].id", 701);
                SetFixtureValue(sample, "items[0].label", "item-701");
            }
            return true;
        }

        return false;
    }
}
