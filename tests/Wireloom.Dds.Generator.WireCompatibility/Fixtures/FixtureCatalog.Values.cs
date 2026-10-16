using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.WireCompatibility;

internal static partial class FixtureCatalog
{
    // Returns true when this group owns the case and has populated its fixture.
    private static bool TryPopulateValue(DynamicData sample, string caseId, string fixture)
    {
        if (caseId == "01-boundaries")
        {
            SetFixtureValue(sample, "shortValue", short.MinValue);
            SetFixtureValue(sample, "longValue", int.MaxValue);
            SetFixtureValue(sample, "longLongValue", long.MinValue);
            SetFixtureValue(sample, "int8Value", sbyte.MinValue);
            SetFixtureValue(sample, "int64Value", long.MaxValue);
            SetFixtureValue(sample, "floatValue", 1.25f);
            SetFixtureValue(sample, "doubleValue", -2.5d);
            return true;
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
            return true;
        }

        if (caseId == "02-names-constants")
        {
            SetFixtureValue(sample, "event", 401);
            SetFixtureValue(sample, "point.x", 402);
            SetFixtureValue(sample, "point.y", 403);
            return true;
        }

        if (caseId == "02-scopes")
        {
            SetFixtureValue(sample, "event", 501);
            SetFixtureValue(sample, "relative.x", 502);
            SetFixtureValue(sample, "relative.y", 503);
            SetFixtureValue(sample, "absolute.x", 504);
            SetFixtureValue(sample, "absolute.y", 505);
            SetFixtureValue(sample, "values", new[] { 506, 507, 508 });
            return true;
        }

        if (caseId == "02-constant-expressions")
        {
            SetFixtureValue(sample, "values", new[] { 601, 602, 603, 604 });
            SetFixtureValue(sample, "array[0]", 611);
            SetFixtureValue(sample, "array[1]", 612);
            SetFixtureValue(sample, "array[2]", 613);
            SetFixtureValue(sample, "array[3]", 614);
            SetFixtureValue(sample, "array[4]", 615);
            return true;
        }

        if (caseId == "02-formatting")
        {
            SetFixtureValue(sample, "text", "formatted-701");
            return true;
        }

        if (caseId == "02-multiple")
        {
            SetFixtureValue(sample, "first.value", 801);
            SetFixtureValue(sample, "second.name", "second-801");
            return true;
        }

        if (caseId == "03-enum-values-prefix" || caseId == "03-enum-values-explicit")
        {
            return true;
        }

        if (caseId == "03-enums-aliases" && fixture == "enum-alternate")
        {
            SetFixtureValue(sample, "color", 2);
            SetFixtureValue(sample, "aliasColor", 1);
            return true;
        }

        if (caseId == "03-alias-aggregate")
        {
            SetFixtureValue(sample, "point.x", 1001);
            SetFixtureValue(sample, "point.y", 1002);
            return true;
        }

        if (caseId == "03-alias-collections")
        {
            SetFixtureValue(sample, "values[0][0]", 1101);
            SetFixtureValue(sample, "values[0][1]", 1102);
            SetFixtureValue(sample, "values[1][0]", 1111);
            SetFixtureValue(sample, "values[1][1]", 1112);
            return true;
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
            return true;
        }

        if (caseId == "04-boundaries")
        {
            SetFixtureValue(sample, "narrow", fixture == "string-boundary" ? "12345678" : "edge-8");
            SetFixtureValue(sample, "wide", fixture == "string-boundary" ? "ABCDEFGH" : "wide-8");
            return true;
        }

        return false;
    }
}
