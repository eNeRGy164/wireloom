# Wire qualification harness

The harness prepares independent peers, plans directed exchanges, and validates
attributable reports. It is supporting qualification tooling outside the
Wireloom product. Use the [runbook](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md)
for commands and results, the [fixture reference](wire-fixtures.md) for values
and setup, and [workarounds](../../tests/Wireloom.Dds.Generator.WireCompatibility/WORKAROUNDS.md)
for exceptional reference-peer adaptations.

## Runner

The [Runner project](../../tests/Wireloom.Dds.Generator.WireCompatibility.Runner/)
owns one matrix invocation.

| Area                                                    | Responsibility                                                                                                 |
| :------------------------------------------------------ | :------------------------------------------------------------------------------------------------------------- |
| `Program.cs`, `MatrixRunner.cs`                         | Compose dependencies and own run lifecycle/finalization.                                                       |
| `MatrixOptions.cs`, `Infrastructure/MatrixWorkspace.cs` | Snapshot settings and allocate run/case paths.                                                                 |
| `Planning/`                                             | Read corpus/profile applicability, expand fixture pairings, and assign complete cases to deterministic shards. |
| `Planning/PeerPairingPolicy.cs`                         | Resolve ordered peers, profile exclusions, and fixture overrides consistently.                                 |
| `Planning/PlanArtifactWriter.cs`                        | Write retained plan/shard metadata and the canonical scenario file.                                            |
| `Peers/`                                                | Select retained oracles, discover types, and build isolated managed/native case peers.                         |
| `Infrastructure/IdlSourceResolver.cs`                   | Resolve wrapper/canonical IDL and transitive includes.                                                         |
| `Execution/ExchangeRunner.cs`                           | Schedule bounded reader-before-writer exchanges and lease unique active DDS domains.                           |
| `Evidence/`                                             | Capture per-case input/output hashes and typed generation provenance.                                          |
| `Reporting/`                                            | Preserve partial evidence, render outcomes, and decide the workflow result.                                    |

`Peers/RtiTypeDiscovery.cs`, `Peers/WireloomTypeDiscovery.cs`,
`Peers/ManagedPeerBuilder.cs`, and `Peers/NativePeerBuilder.cs` belong to
Runner. The Verifier can reuse behavior through its command surface without
owning these source files.

## Verifier and shared contracts

The [Verifier project](../../tests/Wireloom.Dds.Generator.WireCompatibility.Verifier/)
hosts standalone catalog/report/shard commands and license-free black-box checks.

| Area                                                                                                                                            | Responsibility                                                            |
| :---------------------------------------------------------------------------------------------------------------------------------------------- | :------------------------------------------------------------------------ |
| `Program.cs`, `VerifierApplication.cs`, `VerifierCommandApplication.cs`                                                                         | Compose verification and dispatch standalone commands.                    |
| `VerifierWorkspace.cs`, `Verification/VerificationContext.cs`                                                                                   | Own temporary output and command invocation/check accounting.             |
| `Verification/CaseSelectionVerification.cs`                                                                                                     | Check profile-aware case selection.                                       |
| `Verification/ShardReportVerification.cs`, `Verification/ShardIntegrityVerification.cs`                                                         | Exercise merging, completeness, uniqueness, and correct shard assignment. |
| `Verification/ReportProvenanceVerification.cs`, `Verification/ReportLifecycleVerification.cs`, `Verification/ReportPresentationVerification.cs` | Check provenance, failure recovery, sparse reports, and rendering.        |
| `Verification/RtiCharacterReferenceVerification.cs`                                                                                             | Check guarded character-binding adaptations.                              |

The [Shared project](../../tests/Wireloom.Dds.Generator.WireCompatibility.Shared/)
owns the scenario TSV and peer-label/pairing contracts consumed by both
applications. File formats that cross process/report boundaries remain explicit;
internal planning and build handoffs use typed in-memory records.

## Planning and scheduling invariants

Planning creates the complete expected exchange set before peer builds.
Applicability exclusions differ from unimplemented cases and observed failures.
Selected fixture pairings determine which peers actually need building.

The coordinator warms shared managed references with one case, then prepares
other case peers through bounded queues. Case-specific generated/intermediate/
output paths are isolated. Shared restored assets and project references are
read-only during concurrent case builds. Native CMake builds use two compiler
jobs each. The [runbook settings](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#container-variants-and-local-wire-run)
control build and exchange concurrency.

Each active exchange leases a unique DDS domain ID from 20 upward. Return it
only after both peer processes exit. Check peer exit codes and the required
`PASS` output independently; continue after handled peer failures while
preserving their outcomes.

Peers start with `RTI_MONITORING2_ENABLE=false` to avoid an unrelated monitoring
participant contending for local discovery ports. DDS communication transports
remain enabled.

## Source selection and report integrity

A per-run manifest snapshot supplies ordered case metadata. The profile's
`wire-oracles.json` chooses captured, adjusted, wrapped, or unavailable peers.
Explicit selection modes suppress fallback when required; an absent entry can
use the conventional retained source directory if present. Source availability
and successful builds remain separate from passing exchanges.

Wrapper selection and transitive include traversal are shared between capture,
preparation, and report validation. Missing or mismatched required provenance
prevents a complete evidence claim. Preserve raw captures and record all
scratch adaptations rather than modifying the raw oracle to make a test pass.

The [report contract](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#read-the-results)
rejects missing, duplicate, inconsistent, or misassigned exchanges. A complete
report can still contain failed exchanges. The license-free Verifier and
Runner `--verify-harness` mode exercise these rules without a DDS license.

## Fixture assets

The [peer/fixture project](../../tests/Wireloom.Dds.Generator.WireCompatibility/)
contains:

- `Fixtures/` and `FixtureCatalog.cs`: managed fixture construction and values.
- `Native/main.cpp.in`: DynamicData peer.
- `Native/typed-fixture-peer.hpp.in`: typed C++ fixture paths.
- `Native/peer-qos.hpp`: shared reliable-writer timing.
- `RtiOracleProbe/`: focused RTI API probes outside the wire matrix.

Historical migration comparisons and single-host measurements are retained in
the [October 27 validation record](evidence/wire-runner-2026-10-27.md).
They are observations for that work, not current throughput budgets.

[Documentation](../README.md) · [Quality procedure](quality.md)
