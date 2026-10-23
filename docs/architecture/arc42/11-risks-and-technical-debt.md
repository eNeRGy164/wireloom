# 11. Risks and technical debt

The main adoption risks are compatibility beyond the retained corpus, known
mapping gaps, and dependence on licensed interoperability evidence. The table
below states their impact and mitigation; reference-binding details follow it.

## 11.1 Known risks

| Risk                                                                                      | Impact                                                                              | Likelihood | Mitigation                                                                                                                                                                                               |
| :---------------------------------------------------------------------------------------- | :---------------------------------------------------------------------------------- | :--------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Coverage outside the retained corpus is unverified                                        | Consumers may assume broader RTI parity than the evidence establishes               | High       | Publish the feature index and keep claims case-specific                                                                                                                                                  |
| RTI toolchain access is license-controlled                                                | Regenerating reference output requires access to the exact RTI toolchain            | Medium     | Retain generated reference sources, original notices, tool versions, hashes, and reproduction commands                                                                                                   |
| Qualification depends on licensed exact-version RTI environments                          | Mapped package versions can be mistaken for tested compatibility                    | Medium     | Keep profile mapping separate from exact-version qualification; retain matching package, container, and generator evidence for each LTS                                                                  |
| The `wchar` union peer uses a manually constructed C++ `DynamicType`                      | Its member IDs or labels could drift from the corpus IDL                            | Low        | Keep the definition adjacent to its non-default fixture                                                                                                                                                  |
| Direct sequence arrays flatten by default; Idl evidence covers only the retained fixtures | The retained C# mappings differ from RTI C++ arrays, preventing cross-peer matching | Medium     | Keep default C018 evidence and dedicated C149 Idl-case results: 16 / 16 Wireloom/C++ exchanges pass on each exact LTS profile. Recursive direct sequence arrays are rejected in that mode                |
| Present optional wide-string sequences expose an RTI C++ typed-peer limitation            | The typed serializer fails for present wide-string sequences                        | Medium     | The three-peer matrix uses DynamicData and RTI C API setters for those fixtures; absent, empty, and narrow-only controls remain typed. Revisit the typed serializer limitation with a compatible RTI fix |
| Interactive IDE behavior is not fully verified                                            | Developer experience may differ outside the tested build path                       | Medium     | Keep interactive IDE claims limited to verified behavior                                                                                                                                                 |
| Stricter identifier validation changes acceptance and warnings                            | Reserved names or warnings-as-errors policies can break existing consumer builds    | Medium     | Rename reserved identifiers or exempt `DDSG0108` in default mode; document the supported rules and RTI differences in the [configuration guide](../../guides/configuration.md#enable-strict-validation)  |
| RTI 7.3.1 emits invalid C# RPC transport code for service-interface cases C157/C159       | Successful generation can be mistaken for usable RPC bindings                       | Medium     | Keep the failed raw oracle and compile outcome visible; track RPC generation separately in [#75](https://github.com/eNeRGy164/wireloom/issues/75)                                                        |

## 11.2 Technical debt

| Item                                                      | Impact                                                               | Priority | Notes                                                                                                                                                                                                                               |
| :-------------------------------------------------------- | :------------------------------------------------------------------- | :------- | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| All inputs currently form one collected incremental batch | Unrelated roots may invalidate together and caching is less granular | Medium   | The [incremental generator baseline](../../performance/incremental-generator-baseline.md) confirms current reuse and shared source-output invalidation; introduce per-root graph caching only when further measurement justifies it |

A fixed preprocessing ceiling can reject unusually generated but finite IDL.
The exact limits are therefore published and should only be raised with
adversarial evidence that preserves deterministic build safety.

The incremental baseline is measurement evidence rather than a caching design.
It records current tracked-step reuse, elapsed timings, and the fact that a
changed root or include still invalidates the shared source-output batch.

The current deterministic limits are:

- 4,194,304 UTF-16 code units of preprocessed output per input.
- 100,000 macro expansion operations per input.
- 64 nested macro expansion levels.
- 256 nested conditional-expression levels.
- 128 include nesting levels.
- 100,000 source-origin alignment work units per expanded line.

Recursive fixed-array evidence covers self-referencing typed trees.
Default TypeObject v2 registration rejects recursive types. Unadapted
RTI C# array-plugin initialization can dereference its singleton before assignment
when entered through `Support.Instance`; Wireloom uses its recursive accessor
and verifies cold startup without that workaround. Generated
C++ array TypeCode initialization omits the visited-node chain, causing unbounded
metadata recursion. The wire harness records scoped adaptations for those
limitations. General mutual recursion and recursive collection typedef runtime
behavior remain unverified.
