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
The default suite uses RTI 7.7.0; the matching 7.3.1 package and lock file are
available for profile checks. Package restores are isolated by RTI version.
For the 7.3.1 run, use `-p:RtiConnextDdsVersion=7.3.1`; its lock file is
`packages.7.3.1.lock.json`. Both suites use the matching RTI native libraries
from NuGet. They create no domain participants and require no DDS peers,
discovery, or RTI generator installation. A native-loading failure is a test
failure, not a skip.
The Quality workflow executes all eight combinations of Windows/Ubuntu,
RTI 7.3.1/7.7.0, and `net8.0`/`net10.0`, together with fresh-package tests.
Preview and release workflows retain their default-target validation.

To select a consumer target without retargeting the referenced analyzer, pass
`-p:WireloomConsumerTargetFramework=net8.0` on both restore and run. The .NET 8
runtime must be installed alongside the repository SDK. For example:

```powershell
dotnet restore $project --locked-mode -p:RtiConnextDdsVersion=7.3.1 -p:WireloomConsumerTargetFramework=net8.0
dotnet run --project $project --no-restore --configuration Release -p:RtiConnextDdsVersion=7.3.1 -p:WireloomConsumerTargetFramework=net8.0
```

The `net8.0` restores use `packages.<RTI-version>.net8.0.lock.json`. Default
`net10.0` restores keep the existing lock files. The analyzer continues to
target `netstandard2.0`.

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

This summary describes generated API behavior exercised by the suite. It is
not a generated-code coverage metric or a DDS wire-compatibility claim.

| Behavior area                  | Contracts exercised                                                                                                                                   |
| :----------------------------- | :---------------------------------------------------------------------------------------------------------------------------------------------------- |
| Managed values and collections | Defaults, including enum arrays with `@default_literal`, mutation, copies, equality, and hashes across primitives, aggregates, sequences, and arrays. |
| Inheritance                    | Inherited members, optional values, formatting, equality, and native conversion.                                                                      |
| Unions                         | Construction, discriminators, branch access and transitions, copies, equality, and hashes.                                                            |
| Typedefs and aliases           | Constructors, `Value`, copies, formatting, and metadata across scalar, aggregate, collection, and chained aliases.                                    |
| Native APIs                    | Initialization, including enum arrays with `@default_literal`, conversion, key filtering, and cleanup with both allocation modes.                     |
| TypeSupport                    | `Instance`, member metadata, bounds, dimensions, enums, aliases, and metadata visibility.                                                             |
| Recursive startup              | Fresh-process access/discovery, recursive-array serialization, sequence support initialization, and public `DynamicType` rejection.                   |

Boundaries and interpretation:

- Null copies retain member initializers; constructor-only IDL defaults do not
  run. Getter-only sequences are mutated through their containers.
- RTI rejects 64-bit union discriminators before code generation. Unbounded
  typedefs use the effective limits of 100 sequence elements and 255 string
  bytes/code units.
- `allocateMemory=false` requires existing string and array backing storage.
  Array hash checks require valid fixed-size arrays and assert equal-value
  consistency, not particular values or stronger hash distribution.
- Enum values use generated C# enum members and native integer storage; they
  have no enum-specific managed constructors or equality methods. Enum-bearing
  samples, aliases, metadata, and support cover the applicable APIs. C148
  verifies that managed and native enum arrays use the declared
  `@default_literal`.

<details>
<summary>Maintainer reference: test classes by behavior area</summary>

Theory parameters identify the concrete shapes, members, discriminator labels,
and allocation/key combinations reported by the tests.

### Managed values and inheritance

- `ScalarSpecs`, `ScalarEqualitySpecs`, `CollectionSpecs`, `CollectionEqualitySpecs`
- `EnumArrayDefaultSpecs`
- `InheritanceSpecs`, `AggregateNativeSpecs`

### Unions

- `UnionSpecs`, `DiscriminatorSpecs`, `NumericCharacterLabelSpecs`
- `UnionConstructionSpecs`, `UnionConstructionNativeSpecs`, `StringSequenceUnionSpecs`

