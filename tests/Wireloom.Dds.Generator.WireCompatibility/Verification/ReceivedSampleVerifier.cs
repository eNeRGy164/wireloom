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
        switch (options.CaseId)
        {
            case "07-union-char-boundaries":
                IndependentSampleVerifier.VerifyCharacterUnionFixture(expectedDynamic!.Type, sample!, options.Fixture);
                break;
            case "09-optional-string-sequences":
                IndependentSampleVerifier.VerifyOptionalStringSequenceFixture(sample!, options.Fixture);
                break;
            case "09-optional-recursive-array":
                IndependentSampleVerifier.VerifyOptionalRecursiveArrayFixture(expected!, sample!, context);
                break;
            case "09-optional-aggregate-member":
                IndependentSampleVerifier.VerifyOptionalAggregateMemberFixture(expected!, sample!, context);
                break;
            case "05-unbounded-sequence-aliases":
            case "07-union-char-numeric":
            case "07-union-char-numeric-255":
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
