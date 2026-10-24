# 11. Risks and technical debt

The main adoption risks are compatibility beyond the retained corpus, known
mapping gaps, and dependence on licensed interoperability evidence. The table
below states their impact and mitigation; reference-binding details follow it.

## 11.1 Known risks

| Risk                                                                                      | Impact                                                                                                       | Likelihood | Mitigation                                                                                                                                                                                                                                                                                                                                                                                                                             |
| :---------------------------------------------------------------------------------------- | :----------------------------------------------------------------------------------------------------------- | :--------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Coverage outside the retained corpus is unverified                                        | Consumers may assume broader RTI parity than the evidence establishes                                        | High       | Publish the feature index and keep claims case-specific                                                                                                                                                                                                                                                                                                                                                                                |
| Very large configured finite limits can demand substantial runtime memory                 | RTI may need to reserve or grow large sequence and string buffers                                             | Medium     | Keep settings within positive Int32 representation, document that large values are a consumer resource choice, and verify representative configured bounds at runtime                                                                                                                                                                                                                                                                   |
| RTI toolchain access is license-controlled                                                | Regenerating reference output requires access to the exact RTI toolchain                                     | Medium     | Retain generated reference sources, original notices, tool versions, hashes, and reproduction commands                                                                                                                                                                                                                                                                                                                                 |
| Qualification depends on licensed exact-version RTI environments                          | Mapped package versions can be mistaken for tested compatibility                                             | Medium     | Keep profile mapping separate from exact-version qualification; retain matching package, container, and generator evidence for each LTS                                                                                                                                                                                                                                                                                                |
| The `wchar` union peer uses a manually constructed C++ `DynamicType`                      | Its member IDs or labels could drift from the corpus IDL                                                     | Low        | Keep the definition adjacent to its non-default fixture                                                                                                                                                                                                                                                                                                                                                                                |
| Direct sequence arrays flatten by default; Idl evidence covers only the retained fixtures | The retained C# mappings differ from RTI C++ arrays, preventing cross-peer matching                          | Medium     | Keep default C018 evidence and dedicated C149 Idl-case results: 16 / 16 Wireloom/C++ exchanges pass on each exact LTS profile. Recursive direct sequence arrays are rejected in that mode                                                                                                                                                                                                                                              |
| Present optional wide-string sequences expose an RTI C++ typed-peer limitation            | The typed serializer fails for present wide-string sequences                                                 | Medium     | The three-peer matrix uses DynamicData and RTI C API setters for those fixtures; absent, empty, and narrow-only controls remain typed. Revisit the typed serializer limitation with a compatible RTI fix                                                                                                                                                                                                                               |
| Interactive IDE behavior is not fully verified                                            | Developer experience may differ outside the tested build path                                                | Medium     | Keep interactive IDE claims limited to verified behavior                                                                                                                                                                                                                                                                                                                                                                               |
| Stricter identifier validation changes acceptance and warnings                            | Reserved names or warnings-as-errors policies can break existing consumer builds                             | Medium     | Rename reserved identifiers or exempt `DDSG0108` in default mode; document the supported rules and RTI differences in the [configuration guide](../../guides/configuration.md#enable-strict-validation)                                                                                                                                                                                                                                |
| RTI 7.3.1 emits invalid C# RPC transport code for service-interface cases C157/C159       | Successful generation can be mistaken for usable RPC bindings                                                | Medium     | Keep the failed raw oracle and compile outcome visible; track RPC generation separately in [#75](https://github.com/eNeRGy164/wireloom/issues/75)                                                                                                                                                                                                                                                                                      |
| Recursive sequence typedefs cannot meet the RTI C++ interoperability requirement          | Generating C#-only support could create a type that ordinary RTI C++ peers cannot build or match             | Medium     | Standard RTI C++11 generation with `-unboundedSupport` emits uncompilable TypeCode code for self- and mutually recursive sequence typedefs on RTI 7.3.1 and 7.7.0. Wireloom rejects these forms before emission with a source-located diagnostic. Revisit only after RTI provides a compilable C++ path; direct recursive sequence members remain a separate shape                                                                     |
| Direct self-recursive sequence wire support requires TypeObject v1 on RTI 7.7.0           | Default RTI 7.7.0 participants cannot register recursive SCC types; mutual struct recursion is not qualified | Medium     | Bidirectional Wireloom C# / typed RTI C++ wire checks pass for the self-recursive shape on 7.3.1 and 7.7.0 with `type_object_max_serialized_length=8192` and only `SERVICE_REQUEST_CHANNEL` enabled. Default 7.7.0 TypeObject v2 fails registration with RTI's recursive-SCC diagnostic. Mutual recursive struct graphs remain unqualified. The RTI 7.3.1 C++ code generator also needs the documented module-qualification correction |

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

Recursive fixed-array evidence covers self-referencing typed trees. OMG IDL
permits forward-declared structs as sequence elements, but Wireloom's support
boundary also requires a usable peer implementation. Standard RTI C++11
`rtiddsgen -unboundedSupport` output for self- and mutually recursive sequence
typedefs does not compile on RTI 7.3.1 or 7.7.0, so Wireloom rejects those forms
before emission. This is an interoperability limitation, not a claim that the
IDL forms are illegal.

Direct self-recursive sequence members pass bidirectional typed Wireloom C# /
RTI C++ wire checks on both LTS versions with TypeObject v1. RTI 7.7.0's default
TypeObject v2 profile rejects recursive strongly connected components at
registration. RTI 7.3.1 generated C++ needs the documented module-qualification
correction. The [direct-recursion matrix fixture](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#direct-recursive-sequence-matrix-fixture)
records the tested peer setup and exchange. Mutual recursive struct graphs and
recursive array TypeCode initialization remain unqualified. See RTI's
[7.7.0 migration guidance](https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/migration_guide/770/general770.html#recursive-type-support)
and [Extensible Types overview](https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/extensible_types_guide/extensible_types/XTypes_Intro.htm).

Unadapted RTI C# array-plugin initialization can dereference its singleton before
assignment when entered through `Support.Instance`; Wireloom uses its recursive
accessor and verifies cold startup without that workaround. Generated C++ array
TypeCode initialization omits the visited-node chain, causing unbounded metadata
recursion. The wire harness records scoped adaptations for those limitations.
Mutual recursive sequence typedefs are rejected by Wireloom. Direct recursive
sequence serializer and typed Wireloom C# / RTI C++ wire exchanges pass for
the direct self-recursive shape on both profiles with the documented
TypeObject v1 participant configuration. Mutual recursive struct graphs and
recursive array TypeCode initialization remain unqualified.
