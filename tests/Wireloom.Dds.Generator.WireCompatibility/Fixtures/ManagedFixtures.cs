#nullable enable annotations

using Rti.Dds.Topics;
using Rti.Types.Dynamic;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Creates managed fixtures for cases that need typed values or cannot use public DynamicType.</summary>
internal static partial class ManagedFixtures
{
    /// <summary>Creates the expected managed sample using the selected corpus fixture.</summary>
    internal static T Create<T>(PeerOptions options, ISerializer<T> serializer, DynamicData? expectedDynamic)
    {
        if (options.CaseId == "06-aggregates" && options.Fixture == "direct-recursive-sequence")
        {
            return CreateDirectRecursiveSequenceFixture<T>();
        }

        return options.CaseId switch
        {
#if WIRE_COMPATIBILITY_SEQUENCE_ARRAYS
            "05-array-of-sequences-idl" => CreateSequenceArrayFixture<T>(options.Fixture),
#endif
            "09-defaults-ranges" => CreateDefaultsAndRangesFixture<T>(options.Fixture),
            "01-boundaries" or "01-full-widths" => CreatePrimitiveBoundaryFixture<T>(options.CaseId, options.Fixture),
            "09-optional-recursive-array" => CreateOptionalRecursiveArrayFixture<T>(options.Fixture),
            "10-default-literal-enum-array" => CreateEnumArrayDefaultFixture<T>(options.Fixture),
            "09-optional-string-sequences" => CreateOptionalStringSequenceFixture<T>(options.Fixture),
            "09-optional-aggregate-member" => CreateOptionalAggregateMemberFixture<T>(options.Fixture),
            "07-union-char-boundaries" => CreateCharacterUnionFixture<T>(options.Fixture),
#if WIRE_COMPATIBILITY_UNBOUNDED_SEQUENCE_ALIASES
            "05-unbounded-sequence-aliases" => CreateUnboundedSequenceAliasesFixture<T>(options.Fixture),
#endif
#if WIRE_COMPATIBILITY_UNION_STRING_SEQUENCE_ALIASES
            "07-union-string-sequence-aliases" => CreateStringSequenceUnionFixture<T>(options.Fixture),
#endif
#if WIRE_COMPATIBILITY_NUMERIC_CHAR_UNION
            "07-union-char-numeric" => CreateNumericCharacterUnionFixture<T>(options.Fixture),
#endif
#if WIRE_COMPATIBILITY_NUMERIC_CHAR_255_UNION
            "07-union-char-numeric-255" => CreateHighNumericCharacterUnionFixture<T>(options.Fixture),
#endif
#if WIRE_COMPATIBILITY_IDL_GAPS
            "14-extensibility-full" or "14-autoid-scope" or "14-annotation-arguments"
                or "14-member-declarators" or "14-union-label-expressions" or "14-union-scoped-enum"
                or "14-union-array-branch" or "14-string-expression-bound" or "14-typed-defaults"
                or "14-enum-constant" => CreateIdlGapFixture<T>(options),
#endif
            _ => serializer.FromDynamicData(expectedDynamic!)
        };
    }
}
