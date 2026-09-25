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

| ID   | Quality                | Stimulus                                                | Response                                                                       | Metric / target                                        | Chapter 1 goal      |
| :--- | :--------------------- | :------------------------------------------------------ | :----------------------------------------------------------------------------- | :----------------------------------------------------- | :------------------ |
| Q-01 | Compatibility evidence | Run the manifest-driven corpus                          | Positive cases compile and accepted output preserves the oracle contract shape | Current snapshot: 225 checks, 224 passing, 1 isolated  | Compatibility       |
| Q-02 | Diagnostic clarity     | Add invalid or unsupported IDL                          | Build fails with a stable diagnostic at the source location                    | No silent omission; `DDSG0001` for generation failures | Diagnostics         |
| Q-03 | Package integration    | Consume the packed `.nupkg` without a project reference | Analyzer targets load and generated types compile                              | Dedicated package integration suite passes             | Build integration   |
| Q-04 | Maintainability        | Change one compiler stage                               | Focused tests can exercise the stage without the full repository corpus        | In-memory unit-test layer remains independent          | Maintainability     |
| Q-05 | Supply chain           | Publish a preview or release package                    | Tests, SBOM, and provenance steps run in CI                                    | Workflow gates remain green                            | Supply-chain safety |
