using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Builds deterministic DynamicData values, grouped by the IDL feature under test.</summary>
internal static partial class FixtureCatalog
{
    /// <summary>Creates one fixture; cases without explicit values retain RTI's defaults.</summary>
    internal static DynamicData CreateFixture(DynamicType dynamicType, string caseId, string fixture)
    {
        var sample = new DynamicData(dynamicType);
        if (TryPopulateSpecialCase(sample, caseId, fixture))
        {
            return sample;
        }

        if (caseId == "09-optional-string-sequences" || fixture == "default"
            || fixture.StartsWith("optional-", StringComparison.Ordinal))
        {
            return sample;
        }

        if (TryPopulateSequence(sample, caseId, fixture) || TryPopulateUnion(sample, caseId, fixture))
        {
            return sample;
        }

        if (fixture != "non-default"
            && !(caseId == "03-enums-aliases" && fixture == "enum-alternate")
            && !(caseId == "04-strings" && fixture is "string-empty" or "string-boundary")
            && !(caseId == "04-boundaries" && fixture == "string-boundary"))
        {
            return sample;
        }

        if (TryPopulateValue(sample, caseId, fixture)
            || TryPopulateAggregate(sample, caseId, fixture)
            || TryPopulateMetadata(sample, caseId, fixture)
            || TryPopulatePreprocessing(sample, caseId, fixture))
        {
            return sample;
        }

        PopulateCollection(sample, caseId);
        return sample;
    }
}
