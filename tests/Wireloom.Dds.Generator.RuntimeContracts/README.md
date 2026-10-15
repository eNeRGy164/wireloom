# Generated API runtime contracts

This project implements the detection layer for
[#39](https://github.com/eNeRGy164/wireloom/issues/39). Small IDL fixtures are
compiled by the current generator during the build. Tests call generated C#
APIs directly, including internal plugins in the same assembly. There is no
runtime Roslyn compilation, reflection-based invocation, or Python execution.

The goal is **RTI C# or better**. Existing documentation, IDL semantics, and
retained RTI evidence establish expectations. RTI implementation quirks are
characterization evidence, not an automatic ceiling on the API. Improvements
beyond RTI C# can use RTI C++ support as evidence, but interoperability claims
still require independent wire verification. API execution, serialization, and
DDS wire compatibility remain distinct evidence categories.

## Run

From the repository root, using the pinned SDK:

```powershell
$project = 'tests/Wireloom.Dds.Generator.RuntimeContracts/Wireloom.Dds.Generator.RuntimeContracts.csproj'
dotnet restore $project --locked-mode
dotnet run --project $project --no-restore --configuration Release
```

Use Microsoft.Testing.Platform filters after `--`, for example
`-- --filter-namespace Wireloom.Dds.Generator.RuntimeContracts.Native`.
The suite uses RTI 7.7.0 native libraries supplied by the existing NuGet package.
It creates no domain participants and requires no DDS peers, discovery, or RTI
generator installation. A native-loading failure is a test failure, not a skip.
The quality workflow executes the suite on Windows and Ubuntu; preview and
release workflows run it before packaging for integration validation.

## Ownership and organization

- `Fixtures`: authored IDL and small typed sample factories. Each module owns
  its own generated types, independently of corpus snapshots.
- `Managed`: defaults, constructors, property mutation, equality, hashing,
  formatting, and struct/valuetype inheritance.
- `Unions`: selection, invalid access, transitions, copies, and supported
  discriminator kinds.
- `Aliases`: scalar, enum, string, aggregate, union, collection, and chain APIs.
- `Native`: allocation, conversions, key filtering, and destruction. The generic
  helper owns its native struct locally; native storage is never passed by
  value to cleanup helpers. Every allocation has `try/finally` full cleanup.
- `TypeSupport`: observable RTI metadata and support initialization.
- `KnownFailures`: isolated compile inputs and explicitly skipped desired
  contracts. These gaps prevent a claim that #39 is complete.

RTI's `allocateMemory=false` mode requires existing backing storage for strings
and arrays. `NativeSample` allocates storage first for those cases, then invokes
initialization with the requested flags. This tests reuse of valid storage,
rather than attempting conversion through unallocated pointers. The helper
exercises optional-only destruction before full cleanup; dedicated lifecycle
tests also read required data after optional-only cleanup and verify absence.

## Coverage matrix

Names below identify test classes; theory parameters identify concrete shapes,
members, discriminator labels, and allocation/key combinations in test results.

| Generated API / shape                                               | Executable tests                                                                                                                                                         | Gaps / interpretation                                                                                                                                                                                                      |
| ------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Scalar defaults, value constructor, null/non-null copy, properties  | `ScalarSpecs`                                                                                                                                                            | Null copy retains initializers; constructor-only IDL defaults do not run                                                                                                                                                   |
| Scalar equality overloads and hash consistency                      | `ScalarSpecs`, `ScalarEqualitySpecs`                                                                                                                                     | Each primitive width, char/wchar, enum, string/wstring, and explicit default is changed independently                                                                                                                      |
| Aggregate/collection defaults, copies, properties, equality, hashes | `CollectionSpecs`, `CollectionEqualitySpecs`                                                                                                                             | Required/optional, absent/present, bounded/unbounded sequences, primitive/aggregate/string elements, vector/matrix                                                                                                         |
| Struct and valuetype inheritance                                    | `InheritanceSpecs`, `AggregateNativeSpecs`                                                                                                                               | Inherited data, optional members, equality, hashing, formatting, and conversion                                                                                                                                            |
| Union construction, discriminators, properties, `Get`, setters      | `UnionSpecs`, `DiscriminatorSpecs`, `UnionConstructionSpecs`, `UnionConstructionNativeSpecs`                                                                             | Explicit, multiple, default, inactive, and no-active-branch paths; XTypes type defaults, negative enum values, and initialized null copies (F4/F9/F10 resolved)                                                            |
| Union copies, equality, hashes, formatting                          | `UnionSpecs`, `DiscriminatorSpecs`                                                                                                                                       | Primitive, aggregate, sequence, array, default branches; independent collection copies                                                                                                                                     |
| Supported discriminator kinds                                       | `DiscriminatorSpecs`                                                                                                                                                     | Boolean, int8/uint8, octet, short/ushort, long/ulong, char/wchar, enum, typedef; 64-bit discriminators are unsupported corpus cases, not runtime fixtures                                                                  |
| Typedef constructors, `Value`, copies, equality, hashes             | `ScalarAliasSpecs`, `AggregateAliasSpecs`, `CollectionAliasSpecs`, `UnionAliasSpecs`, `ReferenceOwnershipSpecs`                                                          | Scalar/enum/string/wstring/aggregate/union/sequence/array/chains; value-constructor reference ownership; unbounded typedef regression coverage in `UnboundedAliasSpecs` (F1 resolved); aggregate sequence copy decision F5 |
| Unbounded typedefs                                                  | `UnboundedAliasSpecs`, `UnboundedAliasNativeSpecs`, package `UnboundedAliasSpecs`                                                                                        | F1 resolved; narrow/wide strings at 255 code units/bytes; primitive/enum/aggregate/alias/string sequences at effective bound 100; both allocation flags and metadata visibility values                                     |
| `ToString` diagnostics                                              | `AliasFormattingSpecs`, `ScalarSpecs`, `CollectionSpecs`, `InheritanceSpecs`, `UnionSpecs`, `AliasMetadataSpecs`, `UnionAliasSpecs`                                      | F2 uses managed scalar/collection formatting and retains RTI aggregate formatting; wide-string alias metadata and support now pass                                                                                         |
| Native `Initialize`, `ToNative`, `FromNative`, both cleanup modes   | `AggregateNativeSpecs`, `ScalarAliasNativeSpecs`, `AggregateAliasNativeSpecs`, `CollectionAliasNativeSpecs`, `UnionNativeSpecs`, `DiscriminatorSpecs`, `UnionAliasSpecs` | All allocation flag pairs; enum/strings/wstrings, aliases/chains, arrays/matrices, bounded/unbounded sequences, optional values/collections, inheritance, every union branch                                               |
| Key-only behavior in each direction                                 | `AggregateNativeSpecs.FiltersKeysInEachDirection`, `NestedKeySpecs`, alias/union native theories                                                                         | Independent source/native/destination sentinels; nested key propagation; optional aggregate-array forwarding                                                                                                               |
| Metadata creation and `Instance`                                    | `MetadataSpecs`, `AliasMetadataSpecs`, `RuntimeSmokeSpecs`                                                                                                               | Both visibility values, member types/IDs/keys/optionals, bounds, dimensions, enum values, aliases, inheritance, multi/default/signed-char labels; wide-string alias metadata is covered by F3 regression tests             |

Enums do not generate managed constructors/equality methods or native sample
representations of their own; enum-bearing samples, aliases, metadata, and
support cover the applicable APIs. Getter-only sequences are mutated through
their containers. Array hash tests require valid fixed-size arrays and assert
equal-value consistency, not particular numbers or stronger hash distribution.

## Known findings

All findings remain associated with [#39](https://github.com/eNeRGy164/wireloom/issues/39).
Separate GitHub issue publication was blocked by automatic approval review;
these local findings and executable reproductions are retained for follow-up.
The initial suite characterized generator behavior. F1–F4 and F9/F10 are now resolved
and protected by regular fixtures and runtime/package regressions. F2 improves
on the observed RTI C# default formatting behavior; F3 corrects its prerequisite
wide-string support initialization.

| ID  | Finding and evidence                                                                                                                                                          | Reproduction / disposition                                                                                                                                                                                                                                                            |
| --- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| F1  | Resolved: unbounded string/wstring typedefs import `Omg.Types`; unbounded sequence typedefs share the effective 100-element bound across attributes, allocation, and metadata | Regular `Fixtures/unbounded-strings.idl` and `unbounded-sequence.idl`; `UnboundedTypedefSpecs`, `UnboundedAliasSpecs`, `UnboundedAliasNativeSpecs`, and packed-package regressions                                                                                                    |
| F2  | Resolved: managed fallbacks improve scalar/collection root formatting beyond RTI C#; RTI formatting remains for aggregates; [comparison and contract](F2-EVIDENCE.md)         | `AliasFormattingSpecs` checks all 15 original shapes plus escaping, Unicode, culture, null/empty values, nested dimensions, and retained RTI aggregate formatting; aliases keep the original RTI delegate and support classes hide the default formatter with a typed instance method |
| F3  | Resolved: wide-string aliases use `TypeKind.WideString` and `WideStringValue`, matching RTI and allowing the support instance to initialize                                   | `AliasMetadataSpecs.CreatesWideStringAliasMetadata` checks both visibility values, bounds, support initialization, and formatting                                                                                                                                                     |
| F4  | Resolved: null-source union copies initialize the same discriminator and selected value as parameterless construction; [C#/C++ comparison and repair](F4-EVIDENCE.md)         | `UnionConstructionSpecs`, `UnionConstructionNativeSpecs`, and packed-package tests cover nonzero/negative enums and independent aggregate/sequence/array branches; the F4 skip is removed                                                                                             |
| F5  | Aggregate sequence typedef copy creates a new container but shares mutable elements; the retained RTI C# oracle deep-copies elements (confirmed compatibility defect)         | Passing `ChildrenPreservesItsValueContract` characterizes current behavior; skipped `CopiesAggregateSequenceElementsIndependently` records a proposed ownership improvement, not an assumed IDL requirement                                                                           |
| F6  | Accepted numeric char/wchar labels emit integer expressions where C# requires char values, producing CS0266                                                                   | `KnownFailures/numeric-character-labels.idl`; literal-label equivalents pass at runtime; semantic/RTI acceptance requires follow-up evidence                                                                                                                                          |
| F7  | String typedef chains leave non-nullable `Value` uninitialized (CS8618 with warnings as errors); alias resolution also loses string width/bounds                              | `KnownFailures/string-alias-chains.idl`; separate follow-up discovered while checking F1 variants                                                                                                                                                                                     |
| F8  | String-array typedef native/plugin emission uses unmanaged generic APIs with string elements (CS8377)                                                                         | Isolated `KnownFailures/string-array-aliases.idl`; separate follow-up, with no compiled string-array capability claim                                                                                                                                                                 |
| F9  | Resolved: negative enum default casts parenthesize the negative operand                                                                                                       | Regular `Fixtures/union-construction.idl` compiles without adaptations; managed/native and packed-package tests exercise -7 defaults                                                                                                                                                  |
| F10 | Resolved: parameterless union discriminators use XTypes type defaults independently of default-branch setters                                                                 | `UnionConstructionSpecs`, emission-plan tests, and seven retained-oracle corpus comparisons cover zero/false defaults; default-branch setters still select unused labels                                                                                                              |

Reproduce each compile failure independently:

```powershell
dotnet build $project --no-restore --configuration Release -p:RuntimeContractKnownFailure=numeric-character-labels
dotnet build $project --no-restore --configuration Release -p:RuntimeContractKnownFailure=string-alias-chains
dotnet build $project --no-restore --configuration Release -p:RuntimeContractKnownFailure=string-array-aliases
```

Each command is expected to fail. Omit the property on the next normal build.
The known inputs are explicit generation roots only in these opt-in builds.
To investigate a skipped runtime contract, remove its `Skip` locally and run
the focused test; do not convert an unexpected exception into a passing API
contract. MTP reports a skipped theory as one skipped test rather than expanding
its individual rows.

## Generated-source coverage

Maintained-source and generated-fixture reports remain separate for inspection.
The Coveralls job waits for both runtime jobs, downloads their reports into
separate directories, and builds the fixtures so their generated sources exist
in the upload workspace. A file-based C# script normalizes source paths across
Windows and Linux and rejects missing source files. The Coveralls action then
discovers and submits the generator report and both runtime reports together.
The shared root `coverage.settings.xml` includes the generator and runtime
contract assemblies, retains generated fixture sources under `obj`, and
excludes authored runtime test code by namespace. Other test assemblies and
build intermediates remain excluded.
Microsoft's [coverage configuration](https://github.com/microsoft/codecoverage/blob/main/docs/configuration.md)
places `IncludeTestAssembly` directly under `Configuration`; omitting it can
produce an empty report because MTP excludes test assemblies by default.

```powershell
dotnet run --project $project --no-restore --configuration Release -- `
  --coverage --coverage-settings coverage.settings.xml `
  --coverage-output generated.cobertura.xml --coverage-output-format cobertura `
  --results-directory coverage/runtime
dotnet run --file tests/Wireloom.Dds.Generator.RuntimeContracts/scripts/verify-coverage.cs -- coverage/runtime/generated.cobertura.xml
```

The file-based C# verifier rejects empty reports, missing branch counters,
missing fixture families, and reports containing authored test sources. Review
branch coverage first, then line coverage and uncovered methods against the
matrix. The report includes unused fixture support APIs and known failures;
it is not a claim of full generated API coverage.

Actual commands, platform versions, pass counts, coverage counters, and remaining
validation limits are recorded in [EVIDENCE.md](EVIDENCE.md).

The retained RTI C# baseline for F1 and its F5 comparison are documented in
[unbounded typedef oracle evidence](../../docs/corpus/UNBOUNDED-ALIASES.md).
