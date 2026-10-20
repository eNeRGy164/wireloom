# Architecture memory

This is a compact, derived retrieval aid for implementation agents. The
authoritative architecture lives in the [arc42 chapters](arc42/index.md),
contracts, and support policy; edit those sources first, then refresh this file
and [machine-readable memory](../../.agents/architecture-memory.yaml).

Refreshed from the current arc42 sources on 2026-10-20. Treat the source map
below as authoritative; this summary is a retrieval aid, not a replacement.

## Agent summary

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
  through MSBuild/Roslyn. Sequence-array mapping is independently file-owned:
  the project default `DdsIdlSequenceArrayMapping=RtiCSharp` can be overridden
  with `SequenceArrayMapping=Idl` on a declaring file, including shared includes.
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
  with four isolated shards per version and a separate report, summary, and
  artifact for each. C147 passes all 18 directed exchanges per profile after
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
  output. Check the detailed source and runtime evidence before modifying
  these contracts.

## Repository defaults

| Setting                | Current value                              | Qualification                                                                                                          |
| :--------------------- | :----------------------------------------- | :--------------------------------------------------------------------------------------------------------------------- |
| Consumer minimum       | `net8.0`, C# 12+                           | Support policy applies                                                                                                 |
| Consumer compiler host | Roslyn 4.8.0 (`SDK 8.0.100`)               | Minimum qualified SDK host                                                                                             |
| Repository target      | `net10.0`                                  | Pinned SDK in `global.json`                                                                                            |
| Generator target       | `netstandard2.0`                           | Analyzer/source-generator package                                                                                      |
| RTI package floor      | `7.3.1+`                                   | Reference eligibility only, not compatibility evidence                                                                 |
| RTI profile range      | `7.3.1+`                                   | Nearest profile selected by assembly identity                                                                          |
| Qualified RTI versions | `7.3.1`, `7.7.0`                           | Exact LTS version evidence only                                                                                        |
| Test platform          | Microsoft.Testing.Platform, xUnit v3       | Runner filters follow `--`                                                                                             |
| Sequence-array mapping | `RtiCSharp`; opt-in `Idl` per project/file | File ownership; runtime coverage on both LTS profiles; C149 Idl case passes 16 focused DDS exchanges per exact profile |

## Open risks

- RTI 7.3.1 generated code passes exact NuGet compilation and runtime-contract
  tests; exact-version wire qualification remains outstanding. Intermediate
  and later releases remain unqualified.
- C136 exposes `ISequence<Scalar>` where RTI C# exposes `ISequence<int>`;
  investigation is tracked in [issue #62](https://github.com/eNeRGy164/wireloom/issues/62).
- `05-array-of-sequences` retains the flattened RTI C# shape by default and its
  recorded wire failures. Opt-in `Idl` mode plans arrays of sequence wrappers
  with anonymous sequence metadata; it has runtime coverage on both profiles.
  Recursive direct sequence arrays are rejected in this mode. The dedicated
  retained C149 corpus case passes its 16 focused Wireloom/C++ DDS
  exchanges per profile; broader qualification remains open. See chapters 5,
  8, 9, 10, and 11.
- Compatibility beyond retained cases, host/IDE combinations, and adapted RTI
  wire fixtures remains unqualified.

## Source map

- [arc42 index and chapters](arc42/index.md)
- [Corpus guide](../corpus/README.md) and [feature coverage](../corpus/FEATURE-COVERAGE.md)
- [Support and compatibility policy](../guides/support-and-compatibility.md)
- [Configuration reference](../guides/configuration.md)
- [Runtime contract guide](../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md)
- [Wire compatibility runbook](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md)
- [Corpus inventory and evidence rules](../corpus/README.md)
- [Machine-readable architecture memory](../../.agents/architecture-memory.yaml)
