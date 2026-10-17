# 5. Building block view

Wireloom separates build integration, input discovery and preprocessing,
semantic compilation, emission planning, and generated-source writing. The
level-1 view shows those boundaries; level 2 expands the compiler. Detailed
emission contracts follow the structural views.

## 5.1 Level 1: White-box overall system

![Level 1 building blocks from consumer project and MSBuild inputs through the Roslyn generator to generated C# and the RTI runtime.](images/05/level-1-overview.png)

At level 1, the managed compiler is intentionally shown as a black box. Its
front end, preprocessing boundary, resolution, validation, emission planning,
and writer handoff are expanded in the level-2 view below.

| Building block               | Role                                                       | Main boundary                                     |
| :--------------------------- | :--------------------------------------------------------- | :------------------------------------------------ |
| Roslyn generator             | Accepts inputs, reports diagnostics, and adds documents    | `Generator`                                       |
| Runtime reference adapter    | Resolves RTI assembly version and reports reference errors | `RuntimeReferenceGuard`                           |
| Input graph and preprocessor | Resolves IDL roots/includes and applies preprocessing      | `IdlInputGraph`, `IdlPreprocessor`                |
| Front end                    | Parses, binds, and validates declarations                  | `Compiler/FrontEnd`                               |
| Compilation orchestration    | Coordinates compilation state and emission handoff         | `IdlCompilation`, `CompilationContext`            |
| Emission planning            | Selects managed, native, and support output shapes         | `Emission/Model`, `Planning`                      |
| RTI target profile           | Holds immutable API emission policies for one compile      | `RtiApiProfile`                                   |
| Emitters                     | Write generated C# documents                               | Managed, native, TypeSupport, and plugin emitters |
| MSBuild targets              | Pass IDL roots and include metadata to Roslyn              | Packed NuGet package                              |
| Corpus and integration tests | Verify acceptance, API shape, and package use              | Separate test layers                              |

## 5.2 Level 2: Compiler and preprocessing

The compiler front end and its preprocessing boundary are the most useful
level-2 zoom because they separate build-input ownership from target-independent
IDL meaning.

![Compiler and preprocessing flow from build metadata and IDL inputs through parsing, semantic validation, and emission planning.](images/05/level-2-compiler-and-preprocessing.png)

| Stage                     | Responsibility                                           | Main implementation                                                                                                |
| :------------------------ | :------------------------------------------------------- | :----------------------------------------------------------------------------------------------------------------- |
| Root intake               | Builds the graph from `DdsIdl` roots and project options | [`IdlInputGraph`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Preprocessing/IdlInputGraph.cs)            |
| Include resolution        | Resolves paths while preserving root/include identity    | [`IdlIncludeResolver`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Preprocessing/IdlIncludeResolver.cs)  |
| Preprocessing             | Applies directives and macros with source mapping        | [`IdlPreprocessor`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Preprocessing/IdlPreprocessor.cs)        |
| Parsing                   | Converts preprocessed text into declarations             | [`IdlDeclarationParser`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Parsing/IdlDeclarationParser.cs)    |
| Semantic model            | Assigns stable symbols and target-independent meaning    | [`IdlSymbolTable`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Symbols/IdlSymbolTable.cs)                |
| Compilation state         | Coordinates parse, bind, and validate phases             | [`IdlCompilation`](../../../src/Wireloom.Dds.Generator/Compiler/IdlCompilation.cs)                                 |
| Resolution and validation | Resolves references and reports semantic errors          | [`CompilationResolver`](../../../src/Wireloom.Dds.Generator/Compiler/CompilationResolver.cs)                       |
| Emission planning         | Projects validated declarations into output plans        | [`EmissionTypeProjector`](../../../src/Wireloom.Dds.Generator/Compiler/Emission/Planning/EmissionTypeProjector.cs) |

This view is structural rather than a runtime sequence: it shows ownership and
handoffs, while build-time ordering and failure scenarios remain in [chapter 6](06-runtime-view.md).

The front end follows an explicit parse -> bind -> validate sequence. Parsing
records syntax and source context, binding resolves references and deferred
bounds against the complete symbol graph, and validation applies semantic
rules before emission planning begins. Emitters return generated documents to a
central result boundary, which owns duplicate hint-name validation.

The runtime reference adapter inspects the resolved `Rti.ConnextDds` assembly
identity. `IdlCompilation` receives the selected immutable `RtiApiProfile` and
passes it into target-specific emission planning. `CompilationContext` and
front-end semantic types remain focused on symbols and IDL meaning. Emitters
render the decisions in their plans; they do not compare RTI versions. A
future LTS profile adds catalog policies and evidence without duplicating the
parse, bind, validate, or emission pipeline.

## 5.3 Generated contract rules

### Union discriminators and construction

Union type-support emission preserves the managed discriminator's IDL value
range while adapting metadata labels to RTI's signed native representation.
Unsigned 32-bit labels use an explicit unchecked `int` conversion. Narrow
character labels use signed 8-bit values in RTI metadata, matching RTI-generated
C++ compiled with a single-byte execution charset. Managed `char` properties
and branch comparisons retain the original character values. Narrow `char`
labels and default selection are limited to the native byte range 0..255,
including character literals and typedef discriminators. Wide `wchar` labels
retain the 16-bit range 0..65535.
For the 7.7.0 profile, union construction uses the XTypes default of the discriminator type: zero
for integers and characters, false for booleans, and the first enum literal,
including negative values. Selecting the default branch through its setter
still chooses an unused discriminator. Null-source copies initialize the same
discriminator and selected branch as the parameterless constructor. The shared
initialization emitter runs only on the null-copy path, avoiding replacement
allocations for non-null sources. Enum casts parenthesize negative operands.
The 7.3.1 profile retains its legacy constructor selection: zero for primitive
discriminators with a default branch; for enum discriminators, zero when it is
an explicit label and otherwise the lowest labeled case. Without a default
branch it uses the lowest labeled case. Default-branch setters remain
independent and choose an unused discriminator in either profile.
These managed contracts are covered by
[`UnionConstructionSpecs`](../../../tests/Wireloom.Dds.Generator.RuntimeContracts/Unions/UnionConstructionSpecs.cs),
[`UnionConstructionNativeSpecs`](../../../tests/Wireloom.Dds.Generator.RuntimeContracts/Native/UnionConstructionNativeSpecs.cs),
and the [corpus feature index](../../corpus/FEATURE-COVERAGE.md).

### Aliases, collections, and optional members

Emission planning uses the underlying struct or union type for aggregate
members and array or sequence elements whose IDL type is a typedef chain.
Standalone typedef declarations remain generated, while aggregate APIs,
collection elements, and native conversions use the underlying aggregate
support. This matches RTI's generated C# shape: raw arrays and sequences of
aggregate aliases expose initialized aggregate values and convert through the
aggregate's native type support.

Collection alias emission plans resolve an omitted sequence bound to the
existing effective limit of 100 elements. Managed attributes and summaries,
native allocation, and DynamicType metadata consume that same resolved bound;
the semantic declaration retains whether the IDL supplied an explicit bound.
String typedefs retain their effective 255-byte/code-unit limit and import the
attribute namespace whenever their generated value property uses `Bound`.
Direct union branches that contain string sequences use the same specialized
`NativeStringSeq` or `NativeWstringSeq` lifecycle as ordinary sequence members.
The resolved element bound flows through native initialization and conversion;
typedef-backed union branches delegate through their generated alias wrappers.
Retained corpus cases C135/C136 confirm these defaults against RTI-generated
C# with a typed runtime probe on Windows and Linux. The probe also records
RTI's independent aggregate sequence element copies and its scalar sequence
element typedef erasure; Wireloom currently retains scalar wrappers. These
are API observations, with no wire compatibility claim. Runtime behavior is
covered by the [runtime contract matrix](../../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md#coverage-matrix).
Direct and typedef-backed narrow/wide string-sequence union branches are also
covered by [`StringSequenceUnionSpecs`](../../../tests/Wireloom.Dds.Generator.RuntimeContracts/Unions/StringSequenceUnionSpecs.cs)
and retained RTI C# API oracles. Source-shape comparison does not establish DDS
wire interoperability for those shapes.

Alias formatting keeps RTI type-support formatting for struct and union roots
and aggregate elements. Scalar, enum, and string roots use their managed
values because RTI's root DynamicData formatting rejects those kinds.
Aliases retain their original one-line `Support.Instance.ToString(this)`
delegate. For unsupported root kinds, the generated support class declares
`public new string ToString(Alias sample)`, hiding RTI's nonvirtual default
formatter. Collection loops and string escaping stay in the support document,
preserving dimensions, order, Unicode, and quoted string elements. Base-typed
calls and explicit print-format overloads retain RTI behavior. The original
support factory and singleton initialization remain unchanged. Wide-string
alias annotations use `WideStringValue` and `TypeKind.WideString`, matching
RTI and allowing the support instance to initialize. These diagnostic helpers
add no runtime dependency or new serialization behavior. See the
[`AliasFormattingSpecs`](../../../tests/Wireloom.Dds.Generator.RuntimeContracts/Aliases/AliasFormattingSpecs.cs)
runtime contract.

Optional struct members whose value type is a struct or union use nullable
managed references and `NativeManagedOptional` storage. Their conversion plan
uses the aggregate's native type support, while DynamicType metadata marks the
member optional. When the declared member type is a typedef chain, its
DynamicType metadata preserves the declared alias support, matching RTI; the
managed property and native conversion still use the underlying aggregate.
Absent values remain null in the managed contract.

### Recursive fixed arrays

Optional fixed-array members break recursive value-layout cycles because their
native storage is indirect. Validation unwraps typedefs before identifying the
array and preserves required-array and inline aggregate cycle diagnostics.
Self-referencing optional arrays and recursive sequences use
`GetOrCreateInstanceImpl()` in member metadata. The recursive plugin placeholder
allows reentrant support construction before the singleton initializer assigns
`Instance`. This intentionally improves on RTI 7.7.0 recursive-array output:
applications need no standalone plugin bootstrap before normal support discovery.
Optional arrays retain fixed-array dimensions and `NativeManagedOptionalArray`
conversions. Optional aggregate-array
conversions forward `keysOnly` to the element support and preserve positional
`optionalsOnly` cleanup.
Optional aggregate-array hashing adds the first element when present and `-1`
when absent, matching the RTI C# oracle.

## 5.4 Preprocessing limits

The input graph and preprocessor enforce deterministic work boundaries: macro
expansion is capped at 100,000 expansion operations and each preprocessed file
at 4,194,304 UTF-16 code units of text, while conditional expressions are capped at 256 nested
parser levels. Source-origin spans map generated fragments back to exact input
text or to the macro invocation that generated them, including EOF.
