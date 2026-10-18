# Corpus feature coverage

Wireloom selects its RTI API profile from the resolved `Rti.ConnextDds`
assembly. Versions from 7.3.1 up to, but not including, 7.7.0 use the 7.3.1
profile; version 7.7.0 and later use the 7.7.0 profile. Only exact 7.3.1 and
7.7.0 have qualification evidence.

The corpus groups its 78 positive feature cases into the same categories used
by the version-specific adoption summaries. The 7.3.1 profile generates all 77
applicable positive cases; C142 is not applicable. The 7.7.0 profile generates
all 78 positive cases. “Implemented”
describes Wireloom generation support; it does not promise that RTI accepts the
construct or that all generated types interoperate at runtime. The version
pages describe profile-specific details, RTI oracle outcomes, shape
differences, restrictions, and case-level wire evidence. In addition to the
positive cases, the corpus contains 60 negative probes and 4 integration entry
points; their outcomes are recorded on the detailed pages.

| Feature category             | Coverage represented                                                                                        | 7.3.1 profile                                                                                                 | 7.7.0 profile                                                                                                 |
| :--------------------------- | :---------------------------------------------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------ | :------------------------------------------------------------------------------------------------------------ |
| Primitives and aliases       | Primitive values and boundaries, enums, constants, scopes, aggregate and scalar typedef chains              | Implemented                                                                                                   | Implemented                                                                                                   |
| Collections and aggregates   | Strings, sequences, arrays, nested collections, structs, inheritance, recursion, and valuetypes             | Implemented with documented shape restrictions; direct arrays of sequence members have an unsupported mapping | Implemented with documented shape restrictions; direct arrays of sequence members have an unsupported mapping |
| Unions                       | Primitive and enum discriminators, defaults, aliases, multiple labels, and collection branches              | Implemented; known cross-language interoperability gaps are recorded per case                                 | Implemented; known cross-language interoperability gaps are recorded per case                                 |
| Annotations and evolution    | Topic, key, default, member ID, optional-member, extensibility, and data-representation annotations         | Implemented for applicable features; `@must_understand` is N/A                                                | Implemented                                                                                                   |
| Preprocessing                | Includes, command-line defines, conditional branches, macros, macro operators, and platform-specific probes | Implemented                                                                                                   | Implemented                                                                                                   |
| Restrictions and workarounds | Negative probes, ignored constructs, and integration cases                                                  | Evaluated per case; rejection may come from Wireloom target checks or RTI limitations                         | Evaluated per case; rejection may come from Wireloom validation or RTI limitations                            |

## Annotation case applicability

| Case                        | 7.3.1                                                                       | 7.7.0                                                  |
| :-------------------------- | :-------------------------------------------------------------------------- | :----------------------------------------------------- |
| C040 · `09-autoid-hash`     | Hash IDs supported; RTI C# and C++ generation accepted                      | Hash IDs supported; RTI C# and C++ generation accepted |
| C142 · `09-must-understand` | N/A: introduced after this release; explicit compilation reports `DDSG0004` | Supported; RTI C# and C++ generation accepted          |

The split preserves all existing provenance tags. C142 has an explicit new
tag. Both cases have fresh exact-version RTI captures. Historical wire results
below predate the split: they do not qualify the revised C040 or new C142
inputs. Fresh wire exchanges for these inputs remain pending.

## Recent local wire results

Wire results are separate from generation coverage. Each matrix exchange uses
a producer and consumer pair; a pass count does not establish support for every
case in a category.

| RTI version | Exchanges | Passed | Failed | Status of the recorded run                                                                                                                                                                                                                  |
| :---------- | --------: | -----: | -----: | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| 7.3.1       |     1,377 |  1,233 |    144 | Full local matrix completed in four Docker shards.                                                                                            |
| 7.7.0       |     1,386 |  1,249 |    137 | Retained full local matrix; 69 failures were classified as expected in that report. |

See the [7.3.1 feature details](7.3.1/FEATURE-COVERAGE.md) and
[7.7.0 feature details](7.7.0/FEATURE-COVERAGE.md) for per-case artifacts,
RTI generator outcomes, limitations, and links to the underlying wire reports.
