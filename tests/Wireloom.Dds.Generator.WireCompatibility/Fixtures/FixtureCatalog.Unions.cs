using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.WireCompatibility;

internal static partial class FixtureCatalog
{
    private static readonly HashSet<string> UnionBoundaryCaseIds =
    [
        "07-union-octet",
        "07-union-short-boundaries",
        "07-union-long",
        "07-union-ushort",
        "07-union-ulong",
        "07-union-int8",
        "07-union-uint8",
        "07-union-int16",
        "07-union-uint16",
        "07-union-int32",
        "07-union-uint32",
        "07-union-char-boundaries"
    ];

    // Returns true when this group owns the case and has populated its fixture.
    private static bool TryPopulateUnion(DynamicData sample, string caseId, string fixture)
    {
        if (fixture is "union-number" or "union-text" or "union-default"
            or "union-payload" or "union-enabled" or "union-disabled"
            or "union-zero" or "union-ten")
        {
            if (caseId == "07-unions")
            {
                if (fixture == "union-number")
                {
                    SetFixtureValue(sample, "number", 7301);
                }

                if (fixture == "union-text")
                {
                    SetFixtureValue(sample, "text", "union-text");
                }

                if (fixture == "union-default")
                {
                    SetFixtureValue(sample, "flag", true);
                }

                return true;
            }
            if (caseId == "07-union-enum")
            {
                if (fixture == "union-number")
                {
                    SetFixtureValue(sample, "number", 7401);
                }

                if (fixture == "union-text")
                {
                    SetFixtureValue(sample, "text", "enum-union-text");
                }

                if (fixture == "union-payload")
                {
                    SetFixtureValue(sample, "payload.code", 7401);
                    SetFixtureValue(sample, "payload.label", "payload-7401");
                }
                return true;
            }
            if (caseId == "07-union-aliases")
            {
                if (fixture == "union-zero")
                {
                    SetFixtureValue(sample, "value.zero", 8101);
                }

                if (fixture == "union-ten")
                {
                    SetFixtureValue(sample, "value.ten", "alias-ten");
                }

                return true;
            }
            if (caseId == "07-union-boolean")
            {
                if (fixture == "union-enabled")
                {
                    SetFixtureValue(sample, "enabled", 7901);
                }

                if (fixture == "union-disabled")
                {
                    SetFixtureValue(sample, "disabled", "disabled");
                }

                return true;
            }
        }

        if (caseId.StartsWith("07-union-", StringComparison.Ordinal) || caseId == "07-unions")
        {
            if (UnionBoundaryCaseIds.Contains(caseId)
                && fixture is "union-minimum" or "union-maximum" or "union-default")
            {
                var boundaryMember = fixture switch
                {
                    "union-minimum" => "minimum",
                    "union-maximum" => "maximum",
                    _ => "fallback"
                };
                SetFixtureValue(sample, boundaryMember, 8201);
                return true;
            }

            var member = fixture switch
            {
                "union-number" => caseId switch
                {
                    "07-union-short" => "negative",
                    "07-union-char" or "07-union-wchar-label" => "letter",
                    "07-union-multilabel" => "number",
                    _ => "number"
                },
                "union-text" => "text",
                "union-default" => "other",
                "union-payload" => "qualifiedPayload",
                "sequence-empty" or "sequence-single" => "values",
                _ => string.Empty
            };
            if (member == "qualifiedPayload")
            {
                SetFixtureValue(sample, "qualifiedPayload.code", 7801);
                SetFixtureValue(sample, "qualifiedPayload.label", "scoped-7801");
            }
            else if (member == "values")
            {
                SetFixtureValue(sample, "values", fixture == "sequence-empty"
                    ? Array.Empty<int>()
                    : new[] { 7811 });
            }
            else if (!string.IsNullOrEmpty(member))
            {
                var value = member switch
                {
                    "number" => caseId switch
                    {
                        "07-union-enum" => (object)7401,
                        "07-union-multilabel" => 7501,
                        _ => 7301
                    },
                    "negative" => 7601,
                    "letter" => caseId == "07-union-wchar-label" ? 17701 : 7701,
                    "text" => caseId switch
                    {
                        "07-union-enum" => "enum-union-text",
                        "07-union-multilabel" => "multi-label-text",
                        "07-union-short" => "short-union-text",
                        "07-union-char" => "char-union-text",
                        "07-union-wchar-label" => "wide-union-text",
                        _ => "union-text"
                    },
                    _ => true
                };
                SetFixtureValue(sample, member, value);
            }
            return true;
        }

        return false;
    }
}
