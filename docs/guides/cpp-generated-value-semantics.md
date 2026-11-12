# Generated C++ value semantics

This guide describes optional presence and union values in Wireloom's Modern
C++ output. The examples use the retained RTI 7.7.0 corpus shapes. Check the
[generator guide](../../src/Wireloom.Dds.Cpp.Generator/README.md) and exact
fixture evidence before applying them to another shape or profile.

## Optional members

Generated struct members are public fields. A supported optional member uses
`dds::core::optional<T>` and starts unset. Presence is independent of the
payload: an absent value differs from a present zero, empty string, or empty
collection.

For the [optional evolution contract](../corpus/idl/features/09-evolution-optional.idl):

```idl
module CorpusOptionalEvolution {
    @appendable struct Message {
        @id(1) long requiredValue;
        @id(2) @optional long optionalValue;
        @id(3) @optional string<16> optionalText;
    };
};
```

Use the generated fields directly:

```cpp
#include "09-evolution-optional.hpp"
#include <cassert>
#include <cstdint>
#include <string>

void use_optional_members()
{
    CorpusOptionalEvolution::Message message;
    assert(!message.optionalValue.has_value());
    assert(!message.optionalText.has_value());

    message.optionalValue = std::int32_t{0};
    message.optionalText = std::string{};
    assert(message.optionalValue.has_value());
    assert(message.optionalValue.value() == 0);
    assert(message.optionalText.has_value());
    assert(message.optionalText.value().empty());

    message.optionalValue.reset();
    message.optionalText.reset();
    assert(!message.optionalValue.has_value());
    assert(!message.optionalText.has_value());
}
```

The fields have types `dds::core::optional<std::int32_t>` and
`dds::core::optional<std::string>`. Call `value()` only when a value is present.
The string bound remains 16 in the reflected metadata; optionality does not
replace the bound, member ID, or underlying type.

RTI-generated structs commonly expose accessor functions instead. Wireloom's
public fields are a source-level API difference; using RTI's optional storage
type does not make those call sites interchangeable.

## Optional collections

The [optional collection contract](../corpus/idl/features/09-optional-collections.idl)
contains an optional `sequence<long, 4>` and an optional fixed `long[2]`.
Their storage composes presence with the collection type:

| Member | RTI 7.7.0 storage |
| :----- | :--------------- |
| `values` | `dds::core::optional<omg::types::bounded_sequence<std::int32_t, 4>>` |
| `items` | `dds::core::optional<dds::core::array<std::int32_t, 2>>` |

An empty sequence becomes present only when a value is assigned:

```cpp
#include "09-optional-collections.hpp"
#include <cassert>
#include <cstdint>

void distinguish_absent_and_empty()
{
    CorpusOptionalCollections::Sample sample;
    assert(!sample.values.has_value());

    sample.values = omg::types::bounded_sequence<std::int32_t, 4>{};
    assert(sample.values.has_value());
    assert(sample.values.value().empty());

    sample.values.reset();
    assert(!sample.values.has_value());
}
```

DynamicType metadata preserves the optional flag alongside member IDs 1 and
2, sequence bound 4, and array dimension 2. Native sample access must preserve
the same absent/present distinction. Collection bounds and supported element
types remain shape- and profile-specific.

Optional aggregate fields and union branches are different contracts. In C115
`09-optional-aggregate-member`, the accepted `@optional` annotation on
`Choice.payload` does not wrap the active branch in `dds::core::optional`:
that branch remains a required payload when selected. See the
[aggregate Choice fixture](../../tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppOptionalAggregateChoice/README.md).

## Union values

A generated union has a discriminator, checked branch accessors, and ordinary
typed storage for every branch. This follows the retained RTI 7.7.0 Modern C++
mapping: its internal `Union_` is a struct, not native C++ union storage.
Inactive branch values remain constructed, which avoids manual active-member
lifetime handling but consumes storage and initialization work for all branches.

For the [union contract](../corpus/idl/features/07-unions.idl):

```idl
module CorpusUnions {
    union Choice switch(long) {
        case 0: long number;
        case 1: string text;
        default: boolean flag;
    };
};
```

Branch setters select their explicit label. Changing the discriminator alone
does not reset branch storage:

```cpp
#include "07-unions.hpp"
#include <cassert>
#include <string>

void select_union_branches()
{
    CorpusUnions::Choice choice;
    choice.number(42);
    assert(choice._d() == 0);

    choice.text(std::string{"hello"});
    assert(choice._d() == 1);
    // choice.number() would throw dds::core::PreconditionNotMetError here.

    choice._d(0);
    assert(choice.number() == 42);

    choice.flag(true, 2); // 2 selects the default branch.
    choice.flag(false);  // The one-argument setter keeps that discriminator.
    assert(choice._d() == 2);
    assert(!choice.flag());
}
```

The one-argument default-branch setter requires that branch to be active. The
explicit-discriminator overload rejects a value belonging to another branch.
Branch getters return mutable or const references and reject inactive access.
Equality compares discriminators and the selected branch value; stream output
prints the active branch. For a union without a default branch, an unmatched
discriminator selects no branch. Default construction uses zero for supported
primitive discriminators and the resolved enum default for enum discriminators.

Union typedefs retain their declared C++ alias names and DynamicType alias
layers. The canonical union owns the value type and plugin context. Native
metadata and plugins must describe the generated layout; ordinary value
operations alone do not establish serialization or DDS interoperability.

## Native metadata and qualification

Optional flags, IDs, bounds, dimensions, and union labels remain independent
metadata facts. Selected shapes permit DynamicType inspection without creating
a plugin, following the [initialization design](../architecture/adr/ADR-007-stage-modern-cpp-type-registration.md).
Metadata inspection does not qualify registration, lifecycle, CDR, or live DDS.

Keep these boundaries when checking a contract:

- The optional self-recursive fixed-array case C134 uses optional storage for
  indirection and references an existing metadata shell. It does not establish
  general recursive-graph support or plugin compatibility.
- C174's optional unbounded narrow string requires `--unbounded-support`.
  Its optional aggregate can expose metadata separately from plugin creation;
  this does not generalize support to other unbounded optional shapes.
- C114's optional narrow/wide string sequences have their own exact-shape and
  platform limits. The recorded Linux wide-string rerun crashed in both RTI
  and Wireloom; zero-byte comparisons after those failures are invalid.

Use the current fixture guides for
[optional scalars and strings](../../tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppOptionalEvolution/README.md),
[optional collections](../../tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppOptionalCollections/README.md),
[optional string sequences](../../tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppOptionalStringSequences/README.md),
[optional aggregates](../../tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppOptionalAggregateHolder/README.md),
and [unions](../../tests/Wireloom.Dds.Generator.WireCompatibility/Native/CppUnions/README.md).
Exact outcomes belong to retained run reports and the
[versioned feature coverage](../corpus/7.7.0/FEATURE-COVERAGE.md).

[Documentation](../README.md) · [C# value semantics](generated-value-semantics.md)
