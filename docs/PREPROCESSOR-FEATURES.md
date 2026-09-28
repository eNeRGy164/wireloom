# Preprocessor feature coverage

This document tracks the preprocessing behavior implemented by Wireloom and
the compatibility work still outstanding. The identifiers use the `PP###`
namespace and are deliberately separate from corpus `C###` identifiers.

The reference column names the behavior being compared: ISO C/C++ means the
standard preprocessing model, while MSVC/CL means behavior documented or
implemented by Microsoft's `cl.exe` preprocessor.

## Status

| Status          | Meaning                                                 |
| :-------------- | :------------------------------------------------------ |
| Implemented     | Implemented and covered by a unit test                  |
| Partial         | Partially implemented or intentionally narrowed for IDL |
| Not implemented | Not implemented                                         |

Current coverage is 40 implemented features, 10 partial features, and 1 feature
not implemented. Partial support currently means:

- PP013 supports common identifier token pasting; complete lexical token-paste
  compatibility is still outstanding.
- PP026 supports selected single-character and encoding-prefixed constants plus
  common, octal, and hexadecimal escapes; multi-character constants and full
  implementation-defined character semantics are still outstanding.
- PP029 supports full-width unsigned literals and selected `ULL` comparisons;
  complete usual-arithmetic-conversion rules are still outstanding.
- PP030 supports digit separators and selected overflow forms; complete
  implementation-defined literal behavior is still outstanding.
- PP041 remaps `__FILE__` and `__LINE__`; diagnostics retain physical source
  locations.
- PP042 supports `#pragma once`, inline pragma forms, and `#pragma message`;
  other pragma state semantics remain no-ops.
- PP043 routes IDL-relevant `#import` and `#using` forms through the include
  graph; MSVC type-library semantics are out of scope.
- PP046 supports deterministic `__COUNTER__`; date/time predefined macros are
  not implemented.
- PP047 supports `_Pragma("once")` and `__pragma(once)`; other payloads are
  consumed without compiler-state effects.
- PP048 supports graph-backed quoted and angle-bracket `__has_include` probes;
  related feature probes are not implemented.

## Source and comment handling

| ID    | Feature                                                    | Reference         | Status      |
| :---- | :--------------------------------------------------------- | :---------------- | :---------- |
| PP001 | `//` line comments                                         | C/C++ and MSVC/CL | Implemented |
| PP002 | `/* ... */` block comments                                 | C/C++ and MSVC/CL | Implemented |
| PP003 | Comment markers protected inside string/character literals | C/C++ and MSVC/CL | Implemented |
| PP004 | Backslash-newline line splicing                            | C/C++ and MSVC/CL | Implemented |
| PP005 | Unterminated comment diagnostics                           | C/C++ and MSVC/CL | Implemented |

## Macro definitions and expansion

| ID    | Feature                                        | Reference                           | Status      |
| :---- | :--------------------------------------------- | :---------------------------------- | :---------- |
| PP006 | Object-like `#define`                          | C/C++ and MSVC/CL                   | Implemented |
| PP007 | Function-like `#define`                        | C/C++ and MSVC/CL                   | Implemented |
| PP008 | Nested macro expansion and rescanning          | C/C++ and MSVC/CL                   | Implemented |
| PP009 | Recursive macro suppression                    | C/C++ and MSVC/CL                   | Implemented |
| PP010 | Macro argument parsing with nested parentheses | C/C++ and MSVC/CL                   | Implemented |
| PP011 | Macro arguments containing literals            | C/C++ and MSVC/CL                   | Implemented |
| PP012 | Stringizing operator `#`                       | C/C++ and MSVC/CL                   | Implemented |
| PP013 | Token-pasting operator `##`                    | C/C++ and MSVC/CL                   | Partial     |
| PP014 | MSVC charizing operator `#@`                   | MSVC/CL                             | Implemented |
| PP015 | Variadic macros and `__VA_ARGS__`              | C/C++ and MSVC/CL                   | Implemented |
| PP016 | Variadic `__VA_OPT__`                          | C++20 and modern MSVC/CL            | Implemented |
| PP017 | Macro argument-count diagnostics               | MSVC/CL-compatible project behavior | Implemented |
| PP018 | Empty macro arguments and placemarkers         | C/C++ and MSVC/CL                   | Implemented |
| PP019 | Named MSVC variadic extension                  | MSVC/CL                             | Implemented |
| PP020 | Full token-based replacement semantics         | C/C++ and MSVC/CL                   | Implemented |

## Conditional compilation and expressions

