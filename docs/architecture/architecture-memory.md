# Architecture memory

This is a compact, derived retrieval aid for implementation agents. The
authoritative architecture lives in the [arc42 chapters](arc42/index.md),
contracts, and support policy; edit those sources first, then refresh this file
and [machine-readable memory](../../.agents/architecture-memory.yaml).

This summary reflects the current arc42 sources. Treat the source map below as
authoritative; this summary is a retrieval aid, not a replacement.

## Agent summary

- **Language authority:** OMG IDL and DDS-XTypes define supported construct
  semantics; RTI captures establish tested compatibility rather than a language
  ceiling. FINAL and APPENDABLE enum annotations accept both spellings.
  See [strategy](arc42/04-solution-strategy.md) and
  [value rules](arc42/08-crosscutting-concepts.md#88-annotation-constant-and-default-semantics).

- **Value pipeline ownership:** Parsing records member annotation syntax and original
  source offsets. `IdlValueBinder` sequences constants, bounds-dependent values,
  typedef defaults, and member values; `IdlValueResolver` owns typed rules.
  Constants and defaults share `IdlScalarValue` variants for integer, boolean,
  character, floating-point, string, and enum values, retaining width and enum
  identity. Numeric expression dispatch applies OMG semantics for supported
  integral, `float`, and `double` contexts by reusing the existing integral and
  floating evaluators. `IdlConstantExpressionEvaluator` retains each integer
  operand's rank and signedness, promotes pairwise, and checks intermediate
  results; `@id` uses this same path. Boolean, character, string, and enum values use dedicated
  literal/name binders rather than a general-purpose expression tree. Structs, unions,
  and enums share typed prefix parsing with duplicate extensibility checks.
  String tokens are decoded individually before concatenation and final bound
  checks. `ScalarValueEmissionPlan` shares managed/native defaults and annotation
  values across members and typedefs. Union branches share one copy operation.
  See [building blocks](arc42/05-building-block-view.md) and
  [binding order](arc42/06-runtime-view.md#61-consumer-build-and-generation).

- **Annotation parsing status:** `IdlSyntaxReader` captures ordered balanced applications and source offsets;
  `IdlAnnotationCatalog` defines known parameter kinds/defaults, legal targets,
  positional rules, repeat/conflict metadata, and support status. It validates
  argument shapes while unknown applications remain opaque. Target readers
  apply catalog definitions and preserve context-specific behavior. Public
  compiler tests assert generated output and diagnostics for mixed known and
  unknown applications across declaration, member, enum-literal, and union
  targets. Catalog repeat/conflict validation covers declaration prefixes,
  members, enum literals, and union branches. Local struct/union `@autoid` and
  typedef `@default` applications are retained on semantic targets; typedef
  expressions are evaluated during binding after target type resolution.
  Module scope records retain module-level annotations and resolve the
  member-ID policy inherited by nested declarations because member IDs are
  assigned during declaration parsing.
  `IdlTypeAnnotationPrefix` buffers unknown warnings until a matching type parser accepts the declaration.
  Member boolean flags bind constant references after the symbol graph is available. Numeric expression dispatch
  reuses integral and floating evaluators with OMG constant-expression semantics
  in supported integral, `float`, and `double` contexts; floating expressions reject integer literal operands and integral
  constant references. OMG integer literals and numeric operators are covered,
  including octal literals and shift limits. Referenced integral constants
  retain declared rank during expression promotion, including aliases; wide
  literals do not truncate before operator evaluation. Boolean,
  character, string, and enum values bind through their typed literal/name
  rules. `long double` and fixed-point remain unsupported, with an extensible
  internal scalar model.
  Supported defaults use typed range comparisons; native conversion initializes
  aggregate elements when activating union array branches.

- **Motivation and goals:** The original RTI generation setup added about 2 GB
  to C# Docker build images, required tools on developer machines, and depended
  on environment-variable/MSBuild wiring that was difficult to reproduce.
  Generated files also produced substantial analyzer noise as regular source
  files. Preserve the resulting goals of small generation tooling, minimal
  local setup, declarative repeatable builds, and generated C# that fits strict
  tooling. These are product evaluation criteria as well as implementation
  goals. See [introduction and goals](arc42/01-introduction-and-goals.md).
- **Product boundary:** Wireloom is an independent compile-time Roslyn source
  generator integrated through MSBuild. It generates C# and build integration;
  it does not provide or replace the RTI runtime, DDS operation, or RTI support.
  Support is best effort, with no official RTI relationship. See [context and
  scope](arc42/03-context-and-scope.md).
- **Qualified environment:** The generator targets `netstandard2.0`; consumers
  need C# 12+ and an RTI runtime reference of at least 7.3.1. It references
  Roslyn 4.8.0 so it loads in the first .NET 8 C# 12 SDK host. The resolved
  assembly selects the 7.3.1 profile for versions below 7.7.0 and the 7.7.0
  profile for 7.7.0 and later. Only exact 7.3.1 and 7.7.0 are qualification
  targets; profile mapping does not qualify intermediate or later packages.
  See
  [constraints](arc42/02-architecture-constraints.md) and [support policy](../guides/support-and-compatibility.md).
- **Compiler flow:** Explicit `DdsIdl` roots and project-wide metadata enter
  through MSBuild/Roslyn. Finite unbounded-value limits default to 100 sequence
  elements and 255 string bytes/code units; project properties set defaults and
  file metadata can override them. Explicit IDL bounds win. Sequence-array mapping is independently file-owned:
  the project default `DdsIdlSequenceArrayMapping=RtiCSharp` can be overridden
  with `SequenceArrayMapping=Idl` on a declaring file, including shared includes.
  The emission plan applies this mapping to direct sequence-array union branches
  as well as class members.
  The managed pipeline resolves includes, preprocesses,
  parses, binds, validates, plans emission, and writes generated documents.
  Input that prevents safe managed output fails with source-located diagnostics.
  Deliberately ignored constructs and potentially lossy mappings report
  diagnostics while generation continues; unsupported annotation uses report
  `DDSG0102` before failing with `DDSG0001`. Target-profile incompatibilities
  report `DDSG0004`; in particular, 7.3.1 rejects `@must_understand` and emits
  no source, while 7.7.0 preserves it in DynamicType metadata. See [building
  blocks](arc42/05-building-block-view.md)
  and [runtime flow](arc42/06-runtime-view.md).
- **Shared compiler API:** `Wireloom.Dds.Compiler` is the host-neutral,
  non-packable `netstandard2.0` project shared by repository C# callers. Its
  in-memory API requires an explicit RTI profile and returns one
  `IdlCompilationResult` containing documents and diagnostics; errors may
  return no documents with diagnostics. Each call requires a synchronous
  MD5 delegate; the generator supplies `MD5.HashData`, and the compiler keeps
  UTF-8 encoding, digest validation, byte ordering, and masking. It snapshots input lists, preserves
  current batch and path behavior, observes cancellation, and does not access
  the filesystem. The Roslyn adapter discovers the resolved RTI version,
  selects the profile, maps diagnostics, and adds source documents. The single
  published generator package bundles both version-matched assemblies; the
  compiler is not published separately or added as an application runtime
  reference. See [ADR-001](adr/ADR-001-share-a-host-neutral-compiler-project.md),
  [building blocks](arc42/05-building-block-view.md), and
  [cross-cutting concepts](arc42/08-crosscutting-concepts.md#89-shared-compiler-api).
- **IDL annotation and value binding:** The shared reader captures ordered balanced annotation applications and source spans;
  context parsers retain unresolved values until symbol binding.
  The catalog validates duplicate named keys for known applications and leaves unknown arguments opaque.
  Type-prefix parsing accepts unknown applications between supported annotations and reports each once after target selection.
  Member boolean annotation constants bind after discrete constants, including forward and scoped references.
  Shared syntax reading preserves quoted
  delimiters. The binder resolves enum identities, chained constants, member
  IDs, labels, string bounds, and type-aware defaults before emission. Explicit
  member IDs override automatic hash IDs and resolve before duplicate checks.
  Effective IDs continue sequentially after explicit IDs across inheritance;
  hash IDs leave the sequential counter unchanged. Validation checks inherited
  and local IDs together, and emission renders the bound IDs.
  A local default overrides an annotated typedef default; the effective value
  feeds managed initialization, native initialization, and DynamicType metadata.
  C163–C172 record exact-profile generation outcomes for these forms. Their RTI
  C# and C++ reference sources are retained with original notices; Wireloom C# output compiles
  against both exact RTI API profiles. Sixteen typed fixtures cover directed
  exchanges among all three peers on both profiles. C164 and
  C172 use topic wrappers; RTI 7.3.1 reference generation stages byte-identical
  include files. C171 defaults are `inherited=7`, `overridden=9`,
  `text="alias"`, `ratio=1.5`, and `single=1.25`. When a compatible older
  appendable writer omits the trailing members, the reader applies those
  declared defaults; RTI 7.3.1 lacks the member-annotation API for this path.
  Runtime
  contracts check fractional and non-ASCII wide-string initialization on both
  LTS profiles and exact annotation metadata on 7.7.0. RTI 7.7.0 DynamicData
  clear restoration truncates fractional defaults and returns `??` for a
  non-ASCII wide-string default; cleared integral, narrow-string, and
  signed-zero defaults are covered separately.
  See
  [cross-cutting semantics](arc42/08-crosscutting-concepts.md#88-annotation-constant-and-default-semantics)
  and [corpus coverage](../corpus/FEATURE-COVERAGE.md).
- **Strict validation:** Defaults to `false`; any generating root enables it
  for the batch. Pure rules evaluate source-mapped identifier facts, and a
  separate policy chooses warning or failure. Interface bodies are parsed
  without emitting types: exact keywords fail in all bodies, while ordinary
  interfaces retain RTI acceptance for specification-only names and DDS
  services apply the naming policy. Parameters have independent operation
  scopes; constructed-type nesting is bounded at 64. See [building blocks](arc42/05-building-block-view.md)
  and the [configuration reference](../guides/configuration.md#enable-strict-validation).
- **RTI profile ownership:** `RuntimeReferenceGuard` reads the resolved RTI
  assembly version and reports reference errors. `IdlCompilation` selects and
  carries an immutable profile into emission planning. Semantic types and
  `CompilationContext` remain RTI-independent; emitters render profile-aware
  plans without version comparisons. Profile policies include DynamicType
  names, annotation APIs, plugin masks, and union constructor defaults. The
  7.3.1 profile uses legacy union discriminator selection; default setters
  remain independent of constructor defaults. Profile-specific annotation
  incompatibilities are diagnosed before emission. See the [building block
  view](arc42/05-building-block-view.md).
- **Continuous LTS verification:** Quality CI runs matching-oracle corpus
  jobs for RTI 7.3.1 and 7.7.0, plus eight Windows/Ubuntu × RTI LTS ×
  `net8.0`/`net10.0` runtime and fresh-package jobs. The repository SDK builds
  the package; package consumers use C# 12. Dedicated SDK 8.0.100 containers
  restore, build, and run the fresh package consumer with no .NET 10 SDK or
  runtime for each RTI LTS package. This qualifies the minimum .NET 8 C# 12
  Roslyn host and does not replace licensed wire evidence. See
  [deployment](arc42/07-deployment-view.md) and
  [verification scope](../guides/support-and-compatibility.md#continuous-lts-verification).
- **Checked-in publisher/subscriber sample:** The repository example consumes
  released Wireloom.Dds.Generator 0.4.0 from NuGet.org. Quality CI compiles it
  on Ubuntu 26.04 and Windows 2025 without loading native DDS libraries or
  requiring an RTI license; local DDS execution requires RTI native assets and
  a valid license. See the [sample guide](../../samples/HelloWorld/README.md),
  [deployment](arc42/07-deployment-view.md), and
  [quality scenarios](arc42/10-quality-requirements.md).
- **Local container tooling:** Docker Desktop runs pinned RTI oracle captures
  and Community Qodana; keep licenses outside the repository and mount them
  read-only for licensed wire runs. See the [LTS runbook](../corpus/LTS-PROFILES.md)
  and [quality procedure](../CONTRIBUTING.md#quality-evidence).
- **LTS evidence applicability:** Corpus oracle reads use the selected LTS
  source directories and outcomes. Wire planning reports unavailable RTI
  features separately as `notApplicableCases`; they add no exchanges, failures,
  or Wireloom implementation gaps. C040 isolates hash IDs on both LTS versions;
  C142 isolates `@must_understand`, which is N/A on 7.3.1. Wire evidence must
  match the exact case inputs and RTI profile. New capture/export evidence retains
  source identity, input/output hashes, tools, arguments, and outcomes; legacy
  missing provenance stays unknown, and dirty-tree runs identify a base commit
  rather than a clean tested revision. The [LTS runbook](../corpus/LTS-PROFILES.md)
  owns reproduction. See [evidence vocabulary](arc42/08-crosscutting-concepts.md#81-evidence-vocabulary).
- **Compatibility evidence:** Keep generated API shape, runtime behavior,
  serialization, and DDS wire interoperability as separate claims. Corpus
  oracles check required types, properties/types, and enum members, rather than
  identical public APIs; runtime contracts execute generated APIs;
  licensed wire evidence checks all nine directed pairings among Wireloom C#,
  RTI C#, and RTI C++ for ordinary cases. C149 records its selected mode, peer
  scope, exclusion reasons, and source hashes as a dedicated corpus case. Its
  `Idl` mapping passes 16 / 16 focused Wireloom/C++ exchanges per exact LTS profile;
  RTI C# is excluded because it describes the flattened type. The manual workflow runs both qualified LTS profiles,
  with eight isolated shards per version and a separate report, summary, and
  artifact for each. It runs in the trimmed .NET/RTI native image using
  checked-in C++ peers without Java or `rtiddsgen`. Docker layers are cached
  per RTI version; stable ccache keys include OS, RTI and compiler versions,
  shard, and source-input hash, with compatible restore prefixes. GitHub's
  branch cache scope still applies. See the
  [deployment view](arc42/07-deployment-view.md) and
  [wire runbook](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md).
  C147 passes all 18 directed exchanges per profile after
  guarded RTI reference corrections; the retained RTI C# source records its
  unmodified compilation failure. See the [corpus guide](../corpus/README.md),
  [feature coverage](../corpus/FEATURE-COVERAGE.md), and [wire runbook](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md).
- **API policy:** RTI C# is a baseline, not an API ceiling. Deliberate API
  differences and improvements need explicit evidence; an API-shape match does
  not establish wire compatibility. See [decisions](arc42/09-architectural-decisions.md).
- **High-risk contracts:** Preserve optional presence, union discriminator
  ranges and RTI metadata representation, bounded collection semantics, and
  alias behavior across managed/native/metadata emission. Enum arrays with
  `@default_literal` initialize from that literal in both managed and native
  output. Direct fixed-array union branches initialize each aggregate array
  element, convert each element, and deep-copy aggregate values when copied;
  direct sequence-array union branches follow the configured mapping. Clearing
  DynamicData members restores integral and string defaults on 7.7.0. Floating
  default restoration and DDS
  deserialization with missing fields remain unverified. On 7.3.1 the member
  annotation API is absent.
  Check the detailed source and runtime evidence before modifying these
  contracts.

## Repository defaults

| Setting                 | Current value                                      | Qualification                                                                                                          |
| :---------------------- | :------------------------------------------------- | :--------------------------------------------------------------------------------------------------------------------- |
| Consumer minimum        | `net8.0`, C# 12+                                   | Support policy applies                                                                                                 |
| Consumer compiler host  | Roslyn 4.8.0 (`SDK 8.0.100`)                       | Minimum qualified SDK host                                                                                             |
| Repository target       | `net10.0`                                          | Pinned SDK in `global.json`                                                                                            |
| Generator target        | `netstandard2.0`                                   | Analyzer/source-generator package                                                                                      |
| Compiler target        | `netstandard2.0`; non-packable          | Bundled beside the generator in the single published package; not an independently published package |
| RTI package floor       | `7.3.1+`                                           | Reference eligibility only, not compatibility evidence                                                                 |
| RTI profile range       | `7.3.1+`                                           | Nearest profile selected by assembly identity                                                                          |
| Qualified RTI versions  | `7.3.1`, `7.7.0`                                   | Exact LTS version evidence only                                                                                        |
| Test platform           | Microsoft.Testing.Platform, xUnit v3               | Runner filters follow `--`                                                                                             |
| Sequence-array mapping  | `RtiCSharp`; opt-in `Idl` per project/file         | File ownership; runtime coverage on both LTS profiles; C149 Idl case passes 16 focused DDS exchanges per exact profile |
| Unbounded finite limits | 100 sequence elements; 255 string bytes/code units | Project defaults with file-level overrides; explicit IDL bounds win; positive Int32 only                               |
| Strict validation       | `false`; any root enables the batch                | DDSG0108 by default; strict failure before emission                                                                    |

## Open risks

- RTI 7.3.1 generated code passes exact NuGet compilation and runtime-contract
  tests; exact-version wire qualification remains outstanding. Intermediate
  and later releases remain unqualified.
- `05-array-of-sequences` retains the flattened RTI C# shape by default and its
  recorded wire failures. Opt-in `Idl` mode plans arrays of sequence wrappers
  with anonymous sequence metadata; it has runtime coverage on both profiles.
  Recursive direct sequence arrays are rejected in this mode. The dedicated
  retained C149 corpus case passes its 16 focused Wireloom/C++ DDS
  exchanges per profile; broader qualification remains open. See chapters 5,
  8, 9, 10, and 11.
- The new `DDSG0108` warning may fail warnings-as-errors builds; exact reserved
  keywords now fail in both modes. Enum literals follow the enclosing IDL
  scope, including the documented stricter behavior than RTI between enums.
- RTI 7.3.1 C# RPC transport output for C157/C159 fails compilation; preserve
  the raw failure and track interface/RPC generation separately in
  [issue #75](https://github.com/eNeRGy164/wireloom/issues/75).
- Recursive sequence typedefs are unsupported by Wireloom and are rejected
  before emission. Standard RTI C++11 `rtiddsgen -unboundedSupport` output for
  self- and mutually recursive typedefs does not compile on RTI 7.3.1 / code
  generator 4.3.1 or RTI 7.7.0 / code generator 4.7.0, so a C#-only metadata
  workaround would not meet the interoperability requirement. The directly
  self-recursive sequence member is supported across Wireloom C# and typed RTI
  C++ on both LTS versions when participants use TypeObject v1
  (`type_object_max_serialized_length=8192`, `SERVICE_REQUEST_CHANNEL` only).
  RTI 7.7.0 default TypeObject v2 rejects the recursive SCC at registration.
  RTI 7.3.1 C++ generation requires the documented module-qualification
  correction. The bidirectional matrix fixture checks literal root/child/leaf
  values. Mutual recursive struct graphs remain unqualified. See [chapter 11](arc42/11-risks-and-technical-debt.md).
- Compatibility beyond retained cases, interactive IDE behavior, and adapted
  RTI wire fixtures remains unqualified.
- The analyzer package must keep its generator and adjacent compiler
  assemblies at one version; package and minimum-host loading are covered by
  integration tests. Direct callers depend on the current path interpretation,
  and literal-formatting parity is guarded by tests. The per-compilation MD5
  boundary is implemented; browser execution still needs a compatible
  provider and end-to-end qualification in
  [issue #108](https://github.com/eNeRGy164/wireloom/issues/108).

## Decision history

[Chapter 9](arc42/09-architectural-decisions.md) summarizes decisions in a compact timeline, oldest first.
Inherited policy dates identify their first documentation in that chapter;
ADR entries retain their recorded date and status. ADR-001 and ADR-002 are
Accepted; ADR-002 was approved October 26, 2026.

[ADR-002 (Accepted)](adr/ADR-002-inject-member-id-md5-per-compilation.md)
records the required synchronous MD5 delegate supplied per compilation. The
boundary is implemented: the generator supplies BCL hashing, while the
compiler retains UTF-8, digest validation, byte ordering, and masking. Browser
provider implementation and qualification remain outstanding.

## Source map

- [arc42 index and chapters](arc42/index.md)
- [Corpus guide](../corpus/README.md) and [feature coverage](../corpus/FEATURE-COVERAGE.md)
- [Support and compatibility policy](../guides/support-and-compatibility.md)
- [Configuration reference](../guides/configuration.md)
- [Runtime contract guide](../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md)
- [Wire compatibility runbook](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md)
- [Corpus inventory and evidence rules](../corpus/README.md)
- [ADR-001: shared compiler project](adr/ADR-001-share-a-host-neutral-compiler-project.md)
- [ADR-002 (Accepted): per-compilation MD5](adr/ADR-002-inject-member-id-md5-per-compilation.md)
- [Machine-readable architecture memory](../../.agents/architecture-memory.yaml)
