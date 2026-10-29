# Wire fixture reference

Use this maintainer reference when adding fixture values or investigating a
peer-specific path. For execution and report interpretation, use the
[wire runbook](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md).

## Reliable-writer timing

All managed and native writers use explicit 50 ms `heartbeat_period`,
`fast_heartbeat_period`, and `late_joiner_heartbeat_period` settings with
`max_heartbeat_retries` set to 600, retaining a 30-second inactivity window.
RTI 7.3.1 defaults to 3-second heartbeat periods; RTI changed these defaults
to 200 ms in [7.6.0](https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/release_notes/whats_new/whats_new_in_760.html#updated-default-heartbeat-periods-for-faster-repairs-of-lost-samples).
Explicit settings keep one-sample acknowledgment waits comparable across the
two LTS profiles. Writers still wait for reliable acknowledgments, and readers
still verify the complete payload before an exchange can pass.

## Case-specific fixture construction

Use case-specific typed sample construction and independent value checks for
new or revised wire cases. Share participant/writer/reader lifetime, matching,
timeouts, and report capture. Keep member assignments, optional presence, collection
contents, and union selection in the case's fixture code. Avoid reflection or
string paths where generated member access can make a wrong mapping fail compilation.
Exercise each builder and its independent verifier before starting DDS participants.
A fixture should fail with the case and member identified, rather than becoming
a reader-readiness timeout or an apparent vendor compatibility failure.

The existing harness still has a generic DynamicData fixture path. It is a test
convenience, not an independent oracle for generated defaults or a prerequisite
for using generated types. A fixture-building failure must be investigated as
a harness/API-path problem before it is attributed to DDS interoperability.

Managed case helpers are in `Fixtures/ManagedFixtures.*.cs`; C++ case-specific
construction is in `Native/typed-fixture-peer.hpp.in`. Typed sample construction
and topic wrappers are normal test setup and do not produce adaptation markers.
The [workaround guide](../../tests/Wireloom.Dds.Generator.WireCompatibility/WORKAROUNDS.md) covers changes beyond that baseline.

## Annotation and value-binding cases

C163–C172 use typed fixtures in `ManagedFixtures.IdlGaps.cs` and the C++ peer.
Their sixteen fixtures produce 144 directed exchanges per exact RTI profile.
They exercise explicit values, constructor defaults, all declared branches in
the added unions, every fixed-array element, and exact-boundary strings.
C164 and C172 use distinctly named topic wrappers that include unchanged
canonical IDL and combine its declarations into one sample. Generation
provenance records the canonical inputs and wrapper/dependency hashes.

For C164, the runner reuses RTI C# outputs only when one source and one plugin
with the exact expected names share a directory and contain the expected IDL
types. It preserves them with hashes. If RTI emits two copies, it removes a
nested copy only when both generated files match the expected paths and are
byte-identical, preserving both raw files first. RTI C++ dependency traversal
is recursive: the runner reuses the unique RTI 7.7.0 outputs at their exact
relative paths, or stages the RTI 7.3.1 generated headers at those paths after
byte comparison. It keeps generated and staged copies with hashes. Scenario
endpoint metadata reports these harness adaptations.

For RTI 7.3.1 C#, `rtiddsgen` resolves these canonical includes by short
basename beside the wrapper. The runner supplies byte-identical wrapper,
canonical, and C164 dependency copies from an isolated scratch tree, then keeps
the staged inputs and source/staged hashes in the evidence. The canonical IDL
files in the snapshot remain unchanged.

The C171 type declares `inherited=7`, `overridden=9`, `text="alias"`,
`ratio=1.5`, and `single=1.25`. Its focused sparse case uses an older appendable
C++ DynamicData writer type containing only member ID 0 (`inherited`) and sends
`inherited=17101`. The full Wireloom C# reader supplies the declared defaults
for the four absent trailing members. This sparse schema is compatible through
RTI 7.7.0's member-annotation path; RTI 7.3.1 does not expose that metadata API.

The in-memory `ClearMember` probe is a separate runtime-contract check. On
RTI 7.7.0 it restores integral, narrow-string, and signed-zero defaults, but
fractional defaults are truncated and non-ASCII wide-string defaults are
returned as `??`. A separate runtime metadata check confirms the exact
fractional and wide-string annotation values on 7.7.0, and managed/native
initialization is checked on both profiles; the clear behavior is retained as
a profile limitation.

## DynamicData fixture API limits

These observations concern the fixture-building API, not the tested typed wire path:

- RTI `7.3.1` DynamicData exposes IDL `int8` as `Uint8`; negative `sbyte` assignment
  fails. Scalar fixtures assign generated members directly and readers check all values.
- RTI `7.3.1` DynamicData does not carry the member-default annotation used by C042.
  The fixture uses the generated constructor, assigns `value` and `ranged`, and leaves
  `color` untouched. The reader independently requires `BLUE` (`2`); setting it explicitly
  would hide a constructor-default defect.
- The tested C++ libraries do not link `DynamicData::value<std::wstring>` on either LTS.
  String and collection cases construct generated typed samples; this is ordinary C++
  usage and leaves their serializer and IDL unchanged.

The negative `int8` union discriminator issue [CORE-14660](#rti-731-negative-int8-union-discriminator)
affects the actual wire/serializer path; the report shows its observed failures.

## Recursive fixture metadata

RTI explicitly rejects public `TypeSupport.DynamicType` access for recursive types
with `NotSupportedException`; see the [documented C# limitation](https://community.rti.com/static/documentation/connext-dds/7.3.1/doc/manuals/connext_dds_professional/release_notes/known_issues/known_issues_with_dynamic_data.html).
Constructing a finite typed tree avoids that unsupported API and is normal fixture
setup. Absent, present, and nested fixtures still require independent checks of every node.
Generated plugin initialization, TypeCode traversal, discovery limits, and writer memory
are separate adjustments documented in the workaround guide.

## Reference generation settings

RTI C# and C++ peers are selected from the checked-in corpus and copied into
scratch build directories. Wire execution does not invoke `rtiddsgen`. Wrapper
specific peers and known corrections have dedicated corpus folders with input
and generated-source hashes. Some cases record reference generation settings
in `docs/corpus/manifest.json`:

- `rtiCppCodegenOptions` records options used to capture that case's RTI C++
  oracle. Any documented qualification, such as RTI's experimental
  recursive-structure support, is copied into scenario evidence.
- `rtiCppIdlAdaptations` records changes used only when capturing the RTI C++
  oracle. They do not change the corpus IDL or Wireloom inputs.

The exclusion registries `wire-case-exclusions-7.3.1.json` and
`wire-case-exclusions-7.7.0.json` record cases that are not applicable to each
RTI profile. They do not classify exchange failures.

## Numeric character-union expected values

Both managed and C++ peers require the same discriminator and selected payload:

| Case                        | Fixture         | Discriminator | Selected member | Payload |
| --------------------------- | --------------- | ------------: | --------------- | ------: |
| `07-union-char-numeric`     | `union-127`     |           127 | `value`         |   13701 |
| `07-union-char-numeric-255` | `union-255`     |           255 | `value`         |   13702 |
| Both cases                  | `union-default` |    65 (`'A'`) | `fallback`      |   13703 |

The C++ default fixture explicitly selects 65 after assigning its fallback value;
its generated constructor otherwise retains 0. Readers check these fixed values
independently of fixture construction and generated equality. Selecting the fixture's
discriminator is ordinary sample construction.

## Reproducible RTI character-binding workarounds

See [numeric character unions](../../tests/Wireloom.Dds.Generator.WireCompatibility/WORKAROUNDS.md#numeric-character-unions) for the
failing generated code, guarded post-generation commands, compiler settings,
and exact discriminator/payload checks.

## Recursive-array writer memory

For `09-optional-recursive-array`, all three peers set the writer property
`dds.data_writer.history.memory_manager.fast_pool.pool_buffer_max_size`
to `4096` bytes on both LTS versions. RTI 7.3.1 otherwise attempts to
preallocate about 64 GiB from the recursive type's maximum serialized size,
preventing writer creation before any sample is sent. The threshold controls
fast-pool allocation; it does not cap serialized sample size, and larger
samples can allocate separately. Reader QoS and fixture values are unchanged.

JSON endpoint adaptations and the Markdown Adaptations column disclose this
case-specific writer policy alongside the existing recursive metadata
adaptations. This harness configuration does not modify generated bindings.

## RTI 7.3.1 negative int8 union discriminator

`07-union-int8` remains valid positive IDL. Its `union-minimum` fixture uses
`-128` and has eight failing pairings on RTI 7.3.1. RTI stores the negative
discriminator as an unsigned
value, causing managed DynamicData branch lookup failures and interpreted C#
serializer payload loss. RTI documents this as
[CORE-14660, fixed in 7.6.0](https://community.rti.com/static/documentation/connext-dds/7.7.0.1/doc/manuals/connext_dds_professional/release_notes/whats_fixed/760/fixes_dynamic_data.html).
The observed failures are documented in
[wire run 39131568513](https://github.com/eNeRGy164/wireloom/actions/runs/39131568513)
at revision `3e9450f2` and focused serializer probes against both the unchanged
RTI oracle and Wireloom output.

The observed failures apply only to `union-minimum` and the affected pairings.
This runtime defect does not make the feature not applicable.

## IDL sequence-array corpus case

`05-array-of-sequences-idl` uses a separately named IDL file with the same
declarations as C018 and `DdsIdlSequenceArrayMapping=Idl`. It is an ordinary
corpus case, so default C018 generation and results remain unchanged. Wireloom
C# and unmodified RTI C++ run all four directed pairings
for each fixture:

| Fixture                | Values checked                                                                                         |
| ---------------------- | ------------------------------------------------------------------------------------------------------ |
| `sequence-empty`       | Both array positions hold empty numeric and string sequences.                                          |
| `sequence-unequal`     | Numeric lengths are two/one; string lengths are one/two, with distinct payloads.                       |
| `sequence-mixed-empty` | An empty numeric sequence precedes a populated one; a populated string sequence precedes an empty one. |
| `sequence-boundary`    | Signed numeric limits, a 16-character string, an empty string, and the string sequence bound of three. |

The managed reader checks every position and sequence against literal expected
values without generated equality. The typed C++ reader compares the complete
independently populated generated sample. Each profile plans 16 exchanges.
The direct-form RTI C# peer is explicitly excluded because it flattens the
array-of-sequences type; its five pairings show N/A with the reason in the
report. This does not qualify Java, Python, typedef-based C# peers, or all
possible sequence-array shapes.

The runner rebuilds each case into a separate generated-source directory.
`GenerationProvenanceRecorder` captures evidence in-process through the shared
builder and writer. Native preparation, capture, and report validation share
wrapper selection through `IdlSourceResolver`; capture and validation also
share its transitive-include traversal.
`logs/<case>-generation.json` records the selected mode, IDL, exact runtime,
and SHA-256 hashes of the IDL and generated source files. C149 scenarios retain
this provenance in `results.json`, including after shard merging.
Missing or mismatched provenance prevents completed exchanges from being
published as valid evidence.

To run only the dedicated case inside the licensed helper container:

```bash
export WIRE_COMPATIBILITY_CASE_FILTER=05-array-of-sequences-idl
export WIRE_COMPATIBILITY_RTI_VERSION=7.7.0 # Repeat with 7.3.1.
dotnet run --project tests/Wireloom.Dds.Generator.WireCompatibility.Runner/Wireloom.Dds.Generator.WireCompatibility.Runner.csproj --configuration Release
```
