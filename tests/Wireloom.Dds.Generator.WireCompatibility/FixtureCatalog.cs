using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Builds deterministic DynamicData values for each wire fixture.</summary>
internal static class FixtureCatalog
{
    internal static DynamicData CreateFixture(
        Rti.Types.Dynamic.DynamicType dynamicType,
        string caseId,
        string fixture)
    {
        var sample = new DynamicData(dynamicType);
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
            return sample;
        }

        if (caseId == "09-optional-collections")
        {
            if (fixture is "optional-single" or "optional-multiple")
            {
                SetFixtureValue(sample, "values", fixture == "optional-single"
                    ? new[] { 9501 }
                    : new[] { 9501, 9502, 9503 });
                SetFixtureValue(sample, "items[0]", 9511);
                SetFixtureValue(sample, "items[1]", 9512);
            }
            else if (fixture == "optional-empty")
            {
                SetFixtureValue(sample, "values", Array.Empty<int>());
            }
            return sample;
        }

        if (caseId == "09-optional-string-sequences" || fixture == "default"
            || fixture.StartsWith("optional-", StringComparison.Ordinal))
        {
            return sample;
        }

        if (caseId == "02-scopes" && fixture.StartsWith("sequence-", StringComparison.Ordinal))
        {
            SetFixtureValue(sample, "event", 501);
            SetFixtureValue(sample, "relative.x", 502);
            SetFixtureValue(sample, "relative.y", 503);
            SetFixtureValue(sample, "absolute.x", 504);
            SetFixtureValue(sample, "absolute.y", 505);
            SetFixtureValue(sample, "values", fixture == "sequence-empty"
                ? Array.Empty<int>()
                : new[] { 506 });
            return sample;
        }

        if (caseId == "02-constant-expressions" && fixture.StartsWith("sequence-", StringComparison.Ordinal))
        {
            SetFixtureValue(sample, "values", fixture == "sequence-empty"
                ? Array.Empty<int>()
                : new[] { 601 });
            for (var index = 0; index < 5; index++)
            {
                SetFixtureValue(sample, $"array[{index}]", 611 + index);
            }
            return sample;
        }

        if (caseId == "03-alias-collections" && fixture.StartsWith("sequence-", StringComparison.Ordinal))
        {
            if (fixture == "sequence-single")
            {
                SetFixtureValue(sample, "values[0][0]", 1101);
            }
            return sample;
        }

        if (caseId == "05-collections" && fixture.StartsWith("sequence-", StringComparison.Ordinal))
        {
            SetFixtureValue(sample, "unbounded", fixture == "sequence-empty"
                ? Array.Empty<int>()
                : new[] { 301 });
            SetFixtureValue(sample, "bounded", fixture == "sequence-empty"
                ? Array.Empty<int>()
                : new[] { 401 });
            if (fixture == "sequence-single")
            {
                SetFixtureValue(sample, "items[0].id", 701);
                SetFixtureValue(sample, "items[0].label", "item-701");
            }
            return sample;
        }

        if (fixture == "union-number" || fixture == "union-text" || fixture == "union-default"
            || fixture == "union-payload" || fixture == "union-enabled" || fixture == "union-disabled"
            || fixture == "union-zero" || fixture == "union-ten")
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

                return sample;
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
                return sample;
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

                return sample;
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

