# Observed RTI `rtiddsgen` C# code-generation defects

This catalog records reproducible cases where a retained RTI C# generator
accepted an IDL input and generated C# that failed compilation or did not
preserve the generated value correctly. It is based on the repository's
versioned corpus and retained source captures. “Defect” here describes the
observed output in the named `rtiddsgen` release and case; it is not a claim
that RTI has confirmed or fixed the issue, or that every generator release or
configuration is affected.

Wireloom is compared only for the same corpus input and behavior covered by
retained evidence. Some RTI-specific features, including DDS service/RPC
bindings, are outside Wireloom's supported output scope. Those cases illustrate
the different scope and output behavior, not a Wireloom implementation of the
RTI feature.

## Confirmed generated-C# failures

| RTI release / generator         | Cases                                                                 | Observed RTI output defect                                                                                                                                                                                                                        | Wireloom evidence                                                                                                                                                                                                                                                                                                                                                               |
| :------------------------------ | :-------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| 7.3.1 / 4.3.1 and 7.7.0 / 4.7.0 | C021 `06-alias-composition`                                           | Generated native conversion code uses the parameter named `sample` as the receiver instead of the generated `this.sample` field. The calls fail with `CS1061`.                                                                                    | Wireloom's retained generated case compiles in the same oracle measurement; the reference issue is documented under [shadowed C# member](../../tests/Wireloom.Dds.Generator.WireCompatibility/WORKAROUNDS.md#shadowed-c-member).                                                                                                                                                |
| 7.3.1 / 4.3.1 and 7.7.0 / 4.7.0 | C137 `07-union-char-numeric`; C141 `07-union-char-numeric-255`        | Generated numeric `char` discriminator assignments and switch labels omit casts, causing `CS0266`. The C141 peer also needs corrections to preserve the incoming fallback discriminator and represent label 255 in internal metadata.             | The corpus emits compilable Wireloom output for these cases. RTI reference adaptations and the distinct fallback/metadata behavior are documented under [numeric character unions](../../tests/Wireloom.Dds.Generator.WireCompatibility/WORKAROUNDS.md#numeric-character-unions). Wire interoperability uses adapted scratch references; retained RTI sources remain unchanged. |
| 7.3.1 / 4.3.1 and 7.7.0 / 4.7.0 | C119 `07-union-ulong`; C125 `07-union-uint32`                         | Generated union metadata casts `4294967295` to `int` without `unchecked`, causing `CS0221`.                                                                                                                                                       | Wireloom retains generated output for both cases. The issue and reference-peer compile failure are recorded in each version's [7.3.1 feature coverage](7.3.1/FEATURE-COVERAGE.md) and [7.7.0 feature coverage](7.7.0/FEATURE-COVERAGE.md).                                                                                                                                      |
| 7.7.0 / 4.7.0                   | C117 `07-union-octet`                                                 | The accepted C# output refers to the discriminator type as `octet`, which is unresolved by C# and causes `CS0246`.                                                                                                                                | Wireloom generates the case; see [7.7.0 feature coverage](7.7.0/FEATURE-COVERAGE.md#positive-cases). The RTI reference peer cannot compile, so it cannot serve as an executable comparison for this case.                                                                                                                                                                       |
| 7.7.0 / 4.7.0                   | C147 `10-resolve-name-wstring-aliases`                                | The accepted raw C# output fails compilation with `CS0400` and `CS8377`. The interoperability harness applies guarded namespace and TypeCode-reference adaptations only to scratch copies; the retained oracle remains unmodified and failing.    | Wireloom's generated case is retained in [7.7.0 generator output](7.7.0/generator/features/10-resolve-name-wstring-aliases/). This is a raw RTI C# compilation finding; wire results rely on the documented adapted reference setup.                                                                                                                                            |
| 7.3.1 / 4.3.1                   | C157 `12-interface-keyword-default`; C159 `12-interface-case-default` | RTI accepts and generates C# for DDS service interfaces, but the RPC transport plugin contains the unexpanded `${member.unmanageDefaultValue}` template expression. Compilation fails. The ordinary data type in each input is separately usable. | Wireloom reports identifier findings and generates the ordinary `Sample` data type; it does not emit interface/RPC bindings. This case is tracked separately in [issue #75](https://github.com/eNeRGy164/wireloom/issues/75). The 7.7.0 captures compile because they omit the C# interface/transport declarations.                                                             |

The [dated Qodana scan](csharp-quality.md#errors) reports its compiler-error
category under its declared staging and analysis conditions. It is not a
separate reproducible `dotnet build` measurement for every case. The case
captures and exact-profile compilation checks above remain the evidence for
the listed raw-output failures.

## Other oracle findings kept separate

These results are useful to compatibility work but are not counted above as
confirmed defects in generated code for a valid feature input:

- C087 and C088 use duplicate union labels/defaults. RTI accepts the probes and
  its generated C# fails compilation, while Wireloom rejects them. These are
  validation differences for invalid IDL, not valid-feature code-generation
  failures.
- C112 probes names that collide with generated C# members and RTI's generated
  output fails with `CS0102`; Wireloom rejects the generated-name collision.
  Keep this classified as a name-safety/validation result rather than general
  RTI feature support.
- C143–C146 use unresolved array, sequence, string, or wstring bounds with
  `@resolve_name(false)`. RTI 7.7.0 emits unresolved C# references; Wireloom
  rejects bounds that cannot be resolved. Their intended external-name
  semantics should be considered before describing these cases as defects.
- RTI C++ or DDS wire-peer failures, unavailable release features, and required
  harness adaptations are tracked in the per-version feature pages and
  [wire-test workarounds](../../tests/Wireloom.Dds.Generator.WireCompatibility/WORKAROUNDS.md).
  They are not C# code-generation defects unless a case separately demonstrates
  invalid generated C#.

## Evidence and reproduction limits

The case IDs link the findings to the shared IDL inventory and profile-specific
RTI C# captures. The version feature pages summarize generation and compile
outcomes; the raw source folders retain the actual generated files. The corpus
[LTS runbook](LTS-PROFILES.md) documents exact RTI images, capture, and C# oracle
compilation. Reproduction requires the corresponding RTI toolchain and package
version. Retained RTI output is third-party reference material and must remain
unaltered; harness-only adaptations belong in scratch output.

Compilation failures establish that the captured generated source is not
usable as a C# compilation under the recorded harness. They do not by
themselves establish runtime or wire incompatibility. Conversely, successful
compilation is not proof of correct serialization or interoperability. The
catalog is a set of case-level observations, not a statistical estimate of
overall product quality or a claim of exhaustive RTI coverage.
