# 5. Building block view

## 5.1 Level 1: White-box overall system

The source for the diagram is [level-1-overview.puml](images/05/level-1-overview.puml).

| Building block               | Responsibility                                                                                        | Depends on                   | Notes                                     |
| :--------------------------- | :---------------------------------------------------------------------------------------------------- | :--------------------------- | :---------------------------------------- |
| Roslyn generator boundary    | Reads `AdditionalFiles`, checks host prerequisites, reports diagnostics, and adds generated documents | Roslyn                       | `Generator`                               |
| Input graph and preprocessor | Resolves roots/includes and applies defines, undefines, and conditional directives                    | IDL files and build metadata | `IdlInputGraph`, `IdlPreprocessor`        |
| Parser and semantic model    | Parses declarations, builds symbols, resolves types, and validates semantics                          | Preprocessed IDL             | Independent of Roslyn                     |
| Emission model and plans     | Projects resolved declarations into managed/native/support emission decisions                         | Semantic model               | Shared member policies                    |
| Emitters                     | Writes managed types, native companions, TypeSupport, plugins, unions, and metadata                   | Emission plans               | Deterministic generated source            |
| MSBuild package targets      | Converts `DdsIdl` roots and IDL include locations into compiler inputs                                | Consumer project             | Packed in the NuGet package               |
| Corpus and integration tests | Checks acceptance, diagnostics, RTI C# shape, and packed-package consumption                          | Generator and fixtures       | Separate fast, corpus, and package layers |
