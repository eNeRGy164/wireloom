# Generated API runtime contracts

This project verifies generated C# APIs with focused IDL fixtures compiled by
the generator during the build. Tests call the generated C#
APIs directly, including internal plugins in the same assembly. There is no
runtime Roslyn compilation, reflection-based invocation, or Python execution.

The compatibility target is **RTI C# or better**. IDL semantics and retained
RTI oracle implementations establish expectations. RTI implementation quirks are
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

RTI's `allocateMemory=false` mode requires existing backing storage for strings
and arrays. `NativeSample` allocates storage first for those cases, then invokes
initialization with the requested flags. This tests reuse of valid storage,
rather than attempting conversion through unallocated pointers. The helper
exercises optional-only destruction before full cleanup; dedicated lifecycle
tests also read required data after optional-only cleanup and verify absence.

## Coverage matrix

Names below identify test classes; theory parameters identify concrete shapes,
members, discriminator labels, and allocation/key combinations in test results.

| Generated API / shape                                               | Executable tests                                                                                                                                                                                        | Coverage / interpretation                                                                                                                                                                                                      |
| ------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Scalar defaults, value constructor, null/non-null copy, properties  | `ScalarSpecs`                                                                                                                                                                                           | Null copy retains initializers; constructor-only IDL defaults do not run                                                                                                                                                       |
| Scalar equality overloads and hash consistency                      | `ScalarSpecs`, `ScalarEqualitySpecs`                                                                                                                                                                    | Each primitive width, char/wchar, enum, string/wstring, and explicit default is changed independently                                                                                                                          |
| Aggregate/collection defaults, copies, properties, equality, hashes | `CollectionSpecs`, `CollectionEqualitySpecs`                                                                                                                                                            | Required/optional, absent/present, bounded/unbounded sequences, primitive/aggregate/string elements, vector/matrix                                                                                                             |
| Struct and valuetype inheritance                                    | `InheritanceSpecs`, `AggregateNativeSpecs`                                                                                                                                                              | Inherited data, optional members, equality, hashing, formatting, and conversion                                                                                                                                                |
| Union construction, discriminators, properties, `Get`, setters      | `UnionSpecs`, `DiscriminatorSpecs`, `NumericCharacterLabelSpecs`, `UnionConstructionSpecs`, `UnionConstructionNativeSpecs`                                                                              | Explicit, numeric/high-byte, multiple, default, inactive, and no-active-branch paths; XTypes type defaults, negative enum values, and initialized null copies                                                               |
| Union copies, equality, hashes, formatting                          | `UnionSpecs`, `DiscriminatorSpecs`                                                                                                                                                                      | Primitive, aggregate, sequence, array, default branches; independent collection copies                                                                                                                                         |
| Supported discriminator kinds                                       | `DiscriminatorSpecs`, `NumericCharacterLabelSpecs`                                                                                                                                                      | Boolean, int8/uint8, octet, short/ushort, long/ulong, literal/numeric char/wchar, enum, typedef; 64-bit discriminators are non-applicable because RTI rejects them before code generation                                          |
| Typedef constructors, `Value`, copies, equality, hashes             | `ScalarAliasSpecs`, `StringAliasChainSpecs`, `AggregateAliasSpecs`, `CollectionAliasSpecs`, `UnionAliasSpecs`, `ReferenceOwnershipSpecs`                                                                | Scalar/enum/string/wstring/aggregate/union/sequence/array/chains; value-constructor reference ownership; bounded/unbounded narrow and wide string chains; aggregate sequence copy behavior                                        |
| Unbounded typedefs                                                  | `UnboundedAliasSpecs`, `UnboundedAliasNativeSpecs`, package `UnboundedAliasSpecs`                                                                                                                       | Narrow/wide strings at 255 code units/bytes; primitive/enum/aggregate/alias/string sequences at effective bound 100; both allocation flags and metadata visibility values                                                      |
| `ToString` diagnostics                                              | `AliasFormattingSpecs`, `ScalarSpecs`, `CollectionSpecs`, `InheritanceSpecs`, `UnionSpecs`, `AliasMetadataSpecs`, `UnionAliasSpecs`                                                                     | Managed scalar and collection formatting; quoted strings through named aliases, including null elements; RTI formatting for aggregate roots; wide-string alias metadata and support                                                                                               |
| Native `Initialize`, `ToNative`, `FromNative`, both cleanup modes   | `AggregateNativeSpecs`, `ScalarAliasNativeSpecs`, `StringAliasChainNativeSpecs`, `AggregateAliasNativeSpecs`, `CollectionAliasNativeSpecs`, `StringArrayAliasNativeSpecs`, `UnionNativeSpecs`, `DiscriminatorSpecs`, `UnionAliasSpecs` | All allocation flag pairs; enum/strings/wstrings, aliases/chains, arrays/matrices including bounded narrow/wide string arrays, bounded/unbounded sequences, optional values/collections, inheritance, every union branch                       |
| Key-only behavior in each direction                                 | `AggregateNativeSpecs.FiltersKeysInEachDirection`, `NestedKeySpecs`, alias/union native theories                                                                                                        | Independent source/native/destination sentinels; nested key propagation; optional aggregate-array forwarding                                                                                                                   |
| Metadata creation and `Instance`                                    | `MetadataSpecs`, `AliasMetadataSpecs`, `StringAliasChainMetadataSpecs`, `StringArrayAliasMetadataSpecs`, `RuntimeSmokeSpecs`                                                                            | Both visibility values, member types/IDs/keys/optionals, bounds, dimensions, enum values, aliases, inheritance, multi/default/signed-char labels, and narrow/wide string-array shapes                                             |

Enums do not generate managed constructors/equality methods or native sample
representations of their own; enum-bearing samples, aliases, metadata, and
support cover the applicable APIs. Getter-only sequences are mutated through
their containers. Array hash tests require valid fixed-size arrays and assert
equal-value consistency, not particular numbers or stronger hash distribution.

## Coverage

This suite is not instrumented for Coveralls. Its purpose is to assert runtime
contracts through executable tests, and its behavior matrix is not a generated
code coverage metric. Coveralls reports maintained generator source coverage;
generated fixture files under `obj` and test project code are excluded.

The corpus oracle fixtures and the typed runtime tests provide the compatibility
baseline. API behavior, serialization, and DDS wire interoperability remain
separate validation concerns.
