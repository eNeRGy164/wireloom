# Wire-test workarounds

This guide records generated-binding corrections, runtime settings, and reference-model
deviations. Ordinary typed fixtures and topic wrappers are
[harness setup](README.md#case-specific-fixture-construction), not compatibility workarounds.
Version lists the releases with the described limitation or configuration requirement.
Snippets use the corpus type names; linked sources contain the complete implementation.
Retained RTI oracle sources are not edited.

- [1. Optional wide-string sequences](#optional-wide-string-sequences)
- [2. Shadowed C# member](#shadowed-c-member)
- [3. Numeric character casts](#numeric-character-unions)
- [4. Character fallback discriminator](#character-fallback-discriminator)
- [5. Character label metadata](#character-label-metadata)
- [6. Unsigned character compilation](#unsigned-character-compilation)
- [7. Recursive metadata propagation](#recursive-metadata-propagation)
- [8. Recursive C# plugin bootstrap](#recursive-csharp-bootstrap)
- [9. Recursive array TypeCode traversal](#recursive-array-typecode-traversal)
- [10. Recursive writer memory](#recursive-writer-memory)
- [11. Recursive type generation](#recursive-type-generation)
- [12. Recursive type-name qualification](#recursive-aggregate-qualification)
- [13. Wide-character union reference](#wide-character-union-reference)
- [14. FlatData reference input](#flatdata-reference-input)

<a id="optional-wide-string-sequences"></a>

## 1. Optional wide-string sequences

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

## 2. Shadowed C# member

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

## 3. Numeric character casts

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

## 4. Character fallback discriminator

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

## 5. Character label metadata

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

## 6. Unsigned character compilation

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

## 7. Recursive metadata propagation

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

## 8. Recursive C# plugin bootstrap

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

## 9. Recursive array TypeCode traversal

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

## 10. Recursive writer memory

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

## 11. Recursive type generation

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

## 12. Recursive type-name qualification

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

## 13. Wide-character union reference

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

## 14. FlatData reference input

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

[FlatData is a C++ binding](https://community.rti.com/static/documentation/connext-dds/7.4.0/doc/manuals/connext_dds_professional/users_manual/users_manual/Using_BuiltinAnnotations.htm); RTI C# ignores the annotation.
For the reference exchange, remove the mapping annotation from scratch C++ input:

```sh
sed '/@language_binding(FLAT_DATA)/d' "$idl_path" > "$native_dir/idl.idl"
```

This preserves the canonical extensibility and XCDR2 sample shape while selecting
ordinary C++ mapping. It is a reference-model deviation, not a FlatData fix.
The canonical IDL and C# inputs are unchanged; FlatData C++ layout is unqualified.
See [IDL adaptation](scripts/run-matrix.sh).