| ID    | Feature                                                                 | Reference         | Status      |
| :---- | :---------------------------------------------------------------------- | :---------------- | :---------- |
| PP021 | `#if`, `#elif`, `#else`, `#endif`                                       | C/C++ and MSVC/CL | Implemented |
| PP022 | `#ifdef` and `#ifndef`                                                  | C/C++ and MSVC/CL | Implemented |
| PP023 | `defined(name)` and `defined name`                                      | C/C++ and MSVC/CL | Implemented |
| PP024 | Logical, relational, equality, arithmetic, bitwise, and shift operators | C/C++ and MSVC/CL | Implemented |
| PP025 | Conditional operator `?:`                                               | C/C++ and MSVC/CL | Implemented |
| PP026 | Selected character constants in expressions                             | C/C++ and MSVC/CL | Partial     |
| PP027 | Decimal, hexadecimal, octal, binary, and suffixed integers              | C/C++ and MSVC/CL | Implemented |
| PP028 | Short-circuit evaluation                                                | C/C++ and MSVC/CL | Implemented |
| PP029 | Full C signed/unsigned constant-expression semantics                    | C/C++ and MSVC/CL | Partial     |
| PP030 | Full implementation-defined literal and overflow behavior               | C/C++ and MSVC/CL | Partial     |

## File inclusion and directives

| ID    | Feature                             | Reference                             | Status      |
| :---- | :---------------------------------- | :------------------------------------ | :---------- |
| PP031 | Quoted `#include` search            | C/C++ and MSVC/CL                     | Implemented |
| PP032 | Angle-bracket `#include` search     | C/C++ and MSVC/CL                     | Implemented |
| PP033 | Macro-expanded include operands     | C/C++ and MSVC/CL                     | Implemented |
| PP034 | Traditional include guards          | C/C++ and MSVC/CL                     | Implemented |
| PP035 | `#pragma once`                      | MSVC/CL and common compiler extension | Implemented |
| PP036 | Guarded include cycles              | C/C++ and MSVC/CL                     | Implemented |
| PP037 | Unguarded include-cycle diagnostics | Project safety behavior               | Implemented |
| PP038 | `#undef`                            | C/C++ and MSVC/CL                     | Implemented |
| PP039 | `#error`                            | C/C++ and MSVC/CL                     | Implemented |
| PP040 | `#warning`                          | C/C++ and MSVC/CL                     | Implemented |
| PP041 | `#line` source-location remapping   | C/C++ and MSVC/CL                     | Partial     |
| PP042 | General `#pragma` semantics         | C/C++ and MSVC/CL                     | Partial     |
| PP043 | `#import` and `#using`              | MSVC/CL                               | Partial     |

## Predefined and implementation-specific features

| ID    | Feature                                         | Reference                   | Status          |
| :---- | :---------------------------------------------- | :-------------------------- | :-------------- |
| PP044 | `__FILE__` and `__LINE__`                       | C/C++ and MSVC/CL           | Implemented     |
| PP045 | MSVC version and platform predefined macros     | MSVC/CL                     | Not implemented |
| PP046 | `__COUNTER__`, date, and time predefined macros | MSVC/CL                     | Partial         |
| PP047 | `__pragma` and `_Pragma`                        | MSVC/CL and C99/C++11       | Partial         |
| PP048 | `__has_include` and related feature probes      | Modern C/C++ and MSVC/CL    | Partial         |
| PP049 | Include-depth protection                        | Project safety behavior     | Implemented     |
| PP050 | Preprocessed source-origin diagnostic mapping   | Project diagnostic contract | Implemented     |
| PP051 | Deterministic preprocessing work limits         | Project safety behavior     | Implemented     |

## Scope and next work

The `Implemented` entries describe behavior exercised by Wireloom unit tests;
they do not claim byte-for-byte output parity with `cl /P`.

## Adversarial regression matrix

`Implemented` applies only to the rows and restricted token/arithmetic domains
described above. Inputs outside those domains are rejected rather than treated
as evidence of general C-preprocessor compatibility.

| Contract                 | Adversarial cases                                                                              | Evidence                                              |
| :----------------------- | :--------------------------------------------------------------------------------------------- | :---------------------------------------------------- |
| PP006–PP020              | No-space bodies, duplicate/named parameters, lazy arguments, stringize/paste, recursion        | Macro specs                                           |
| PP013                    | Punctuation, invalid and chained pastes, placemarkers, literals, comments, long chains         | Macro specs; restricted preprocessing-token scope     |
| PP021–PP030              | Unsigned shifts/masks, mixed comparisons, literal boundaries, overflow, nesting                | Conditional specs; unsupported arithmetic is rejected |
| PP031–PP043, PP048–PP049 | Guarded/unguarded cycles, malformed operands/directives, macro-expanded probes, `#line` bounds | Include, directive, and graph specs                   |
| PP050                    | Longer/shorter expansion, post-expansion, included-file, EOF, and sequential-parse diagnostics | Diagnostic and validation specs                       |
| PP051                    | Per-input macro-work reset and bounded macro expansion                                         | Limit specs                                           |
