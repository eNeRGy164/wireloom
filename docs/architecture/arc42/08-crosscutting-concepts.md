# 8. Cross-cutting concepts

## 8.1 Evidence vocabulary

The project distinguishes RTI acceptance, managed implementation, generated
contract shape, runtime eligibility, and verified wire compatibility. The
corpus feature index is the authoritative status view for source generation.
The generator selects an immutable emission profile from the resolved
`Rti.ConnextDds` assembly identity. Missing and below-minimum references report
`DDSG0003`; versions 7.3.1 through below 7.7.0 select the 7.3.1 profile, and
7.7.0 or later select the 7.7.0 profile. No separate target-version override
is exposed. Only exact 7.3.1 and 7.7.0 have qualification status; mapping other
versions to a profile does not assert that they have been tested. The root
corpus IDL is shared, with version-scoped oracle and generated output folders.

Oracle comparisons always use the selected exact LTS capture, including its
source directories and generation outcomes. Wire planning separately records
`notApplicableCases` for features unavailable in the selected RTI version.
Those cases retain explanatory evidence but contribute no exchanges,
compatibility failures, or Wireloom implementation gaps. Requesting such a
case explicitly fails before peer builds and type-support discovery. New oracle
captures and Wireloom exports retain source revision/dirty state, input and
output hashes, tool versions, arguments, and outcomes. Legacy missing provenance
remains unknown; unattributed local totals cannot establish qualification. The
[LTS runbook](../../corpus/LTS-PROFILES.md) owns reproduction commands and
attribution requirements.
Feature cases isolate availability boundaries: C040 (`09-autoid-hash`)
checks hash member IDs on both LTS versions; C142 (`09-must-understand`) is
not applicable to 7.3.1. The shared positive inventory contains 81 cases, of
which 80 apply to 7.3.1 and all 81 apply to 7.7.0. Wire evidence is tied to
the exact case inputs and profile; changed inputs require fresh exchanges.

## 8.2 Diagnostics and source locations

Generator-owned diagnostics use stable IDs. IDL failures retain the original
input path and offset so a consumer can fix the source rather than inspect a
generated file. Unsupported or invalid input that prevents safe managed output
is rejected with `DDSG0001`. Recognized but unsupported annotation uses report
`DDSG0102` before generation fails with `DDSG0001`.

Target-profile incompatibilities have a separate error path. The 7.3.1 profile
does not support `@must_understand`; using it reports `DDSG0004` at the source
location and emits no generated sources. The 7.7.0 profile preserves it in
DynamicType metadata.

Some constructs deliberately continue with a warning or informational
diagnostic: unknown annotations are ignored with `DDSG0101`, IDL interfaces are accepted without
emitting C# interface types (`DDSG0103`), and direct arrays of sequences warn
that the default generated mapping may not preserve IDL semantics (`DDSG0105`).
The opt-in `Idl` sequence-array mapping preserves both layers and omits that
warning. Invalid mapping values and unsupported recursive direct sequence arrays
produce source-located `DDSG0001` before emission. Macro
argument-count mismatches and active `#warning` directives also report
warnings while preprocessing continues (`DDSG0104`/`DDSG0106`); active
`#pragma message` directives are informational (`DDSG0107`). The package guide
documents the adopter impact and how to promote selected diagnostics to errors.

Naming violations report `DDSG0108` while generation continues by default.
Setting `Strict="true"` on any generating root instead rejects case-variant
keywords, same-scope case-only names, and new keys in derived aggregates with
`DDSG0001` before emission. Exact reserved keywords fail in both modes.
Strictness applies to the batch; a non-generating input cannot enable it.

Interface bodies are parsed for declared identifiers even when `DDSG0103`
reports omitted bindings. Exact reserved keywords fail in every body.
Following RTI, ordinary interface bodies do not apply specification warnings;
`@service("DDS")` bodies apply keyword-case and scope-collision rules, with
separate parameter scopes per operation. Body types are not emitted, and full
interface type resolution and RPC semantics remain outside this validation.

