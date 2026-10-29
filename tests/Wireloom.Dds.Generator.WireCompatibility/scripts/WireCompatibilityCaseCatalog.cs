using System.Collections.Immutable;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Owns the named fixtures used to exercise each wire-compatible corpus case.</summary>
internal static class WireCompatibilityCaseCatalog
{
    private static readonly ImmutableHashSet<string> NonDefaultCases = new HashSet<string>(StringComparer.Ordinal)
    {
        "01-boundaries", "01-full-widths", "02-names-constants", "02-scopes", "02-constant-expressions",
        "02-formatting", "02-multiple", "03-alias-aggregate", "03-alias-collections", "04-strings",
        "04-boundaries", "04-unbounded-string-aliases", "04-string-alias-chains", "05-unbounded-sequence-aliases",
        "05-collections", "05-shapes", "06-aggregates", "06-alias-composition", "07-unions", "07-union-enum",
        "07-union-multilabel", "07-union-short", "07-union-char", "07-union-char-numeric",
        "07-union-char-numeric-255", "07-union-scoped", "07-union-boolean", "07-union-aliases",
        "07-union-wchar-label", "07-union-string-sequence-aliases", "07-union-string-sequence-bounded-direct",
        "08-keys", "08-key-nested", "08-key-inherited", "08-key-boundaries", "08-key-union", "09-extensibility",
        "09-default", "09-id-gaps", "09-autoid-hash", "09-must-understand", "09-defaults-ranges",
        "10-annotations", "10-annotations-extended", "10-annotations-rti", "10-flat-data-binding",
        "09-data-representation", "09-allowed-data-representation", "11-preprocessing", "11-conditionals",
        "11-macros", "11-macro-operators", "11-preprocessor-advanced", "11-include-search", "11-comments",
    }.ToImmutableHashSet(StringComparer.Ordinal);

    /// <summary>Returns the unique fixture names for a case and exact RTI profile.</summary>
    internal static IReadOnlyList<string> FixturesFor(string caseId, string rtiVersion)
    {
        string[] fixtures = caseId switch
        {
            "05-array-of-sequences-idl" => ["sequence-empty", "sequence-unequal", "sequence-mixed-empty", "sequence-boundary"],
            "03-enums-aliases" => ["default", "enum-alternate"],
            "02-scopes" or "02-constant-expressions" or "05-collections" => ["sequence-empty", "sequence-single", "non-default"],
            "04-strings" => ["string-empty", "string-boundary", "non-default"],
            "04-unbounded-string-aliases" => ["string-empty", "string-over-default-bound"],
            "04-string-alias-chains" => ["string-empty", "string-boundary"],
            "05-unbounded-sequence-aliases" => ["sequence-empty", "sequence-multiple"],
            "06-aggregates" => ["non-default", "direct-recursive-sequence"],
            "04-boundaries" => ["string-boundary", "non-default"],
            "03-alias-collections" => ["sequence-empty", "sequence-single", "non-default"],
            "09-evolution-optional" => ["optional-absent", "optional-value-only", "optional-text-only", "optional-present"],
            "09-optional-collections" => ["optional-absent", "optional-empty", "optional-single", "optional-multiple"],
            "09-optional-recursive-array" => ["optional-absent", "optional-present", "optional-nested"],
            "09-optional-aggregate-member" => ["optional-absent", "optional-present"],
            "09-optional-string-sequences" => ["optional-absent", "optional-narrow-only", "optional-wide-only", "optional-empty", "optional-multiple"],
            "07-unions" => ["union-number", "union-text", "union-default"],
            "07-union-enum" => ["union-number", "union-text", "union-payload"],
            "07-union-multilabel" => ["union-number", "union-text"],
            "07-union-short" or "07-union-char" or "07-union-wchar-label" => ["union-number", "union-text", "union-default"],
            "07-union-boolean" => ["union-enabled", "union-disabled"],
            "07-union-octet" or "07-union-short-boundaries" or "07-union-long" or "07-union-ushort"
                or "07-union-ulong" or "07-union-int8" or "07-union-uint8" or "07-union-int16"
                or "07-union-uint16" or "07-union-int32" or "07-union-uint32" or "07-union-char-boundaries"
                => ["union-minimum", "union-maximum", "union-default"],
            "07-union-scoped" => ["union-payload", "sequence-empty", "sequence-single"],
            "07-union-aliases" => ["union-zero", "union-ten"],
            "07-union-char-numeric" => ["union-127", "union-default"],
            "07-union-char-numeric-255" => ["union-255", "union-default"],
            "10-resolve-name-wstring-aliases" => ["string-empty", "string-boundary"],
            "07-union-string-sequence-aliases" => ["union-direct-narrow", "union-direct-wide", "union-alias-narrow", "union-alias-wide", "union-bounded-narrow", "union-bounded-wide"],
            "07-union-string-sequence-bounded-direct" => ["union-narrow-empty", "union-narrow-multiple", "union-wide-empty", "union-wide-multiple"],
            "14-extensibility-full" or "14-member-declarators" => ["non-default"],
            "14-autoid-scope" => ["scope-nested"],
            "14-annotation-arguments" => ["default", "non-default"],
            "14-union-label-expressions" => ["symbolic", "arithmetic", "hexadecimal"],
            "14-union-scoped-enum" => ["enum-selected", "enum-first"],
            "14-union-array-branch" => ["array-branch", "scalar-branch"],
            "14-string-expression-bound" => ["string-boundary"],
            "14-typed-defaults" when rtiVersion == "7.7.0" => ["default", "non-default", "sparse-omitted"],
            "14-typed-defaults" => ["default", "non-default"],
            "14-enum-constant" => ["constant-and-label"],
            _ => [NonDefaultCases.Contains(caseId) ? "non-default" : "default"],
        };

        if (fixtures.Distinct(StringComparer.Ordinal).Count() != fixtures.Length)
        {
            throw new InvalidDataException($"Fixture catalog for {caseId} contains duplicate names.");
        }

        return fixtures;
    }
}
