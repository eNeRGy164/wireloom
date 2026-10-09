# 5. Building block view

## 5.1 Level 1: White-box overall system

The source for the diagram is [level-1-overview.puml](images/05/level-1-overview.puml).

At level 1, the managed compiler is intentionally shown as a black box. Its
front end, preprocessing boundary, resolution, validation, emission planning,
and writer handoff are expanded in the level-2 view below.

| Building block               | Responsibility                                                                                        | Depends on                   | Notes                                                         |
| :--------------------------- | :---------------------------------------------------------------------------------------------------- | :--------------------------- | :------------------------------------------------------------ |
| Roslyn generator boundary    | Reads `AdditionalFiles`, checks host prerequisites, reports diagnostics, and adds generated documents | Roslyn                       | `Generator`                                                   |
| Input graph and preprocessor | Resolves roots/includes and applies defines, undefines, and conditional directives                    | IDL files and build metadata | `IdlInputGraph`, `IdlPreprocessor`                            |
| Front end                    | Preprocesses input, parses declarations, binds references, and validates semantics                    | Preprocessed IDL             | `Compiler/FrontEnd`; independent of Roslyn                    |
| Compilation orchestration    | Coordinates compilation state, resolution, and the handoff to emission                                | Front end and emission       | `IdlCompilation`, `CompilationContext`, `CompilationResolver` |
| Emission model and plans     | Projects resolved declarations into managed/native/support emission decisions                         | Resolved semantic model      | `Emission/Model`, `Planning`, and shared member policies      |
| Emitters                     | Writes managed types, native companions, TypeSupport, plugins, unions, and metadata                   | Emission plans               | Deterministic generated source                                |
| MSBuild package targets      | Converts `DdsIdl` roots and IDL include locations into compiler inputs                                | Consumer project             | Packed in the NuGet package                                   |
| Corpus and integration tests | Checks acceptance, diagnostics, RTI C# shape, and packed-package consumption                          | Generator and fixtures       | Separate fast, corpus, and package layers                     |

Emission planning uses the underlying struct or union type for aggregate
members and array or sequence elements whose IDL type is a typedef chain.
Standalone typedef declarations remain generated, while aggregate APIs,
collection elements, and native conversions use the underlying aggregate
support. This matches RTI's generated C# shape: raw arrays and sequences of
aggregate aliases expose initialized aggregate values and convert through the
aggregate's native type support.

Optional struct members whose value type is a struct or union use nullable
managed references and `NativeManagedOptional` storage. Their conversion plan
uses the aggregate's native type support, while DynamicType metadata marks the
member optional. When the declared member type is a typedef chain, its
DynamicType metadata preserves the declared alias support, matching RTI; the
managed property and native conversion still use the underlying aggregate.
Absent values remain null in the managed contract.

The input graph and preprocessor enforce deterministic work boundaries: macro
expansion is capped at 100,000 expansion operations and each preprocessed file
at 4 MiB of text, while conditional expressions are capped at 256 nested
parser levels. Source-origin spans map generated fragments back to exact input
text or to the macro invocation that generated them, including EOF.

## 5.2 Level 2: Compiler and preprocessing

The compiler front end and its preprocessing boundary are the most useful
level-2 zoom because they separate build-input ownership from target-independent
IDL meaning. The source for the diagram is
[level-2-compiler-and-preprocessing.puml](images/05/level-2-compiler-and-preprocessing.puml).

| Building block                 | Responsibility                                                                                                                                      | Key dependencies and interfaces                                                                                                                                                                                                                                                                                             |
| :----------------------------- | :-------------------------------------------------------------------------------------------------------------------------------------------------- | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Root and build-metadata intake | Turns `DdsIdl` roots and project-wide include/define metadata into an explicit input graph and owns recursive traversal, active-path tracking, and cycle/depth diagnostics. | [`IdlInputGraph`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Preprocessing/IdlInputGraph.cs); MSBuild metadata enters through the Roslyn generator boundary. |
| Include resolver               | Resolves candidate paths for `#include` references and preserves the root/include distinction.                                                                         | [`IdlIncludeResolver`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Preprocessing/IdlIncludeResolver.cs); IDL files and include directories. |
| IDL preprocessor               | Applies defines, undefines, conditional directives, and macros while retaining source-origin information and enforcing expansion and output limits. Focused services sit behind the stable preprocessor boundary. | [`IdlPreprocessor`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Preprocessing/IdlPreprocessor.cs); `PreprocessedIdl` is the parser-facing handoff. |
| Parser                         | Converts preprocessed IDL text into declaration-level syntax and source context without performing full semantic validation.                          | [`IdlDeclarationParser`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Parsing/IdlDeclarationParser.cs) and specialized parsers; consumes `PreprocessedIdl`. |
| Symbol and semantic model      | Gives declarations stable names and target-independent meaning during binding, before any generated C# or native shape is selected.                 | [`IdlSymbolTable`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Symbols/IdlSymbolTable.cs), semantic model types, and type resolution. |
| Compilation state              | Owns the target-independent compilation unit and coordinates the explicit parse, bind, and validate phases with project-wide context.              | [`IdlCompilation`](../../../src/Wireloom.Dds.Generator/Compiler/IdlCompilation.cs) and [`CompilationContext`](../../../src/Wireloom.Dds.Generator/Compiler/CompilationContext.cs). |
| Resolution and validation      | Resolves references after the declaration graph is available and runs deferred semantic checks, producing source-located diagnostics instead of incomplete meaning. | [`CompilationResolver`](../../../src/Wireloom.Dds.Generator/Compiler/CompilationResolver.cs), [`IdlTypeResolver`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Semantic/IdlTypeResolver.cs), and [`IdlSemanticValidator`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Semantic/IdlSemanticValidator.cs). |
| Emission model and plans       | Projects validated declarations into resolved shape, naming, union, member, and document plans consumed by managed, native, and TypeSupport emitters. | [`EmissionTypeProjector`](../../../src/Wireloom.Dds.Generator/Compiler/Emission/Planning/EmissionTypeProjector.cs); hands off to the level-1 Emission model and plans block. |

This view is structural rather than a runtime sequence: it shows ownership and
handoffs, while build-time ordering and failure scenarios remain in [chapter 6](06-runtime-view.md).

The front end follows an explicit parse -> bind -> validate sequence. Parsing
records syntax and source context, binding resolves references and deferred
bounds against the complete symbol graph, and validation applies semantic
rules before emission planning begins. Emitters return generated documents to a
central result boundary, which owns duplicate hint-name validation.
