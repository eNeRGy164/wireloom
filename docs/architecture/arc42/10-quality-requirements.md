# 10. Quality requirements

Quality goals are described in [chapter 1](01-introduction-and-goals.md). The
scenarios below make the current verification approach concrete.

## 10.1 Quality tree

```text
Wireloom quality
├── Compatibility evidence
├── Build-time developer experience
├── Diagnostic clarity
├── Compiler maintainability
└── Supply-chain integrity
```

## 10.2 Quality scenarios

| ID   | Quality                | Stimulus                                                         | Response                                                                                                                                                                                          | Metric / target                                                                                                                                                                                                                                                                                                         | Chapter 1 goal      |
| :--- | :--------------------- | :--------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :------------------ |
| Q-01 | Compatibility evidence | Run the manifest-driven corpus                                   | Positive cases compile and accepted output preserves required oracle contract shape and targeted DynamicType metadata                                                                            | Corpus compliance validates every manifest case; source export remains a separate opt-in check                                                                                                                                                                                                   | Compatibility       |
| Q-02 | Diagnostic clarity     | Add invalid or unsupported IDL                                   | Build fails with a stable diagnostic at the source location                                                                                                                                       | No silent omission; `DDSG0001` for generation failures                                                                                                                                                                                                                                                                  | Diagnostics         |
| Q-03 | Package integration    | Consume the packed `.nupkg` without a project reference          | Analyzer targets load and generated types compile                                                                                                                                                 | Dedicated package integration suite passes                                                                                                                                                                                                                                                                              | Build integration   |
| Q-04 | Maintainability        | Change one compiler stage                                        | Focused tests can exercise the stage without the full repository corpus                                                                                                                           | In-memory unit-test layer remains independent                                                                                                                                                                                                                                                                           | Maintainability     |
| Q-05 | Supply chain           | Publish a preview or release package                             | Tests, SBOM, and provenance steps run in CI                                                                                                                                                       | Workflow gates remain green                                                                                                                                                                                                                                                                                             | Supply-chain safety |
| Q-06 | Build safety           | Supply adversarial macro expansion or cancel generation          | Generation terminates deterministically without partial parser input                                                                                                                              | 100,000 macro operations; 4 MiB output; cancellation observed                                                                                                                                                                                                                                                           | Reliability         |
| Q-07 | Incremental build      | Change a root, include, unrelated input, or one of several roots | Roslyn reuses unaffected input steps and the measured invalidation scope remains explicit                                                                                                         | Baseline harness records tracked-step reasons and elapsed timings; per-root output caching remains a documented follow-up                                                                                                                                                                                               | Build integration   |
| Q-08 | Wire compatibility     | Manually dispatch the licensed DDS matrix                        | Every curated Wireloom-positive fixture runs as an independent exchange across all nine directed pairings of Wireloom C#, RTI C#, and RTI C++ on exact RTI 7.7.0; Markdown and JSON reports record all outcomes and sanitized diagnostics | Generated reports record each selected fixture and pairing; Wireloom C# array-of-sequences shape failures remain expected                                                                                                                                                                         | Compatibility       |

The C134 recursive fixed-array scenario checks absent/present hashing,
independent nested copies, equality, and optional annotations. Its wire
adaptations and limitations are described in chapter 6.

Generated API runtime contracts are verified separately from corpus shape and
wire compatibility. The runtime suite builds focused IDL fixtures and executes
managed value methods, native conversion/lifecycle APIs, and DynamicType/support
APIs on Windows and Linux. Its [coverage matrix](../../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md#coverage-matrix)
identifies executable coverage, applicable gaps, and the RTI C# or better
Coveralls measures maintained generator source; generated fixture and test
project code are excluded. Unresolved required contracts remain gaps rather
than completed compatibility evidence.
