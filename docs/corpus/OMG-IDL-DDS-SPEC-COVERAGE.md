# OMG IDL and DDS-XTypes coverage investigation

This investigation records gaps found by comparing Wireloom's tested IDL subset
with OMG IDL 4.2 and DDS-XTypes 1.3. It supplements the
[managed-generator corpus feature index](FEATURE-COVERAGE.md); it is not a claim
of complete conformance to either specification. Corpus acceptance and rejection
are evidence of Wireloom behavior, not proof that RTI or every DDS implementation
has the same behavior.

The [investigation issue](https://github.com/eNeRGy164/wireloom/issues/24)
requested coverage checks for common and extended features. The following
follow-up proposals are intentionally split by feature so they can be prioritized
and implemented independently.

## Verified coverage

The corpus has positive cases for primitive types, strings, arrays and
sequences, aggregates, valuetypes, enums, unions, constants, typedefs, and
selected DDS annotations. In particular:

- `C016`–`C018` exercise collection shapes.
- `C019`–`C021` exercise aggregates, valuetypes, and composed aliases.
- `C022`–`C029` exercise unions, including enum and selected primitive
  discriminators.
- `C038`–`C039` and `C114` exercise optional primitive, reference, collection,
  and string-sequence members.
- `C093` and `C094` record rejection of `bitmask` and `bitset`.
- `C058` records rejection of direct sequence-of-sequences.
- `C081` records rejection of forward aggregate declarations.
- `C082` records rejection of `long long` union discriminators.
- `C062` records rejection of `map`.

The example from issue #24 is within the existing subset: optional sequence
members are supported, as are union branches whose types are an aggregate or a
bounded string. A focused unit test exercises those features together.

## Proposed follow-up issues

### Support optional aggregate members

- **Spec:** [DDS-XTypes 1.3](https://www.omg.org/spec/DDS-XTypes/1.3/PDF),
  §7.2, Type System (`@optional` members).
- **Evidence:** `IdlTypeParser.ParseMember` rejects an optional member whose
  resolved type is an aggregate; `IdlValidationSpecs.RejectsOptionalAggregateMembers`
  asserts this behavior.
- **Minimal IDL:**

  ```idl
  struct Payload { long value; };
  struct Holder { @optional Payload payload; };
  ```

### Support direct sequence-of-sequences

- **Spec:** [OMG IDL 4.2](https://www.omg.org/spec/IDL/4.2/PDF), sequence type.
- **Evidence:** Corpus case `C058` is rejected; the member type grammar parses
  one sequence element type, not a nested sequence.
- **Minimal IDL:**

  ```idl
  struct Holder { sequence<sequence<long, 4>, 2> values; };
  ```

### Support forward aggregate declarations

- **Spec:** [OMG IDL 4.2](https://www.omg.org/spec/IDL/4.2/PDF), forward
  declarations and constructed types.
- **Evidence:** Corpus case `C081` is rejected.
- **Minimal IDL:**

  ```idl
  struct Payload;
  struct Holder { sequence<Payload> payloads; };
  struct Payload { long value; };
  ```

### Support 64-bit union discriminators

- **Spec:** [OMG IDL 4.2](https://www.omg.org/spec/IDL/4.2/PDF),
  §8.2.5, Union Declaration.
- **Evidence:** Corpus case `C082` rejects `long long` discriminators. The
  discriminator range and label representation in `IdlUnionBranch` are limited
  to 32-bit values; `C100` is a separate pending RTI probe for a 64-bit label.
- **Minimal IDL:**

  ```idl
  union Choice switch(long long) {
      case 2147483648: long value;
  };
  ```

### Support `map` types

- **Spec:** [DDS-XTypes 1.3](https://www.omg.org/spec/DDS-XTypes/1.3/PDF),
  §7.4.1, Map.
- **Evidence:** Corpus case `C062` rejects a map declaration.
- **Minimal IDL:**

  ```idl
  struct Lookup { map<string, long> values; };
  ```

### Support `bitset` types

- **Spec:** [DDS-XTypes 1.3](https://www.omg.org/spec/DDS-XTypes/1.3/PDF),
  §7.4.2, Bitset.
- **Evidence:** Corpus case `C094` rejects a bitset declaration.
- **Minimal IDL:**

  ```idl
  bitset Flags { bitfield<3> value; };
  ```

### Support `bitmask` types

- **Spec:** [DDS-XTypes 1.3](https://www.omg.org/spec/DDS-XTypes/1.3/PDF),
  §7.4.3, Bitmask.
- **Evidence:** Corpus case `C093` rejects a bitmask declaration.
- **Minimal IDL:**

  ```idl
  bitmask Flags { A, B, C };
  ```

## Scope notes

The corpus also records rejected `fixed` and `any` declarations, and warning-only
handling for interfaces. These are OMG IDL constructs, but this investigation
does not promote them to DDS topic-type support work: their applicability to
Wireloom's generated DDS data contracts needs a separate RTI/DDS-XTypes
compatibility probe. Likewise, unsupported syntax alone is not evidence that a
construct is valid for a DDS topic type.

The supplied issue example is:

```idl
module Example {
  struct Payload {
    @optional sequence<long> values;
  };
  union Choice switch(long) {
    case 0: Payload payload;
    case 1: string<32> label;
  };
};
```

Its optional sequence member and aggregate/string union branches are already
covered as independent features. Passing this combination does not imply support
for optional aggregate members, all union discriminator types, or all OMG IDL
and DDS-XTypes declarations.
