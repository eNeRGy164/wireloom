using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.WireCompatibility;

internal static partial class FixtureCatalog
{
    // Returns true when this group owns the case and has populated its fixture.
    private static bool TryPopulateMetadata(DynamicData sample, string caseId, string fixture)
    {
        if (caseId == "08-keys")
        {
            SetFixtureValue(sample, "identity.tenant", 8201);
            SetFixtureValue(sample, "identity.name", "tenant-8201");
            SetFixtureValue(sample, "payload", 8202);
            return true;
        }

        if (caseId == "08-key-nested")
        {
            SetFixtureValue(sample, "identity.code", 8301);
            SetFixtureValue(sample, "identity.revision", 8302);
            SetFixtureValue(sample, "payload", 8303);
            return true;
        }

        if (caseId == "08-key-inherited")
        {
            SetFixtureValue(sample, "tenant", 8401);
            SetFixtureValue(sample, "baseValue", 8402);
            SetFixtureValue(sample, "localId", 8403);
            SetFixtureValue(sample, "payload", 8404);
            return true;
        }

        if (caseId == "08-key-boundaries")
        {
            SetFixtureValue(sample, "coordinates[0]", 8501);
            SetFixtureValue(sample, "coordinates[1]", 8502);
            SetFixtureValue(sample, "payload", 8503);
            return true;
        }

        if (caseId == "08-key-union")
        {
            SetFixtureValue(sample, "identity.number", 8601);
            SetFixtureValue(sample, "payload", 8602);
            return true;
        }

        if (caseId == "09-extensibility")
        {
            SetFixtureValue(sample, "id", 9101);
            SetFixtureValue(sample, "text", "appendable-9101");
            return true;
        }

        if (caseId == "09-default")
        {
            SetFixtureValue(sample, "id", 9201);
            SetFixtureValue(sample, "text", "default-9201");
            return true;
        }

        if (caseId == "09-id-gaps")
        {
            SetFixtureValue(sample, "first", 9301);
            SetFixtureValue(sample, "seventh", 9307);
            return true;
        }

        if (caseId == "09-autoid-hash")
        {
            SetFixtureValue(sample, "id", 9601);
            SetFixtureValue(sample, "text", "autoid-9601");
            return true;
        }

        if (caseId == "09-defaults-ranges")
        {
            SetFixtureValue(sample, "value", 97);
            SetFixtureValue(sample, "ranged", -17);
            return true;
        }

        if (caseId == "10-annotations")
        {
            SetFixtureValue(sample, "id", 10101);
            SetFixtureValue(sample, "value", 10);
            return true;
        }

        if (caseId == "10-annotations-extended")
        {
            SetFixtureValue(sample, "id", 10201);
            SetFixtureValue(sample, "value", 11);
            return true;
        }

        if (caseId == "10-annotations-rti")
        {
            SetFixtureValue(sample, "id", 10301);
            SetFixtureValue(sample, "value", -13);
            SetFixtureValue(sample, "text", "annotation-10301");
            SetFixtureValue(sample, "externalText", "external-10301");
            return true;
        }

        if (caseId == "10-flat-data-binding")
        {
            SetFixtureValue(sample, "id", 10401);
            SetFixtureValue(sample, "value", 14);
            return true;
        }

        return false;
    }
}
