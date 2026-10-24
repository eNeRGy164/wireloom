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
not applicable to 7.3.1. The shared positive inventory contains 91 cases, of
which 90 apply to 7.3.1 and all 91 apply to 7.7.0. Wire evidence is tied to
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
  is separate. Direct sequence-array union branches use this same configured
  projection.
- A default union branch that is a fixed array of aggregate values initializes
  each element during construction. Copy construction creates an independent
  array and copies each aggregate element, so the copy does not share those
  values with the source union.
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

## 8.8 Annotation, constant, and default semantics

Supported language constructs follow OMG IDL and DDS-XTypes semantics.
Retained RTI output records observed compatibility for specific inputs and
tool versions; it does not limit valid spellings or override the specifications.
RTI extensions and remaining Wireloom implementation limits are identified
separately from those language rules.

The front end captures ordered annotation applications with balanced raw text
and source offsets before known-name argument validation. Unknown applications
remain opaque and are ignored with diagnostics. The built-in catalog defines
parameter kinds, defaults, legal targets, positional rules, repeat/conflict
metadata, and supported versus recognized-unsupported names. Context-specific
readers apply those definitions while retaining target-specific semantic and
compatibility behavior. It
distinguishes an absent boolean annotation from an explicit `false`; shorthand
and full-form extensibility annotations resolve to the same model. A bare
`@autoid` selects `HASH`. Module auto-ID applies within that module block,
reopened blocks are resolved independently, and a declaration-local setting
does not leak to sibling declarations or included top-level declarations.
Profile-specific annotation availability is checked at the compilation and
emission boundary.

Known applications reject repeated named argument keys and invalid argument
shapes. Type prefixes, members, enum literals, and union branches consume
ordered applications; unknown arguments stay opaque. The typed expression
dispatcher reuses integral and floating evaluators with OMG numeric expression
semantics, while boolean member flags bind constants after the complete symbol
graph is available. Boolean, character, string, and enum values remain typed
literals or names resolved by their dedicated value rules; `long double` and
fixed-point remain unsupported. The catalog validates repeat/conflict rules
across captured target lists, preserving context-specific diagnostics.
Local struct/union `@autoid` and typedef `@default` applications are retained
on their semantic targets, and typedef default expressions are read during
binding after target resolution. Module scope records retain module-level
annotations and resolve the member-ID policy inherited by nested declarations,
because member IDs are assigned during declaration parsing.
Unknown applications, including those interleaved with member annotations,
are consumed as complete scoped applications, including balanced and quoted
arguments, before the following declaration is checked.

Balanced application records are transient parser inputs; the semantic model
stores effective values and unresolved expressions needed for binding rather
than duplicating source text. For an annotated comma-separated member
declaration, the parser consumes the list once and applies effective metadata
to the final declarator, matching the retained RTI case. The OMG question
about annotation application to multiple declarators, IDL43-37, remains open;
Wireloom's current behavior is a compatibility policy, not a claimed normative resolution
([OMG open IDL issues](https://issues.omg.org/issues/spec/IDL?view=OPEN)).

An enum accepts FINAL or APPENDABLE extensibility in either shorthand or full
form. MUTABLE is invalid for enumerated types. Applying extensibility more
than once is an error, including equivalent shorthand and full-form uses.
These rules follow [DDS-XTypes 1.3 sections 7.2.2.4.1 and 7.3.1.2.1.8](https://www.omg.org/spec/DDS-XTypes/1.3/PDF).

Quoted literal contents do not terminate declaration bodies or annotation
arguments. Explicit member-ID expressions resolve after constant binding and
before duplicate-ID validation, and take precedence over automatic hash IDs.
Sequential IDs continue after the preceding explicit or sequential ID,
including inherited members. Hash IDs do not advance that counter. The binder
records effective IDs separately from declared annotations; validation checks
the complete inherited and local member set, and emission renders those IDs.

Constant expressions, chained enum-valued constants, union labels, string bounds, and
defaults use the shared binding and evaluation path. Enum values retain their
declaring enum identity for assignment and label validation. Bound scalar
values distinguish integers, booleans, characters, floating-point values,
strings, and enumerators. Only integer values enter integer expressions such
as bounds and member IDs; boolean and character constants are resolved by
their own default and discriminator rules. Integral evaluation retains literal
values until their expression type is determined, then promotes operands
using their individual rank and signedness and checks each intermediate
result. The declared target type is validated after expression evaluation;
`@id` literals share this integer-expression path, including octal and
hexadecimal forms. `@unit` and `@hashid` string arguments use the shared
literal decoder, which combines adjacent tokens and supports wide literals
before storing the resulting value. Floating-point range annotations fail
during binding with a source-located IDL diagnostic. Union labels are checked
for duplicates after evaluation before emission.
Direct fixed-array union branches remain arrays in the semantic and emission
models, and use the same initialization, copying, conversion, and destruction
paths as other arrays.

Defaults resolve after the declared type and alias chain are known. A default
on the nearest annotated typedef in the chain applies unless the member has an
explicit default. The resolved typed scalar value feeds
managed initialization, native initialization, and DynamicType annotations.
The default is therefore a type-aware semantic value rather than a literal
string interpreted independently by each emitter. Profile runtime support for
DynamicType defaults is recorded with the corresponding corpus and runtime
evidence. The 7.3.1 API has no member annotation support for missing-field
defaults. On 7.7.0, runtime tests restore integral and narrow-string defaults
after clearing DynamicData members. Managed/native initialization preserves
fractional float/double and non-ASCII wide-string defaults on both LTS
profiles. Runtime metadata tests confirm that RTI 7.7.0 exposes the fractional
and wide-string annotation values exactly. Its `ClearMember` path does not
preserve fractional metadata defaults (1.5 becomes 1.0, 2.5 becomes 2.0, and
1.25 becomes 1.0) and returns `??` for a cleared non-ASCII wide-string
default; these are recorded as DynamicData restoration limitations, not
generator initialization failures.
Negative-zero sign bits survive cleared-member restoration on 7.7.0. For C171,
a one-member appendable writer sends `inherited=17101`; the full reader receives
`overridden=9`, `text="alias"`, `ratio=1.5`, and `single=1.25` from the omitted
trailing members' declared defaults. This behavior uses DDS deserialization,
separately from in-memory `ClearMember` behavior. RTI 7.3.1 lacks the
member-annotation API used for metadata restoration and sparse-field defaults.

Range validation compares integral, boolean, and character defaults with the
declared limits, including defaults inherited from typedefs. Boolean and
character values use their ordering representation only for this comparison;
they remain ineligible for integer expressions such as bounds and member IDs.

Character defaults require the matching `char` or `wchar` literal or constant
type, including through aliases; an integer expression is not a character
default. String literals also require matching width: every token assigned to
a `wstring` requires the `L` prefix, and a narrow string cannot use that prefix.
Adjacent string literals are decoded independently and then joined,
so hexadecimal and octal escapes cannot consume characters from the next
token. Bounds apply to the final value. These rules follow
[IDL 4.2 sections 7.2.6.2 and 7.2.6.3](https://www.omg.org/spec/IDL/4.2/PDF).
Focused probes on both RTI LTS generators reject the retained adjacent-string
typedef-default input rather than applying the standard concatenation rule;
Wireloom's regression and runtime tests establish its behavior independently.

The existing `wchar` union-label syntax also accepts unprefixed character
literals, as recorded by C113/C131. This is a legacy Wireloom extension;
constant and default assignments still require matching narrow/wide types.
