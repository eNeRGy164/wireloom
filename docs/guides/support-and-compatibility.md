# Support and compatibility

This page defines Wireloom's support boundary, RTI version qualification,
compatibility evidence, and decision guidance. Wireloom is an independent
project with no official relationship to RTI. It generates C# source and
participates in the consumer build; the RTI Connext DDS runtime remains
responsible for DDS behavior and communication.

## Support boundary

Wireloom support is best effort and has no response-time or resolution
commitment. Open Wireloom defects and requests in the
[GitHub issue tracker](https://github.com/eNeRGy164/wireloom/issues). Wireloom's
scope is generated code, samples, plugins, and integration into the .NET build
and IDE, including MSBuild and compiler integration.

The RTI runtime, its installation, licensing, configuration, and DDS operation
remain RTI's scope. Use RTI support channels and entitlements you already have
for RTI runtime questions; Wireloom does not provide or extend RTI support.
Using Connext DDS still requires the applicable RTI license. Wireloom does not
replace the runtime or its license.

## RTI version qualification

The generator selects an API profile from the resolved `Rti.ConnextDds`
assembly version. Versions below **7.3.1**, or a missing runtime reference,
produce `DDSG0003` and no generated output. Versions from 7.3.1 up to 7.7.0
select the 7.3.1 profile; 7.7.0 and later select the 7.7.0 profile. There is no
target-version override: generated code follows the runtime actually resolved
by the consumer's build.

Only the exact **7.3.1** and **7.7.0** LTS versions are qualification targets.
Other versions map to the nearest profile but are not separately tested or
qualified by that mapping. Qualification requires checking generated APIs
against the matching RTI C# oracle, compiling and exercising generated runtime
contracts, and reviewing wire evidence for the exact LTS toolchain. The wire
matrix target is all nine directed pairings for each selected fixture and
exact RTI version. See the [versioned corpus](../corpus/README.md#versioned-lts-evidence)
and [wire compatibility guide](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md).

For an upgrade, check the selected profile and its exact-version evidence before
changing the consumer's RTI package. Crossing the 7.7.0 boundary changes the
generated API profile; rebuild and review generated contracts when moving
between the profiles. The [recorded wire evidence](../corpus/FEATURE-COVERAGE.md#runtime-evidence-baseline)
identifies tested revisions, reports, remaining failures, and peer adaptations.
Qualification is specific to the documented features and peer profiles; it is
not a claim that every scenario passed.

## Continuous LTS verification

The [Quality workflow](../../.github/workflows/quality.yml) runs on pull
requests, pushes to `main`, and manual dispatches. It checks both exact RTI
LTS versions without requiring a licensed wire environment:

| Check                                                | RTI versions | Consumer targets              | Hosts                      |
| :--------------------------------------------------- | :----------- | :---------------------------- | :------------------------- |
| Corpus compliance against matching retained oracles  | 7.3.1, 7.7.0 | Repository target (`net10.0`) | Ubuntu 24.04               |
| Generated runtime contracts                          | 7.3.1, 7.7.0 | `net8.0`, `net10.0`           | Windows 2025, Ubuntu 24.04 |
| Fresh packed-analyzer integration with C# 12         | 7.3.1, 7.7.0 | `net8.0`, `net10.0`           | Windows 2025, Ubuntu 24.04 |
| Applicability, shard planning, and report validation | 7.3.1, 7.7.0 | Repository tooling            | Ubuntu 24.04               |

All eight runtime/package combinations fail their jobs on errors, with
independent failure reporting and retained test output. Runtime restores use version- and
target-specific lock files. Corpus runs select `CORPUS_RTI_PROFILE` explicitly;
unavailable features remain N/A and contribute no compatibility failures.
The existing 7.7.0 unbounded-alias oracle probe also runs on both hosts.

Builds use the SDK selected by `global.json`; .NET 8 is installed alongside it
so `net8.0` consumers execute on the .NET 8 runtime. This matrix verifies
consumer targets and generated APIs, and does not establish compatibility with
the .NET 8 SDK's older Roslyn host. Repository defaults remain `net10.0`, and
the analyzer remains `netstandard2.0`.

These checks do not establish DDS wire compatibility. Licensed, exact-toolchain
wire exchanges remain in the manually dispatched wire workflow. The local
and CI evidence for a particular revision must still be reviewed before
claiming qualification.

## Compatibility and stability

The compatibility goal is for existing application code to compile unchanged
against Wireloom output for supported IDL shapes. That is a goal verified by
shape-specific evidence, not a blanket claim for every IDL construct or RTI
release. Check the [feature coverage index](../corpus/FEATURE-COVERAGE.md) and
the API comparison in the [migration guide](../migration/rtiddsgen-to-wireloom.md).
For example, C136 currently exposes `ISequence<Scalar>` in Wireloom where the
RTI 7.7.0 C# oracle exposes `ISequence<int>`. This is tracked for investigation
in [issue #62](https://github.com/eNeRGy164/wireloom/issues/62).

The configuration API differs from `rtiddsgen` options. It may change when
issues are found; consult release notes and the package guide before upgrading.
Diagnostic IDs are expected to remain stable. New IDs may be added, and their
severity and meaning are documented in the package's
[diagnostic reference](../../src/Wireloom.Dds.Generator/README.md#diagnostics).

Wireloom is currently in the 0.x release line, where breaking changes may be
made and are called out in release notes. The policy for breaking changes
after 1.0 has not been decided.

## Choosing a generation path

| Choose Wireloom when                                                            | Keep or choose RTI generation when                                                 |
| :------------------------------------------------------------------------------ | :--------------------------------------------------------------------------------- |
| Your IDL shapes are covered and your RTI version is qualified.                  | Required IDL shapes or `rtiddsgen` options are outside Wireloom's coverage.        |
| You want IDL generation and diagnostics integrated into the .NET build and IDE. | Your existing generation workflow is qualified and meets your needs.               |
| Best-effort support is acceptable and your team can own validation.             | Your requirements include a generation tool covered by your RTI support agreement. |

Both paths use the RTI Connext DDS runtime. Wireloom is an independent
generator project and does not establish or change a support relationship with
RTI.
