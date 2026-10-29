# IDL semantic reference

This guide records detailed language rules and retained compatibility findings
for maintainers changing parsing, binding, validation, or emission.
The architectural invariants are in
[IDL meaning and identity](../architecture/arc42/08-crosscutting-concepts.md#81-idl-meaning-and-identity);
the [support policy](support-and-compatibility.md) and
[feature index](../corpus/FEATURE-COVERAGE.md) own support and qualification status.

## Semantic authority

Supported language constructs follow OMG IDL and DDS-XTypes semantics.
Retained RTI output records observed compatibility for specific inputs and
tool versions; it does not limit valid spellings or override the specifications.
RTI extensions and remaining Wireloom implementation limits are identified
separately from those language rules.

## Typed values in practice

For two enums with the same numeric value, identity still matters:

```idl
enum Color { RED };
enum State { READY };
const Color selected = Color::RED; // Color, not an untyped integer.
```

A name bound to `Color::RED` cannot satisfy a `State` default or discriminator
label merely because both enumerators have value zero. The binder retains the
declaring enum alongside its value. Likewise, a narrow character constant,
an integer, and a Boolean are not interchangeable bound values. The
[architectural concept](../architecture/arc42/08-crosscutting-concepts.md#81-idl-meaning-and-identity)
explains why this invariant is shared by every emitter.

## Annotation applications

The front end captures ordered annotation applications with balanced raw text
and source offsets before known-name argument validation. Unknown applications
remain opaque and are ignored with diagnostics. The built-in catalog defines
parameter kinds, defaults, legal targets, positional rules, repeat/conflict
metadata, and supported versus recognized-unsupported names. Context-specific
readers apply those definitions while retaining target-specific semantic and
compatibility behavior. It distinguishes an absent boolean annotation from an
explicit `false`; shorthand
and full-form extensibility annotations resolve to the same model. A bare
`@autoid` selects `HASH`. Module auto-ID applies within that module block,
reopened blocks are resolved independently, and a declaration-local setting
does not leak to sibling declarations or included top-level declarations.
Profile-specific annotation availability is checked at the compilation and
emission boundary.

Known applications reject repeated named argument keys and invalid argument
shapes. Type prefixes, members, enum literals, and union branches consume
ordered applications; unknown arguments stay opaque. The typed expression
dispatcher reuses integral and floating evaluators with OMG numeric expression
semantics, while boolean member flags bind constants after the complete symbol
graph is available. Boolean, character, string, and enum values remain typed
literals or names resolved by their dedicated value rules. Ordinary `long double`
members map to `LongDouble`, but `long double` constants remain unsupported.
The value resolver also rejects `@min`, `@max`, `@range`, and `@default` for
`long double`. The catalog validates repeat/conflict rules
across captured target lists, preserving context-specific diagnostics.
Local struct/union `@autoid` and typedef `@default` applications are retained
on their semantic targets, and typedef default expressions are read during
binding after target resolution. Module scope records retain module-level
annotations and the member-ID policy inherited by nested declarations.
Effective member-ID rules combine captured hash metadata with bound explicit
and sequential IDs.
Unknown applications, including those interleaved with member annotations,
are consumed as complete scoped applications, including balanced and quoted
arguments, before the following declaration is checked.

Balanced application records are transient parser inputs; the semantic model
stores effective values and unresolved expressions needed for binding rather
than duplicating source text. For an annotated comma-separated member
declaration, the parser consumes the list once and applies effective metadata
to the final declarator, matching the retained RTI case. The OMG question
about annotation application to multiple declarators is tracked as IDL43-37.
Wireloom's current behavior is a compatibility policy, not a claimed normative
resolution ([OMG IDL issues](https://issues.omg.org/issues/spec/IDL)).

An enum accepts FINAL or APPENDABLE extensibility in either shorthand or full
form. MUTABLE is invalid for enumerated types. Applying extensibility more
than once is an error, including equivalent shorthand and full-form uses.
These rules follow [DDS-XTypes 1.3 sections 7.2.2.4.1 and 7.3.1.2.1.8](https://www.omg.org/spec/DDS-XTypes/1.3/PDF).

## Constants, type identity, and member IDs

Quoted literal contents do not terminate declaration bodies or annotation
arguments. Explicit member-ID expressions resolve after constant binding and
before duplicate-ID validation, and take precedence over automatic hash IDs.
Sequential IDs continue after the preceding explicit or sequential ID,
including inherited members. Hash IDs do not advance that counter. The binder
records effective IDs separately from declared annotations; validation checks
the complete inherited and local member set, and emission renders those IDs.

Constant expressions, chained enum-valued constants, union labels, string bounds, and
defaults use the shared binding and evaluation path. Enum values retain their
declaring enum identity for assignment and label validation. Bound scalar
values distinguish integers, booleans, characters, floating-point values,
strings, and enumerators. Only integer values enter integer expressions such
as bounds and member IDs; boolean and character constants are resolved by
their own default and discriminator rules. Integral evaluation retains literal
values until their expression type is determined, then promotes operands
using their individual rank and signedness and checks each intermediate
result. The declared target type is validated after expression evaluation;
`@id` literals share this integer-expression path, including octal and
hexadecimal forms. `@unit` and `@hashid` string arguments use the shared
literal decoder, which combines adjacent tokens and supports wide literals
before storing the resulting value. Floating-point range annotations fail
during binding with a source-located IDL diagnostic. Union labels are checked
for duplicates after evaluation before emission.
Direct fixed-array union branches remain arrays in the semantic and emission
models, and use the same initialization, copying, conversion, and destruction
paths as other arrays.

## Defaults and runtime observations

Defaults resolve after the declared type and alias chain are known. A default
on the nearest annotated typedef in the chain applies unless the member has an
explicit default. The resolved typed scalar value feeds
managed initialization, native initialization, and DynamicType annotations.
The default is therefore a type-aware semantic value rather than a literal
string interpreted independently by each emitter. Profile runtime support for
DynamicType defaults is recorded with the corresponding corpus and runtime
evidence. The 7.3.1 API has no member annotation support for missing-field
defaults. On 7.7.0, runtime tests restore integral and narrow-string defaults
after clearing DynamicData members. Managed/native initialization preserves
fractional float/double and non-ASCII wide-string defaults on both LTS
profiles. Runtime metadata tests confirm that RTI 7.7.0 exposes the fractional
and wide-string annotation values exactly. Its `ClearMember` path does not
preserve fractional metadata defaults (1.5 becomes 1.0, 2.5 becomes 2.0, and
1.25 becomes 1.0) and returns `??` for a cleared non-ASCII wide-string
default; these are recorded as DynamicData restoration limitations, not
generator initialization failures.
Negative-zero sign bits survive cleared-member restoration on 7.7.0. For C171,
a one-member appendable writer sends `inherited=17101`; the full reader receives
`overridden=9`, `text="alias"`, `ratio=1.5`, and `single=1.25` from the omitted
trailing members' declared defaults. This behavior uses DDS deserialization,
separately from in-memory `ClearMember` behavior. RTI 7.3.1 lacks the
member-annotation API used for metadata restoration and sparse-field defaults.

## Literal types and range validation

Range validation compares integral, boolean, and character defaults with the
declared limits, including defaults inherited from typedefs. Boolean and
character values use their ordering representation only for this comparison;
they remain ineligible for integer expressions such as bounds and member IDs.

Character defaults require the matching `char` or `wchar` literal or constant
type, including through aliases; an integer expression is not a character
default. String literals also require matching width: every token assigned to
a `wstring` requires the `L` prefix, and a narrow string cannot use that prefix.
Adjacent string literals are decoded independently and then joined,
so hexadecimal and octal escapes cannot consume characters from the next
token. Bounds apply to the final value. These rules follow
[IDL 4.2 sections 7.2.6.2 and 7.2.6.3](https://www.omg.org/spec/IDL/4.2/PDF).
Focused probes on both RTI LTS generators reject the retained adjacent-string
typedef-default input rather than applying the standard concatenation rule;
Wireloom's regression and runtime tests establish its behavior independently.

The existing `wchar` union-label syntax also accepts unprefixed character
literals, as recorded by C113/C131. This is a legacy Wireloom extension;
constant and default assignments still require matching narrow/wide types.

## Implementation and evidence references

The [parse context](../../src/Wireloom.Dds.Compiler/Compiler/FrontEnd/Parsing/IdlParseContext.cs),
[value binder](../../src/Wireloom.Dds.Compiler/Compiler/FrontEnd/Semantic/IdlValueBinder.cs),
and [member-ID binder](../../src/Wireloom.Dds.Compiler/Compiler/FrontEnd/Semantic/IdlMemberIdBinder.cs)
implement the effective semantic rules. Runtime-specific default behavior is
covered by the [generated API contracts](../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md);
DDS evidence remains in the
[wire runbook](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md).

[Documentation](../README.md) · [Shared compiler API](../reference/compiler-api.md)
