# 10. Quality requirements

Quality goals are described in [chapter 1](01-introduction-and-goals.md). The
scenarios below make the current verification approach concrete.

## 10.1 Quality tree

```text
Wireloom quality
├── Compatibility evidence
├── Build-time developer experience
├── Build safety
├── Diagnostic clarity
├── Compiler maintainability
└── Supply-chain integrity
```

## 10.2 Quality scenarios

### Compatibility evidence

| ID   | Scenario                          | Acceptance and evidence                                                                                                                                                                                                                     |
| :--- | :-------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Q-01 | Run the manifest corpus           | Compliance records every manifest case; retained oracle sources are compared for required API shape. Selected generated outputs compile against exact RTI APIs, while runtime and wire verification remain separate evidence.               |
| Q-09 | Select sequence-array mapping     | The default output remains RTI C# compatible; opt-in runtime tests preserve both layers on both LTS profiles. Packed consumers verify project defaults and file overrides. Dedicated C149 retains ordinary corpus sources.                  |
| Q-11 | Configure unbounded finite limits | Project defaults and file overrides produce matching generated bounds for sequence and string members, typedefs, unions, and included files; explicit IDL bounds remain authoritative. Invalid values fail before emission.                 |
| Q-12 | Normalize escaped IDL names       | The opt-in project setting normalizes declarations, references, macro expansions, and metadata, preserves string literals and source locations, permits escaped keywords in strict mode, and rejects normalized collisions before emission. |
| Q-08 | Dispatch the licensed matrix      | Exact LTS package and container versions are paired; reports show all nine directed pairings and expected array-of-sequences failures for the selected target.                                                                              |

