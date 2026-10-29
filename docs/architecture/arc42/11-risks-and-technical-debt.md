# 11. Risks and technical debt

The main risks concern adopting Wireloom: a successful build or preview can
hide a missing capability, an application-value difference, or a qualification
gap. The supporting Playground also has qualification and recovery limits.
Priorities follow the [product goals](01-introduction-and-goals.md#12-quality-goals)
and [quality requirements](10-quality-requirements.md).

## 11.1 Assessment and priority

**Impact** describes the consequence if the item affects a user. **Exposure**
describes what the evidence establishes:

- **Known gap:** A missing capability, qualification, or observed mismatch exists.
- **Conditional:** Exposure depends on the selected input, configuration,
  environment, or an operational failure.
- **Unmeasured:** The evidence does not yet establish the relevant behavior or
  budget. This is uncertainty, not proof of a defect.

**Priority** follows the chapter 1 goals:

- **High:** Address adoption blockers and unverified core promises before making
  the affected compatibility or support claim. This does not block unrelated,
  already qualified uses or every release.
- **Medium:** Close remaining Playground qualification and operational gaps,
  or act when a documented condition makes the risk relevant.
- **Low:** Improve only when measurement or a changed requirement justifies it.

**Mitigation** describes how to reduce exposure or impact. **Existing** marks
controls already in place; **Planned** marks actions still to implement.
Mitigation can make a risk manageable while a capability or qualification gap
remains open. Next actions and closure criteria state what is needed to resolve
the remaining gap.

Risk entries describe possible harm, debt entries identify an existing
capability or implementation/evidence shortfall, and open decisions identify
choices still needed. Linked guides retain procedures and detailed results.

## 11.2 Active risks

| ID            | Risk                                                                 | Impact | Exposure                                                       | Priority | Mitigation                                                                                                                 |
| :------------ | :------------------------------------------------------------------- | :----- | :------------------------------------------------------------- | :------- | :------------------------------------------------------------------------------------------------------------------------- |
| [R-01](#r-01) | Compatibility claims exceed qualified shapes, versions, or peers     | High   | Known boundaries; unmeasured outside them                      | High     | Existing: feature/profile guidance and issue tracker. Planned: reporting guidance and compatibility issue template.        |
| [R-02](#r-02) | RTI 7.3.1 enum-array defaults produce different application values   | High   | Known mismatch for C148                                        | High     | Existing: published default-value difference and focused runtime contracts; review affected adoption shapes.               |
| [R-03](#r-03) | Exact-version qualification cannot be reproduced when needed         | High   | Conditional on licensed toolchain access and retained evidence | Medium   | Existing: retained oracle sources, notices, hashes, and exact-version reproduction commands.                               |
| [R-04](#r-04) | Interactive .NET workflow fails an adoption expectation              | High   | Known qualification gap                                        | High     | Existing: fresh-package and minimum-host checks reduce build-integration risk; interactive qualification remains open.     |
| [R-05](#r-05) | Consumer analysis may report generated-code noise                    | Medium | Known qualification gap                                        | High     | Existing: generated-code identification and independent emitted-source analysis; consumer-tool qualification remains open. |
| [R-06](#r-06) | Required desktop-browser or accessibility tasks fail                 | Medium | Known qualification gap                                        | Medium   | Existing: portable browser task checks and worker isolation; required browser/accessibility qualification remains open.    |
| [R-07](#r-07) | Browser recovery is mistaken for durable source storage              | High   | Conditional on storage failure, reset, or origin changes       | Medium   | Existing: validated recovery records, ordered save/reset operations, visible failures, and independent source export.      |
| [R-08](#r-08) | Public Playground deployment fails or cannot be promptly rolled back | Medium | Conditional on hosting, assets, and retained candidates        | Medium   | Existing: artifact/provenance and live HTTPS checks, serialized publication, and manual rollback to retained candidates.   |
| [R-09](#r-09) | Large inputs or sample bounds exceed an acceptable resource envelope | High   | Conditional; memory and latency budgets unmeasured             | Medium   | Existing: deterministic processing limits, Playground source-size bounds, worker isolation, and documented finite bounds.  |

<a id="r-01"></a>

### R-01: Compatibility beyond the qualified boundary

**Consequence:** Existing applications can fail to compile, register types, or
exchange the expected DDS samples even though another fixture or API profile
passed. Profile mapping does not qualify every RTI release mapped to it, and
successful generation does not establish native lifecycle or wire behavior.

The [feature index](../../corpus/FEATURE-COVERAGE.md#recorded-wire-evidence),
[profile policy](../../guides/support-and-compatibility.md#rti-version-qualification),
and [peer workarounds](../../../tests/Wireloom.Dds.Generator.WireCompatibility/WORKAROUNDS.md)
record known failures and adapted evidence. Examples include character and
numeric union limitations, optional wide-string typed-peer limitations, and
default-restoration differences. A manually constructed peer or corrected RTI
binding qualifies its recorded scope; it does not demonstrate unmodified
RTI-generated behavior.

Recursive shapes need particular care: recursive sequence typedefs are
rejected because ordinary RTI C++ output is not usable; direct self-recursive
sequence members have scoped TypeObject-v1 exchange evidence. RTI 7.7.0's
default TypeObject-v2 setup rejects the recursive shape. Mutual recursive
struct graphs remain unqualified. Unadapted RTI C++ recursive-array TypeCode
initialization requires the documented traversal correction; default
TypeObject-v2 recursive-array interoperability remains unqualified.
See the [direct-recursion fixture](../../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#direct-recursive-sequence-matrix-fixture).

**Mitigation:** Existing feature/profile documentation and the
[support guide's issue-tracker link](../../guides/support-and-compatibility.md#support-boundary)
help users check the supported scope and report gaps. Planned: add prominent
reporting guidance to the repository README and support guide, with a dedicated
compatibility issue template. Ask people encountering a mismatch to report it
with a minimal IDL/include reproducer, Wireloom and resolved RTI versions,
SDK/OS, relevant options, expected versus actual behavior, and diagnostics or
exchange results. This makes gaps discoverable and reproducible; it does not
establish compatibility for untested inputs. No dedicated template is in place
yet.

**Next action / closure:** Qualify the actual adoption shape, exact release,
peer setup, and application contract before extending a claim. Add a retained
reproducer for a newly encountered gap and classify whether it is Wireloom,
upstream RTI, or unsupported peer behavior. Keep default `RtiCSharp` flattening
and opt-in `Idl` mapping explicit; check effective settings, identifier policy,
and warning promotion when changing an existing contract. The risk remains
ongoing as support expands. See [Q-01](10-quality-requirements.md#q-01-adopt-generated-application-contracts),
[Q-08](10-quality-requirements.md#q-08-exchange-dds-data-with-qualified-peers),
and [Q-09](10-quality-requirements.md#q-09-keep-effective-configuration-coherent).

<a id="r-02"></a>

### R-02: Known enum-array default mismatch

**Consequence:** A consumer can observe different sample values after replacing
generation or exchanging default-constructed samples. This is an observed
semantic difference, beyond the general uncertainty in [R-01](#r-01).

For C148 on RTI 7.3.1, default-constructed Wireloom enum arrays use `Color.RED`,
while retained RTI C# and C++ peers use `Color.GREEN`; cross-implementation
values are asymmetric. The 7.7.0 fixture defaults agree. The
[recorded limitation](../../corpus/FEATURE-COVERAGE.md#recorded-wire-evidence)
and [runtime contracts](../../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md#coverage-matrix)
distinguish Wireloom's declared-default behavior from peer behavior.

**Next action / closure:** Keep this difference visible in affected adoption
reviews. Reproduce it against the exact consumer shape and agree the required
application contract before changing generation; do not silently copy an
upstream quirk or claim drop-in value equivalence. Close the affected gap only
with an explicit documented resolution and matching managed/native/wire
evidence. See [Q-16](10-quality-requirements.md#q-16-preserve-generated-value-and-native-resource-behavior)
and [compatibility policy](09-architectural-decisions.md#921-compatibility-policy-evolution).

<a id="r-03"></a>

### R-03: Licensed qualification and evidence continuity

**Consequence:** A release or upstream change can outpace reproducible oracle
capture and licensed DDS exchange evidence. Missing toolchains or provenance
can prevent checking whether a regression is in Wireloom or its environment.

Exact RTI generators, runtimes, containers, and licenses remain external
qualification prerequisites. Retained captures, notices, hashes, manifests,
and reproduction commands reduce dependence on live access. License-free
package/runtime/report checks do not replace licensed exchanges.

**Next action / closure:** Preserve the [exact LTS capture inputs](../../corpus/LTS-PROFILES.md)
and [wire evidence](../../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#read-the-results).
Before a new claim or release requiring new wire evidence, confirm the exact
environment is available and retain the complete result scope without secrets.
Report integrity and passing exchanges remain separate under
[Q-24](10-quality-requirements.md#q-24-trust-a-compatibility-or-release-evidence-claim).
This is an ongoing external dependency, not missing compiler functionality.

<a id="r-04"></a>

### R-04: Interactive IDE qualification

**Consequence:** Builds pass while generated-symbol discovery, IntelliSense,
XML documentation, IDL refresh, or diagnostic navigation disappoints the
developer. This would undermine a primary reason to adopt Wireloom.

**Next action / closure:** Execute the
[Q-19 workflow](10-quality-requirements.md#q-19-use-generated-apis-interactively)
in **VS Code with documented C# tooling** and **Visual Studio 2026**.
Record IDE/extensions, SDK, RTI version, operations, and results. Include an
IDL edit and error-correction cycle. Command-line package tests establish
important integration facts but cannot close this interactive gap; choose
refresh-latency targets through [O-01](#o-01).

<a id="r-05"></a>

### R-05: Consumer analysis qualification

**Consequence:** Adopting Wireloom can leave the analysis noise that motivated
the replacement, or require consumer-specific exclusions and maintenance.

The [independent emitted-C# scan](../../corpus/csharp-quality.md#scan-scope-and-method)
checks intrinsic output quality under a declared configuration. It removes
generated classification for that purpose and is not evidence that normal
consumer **Qodana** and **SonarQube** configurations handle generated documents
as intended.

**Next action / closure:** Qualify named tool versions and representative
consumer rules, with maintained and generated code together. Record generated
classification, preventable findings, and necessary exclusions separately
from the independent source-quality result.
[Q-18](10-quality-requirements.md#q-18-keep-emitted-c-clean-without-shifting-noise-to-consumers)
defines the required outcome; a blanket all-analyzers claim is not established.

<a id="r-06"></a>

### R-06: Browser and accessibility qualification

**Consequence:** A visitor cannot complete a core evaluation task in a required
browser or through keyboard interaction, or cannot identify errors beyond color.

The [qualification guide](../../testing/playground-qualification.md) records
Chromium/workspace evidence and procedures for multiple engines. Its
dated results and selected keyboard tests do not qualify all current required
browser versions or every accessibility task.

**Next action / closure:** Record released-bundle results for desktop
**Chrome, Edge, and Firefox**, including exact version/OS and the core
keyboard/focus/label/noncolor-error tasks from
[Q-28](10-quality-requirements.md#q-28-use-the-supported-desktop-browsers) and
[Q-29](10-quality-requirements.md#q-29-complete-core-tasks-with-accessible-interaction).
Close gaps by task and environment. WebKit checks are supplementary; they do
not establish Safari/mobile support or formal accessibility conformance.
The broader UI coverage shortfall is tracked separately as [D-03](#d-03).

<a id="r-07"></a>

### R-07: Fallible local recovery

**Consequence:** A user loses source or assumes saved work will appear at a
different hostname. Browser storage may be unavailable, evicted, or invalid;
local recovery cannot write changes back to imported host files.

The implemented recovery ordering protects valid stored snapshots and newer
edits, but it does not guarantee durable storage, restoration of unsaved edits,
or automatic migration between origins.

**Next action / closure:** Keep storage failures visible, retain source backup
independent of generation, and preserve restore/save/reset ordering under
[Q-26](10-quality-requirements.md#q-26-recover-saved-work-without-losing-newer-edits).
Before an origin change, provide the
[export/import migration guidance](../../guides/playground-deployment.md#custom-domain-later).
Recheck fault/recovery behavior when persistence changes. Durable cross-origin
recovery would require a new requirement and design; it is not silently
promised by the current [workspace contract](../../guides/playground-workspaces.md#results-exports-and-local-recovery).

<a id="r-08"></a>

### R-08: Public deployment and rollback

**Consequence:** A locally working bundle is unavailable or behaves incorrectly
at the public URL, and the last suitable candidate may no longer be retained.

Pages has limited response-header control. Asset inventory/hash checks,
base-path validation, serialized publication, and live HTTPS verification reduce
exposure. Failed live checks require inspection or manual rollback; there is
no automatic rollback. Rollback depends on a compatible candidate remaining
available within the runbook's retention period.

**Next action / closure:** Apply
[Q-31](10-quality-requirements.md#q-31-detect-and-recover-an-invalid-public-deployment)
and the [deployment/rollback runbook](../../guides/playground-deployment.md#rollback)
for each publication. Retain a suitable eligible candidate for planned changes.
After expiry, use a reviewed revert and new validated build. Change the bundle
when its base path changes; a hostname change alone need not require a new
build if that base path remains correct. Revisit hosting only when a concrete
required capability exceeds Pages controls.

<a id="r-09"></a>

### R-09: Resource and processing envelope

**Consequence:** Large finite sequence/string bounds can demand substantial
native runtime buffers; large/adversarial compiler inputs can exceed acceptable
time or memory. A valid positive Int32 setting is not an affordability guarantee.

The compiler enforces deterministic preprocessing work/output/nesting limits.
The Playground additionally bounds supplied source size and isolates compilation
in a replaceable worker. These controls are not a hard process-memory quota.
Fixed preprocessing ceilings can also reject unusually generated but finite IDL.

**Next action / closure:** Measure representative compiler workloads and
consumer sample bounds before selecting budgets under [O-01](#o-01). Preserve
[Q-06 safety/cancellation](10-quality-requirements.md#q-06-bound-processing-and-honor-cancellation-safely);
raise the [published limits](../../guides/configuration.md#fixed-behavior-and-configurable-limits)
only with adversarial evidence. Distinguish build-safety limits from
sample-value allocation and the missing unbounded capability in [D-01](#d-01).

## 11.3 Existing debt and capability gaps

| ID                             | Existing shortfall                                                          | Impact                                                                                    | Priority | Next action and completion evidence                                                                                                                                                                             |
| :----------------------------- | :-------------------------------------------------------------------------- | :---------------------------------------------------------------------------------------- | :------- | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| <a id="d-01"></a>[D-01](#d-01) | General RTI-style unbounded sequence/string support is missing              | Blocks adoption for applications relying on that allocation/deserialization contract      | High     | Design and implement the opt-in contract in [#131](https://github.com/eNeRGy164/wireloom/issues/131); qualify generated metadata/native lifecycle, package integration, and exact-profile runtime/wire behavior |
| <a id="d-02"></a>[D-02](#d-02) | Collected incremental output batch invalidates unrelated roots together     | Coarser regeneration can increase edit/build cost; no agreed budget breach is established | Low      | Extend the [incremental baseline](../../performance/incremental-generator-baseline.md) to representative workloads; introduce finer caching only when measurement justifies its complexity                      |
| <a id="d-03"></a>[D-03](#d-03) | Broader combined browser UI coverage target is unmet in the retained report | UI-only branches have less regression coverage than the declared broader target           | Medium   | Review uncovered behaviors and add meaningful assertions; retain updated combined reports and required CI results, keeping the direct Node gate separate                                                        |

**D-01:** Configurable finite fallbacks do not provide the allocation and
deserialization semantics of `rtiddsgen -unboundedSupport`. See the
[configuration boundary](../../guides/configuration.md#fixed-behavior-and-configurable-limits)
and [migration map](../../migration/rtiddsgen-to-wireloom.md#rtiddsgen-option-map).
Recursive-shape interoperability restrictions in [R-01](#r-01) remain separate; solving
general unbounded support does not automatically solve them.

**D-03:** The [qualification report](../../testing/playground-qualification.md#current-local-results)
records the measured coverage scope and results. The direct Node scope has its
own gate; passing it does not meet the broader combined UI target. Closure
requires current evidence for the relevant behavior and scope, including the
required CI results.

## 11.4 Open decisions

| ID                             | Open choice                                                                                             | Priority / decision point                                               | Next step                                                                                                                                                                                                                                          |
| :----------------------------- | :------------------------------------------------------------------------------------------------------ | :---------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| <a id="o-01"></a>[O-01](#o-01) | Representative workloads and numerical build/IDE/browser/memory/footprint/cancellation/recovery budgets | Medium; decide before making numerical acceptance or performance claims | Use [chapter 10's open targets](10-quality-requirements.md#103-open-numerical-targets-and-qualification-work), measure declared environments, and agree acceptance/regression limits; historical timings and watchdogs are not stakeholder budgets |
| <a id="o-02"></a>[O-02](#o-02) | Compatibility and breaking-change policy after 1.0                                                      | Medium before a 1.0 commitment                                          | Define the public stability/versioning contract and migration expectations; the current [support policy](../../guides/support-and-compatibility.md#compatibility-and-stability) explicitly leaves this undecided                                   |

Core accessibility behavior is already required. A formal conformance level
has not been selected; that does not postpone [R-06](#r-06)'s agreed task baseline or
create a conformance claim.

---

[< 10. Quality requirements](10-quality-requirements.md) | [Index](index.md) | [12. Glossary >](12-glossary.md)
