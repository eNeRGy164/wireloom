# Corpus feature coverage

Wireloom selects its RTI API profile from the resolved `Rti.ConnextDds`
assembly. Versions from 7.3.1 up to, but not including, 7.7.0 use the 7.3.1
profile; version 7.7.0 and later use the 7.7.0 profile. Only exact 7.3.1 and
7.7.0 have qualification evidence.

The corpus contains 80 positive feature cases, 64 negative probes, and 4
integration entry points. Wireloom accepts 79 applicable positive cases under
the 7.3.1 profile; C142 is not applicable. It accepts all 80 positive cases
under the 7.7.0 profile. “Implemented” describes Wireloom generation support;
it does not promise RTI acceptance or runtime interoperability. See the
version pages for profile-specific evidence and restrictions.

| Feature category             | Coverage represented                                                                                        | 7.3.1 profile                                                                                                              | 7.7.0 profile                                                                                                 |
| :--------------------------- | :---------------------------------------------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------ |
| Primitives and aliases       | Primitive values and boundaries, enums, constants, scopes, aggregate and scalar typedef chains              | Implemented                                                                                                                | Implemented                                                                                                   |
| Collections and aggregates   | Strings, sequences, arrays, nested collections, structs, inheritance, recursion, and valuetypes             | Implemented with documented shape restrictions; direct arrays of sequence members have an unsupported mapping              | Implemented with documented shape restrictions; direct arrays of sequence members have an unsupported mapping |
| Unions                       | Primitive and enum discriminators, defaults, aliases, multiple labels, and collection branches              | Implemented; known cross-language interoperability gaps are recorded per case                                              | Implemented; known cross-language interoperability gaps are recorded per case                                 |
| Annotations and evolution    | Topic, key, default, member ID, optional-member, extensibility, and data-representation annotations         | Implemented for applicable features; `@must_understand` is N/A                                                         | Implemented                                                                                                   |
| Preprocessing                | Includes, command-line defines, conditional branches, macros, macro operators, and platform-specific probes | Implemented                                                                                                                | Implemented                                                                                                   |
| Restrictions and workarounds | Negative probes, ignored constructs, and integration cases                                                  | Evaluated per case; rejection may come from Wireloom target checks or RTI limitations                                      | Evaluated per case; rejection may come from Wireloom validation or RTI limitations                            |

## Annotation case applicability

| Case                        | 7.3.1                                                                       | 7.7.0                                                  |
| :-------------------------- | :-------------------------------------------------------------------------- | :----------------------------------------------------- |
| C040 · `09-autoid-hash`     | Hash IDs supported; RTI C# and C++ generation accepted                      | Hash IDs supported; RTI C# and C++ generation accepted |
| C142 · `09-must-understand` | N/A: introduced after this release; explicit compilation reports `DDSG0004` | Supported; RTI C# and C++ generation accepted          |

## Recorded wire evidence

Generation coverage and DDS interoperability are separate. The profile pages
record the current limitations and required peer setup. The
[workaround guide](../../tests/Wireloom.Dds.Generator.WireCompatibility/WORKAROUNDS.md)
shows each failing path and code change. Detailed fixture,
pairing, and adaptation results belong in wire reports; see the
[wire-matrix guide](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#read-the-results)
and [reproduction runbook](LTS-PROFILES.md).

| Limitation or required setup                         | RTI 7.3.1                                                                                                                                                                                                                                                                 | RTI 7.7.0                                                                                                                   |
| :--------------------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | :-------------------------------------------------------------------------------------------------------------------------- |
| Direct arrays of sequences (C018)                    | C# mappings flatten the array layer; cross-language interoperability is unsupported.                                                                                                                                                                                      | Same limitation.                                                                                                            |
| Numeric `char` union labels (C137, C141)             | RTI generated bindings require the [character-binding corrections](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#reproducible-rti-character-binding-workarounds).                                                                                        | Same corrections.                                                                                                           |
| Negative `int8` union discriminator (C120)           | RTI CORE-14660 affects the `-128` fixture and its wire pairings.                                                                                                                                                                                                          | —                                                                                                                           |
| `uint32` / `unsigned long` union labels (C125, C119) | RTI C# generated metadata fails compilation with `CS0221`.                                                                                                                                                                                                                | Same limitation.                                                                                                            |
| `octet` union discriminator (C117)                   | —                                                                                                                                                                                                                                                                         | RTI C# generates an unresolved `octet` type (`CS0246`).                                                                     |
| `wchar` union discriminator (C113)                   | RTI C# generation is unavailable; the C++ reference uses a manually constructed `DynamicType`.                                                                                                                                                                            | Same limitation and setup.                                                                                                  |
| Optional recursive fixed arrays (C134)               | Requires the [recursive peer setup and writer memory configuration](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#recursive-array-writer-memory). Default `TypeObject` v2 interoperability is unqualified.                                               | Same setup and qualification limit.                                                                                         |
| Enum-array defaults (C148) | Default-constructed Wireloom samples send `Color.RED`; RTI C# and C++ peers send `Color.GREEN`, so four of nine cross-implementation exchanges fail. | All default-constructed peers send `Color.RED`; all nine exchanges pass. |

See the [7.3.1 feature details](7.3.1/FEATURE-COVERAGE.md) and
[7.7.0 feature details](7.7.0/FEATURE-COVERAGE.md) for per-case artifacts,
RTI generator outcomes, restrictions, and workarounds.
