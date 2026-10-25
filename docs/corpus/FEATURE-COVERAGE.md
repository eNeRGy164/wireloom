# Corpus feature coverage

Wireloom selects its RTI API profile from the resolved `Rti.ConnextDds`
assembly. Versions from 7.3.1 up to, but not including, 7.7.0 use the 7.3.1
profile; version 7.7.0 and later use the 7.7.0 profile. Only exact 7.3.1 and
7.7.0 have qualification evidence.

The corpus contains 91 positive feature cases, 77 negative probes, and 4
integration entry points. “Implemented” describes Wireloom generation support;
it does not promise RTI acceptance or runtime interoperability. C142 is not
applicable to RTI 7.3.1. See the version pages for profile-specific behavior
and restrictions.

| Feature category             | Coverage represented                                                                                                              | 7.3.1 profile                                                                         | 7.7.0 profile                                                                      |
| :--------------------------- | :-------------------------------------------------------------------------------------------------------------------------------- | :------------------------------------------------------------------------------------ | :--------------------------------------------------------------------------------- |
| Primitives and aliases       | Primitive values and boundaries, enums, enum-valued constants, scopes, and annotated scalar or aggregate typedef chains           | Implemented                                                                           | Implemented                                                                        |
| Collections and aggregates   | Strings with expression bounds, sequences, arrays, nested collections, structs, inheritance, recursion, and value types           | Implemented; configurable `RtiCSharp` and `Idl` mappings                              | Implemented; configurable `RtiCSharp` and `Idl` mappings                           |
| Unions                       | Primitive and enum discriminators, symbolic/scoped/expression labels, defaults, aliases, and fixed-array or collection branches   | Implemented; known cross-language interoperability gaps are recorded per case         | Implemented; known cross-language interoperability gaps are recorded per case      |
| Annotations and evolution    | Topic, boolean/named/constant arguments, member IDs, optional-member, extensibility, auto-ID, and data-representation annotations | Implemented for applicable features; `@must_understand` is N/A                        | Implemented                                                                        |
| Preprocessing                | Includes, command-line defines, conditional branches, macros, macro operators, and platform-specific probes                       | Implemented                                                                           | Implemented                                                                        |
| Specification validation     | Keyword and same-scope case-only names, exact reserved keywords, and derived keys, and interface-body declarations (C150–C162)    | Implemented; default naming warnings and opt-in strict rejection                      | Implemented; default naming warnings and opt-in strict rejection                   |
| Restrictions and workarounds | Negative probes, ignored constructs, and integration cases                                                                        | Evaluated per case; rejection may come from Wireloom target checks or RTI limitations | Evaluated per case; rejection may come from Wireloom validation or RTI limitations |

## Annotation case applicability

| Case                        | 7.3.1                                                               | 7.7.0              |
| :-------------------------- | :------------------------------------------------------------------ | :----------------- |
| C040 · `09-autoid-hash`     | Hash IDs supported                                                  | Hash IDs supported |
| C142 · `09-must-understand` | N/A: introduced after this release; explicit use reports `DDSG0004` | Supported          |

## Current grouped IDL cases

The parser captures ordered balanced applications and ignores unknown member
applications without interpreting their arguments. A full catalog-wide
mixed-order generated-output case and shared OMG-typed constant-expression
coverage remain tracked in [#91](https://github.com/eNeRGy164/wireloom/issues/91).

C163–C172 cover full-form extensibility, module and local auto-ID scope,
annotation arguments, multiple member declarators, symbolic and expression
union labels, scoped enum labels and constants, direct fixed-array union
branches, expression string bounds, and typed defaults. Profile-specific
RTI behavior and restrictions are recorded in the corresponding feature pages.

Typed defaults declared by C171 initialize generated values, and RTI 7.7.0
readers apply those defaults to trailing fields omitted by a compatible
appendable writer. RTI 7.3.1 does not expose the member-annotation API used by
the metadata-restoration path. See the
[fixture scope](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#annotation-and-value-binding-cases).

## Recorded wire evidence

Dedicated case C149, `05-array-of-sequences-idl`, selects the `Idl` mapping for
the declarations also covered by C018. This isolates the mapping choice from
the declaration shape. See the
[configuration reference](../guides/configuration.md) and [verification
summary](../testing/sequence-array-mapping.md#focused-verification).

Generation coverage and DDS interoperability are separate. The profile pages
record the current limitations and required peer setup. The
[workaround guide](../../tests/Wireloom.Dds.Generator.WireCompatibility/WORKAROUNDS.md)
shows each failing path and code change. Detailed fixture,
pairing, and adaptation results belong in wire reports; see the
[wire-matrix guide](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#read-the-results)
and [reproduction runbook](LTS-PROFILES.md).

| Limitation or required setup                         | RTI 7.3.1                                                                                                                                                                                                                   | RTI 7.7.0                                                             |
| :--------------------------------------------------- | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :-------------------------------------------------------------------- |
| Direct arrays of sequences (C018, C149)              | Set `Idl` to preserve both layers; default `RtiCSharp` flattens them. C149 is the dedicated IDL-preserving corpus case.                                                                                                     | Same mapping behavior.                                                |
| Numeric `char` union labels (C137, C141)             | RTI generated bindings require the [character-binding corrections](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#reproducible-rti-character-binding-workarounds).                                          | Same corrections.                                                     |
| Negative `int8` union discriminator (C120)           | RTI CORE-14660 affects the `-128` fixture and its wire pairings.                                                                                                                                                            | —                                                                     |
| `uint32` / `unsigned long` union labels (C125, C119) | RTI C# generated metadata fails compilation with `CS0221`.                                                                                                                                                                  | Same limitation.                                                      |
| `octet` union discriminator (C117)                   | —                                                                                                                                                                                                                           | RTI C# generates an unresolved `octet` type (`CS0246`).               |
| `wchar` union discriminator (C113)                   | RTI C# generation is unavailable; the C++ reference uses a manually constructed `DynamicType`.                                                                                                                              | Same limitation and setup.                                            |
| Optional recursive fixed arrays (C134)               | Requires the [recursive peer setup and writer memory configuration](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#recursive-array-writer-memory). Default `TypeObject` v2 interoperability is unqualified. | Same setup and qualification limit.                                   |
| Enum-array defaults (C148)                           | Default-constructed Wireloom samples send `Color.RED`; RTI C# and C++ peers send `Color.GREEN`, producing asymmetric cross-implementation values.                                                                           | Default-constructed peers use `Color.RED`, matching Wireloom's value. |

See the [7.3.1 feature details](7.3.1/FEATURE-COVERAGE.md) and
[7.7.0 feature details](7.7.0/FEATURE-COVERAGE.md) for per-case behavior,
profile limitations, and workarounds.
