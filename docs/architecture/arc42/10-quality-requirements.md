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

| ID   | Scenario                     | Acceptance and evidence                                                                                                                                           |
| :--- | :--------------------------- | :---------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Q-01 | Run the manifest corpus      | Positive cases compile and preserve required oracle shape and targeted DynamicType metadata; compliance covers every manifest case. Source export remains opt-in. |
| Q-08 | Dispatch the licensed matrix | Exact LTS package and container versions are paired; reports show all nine directed pairings and expected array-of-sequences failures for the selected target.    |

### Build-time developer experience

| ID   | Scenario                                | Acceptance and evidence                                                                                                              |
| :--- | :-------------------------------------- | :----------------------------------------------------------------------------------------------------------------------------------- |
| Q-03 | Consume the packed `.nupkg`             | Analyzer targets load and generated types compile without a project reference; package integration passes.                           |
| Q-07 | Change roots, includes, or other inputs | Roslyn reuses unaffected inputs; the baseline records tracked-step reasons and timings. Per-root output caching remains a follow-up. |

### Build safety

| ID   | Scenario                                   | Acceptance and evidence                                                                                                                                 |
| :--- | :----------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Q-06 | Exercise macro limits or cancel generation | Processing ends without partial parser input; caps are 100,000 macro operations and 4,194,304 UTF-16 code units per file, and cancellation is observed. |

### Diagnostic clarity

| ID   | Scenario                       | Acceptance and evidence                                                                                                                                                                     |
| :--- | :----------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Q-02 | Add invalid or unsupported IDL | Input that prevents safe managed output fails with source-located `DDSG0001`; deliberately ignored constructs and potentially lossy mappings report diagnostics while generation continues. |

### Compiler maintainability

| ID   | Scenario                  | Acceptance and evidence                                                      |
| :--- | :------------------------ | :--------------------------------------------------------------------------- |
| Q-04 | Change one compiler stage | Focused in-memory tests exercise that stage without running the full corpus. |

### Supply-chain integrity

| ID   | Scenario                             | Acceptance and evidence                                                             |
| :--- | :----------------------------------- | :---------------------------------------------------------------------------------- |
| Q-05 | Publish a preview or release package | CI runs tests, SBOM generation, and provenance steps; required workflow gates pass. |

The C134 recursive fixed-array scenario checks absent/present hashing,
independent nested copies, equality, and optional annotations. Fresh-process
runtime and package tests check singleton/discovery startup and complete
recursive-array serialization, including multidimensional and typedef forms.
The original RTI C# oracle retains a separately reproducible startup limitation.
Its wire adaptations and limitations are described in [chapter 6](06-runtime-view.md).

Generated API runtime contracts are verified separately from corpus shape and
wire compatibility. The runtime suite builds focused IDL fixtures and executes
managed value methods, native conversion/lifecycle APIs, and DynamicType/support
APIs on Windows and Linux. The Quality workflow runs both exact RTI LTS
packages across `net8.0` and `net10.0` consumer targets on both hosts, with
fresh packed-analyzer integration and matching-profile corpus runs. The pinned
repository SDK builds all cells; consumer runtime checks do not qualify older
SDK/Roslyn hosts. Licensed wire checks remain separate. Its [coverage matrix](../../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md#coverage-matrix)
identifies executable coverage, applicable gaps, and the RTI C# or better
expectation. Coveralls measures maintained generator source; generated fixture
and test project code are excluded. Unresolved required contracts remain gaps
rather than completed compatibility evidence.
