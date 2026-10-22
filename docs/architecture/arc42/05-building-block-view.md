# 5. Building block view

Wireloom separates build integration, IDL compilation, emission planning, and
generated-source writing. The level-1 view shows the system boundaries; level
2 shows compiler ownership and handoffs.

## 5.1 Level 1: White-box overall system

![Level 1 building blocks from consumer project and MSBuild inputs through the Roslyn generator to generated C# and the RTI runtime.](images/05/level-1-overview.png)

At this level the managed compiler is a single block. Its internal stages are
expanded in the level-2 view.

| Building block               | Responsibility                                                | Main boundary                                     |
| :--------------------------- | :------------------------------------------------------------ | :------------------------------------------------ |
| Roslyn generator             | Accepts build inputs, reports diagnostics, and adds documents | `Generator`                                       |
| Runtime reference adapter    | Checks the RTI assembly and selects its API profile           | `RuntimeReferenceGuard`                           |
| Input graph and preprocessor | Resolves IDL roots and includes; applies preprocessing        | `IdlInputGraph`, `IdlPreprocessor`                |
| Front end                    | Parses, binds, and validates IDL                              | `Compiler/FrontEnd`                               |
| Compilation orchestration    | Coordinates compilation and emission handoff                  | `IdlCompilation`, `CompilationContext`            |
| Emission planning            | Chooses generated API and support shapes                      | `Emission/Model`, `Planning`                      |
| Emitters                     | Render generated C# documents                                 | Managed, native, TypeSupport, and plugin emitters |
| MSBuild targets              | Pass IDL roots and metadata to Roslyn                         | Packed NuGet package                              |
| Corpus and integration tests | Verify acceptance, generated API shape, and package use       | Separate test layers                              |

## 5.2 Level 2: Compiler and preprocessing

![Compiler and preprocessing flow from build metadata and IDL inputs through parsing, semantic validation, and emission planning.](images/05/level-2-compiler-and-preprocessing.png)

| Stage                     | Responsibility                                           | Main implementation                                                                                                |
| :------------------------ | :------------------------------------------------------- | :----------------------------------------------------------------------------------------------------------------- |
| Root intake               | Builds the graph from `DdsIdl` roots and project options | [`IdlInputGraph`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Preprocessing/IdlInputGraph.cs)            |
| Include resolution        | Resolves paths while preserving root/include identity    | [`IdlIncludeResolver`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Preprocessing/IdlIncludeResolver.cs)  |
| Preprocessing             | Applies directives and macros with source mapping        | [`IdlPreprocessor`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Preprocessing/IdlPreprocessor.cs)        |
| Parsing                   | Reads ordered annotations, type/declarator syntax, and unresolved value expressions while preserving source locations | [`IdlDeclarationParser`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Parsing/IdlDeclarationParser.cs), `IdlTypeParser` |
| Semantic model            | Assigns stable symbols and target-independent meaning, including enum identity and typedef annotations              | [`IdlSymbolTable`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Symbols/IdlSymbolTable.cs)                |
| Compilation state         | Coordinates parse, bind, and validate                    | [`IdlCompilation`](../../../src/Wireloom.Dds.Generator/Compiler/IdlCompilation.cs)                                 |
| Resolution and validation | Binds constants, enum identities, effective member IDs, deferred bounds, union labels, and typed defaults; reports source-located semantic errors | `IdlTypeBinder`, `IdlValueBinder`, `IdlValueResolver`, `IdlMemberIdBinder` |
| Specification rules       | Detects keyword, scope, and derived-key violations       | `IdlIdentifierValidator`, `IdlSemanticValidator`                                                                   |
| Validation policy         | Applies the batch strictness to specification findings   | `IdlValidationPolicy`                                                                                              |
| Emission planning         | Projects validated declarations, typed defaults, and union array lifecycle into output plans                      | [`EmissionTypeProjector`](../../../src/Wireloom.Dds.Generator/Compiler/Emission/Planning/EmissionTypeProjector.cs) |

The front end reads supported annotations through declaration, type-prefix,
member, enum-literal, and union-branch readers. It retains unresolved value
expressions and source locations, but does not retain a complete ordered list
of annotation applications attached to each target. `IdlSyntaxReader` reads
balanced delimiters and quoted literals without rewriting source offsets.
The binder resolves references, constants, member IDs, bounds, union labels, and defaults against the complete symbol graph,
then validates semantics before planning emission.
`IdlParseContext` coordinates these stages; `IdlValueBinder` owns queued
constant, typedef, and member values. `IdlValueResolver` applies typed default
and range rules, and `IdlStringLiteralDecoder` decodes individual literal
tokens before concatenating their values. Parsers retain annotation syntax and
original locations without speculative value evaluation or exception-based
deferral. Structs, unions, and enums share ordered prefix annotation parsing;
extensibility is captured once as a typed kind rather than overwritten text.
Annotation argument and usage rules remain distributed across these readers.
Verified order and argument-shape gaps are tracked in
[#91](https://github.com/eNeRGy164/wireloom/issues/91).
`IdlScalarValue` represents mutually exclusive integer, boolean, character,
floating-point, string, and enumeration values. Character and string values
retain their narrow/wide identity, and enumeration values retain their
declaring type. Constants and defaults use this same representation; an
integer evaluator can consume only an integer value. This replaces nullable
parallel scalar fields whose combinations could lose the original IDL type.
Its semantic model does not depend on the RTI version. The runtime reference
adapter selects an immutable `RtiApiProfile`; compilation orchestration passes
that profile to emission planning. Emitters render plans without comparing
RTI versions. A central result boundary checks duplicate hint names across
generated documents.

Parsers capture identifier spellings, lexical scopes, and original source
locations. `IdlInterfaceIdentifierParser` collects body declarations without
adding types to emission or binding interface references. Its scope facts
distinguish ordinary interfaces from DDS services and operation parameters. Specification rules return typed findings; the validation policy
decides whether to ignore, warn, or fail, and Roslyn presents the resulting
diagnostics. The validation scope index is private to each compilation;
collision checks preserve case-sensitive binding and generated names.

This view describes ownership rather than execution order. Build-time flow and
failure paths are in [chapter 6](06-runtime-view.md). Deterministic
preprocessing limits are in
[chapter 8.7](08-crosscutting-concepts.md#87-deterministic-preprocessing-safety).

## 5.3 Emission boundary

Emission planning owns decisions that affect generated APIs, native storage,
DynamicType metadata, and initialization. Emitters write those decisions to
source documents; they do not choose mappings independently. This keeps RTI
profile differences and IDL-shape policies out of parsing and semantic
validation.

`ScalarValueEmissionPlan` supplies managed defaults, native scalar defaults,
and DynamicType annotation values to both member and typedef emission.
Explicit and default union branches use the same branch-copy operation,
including aggregate-array element ownership.

`SequenceArrayEmissionPlan` selects opt-in direct sequence-array projections
from each member's source input after binding and validation, including class
members and union branches. It reserves deterministic helper identities,
reuses collection-wrapper emitters, and provides the same projection for
inherited class members. Managed/native wrappers preserve element ownership;
enclosing member metadata embeds an anonymous sequence inside the array. The
semantic model is not rewritten with synthetic IDL types. Unsupported
recursive element paths fail before source emission.

The generated-value contracts and their evidence are maintained in
[chapter 8.6](08-crosscutting-concepts.md#86-rti-compatible-generated-value-semantics),
the [runtime contract matrix](../../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md#coverage-matrix),
and the [corpus feature index](../../corpus/FEATURE-COVERAGE.md).
