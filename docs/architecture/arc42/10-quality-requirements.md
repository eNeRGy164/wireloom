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

| ID   | Scenario                          | Acceptance and evidence                                                                                                                                                                                                       |
| :--- | :-------------------------------- | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Q-01 | Run the manifest corpus           | Compliance records every manifest case; retained oracle sources are compared for required API shape. Selected generated outputs compile against exact RTI APIs, while runtime and wire verification remain separate evidence. |
| Q-09 | Select sequence-array mapping     | The default output remains RTI C# compatible; opt-in runtime tests preserve both layers on both LTS profiles. Packed consumers verify project defaults and file overrides. Dedicated C149 retains ordinary corpus sources.    |
| Q-11 | Configure unbounded finite limits | Project defaults and file overrides produce matching generated bounds for sequence and string members, typedefs, unions, and included files; explicit IDL bounds remain authoritative. Invalid values fail before emission.   |
| Q-12 | Normalize escaped IDL names       | The opt-in project setting normalizes declarations, references, macro expansions, and metadata, preserves string literals and source locations, permits escaped keywords in strict mode, and rejects normalized collisions before emission. |
| Q-08 | Dispatch the licensed matrix      | Exact LTS package and container versions are paired; reports show all nine directed pairings and expected array-of-sequences failures for the selected target.                                                                |

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

| ID   | Scenario                                | Acceptance and evidence                                                                                                                                                           |
| :--- | :-------------------------------------- | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Q-03 | Consume the packed `.nupkg`             | Analyzer targets load and generated types compile without a project reference; the package suite links the runtime contracts and passes in a .NET 8-only compiler-host container. |
| Q-07 | Change roots, includes, or other inputs | Roslyn reuses unaffected inputs; the baseline records tracked-step reasons and timings. Per-root output caching remains a follow-up.                                              |

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

| ID   | Scenario                  | Acceptance and evidence                                                                                                                                                                                        |
| :--- | :------------------------ | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Q-04 | Change one compiler stage | Focused in-memory tests exercise that stage without running the full corpus; annotation syntax capture, known-name validation, binding, generated outputs, and diagnostics have separately traceable evidence. |

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
expectation. Coveralls measures maintained generator source; generated fixture
and test project code are excluded. Unresolved required contracts remain gaps
rather than completed compatibility evidence.
