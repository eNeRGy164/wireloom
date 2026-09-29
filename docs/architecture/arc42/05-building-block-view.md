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
| Front end                    | Preprocesses input, parses declarations, builds symbols, resolves types, and validates semantics      | Preprocessed IDL             | `Compiler/FrontEnd`; independent of Roslyn                    |
| Compilation orchestration    | Coordinates compilation state, resolution, and the handoff to emission                                | Front end and emission       | `IdlCompilation`, `CompilationContext`, `CompilationResolver` |
| Emission model and plans     | Projects resolved declarations into managed/native/support emission decisions                         | Resolved semantic model      | `Emission/Model`, `Planning`, and shared member policies      |
| Emitters                     | Writes managed types, native companions, TypeSupport, plugins, unions, and metadata                   | Emission plans               | Deterministic generated source                                |
| MSBuild package targets      | Converts `DdsIdl` roots and IDL include locations into compiler inputs                                | Consumer project             | Packed in the NuGet package                                   |
| Corpus and integration tests | Checks acceptance, diagnostics, RTI C# shape, and packed-package consumption                          | Generator and fixtures       | Separate fast, corpus, and package layers                     |

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
| IDL preprocessor               | Applies defines, undefines, conditional directives, and macros while retaining source-origin information and enforcing expansion and output limits. | [`IdlPreprocessor`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Preprocessing/IdlPreprocessor.cs); `PreprocessedIdl` is the parser-facing handoff.                                                                                                                                                                |
| Parser                         | Converts preprocessed IDL text into declaration-level syntax and recognizes supported constants, types, typedefs, enums, structs, and unions.       | [`IdlDeclarationParser`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Parsing/IdlDeclarationParser.cs) and specialized parsers; consumes `PreprocessedIdl`.                                                                                                                                                        |
| Symbol and semantic model      | Gives declarations stable names and target-independent meaning before any generated C# or native shape is selected.                                 | [`IdlSymbolTable`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Symbols/IdlSymbolTable.cs), semantic model types, and type resolution.                                                                                                                                                                             |
| Compilation state              | Owns the target-independent compilation unit and coordinates parsed declarations with project-wide compilation context.                             | [`IdlCompilation`](../../../src/Wireloom.Dds.Generator/Compiler/IdlCompilation.cs) and [`CompilationContext`](../../../src/Wireloom.Dds.Generator/Compiler/CompilationContext.cs).                                                                                                                                          |
| Resolution and validation      | Resolves references and validates supported semantics, producing source-located diagnostics instead of incomplete meaning.                          | [`CompilationResolver`](../../../src/Wireloom.Dds.Generator/Compiler/CompilationResolver.cs), [`IdlTypeResolver`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Semantic/IdlTypeResolver.cs), and [`IdlSemanticValidator`](../../../src/Wireloom.Dds.Generator/Compiler/FrontEnd/Semantic/IdlSemanticValidator.cs). |
| Emission model and plans       | Projects validated declarations into the shared plans consumed by managed, native, and TypeSupport emitters.                                        | [`EmissionTypeProjector`](../../../src/Wireloom.Dds.Generator/Compiler/Emission/Planning/EmissionTypeProjector.cs); hands off to the level-1 Emission model and plans block.                                                                                                                                                |

This view is structural rather than a runtime sequence: it shows ownership and
handoffs, while build-time ordering and failure scenarios remain in [chapter 6](06-runtime-view.md).
