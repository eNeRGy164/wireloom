#nullable enable annotations

using Rti.Types.Dynamic;
using Wireloom.Dds.Generator.TestSupport;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Selects the payload comparison appropriate to each corpus case.</summary>
internal static class ReceivedSampleVerifier
{
    /// <summary>Checks a received sample without changing the case's established comparison contract.</summary>
    internal static void Verify<T>(T expected, T sample, DynamicData? expectedDynamic, PeerOptions options)
    {
        var context = $"{options.CaseId} fixture {options.Fixture}";
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
}
