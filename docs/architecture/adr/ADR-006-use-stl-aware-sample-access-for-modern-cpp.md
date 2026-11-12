# ADR-006 Use profile-specific native adapters for C++ sample access

- **Date:** 2026-11-06
- **Status:** Accepted
- **Approved by:** Michaël Hompus

## Context

Generated C++ value types must be usable by RTI's registration, lifecycle, and
serialization machinery. Their language mapping follows the existing product
direction; this decision concerns the runtime adapter that connects those
values to the SDK.

RTI's `GenericTypePluginFactory` provides a `CTypePlugin` for C-layout
samples. Generated values can contain STL-backed strings and sequences,
whose representation requires compatible sample-access and lifecycle support.
The C-layout plugin therefore cannot serve as a universal adapter for these
values, even when their DynamicType metadata is available.

RTI's native sample-access and interpreter interfaces can describe those
layouts, but introduce version-sensitive implementation dependencies and
lifecycle responsibilities. Changing applications to DynamicData would avoid
this static-value adapter problem by changing their representation instead.

## Decision

**Generate profile-specific native adapters for value layouts that the generic C-layout factory cannot safely serve.**

Select the adapter from the planned value graph and RTI profile. Retain the
factory path where compatible; use native sample-access and `PRESTypePlugin`
integration where the layout requires it. The implementation has a shared
STL-aware bridge for the 7.3.1 and 7.7.0 profiles, a dedicated legacy 6.1.2
path, and a dedicated FlatData path. These are implementation paths, not a
claim that their supported shapes or qualification are interchangeable.

## Consequences

- Applications retain generated static values while adapters handle native
  layout, allocation, cleanup, registration, and serialization requirements.
- Adapter code depends on RTI profile details, including private TypeCode and
  interpreter interfaces. Applicable ABI guards and validation must follow
  the exact SDK rather than assuming stability across profile ranges.
- Shape selection and native sample access must agree. Unsupported operations
  fail closed rather than passing an incompatible object layout to RTI.
- SDK changes can require adapter changes without changing the public value
  API. Each affected lifecycle and serialization path needs evidence.
- Internal-interface support and licensing review remains open. Following an
  observed RTI access model does not establish permission or a stable public
  extension contract. Adapter implementations must not copy retained
  RTI-generated implementation source.
- The adapter choice does not extend qualification. Supported shapes,
  platform limits, and generation, consumer, CDR, and wire results remain in
  the maintained guides and retained reports.

## Considered options

### Generic factory and C-layout plugin for all values

- **Pros:** Compact integration using the existing factory path.
- **Cons:** Does not provide the required sample-access and lifecycle behavior
  for every generated C++ layout.

### Profile-specific native adapters

- **Pros:** Connects the existing static values to layout-aware RTI machinery
  and retains compatible factory paths.
- **Cons:** Adds generated lifecycle code and version-sensitive dependencies,
  with support and licensing questions requiring review.

### DynamicData-oriented support

- **Pros:** Uses RTI's dynamic representation instead of adapting static
  C++ object layouts.
- **Cons:** Replaces the established typed value surface with dynamic member
  access and changes the application programming model.

## More information

- [Generator scope and qualification](../../../src/Wireloom.Dds.Cpp.Generator/README.md)
- [Native fixture guides](../../../tests/Wireloom.Dds.Generator.WireCompatibility/Native/)
- [Modern C++ qualification risks](../arc42/11-risks-and-technical-debt.md#116-modern-c-qualification-risks)

## References

- Related strategy: [Keep C# and C++ generation separate](../arc42/04-solution-strategy.md#48-keep-c-and-c-generation-separate)
- Related runtime flow: [Qualify generated C++ sample serialization](../arc42/06-runtime-view.md#633-qualify-generated-c-sample-serialization)
- Related code: [Sample-access selection](../../../src/Wireloom.Dds.Cpp.Compiler/Emission/Planning/RtiSampleAccess.cs)
- Related code: [Plugin emission](../../../src/Wireloom.Dds.Cpp.Compiler/Emission/Emitters/PluginEmitter.cs)
