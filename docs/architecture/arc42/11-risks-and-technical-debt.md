# 11. Risks and technical debt

## 11.1 Known risks

| Risk                                                                             | Impact                                                         | Likelihood | Mitigation                                                           |
| :------------------------------------------------------------------------------- | :------------------------------------------------------------- | :--------- | :------------------------------------------------------------------- |
| Feature coverage is incomplete                                                   | Consumers may assume RTI parity that has not been established  | High       | Publish the feature index and keep claims case-specific              |
| Oracle sources and RTI tooling are license-controlled                            | Evidence cannot always be regenerated or redistributed freely  | Medium     | Retain provenance and follow corpus review rules                     |
| Runtime and C++ interoperability evidence is narrower than source-shape evidence | Generated code may compile while a wire-level mismatch remains | Medium     | Add runtime and C++ verification tiers before making those claims    |
| IDE and Linux host behavior is not fully verified                                | Developer experience may differ outside the tested build path  | Medium     | Qualify supported hosts before making an explicit IDE/platform claim |

## 11.2 Technical debt

| Item                                                      | Impact                                                               | Priority | Notes                                                          |
| :-------------------------------------------------------- | :------------------------------------------------------------------- | :------- | :------------------------------------------------------------- |
| Cyclic alias resolution can overflow in-process           | A negative case cannot currently complete through the normal harness | High     | Move the probe to an isolated compiler-process test            |
| All inputs currently form one collected incremental batch | Unrelated roots may invalidate together and caching is less granular | Medium   | Introduce per-root graph caching when measurement justifies it |
| Some chapter and requirement details remain compact       | Future changes may need more explicit decision and quality records   | Medium   | Expand chapters or add ADRs when architecture changes          |
