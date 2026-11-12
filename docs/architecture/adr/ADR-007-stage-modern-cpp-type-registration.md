# ADR-007 Stage C++ type metadata and plugin initialization

- **Date:** 2026-11-06
- **Status:** Accepted
- **Approved by:** Michaël Hompus

## Context

C++ metadata describes graphs of structs, unions, aliases, inheritance, and
member types. A single function-local context that creates metadata and its
plugin together can re-enter its own static initialization when a member
references a type still being initialized. Derived types also require their
parent metadata to exist first.

Some supported value graphs allow DynamicType inspection while their plugin
operations remain unsupported or unqualified. Eager plugin creation in every
metadata accessor prevents that inspection and unnecessarily couples two
different operations. The runtime flow therefore needs an explicit dependency
order and, for selected shapes, separate metadata and plugin entry points.

## Decision

**Initialize dependent C++ type graphs in stages, creating type shells before member metadata and eligible plugins,**
**with a separate lazy plugin stage where metadata access must stand alone.**

The planner orders complete-type dependencies across canonical structs and
unions. Initializers resolve internal references through existing shells
rather than re-entering public accessors. Typedefs retain their metadata
identity while sharing the canonical target's value and plugin ownership.
Selected paths guard metadata and plugin initialization separately; simpler
compatible contexts keep their existing initialization path.

## Consequences

- Supported self-references can resolve to an existing shell without recursive
  static initialization. Parent and complete by-value dependencies precede
  their users.
- Metadata-only accessors can expose an inspectable graph without invoking
  an unsupported plugin operation. This separation is selected by the plan;
  it is not a promise that every DynamicType accessor avoids plugin creation.
- Initialization requires dependency planning, once-only guards, and careful
  separation of metadata construction from plugin side effects.
- Aliases do not duplicate value classes or plugin contexts. Their declared
  identities remain in the metadata graph.
- Staging does not make every recursive layout valid. Unsupported by-value
  cycles and unresolved cyclic graphs are rejected; RTI registration limits
  remain independent of successful metadata construction.
- Tests must cover dependency order and initialization entry points as well
  as emitted declarations. Runtime qualification remains operation-specific.

## Considered options

### One lazy context for metadata and plugins

- **Pros:** Simple for independent, non-recursive types.
- **Cons:** Re-entrant lookup can revisit an incomplete context, and metadata
  inspection inherits plugin failures.

### Reject graphs that require staged initialization

- **Pros:** Avoids dependency planning and multiple initialization stages.
- **Cons:** Excludes representable inheritance and self-reference shapes and
  prevents metadata inspection where only plugin operations are unavailable.

### Dependency-ordered stages with selective lazy plugin creation

- **Pros:** Makes shells available before references and separates metadata
  lookup from plugin side effects where needed.
- **Cons:** Requires explicit graph ownership, ordering, and initialization
  guards; it does not remove native runtime restrictions.

## More information

- [Native fixture guides](../../../tests/Wireloom.Dds.Generator.WireCompatibility/Native/)
- [Modern C++ qualification risks](../arc42/11-risks-and-technical-debt.md#116-modern-c-qualification-risks)

## References

- Related building block: [Shared IDL front end and C++ generator](../arc42/05-building-block-view.md#56-shared-idl-front-end-and-c-generator)
- Related runtime flow: [Generate and consume Modern C++ contracts](../arc42/06-runtime-view.md#65-generate-and-consume-modern-c-contracts)
- Related code: [Type initialization planner](../../../src/Wireloom.Dds.Cpp.Compiler/Emission/Planning/TypeInitializationPlanner.cs)
- Related code: [Staged struct and union initialization](../../../src/Wireloom.Dds.Cpp.Compiler/Emission/Emitters/StructPluginEmitter.cs)
