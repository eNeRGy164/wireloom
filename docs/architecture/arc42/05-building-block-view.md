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
| Parsing                   | Converts preprocessed text into declarations             | [`IdlDeclarationParser`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Parsing/IdlDeclarationParser.cs)    |
| Semantic model            | Assigns stable symbols and target-independent meaning    | [`IdlSymbolTable`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Symbols/IdlSymbolTable.cs)                |
| Compilation state         | Coordinates parse, bind, and validate                    | [`IdlCompilation`](../../../src/Wireloom.Dds.Generator/Compiler/IdlCompilation.cs)                                 |
| Resolution and validation | Resolves references and reports semantic errors          | [`CompilationResolver`](../../../src/Wireloom.Dds.Generator/Compiler/CompilationResolver.cs)                       |
| Emission planning         | Projects validated declarations into output plans        | [`EmissionTypeProjector`](../../../src/Wireloom.Dds.Generator/Compiler/Emission/Planning/EmissionTypeProjector.cs) |

The front end parses declarations, binds references and deferred bounds against
the complete symbol graph, then validates semantics before planning emission.
Its semantic model does not depend on the RTI version. The runtime reference
adapter selects an immutable `RtiApiProfile`; compilation orchestration passes
that profile to emission planning. Emitters render plans without comparing
RTI versions. A central result boundary checks duplicate hint names across
generated documents.

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

The generated-value contracts and their evidence are maintained in
[chapter 8.6](08-crosscutting-concepts.md#86-rti-compatible-generated-value-semantics),
the [runtime contract matrix](../../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md#coverage-matrix),
and the [corpus feature index](../../corpus/FEATURE-COVERAGE.md).
