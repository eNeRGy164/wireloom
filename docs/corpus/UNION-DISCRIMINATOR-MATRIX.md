# Union discriminator compatibility matrix

This matrix separates four claims: IDL validity, RTI `rtiddsgen` acceptance,
generated C# compilation, and runtime serialization. A successful `rtiddsgen`
run alone does not prove that its emitted code compiles or that the runtime can
serialize the type.

## Standards and oracle sources

- [OMG IDL 4.2 specification page](https://www.omg.org/spec/IDL/4.2/About-IDL/)
  (non-PDF edition). Its union discriminator and label rules define the IDL
  language boundary.
- [RTI Connext documentation portal](https://community.rti.com/documentation)
  and the checked-in RTI 7.7.0 / `rtiddsgen` 4.7.0 corpus oracles.
- [Corpus manifest](manifest.json), including exact acceptance/rejection
  diagnostics for C082 (`long long`), C100 (64-bit label probe), and C113
  (`wchar`).

## RTI 7.7.0.1 generator results

Default initialization follows DDS-XTypes 1.3 Table 9: the discriminator starts
at its type's default, independently of a declared default branch. Integer and
character discriminators start at zero, booleans at false, and enums at the
first literal (including negative values). A selected member is initialized
to its type's default. RTI documents the correction in CODEGENII-1550.
The corpus compliance suite compares primitive constructor constants with the
retained RTI sources; typed runtime fixtures separately cover nonzero/negative
enums and initialized reference branches. Null-copy handling is a managed API
contract beyond RTI's observed behavior. See the
[F4/F9/F10 comparison and repair evidence](../../tests/Wireloom.Dds.Generator.RuntimeContracts/F4-EVIDENCE.md).

The probes used `rtiddsgen` 4.7.0.1 from
`rticom/connext-base:7.7.0.1`, with `-ppDisable`, for both `C#` and `C++11`.
The IDL used minimum and maximum labels plus an unoccupied default branch for
the newly added primitive cases. Existing cases retain their previous branch,
enum, alias, and label variations.

| Discriminator type              | OMG IDL 4.2 | RTI C# generation                                                    | RTI C++11 generation | Wireloom C# compile + RTI serializer            |
| :------------------------------ | :---------- | :------------------------------------------------------------------- | :------------------- | :---------------------------------------------- |
| `boolean`                       | valid       | accepted                                                             | accepted             | pass                                            |
| `char`                          | valid       | accepted                                                             | accepted             | pass                                            |
| `wchar`                         | valid       | rejected by parser                                                   | rejected by parser   | pass                                            |
| `octet`                         | valid       | accepted                                                             | accepted             | pass                                            |
| `short`                         | valid       | accepted                                                             | accepted             | pass                                            |
| `int16`                         | valid       | accepted                                                             | accepted             | pass                                            |
| `unsigned short`                | valid       | accepted                                                             | accepted             | pass                                            |
| `uint16`                        | valid       | accepted                                                             | accepted             | pass                                            |
| `long`                          | valid       | accepted                                                             | accepted             | pass                                            |
| `int32`                         | valid       | accepted                                                             | accepted             | pass                                            |
| `unsigned long`                 | valid       | accepted                                                             | accepted             | pass, including `4294967295`                    |
| `uint32`                        | valid       | accepted                                                             | accepted             | pass, including `2147483648` and `4294967295`   |
| `int8`                          | valid       | accepted                                                             | accepted             | pass                                            |
| `uint8`                         | valid       | accepted                                                             | accepted             | pass                                            |
| `long long` / `int64`           | valid       | rejected: “int64 and uint64 are not valid types for a discriminator” | same                 | rejected in corpus cases C082, C100, C127, C129 |
| `unsigned long long` / `uint64` | valid       | rejected: “int64 and uint64 are not valid types for a discriminator” | same                 | rejected in corpus cases C126, C128             |
| enum                            | valid       | accepted (C023 oracle)                                               | accepted             | pass                                            |

The positive corpus includes an RTI C# oracle for every RTI-accepted
discriminator spelling. The boundary cases are C117–C125, C130, C132, and C133;
they retain RTI source/plugin outputs. RTI also accepts `wchar` in OMG IDL,
but `rtiddsgen` rejects it before generating an oracle (C113), while Wireloom
continues to compile it.

## Default and boundary values

The generated-code runtime matrix serializes each explicit value and also
serializes the generated default branch for every case. The following table
defines the values exercised for RTI-supported discriminators:

| Discriminator    | Default          | Minimum          | Maximum         | In-process RTI serializer                                                                                    | Live DDS peer matrix                                                                                                    |
| :--------------- | :--------------- | :--------------- | :-------------- | :----------------------------------------------------------------------------------------------------------- | :---------------------------------------------------------------------------------------------------------------------- |
| `boolean`        | `FALSE`          | `FALSE`          | `TRUE`          | pass                                                                                                         | existing live boolean cases pass                                                                                        |
| `char`           | NUL (`'\\0'`)    | NUL (`'\\0'`)    | U+00FF (`'ÿ'`)  | Wireloom preserves discriminator and payload with signed metadata labels matching correctly compiled RTI C++ | Historical failures and the corrected setup are described below; unadapted RTI C# still loses the high-bit branch value |
| `octet`          | 0                | 0                | 255             | pass                                                                                                         | all four pairings pass at default/minimum/maximum                                                                       |
| `short`          | 0                | -32768           | 32767           | pass                                                                                                         | focused fixture added after the 492-exchange run; live peer result pending                                              |
| `int8`           | 0                | -128             | 127             | pass                                                                                                         | all four pairings pass at default/minimum/maximum                                                                       |
| `uint8`          | 0                | 0                | 255             | pass                                                                                                         | C++→C# fails at minimum; remaining pairings and values pass                                                             |
| `short`          | 0                | -32768           | 32767           | pass                                                                                                         | short-boundaries fixture added after the 492-exchange run; focused live exchange remains to be run                      |
| `int16`          | 0                | -32768           | 32767           | pass                                                                                                         | all four pairings pass at default/minimum/maximum                                                                       |
| `unsigned short` | 0                | 0                | 65535           | pass                                                                                                         | all four pairings pass at default/minimum/maximum                                                                       |
| `uint16`         | 0                | 0                | 65535           | pass                                                                                                         | all four pairings pass at default/minimum/maximum                                                                       |
| `long`           | 0                | -2147483648      | 2147483647      | pass                                                                                                         | long-boundaries fixture added after the 492-exchange run; focused live exchange remains to be run                       |
| `int32`          | 0                | -2147483648      | 2147483647      | pass                                                                                                         | all four pairings pass at default/minimum/maximum                                                                       |
| `unsigned long`  | 0                | 0                | 4294967295      | pass                                                                                                         | C#→C# and C++→C# fail at maximum with `Value was either too large or too small for an Int32`; C++→C++ and C#→C++ pass   |
| `uint32`         | 0                | 0                | 4294967295      | pass                                                                                                         | same maximum failure as `unsigned long`; minimum/default pass                                                           |
| enum             | first enumerator | first enumerator | last enumerator | pass                                                                                                         | existing enum union cases pass                                                                                          |

The original `char` metadata used positive label 255 while RTI selected native
character branches using signed value -1. Both Wireloom and the RTI C# oracle
then lost the maximum branch value. Wireloom now emits a signed metadata label,
while its managed discriminator remains `char` value 255. Serializer tests
require the assigned payload to survive at values 127, 128, 254, and 255.

This representation is checked against RTI-generated C++, not inferred from
its C# oracle. RTI C++ emits `static_cast<int>('ÿ')` in its TypeCode. With
GCC's default UTF-8 execution charset, that is multicharacter integer 50111,
which narrows to byte 191 and fails serialization. Compiling the unchanged
output with `-fexec-charset=ISO-8859-1` yields label -1, byte 255, and preserves
payload 8201 in a direct C++ CDR round trip. The harness applies and reports
that setting only for the character boundary fixture.

The licensed live peer matrix was run against RTI 7.7.0 for all corpus cases,
including three fixtures (default, minimum, maximum) for each new primitive
union case. It completed 492 exchanges: 467 passed and 25 failed, of which 8
were already registered expected failures. The output is in
`artifacts/wire-compatibility/results.md` and `results.json` (local run
artifacts). The C# `unsigned long`/`uint32` maximum failures occur even though
the C++ peer handles those values and Wireloom's managed discriminator stays
`uint`; investigation of the later 1,179-exchange report reproduced the Int32
range error in the harness's `IndependentSampleVerifier.FindUnionMember`,
before DDS participant creation. The verifier now normalizes both metadata
labels and snapshot discriminators to the declared type's range. This was a
verifier defect, not a failure to serialize the unsigned value. The `uint8`
minimum C++→C# failure and the `char` C++ interoperability failures are also
kept visible as observed failures pending an RTI-oracle control.

“Wireloom C# compile + RTI serializer” is covered by the
`RtiAcceptedPrimitiveDiscriminatorsCompileAndExposeRoundTripBehavior` cases in
`tests/Wireloom.Dds.Generator.Tests/Generation/GeneratorSpecs.cs`. The tests
compile actual generated documents against RTI Connext DDS 7.7.0, create the
generated DynamicType and serializer, then compare the round-tripped
discriminator and branch value. They cover lower and upper bounds for signed
and unsigned 8-, 16-, and 32-bit types, as well as `boolean`, `char`, `wchar`,
`octet`, and enum discriminators.

## Discriminator values and RTI metadata labels

A union's **discriminator value** selects its active branch. An IDL case label
names the logical value that selects that branch. RTI's **metadata label** is
the integer representation supplied to its type-description API; it may look
different because that API interprets the native discriminator as signed.
The branch's **payload** is separate from both: a payload such as 8201 does
not undergo the label conversion.

For an IDL narrow `char`, distinguish these representations:

| IDL case / managed discriminator | Logical numeric value | DDS discriminator byte | RTI metadata label |
| :------------------------------- | --------------------: | :--------------------- | -----------------: |
| `'\0'` / `(char)0`               |                     0 | `0x00`                 |                  0 |
| `(char)127`                      |                   127 | `0x7F`                 |                127 |
| `(char)128`                      |                   128 | `0x80`                 |               -128 |
| `'þ'` / `(char)254`              |                   254 | `0xFE`                 |                 -2 |
| `'ÿ'` / `(char)255`              |                   255 | `0xFF`                 |                 -1 |

Wireloom preserves the original character in its managed discriminator,
property access checks, and native byte. It converts only the RTI metadata
label: values 0–127 remain unchanged; values 128–255 become `value - 256`.
Thus `case 'þ'` remains logical case 254, while its generated `UnionMember`
metadata contains -2. It is not an IDL case of -2, and no negative character is
sent. This signed 8-bit rule applies to narrow `char`, not `wchar`, `octet`,
or `uint8` labels. Numeric C# construction such as `(char)254` denotes the same
character and wire byte as `'þ'`; it does not change the metadata requirement.

This matches the RTI C++ TypeCode on the tested signed-char toolchain with the
single-byte execution charset described above. Wireloom's RTI serializer
checks preserve both discriminator 254 and its assigned branch payload, and
inspect the corresponding metadata label -2. The independent wire verifier
normalizes signed metadata back to logical discriminator values before
comparing branches and snapshots.

Full-width unsigned 32-bit labels have a related, separate representation:
managed `uint` value 4294967295 remains that value, its wire bits remain
`0xFFFFFFFF`, and RTI's signed 32-bit metadata label is -1. Wireloom emits the
explicit unchecked conversion described in the next section. The conversion
is specific to the discriminator type and its width; it is not a general rule
that all values above 127 become negative labels.

## High `uint32` labels

RTI C# `rtiddsgen` accepts `unsigned long` labels above `Int32.MaxValue`, but
its generated plugin emits constant casts such as `(int)2147483648`. Compiling
that output fails with CS0221 unless the cast is `unchecked`. RTI C++11 emits
the full-width `uint32_t` discriminator and successfully round-tripped both
`2147483648` and `4294967295` through the RTI serializer in the tested
`x64Linux4gcc8.5.0` environment. Its DynamicType metadata also casts the label
to signed `int`; that out-of-range C++ conversion is implementation-defined,
so the result is evidence for the tested runtime/compiler, not a portable C++
guarantee.

Wireloom preserves `uint` in the managed union and native representation. Its
RTI metadata conversion is emitted explicitly as
`unchecked((int)label)`, matching RTI's `int[]` label parameter without
truncating the managed discriminator or branch comparison. The generated
Wireloom source compiles against the RTI package, and RTI serializer round
trips preserve both boundary values.

## Range probes

Narrow `char` labels must fit 0..255, whether written as numeric labels or
character literals and whether the discriminator is declared directly or
through a typedef. Values such as 256 and `'Ā'` are rejected before emission.
A default branch cannot be declared after all 256 narrow-character values
have been occupied. `wchar` retains its separate 0..65535 range and can select
default value 256 when its first 256 values are occupied.

RTI `rtiddsgen` acceptance is not a discriminator-range validator. It accepted
generation for these out-of-range label probes in both C# and C++11:

| Discriminator    | Out-of-range label | Wireloom result          |
| ---------------- | -----------------: | ------------------------ |
| `unsigned long`  |       `4294967296` | rejected before emission |
| `unsigned short` |            `65536` | rejected before emission |
| `long`           |       `2147483648` | rejected before emission |
| `octet`          |              `256` | rejected before emission |

Wireloom's boundary tests also reject labels below the signed minima and
negative labels for unsigned discriminators. This follows the IDL discriminator
type's value range rather than treating RTI's permissive parser acceptance as
proof that an invalid label is meaningful at runtime.

## Remaining runtime scope

The matrix includes both in-process RTI serializer behavior and the licensed
RTI 7.7.0 live peer outcomes above. The live report also contains the existing
unrelated corpus exchanges and expected failures; interpret the discriminator
results by case and fixture rather than treating the aggregate pass count as a
union-only score.

## PR 45 follow-up validation

The original workflow run [38566747727](https://github.com/eNeRGy164/wireloom/actions/runs/38566747727)
recorded 30 failed exchanges. Its Wireloom-to-Wireloom failures were reproduced
against commit `0c056716d890929c3513a67646acfc11b8ae0395` before changes:
two reader shutdown races, two checked verifier conversions, and one
character-label representation mismatch.

Local RTI 7.7.0 validation on October 14, 2026:

| Check                                                                                         | Result                                                                                                             |
| --------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------ |
| Unit tests with coverage                                                                      | 1,187 passed; branch coverage 4,659/4,958 (93.97%); line coverage 6,797/6,865 (99.01%)                             |
| Corpus compliance and generated-source export                                                 | 273 passed; the character plugin's maximum metadata label is -1                                                    |
| Fresh package integration                                                                     | 7 passed using the isolated `0.3.0-wire-fixes` package                                                             |
| Community Qodana                                                                              | Completed with zero SARIF findings; generator project only                                                         |
| Linux wire controls: valuetypes, int16, ulong, uint32, collections, optional string sequences | 132 passed; 30 not run because the unmodified RTI C# unsigned-boundary bindings fail to compile                    |
| Linux character-boundary controls with the explicit C++ charset                               | 22 passed; 5 failed at maximum, all involving unmodified RTI C# payload loss                                       |
| Forced delayed C# readers after writer shutdown                                               | Valuetype and int16 maximum payloads pass with one valid sample and one `NotAliveNoWriters` lifecycle notification |

All 21 selected Wireloom-to-Wireloom fixture exchanges pass. The character
controls also pass Wireloom-to-C++, C++-to-Wireloom, and C++-to-C++ at minimum,
maximum, and default. The five remaining character failures are
Wireloom-to-RTI-C#, RTI-C#-to-Wireloom, RTI-C#-to-RTI-C#,
RTI-C#-to-C++, and C++-to-RTI-C# at maximum. Expected payload 8201 becomes 0;
these remain reported failures, without modifying the C# oracle.

The focused reports are local artifacts at
`artifacts/wire-focus-fixes/results.json` and
`artifacts/wire-focus-char-cpp/results.json`. The former includes an earlier
character run before the C++ charset correction; use the latter for the final
character outcomes. Local controls do not substitute for CI and Coveralls;
those checks must run after these changes are pushed.
