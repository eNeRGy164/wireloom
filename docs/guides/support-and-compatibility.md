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

Wireloom qualifies exact RTI Connext versions for which the evidence has been
run and reviewed. The current qualified version is **7.7.0**. The runtime
package guard accepts versions from **7.3.1** onward, but that lower bound is a
reference check, not a supported-version range.

The next compatibility step is to qualify 7.3.1 and investigate intervening
versions. A version becomes qualified only after checking the generated API
against its RTI C# oracle, compiling and exercising the generated runtime
contracts, and running the full directed wire matrix among Wireloom C#, RTI
C#, and RTI C++ peers. The wire-matrix target is all nine directed pairings
for each selected fixture and exact RTI version. An unlisted version is
unqualified, even if the package guard accepts it.

For an upgrade, check this list and the linked evidence before changing the
consumer's RTI package. Keep the prior qualified package version available for
rollback. If the candidate version is not listed or its required evidence is
incomplete, retain the qualified version until that candidate is investigated
and qualified. The [current recorded wire evidence](../corpus/FEATURE-COVERAGE.md#runtime-evidence-baseline)
identifies the tested revision, report, remaining failures, and peer adaptations.
The [wire compatibility guide](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md)
explains the matrix and reproduction. Version qualification remains specific to
the documented feature and peer profiles; it is not a claim that every scenario
passed.

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

| Choose Wireloom when | Keep or choose RTI generation when |
| :-- | :-- |
| Your IDL shapes are covered and your RTI version is qualified. | Required IDL shapes or `rtiddsgen` options are outside Wireloom's coverage. |
| You want IDL generation and diagnostics integrated into the .NET build and IDE. | Your existing generation workflow is qualified and meets your needs. |
| Best-effort support is acceptable and your team can own validation. | Your requirements include a generation tool covered by your RTI support agreement. |

Both paths use the RTI Connext DDS runtime. Wireloom is an independent
generator project and does not establish or change a support relationship with
RTI.
