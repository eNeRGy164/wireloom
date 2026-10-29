# 1. Introduction and goals

For .NET teams using [RTI](12-glossary.md#rti) Connext [DDS](12-glossary.md#dds),
[IDL](12-glossary.md#idl)-to-C# generation introduces a separate toolchain, environment variables, and custom MSBuild scripts into
everyday development. Generated files checked into source control add review
noise and can dominate the issues reported by tools such as Qodana and
SonarQube. These costs can become an accepted part of the workflow even when
teams want a simpler, modern .NET experience.

Wireloom brings generation into the normal .NET build. Its primary goal is a
drop-in replacement for RTI's C# generation step: the same supported IDL should
keep existing applications working, preserving API usage and DDS behavior.
Teams declare their generation inputs in project configuration and restore the
generator package. Generated C# remains available in the IDE and extensible
through handwritten partial types, without requiring generated files in source
control or flooding project analysis reports. The application continues to use
the RTI runtime for serialization and DDS communication.

Small generation tooling supports that experience. In the original pipeline
that motivated Wireloom, provisioning RTI generation tools added about **2 GB**
to developer machines and C# Docker build images. That observation is specific
to the selected tools in that setup. Wireloom removes the consumer build's dependency on those
generation tools. Teams can keep them installed and use them for other purposes.

The browser Playground is a supporting demo and evaluation tool. It offers a
high-quality preview of generated C# and diagnostics without installing
Wireloom, while keeping IDL source local to the browser. The project's primary
goals concern the .NET development experience; browser previews do not establish
RTI runtime or DDS wire compatibility.

## 1.1 Requirements overview

- Replace the C# generation step for supported IDL while preserving existing
  application code, generated API contracts, and DDS behavior. Record current
  compatibility gaps in [chapter 11](11-risks-and-technical-debt.md), with
  qualification policy in the [support guide](../../guides/support-and-compatibility.md)
  and application differences in the
  [migration guide](../../migration/rtiddsgen-to-wireloom.md#generated-c-api-differences).
- Keep generation inside the consuming .NET build with small tooling restored
  as a NuGet analyzer, without requiring an external generator installation.
- Express generation inputs and options in project configuration so a clean
  restore and build reproduce the setup without machine-specific tool paths,
  environment-variable wiring, or custom generation scripts.
- Generate C# that fits strict compiler and static-analysis policies without
  overwhelming project reports with generated-code findings.
- Make generated types available in the IDE without checking generated files
  into source control; preserve handwritten partial extensions across builds.
- Produce deterministic managed types plus the type-specific support expected by
  the RTI runtime.
- Report unsupported or invalid constructs at their IDL source locations,
  rejecting them when safe managed output cannot be produced. Any accepted
  input with ignored or potentially lossy behavior must produce a diagnostic.
- Verify compatibility through small, attributable cases with matching RTI
  oracles, application API usage, runtime contracts, and directed wire exchanges.
  Exact RTI 7.3.1 and 7.7.0 have qualification evidence for documented cases;
  the package version floor and profile mapping do not qualify other releases.

Explicit non-goals:

- Implementing DDS transport or replacing the RTI runtime.
- Invoking Java, native tooling, or `rtiddsgen` during a consumer build.
- Replacing or extending RTI runtime support, licensing, or vendor support.

Wireloom is an independent project with no official relationship to RTI.
Wireloom support is best effort and covers generated code, samples, plugins,
and build/IDE integration. See the
[support and compatibility policy](../../guides/support-and-compatibility.md).

## 1.2 Quality goals

| Priority | Quality                                                                                                         | Scenario (short)                                     | Acceptance criteria                                                                                                                                                                                                            |
| :------: | :-------------------------------------------------------------------------------------------------------------- | :--------------------------------------------------- | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
|    1     | [Drop-in compatibility](10-quality-requirements.md#q01-adopt-generated-application-contracts)                   | A team replaces RTI's C# generation for the same IDL | For supported shapes, existing application call sites compile unchanged and runtime contracts and directed DDS exchanges preserve expected behavior; evidence and remaining gaps are recorded for each exact RTI profile       |
|    2     | [Build integration](10-quality-requirements.md#q03-build-in-a-clean-supported-environment)                      | A clean developer or CI environment builds contracts | Restore and build generate supported contracts without an external generator installation, machine-specific tool paths, environment-variable wiring, or custom generation scripts; packed-package checks cover supported hosts |
|    3     | [Generated C# quality](10-quality-requirements.md#q18-keep-emitted-c-clean-without-shifting-noise-to-consumers) | A team builds, analyzes, and extends its contracts   | Generated files require no source-control entries; handwritten partial extensions survive regeneration; measured compiler checks pass and preventable analyzer findings are zero, with intentional exceptions recorded         |
|    4     | [Actionable diagnostics](10-quality-requirements.md#q02-correct-an-idl-problem-from-its-diagnostic)             | Invalid or unsupported IDL reaches generation        | A stable diagnostic identifies the original IDL location and explains the problem; unsafe input fails, and accepted ignored or potentially lossy behavior produces a diagnostic                                                |
|    5     | [Compiler maintainability](10-quality-requirements.md#q04-change-one-responsibility-with-focused-feedback)      | A maintainer changes compiler behavior               | Focused tests exercise changed behavior without the full corpus; compatibility evidence is attributable to individual cases                                                                                                    |

These priorities guide architectural trade-offs. The
[support policy](../../guides/support-and-compatibility.md) defines the current
compatibility scope and remaining gaps. The
[generated C# quality report](../../corpus/csharp-quality.md) records measured
compiler diagnostics, Qodana findings, intentional exclusions, and the analysis scope. Detailed quality, supply-chain, and browser scenarios
belong in [chapter 10](10-quality-requirements.md).

## 1.3 Stakeholders

These names identify human roles. One person may fill several roles, and a
team may share a role's responsibilities. Compatibility review is part of
application adoption and Wireloom maintenance. The
[runtime scenarios](06-runtime-view.md) connect each
role to its interactions with the system.

| Stakeholder                    | Responsibilities                                                                                 | Expectations                                                                                                                               |
| :----------------------------- | :----------------------------------------------------------------------------------------------- | :----------------------------------------------------------------------------------------------------------------------------------------- |
| .NET DDS application developer | Maintain IDL and application code; adopt or upgrade Wireloom and check application compatibility | Existing applications keep working with the same supported IDL; generated types remain extensible without source-control or analysis noise |
| Build and CI engineer          | Configure developer and CI builds and their quality-analysis pipelines                           | Reproducible generation through normal restore and build, with small tooling and no generation-specific machine setup or custom scripts    |
| Wireloom maintainer            | Evolve the compiler, integrations, and package; verify and document compatibility                | Focused tests, actionable diagnostics, and attributable API, runtime-contract, and DDS wire evidence for named RTI versions                |
| Playground user                | Try examples or supplied IDL and inspect generated C# and diagnostics                            | A reliable local demo without installing Wireloom or sending IDL source to a service                                                       |

---

[Index](index.md) | [2. Architecture constraints >](02-architecture-constraints.md)
