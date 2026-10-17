# Architecture memory

This is a compact, derived retrieval aid for implementation agents. The
authoritative architecture lives in the [arc42 chapters](arc42/index.md),
contracts, and support policy; edit those sources first, then refresh this file
and [machine-readable memory](../../.agents/architecture-memory.yaml).

Refreshed from the current arc42 sources on 2026-10-17. Treat the source map
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
  need C# 12+ and an RTI runtime reference of at least 7.3.1. The resolved
  assembly selects the 7.3.1 profile for versions below 7.7.0 and the 7.7.0
  profile for 7.7.0 and later. Only exact 7.3.1 and 7.7.0 are qualification
  targets; profile mapping does not qualify intermediate or later packages.
  See
  [constraints](arc42/02-architecture-constraints.md) and [support policy](../guides/support-and-compatibility.md).
- **Compiler flow:** Explicit `DdsIdl` roots and project-wide metadata enter
  through MSBuild/Roslyn. The managed pipeline resolves includes, preprocesses,
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
- **Compatibility evidence:** Keep generated API shape, runtime behavior,
  serialization, and DDS wire interoperability as separate claims. Corpus
  oracles check required types, properties/types, and enum members, rather than
  identical public APIs; runtime contracts execute generated APIs;
  licensed wire evidence checks all nine directed pairings among Wireloom C#,
  RTI C#, and RTI C++. See the [corpus guide](../corpus/README.md),
  [feature coverage](../corpus/FEATURE-COVERAGE.md), and [wire runbook](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md).
- **API policy:** RTI C# is a baseline, not an API ceiling. Deliberate API
  differences and improvements need explicit evidence; an API-shape match does
  not establish wire compatibility. See [decisions](arc42/09-architectural-decisions.md).
- **High-risk contracts:** Preserve optional presence, union discriminator
  ranges and RTI metadata representation, bounded collection semantics, and
  alias behavior across managed/native/metadata emission. Check the detailed
  source and runtime evidence before modifying these contracts.

## Repository defaults

| Setting               | Current value                             | Qualification                                          |
| :-------------------- | :---------------------------------------- | :----------------------------------------------------- |
| Consumer minimum      | `net8.0`, C# 12+                          | Support policy applies                                 |
| Repository target     | `net10.0`                                 | Pinned SDK in `global.json`                            |
| Generator target      | `netstandard2.0`                          | Analyzer/source-generator package                      |
| RTI package floor     | `7.3.1+`                                  | Reference eligibility only, not compatibility evidence |
| RTI profile range     | `7.3.1+`                                  | Nearest profile selected by assembly identity          |
| Qualified RTI versions| `7.3.1`, `7.7.0`                          | Exact LTS version evidence only                         |
| Test platform         | Microsoft.Testing.Platform, xUnit v3      | Runner filters follow `--`                             |

## Open risks

- RTI 7.3.1 generated code passes exact NuGet compilation and runtime-contract
  tests; exact-version wire qualification remains outstanding. Intermediate
  and later releases remain unqualified.
- C136 exposes `ISequence<Scalar>` where RTI C# exposes `ISequence<int>`;
  investigation is tracked in [issue #62](https://github.com/eNeRGy164/wireloom/issues/62).
- `05-array-of-sequences` does not preserve the IDL array shape in managed C#;
  Wireloom C# wire pairings fail as recorded in the feature index.
- Compatibility beyond retained cases, host/IDE combinations, and adapted RTI
  wire fixtures remains unqualified.

## Source map

- [arc42 index and chapters](arc42/index.md)
- [Corpus guide](../corpus/README.md) and [feature coverage](../corpus/FEATURE-COVERAGE.md)
- [Support and compatibility policy](../guides/support-and-compatibility.md)
- [Runtime contract guide](../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md)
- [Wire compatibility runbook](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md)
- [Corpus inventory and evidence rules](../corpus/README.md)
- [Machine-readable architecture memory](../../.agents/architecture-memory.yaml)
