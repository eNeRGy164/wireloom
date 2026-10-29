# Generated value semantics

This guide describes the managed behavior exercised by Wireloom's runtime
contract tests. It helps when choosing how to initialize, copy, mutate, and
store generated values. It does not establish RTI wire compatibility for every
IDL shape; use the [wire compatibility evidence](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md)
for tested cross-language exchanges.

## Initialize values

Required sequences start empty, required arrays are allocated, and required
aggregate members are initialized for generated default constructors. Enum
arrays use the enum's declared `@default_literal` when present; other new arrays
use their element type's default value. Optional members start absent and are
represented by nullable C# members where the shape supports optionality:

```idl
module Telemetry {
  struct Nested {
    long value;
  };

  struct Sample {
    sequence<long> values;
    long fixed_values[2];
    Nested nested;
    @optional long maybe;
  };
};
```

For example, a sequence can be populated through its mutable container and an
optional member can be set to `null` to represent absence:

```csharp
var sample = new Telemetry.Sample();
sample.values.Add(7);
sample.fixed_values[0] = 1;
sample.maybe = null;
```

The `@optional` annotation requests optionality; check the generated
declaration for the exact nullable member type. The
[optional collection cases](../corpus/FEATURE-COVERAGE.md#current-grouped-idl-cases) and
[`ScalarSpecs`](../../tests/Wireloom.Dds.Generator.RuntimeContracts/Managed/ScalarSpecs.cs)
cover the tested defaults and optional shapes.

## Understand copying and ownership

The generated copy constructor copies mutable nested aggregates, sequences,
arrays, and union values for the tested shapes. Mutating the copy does not
change those copied elements in the source. See
[`SequenceCopySpecs`](../../tests/Wireloom.Dds.Generator.RuntimeContracts/Aliases/SequenceCopySpecs.cs)
and the [runtime contract matrix](../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md#coverage-matrix)
for the covered cases.

Value constructors for alias wrappers have different ownership behavior: they
retain the supplied aggregate, sequence, or array reference. Mutating the
original object after constructing the wrapper is observable through the
wrapper's `Value`. Use a generated copy constructor when the tested copy
semantics are needed; do not assume every constructor makes an independent
copy. [`ReferenceOwnershipSpecs`](../../tests/Wireloom.Dds.Generator.RuntimeContracts/Aliases/ReferenceOwnershipSpecs.cs)
records the reference-retention cases.

A copy constructor called with `null` is also not a substitute for the default
constructor. IDL defaults expressed as member initializers remain in effect,
while collection and aggregate allocations performed only by the parameterless
constructor do not run on that path. Use the ordinary default constructor when
those allocations are required. The behavior is covered by
[`ScalarSpecs`](../../tests/Wireloom.Dds.Generator.RuntimeContracts/Managed/ScalarSpecs.cs)
and the [runtime contract notes](../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md#coverage-matrix).

## Mutate collections and optional members

Required sequence properties generally expose a mutable sequence object, so
modify the container with `Add` and `Remove`. Optional sequences can be set to
`null` for absence; an empty sequence and an absent sequence are distinct
states. Optional arrays can likewise be absent. Generated C# mutation does not
necessarily enforce IDL bounds at assignment time; consult the package guide's
[effective limits](../../src/Wireloom.Dds.Generator/README.md#effective-limits-for-unbounded-values)
and runtime evidence before relying on mutation-time validation.

## Use unions safely

Assigning a union branch selects that branch. Reading a property for an
inactive branch throws `InvalidOperationException`; read the active value
through the union's generated `Get()` API or select a branch before reading
its property. An explicit `Set` with an invalid discriminator throws
`ArgumentException`. See [`UnionSpecs`](../../tests/Wireloom.Dds.Generator.RuntimeContracts/Unions/UnionSpecs.cs).

## Avoid mutable hash keys

Generated equality and hash codes reflect the current member values. If a
generated object is used as a key in a dictionary or set, mutating it after
insertion can change its hash code and make lookup behavior surprising. Prefer
immutable key data or remove and reinsert the object after changing it.

## Profile-specific defaults

The generated API profile follows the resolved RTI package version. RTI 7.3.1
through versions below 7.7.0 use the 7.3.1 API profile; 7.7.0 and later use
the 7.7.0 profile. DynamicType octet names, annotation metadata, and union
constructor initialization can therefore differ between generated outputs.
Regenerate and review contracts when changing across the 7.7.0 boundary.
For union constructors, 7.7.0 uses the discriminator type's XTypes default.
The 7.3.1 profile uses an unused primitive discriminator when a default branch
exists; enum unions with a default branch use the first declared enum value,
independently of explicit case labels or `@default_literal`. This also applies
to default-only unions and aliased enum discriminators. Unions without a default
branch use the first declared case label. Default-branch setters continue to
select an unused discriminator.

For enum arrays, a declared `@default_literal` initializes every element in
Wireloom's managed and native arrays. This includes typedef arrays, enum alias
chains, and multidimensional member arrays. C148 and
[`EnumArrayDefaultSpecs`](../../tests/Wireloom.Dds.Generator.RuntimeContracts/Managed/EnumArrayDefaultSpecs.cs)
cover these defaults; the native path is checked by
[`EnumArrayDefaultNativeSpecs`](../../tests/Wireloom.Dds.Generator.RuntimeContracts/Native/EnumArrayDefaultNativeSpecs.cs).

## Maintainer reference: generated storage contracts

These are current emitter contracts for the covered shapes. Review their
[architectural ownership rules](../architecture/arc42/08-crosscutting-concepts.md#84-generated-values-and-resource-ownership)
when changing managed methods or native support. The
[runtime contract matrix](../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md#coverage-matrix)
owns executable coverage and gaps.

### Equality and hashing

- Array hash codes use the first element, not the array length. This preserves
  RTI's intentionally weak hash behavior. Empty arrays are not a supported
  input for this generated hash path.
- Primitive arrays use direct `SequenceEqual`. Arrays containing aggregate
  elements use the casted `Cast<T>().SequenceEqual(...)` path required by the
  runtime collection API.

For optional arrays of aggregate elements, hashing uses an explicit presence
branch: add the first aggregate element when present and add `-1` otherwise.
An integer/aggregate conditional expression cannot be passed to generic
`HashCode.Add<T>`. C134 checks this rule alongside nullable presence, deep copy,
optional metadata, and native fixed-array conversion.

### Native cleanup

`optionalsOnly` cleanup is context-sensitive. Types with direct optional
members destroy those members and return before required cleanup. Derived
types return after base cleanup because the base may contain optional
members. Direct strings, nested aggregates, aggregate-element collections,
and sequence arrays retain the RTI outer guard. Primitive-only fields and
primitive-only collections delegate cleanup to their member wrapper and do
not need an outer guard.

Native conversion also distinguishes allocation-enabled calls from calls that
reuse existing storage. Review conversion, initialization, and destruction
together when changing a member shape; the
[native emitter](../../src/Wireloom.Dds.Compiler/Compiler/Emission/Emitters/NativeTypeEmitter.cs)
and linked runtime contracts define the supported paths.

### Diagnostic formatting

Alias diagnostic formatting intentionally improves on RTI C#'s unsupported
scalar and collection root formatting. Struct and union formatting continues
through RTI; aliases needing a fallback use a hidden typed instance method in
their existing support class rather than embedding loops in the data class.
Invariant numbers, readable Unicode, quoted collection strings, and nested
array dimensions are executable runtime contracts. This is diagnostic output,
with separate evidence from serialization and DDS interoperability.

## Evidence boundary

These statements summarize the shapes covered by the runtime contract suite.
They describe Wireloom's generated managed values and adapters; they do not
imply that RTI C# has identical generated source, that every RTI API has the
same ownership semantics, or that a given shape has been proven interoperable
on the DDS wire. The runtime test README links each behavior to its test and
lists gaps in coverage.

[Documentation](../README.md) · [Handwritten partial extensions](developer-workflow.md#extend-a-generated-type)
