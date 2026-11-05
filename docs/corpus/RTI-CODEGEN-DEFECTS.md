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

| RTI release / generator                     | Cases                                                                 | Observed RTI output defect                                                                                                                                                                                                                        | Wireloom evidence                                                                                                                                                                                                                                                                                                                                                               |
| :------------------------------------------ | :-------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| 6.1.2 / 3.1.2, 7.3.1 / 4.3.1, 7.7.0 / 4.7.0 | C021 `06-alias-composition`                                           | Generated native conversion code uses the parameter named `sample` as the receiver instead of the generated `this.sample` field. The calls fail with `CS1061`.                                                                                    | Wireloom's retained generated case compiles in the same oracle measurement; the reference issue is documented under [shadowed C# member](../../tests/Wireloom.Dds.Generator.WireCompatibility/WORKAROUNDS.md#shadowed-c-member).                                                                                                                                                |
| 6.1.2 / 3.1.2, 7.3.1 / 4.3.1, 7.7.0 / 4.7.0 | C137 `07-union-char-numeric`; C141 `07-union-char-numeric-255`        | Generated numeric `char` discriminator assignments and switch labels omit casts, causing `CS0266`. The C141 peer also needs corrections to preserve the incoming fallback discriminator and represent label 255 in internal metadata.             | The corpus emits compilable Wireloom output for these cases. RTI reference adaptations and the distinct fallback/metadata behavior are documented under [numeric character unions](../../tests/Wireloom.Dds.Generator.WireCompatibility/WORKAROUNDS.md#numeric-character-unions). Wire interoperability uses adapted scratch references; retained RTI sources remain unchanged. |
| 6.1.2 / 3.1.2, 7.3.1 / 4.3.1, 7.7.0 / 4.7.0 | C119 `07-union-ulong`; C125 `07-union-uint32`                         | Generated union metadata casts `4294967295` to `int` without `unchecked`, causing `CS0221`.                                                                                                                                                       | Wireloom retains generated output for both cases. The issue and reference-peer compile failure are recorded in each version's [7.3.1 feature coverage](7.3.1/FEATURE-COVERAGE.md) and [7.7.0 feature coverage](7.7.0/FEATURE-COVERAGE.md).                                                                                                                                      |
| 7.7.0 / 4.7.0                               | C117 `07-union-octet`                                                 | The accepted C# output refers to the discriminator type as `octet`, which is unresolved by C# and causes `CS0246`.                                                                                                                                | Wireloom generates the case; see [7.7.0 feature coverage](7.7.0/FEATURE-COVERAGE.md#positive-cases). The RTI reference peer cannot compile, so it cannot serve as an executable comparison for this case.                                                                                                                                                                       |
| 6.1.2 / 3.1.2 and 7.7.0 / 4.7.0             | C147 `10-resolve-name-wstring-aliases`                                | The accepted raw C# output fails compilation with `CS0400` and `CS8377`. The interoperability harness applies guarded namespace and TypeCode-reference adaptations only to scratch copies; the retained oracle remains unmodified and failing.    | Wireloom's generated case is retained in [7.7.0 generator output](7.7.0/generator/features/10-resolve-name-wstring-aliases/). This is a raw RTI C# compilation finding; wire results rely on the documented adapted reference setup.                                                                                                                                            |
| 7.3.1 / 4.3.1                               | C157 `12-interface-keyword-default`; C159 `12-interface-case-default` | RTI accepts and generates C# for DDS service interfaces, but the RPC transport plugin contains the unexpanded `${member.unmanageDefaultValue}` template expression. Compilation fails. The ordinary data type in each input is separately usable. | Wireloom reports identifier findings and generates the ordinary `Sample` data type; it does not emit interface/RPC bindings. This case is tracked separately in [issue #75](https://github.com/eNeRGy164/wireloom/issues/75). The 7.7.0 captures compile because they omit the C# interface/transport declarations.                                                             |
| 6.1.2 / 3.1.2                               | C175 `01-int8-boundaries`; C120 `07-union-int8`                       | Signed IDL `int8` maps to C# `byte`; negative constants and labels fail with `CS0031`.                                                                                                                                                            | Wireloom preserves the matching `byte` API. Signed-int8 behavior is outside the supported RTI C# scope.                                                                                                                                                                                                                                                                         |
| 6.1.2 / 3.1.2                               | C011 `03-enum-values-explicit` aggregate wrapper                      | A negative default enum value produces invalid casts such as `(CorpusEnumValues.Explicit) -2`, causing `CS0075`. The enum-only input compiles.                                                                                                    | The [wrapper capture](6.1.2/rtiddsgen/csharp/wire-wrapped/03-enum-values-explicit/) records the failing reference shape; Wireloom-to-C++ exchanges have separate coverage.                                                                                                                                                                                                      |
| 6.1.2 / 3.1.2                               | C157–C160 DDS service interface probes                                | RPC/plugin templates produce invalid C# in default and strict modes.                                                                                                                                                                              | Wireloom generates ordinary data types and checks names; interface/RPC bindings remain outside its scope under [issue #75](https://github.com/eNeRGy164/wireloom/issues/75).                                                                                                                                                                                                    |

The case captures and exact-profile compilation checks above establish the
listed RTI raw-output failures. The
[emitted-C# comparison](csharp-quality.md#errors) retains RTI and Wireloom
inspection findings under its declared staging conditions, alongside separate
compiler diagnostics. Qodana counts can differ from compiler counts when one
syntax failure produces several inspection locations.

<a id="rti-612--rtiddsgen-312"></a>

## Reference restrictions

RTI's [4.2 fixes](https://community.rti.com/static/documentation/connext-dds/7.3.0/doc/manuals/connext_dds_professional/code_generator/release_notes/whats_fixed/whats_fixed420.html)
identify the 6.1.2 signed-int8 defects as CODEGENII-1495/1593. Negative
`int8` annotation values are covered by CODEGENII-1949, and negative enum
defaults by
[CODEGENII-1799](https://community.rti.com/static/documentation/connext-dds/7.3.0/doc/manuals/connext_dds_professional/code_generator/release_notes/whats_fixed/whats_fixed410.html#major-negative-enums-produced-compilation-error-in-c).
These RTI C# reference behaviors are unsupported. Wireloom applies its own
name-safety checks rather than reproducing strict RTI validation behavior.

The CODEGENII-2018 XCDR2 union-key restriction is a runtime/configuration
exclusion. Feature availability and reference restrictions are recorded in
the [support policy](../guides/support-and-compatibility.md#rti-version-qualification).

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
