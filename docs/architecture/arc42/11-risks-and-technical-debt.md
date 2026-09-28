# 11. Risks and technical debt

## 11.1 Known risks

| Risk                                                                             | Impact                                                                | Likelihood | Mitigation                                                           |
| :------------------------------------------------------------------------------- | :-------------------------------------------------------------------- | :--------- | :------------------------------------------------------------------- |
| Coverage outside the retained corpus is unverified                               | Consumers may assume broader RTI parity than the evidence establishes | High       | Publish the feature index and keep claims case-specific              |
| Oracle sources and RTI tooling are license-controlled                            | Evidence cannot always be regenerated or redistributed freely         | Medium     | Retain provenance and follow corpus review rules                     |
| Oracle capture is retained as pending review evidence                            | Source-shape results may be used beyond their reviewed scope          | Medium     | Keep the manifest state visible and complete runtime/wire review     |
| Runtime and C++ interoperability evidence is narrower than source-shape evidence | Generated code may compile while a wire-level mismatch remains        | Medium     | Add runtime and C++ verification tiers before making those claims    |
| IDE and Linux host behavior is not fully verified                                | Developer experience may differ outside the tested build path         | Medium     | Qualify supported hosts before making an explicit IDE/platform claim |

## 11.2 Technical debt

| Item                                                      | Impact                                                               | Priority | Notes                                                          |
| :-------------------------------------------------------- | :------------------------------------------------------------------- | :------- | :------------------------------------------------------------- |
| All inputs currently form one collected incremental batch | Unrelated roots may invalidate together and caching is less granular | Medium   | Introduce per-root graph caching when measurement justifies it |
| Some chapter and requirement details remain compact       | Future changes may need more explicit decision and quality records   | Medium   | Expand chapters or add ADRs when architecture changes          |

A fixed preprocessing ceiling can reject unusually generated but finite IDL.
The exact limits are therefore published and should only be raised with
adversarial evidence that preserves deterministic build safety.

The current deterministic limits are:

- 4 MiB of preprocessed output per input.
- 100,000 macro expansion operations per input.
- 64 nested macro expansion levels.
- 256 nested conditional-expression levels.
- 128 include nesting levels.
- 100,000 source-origin alignment work units per expanded line.
