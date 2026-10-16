using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.WireCompatibility;

internal static partial class FixtureCatalog
{
    // Returns true when this group owns the case and has populated its fixture.
    private static bool TryPopulatePreprocessing(DynamicData sample, string caseId, string fixture)
    {
        if (caseId is "11-preprocessing" or "11-include-search")
        {
            var baseValue = caseId == "11-preprocessing" ? 1101 : 1161;
            SetFixtureValue(sample, "item.value", baseValue);
            SetFixtureValue(sample, "value", baseValue + 1);
            return true;
        }

        if (caseId == "11-conditionals")
        {
            SetFixtureValue(sample, "value", 1111);
            return true;
        }

        if (caseId == "11-macros")
        {
            SetFixtureValue(sample, "value", 1121);
            return true;
        }

        if (caseId == "11-macro-operators")
        {
            SetFixtureValue(sample, "field", 1131);
            return true;
        }

        if (caseId == "11-preprocessor-advanced")
        {
            SetFixtureValue(sample, "value", 1151);
            return true;
        }

        if (caseId == "11-comments")
        {
            SetFixtureValue(sample, "text", "comment");
            return true;
        }

        return false;
    }
}
