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

| ID          | Risk                                                                 | Impact | Exposure                                                       | Priority | Mitigation                                                                                                                     |
| :---------- | :------------------------------------------------------------------- | :----- | :------------------------------------------------------------- | :------- | :----------------------------------------------------------------------------------------------------------------------------- |
| [R01](#r01) | Compatibility claims exceed qualified shapes, versions, or peers     | High   | Known boundaries; unmeasured outside them                      | High     | Existing: feature/profile guidance, reporting guide, and compatibility issue template.                                         |
| [R02](#r02) | RTI 7.3.1 enum-array defaults produce different application values   | High   | Known mismatch for C148                                        | High     | Existing: published default-value difference and focused runtime contracts; review affected adoption shapes.                   |
| [R03](#r03) | Exact-version qualification cannot be reproduced when needed         | High   | Conditional on licensed toolchain access and retained evidence | Medium   | Existing: retained oracle sources, notices, hashes, and exact-version reproduction commands.                                   |
| [R04](#r04) | Interactive .NET workflow fails an adoption expectation              | High   | Conditional on IDE/profile changes                             | Medium   | Existing: named IDE/profile workflows pass with build-after-edit and explicit refresh; recheck the final packed candidate.     |
| [R05](#r05) | Consumer analysis may report generated-code noise                    | Medium | Conditional on tool/profile changes or custom rules            | Medium   | Existing: named consumer profiles pass controls and regeneration; independent emitted-source findings are reviewed separately. |
| [R06](#r06) | Required desktop-browser or accessibility tasks fail                 | Medium | Conditional on browser, platform, or task-scope changes        | Medium   | Existing: named Windows Chrome/Edge/Firefox task checks pass; repeat on bundle changes and qualify additional environments.    |
| [R07](#r07) | Browser recovery is mistaken for durable source storage              | High   | Conditional on storage failure, reset, or origin changes       | Medium   | Existing: validated recovery records, ordered save/reset operations, visible failures, and independent source export.          |
| [R08](#r08) | Public Playground deployment fails or cannot be promptly rolled back | Medium | Conditional on hosting, assets, and retained candidates        | Medium   | Existing: artifact/provenance and live HTTPS checks, serialized publication, and manual rollback to retained candidates.       |
| [R09](#r09) | Large inputs or sample bounds exceed an acceptable resource envelope | High   | Conditional; memory and latency budgets unmeasured             | Medium   | Existing: deterministic processing limits, Playground source-size bounds, worker isolation, and documented finite bounds.      |

<a id="r01"></a>

### R01: Compatibility beyond the qualified boundary

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

**Mitigation:** Existing feature/profile documentation helps users check the
supported scope. The [reporting guide](../../guides/reporting-issues.md) and
[compatibility issue template](../../../.github/ISSUE_TEMPLATE/compatibility.md)
request a minimal IDL/include reproducer, Wireloom and resolved RTI versions,
SDK/OS, relevant options, expected versus actual behavior, and diagnostics or
exchange results. The repository README and support guide link that route.
This makes gaps discoverable and reproducible; it does not establish
compatibility for untested inputs.

**Next action / closure:** Qualify the actual adoption shape, exact release,
peer setup, and application contract before extending a claim. Add a retained
reproducer for a newly encountered gap and classify whether it is Wireloom,
upstream RTI, or unsupported peer behavior. Keep default `RtiCSharp` flattening
and opt-in `Idl` mapping explicit; check effective settings, identifier policy,
and warning promotion when changing an existing contract. The risk remains
ongoing as support expands. See [Q01](10-quality-requirements.md#q01-adopt-generated-application-contracts),
[Q08](10-quality-requirements.md#q08-exchange-dds-data-with-qualified-peers),
and [Q09](10-quality-requirements.md#q09-keep-effective-configuration-coherent).

<a id="r02"></a>

### R02: Known enum-array default mismatch

**Consequence:** A consumer can observe different sample values after replacing
generation or exchanging default-constructed samples. This is an observed
semantic difference, beyond the general uncertainty in [R01](#r01).

For C148 on RTI 7.3.1, default-constructed Wireloom enum arrays use `Color.RED`,
while retained RTI C# and C++ peers use `Color.GREEN`; cross-implementation
values are asymmetric. The 7.7.0 fixture defaults agree. The
[recorded limitation](../../corpus/FEATURE-COVERAGE.md#recorded-wire-evidence)
and [runtime contracts](../../../tests/Wireloom.Dds.CSharp.RuntimeContracts/README.md#coverage-matrix)
distinguish Wireloom's declared-default behavior from peer behavior.

**Next action / closure:** Keep this difference visible in affected adoption
reviews. Reproduce it against the exact consumer shape and agree the required
application contract before changing generation; do not silently copy an
upstream quirk or claim drop-in value equivalence. Close the affected gap only
with an explicit documented resolution and matching managed/native/wire
evidence. See [Q16](10-quality-requirements.md#q16-preserve-generated-value-and-native-resource-behavior)
and [compatibility policy](09-architectural-decisions.md#921-compatibility-policy-evolution).

<a id="r03"></a>

### R03: Licensed qualification and evidence continuity

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
[Q24](10-quality-requirements.md#q24-trust-a-compatibility-or-release-evidence-claim).
This is an ongoing external dependency, not missing compiler functionality.

<a id="r04"></a>

### R04: Interactive IDE qualification

**Consequence:** Builds pass while generated-symbol discovery, IntelliSense,
XML documentation, IDL refresh, or diagnostic navigation disappoints the
developer. This would undermine a primary reason to adopt Wireloom.

**Mitigation:** Build after editing IDL. In VS Code, use a build task to refresh
generation and route compiler diagnostics to their source locations. If
IntelliSense remains stale, rerun source generators. The
[developer workflow](../../guides/developer-workflow.md) describes these steps.
Automatic IDL refresh is optional and does not block 1.0.

Interactive checks pass in both IDEs with RTI 7.3.1/`net8.0` and RTI
7.7.0/`net10.0`, using C# 12 and SDK 10.0.403. The checked scope includes
root and included-file edits, generated definitions and documentation,
diagnostic navigation and correction, and handwritten partial extensions.

**Next action / closure:** Recheck the
[Q19 workflow](10-quality-requirements.md#q19-use-generated-apis-interactively)
against the final packed candidate and when changing IDEs, tooling, SDKs, or
consumer profiles. Numerical refresh-latency targets remain under [O01](#o01).

<a id="r05"></a>

### R05: Consumer analysis configuration changes

**Consequence:** Adopting Wireloom can leave the analysis noise that motivated
the replacement, or require consumer-specific exclusions and maintenance.

The [consumer qualification](../../testing/consumer-analysis.md#qualified-scope)
passes for the named Qodana and SonarQube profiles, both RTI versions, and
.NET 8/10 consumers. Controls in maintained files and handwritten partials are
detected; removing them and regenerating root/include changes leaves zero
findings. No additional normal-consumer exclusions are required.

The [independent emitted-C# scan](../../corpus/csharp-quality.md#scan-scope-and-method)
removes generated classification and separately reviews intrinsic output
quality. Its intentional findings and wrapper-project exclusions do not become
consumer-wide suppressions.

**Mitigation / next action:** When changing tools, profiles, packaging, or
generation, repeat the [consumer procedure](../../testing/consumer-analysis.md#qualification-procedure)
with positive controls. Qualify custom rules before making a claim for them.
[Q18](10-quality-requirements.md#q18-keep-emitted-c-clean-without-shifting-noise-to-consumers)
defines the required outcome; a blanket all-analyzers claim is not established.

<a id="r06"></a>

### R06: Browser and accessibility qualification

**Consequence:** A visitor cannot complete a core evaluation task in a required
browser or through keyboard interaction, or cannot identify errors beyond color.

The [qualification guide](../../testing/playground-qualification.md#recorded-results)
records passing Windows desktop Chrome for Testing, installed Edge, and
Playwright Firefox results. Their declared phase scopes exercise the core
workspace and keyboard/focus/label/noncolor-error tasks; native clipboard and
multiline paste remain outside Firefox's portable scope.

**Mitigation / next action:** Repeat the declared checks when the bundle or
tooling changes, and qualify additional environments before extending the claim.
Preserve the task baselines from
[Q28](10-quality-requirements.md#q28-use-the-supported-desktop-browsers) and
[Q29](10-quality-requirements.md#q29-complete-core-tasks-with-accessible-interaction).
WebKit checks are supplementary; they do
not establish Safari/mobile support or formal accessibility conformance.
The broader UI coverage shortfall is tracked separately as [D03](#d03).

<a id="r07"></a>

### R07: Fallible local recovery

**Consequence:** A user loses source or assumes saved work will appear at a
different hostname. Browser storage may be unavailable, evicted, or invalid;
local recovery cannot write changes back to imported host files.

The implemented recovery ordering protects valid stored snapshots and newer
edits, but it does not guarantee durable storage, restoration of unsaved edits,
or automatic migration between origins.

**Next action / closure:** Keep storage failures visible, retain source backup
independent of generation, and preserve restore/save/reset ordering under
[Q26](10-quality-requirements.md#q26-recover-saved-work-without-losing-newer-edits).
Before an origin change, provide the
[export/import migration guidance](../../guides/playground-deployment.md#custom-domain-later).
Recheck fault/recovery behavior when persistence changes. Durable cross-origin
recovery would require a new requirement and design; it is not silently
promised by the current [workspace contract](../../guides/playground-workspaces.md#results-exports-and-local-recovery).

<a id="r08"></a>

### R08: Public deployment and rollback

**Consequence:** A locally working bundle is unavailable or behaves incorrectly
at the public URL, and the last suitable candidate may no longer be retained.

Pages has limited response-header control. Asset inventory/hash checks,
base-path validation, serialized publication, and live HTTPS verification reduce
exposure. Failed live checks require inspection or manual rollback; there is
no automatic rollback. Rollback depends on a compatible candidate remaining
available within the runbook's retention period.

The current public deployment passes HTTPS asset/hash and browser smoke checks.
A retained older candidate passes local smoke verification and the required
Quality/Coveralls, source, artifact, base-path, and environment checks. This
establishes rollback readiness without replacing the live site.

**Next action / closure:** Apply
[Q31](10-quality-requirements.md#q31-detect-and-recover-an-invalid-public-deployment)
and the [deployment/rollback runbook](../../guides/playground-deployment.md#rollback)
for each publication. Retain a suitable eligible candidate for planned changes.
After expiry, use a reviewed revert and new validated build. Change the bundle
when its base path changes; a hostname change alone need not require a new
build if that base path remains correct. Revisit hosting only when a concrete
required capability exceeds Pages controls.

<a id="r09"></a>

### R09: Resource and processing envelope

**Consequence:** Large finite sequence/string bounds can demand substantial
native runtime buffers; large/adversarial compiler inputs can exceed acceptable
time or memory. A valid positive Int32 setting is not an affordability guarantee.

The compiler enforces deterministic preprocessing work/output/nesting limits.
The Playground additionally bounds supplied source size and isolates compilation
in a replaceable worker. These controls are not a hard process-memory quota.
Fixed preprocessing ceilings can also reject unusually generated but finite IDL.

**Next action / closure:** Measure representative compiler workloads and
consumer sample bounds before selecting budgets under [O01](#o01). Preserve
[Q06 safety/cancellation](10-quality-requirements.md#q06-bound-processing-and-honor-cancellation-safely);
raise the [published limits](../../guides/configuration.md#fixed-behavior-and-configurable-limits)
only with adversarial evidence. Distinguish build-safety limits from
sample-value allocation and the [unbounded qualification envelope](../../guides/support-and-compatibility.md#rti-version-qualification).

## 11.3 Existing debt and capability gaps

| ID                          | Existing shortfall                                                                         | Impact                                                                                                        | Priority | Next action and completion evidence                                                                                                                                                                                    |
| :-------------------------- | :----------------------------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------ | :------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| <a id="d02"></a>[D02](#d02) | Collected incremental output batch invalidates unrelated roots together                    | Coarser regeneration can increase edit/build cost; no agreed budget breach is established                     | Low      | Extend the [incremental baseline](../../performance/incremental-generator-baseline.md) to representative workloads; introduce finer caching only when measurement justifies its complexity                             |
| <a id="d03"></a>[D03](#d03) | Broader combined browser UI branch-coverage target remains unmet                           | UI-only branches have less regression coverage than the declared broader target                               | Medium   | Add meaningful checks for remaining UI branches, preserve the direct Node and browser gates, and review CI results.                                                                                                    |
| <a id="d04"></a>[D04](#d04) | C142 must-understand behavior is qualified only at generation                              | Consumer, RTI plugin, CDR, and wire behavior remain unknown                                                   | Medium   | Keep C142 generation scope narrow; qualify downstream evidence separately before making those claims.                                                                                                                  |
| <a id="d05"></a>[D05](#d05) | The standards interpretation of module `@autoid(HASH)` across reopened blocks remains open | Current analyzer, C#, and C++ evidence agree on block-scoped IDs, but the normative reading remains uncertain | Medium   | Keep the behavior evidenced and review the documented scope against OMG DDS-XTypes 1.3 §7.3.1.2.1.1 and IDL 4.2 §7.4.1.4.2 before treating it as a general standards resolution. Preserve regression and RTI evidence. |

**D03:** The [qualification report](../../testing/playground-qualification.md#current-local-results)
records the current scopes and gates. Combined UI coverage passes the enforced
70% branch / 80% line gate. The broader 98% branch / 99% line target remained
open at generator 1.0.0. Remaining work includes
storage/preferences fallback paths, workspace import/cancellation and alternate
actions, and dialog validation. The direct Node gate remains separate. Closing
D03 requires meaningful checks and current local/CI results for those branches;
the release decision does not lower its target.

**D04:** The [C++ coverage record](../../corpus/7.7.0/FEATURE-COVERAGE.md)
retains C142's generation-only scope. Downstream API, plugin, CDR, and wire
qualification remain separate work.

**D05:** The [module auto-ID scope reference](../../guides/idl-semantics.md#module-auto-id-scope)
records current block-scoped behavior, exact RTI IDs, and the open standards
question. Changing that scope can change member IDs and compatibility; the
observed agreement does not settle the normative interpretation.

## 11.4 Open decisions

| ID                          | Open choice                                                                                             | Priority / decision point                                               | Next step                                                                                                                                                                                                                                          |
| :-------------------------- | :------------------------------------------------------------------------------------------------------ | :---------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| <a id="o01"></a>[O01](#o01) | Representative workloads and numerical build/IDE/browser/memory/footprint/cancellation/recovery budgets | Medium; decide before making numerical acceptance or performance claims | Use [chapter 10's open targets](10-quality-requirements.md#103-open-numerical-targets-and-qualification-work), measure declared environments, and agree acceptance/regression limits; historical timings and watchdogs are not stakeholder budgets |

Core accessibility behavior is already required. A formal conformance level
has not been selected; that does not postpone [R06](#r06)'s agreed task baseline or
create a conformance claim.

## 11.5 Mitigated debt (historical)

<a id="d01"></a>

### D01: Unbounded receive-lifecycle and fragmentation qualification

**Status: Mitigated.** The former gap was the absence of repeated receive-side
growth, shrink, and reuse evidence and confirmed fragmented-payload reception
for RTI-style unbounded support.

C174 verifies five application-paced phases on persistent typed readers:
empty, large, small, empty, and larger. It covers unbounded sequences,
narrow/wide strings, aliases, optional/nested transitions, and bounded
neighbors, with loan return before each phase acknowledgment. Both exact RTI
LTS profiles, XCDR1/XCDR2, all nine directed peer pairings, and ordinary and
fragmented fixtures are covered: 72 scenarios and 360 verified phase payloads.
Fragmented fixtures retain positive fragment/reassembly counters and effective
transport/resource settings. See the [C174 corpus record](../../corpus/manifest.json)
and the [7.3.1](../../corpus/7.3.1/FEATURE-COVERAGE.md) and
[7.7.0](../../corpus/7.7.0/FEATURE-COVERAGE.md) evidence indexes.

This mitigation applies to the declared payloads and QoS settings. The
[support policy](../../guides/support-and-compatibility.md#rti-version-qualification)
owns the current qualification envelope and distinguishes Wireloom endpoint
checks from RTI reference controls. Unrestricted sample sizes and general RTI
runtime cleanup guarantees are outside this debt item's closure criterion.
The ongoing compatibility and resource risks remain in [R01](#r01) and
[R09](#r09).

## 11.6 Modern C++ qualification risks

C++ generation does not imply consumer, plugin, CDR, or live-wire support for
every accepted shape. The [generator guide](../../../src/Wireloom.Dds.Cpp.Generator/README.md)
and [native fixtures](../../../tests/Wireloom.Dds.Generator.WireCompatibility/Native/)
define supported shapes and checks. Qualification requires exact run reports.

| Risk                               | Consequence                                                                                                              | Mitigation / next step                                                                                                                                             |
| :--------------------------------- | :----------------------------------------------------------------------------------------------------------------------- | :----------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Version-sensitive RTI integration  | SDK changes can invalidate private TypeCode layout, sample-access, interpreter, and plugin ABI assumptions.              | Keep exact-version guards and shape gates; revalidate native consumers and wire behavior before extending a profile.                                               |
| C-layout versus STL-backed storage | A generated value or inspectable DynamicType can still fail during registration, lifecycle, or serialization.            | Select compatible sample access and keep unsupported operations fail-closed under [ADR-006](../adr/ADR-006-use-stl-aware-sample-access-for-modern-cpp.md).         |
| Recursive and composed type graphs | Metadata may work while plugin initialization or TypeObject registration remains unsupported.                            | Preserve staged initialization and explicit rejection; qualify each operation independently under [ADR-007](../adr/ADR-007-stage-modern-cpp-type-registration.md). |
| C++ API differences                | Public optional fields and other mapping differences can require consumer changes despite usable metadata.               | Keep differences explicit and verify actual consumer usage; see [C++ optional-member semantics](../../guides/cpp-generated-value-semantics.md#optional-members).   |
| Qualification overreach            | A local CDR pass, adjusted oracle, or single-platform result can be mistaken for general interoperability.               | Preserve exact scope and platform failures in retained reports; require independent mixed-peer evidence for wire claims.                                           |
| Native AOT delivery                | Embedded runtime fixes require new packages; two host payloads do not qualify other architectures or Linux environments. | Service pinned payloads and verify packaged tools on each supported host under [ADR-005](../adr/ADR-005-deliver-native-cpp-generator-through-nuget.md).            |

Internal RTI interface support/licensing review remains open under
[ADR-006](../adr/ADR-006-use-stl-aware-sample-access-for-modern-cpp.md).
Feed publication, signing, and release qualification remain separate C++ release
tasks. Changes to shared IDL assembly composition also require C# analyzer and
Playground package/bundle checks.

Recorded platform limits remain material: the C114 optional-string-sequence
Linux rerun crashed in both RTI and Wireloom for wide-string states; zero-byte
comparisons after those failures are invalid. Windows success does not qualify
those Linux states. The [fixture](../../../tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppOptionalStringSequences/README.md)
defines the exercised shape.

C047's recorded FlatData interoperability is limited to RTI 7.7.0.0 and the
final, unkeyed, fixed-size two-`long`, XCDR2-only sample. Same-plugin UDP and
two-way RTI C# peer exchanges do not qualify other FlatData shapes or profiles;
the private plugin/interface dependency remains under
[ADR-006](../adr/ADR-006-use-stl-aware-sample-access-for-modern-cpp.md).

These risks refine [R01](#r01)'s compatibility boundary. Detailed fixture
results remain in their run reports rather than being repeated here.

---

[< 10. Quality requirements](10-quality-requirements.md) | [Index](index.md) | [12. Glossary >](12-glossary.md)
