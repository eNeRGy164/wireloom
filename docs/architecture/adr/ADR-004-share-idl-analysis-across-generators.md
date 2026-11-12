# ADR-004 Share IDL analysis across generators

- **Date:** 2026-11-05
- **Status:** Accepted
- **Approved by:** Michaël Hompus

## Context

The host-neutral C# compiler already serves Roslyn and the browser Playground
under [ADR-001](ADR-001-share-a-host-neutral-compiler-project.md). Adding C++
introduces another language backend with its own mapping, native SDK
integration, and build entry point. The early C++ emitter lived inside the C#
compiler, which would make the analyzer package carry both backends.

Both backends need the same preprocessing, binding, source provenance, and
IDL/XTypes analysis. The architectural choice is where to place that shared
work so that language-specific dependencies and policies can evolve separately.

## Decision

**Extract shared IDL analysis into `Wireloom.Dds.Idl` and place C# and C++ planning and emission in separate compiler assemblies.**

The shared assembly exposes bound declarations and annotations through its
public semantic model; parser, binder, and symbol-table machinery remain
internal. Each compiler owns target validation, RTI profiles, mapping, and
emission. Hosts use shared request/result contracts with backend-specific
profiles and generated document types.

## Consequences

- Both backends reuse language analysis and can exercise the same authored
  IDL corpus without duplicating the front end.
- The C# analyzer carries the shared IDL assembly and C# compiler without
  carrying C++ planning or emission. The Playground references both compilers.
- The semantic API becomes a maintained boundary: backends need sufficient
  resolved facts without depending on parser internals.
- The analyzer host baseline constrains the shared assembly. Bundled
  assemblies must stay version-aligned and be checked in their actual hosts.
- Host input discovery remains outside analysis. The native CLI has a
  restricted internal preprocessing integration to discover filesystem-backed
  includes; the compiler backends consume the public bound model.

## Considered options

### One compiler assembly with both emitters

- **Pros:** Reuses the existing project and internal front-end APIs directly.
- **Cons:** Bundles C++ code with the C# analyzer and couples target policies
  and dependencies inside one compiler.

### Shared analysis with separate compiler assemblies

- **Pros:** Reuses IDL meaning while isolating target-specific code and hosts.
- **Cons:** Adds an assembly and requires an explicit semantic API and aligned
  artifact composition.

### Independent front ends

- **Pros:** Each backend can change its analysis independently.
- **Cons:** Duplicates preprocessing, binding, diagnostics, and maintenance
  for the same input language.

## More information

- [Shared compiler API](../../reference/compiler-api.md)
- [Original host-neutral compiler decision](ADR-001-share-a-host-neutral-compiler-project.md)

## References

- Related building block: [Shared IDL front end and C++ generator](../arc42/05-building-block-view.md#56-shared-idl-front-end-and-c-generator)
- Related runtime flow: [Compile an in-memory IDL batch](../arc42/06-runtime-view.md#64-c1-compile-an-in-memory-idl-batch)
- Related code: [Public IDL analysis](../../../src/Wireloom.Dds.Idl/Api/IdlAnalyzer.cs)