                return sample;
            }
        }

        if (caseId.StartsWith("07-union-", StringComparison.Ordinal) || caseId == "07-unions")
        {
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
            return sample;
        }

        if (fixture != "non-default"
            && !(caseId == "03-enums-aliases" && fixture == "enum-alternate")
            && !(caseId == "04-strings" && fixture is "string-empty" or "string-boundary")
            && !(caseId == "04-boundaries" && fixture == "string-boundary"))
        {
            return sample;
        }

        if (caseId == "01-boundaries")
        {
            SetFixtureValue(sample, "shortValue", short.MinValue);
            SetFixtureValue(sample, "longValue", int.MaxValue);
            SetFixtureValue(sample, "longLongValue", long.MinValue);
            SetFixtureValue(sample, "int8Value", sbyte.MinValue);
            SetFixtureValue(sample, "int64Value", long.MaxValue);
            SetFixtureValue(sample, "floatValue", 1.25f);
            SetFixtureValue(sample, "doubleValue", -2.5d);
            return sample;
        }

        if (caseId == "01-full-widths")
        {
            SetFixtureValue(sample, "i8", (sbyte)-101);
            SetFixtureValue(sample, "i16", (short)-16001);
            SetFixtureValue(sample, "i32", -320001);
            SetFixtureValue(sample, "i64", -6400001L);
            SetFixtureValue(sample, "u8", (byte)201);
            SetFixtureValue(sample, "u16", (ushort)52001);
            SetFixtureValue(sample, "u32", 3200001U);
            SetFixtureValue(sample, "u64", 6400001UL);
            return sample;
        }

        if (caseId == "02-names-constants")
        {
            SetFixtureValue(sample, "event", 401);
            SetFixtureValue(sample, "point.x", 402);
            SetFixtureValue(sample, "point.y", 403);
            return sample;
        }

        if (caseId == "02-scopes")
        {
            SetFixtureValue(sample, "event", 501);
            SetFixtureValue(sample, "relative.x", 502);
            SetFixtureValue(sample, "relative.y", 503);
            SetFixtureValue(sample, "absolute.x", 504);
            SetFixtureValue(sample, "absolute.y", 505);
            SetFixtureValue(sample, "values", new[] { 506, 507, 508 });
            return sample;
        }

        if (caseId == "02-constant-expressions")
        {
            SetFixtureValue(sample, "values", new[] { 601, 602, 603, 604 });
            SetFixtureValue(sample, "array[0]", 611);
            SetFixtureValue(sample, "array[1]", 612);
            SetFixtureValue(sample, "array[2]", 613);
            SetFixtureValue(sample, "array[3]", 614);
            SetFixtureValue(sample, "array[4]", 615);
            return sample;
        }

        if (caseId == "02-formatting")
        {
            SetFixtureValue(sample, "text", "formatted-701");
            return sample;
        }

        if (caseId == "02-multiple")
        {
            SetFixtureValue(sample, "first.value", 801);
            SetFixtureValue(sample, "second.name", "second-801");
            return sample;
        }

        if (caseId == "03-enum-values-prefix" || caseId == "03-enum-values-explicit")
        {
            return sample;
        }

        if (caseId == "03-enums-aliases" && fixture == "enum-alternate")
        {
            SetFixtureValue(sample, "color", 2);
            SetFixtureValue(sample, "aliasColor", 1);
            return sample;
        }

        if (caseId == "03-alias-aggregate")
        {
            SetFixtureValue(sample, "point.x", 1001);
            SetFixtureValue(sample, "point.y", 1002);
            return sample;
        }

        if (caseId == "03-alias-collections")
        {
            SetFixtureValue(sample, "values[0][0]", 1101);
            SetFixtureValue(sample, "values[0][1]", 1102);
            SetFixtureValue(sample, "values[1][0]", 1111);
            SetFixtureValue(sample, "values[1][1]", 1112);
            return sample;
        }

        if (caseId == "04-strings")
        {
            if (fixture == "string-empty")
            {
                SetFixtureValue(sample, "unbounded", string.Empty);
                SetFixtureValue(sample, "bounded", string.Empty);
                SetFixtureValue(sample, "wideUnbounded", string.Empty);
                SetFixtureValue(sample, "wideBounded", string.Empty);
            }
            else if (fixture == "string-boundary")
            {
                SetFixtureValue(sample, "unbounded", "unbounded-boundary");
                SetFixtureValue(sample, "bounded", "12345678");
                SetFixtureValue(sample, "wideUnbounded", "wide-boundary");
                SetFixtureValue(sample, "wideBounded", "ABCDEFGH");
            }
            else
            {
                SetFixtureValue(sample, "unbounded", "narrow-unbounded");
                SetFixtureValue(sample, "bounded", "bound801");
                SetFixtureValue(sample, "wideUnbounded", "wide-unbounded");
                SetFixtureValue(sample, "wideBounded", "wide801");
            }
            return sample;
        }

        if (caseId == "04-boundaries")
        {
            SetFixtureValue(sample, "narrow", fixture == "string-boundary" ? "12345678" : "edge-8");
            SetFixtureValue(sample, "wide", fixture == "string-boundary" ? "ABCDEFGH" : "wide-8");
            return sample;
        }

        if (caseId == "06-alias-composition")
        {
            SetFixtureValue(sample, "id", 1201);
            SetFixtureValue(sample, "values", new[] { 1.25f, -2.5f, 3.75f, 4.5f });
            return sample;
        }

        if (caseId == "09-data-representation" || caseId == "09-allowed-data-representation")
        {
            SetFixtureValue(sample, "value", 1301);
            return sample;
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
            return sample;
        }

        if (caseId == "06-aggregates")
        {
            SetFixtureValue(sample, "base.base", 6101);
            SetFixtureValue(sample, "derived.base", 6201);
            SetFixtureValue(sample, "derived.derived", "derived-6201");
            return sample;
        }

        if (caseId == "08-keys")
        {
            SetFixtureValue(sample, "identity.tenant", 8201);
            SetFixtureValue(sample, "identity.name", "tenant-8201");
            SetFixtureValue(sample, "payload", 8202);
            return sample;
        }

        if (caseId == "08-key-nested")
        {
            SetFixtureValue(sample, "identity.code", 8301);
            SetFixtureValue(sample, "identity.revision", 8302);
            SetFixtureValue(sample, "payload", 8303);
            return sample;
        }

        if (caseId == "08-key-inherited")
        {
            SetFixtureValue(sample, "tenant", 8401);
            SetFixtureValue(sample, "baseValue", 8402);
            SetFixtureValue(sample, "localId", 8403);
            SetFixtureValue(sample, "payload", 8404);
            return sample;
        }

        if (caseId == "08-key-boundaries")
        {
            SetFixtureValue(sample, "coordinates[0]", 8501);
            SetFixtureValue(sample, "coordinates[1]", 8502);
            SetFixtureValue(sample, "payload", 8503);
            return sample;
        }

        if (caseId == "08-key-union")
        {
            SetFixtureValue(sample, "identity.number", 8601);
            SetFixtureValue(sample, "payload", 8602);
            return sample;
        }

        if (caseId == "09-extensibility")
        {
            SetFixtureValue(sample, "id", 9101);
            SetFixtureValue(sample, "text", "appendable-9101");
            return sample;
        }

        if (caseId == "09-default")
        {
            SetFixtureValue(sample, "id", 9201);
            SetFixtureValue(sample, "text", "default-9201");
            return sample;
        }

        if (caseId == "09-id-gaps")
        {
            SetFixtureValue(sample, "first", 9301);
            SetFixtureValue(sample, "seventh", 9307);
            return sample;
        }

        if (caseId == "09-autoid-hash")
        {
            SetFixtureValue(sample, "id", 9601);
            SetFixtureValue(sample, "text", "autoid-9601");
            return sample;
        }

        if (caseId == "09-defaults-ranges")
        {
            SetFixtureValue(sample, "value", 97);
            SetFixtureValue(sample, "ranged", -17);
            return sample;
        }

        if (caseId == "10-annotations")
        {
            SetFixtureValue(sample, "id", 10101);
            SetFixtureValue(sample, "value", 10);
            return sample;
        }

        if (caseId == "10-annotations-extended")
        {
            SetFixtureValue(sample, "id", 10201);
            SetFixtureValue(sample, "value", 11);
            return sample;
        }

        if (caseId == "10-annotations-rti")
        {
            SetFixtureValue(sample, "id", 10301);
            SetFixtureValue(sample, "value", -13);
            SetFixtureValue(sample, "text", "annotation-10301");
            SetFixtureValue(sample, "externalText", "external-10301");
            return sample;
        }

        if (caseId == "10-flat-data-binding")
        {
            SetFixtureValue(sample, "id", 10401);
            SetFixtureValue(sample, "value", 14);
            return sample;
        }

        if (caseId == "11-preprocessing" || caseId == "11-include-search")
        {
            var baseValue = caseId == "11-preprocessing" ? 1101 : 1161;
            SetFixtureValue(sample, "item.value", baseValue);
            SetFixtureValue(sample, "value", baseValue + 1);
            return sample;
        }

        if (caseId == "11-conditionals")
        {
            SetFixtureValue(sample, "value", 1111);
            return sample;
        }

        if (caseId == "11-macros")
        {
            SetFixtureValue(sample, "value", 1121);
            return sample;
        }

        if (caseId == "11-macro-operators")
        {
            SetFixtureValue(sample, "field", 1131);
            return sample;
        }

        if (caseId == "11-preprocessor-advanced")
        {
            SetFixtureValue(sample, "value", 1151);
            return sample;
        }

        if (caseId == "11-comments")
        {
            SetFixtureValue(sample, "text", "comment");
            return sample;
        }

        if (caseId != "05-collections")
        {
            return sample;
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

        return sample;
    }

    private static void SetFixtureValue(DynamicData sample, string memberName, object value)
    {
        try
        {
            sample.SetAnyValue(memberName, value);
        }
        catch (Exception exception)
        {
            throw new FixtureException(
                $"Could not set fixture member '{memberName}' ({exception.GetType().Name}).");
        }
    }

}