Compiler warning policies affect diagnostic presentation, not semantic
acceptance. `DDSG0108` can fail a warnings-as-errors build in default mode;
suppressing it cannot bypass a strict failure. The
[configuration reference](../../guides/configuration.md#enable-strict-validation)
describes the rules, warning exemptions, and differences from RTI's mode.

## 8.3 Determinism and invalidation

Physical IDL files are canonical inputs. Shared files are emitted once per
project, generated hint names are deterministic, and edits to tracked includes
invalidate generation. The current measured behavior is recorded in the
[incremental generator baseline](../../performance/incremental-generator-baseline.md):
unchanged and unrelated non-IDL inputs remain cached, while tracked IDL edits
invalidate the shared source-output batch. Per-root incremental caching is not
yet implemented.

## 8.4 Dependency and supply-chain boundaries

The consumer owns the RTI runtime reference. Roslyn compiler packages remain
private build dependencies. Central package management, lock files, NuGet audit,
pinned GitHub Actions, SBOM generation, and provenance attestation protect the
package path.

## 8.5 Test separation

Fast in-memory tests validate compiler invariants and the independent emission
plans; characterization tests protect current diagnostic and generated-output
contracts; corpus tests validate feature-level acceptance and required public
output shape; packed-package tests validate the actual NuGet consumption path.
The corpus shape comparison does not establish exact generated-source, native
support, runtime, or wire equivalence.

The separate [runtime contract suite](../../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md)
uses build-time generation and strongly typed calls to managed, native, and
type-support APIs. Focused IDL fixtures and tests are grouped by responsibility.
Windows and Linux validation complement maintained generator source coverage;
generated fixture files and test project code are excluded from Coveralls.
The packed-package suite links these runtime contracts and runs them against
the published analyzer assets, including a .NET 8-only compiler-host container.
Explicit skipped contracts and opt-in compile reproductions retain unresolved
findings; they do not establish a complete API contract or wire compatibility.

## 8.6 RTI-compatible generated value semantics

The generated API and native plugin are compatibility artifacts. The goal is
RTI C# or better: RTI's observable behavior provides a baseline and evidence,
not a ceiling. Existing quirks remain characterized until an intentional
behavior change is implemented and documented. RTI C++ wire support can justify
a more capable managed API; interoperability claims still require wire
verification. The current behavior below is not a requirement to reproduce
every RTI limitation in future APIs.

Alias diagnostic formatting intentionally improves on RTI C#'s unsupported
scalar and collection root formatting. Struct and union formatting continues
through RTI; aliases needing a fallback use a hidden typed instance method in
their existing support class rather than embedding loops in the data class.
Invariant numbers, readable Unicode, quoted collection strings, and nested
array dimensions are executable runtime contracts. This is diagnostic output,
with separate evidence from serialization and DDS interoperability.

- Array hash codes use the first element, not the array length. This preserves
  RTI's intentionally weak hash behavior. Empty arrays are not a supported
  input for this generated hash path.
- Primitive arrays use direct `SequenceEqual`. Arrays containing aggregate
  elements use the casted `Cast<T>().SequenceEqual(...)` path required by the
  runtime collection API.
- Newly allocated CLR arrays use zero initialization unless the element type
  is an enum with a declared `@default_literal`. In that case, generated
  managed and native initialization fills every array element with the declared
  literal, including typedef arrays, enum alias chains, and multidimensional
  members. Caller-supplied arrays remain caller owned by the value constructor.
  C148 and the runtime contract suite verify this behavior.
- Sequence members with array dimensions retain those dimensions in the
  emission model even though the managed public shape remains the RTI sequence
  shape by default (`RtiCSharp`). Integer sequence arrays use `NativeSeq`; bounded string sequence
  arrays use `NativeStringSeq` with the string bound passed through
  `FromNative`, `Initialize`, and `ToNative`.
- `DdsIdlSequenceArrayMapping=Idl` opts into faithful direct sequence arrays;
  file metadata `SequenceArrayMapping` overrides the project default. Included
  declarations use the included file's setting, independent of root order.
  The generated API uses arrays of sequence wrappers with mutable `Value`
  containers and shared native-array lifecycle paths. The enclosing metadata
  contains anonymous sequence elements rather than synthetic alias or struct
  layers. Runtime fixtures verify numeric, string, enum, scalar-alias, aggregate,
  optional, multidimensional, and inherited cases on both profiles. Recursive
  direct sequence arrays remain unsupported in this mode; DDS wire qualification
  is separate.
- Direct narrow and wide string-sequence union branches use specialized native
  storage and pass element bounds through initialization and conversion.
  Typedef-backed union branches delegate through their generated native alias
  wrappers. Runtime API coverage is separate from DDS wire verification.
- `optionalsOnly` cleanup is context-sensitive. Types with direct optional
  members destroy those members and return before required cleanup. Derived
  types return after base cleanup because the base may contain optional
  members. Direct strings, nested aggregates, aggregate-element collections,
  and sequence arrays retain the RTI outer guard. Primitive-only fields and
  primitive-only collections delegate cleanup to their member wrapper and do
  not need an outer guard.

## 8.7 Deterministic preprocessing safety

Preprocessing is finite by contract. A file may perform at most 100,000 macro
expansion operations and produce at most 4,194,304 UTF-16 code units of
preprocessed text. The output ceiling counts text units, not memory usage. These
limits are deterministic rather than elapsed-time based. Exceeding either
limit, invalid include operands, and cancellation reject the compilation
instead of returning partial text. Physical line starts are indexed once so
repeated `__LINE__` expansion remains linear in total input and output size.

For optional arrays of aggregate elements, hashing uses an explicit presence
branch: add the first aggregate element when present and add `-1` otherwise.
An integer/aggregate conditional expression cannot be passed to generic
`HashCode.Add<T>`. C134 checks this rule alongside nullable presence, deep copy,
optional metadata, and native fixed-array conversion.
