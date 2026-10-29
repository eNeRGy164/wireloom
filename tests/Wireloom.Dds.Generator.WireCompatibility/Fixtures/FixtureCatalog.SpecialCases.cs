using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.WireCompatibility;

internal static partial class FixtureCatalog
{
    // Returns true when this group owns the case and has populated its fixture.
    private static bool TryPopulateSpecialCase(DynamicData sample, string caseId, string fixture)
    {
        if (caseId == "04-unbounded-string-aliases")
        {
            SetFixtureValue(sample, "narrow", fixture == "string-empty" ? string.Empty : new string('n', 300));
            SetFixtureValue(sample, "wide", fixture == "string-empty" ? string.Empty : new string('λ', 300));
            return true;
        }

        if (caseId == "04-string-alias-chains")
        {
            var empty = fixture == "string-empty";
            var narrow = empty ? string.Empty : "1234567890123456";
            var wide = empty ? string.Empty : "12345678";
            foreach (var member in new[] { "narrow", "narrowChain" })
            {
                SetFixtureValue(sample, member, narrow);
            }
            foreach (var member in new[] { "boundedNarrow", "boundedNarrowChain", "boundedNarrowChain2" })
            {
                SetFixtureValue(sample, member, narrow);
            }
            foreach (var member in new[] { "wide", "wideChain" })
            {
                SetFixtureValue(sample, member, wide);
            }
            foreach (var member in new[] { "boundedWide", "boundedWideChain", "boundedWideChain2" })
            {
                SetFixtureValue(sample, member, wide);
            }
            return true;
        }

        if (caseId == "10-resolve-name-wstring-aliases")
        {
            var empty = fixture == "string-empty";
            if (!empty && fixture != "string-boundary")
            {
                throw new FixtureException($"Unknown wstring alias fixture '{fixture}'.");
            }

            SetFixtureValue(sample, "firstLevel", empty ? string.Empty : "12345678");
            SetFixtureValue(sample, "secondLevel", empty ? string.Empty : "ABCDEFGH");
            return true;
        }

        if (caseId == "05-unbounded-sequence-aliases")
        {
            var empty = fixture == "sequence-empty";
            SetFixtureValue(sample, "numbers", empty ? [] : new[] { 101, -102 });
            SetFixtureValue(sample, "states", empty ? [] : new[] { 0, 1 });
            SetFixtureValue(sample, "scalars", empty ? [] : new[] { 201, -202 });
            SetFixtureValue(sample, "texts", empty ? [] : new[] { "narrow-1", "narrow-2" });
            SetFixtureValue(sample, "wideTexts", empty ? [] : new[] { "wide-λ", "wide-2" });
            SetFixtureValue(sample, "boundedTexts", empty ? [] : new[] { "12345678", "bounded" });
            SetFixtureValue(sample, "boundedWideTexts", empty ? [] : new[] { "ABCD", "wide" });
            if (!empty)
            {
                SetFixtureValue(sample, "children[0].value", 301);
                SetFixtureValue(sample, "children[1].value", -302);
            }
            return true;
        }

        if (caseId == "07-union-string-sequence-aliases")
        {
            var memberPath = fixture switch
            {
                "union-direct-narrow" => "direct.narrow",
                "union-direct-wide" => "direct.wide",
                "union-alias-narrow" => "aliases.narrow",
                "union-alias-wide" => "aliases.wide",
                "union-bounded-narrow" => "aliases.boundedNarrow",
                "union-bounded-wide" => "aliases.boundedWide",
                _ => throw new FixtureException($"Unknown string-sequence union fixture '{fixture}'.")
            };
            var wide = fixture.EndsWith("wide", StringComparison.Ordinal);
            SetFixtureValue(sample, $"{memberPath}[0]", wide ? "wide-λ" : "narrow-1");
            SetFixtureValue(sample, $"{memberPath}[1]", wide ? "wide-2" : "narrow-2");
            SetFixtureValue(sample, $"{memberPath}[2]", wide ? "wide-3" : "narrow-3");
            return true;
        }

        if (caseId == "07-union-string-sequence-bounded-direct")
        {
            var wide = fixture.StartsWith("union-wide", StringComparison.Ordinal);
            var member = wide ? "wide" : "narrow";
            if (fixture.EndsWith("multiple", StringComparison.Ordinal))
            {
                SetFixtureValue(sample, $"{member}[0]", wide ? "ABCD" : "12345678");
                SetFixtureValue(sample, $"{member}[1]", wide ? "WIDE" : "87654321");
                SetFixtureValue(sample, $"{member}[2]", wide ? "LAST" : "ABCDEFGH");
            }
            else
            {
                SetFixtureValue(sample, member, Array.Empty<string>());
            }
            return true;
        }

        if (caseId == "09-evolution-optional")
        {
            SetFixtureValue(sample, "requiredValue", 9401);
            if (fixture is "optional-value-only" or "optional-present")
            {
                SetFixtureValue(sample, "optionalValue", 9402);
            }
            if (fixture is "optional-text-only" or "optional-present")
            {
                SetFixtureValue(sample, "optionalText", "optional-present");
            }
            return true;
        }

        if (caseId == "09-optional-collections")
        {
            if (fixture is "optional-single" or "optional-multiple")
            {
                SetFixtureValue(sample, "values", fixture == "optional-single"
                    ? [9501]
                    : new[] { 9501, 9502, 9503 });
                SetFixtureValue(sample, "items[0]", 9511);
                SetFixtureValue(sample, "items[1]", 9512);
            }
            else if (fixture == "optional-empty")
            {
                SetFixtureValue(sample, "values", Array.Empty<int>());
            }
            return true;
        }

        return false;
    }
}
