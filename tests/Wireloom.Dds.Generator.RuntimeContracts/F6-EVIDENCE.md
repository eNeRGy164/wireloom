# F6: numeric character union labels

## Finding

Numeric `char` union labels are valid IDL when the constant is in the
discriminator range, but both Wireloom and RTI Connext DDS 7.7.0 C# generation
emit integer expressions in contexts that require `char`. The generated C#
fails with CS0266. This is a shared C# generator defect, not a Wireloom-only
compatibility gap. RTI C++ generation accepts the same `char` fixture.

The original minimal reproduction was
[`numeric-character-labels.idl`](Fixtures/numeric-character-labels.idl):

```idl
union Narrow switch(char) { case 254: long value; };
```

Wireloom's opt-in build fails at generated managed and native/plugin sites,
including branch assignment and switch arms. RTI 7.7.0 / `rtiddsgen` 4.7.0
also generates `Discriminator = 254` and `case 254:` against a C# `char`; a
small consumer build against `Rti.ConnextDds` 7.7.0 fails with CS0266 at those
same kinds of sites. By comparison, RTI's C++ output uses the numeric constant
in the branch setter, switch cases, and type metadata. This probe confirmed
generation acceptance; the generated C++ was not compiled in this environment.

OMG IDL 4.2 permits a union case label to be a constant expression matching,
or automatically castable to, the discriminator type. Therefore `254` is a
valid label for `char`, whose range includes that value. [The IDL 4.2 union
definition](https://www.omg.org/spec/IDL/4.2/PDF) supports treating this as a
valid input, not rejecting it as a semantic error.

The same fixture also contains `union Wide switch(wchar) { case 937: ... }`.
Wireloom currently accepts this extension, but RTI 7.7.0 `rtiddsgen` rejects
`wchar` as a union discriminator before generating code. OMG IDL 4.2's listed
union discriminator types omit `wchar`, so this probe does not establish that
the wide-character form is standard or interoperable. Keep it as a separate
Wireloom-extension case.

## Repair and scope

The emission plan now renders numeric labels as explicit `(char)` constants
where generated managed code and native C# switches require them. DynamicType
metadata continues to use integral labels expected by RTI's `UnionMember` API.
Existing literal-label expressions remain unchanged. Regular runtime coverage
checks the high-byte labels `254` and `255`, a numeric `wchar` label `937`,
multiple-label branch selection, copy/equality/hash behavior, and native
round trips across allocation flag combinations.

F6 is resolved for Wireloom's generated runtime API. The RTI C# comparison
shows the same compile failure, while the standard and RTI C++ generator accept
the narrow `char` case, so this improves on the observed RTI C# baseline. RTI
still rejects `wchar` as a union discriminator; Wireloom's wide-character
coverage documents its extension behavior and does not claim RTI
interoperability. These tests establish generated-source compilation and
managed/native conversion, not DDS wire interoperability for numeric
character discriminators.

## Validation

- Focused runtime regressions: 4 passed, 0 failed, 0 skipped.
- Focused Linux native-runtime smoke: 4 passed on .NET 10.0.11, 0 failed, 0 skipped.
- Generator test suite: 1,244 passed, 0 failed, 0 skipped.
- Runtime contract suite: 495 passed, 0 failed, 0 skipped.
- Corpus compliance: 286 passed, 1 existing export-only skip.
- Maintained-source coverage: 4,726 / 5,026 branches (94.03%); 6,958 / 7,026 lines (99.03%). `ManagedDiscriminatorLabel` has 4/4 branch paths covered.
- Generated-fixture coverage: 1,331 / 2,298 branches (57.92%); 5,416 / 7,678 lines (70.54%); 521 generated classes. The coverage verifier passed.
- Community Qodana: exit 0, zero problems, `Done`.
- RTI C# baseline: `rtiddsgen -replace -ppDisable -language 'C#'` generated the original `char`-only fixture; a consumer build against `Rti.ConnextDds` 7.7.0 reproduced CS0266.
- RTI C++ baseline: `rtiddsgen -replace -ppDisable -language C++11` accepted the same `char`-only fixture and emitted numeric label expressions. The generated C++ was not compiled in this environment.
- RTI C# and C++ generation used Connext DDS 7.7.0 / `rtiddsgen` 4.7.0 on Windows. The Wireloom tests used the repository's pinned .NET 10 SDK.

The CI Coveralls check still requires a pushed revision and completed workflow.
