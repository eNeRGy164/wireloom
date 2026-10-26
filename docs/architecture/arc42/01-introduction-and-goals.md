# 1. Introduction and goals

Wireloom began with the cost of bringing IDL-to-C# generation into an existing
.NET workflow. In the original build pipeline, adding the RTI generation
toolchain increased C# Docker image sizes by about **2 GB**. Developers also
needed additional tools on their machines. Project files accumulated
environment-variable settings and custom MSBuild scripts, and reproducing a
working setup across machines was difficult.

The generated C# introduced another daily cost: it lived in the codebase as
regular source files and produced large numbers of static-analysis errors and
warnings. Keeping generated contracts compatible with the team's C# quality
policies mattered alongside getting the IDL generation step to run.

These experiences established Wireloom's goals: a small generation footprint,
minimal developer-machine setup, declarative project configuration, repeatable
builds, and generated C# that works well with strict compiler and analyzer
settings. The image growth describes the project's original setup; the
footprint of another RTI toolchain depends on its selected components.

Wireloom brings generation into the normal .NET build. A consumer declares
IDL roots in its project and restores the generator package; Wireloom emits
C# source and RTI type-specific support. The application continues to use
the RTI runtime for serialization and DDS communication.

The Browser Playground offers a local evaluation path for visitors who want
to inspect generated C# and diagnostics without first installing Wireloom.
It runs the shared compiler in browser WebAssembly and does not add a browser
DDS runtime or claim wire compatibility.

## 1.1 Requirements overview

- Keep IDL generation inside the consuming .NET build through a NuGet analyzer.
- Keep generation tooling small enough to avoid provisioning the external RTI
  generator toolchain in C# build images or on developer machines.
- Express generation inputs and options in project configuration so a clean
  restore and build reproduce the setup without machine-specific tool paths,
  environment-variable wiring, or custom generation scripts.
- Generate C# that fits strict compiler and static-analysis policies, with
  generated documents managed by the compiler rather than maintained as
  ordinary source files in the codebase.
- Treat `DdsIdl` items as explicit generation roots and track their includes.
- Produce deterministic managed types plus the type-specific support expected by
  the RTI runtime.
- Report unsupported or invalid constructs at their IDL source locations,
  rejecting them when safe managed output cannot be produced. Any accepted
  input with ignored or potentially lossy behavior must produce a diagnostic.
- Compare small, attributable cases with RTI 7.7.0 / `rtiddsgen` 4.7.0 output.
- Let a visitor preview generated C# and diagnostics in a desktop browser while
  keeping IDL source in the local browser session.
- Qualify exact RTI versions through API, runtime-contract, and full directed
  wire-matrix evidence. RTI 7.7.0 is currently qualified; the package guard's
  7.3.1 floor does not establish compatibility with that version.

Explicit non-goals:

- Implementing DDS transport or replacing the RTI runtime.
- Invoking Java, native tooling, or `rtiddsgen` during a consumer build.
- Claiming complete IDL coverage, universal wire compatibility, or C++ peer
  interoperability before the required evidence exists.
- Replacing or extending RTI runtime support, licensing, or vendor support.

Wireloom is an independent project with no official relationship to RTI.
Wireloom support is best effort and covers generated code, samples, plugins,
and build/IDE integration. See the
[support and compatibility policy](../../guides/support-and-compatibility.md).

## 1.2 Quality goals

| Priority | Quality              | Scenario (short)                                     | Acceptance criteria                                                                                                                                                                                     |
| :------- | :------------------- | :--------------------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| 1        | Compatibility        | An accepted corpus case is generated                 | Managed output contains the required types, properties, and enum members from the applicable RTI C# contract shape                                                                                      |
| 2        | Build integration    | A clean developer or CI environment builds contracts | Restore and build generate the supported contracts without an external generator installation, machine-specific tool paths, or custom generation scripts                                                |
| 3        | Generated C# quality | Generated contracts enter a strict C# build          | Nullable and XML-documentation compiler checks pass for the measured corpus; analyzer findings and intentional exclusions remain recorded in the quality report                                         |
| 4        | Diagnostics          | An invalid or unsupported IDL is compiled            | A stable generator diagnostic points to the original input                                                                                                                                              |
| 5        | Maintainability      | A compiler stage changes                             | Parsing, semantic resolution, and emission remain independently testable                                                                                                                                |
| 6        | Supply-chain safety  | A package is published                               | Locked dependencies, audit, SBOM, and provenance workflows remain intact                                                                                                                                |
| 7        | Browser preview      | A visitor compiles IDL in the Playground             | A dedicated worker keeps synchronous compilation off the page thread; both RTI profiles, failure recovery, local-only source handling, and static deployment paths are browser-qualified before release |

The [generated C# quality report](../../corpus/csharp-quality.md) records the
measured compiler diagnostics, Qodana findings, and package size. Detailed
quality scenarios belong in [chapter 10](10-quality-requirements.md).

## 1.3 Stakeholders

| Stakeholder                         | Expectations                                                                              |
| :---------------------------------- | :---------------------------------------------------------------------------------------- |
| .NET DDS application team           | Repeatable setup and generated contracts that fit strict C# tooling                       |
| Developer and CI environment owners | Small generation tooling without an external generator installation                       |
| Wireloom maintainers                | Small compiler stages, clear diagnostics, and reviewable evidence                         |
| Compatibility reviewers             | Feature-level comparison with a named RTI oracle                                          |
| CI and package consumers            | Reproducible tests and a consumable analyzer package                                      |
| Repository compiler hosts           | One in-memory compiler API with explicit RTI profile selection                            |
| Playground visitors                 | A useful first preview of generated documents and diagnostics without installing Wireloom |