Sixteen typed C163–C172 fixtures exercise all directed pairings among Wireloom
C#, RTI C#, and RTI C++. C164 and C172 use wrappers around unchanged canonical
IDL; RTI 7.3.1 reference generation uses byte-identical staged include files.
C171's IDL defines typed defaults and member overrides. A sparse appendable
writer contains only `inherited`; the reader must supply the declared defaults
for omitted trailing members. This is checked through DDS deserialization,
since `DynamicData.ClearMember` leaves non-optional members present. RTI 7.3.1
lacks the member-annotation API used by the sparse-field path, which is scoped
to RTI 7.7.0.
Runtime-contract tests distinguish managed/native fractional and
wide-string initialization from RTI 7.7.0 `ClearMember` limitations for
fractional metadata defaults and non-ASCII wide-string defaults. The [wire guide](../../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#annotation-and-value-binding-cases)
defines fixture and adaptation scope.

### Build-time developer experience

| ID   | Scenario                                | Acceptance and evidence                                                                                                                                                                                              |
| :--- | :-------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Q-03 | Consume the packed `.nupkg`             | Analyzer targets and the adjacent compiler assembly load and generated types compile without a project reference; the package suite links the runtime contracts and passes in a .NET 8-only compiler-host container. |
| Q-07 | Change roots, includes, or other inputs | Roslyn reuses unaffected inputs; the baseline records tracked-step reasons and timings. Per-root output caching remains a follow-up.                                                                                 |
| Q-13 | Build the checked-in DDS sample         | CI restores released Wireloom.Dds.Generator 0.5.0 from NuGet.org in locked mode and builds the sample on Windows and Ubuntu without an RTI license; local licensed exchange steps are in the sample README.          |

### Build safety

| ID   | Scenario                                   | Acceptance and evidence                                                                                                                                 |
| :--- | :----------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Q-06 | Exercise macro limits or cancel generation | Processing ends without partial parser input; caps are 100,000 macro operations and 4,194,304 UTF-16 code units per file, and cancellation is observed. |

### Diagnostic clarity

| ID   | Scenario                       | Acceptance and evidence                                                                                                                                                                          |
| :--- | :----------------------------- | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Q-02 | Add invalid or unsupported IDL | Input that prevents safe managed output fails with source-located `DDSG0001`; deliberately ignored constructs and potentially lossy mappings report diagnostics while generation continues.      |
| Q-10 | Choose strict validation       | Naming violations produce `DDSG0108` and sources by default; strict mode produces source-located `DDSG0001` and no sources. Paired C150–C162 cases and packed-consumer checks verify both modes. |

### Compiler maintainability

| ID   | Scenario                       | Acceptance and evidence                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       |
| :--- | :----------------------------- | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Q-04 | Change one compiler stage      | Focused in-memory tests exercise the shared compiler without running the full corpus; its public result, explicit profile, source-located diagnostics, cancellation, input snapshots, formatting parity, annotation capture, binding, and generated outputs have separately traceable evidence. Compiler unit coverage is gated at 90% branches and 95% lines. The standalone fuzzing project checks crash resistance and does not contribute coverage.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| Q-14 | Publish the browser Playground | Stage 0 acceptance passed on Windows Edge 156.0.4314.42: 32 exact native/browser comparisons, recovery, limits, and responsiveness. Stage 1 adds editing, examples, diagnostic navigation, exact downloads/ZIP, recovery, and asset/worker retry. Edge, Firefox 156, and Playwright WebKit 26.6 checks pass for the scoped flows; WebKit is not Safari qualification. Strict ESLint/format checks, 50 direct Node tests with 95% branch and 98% line coverage thresholds, first-party Chromium browser coverage, and 28 focused adapter cases run in the Quality workflow; Coveralls includes the browser and adapter Cobertura reports, while Community Qodana scans the .NET solution. GitHub Actions/Qodana/Coveralls results await the PR run. See [browser runtime evidence](06-runtime-view.md#64-browser-playground-compilation) and [Stage 1 evidence](../../plans/109-browser-playground-stage1-evidence.md). Accessibility, supported-browser, public-network startup, and release qualification remain #110 work. |

### Supply-chain integrity

| ID   | Scenario                             | Acceptance and evidence                                                             |
| :--- | :----------------------------------- | :---------------------------------------------------------------------------------- |
| Q-05 | Publish a preview or release package | CI runs tests, SBOM generation, and provenance steps; required workflow gates pass. |

The C134 recursive fixed-array scenario checks absent/present hashing,
independent nested copies, equality, and optional annotations. Fresh-process
runtime and package tests check singleton/discovery startup and complete
recursive-array serialization, including multidimensional and typedef forms.
The retained RTI C# oracle has a separately reproducible startup limitation.
Its wire adaptations and limitations are described in [chapter 6](06-runtime-view.md).

Generated API runtime contracts are verified separately from corpus shape and
wire compatibility. The runtime suite builds focused IDL fixtures and executes
managed value methods, native conversion/lifecycle APIs, and DynamicType/support
APIs on Windows and Linux. The Quality workflow runs both exact RTI LTS
packages across `net8.0` and `net10.0` consumer targets on both hosts, with
fresh packed-analyzer integration and matching-profile corpus runs. The pinned
repository SDK builds the package. A separate SDK 8 container restores, builds,
and runs the fresh package consumer with no .NET 10 installation, qualifying
the supported .NET 8 Roslyn host. Licensed wire checks remain separate. Its [coverage matrix](../../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md#coverage-matrix)
identifies executable coverage, applicable gaps, and the RTI C# or better
expectation. Coveralls measures maintained compiler and generator source;
generated fixture and test project code are excluded. The extracted compiler's
exact generated text is compared against the pre-extraction baseline for both
RTI profiles. Unresolved required contracts remain gaps rather than completed
compatibility evidence.

> [!NOTE]
> Additional information: This architecture context is also summarized in
> [Architecture memory](../architecture-memory.md) and
> `.agents/architecture-memory.yaml` so implementation agents can use it
> efficiently in limited context windows.
> The arc42 chapters, linked ADRs, and contracts remain the primary source
> documents. If the derived memory differs, update the source docs first and
> refresh the derived memory.
