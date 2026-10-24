# Wire-test workarounds

This guide records generated-binding corrections, runtime settings, and reference-model
deviations. Ordinary typed fixtures and topic wrappers are
[harness setup](README.md#case-specific-fixture-construction), not compatibility workarounds.
Version lists the releases with the described limitation or configuration requirement.
Snippets use the corpus type names; linked sources contain the complete implementation.
Raw captured RTI oracle sources are not edited; corrections live in versioned
`wire-adjusted` copies with per-file hashes.

- [W001 · Optional wide-string sequences](#w001)
- [W002 · Shadowed C# member](#w002)
- [W003 · Numeric character casts](#w003)
- [W004 · Character fallback discriminator](#w004)
- [W005 · Character label metadata](#w005)
- [W006 · Unsigned character compilation](#w006)
- [W007 · Recursive metadata propagation](#w007)
- [W008 · Recursive C# plugin bootstrap](#w008)
- [W009 · Recursive array TypeCode traversal](#w009)
- [W010 · Recursive writer memory](#w010)
- [W011 · Recursive type generation](#w011)
- [W012 · Recursive type-name qualification](#w012)
- [W013 · Wide-character union reference](#w013)
- [W014 · FlatData reference input](#w014)
- [W015 · Wstring alias peer corrections](#w015)
- [W016 · Unsigned 32-bit union labels](#w016)
- [W017 · ASCII character label capture](#w017)
- [W018 · Generated include staging](#w018)

<a id="optional-wide-string-sequences"></a>
<a id="w001"></a>

## W001 · Optional wide-string sequences

**Scope:** C114 `09-optional-string-sequences`  
**Version:** `7.3.1`, `7.7.0`  
**Peer:** RTI C++ reference

**Applies when:** `optional-wide-only` and `optional-multiple`; other fixtures retain the typed path.

The typed serializer path for a present optional
wide-string sequence crashes on both LTS releases:

```cpp
writer.write(expected); // generated typed sample containing wideValues
// Typed serialization crashes before completing the write.
```

For these fixtures the harness instead uses `DynamicData` endpoints and the
C API's per-element wide-string setter:

```cpp
auto wide = expected.loan_value("wideValues");
std::vector<DDS_Wchar> value{L'w', L'i', L'd', L'e', 0};
DDS_DynamicData_set_wstring(&wide.get().native(), nullptr, 1, value.data());
wide.return_loan();
```

The implementation checks the return code and sets each element. It constructs
16-bit `DDS_Wchar` values explicitly; Linux `wchar_t` is 32-bit. See [native peer](Native/main.cpp.in).

<a id="shadowed-c-member"></a>
<a id="w002"></a>

## W002 · Shadowed C# member

**Scope:** C021 `06-alias-composition`  
**Version:** `7.3.1`, `7.7.0`  
**Peer:** RTI C# reference

**Applies when:** Compiling generated native conversion methods for a member named `sample`.

The generated conversion parameter shadows a member also named `sample`,
producing invalid conversion calls:

```csharp
sample.FromNative(sample.sample, keysOnly: false); // CS1061: receiver is the managed sample.
sample.ToNative(sample.sample, keysOnly: false); // CS1061: receiver is the managed sample.
```

Qualify the member receiver in scratch generated code:

```csharp
this.sample.FromNative(sample.sample, keysOnly: false);
this.sample.ToNative(sample.sample, keysOnly: false);
```

The patch changes only those receivers. See [reference build](scripts/run-matrix.sh).

<a id="numeric-character-unions"></a>
<a id="w003"></a>

## W003 · Numeric character casts

**Scope:** C137 `07-union-char-numeric`; C141 `07-union-char-numeric-255`  
**Version:** `7.3.1`, `7.7.0`  
**Peer:** RTI C# reference

**Applies when:** Compiling numeric `char` discriminator assignments and labels.

Missing C# casts produce `CS0266`. Cast both the discriminator assignment
and corresponding switch label:

```csharp
// Before
Discriminator = 255; // CS0266: int cannot be assigned to char implicitly.
case 255: // CS0266: switch label must be char.
// After
Discriminator = (char)255;
case (char)255:
```

Apply the guarded C# correction after generation and before compilation:

```sh
dotnet matrix-tools.dll adapt-rti-characters csharp 07-union-char-numeric <generated-csharp-dir>
dotnet matrix-tools.dll adapt-rti-characters csharp 07-union-char-numeric-255 <generated-csharp-dir>
```

The command also applies the separate [fallback discriminator](#character-fallback-discriminator)
and [label metadata](#character-label-metadata) corrections. Unexpected generated
source stops the build. See [maintainer commands](README.md#maintainer-checks)
and [patch utility](scripts/matrix-tools.cs).

<a id="character-fallback-discriminator"></a>
<a id="w004"></a>

## W004 · Character fallback discriminator

**Scope:** C137 `07-union-char-numeric`; C141 `07-union-char-numeric-255`  
**Version:** `7.3.1`, `7.7.0`  
**Peer:** RTI C# reference

**Applies when:** Deserializing the `union-default` fixture with discriminator `65` and payload `13703`.

The generated fallback setter replaces the received discriminator with its
constructor default during deserialization:

```csharp
// Before
sample.fallback = fallback;
// Discriminator becomes 0, losing the received default-branch value 65.
// After
sample.Setfallback(fallback, NativeChar.FromUtf8(_d));
// Discriminator remains 65 when the received _d is 65.
```

Use the generated setter that accepts the incoming discriminator. The public
fallback property remains available for normal application assignments. The
[C# character patch command](#numeric-character-unions) applies this correction.
See [patch utility](scripts/matrix-tools.cs).

<a id="character-label-metadata"></a>
<a id="w005"></a>

## W005 · Character label metadata

**Scope:** C141 `07-union-char-numeric-255`  
**Version:** `7.3.1`, `7.7.0`  
**Peer:** RTI C# and RTI C++ references

**Applies when:** Representing label `255` in generated internal union metadata.

The internal RTI label metadata needs the signed-byte representation of
`255`. Without normalization, the selected payload is lost and cross-peer
type matching fails:

```csharp
// C# UnionMember label: before -> after
new int[] {(int) 255}
new int[] { -1 }
```

```cpp
// C++ TypeCode label: before -> after
static_cast<int>(255)
static_cast<int>(-1)
```

Public discriminator `255`, wire byte `0xFF`, and payload `13702` remain unchanged.
The [C# character patch command](#numeric-character-unions) normalizes C# metadata;
apply the C++ metadata correction separately:

```sh
dotnet matrix-tools.dll adapt-rti-characters cpp 07-union-char-numeric-255 <generated-cpp-dir>
```

The C++ binding also needs the independent [unsigned-char compiler setting](#unsigned-character-compilation).
See [patch utility](scripts/matrix-tools.cs).

<a id="unsigned-character-compilation"></a>
<a id="w006"></a>

## W006 · Unsigned character compilation

**Scope:** C141 `07-union-char-numeric-255`  
**Version:** `7.3.1`, `7.7.0`  
**Peer:** RTI C++ generated binding and consuming code on Linux

**Applies when:** Building high-label `char` union branch guards.

With Linux's default signed `char`, the discriminator byte for label `255`
becomes `-1`; generated C++ branch guards compare it with `255` and reject it:

```cpp
// Default signed-char compilation on Linux
std::cout << static_cast<int>(static_cast<char>(255)); // -1, not 255.
```

Compile both the generated binding and its consuming code with `-funsigned-char`.
The harness puts all translation units in the same CMake target:

```cmake
target_compile_options(wire-peer PRIVATE -funsigned-char)
```

The same C++ expression then yields `255`. This compiler setting does not correct
[internal label metadata](#character-label-metadata). See [native build](Native/CMakeLists.txt).

<a id="recursive-metadata-propagation"></a>
<a id="w007"></a>

## W007 · Recursive metadata propagation

**Scope:** C134 `09-optional-recursive-array`  
**Version:** `7.7.0`  
**Peer:** Wireloom, RTI C#, and RTI C++ participants

**Applies when:** Registering the recursive array type for discovery.

Default TypeObject v2 registration fails for the recursive array type on
RTI `7.7.0`. Disable TypeObject and TypeCode propagation before participant
creation:

```csharp
participantQos = participantQos.WithResourceLimits(policy =>
{
    policy.TypeObjectMaxSerializedLength = 0;
    policy.TypeCodeMaxSerializedLength = 0;
});
```

The C++ participant sets the same limits. The harness applies these limits
on both LTS profiles. Default TypeObject v2 interoperability remains unqualified;
this discovery setting does not repair [reference initialization](#recursive-csharp-bootstrap)
or [recursive C++ traversal](#recursive-array-typecode-traversal). See
[managed peer](Peer/TypedPeer.cs) and [native peer](Native/typed-fixture-peer.hpp.in).

<a id="recursive-csharp-bootstrap"></a>
<a id="w008"></a>

## W008 · Recursive C# plugin bootstrap

**Scope:** C134 `09-optional-recursive-array`  
**Version:** `7.3.1`, `7.7.0`  
**Peer:** RTI C# reference

**Applies when:** Initializing generated recursive-array TypeSupport.

Generated recursive array initialization accesses `NodeSupport.Instance` before
its static initialization assigns it:

```csharp
// Before
var support = NodeSupport.Instance;
// Recursive array initialization reaches Instance while it is still null.
// After
NodePlugin.CreateDynamicType(false);
var support = NodeSupport.Instance;
```

Initialize the reference plugin first. The generic runner invokes this method
through reflection; Wireloom requires no plugin bootstrap. This initialization
correction does not enable [public recursive metadata](README.md#recursive-fixture-metadata).
See [typed peer setup](Peer/TypedPeer.cs).

<a id="recursive-array-typecode-traversal"></a>
<a id="w009"></a>

## W009 · Recursive array TypeCode traversal

**Scope:** C134 `09-optional-recursive-array`  
**Version:** `7.3.1`, `7.7.0`  
**Peer:** RTI C++ reference

**Applies when:** Initializing the recursive array element TypeCode.

Generated C++ array TypeCode initialization omits the visited-node chain,
causing unbounded metadata recursion. Forward that chain in the array-element
lookup:

```cpp
// Before
rti::topic::dynamic_type< ::CorpusOptionalRecursiveArray::Node>::get().native()
// After
rti::topic::dynamic_type< ::CorpusOptionalRecursiveArray::Node>::get(&node).native()
```

Patch scratch generated code before compilation. RTI `7.3.1` also needs the
independent [type-name qualification](#recursive-aggregate-qualification)
correction. Generation requires [recursive-type support](#recursive-type-generation).
See [scratch C++ corrections](scripts/run-matrix.sh).

<a id="recursive-writer-memory"></a>
<a id="w010"></a>

## W010 · Recursive writer memory

**Scope:** C134 `09-optional-recursive-array`  
**Version:** `7.3.1`  
**Peer:** Wireloom, RTI C#, and RTI C++ writers

**Applies when:** Creating a writer for any recursive-array fixture, before writing the finite sample.

Creating the writer with default memory settings makes RTI 7.3.1 preallocate
about `64 GiB` from the recursive maximum sample size. The writer cannot attach
when that allocation fails:

```csharp
using var writer = participant.ImplicitPublisher.CreateDataWriter(topic, writerQos);
// With the default pool settings: roughly 32 buffers of 2 GiB on RTI 7.3.1.
// Allocation fails before any fixture sample is written.
```

Set a fast-pool threshold before creating the writer:

```csharp
writerQos = writerQos.WithProperty(properties => properties.Add(
    "dds.data_writer.history.memory_manager.fast_pool.pool_buffer_max_size",
    new Property.Entry("4096", false)));
```

The C++ writer sets the same property. This threshold limits fast-pool buffer
allocation; larger samples can allocate separately. It is not a serialized
sample-size limit. See
[managed writer](Peer/PeerTransport.cs) and [native writer](Native/typed-fixture-peer.hpp.in).

The harness applies this writer setting on both LTS profiles.

<a id="recursive-type-generation"></a>
<a id="w011"></a>

## W011 · Recursive type generation

**Scope:** C019 `06-aggregates`; C134 `09-optional-recursive-array`  
**Version:** `7.3.1`, `7.7.0`  
**Peer:** RTI C++ reference

**Applies when:** Generating C++ bindings for recursive types.

Recursive C++ generation requires the experimental `-unboundedSupport` option:

```sh
# Without recursive-type support
rtiddsgen -language C++11 -d generated docs/corpus/idl/features/06-aggregates.idl
# The recursive declaration is rejected.

# Enable recursive-type support
rtiddsgen -language C++11 -unboundedSupport -d generated docs/corpus/idl/features/06-aggregates.idl
```

The harness passes this option through each case's `rtiCppCodegenOptions`.
Generation with this flag does not correct generated TypeCode references;
RTI `7.3.1` also needs [type-name qualification](#recursive-aggregate-qualification).
See [reference build](scripts/run-matrix.sh).

<a id="recursive-aggregate-qualification"></a>
<a id="w012"></a>

## W012 · Recursive type-name qualification

**Scope:** C019 `06-aggregates`; C134 `09-optional-recursive-array`  
**Version:** `7.3.1`  
**Peer:** RTI C++ reference

**Applies when:** Compiling recursive TypeCode references generated with [recursive-type support](#recursive-type-generation).

RTI `7.3.1` emits recursive TypeCode references without their IDL module
qualification. The same correction applies to both affected types:

```cpp
// C019: before -> after
rti::topic::dynamic_type< Recursive>
rti::topic::dynamic_type< ::CorpusAggregates::Recursive>

// C134: before -> after
rti::topic::dynamic_type< Node>
rti::topic::dynamic_type< ::CorpusOptionalRecursiveArray::Node>
```

Unqualified names fail C++ compilation. Qualify them in scratch generated output.
This does not replace the [visited-node traversal correction](#recursive-array-typecode-traversal)
for C134. See [reference build](scripts/run-matrix.sh).

<a id="wide-character-union-reference"></a>
<a id="w013"></a>

## W013 · Wide-character union reference

**Scope:** C113 `07-union-wchar-label`  
**Version:** `7.3.1`, `7.7.0`  
**Peer:** RTI C++ manual reference; RTI C# reference remains unavailable

**Applies when:** Exchanging the `wchar` union through a manually constructed DynamicType.

`rtiddsgen` rejects `union Choice switch (wchar)`, so there is no generated RTI
C# reference:

```idl
union Choice switch(wchar) {
    case L'a': long letter;
    default: boolean other;
};
// Generation fails: "no viable alternative at input 'wchar' in union".
```

The C++ reference manually builds a matching DynamicType:

```cpp
dds::core::xtypes::UnionType dynamic_type(
    "CorpusWideCharacterUnion::Choice",
    dds::core::xtypes::primitive_type<wchar_t>(),
    {
        dds::core::xtypes::UnionMember(
            "letter", dds::core::xtypes::primitive_type<int32_t>(), 97).id(1),
        dds::core::xtypes::UnionMember(
            "text", dds::core::xtypes::StringType(255), 122).id(2),
        dds::core::xtypes::UnionMember(
            "other", dds::core::xtypes::primitive_type<bool>(),
            dds::core::xtypes::UnionMember::DEFAULT_LABEL).id(3)
    });
dynamic_type.extensibility_kind(dds::core::xtypes::ExtensibilityKind::EXTENSIBLE);
```

This covers the manual C++ model only; it does not make the RTI C# generation
failures pass. See [native type model](Native/main.cpp.in).

<a id="flatdata-reference-input"></a>
<a id="w014"></a>

## W014 · FlatData reference input

**Scope:** C047 `10-flat-data-binding`  
**Version:** `7.3.1`, `7.7.0`  
**Peer:** RTI C++ reference

**Applies when:** Generating ordinary C++ bindings from the shared IDL containing `@language_binding(FLAT_DATA)`.

The canonical IDL selects FlatData but leaves `Sample` with default extensibility.
RTI requires FlatData types to be `@final` or `@mutable`:

```idl
@language_binding(FLAT_DATA)
@data_representation(XCDR2)
module CorpusFlatDataBinding {
    @topic struct Sample { long id; long value; };
};
// Generation rejects Sample: FlatData requires @mutable or @final.
```

[FlatData is a C++ binding](https://community.rti.com/static/documentation/connext-dds/7.4.0/doc/manuals/connext_dds_professional/users_manual/users_manual/Using_BuiltinAnnotations.htm);
RTI C# ignores the annotation.
For the reference exchange, remove the mapping annotation from scratch C++ input:

```sh
sed '/@language_binding(FLAT_DATA)/d' "$idl_path" > "$native_dir/idl.idl"
```

This preserves the canonical extensibility and XCDR2 sample shape while selecting
ordinary C++ mapping. It is a reference-model deviation, not a FlatData fix.
The canonical IDL and C# inputs are unchanged; FlatData C++ layout is unqualified.
See [IDL adaptation](scripts/run-matrix.sh).

<a id="wstring-alias-typecode-tags"></a>
<a id="w015"></a>

## W015 · Wstring alias peer corrections

**Scope:** C147 `10-resolve-name-wstring-aliases`<br>
**Version:** `7.3.1`, `7.7.0`<br>
**Peer:** RTI C# and RTI C++ references

**Applies when:** Compiling RTI C# plugin code or C++ TypeCode metadata for structure members that use the `@resolve_name(false)` wstring aliases.

RTI C++ generation succeeds, but both releases emit `Sample` TypeCode
references using the C++ aliases directly:

```cpp
rti::topic::dynamic_type< BoundedWideText>::get()
rti::topic::dynamic_type< BoundedWideTextAlias>::get()
```

Both aliases resolve to `std::wstring`, which has no generated RTI
`dynamic_type` specialization. The generated header declares a distinct alias
tag for each IDL typedef. Correct the scratch `idl.cxx` references to use those
module-qualified tags:

```cpp
rti::topic::dynamic_type< ::CorpusResolveNameWstringAliases::BoundedWideText_AliasTag_t>::get()
rti::topic::dynamic_type< ::CorpusResolveNameWstringAliases::BoundedWideTextAlias_AliasTag_t>::get()
```

The RTI C# generator has a related namespace error in `SampleUnmanaged`:

```csharp
private global::Implementation.BoundedWideTextUnmanaged firstLevel;
private global::Implementation.BoundedWideTextAliasUnmanaged secondLevel;
```

The types are emitted under
`global::CorpusResolveNameWstringAliases.Implementation`. The wire harness
checks that each bad reference occurs exactly once, then applies the module
qualification to its scratch C# source before compiling the reference peer.
The retained RTI oracle source stays unchanged, so the profile corpus continues
to record that the unmodified C# output fails compilation with `CS0400` and
`CS8377`.

With both guarded corrections, the C147 matrix exercises all nine directed
pairings among Wireloom C#, RTI C#, and RTI C++ for empty and exact-boundary
values. See [the guarded edits](scripts/run-matrix.sh) and [C147's corpus
row](../../../docs/corpus/FEATURE-COVERAGE.md).

<a id="unsigned-32-bit-union-labels"></a>
<a id="w016"></a>

## W016 · Unsigned 32-bit union labels

**Scope:** C119 `07-union-ulong`; C125 `07-union-uint32`; **Version:** `7.3.1`, `7.7.0`
**Peer:** RTI C# reference

**Applies when:** Compiling the generated RTI C# plugin metadata for the
maximum discriminator label `4294967295`.

RTI emits an explicit cast of that unsigned 32-bit label to `int` in the
`UnionMember` metadata. The checked constant conversion fails with `CS0221`:

```csharp
new int[] { (int) 4294967295 } // CS0221
```

The adjusted oracle uses an unchecked conversion:

```csharp
new int[] { unchecked((int) 4294967295) } // -1, the same 0xFFFFFFFF bits
```

This preserves the discriminator's 32-bit representation in RTI's signed
metadata API. The raw generated C# stays under `csharp/features`; the adjusted
peer and its hashes are recorded under `csharp/wire-adjusted` in
`wire-oracles.json`. See the [adjusted source manifests](../../../docs/corpus/7.3.1/rtiddsgen/csharp/wire-adjusted/07-union-uint32/manifest.json).

<a id="ascii-character-label-capture"></a>
<a id="w017"></a>

## W017 · ASCII character label capture

**Scope:** C130 `07-union-char-boundaries`; **Version:** `7.3.1`, `7.7.0`
**Peer:** RTI C# reference

**Applies when:** The captured RTI C# source represents the maximum label for
the IDL `char` discriminator.

The IDL declares ASCII tilde (`~`, U+007E), but the captured RTI C# output uses
U+00FF (`ÿ`) in its branch checks and type metadata. The adjusted oracle changes
those literals to `~`, matching the declared label and the 27 passing pairings
in the latest wire CI run. The original captures remain unchanged in
`csharp/features`; the corrected sources and hashes are in
`csharp/wire-adjusted/07-union-char-boundaries`.

<a id="w018"></a>

## W018 · Generated include staging

**Scope:** C164 `14-autoid-scope`; C172 `14-enum-constant`<br>
**Version:** `7.3.1`, `7.7.0`<br>
**Peer:** RTI C# and RTI C++ references

**Applies when:** Compiling generated peers whose IDL includes resolve by
basename or relative path, or whose generated helper files are shared by
multiple IDL inputs.

The matrix stages include dependencies in an isolated scratch tree. It reuses
generated files only after checking expected names, unique destinations, and
byte identity; otherwise it copies matching headers to IDL-relative paths.
This works around generator include-resolution behavior while retaining the
original corpus inputs and recording source and staged hashes.
