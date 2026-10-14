# Optional recursive fixed-array evidence

Issue [#19](https://github.com/eNeRGy164/wireloom/issues/19) permits optional
fixed arrays to break value-layout cycles. C134 is the retained IDL/C# oracle
case. Required inline cycles continue to fail before emission.

## RTI C# comparison

Captured with RTI Connext 7.7.0 / rtiddsgen 4.7.0.1 on October 14, 2026.
Both C# and C++11 generation accepted the original IDL with `-ppDisable`.
Retained oracle notices and generated sources are unchanged.

| Behavior | Verified contract |
| --- | --- |
| Optional presence | Default-created nodes have null children; present arrays have two elements |
| Managed copy | Independent array containers and recursively copied child nodes |
| Hashing | Add the first child when present; add `-1` when absent |
| Equality | Compare value, presence, and child values; equal copies remain equal |
| Native storage | `NativeManagedOptionalArray`, fixed dimensions, aggregate conversion support |
| Native flags | Forward `keysOnly` for optional aggregate arrays and `optionalsOnly` for cleanup |
| Annotations | Optional child member, IDs 0/1, Int32 default/min/max metadata |
| Recursion | Array member metadata uses Support.Instance without a plugin bootstrap call; sequence metadata retains RTI's GetOrCreateInstanceImpl expression; public DynamicType remains unsupported |

## Local quality gates

Final generated-code review corrections passed:

| Gate | Result |
| --- | --- |
| Release unit suite with coverage | 1,124 passed |
| Corpus compliance | 241 passed; export-only test skipped by default |
| Source export | Passed separately; snapshots regenerated through the corpus exporter |
| Fresh packed-analyzer integration | 13 passed; RTI-compatible array plugin initialization and recursive-sequence singleton initialization included |
| Branch coverage | 4,635 / 4,931 (94.00%) |
| Line coverage | 6,807 / 6,881 (98.92%) |
| Community Qodana | Completed with zero findings; pinned `global.json` remained visible |
| Authored-source whitespace check | Passed; original RTI oracle whitespace is retained |

Use the commands in [CONTRIBUTING.md](../../CONTRIBUTING.md#quality-evidence).
Coverage used `coverage.settings.xml` and the Microsoft.Testing.Platform
coverage extension. Package integration used a fresh `obj/issue19-rti-parity-packages`
cache whose analyzer DLL hash matched the packed generator. Linux wire builds
used a scratch source copy to avoid overwriting Windows restore intermediates.

## Licensed wire results

The corrected source passed all **27/27** exchanges for `optional-absent`,
`optional-present`, and `optional-nested`, across all nine directed pairings of
Wireloom C#, RTI C#, and RTI C++ on exact RTI 7.7.0.
The final focused report is in the ignored local directory
`artifacts/wire-focus-issue19-rti-parity`.

The full matrix on the initial implementation completed **882** scenarios:
**861 passed**, **6 failed** (2 registered expected failures), and **15 not run**.
The affected recursive-array case was rerun after the generated-code review
corrections. The full report is in `artifacts/wire-compatibility-issue19`.

Remaining full-matrix outcomes are outside #19:

- Four array-of-sequences discovery mismatches, including two RTI C# / RTI C++
  pairings. The retained RTI C# plugin also models these members as sequences.
  Broader matrix and shape support remain tracked in
  [#29](https://github.com/eNeRGy164/wireloom/issues/29).
- Two managed-reader failures with one valid sample and one `ValidData == false`
  notification in the same take. Focused controls passed 26/27 for
  `07-union-enum` and 27/27 for `07-union-scoped`; investigation is recorded
  separately in [#46](https://github.com/eNeRGy164/wireloom/issues/46). This does not establish a generator
  serialization defect.
- Fifteen pairings involving unavailable RTI-generated C# for the wchar-label
  union, consistent with its recorded RTI parser rejection.

## Explicit test adaptations

The IDL shape remains unchanged. Results record these adaptations:

1. All three recursive-array peers disable TypeObject/TypeCode propagation.
   RTI 7.7.0 rejects recursive registration with default TypeObject v2.
   The [RTI type-representation guide](https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/extensible_types_guide/extensible_types/Type_Representation.htm)
   documents disabling propagation through participant resource limits.
2. Both managed array peers start plugin metadata creation in the harness before
   singleton access. This avoids the RTI-compatible static-initialization null
   dereference without adding a bootstrap call to either generated plugin.
   Direct singleton startup for recursive arrays is not established by this
   adapted test. Recursive sequences retain RTI's safe accessor expression.
3. Scratch-generated RTI C++ source forwards the existing visited-node chain
   into recursive array TypeCode initialization, avoiding unbounded metadata
   recursion. This changes reference initialization, not the declared type.

These results establish typed sample interoperability under those settings.
They do not establish default TypeObject v2 support or unadapted RTI reference
initialization. General mutual recursion, recursive collection typedef runtime
behavior, and other RTI versions remain outside this evidence.

## Rebase validation on October 15, 2026

The rebase preserves main's union-boundary cases, typed character fixtures,
instance-lifecycle notification handling, sharding, and combined reports.
The recursive-array case now uses C134 because main already assigned C117 to
`07-union-octet`. Existing published corpus tags remain unchanged.

- Release build: zero warnings and errors.
- Unit suite: 1,211 passed.
- Corpus compliance with source export: 276 passed across 134 cases.
- Fresh packed-analyzer integration: 13 passed.
- Branch coverage: 4,684/4,982 (94.02%).
- Line coverage: 6,822/6,890 (99.01%).

The focused recursive-array and character-boundary runs use the rebased
matrix harness; their results are retained in `artifacts/wire-focus-issue19-rebase`
and `artifacts/wire-focus-issue19-rebase-character`.

- Community Qodana: completed with zero findings after conflict resolution.
- Recursive-array wire control: 27/27 passed on the rebased harness.
- Character-boundary control: 22/27 passed; the five high-bit RTI C# payload
  failures match main's documented reference-binding limitation. They remain
  observed failures, not newly registered expected failures.
