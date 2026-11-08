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
and IDE, including MSBuild and compiler integration. Use the
[reporting guide](reporting-issues.md) and [compatibility issue template](https://github.com/eNeRGy164/wireloom/issues/new?template=compatibility.md)
for a reproducible contract or integration failure.

The RTI runtime, its installation, licensing, configuration, and DDS operation
remain RTI's scope. Use RTI support channels and entitlements you already have
for RTI runtime questions; Wireloom does not provide or extend RTI support.
Using Connext DDS still requires the applicable RTI license. Wireloom does not
replace the runtime or its license.

## RTI version qualification

The generator selects an API profile from the resolved `Rti.ConnextDds`
assembly version. A missing reference or a version below 6.1.2 produces
`DDSG0003` and no generated output. There is no target-version override:
generated code follows the runtime resolved by the build.

| Resolved RTI version      | API profile |
| :------------------------ | :---------- |
| 6.1.2 through below 7.3.1 | 6.1.2       |
| 7.3.1 through below 7.7.0 | 7.3.1       |
| 7.7.0 and later           | 7.7.0       |

Each profile follows the corresponding RTI C# API, including primitive
representations, copy ownership, metadata, and union defaults. See
[generated value semantics](generated-value-semantics.md) for those contracts.
Feature availability follows the selected release:

| Feature                                           | 6.1.2 | 7.3.1                                           | 7.7.0                                           |
| :------------------------------------------------ | :---- | :---------------------------------------------- | :---------------------------------------------- |
| Optional sequences and arrays, including typedefs | N/A   | Supported                                       | Supported                                       |
| Recursive aggregates                              | N/A   | Supported within the documented recursion scope | Supported within the documented recursion scope |
| `@must_understand`                                | N/A   | N/A                                             | Supported                                       |
| RTI `@topic` semantics                            | N/A   | Supported                                       | Supported                                       |

Explicit use of unavailable optional collections, recursive aggregates, or
`@must_understand` reports `DDSG0004` and produces no documents. These profile
limits apply to both C# and C++ peers. RTI 6.1.2 ignores `@topic`; ordinary
struct generation remains available.

Reference-binding and runtime restrictions are recorded separately from
feature availability:

| RTI version | Restricted behavior                                                                              | Scope                                                                                                                        |
| :---------- | :----------------------------------------------------------------------------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| 6.1.2       | Signed `int8` semantics, negative constants, and negative `min`, `max`, or `default` annotations | The C# binding uses `byte`; signed behavior is unsupported.                                                                  |
| 6.1.2       | RTI C# aggregate members with negative default enum values                                       | The generated reference binding does not compile; enum-only generation and Wireloom-to-C++ exchanges have separate coverage. |
| 6.1.2       | Strict RTI keyword/case-collision validation parity                                              | Wireloom applies its own name-safety checks.                                                                                 |
| 6.1.2       | XCDR2 topic types with union-valued keys                                                         | Unsupported runtime configuration.                                                                                           |

The [RTI defect catalog](../corpus/RTI-CODEGEN-DEFECTS.md) and versioned feature
matrices record the affected cases and reference-peer corrections. Wireloom
rejects 64-bit union discriminators for every profile. Oracle capture stages
generated include dependencies separately where the RTI toolchain requires it.

Qualification requires matching RTI C# oracle comparisons, generated runtime
contracts, and wire evidence for the exact release. The wire matrix targets
all nine directed peer pairings for each selected fixture.

Profile selection does not qualify other versions in the mapped range. See
the [versioned corpus](../corpus/README.md#lts-evidence-and-reproduction) and
[wire compatibility guide](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md).

For an upgrade, check the selected profile and its exact-version evidence before
changing the consumer's RTI package. Changing between profile ranges changes the
generated API contract; rebuild and review generated contracts when moving
between the profiles. The [recorded wire evidence](../corpus/FEATURE-COVERAGE.md#recorded-wire-evidence)
identifies tested revisions, reports, remaining failures, and peer adaptations.
Qualification is specific to the documented features and peer profiles; it is
not a claim that every scenario passed.

For the 7.3.1 and 7.7.0 profiles, recursive sequence typedefs remain rejected,
and mutual recursive graphs remain unqualified. Direct self-recursive sequence members have scoped TypeObject-v1
exchange evidence with required peer adjustments. Use the
[direct-recursion fixture setup](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#direct-recursive-sequence-matrix-fixture)
and [active compatibility risk](../architecture/arc42/11-risks-and-technical-debt.md#r01)
for the exact boundary.

`DdsIdlUnboundedSupport` and declaring-file `UnboundedSupport` enable RTI-style
unbounded sequences, narrow strings, and wide strings. Finite behavior remains
the default. Corpus cases C135, C136, C173, and C174 select the unbounded mode
for matching RTI and Wireloom peers. C173 and C174 exercise XCDR1 and XCDR2
across all nine directed peer pairings for RTI 7.3.1 and 7.7.0.
The [configuration guide](configuration.md#fixed-behavior-and-configurable-limits)
defines precedence and allocation behavior.

C174 verifies five application-paced phases on persistent typed readers,
including growth, shrink, regrowth, optional/nested transitions, and bounded
neighbors. Each phase checks contents and returns its loan before
acknowledgment. Fragmented fixtures verify large-payload reassembly with
positive fragment counters and no observed drops or rejects. Across the 7.3.1 and 7.7.0
profiles, 40 of its 72 scenarios involve a Wireloom-generated endpoint; the
other 32 are RTI-only reference controls. The direct C++ CDR storage/ownership
probe exercises RTI-generated reference code. Scheduler tests separately
verify bounded process cleanup and evidence handling.

The qualification covers the declared corpus payloads and configured QoS and
resource limits. It does not establish unrestricted sample-size support or
general RTI native-allocation cleanup guarantees. The receive-lifecycle and
fragmentation evidence mitigates [historical D01](../architecture/arc42/11-risks-and-technical-debt.md#d01).

## Continuous LTS verification

The [Quality workflow](../../.github/workflows/quality.yml) runs on pull
requests, pushes to `main`, and manual dispatches. Its license-free checks
cover the following versions and targets:

| Check                                                | RTI versions        | Consumer targets              | Hosts                      |
| :--------------------------------------------------- | :------------------ | :---------------------------- | :------------------------- |
| Corpus compliance against matching retained oracles  | 7.3.1, 7.7.0        | Repository target (`net10.0`) | Ubuntu 26.04               |
| Corpus generation and exact-runtime compilation      | 6.1.2, 7.3.1, 7.7.0 | Repository target (`net10.0`) | Ubuntu 26.04               |
| Generated runtime contracts                          | 6.1.2, 7.3.1, 7.7.0 | `net8.0`, `net10.0`           | Windows 2025, Ubuntu 26.04 |
| Fresh packed-analyzer integration with C# 12         | 6.1.2, 7.3.1, 7.7.0 | `net8.0`, `net10.0`           | Windows 2025, Ubuntu 26.04 |
| .NET 8-only fresh-package consumer                   | 6.1.2, 7.3.1, 7.7.0 | `net8.0`                      | Ubuntu SDK 8 container     |
| Applicability, shard planning, and report validation | 6.1.2, 7.3.1, 7.7.0 | Repository tooling            | Ubuntu 26.04               |

All 12 runtime/package combinations fail their jobs on errors, with
independent failure reporting and retained test output. Runtime restores use version- and
target-specific lock files. Corpus runs select `CORPUS_RTI_PROFILE` explicitly;
unavailable features remain N/A and contribute no compatibility failures.
The RTI C# 7.7.0 alias oracle probe checks both finite fallback and unbounded
captures on both hosts, including native conversions beyond the fallback sizes
in unbounded mode.

Builds use the SDK selected by `global.json`; .NET 8 is installed alongside it
so `net8.0` consumers execute on the .NET 8 runtime. The dedicated consumer
job independently runs the packed analyzer in an `mcr.microsoft.com/dotnet/sdk:8.0.100`
container with no .NET 10 SDK or runtime. It restores, builds, and runs the
complete package-integration suite, including the linked runtime contracts, so
it verifies the .NET 8 SDK's Roslyn host as well as the application target.
Repository defaults remain `net10.0`, and the analyzer remains `netstandard2.0`.

Licensed, exact-toolchain DDS exchanges run separately on relevant pushes to
`main` and same-repository pull requests. Maintainers can also dispatch the
full matrix for an exact revision. See the
[wire runbook](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#github-actions)
for fork and license behavior, and the
[release record](../releases/1.0.0.md#wire-evidence-and-limitations) for the
tagged 1.0.0 results.

## Compatibility and stability

The OMG IDL and DDS-XTypes specifications define the meaning of supported
language constructs. The retained RTI corpus establishes tested compatibility
for particular inputs and tool versions. It is evidence, rather than a complete
language definition; valid forms can be supported beyond that corpus, and RTI
generator limitations are documented separately from Wireloom limitations.

The compatibility goal is for existing application code to compile unchanged
against Wireloom output for supported IDL shapes. That is a goal verified by
shape-specific evidence, not a blanket claim for every IDL construct or RTI
release. Check the [feature coverage index](../corpus/FEATURE-COVERAGE.md) and
the API comparison in the [migration guide](../migration/rtiddsgen-to-wireloom.md).

Deliberate differences from RTI output, such as reliable alias `ToString`
behavior and richer IntelliSense documentation, are documented in the migration
guide and verified separately from wire compatibility. Existing documented
value behavior is covered by the stability rules below.

The following contract applies from 1.0.0 throughout the 1.x release line.
The [0.x release records](../releases/README.md) describe changes from
earlier versions.

### Public contract

For unchanged supported IDL, configuration, and RTI runtime, a 1.x upgrade
preserves application source compatibility, documented generated-value behavior,
handwritten partial extensions, and DDS metadata/wire behavior, subject to the
defect-correction exception below. Existing feature and qualification limits
still apply.

The public contract covers generated data, support, and native APIs that follow
the selected RTI API profile, including public native companions under generated
`Implementation` namespaces. It also covers documented MSBuild items,
properties, metadata, their scope and precedence, and generator diagnostics.
Generated source formatting, file layout, private implementation, and
documentation wording may change.

Generated API evolution follows RTI. Support for new RTI APIs is selected by the
corresponding newer runtime/profile; it does not change the profile used for an
existing qualified RTI version. Wireloom does not independently add members to
existing generated types that could collide with handwritten partial
extensions. Changing the application's IDL or selecting another RTI profile
requires reviewing its generated API and peer compatibility.

The bundled compiler's direct C# APIs are internal integration interfaces,
even where C# accessibility is public. They are not a separately supported
application library and have no 1.x public API stability guarantee.

### Release rules

| Release | Changes allowed                                                                                                                                                                            |
| :------ | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Patch   | Defect corrections for that release line, including correctness and security fixes. No new features or unrelated feature changes.                                                          |
| Minor   | New features, opt-in behavior, new default warnings, and support for additional RTI versions, while preserving existing supported contracts, profiles, defaults, and minimum requirements. |
| Major   | Deliberate breaking changes to supported APIs, value or wire behavior, configuration, defaults, or minimum requirements, and increases to an existing diagnostic's severity.               |

**Defect-correction exception:** A patch may correct faulty output against the
documented RTI/IDL contract even when it breaks compatibility with previous
Wireloom output. For example, correcting a wrong member ID can require peers to
upgrade together. Release notes must identify the defect and impact, including
required regeneration, application adjustments, and coordinated peer upgrades.
Preserving a known defect is not a stability promise. An intentional contract
change cannot be classified as a correction simply to avoid a major release.

Diagnostic IDs and their intended meanings remain stable and are not reused for
unrelated conditions. Message wording may improve. New default warnings belong
in minor releases and must be called out because `TreatWarningsAsErrors` can
turn them into build failures. Increasing an existing diagnostic's severity
requires a major release; a patch may correct its detection without changing
its declared severity. See the
[diagnostic reference](../../src/Wireloom.Dds.CSharp.Generator/README.md#diagnostics).

### Stable defaults and minimum requirements

Throughout 1.x, the defaults remain finite fallbacks of 100 sequence elements
and 255 UTF-8 bytes for narrow strings or UTF-16 code units for wide strings;
unbounded support, strict validation, and leading-underscore normalization
remain disabled; direct sequence arrays use `RtiCSharp` mapping. Explicit IDL
bounds take precedence. The documented batch and declaring-file scopes,
inheritance, and override precedence remain stable. New modes may be opt-in;
changing an existing default requires a major release. See
[configuration](configuration.md#settings-at-a-glance).

The consumer minimums remain .NET 8, C# 12, Roslyn 4.8.0 (the SDK 8.0.100 host),
and RTI 7.3.1 throughout 1.x. The repository's producer SDK and tooling can
advance independently while preserving those consumer requirements.

### Playground compatibility

The Playground's IDL generation, generated C#, and generation settings follow
the same public contract for the selected compiler version and RTI profile.
Its engine and browser worker protocol are internal and may change together.

Later 1.x Playground versions support forward migration of valid saved
workspaces and source exports from earlier 1.x versions, preserving source and
settings through explicitly versioned migrations. Older clients may reject
newer formats clearly rather than silently discard settings. This does not
require cross-version worker compatibility. The
[workspace guide](playground-workspaces.md#results-exports-and-local-recovery)
describes supported formats and recovery behavior.

### Maintenance

Corrections target the latest 1.x release line. Backports to older minor lines
are considered when warranted, on a best-effort basis. There is no fixed support
period or commitment to maintain every minor line. A backported patch contains
corrections relevant to that line without unrelated features.

## Choosing a generation path

| Choose Wireloom when                                                            | Keep or choose RTI generation when                                                 |
| :------------------------------------------------------------------------------ | :--------------------------------------------------------------------------------- |
| Your IDL shapes are covered and your RTI version is qualified.                  | Required IDL shapes or `rtiddsgen` options are outside Wireloom's coverage.        |
| You want IDL generation and diagnostics integrated into the .NET build and IDE. | Your existing generation workflow is qualified and meets your needs.               |
| Best-effort support is acceptable and your team can own validation.             | Your requirements include a generation tool covered by your RTI support agreement. |

Both paths use the RTI Connext DDS runtime. Wireloom is an independent
generator project and does not establish or change a support relationship with
RTI.
