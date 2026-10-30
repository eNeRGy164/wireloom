# 9. Architectural decisions

This chapter records choices that shape future work and the context needed
to avoid reopening the same questions without new evidence. Straightforward
choices have a timeline entry. Choices needing more background have an
expanded note; the three existing ADRs retain their full records.

Accepted ADRs retain their dated context and observations. ADR-003's
single-root UI and timings describe the initial worker host; the
[current multi-file contract](../../guides/playground-workspaces.md#worker-contract-v4)
and [qualification evidence](../../testing/playground-qualification.md)
describe the evolved Playground.

Dates identify the available evidence: implementation, documentation, recorded
selection, or approval. Commit links use **author dates**, which can differ
from later rebase/commit dates. They do not establish when an undocumented
discussion or approval occurred. Short motivations summarize the documented
purpose; an alternative is described as considered only where a source records it.

## 9.1 Decision timeline

Oldest available evidence first. **Current** identifies a choice reflected in
today's architecture; it does not imply an otherwise undocumented formal
approval. Historical changes and the scope of accepted ADRs are explained below.

| Date and evidence                                                                                                                                                                                                                                                                                                                      | Decision and motivation                                                                                                                                                              | State and architectural reference                                                                                                                                                                                                                            |
| :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 2026-09-24 · implementation [d8444122](https://github.com/eNeRGy164/wireloom/commit/d8444122cf41d57d9d7be09d6448fcfd5aeed94b)                                                                                                                                                                                                          | Emit identifiable generated C# with nullable context and partial extension points, keeping maintained extensions separate from regenerated code.                                     | Current; XML documentation refined on October 8 ([6a3554fe](https://github.com/eNeRGy164/wireloom/commit/6a3554fe541965e4b08fa3d9cdd6b833afc30d00)). [Generated-code strategy](04-solution-strategy.md#45-generate-c-that-improves-the-developer-experience) |
| 2026-09-25 · documented [5386f27f](https://github.com/eNeRGy164/wireloom/commit/5386f27f1d1817402301492a238d7cb1ec82e652)                                                                                                                                                                                                              | Integrate generation through a managed Roslyn analyzer package so normal .NET build/IDE operation owns generated documents.                                                          | Current. [Build integration](04-solution-strategy.md#41-integrate-generation-into-the-net-compiler)                                                                                                                                                          |
| 2026-09-25 · documented [5386f27f](https://github.com/eNeRGy164/wireloom/commit/5386f27f1d1817402301492a238d7cb1ec82e652)                                                                                                                                                                                                              | Declare generation roots explicitly and track included IDL separately, avoiding accidental generation of every supplied file.                                                        | Current. [Input contract](05-building-block-view.md#531-shared-idl-compiler)                                                                                                                                                                                 |
| 2026-09-25 · documented [5386f27f](https://github.com/eNeRGy164/wireloom/commit/5386f27f1d1817402301492a238d7cb1ec82e652)                                                                                                                                                                                                              | Separate language meaning from Roslyn integration so compiler behavior can be understood and tested independently.                                                                   | Current direction; physical project extraction followed on October 25. [Compiler strategy](04-solution-strategy.md#42-share-one-compiler-and-keep-host-integration-in-adapters)                                                                              |
| 2026-09-25 · documented [5386f27f](https://github.com/eNeRGy164/wireloom/commit/5386f27f1d1817402301492a238d7cb1ec82e652)                                                                                                                                                                                                              | Require the consumer's explicit compatible RTI reference because generated support must match that runtime's API.                                                                    | Current; refined through API profiles. [Runtime agreement](04-solution-strategy.md#44-isolate-rti-version-differences-in-explicit-profiles)                                                                                                                  |
| 2026-09-25 · documented [5386f27f](https://github.com/eNeRGy164/wireloom/commit/5386f27f1d1817402301492a238d7cb1ec82e652)                                                                                                                                                                                                              | Advance compatibility through small retained oracle-backed cases so support claims remain attributable and scoped.                                                                   | Current. [Evidence concept](08-crosscutting-concepts.md#85-compatibility-and-evidence)                                                                                                                                                                       |
| 2026-09-27 · documentation/implementation [a87be356](https://github.com/eNeRGy164/wireloom/commit/a87be3563921b58d1f3dd07f30c6ab948bd7e2fc), [1ba7c69b](https://github.com/eNeRGy164/wireloom/commit/1ba7c69b249ed8fdb5af41ae3e2432b1dd376c7e)                                                                                         | Combine root-level settings for one compilation batch and separate orchestration from resolution/emission responsibilities.                                                          | Current; file-owned settings refined on October 25. [Configuration ownership](08-crosscutting-concepts.md#81-idl-meaning-and-identity), [compiler view](05-building-block-view.md#531-shared-idl-compiler)                                                   |
| 2026-09-28 · implementation [72802422](https://github.com/eNeRGy164/wireloom/commit/72802422e833155ad002a8f05fb76f654b487b31)                                                                                                                                                                                                          | Bound preprocessing work and output deterministically to keep adversarial expansion from consuming uncontrolled build resources.                                                     | Current; separate from sample-value unboundedness. [Predictable compilation](08-crosscutting-concepts.md#83-predictable-compilation)                                                                                                                         |
| 2026-10-02 · documented [ec8a2bd6](https://github.com/eNeRGy164/wireloom/commit/ec8a2bd6676321b004bda8ea73595bd679d23ff7)                                                                                                                                                                                                              | Refine parsing, binding, and validation boundaries so semantic rules and original-source diagnostics stay consistent.                                                                | Current refinement of compiler separation. [Language analysis](05-building-block-view.md#542-language-analysis)                                                                                                                                              |
| 2026-10-07 · implementation [c813a01b](https://github.com/eNeRGy164/wireloom/commit/c813a01bc8013ec55fc7ff6739abc2ab596b25e0)                                                                                                                                                                                                          | Run licensed DDS exchanges separately in a manually triggered workflow, keeping runtime-dependent evidence outside ordinary license-free builds.                                     | Current supporting verification. [Qualification flow](06-runtime-view.md#632-qualify-dds-exchanges-and-retained-reports)                                                                                                                                     |
| 2026-10-09 · documented [fb2e0910](https://github.com/eNeRGy164/wireloom/commit/fb2e0910e35bc00dfa202b7d9402ac9d565722cb)                                                                                                                                                                                                              | Analyze emitted C# independently of its generated-code classification so suppressed consumer noise does not hide generator quality findings.                                         | Current; later report refreshes are evidence updates. [Quality strategy](04-solution-strategy.md#46-verify-different-contracts-with-separate-test-layers), [analysis report](../../corpus/csharp-quality.md)                                                 |
| 2026-10-15 · implementation [84346603](https://github.com/eNeRGy164/wireloom/commit/84346603b0c0716561736d8624bf956c37af866d)                                                                                                                                                                                                          | Add generated API/runtime contracts as a separate verification layer because source shape alone cannot establish value or native lifecycle behavior.                                 | Current. [Quality requirements](10-quality-requirements.md), [evidence scope](08-crosscutting-concepts.md#85-compatibility-and-evidence)                                                                                                                     |
| 2026-10-15–18 · policy history [84346603](https://github.com/eNeRGy164/wireloom/commit/84346603b0c0716561736d8624bf956c37af866d), [2248577a](https://github.com/eNeRGy164/wireloom/commit/2248577ad5f9fb3a296945e699299234e06b940a), [66a6970f](https://github.com/eNeRGy164/wireloom/commit/66a6970f11f45c1b14167c2529262569f3cfa0b5) | Evolve the relationship between RTI C# compatibility and deliberate improvements; preserve the intervening restrictive wording in the history.                                       | Historical evolution; current clarification recorded on October 28. [Compatibility background](#921-compatibility-policy-evolution)                                                                                                                          |
| 2026-10-16 · documented [e01e17c5](https://github.com/eNeRGy164/wireloom/commit/e01e17c5862472442592ca1b7de05e6fa0fd5878)                                                                                                                                                                                                              | Scope Wireloom support to its generated artifacts and integration; RTI remains responsible for the runtime.                                                                          | Current boundary. [Support policy](../../guides/support-and-compatibility.md#support-boundary)                                                                                                                                                               |
| 2026-10-18 · documented [66a6970f](https://github.com/eNeRGy164/wireloom/commit/66a6970f11f45c1b14167c2529262569f3cfa0b5)                                                                                                                                                                                                              | Select one API profile from the resolved RTI version and qualify exact releases separately, keeping generated support aligned without overstating evidence.                          | Current. [Profile strategy](04-solution-strategy.md#44-isolate-rti-version-differences-in-explicit-profiles)                                                                                                                                                 |
| 2026-10-20 · implementation [e8c8f8d0](https://github.com/eNeRGy164/wireloom/commit/e8c8f8d0ce814261664384da4692290015197971)                                                                                                                                                                                                          | Preserve the .NET 8 / C# 12 consumer baseline and minimum Roslyn-host compatibility while Wireloom's producer toolchain can advance.                                                 | Current. [Consumer deployment](07-deployment-view.md#71-net-build-integration)                                                                                                                                                                               |
| 2026-10-20 · documented [9a468a61](https://github.com/eNeRGy164/wireloom/commit/9a468a61f35efaeaacbfc4ad927f7e8e75538bfd)                                                                                                                                                                                                              | Keep RTI C# sequence-array projection as the default and offer file-owned IDL-preserving mapping, separating adoption compatibility from the alternate shape.                        | Current. [Mapping trade-off](#922-sequence-array-compatibility-trade-off)                                                                                                                                                                                    |
| 2026-10-21 · documented [6e9fc279](https://github.com/eNeRGy164/wireloom/commit/6e9fc279a7daeafb1ad4bbb7266f2ddd13302e9d)                                                                                                                                                                                                              | Separate accepted specification findings from strict semantic rejection; warning promotion remains host policy.                                                                      | Current. [Failure-reporting concept](08-crosscutting-concepts.md#82-source-provenance-and-failure-reporting)                                                                                                                                                 |
| 2026-10-24 · implementation [1ff197af](https://github.com/eNeRGy164/wireloom/commit/1ff197af55969cae8c4b5a8e7de3764b1a9654ff)                                                                                                                                                                                                          | Capture annotation syntax before interpreting it and bind typed values centrally so output paths share effective semantic facts.                                                     | Current refinement. [IDL meaning](08-crosscutting-concepts.md#81-idl-meaning-and-identity)                                                                                                                                                                   |
| 2026-10-25 · implementation [cf43d5dc](https://github.com/eNeRGy164/wireloom/commit/cf43d5dca0bf42bac0ca83e474cdc28d9ea789f8), [227c907a](https://github.com/eNeRGy164/wireloom/commit/227c907a2c071480c763f656403f3ac7d8c65882)                                                                                                       | Keep mapping/effective-size overrides owned by the declaring file and identifier normalization batch-wide so include traversal cannot change policy ownership.                       | Current configuration ownership; unbounded support is opt-in, with the [qualification gap mitigated](11-risks-and-technical-debt.md#d01). [Configuration reference](../../guides/configuration.md#settings-at-a-glance)                                      |
| 2026-10-25 · implementation [00e84f78](https://github.com/eNeRGy164/wireloom/commit/00e84f788e104bbd95f0e7912ca8ac98b17c6795); approval recorded in ADR                                                                                                                                                                                | Extract one host-neutral compiler assembly and bundle it with the sole analyzer package so hosts share behavior and releases stay aligned.                                           | Accepted [ADR-001](../adr/ADR-001-share-a-host-neutral-compiler-project.md). [Shared compiler](05-building-block-view.md#531-shared-idl-compiler)                                                                                                            |
| 2026-10-26 · implementation [2df4b372](https://github.com/eNeRGy164/wireloom/commit/2df4b372c58f23939ef05de993e9650df45a1e83); approval recorded in ADR                                                                                                                                                                                | Require a per-compilation MD5 provider while keeping DDS member-ID semantics in the compiler, allowing host-specific digest production without shared mutable configuration.         | Accepted [ADR-002](../adr/ADR-002-inject-member-id-md5-per-compilation.md); its declared record date is October 25.                                                                                                                                          |
| 2026-10-26 · approval [0d107477](https://github.com/eNeRGy164/wireloom/commit/0d10747755b5550ae437389b74dac038fd1a1663)                                                                                                                                                                                                                | Run the shared compiler in a dedicated WebAssembly worker so synchronous compilation does not occupy the page thread.                                                                | Accepted [ADR-003](../adr/ADR-003-run-playground-compilation-in-a-browser-worker.md). [Playground strategy](04-solution-strategy.md#47-run-the-supporting-playground-locally-in-a-worker)                                                                    |
| 2026-10-26 · selection recorded in plan; implementation [dd005af4](https://github.com/eNeRGy164/wireloom/commit/dd005af444aa20d41dd7230549f8b7c8ad016ed0)                                                                                                                                                                              | Use GitHub Pages and promote a validated complete static candidate without rebuilding, matching static delivery and preserving tested bytes.                                         | Current. [Hosting background](#923-playground-hosting-and-artifact-promotion)                                                                                                                                                                                |
| 2026-10-27 · implementation [279500c1](https://github.com/eNeRGy164/wireloom/commit/279500c1440e8cebf56afecf79b1da69b5dca9ad), [3112de2c](https://github.com/eNeRGy164/wireloom/commit/3112de2c63d6c930f547e276a28b5ab019d5e404), [20704c75](https://github.com/eNeRGy164/wireloom/commit/20704c753ef9e86e729b829ae0b16e490c2ea748)    | Represent browser work as a virtual multi-file workspace with stable editor identity and complete source/settings snapshots; keep the mirrored protocol and stored records explicit. | Current evolution beyond the initial single-root UI. [Workspace concept](08-crosscutting-concepts.md#86-workspace-consistency)                                                                                                                               |
| 2026-10-27 · implementation [087d2df5](https://github.com/eNeRGy164/wireloom/commit/087d2df506c03c2de7cee036cd7acd61f60f2a17)                                                                                                                                                                                                          | Bound concurrent qualification exchanges and isolate active DDS domains, allowing parallel verification without mixing peers.                                                        | Current supporting-tooling policy; tuning values belong in the [wire runbook](../../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md). [Runtime flow](06-runtime-view.md#632-qualify-dds-exchanges-and-retained-reports)                          |
| 2026-10-27–28 · implementation [3cc62834](https://github.com/eNeRGy164/wireloom/commit/3cc62834d5f26447d68a58f7a946f933e1111e5c), [e574b20f](https://github.com/eNeRGy164/wireloom/commit/e574b20fd578c869bf4dcf202a02ded960338308)                                                                                                    | Consolidate wire orchestration into C# applications to reduce the languages and scripts maintainers must maintain.                                                                   | Current; rationale confirmed in the October 28 architecture review. [Maintenance background](#924-wire-tooling-and-maintained-languages)                                                                                                                     |

## 9.2 Background for choices likely to recur

### 9.2.1 Compatibility policy evolution

The repository records a real change of direction, not one unchanged policy:

- October 15: [84346603](https://github.com/eNeRGy164/wireloom/commit/84346603b0c0716561736d8624bf956c37af866d) moved from preserving observable RTI
  implementation quirks toward RTI C# as a baseline with evidenced improvements.
- October 17: [2248577a](https://github.com/eNeRGy164/wireloom/commit/2248577ad5f9fb3a296945e699299234e06b940a) narrowed this to matching RTI C# unless
  a defect was demonstrated, and stated that C++ evidence alone was insufficient.
- October 18: [66a6970f](https://github.com/eNeRGy164/wireloom/commit/66a6970f11f45c1b14167c2529262569f3cfa0b5) restored the broader baseline/improvement
  wording. The October 17 restriction is historical, not today's rule.
- October 28: this architecture review clarified the adoption promise: preserve
  working application API usage and DDS wire behavior. Observable improvements
  to developer experience, such as reliable alias formatting, are acceptable
  when they do not affect DDS wire operations.

The practical question is what the change affects. Improved diagnostics or
IntelliSense need their own behavior evidence; they must not be confused with
changed serialization, type metadata, or wire interoperability. A successful
C++ exchange alone does not prove that an existing application API remains
usable. The [strategy](04-solution-strategy.md#45-generate-c-that-improves-the-developer-experience)
and [support policy](../../guides/support-and-compatibility.md#compatibility-and-stability)
state the current promise; the [evidence concept](08-crosscutting-concepts.md#85-compatibility-and-evidence)
separates the claims.

This is a reconstructed policy history. It does not assert that a formal
options comparison or approval meeting occurred on the commit dates.

### 9.2.2 Sequence-array compatibility trade-off

The retained RTI C# projection of direct arrays of sequences can flatten the
outer array shape, while the IDL and RTI C++ representation retain it.
Keeping that projection by default preserves the expected RTI C# API shape;
selecting the IDL-preserving mapping exposes a different managed shape and
metadata contract. Neither source similarity nor the existence of an alternate
mapping alone establishes wire compatibility.

The recorded choice is to retain the compatible default and make the alternate
mapping explicit and file-owned. This avoids silently changing existing
contracts while allowing the IDL shape to be represented where selected.
Evidence remains specific to that mapping, fixture, profile, and peer scope.

[Issue #58](https://github.com/eNeRGy164/wireloom/issues/58) records the observed
shape/interoperability problem. The
[mapping reference](../../guides/configuration.md#choose-the-sequence-array-mapping)
owns configuration and consequences; the
[mapping evidence](../../testing/sequence-array-mapping.md) owns qualification.
A separately approved historical comparison of all possible defaults was not
recorded.

### 9.2.3 Playground hosting and artifact promotion

The [hosting plan](https://github.com/eNeRGy164/wireloom/blob/fda7b284218e9d29ceab2e9471c4d4b22bef3017/docs/plans/111-publish-playground.md#selected-hosting)
explicitly records GitHub Pages selection on October 26 and compares it with
Azure Static Web Apps and Azure Storage static hosting. Pages fits the existing
repository and static bundle with few resources to operate. The plan retains
Static Web Apps as an alternative if deployed verification exposes a host
limitation; it does not introduce a compiler service.

The trade-off is limited hosting control, including response headers. The
chosen delivery path validates a complete candidate and promotes those bytes,
rather than rebuilding during publication. This preserves the relationship
between the tested artifact and the deployed artifact. Public-host checks and
explicit rollback address deployment failures; they are not an automatic
rollback guarantee.

Current topology, access, base-path configuration, and procedures belong to
[chapter 7](07-deployment-view.md#72-browser-playground-deployment) and the
[deployment runbook](../../guides/playground-deployment.md). The plan retains the recorded hosting comparison.

### 9.2.4 Wire tooling and maintained languages

The primary reason for moving wire orchestration from Bash to C# was
**maintainability through reducing the languages and scripts to maintain**,
confirmed by the project stakeholder on October 28.
C# was already the repository's principal implementation and test language.

The October 27–28 changes moved orchestration into C# and separated Runner,
Verifier, and shared contracts. Typed planning and independent report
verification are consequences of that structure, not a replacement explanation
for the confirmed maintenance goal. Shell helpers remain where required for
RTI environment setup and native builds; the choice did not eliminate all shell
usage.

The previous Bash orchestration is evidenced in history. Other seriously
considered alternatives or a formal comparison were not recorded. Do not infer
a broader language-selection exercise from the implementation change.

These are supporting verification tools outside the product's building-block
tree. [Chapter 5](05-building-block-view.md#55-supporting-verification-tooling)
owns that structure, [chapter 6](06-runtime-view.md#632-qualify-dds-exchanges-and-retained-reports)
owns execution flows, and the
[wire runbook](../../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md)
owns environment and tuning details.

---

[< 8. Cross-cutting concepts](08-crosscutting-concepts.md) | [Index](index.md) | [10. Quality requirements >](10-quality-requirements.md)
