# 6. Runtime view

Wireloom has no long-running runtime process. Its primary runtime scenario is
the consumer's build, followed later by the generated application's use of the
RTI runtime.

## 6.1 Consumer build and generation

![Consumer build sequence from MSBuild IDL inputs through Roslyn generation and compilation to application use of the RTI runtime.](images/06/consumer-build-sequence.png)

1. MSBuild adds explicit `DdsIdl` roots and tracks other `.idl` files as
   non-generating inputs. Root metadata determines strictness for the batch;
   project properties provide effective limits and the batch-wide
   `DdsIdlEnableEscapeChar` name-normalization setting.
2. Roslyn supplies the files, item metadata, compiler options, and compilation
   references to `Generator`.
3. The generator requires C# 12 or later and a resolved RTI runtime reference
   of at least 7.3.1. The Roslyn adapter reads the resolved assembly version
   and compilation orchestration selects the matching immutable profile.
4. `IdlCompiler` resolves the include graph and preprocesses input. If escape
   normalization is enabled, a token-aware pass removes one leading underscore
   from identifiers and composes source origins before parsing. The front end
   then binds normalized references and deferred bounds against the complete
   symbol graph and validates semantics using the batch policy.
5. Emission planning applies profile-specific API policies; emitters render the
   resolved plans and return generated C# documents to the central result.
   Input that prevents safe managed output produces `DDSG0001` at its IDL
   location. Deliberately ignored constructs and potentially lossy mappings
   report diagnostics while generation continues; see the
   [diagnostic rules](08-crosscutting-concepts.md#82-diagnostics-and-source-locations).
6. The consumer compiles generated types together with its hand-written code.

Binding order is explicit: integral, boolean, character, and enum constants, deferred collection
bounds, string and floating constants, explicit and inherited typedef defaults,
inheritance and member types, member annotation values, then effective member
IDs. Semantic validation and emission planning follow these stages. Original
source offsets are captured before work is queued; binding does not depend on
the parser's current input or source mapping.

Macro argument binding, stringification, token pasting, and replacement
rescanning happen during preprocessing. Adjacent string literals retain their
token boundaries through that stage. Value binding decodes each literal token,
concatenates the decoded values, and validates the final encoded length.
Emission formats the bound value for C# without replaying IDL expressions or
applying text replacement to literal contents.

The adapter resolves direct sequence-array mapping from file metadata first,
then the project property, defaulting to `RtiCSharp`. The source file owns the
choice even when reached through multiple roots. Effective sequence and string
limits use file metadata first, then project properties, then defaults of 100
elements and 255 UTF-8 bytes or UTF-16 code units. Explicit IDL bounds override
these effective limits. In `Idl` mode, planning
creates sequence-element wrappers and array metadata without rewriting the
semantic model. Other collections retain their existing emission paths.

## 6.2 Failure paths

- Missing or cyclic includes stop compilation with an IDL diagnostic.
- Unsupported syntax is rejected rather than silently dropped.
- Case-variant keywords and same-scope case-only names report `DDSG0108` and
  continue by default. Strict mode rejects them and new keys in derived types
  with source-located `DDSG0001` before emission.
- Exact reserved keyword identifiers fail in both modes. Suppressing a
  diagnostic does not restore output after semantic validation fails.
- With `DdsIdlEnableEscapeChar`, collisions created by normalized identifiers
  and the identifier `_` (which would normalize to an empty name) fail before
  emission. Implicit hash member IDs use normalized names.
- Invalid sequence-array settings and unsupported recursive direct sequence
  arrays in `Idl` mode produce `DDSG0001` with no partial source emission;
  there is no silent fallback to flattening. The default `RtiCSharp` mapping
  continues to report `DDSG0105` and generate its legacy shape.
- Non-positive or non-Int32 sequence/string size settings produce source-
  located `DDSG0001` before output for the affected input.
- An older language version produces `DDSG0002`.
- A missing compatible RTI runtime reference produces `DDSG0003`.
- Cyclic typedef aliases are rejected during semantic resolution with a source-
  located IDL diagnostic; they do not produce generated output.
- Cancellation is observed during input traversal and preprocessing. Macro-work
  or output limits fail the file with a stable source-located generator failure;
  partial preprocessed text is never passed to the parser.

## 6.3 Wire compatibility evidence

The manually dispatched matrix compares Wireloom-generated C#, RTI-generated
C#, and RTI-generated C++ across all nine directed peer pairings for selected
Wireloom-positive corpus fixtures. A pass requires valid samples and complete
payload comparison; successful discovery alone is insufficient. C147 passes
all 18 directed exchanges on each exact profile. The 7.3.1 package integration
build and runtime-contract suite pass against its exact NuGet package; full
profile wire qualification remains pending.

The manual wire workflow runs independent peer exchanges for selected corpus
cases. A combined report is published only when the selected work has complete,
consistent evidence; setup failures fail the workflow. The [wire compatibility
runbook](../../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#case-shards-and-combined-reports)
documents shard planning, artifact validation, failure handling, and local
operations.

Wire validation requires a valid sample and complete fixture comparison;
discovery or lifecycle-only traffic is not a pass. The runbook documents reader
handling, verifier normalization, and fixture-specific peer behavior.

Known limits remain visible in the evidence: `05-array-of-sequences` has
expected Wireloom C# interoperability failures because its managed shape does
not preserve the IDL array; selected optional wide-string and recursive-array
cases use documented peer/QoS adaptations for RTI 7.7.0 limitations; and the
FlatData fixture validates standard C++ mapping, not RTI's FlatData-specific
layout. These adaptations do not establish unadapted interoperability. The
[wire compatibility runbook](../../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md)
owns fixture setup, peer construction, shard execution, artifacts, and report
details. The separate [runtime contract suite](../../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md)
covers generated API and serialization behavior without establishing the wire
matrix results.

The shared corpus declares `05-array-of-sequences-idl` as a dedicated C149
case. Its separately named IDL has the same declarations as C018 and selects
the `Idl` mapping. It separately plans four directed pairings between Wireloom C# and RTI C++, with four payload
fixtures per exact LTS profile. RTI C# is explicitly excluded because its
direct-form binding describes the flattened type; reports show the exclusion
reason and N/A cells. The default C018 matrix remains unchanged. Case results
retain mode, runtime, IDL hash, and actual generated-source hashes;
missing or mismatched provenance prevents completed exchanges from qualifying.
The focused live runs passed all 16 exchanges on each exact LTS release;
these results qualify only the recorded fixtures and peer scope.
