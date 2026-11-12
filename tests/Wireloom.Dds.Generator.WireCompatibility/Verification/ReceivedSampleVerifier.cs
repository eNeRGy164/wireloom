#nullable enable annotations

using Rti.Types.Dynamic;
using Wireloom.Dds.CSharp.TestSupport;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Selects the payload comparison appropriate to each corpus case.</summary>
internal static class ReceivedSampleVerifier
{
    /// <summary>Checks a received sample without changing the case's established comparison contract.</summary>
    internal static void Verify<T>(T expected, T sample, DynamicData? expectedDynamic, PeerOptions options)
    {
        var context = $"{options.CaseId} fixture {options.Fixture}";
#if WIRE_COMPATIBILITY_UNBOUNDED_SUPPORT
        if (options.CaseId == "14-unbounded-support")
        {
            ManagedFixtures.VerifyUnboundedSupportFixture(sample, options);
            return;
        }
#endif
        if (options.CaseId.StartsWith("14-", StringComparison.Ordinal))
        {
            ManagedFixtures.VerifyIdlGapFixture(sample!, options);
            return;
        }

        if (options.CaseId == "06-aggregates" && options.Fixture == "direct-recursive-sequence")
        {
            VerifyTypedValue(expected!, sample, context);
            return;
        }

        switch (options.CaseId)
        {
#if WIRE_COMPATIBILITY_SEQUENCE_ARRAYS
            case "05-array-of-sequences-idl":
                ManagedFixtures.VerifySequenceArrayFixture(sample!, options.Fixture);
                break;
#endif
            case "09-defaults-ranges":
                IndependentSampleVerifier.VerifyDefaultsAndRangesFixture(sample!, context);
                break;
            case "01-boundaries":
            case "01-int8-boundaries":
            case "01-full-widths":
                IndependentSampleVerifier.VerifyPrimitiveBoundaryFixture(sample!, options.CaseId, context);
                break;
            case "07-union-char-boundaries":
                IndependentSampleVerifier.VerifyCharacterUnionFixture(expectedDynamic!.Type, sample!, options.Fixture);
                break;
            case "09-optional-string-sequences":
                IndependentSampleVerifier.VerifyOptionalStringSequenceFixture(sample!, options.Fixture);
                break;
            case "09-optional-recursive-array":
                IndependentSampleVerifier.VerifyOptionalRecursiveArrayFixture(expected!, sample!, context);
                break;
            case "10-default-literal-enum-array":
                VerifyTypedValue(expected, sample, context);
                break;
            case "09-optional-aggregate-member":
                IndependentSampleVerifier.VerifyOptionalAggregateMemberFixture(expected!, sample!, context);
                break;
            case "07-union-char-numeric":
            case "07-union-char-numeric-255":
                IndependentSampleVerifier.VerifyNumericCharacterUnionFixture(sample!, options.CaseId, options.Fixture);
                break;
            case "05-unbounded-sequence-aliases":
            case "07-union-string-sequence-aliases":
                VerifyTypedValue(expected, sample, context);
                break;
            case "07-union-int8":
                VerifyInt8UnionValue(expected!, sample!, options.Fixture, context);
                break;
            default:
                IndependentSampleVerifier.Verify(expectedDynamic!, sample!, context);
                break;
        }
    }

    private static void VerifyTypedValue<T>(T expected, T actual, string context)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new InvalidDataException($"Expected {expected}; received {actual} for {context}.");
        }
    }

    private static void VerifyInt8UnionValue<T>(T expected, T actual, string fixture, string context)
    {
        var discriminatorProperty = typeof(T).GetProperty("Discriminator")
            ?? throw new InvalidDataException($"The signed int8 union has no discriminator property for {context}.");
        var expectedDiscriminator = NormalizeInt8Discriminator(discriminatorProperty.GetValue(expected));
        var actualDiscriminator = NormalizeInt8Discriminator(discriminatorProperty.GetValue(actual));
        if (expectedDiscriminator != actualDiscriminator)
        {
            throw new InvalidDataException(
                $"Expected signed int8 discriminator {expectedDiscriminator}; received {actualDiscriminator} for {context}.");
        }

        var branch = fixture switch
        {
            "union-minimum" => "minimum",
            "union-maximum" => "maximum",
            "union-default" => "fallback",
            _ => throw new FixtureException($"Unknown signed int8 union fixture '{fixture}'.")
        };
        var branchProperty = typeof(T).GetProperty(branch)
            ?? throw new InvalidDataException($"The signed int8 union has no '{branch}' branch for {context}.");
        var expectedValue = branchProperty.GetValue(expected);
        var actualValue = branchProperty.GetValue(actual);
        if (!Equals(expectedValue, actualValue))
        {
            throw new InvalidDataException(
                $"Expected '{branch}' value {expectedValue}; received {actualValue} for {context}.");
        }
    }

    private static long NormalizeInt8Discriminator(object? value)
    {
        if (value is null)
        {
            throw new InvalidDataException("The signed int8 union discriminator is null.");
        }

        var numericValue = Convert.ToInt64(value, System.Globalization.CultureInfo.InvariantCulture);
        return numericValue > sbyte.MaxValue ? numericValue - (sbyte.MaxValue - sbyte.MinValue + 1L) : numericValue;
    }
}