### Typedefs, aliases, and formatting

- `ScalarAliasSpecs`, `StringAliasChainSpecs`, `AggregateAliasSpecs`
- `CollectionAliasSpecs`, `UnionAliasSpecs`, `ReferenceOwnershipSpecs`
- `UnboundedAliasSpecs`, `UnboundedAliasNativeSpecs`, package `UnboundedAliasSpecs`
- `AliasFormattingSpecs`, `AliasMetadataSpecs`

### Native conversion and key filtering

- `AggregateNativeSpecs`, `ScalarAliasNativeSpecs`, `StringAliasChainNativeSpecs`
- `AggregateAliasNativeSpecs`, `CollectionAliasNativeSpecs`, `CollectionAliasMemberSpecs`, `StringArrayAliasNativeSpecs`
- `UnionNativeSpecs`, `StringSequenceUnionSpecs`, `DiscriminatorSpecs`, `UnionAliasSpecs`
- `EnumArrayDefaultNativeSpecs`
- `AggregateNativeSpecs.FiltersKeysInEachDirection`, `NestedKeySpecs`, and alias/union native theories

### TypeSupport and recursive startup

- `MetadataSpecs`, `AliasMetadataSpecs`, `StringAliasChainMetadataSpecs`
- `StringArrayAliasMetadataSpecs`, `RuntimeSmokeSpecs`
- `RecursiveStartupSpecs` and linked package tests

</details>

## Coverage

This suite is not instrumented for Coveralls. Its purpose is to assert runtime
contracts through executable tests, and its behavior matrix is not a generated
code coverage metric. Coveralls reports maintained generator source coverage;
generated fixture files under `obj` and test project code are excluded.

The corpus oracle fixtures and the typed runtime tests provide the compatibility
baseline. API behavior, serialization, and DDS wire interoperability remain
separate validation concerns.

## Recursive support startup

`RecursiveStartupSpecs` runs direct singleton access and normal
`TypeSupportHelper` discovery in separate processes. Each child runs only the
startup test and writes a completion marker after its assertions. It checks
self-referencing optional arrays, multidimensional arrays, and typedef chains:
serializer creation and complete absent, present, and nested round-trips must
succeed without plugin pre-initialization. A recursive-sequence control checks
support initialization. Public recursive DynamicType must still throw.
The package-integration project links these tests and fixtures to exercise the
freshly packed analyzer as well.

```powershell
dotnet run --project tests/Wireloom.Dds.Generator.RuntimeContracts -c Release -- --filter-class Wireloom.Dds.Generator.RuntimeContracts.TypeSupport.RecursiveStartupSpecs
```

On Windows and Linux with RTI Connext 7.7.0, Wireloom passes both cold-start
paths. The untouched C134 oracle from rtiddsgen 4.7.0 fails both paths with a
`NullReferenceException` inside recursive plugin metadata construction. Its
bootstrap control passes absent, present, and nested serialization checks.
Reproduce that comparison without editing retained oracle files:

```powershell
$root = (Get-Location).Path
$probe = 'tests/Wireloom.Dds.Generator.WireCompatibility/RtiOracleProbe'
dotnet run --project $probe -c Release "-p:RtiOracleProgram=$root/$probe/RecursiveArrayStartupProbe.cs" "-p:RtiOracleSourceDir=$root/docs/corpus/7.7.0/rtiddsgen/csharp/features/09-optional-recursive-array" -- instance
dotnet "$probe/bin/Release/net10.0/WireCompatibility.RtiOracleProbe.dll" discovery
dotnet "$probe/bin/Release/net10.0/WireCompatibility.RtiOracleProbe.dll" bootstrap
```

The oracle probe fails if the expected limitation changes or its bootstrap
control fails. These are startup and serialization results, distinct from the
[licensed wire matrix](../Wireloom.Dds.Generator.WireCompatibility/README.md).
The matrix uses ordinary Wireloom support discovery, retains only the RTI C#
bootstrap, and records the separate discovery and C++ TypeCode adaptations.
This improvement does not establish general mutual recursion or recursive
sequence native allocation support.
