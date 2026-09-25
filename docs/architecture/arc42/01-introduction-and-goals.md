# 1. Introduction and goals

Wireloom removes the external code-generation step from a .NET DDS build. A
consumer declares IDL roots in its project; the Roslyn generator parses the
reachable include graph and emits C# source and RTI type-specific support.

## 1.1 Requirements overview

- Keep IDL generation inside the consuming .NET build through a NuGet analyzer.
- Treat `DdsIdl` items as explicit generation roots and track their includes.
- Produce deterministic managed types plus the type-specific support expected by
  the RTI runtime.
- Reject unsupported or invalid constructs with diagnostics at their IDL source
  locations; do not silently omit declarations.
- Compare small, attributable cases with RTI 7.3.1 / `rtiddsgen` 4.7.0 output.

Explicit non-goals:

- Implementing DDS transport or replacing the RTI runtime.
- Invoking Java, native tooling, or `rtiddsgen` during a consumer build.
- Claiming complete IDL coverage, universal wire compatibility, or C++ peer
  interoperability before the required evidence exists.

## 1.2 Quality goals

| Priority | Quality             | Scenario (short)                          | Acceptance criteria                                                                  |
| -------: | :------------------ | :---------------------------------------- | :----------------------------------------------------------------------------------- |
|        1 | Compatibility       | An accepted corpus case is generated      | Managed output matches the applicable RTI C# contract shape                          |
|        2 | Diagnostics         | An invalid or unsupported IDL is compiled | A stable generator diagnostic points to the original input                           |
|        3 | Build integration   | A consumer references the packed package  | The package contributes the analyzer and MSBuild targets without a project reference |
|        4 | Maintainability     | A compiler stage changes                  | Parsing, semantic resolution, and emission remain independently testable             |
|        5 | Supply-chain safety | A package is published                    | Locked dependencies, audit, SBOM, and provenance workflows remain intact             |

## 1.3 Stakeholders

| Stakeholder               | Expectations                                                      |
| :------------------------ | :---------------------------------------------------------------- |
| .NET DDS application team | A build-native way to generate types from Connext IDL             |
| Wireloom maintainers      | Small compiler stages, clear diagnostics, and reviewable evidence |
| Compatibility reviewers   | Feature-level comparison with a named RTI oracle                  |
| CI and package consumers  | Reproducible tests and a consumable analyzer package              |
