# 9. Architectural decisions

The following decisions are reflected in the current implementation and its
tests. A separate ADR should be added when a decision needs a full options and
trade-offs record.

| Decision                                                | Rationale                                                                         | Status / source                                        |
| :------------------------------------------------------ | :-------------------------------------------------------------------------------- | :----------------------------------------------------- |
| Use a managed Roslyn incremental generator              | Keeps generation inside the .NET compiler and consumer build                      | Current; `src/Wireloom.Dds.Generator/Api/Generator.cs` |
| Keep the compiler core independent of Roslyn            | Makes parsing, semantic validation, and emission easier to test and reason about  | Current; `IdlCompiler` and `FrontEnd`                  |
| Make `DdsIdl` roots explicit                            | Avoids accidental generation of every IDL file and gives each root clear metadata | Current; package targets and integration tests         |
| Require an explicit compatible RTI runtime reference    | Generated support is coupled to the application-selected RTI API surface          | Current; diagnostic `DDSG0003`                         |
| Advance compatibility through small oracle-backed cases | Prevents broad claims from a single successful compilation                        | Current; `docs/corpus` and compliance tests            |
