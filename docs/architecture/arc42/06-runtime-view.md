# 6. Runtime view

Wireloom has no long-running runtime process. Its primary runtime scenario is
the consumer's build, followed later by the generated application's use of the
RTI runtime.

## 6.1 Consumer build and generation

The source for the build-time sequence diagram is
[consumer-build-sequence.puml](images/06/consumer-build-sequence.puml).

1. MSBuild adds explicit `DdsIdl` roots and tracks other `.idl` files as
   non-generating inputs.
2. Roslyn supplies the files, item metadata, compiler options, and compilation
   references to `Generator`.
3. The generator requires C# 12 or later and a resolved RTI runtime reference of
   at least 7.3.1.
   The retained oracle corpus is captured with RTI 7.7.0 / `rtiddsgen` 4.7.0;
   this is an evidence baseline, not the minimum runtime version.
4. `IdlCompiler` resolves the include graph and preprocesses input. The front
   end then parses declarations, binds references and deferred bounds against
   the complete symbol graph, and validates semantics.
5. Emitters return generated C# documents to the central emission result. An invalid or unsupported input instead
   produces `DDSG0001` at the original IDL location.
6. The consumer compiles generated types together with its hand-written code.

## 6.2 Failure paths

- Missing or cyclic includes stop compilation with an IDL diagnostic.
- Unsupported syntax is rejected rather than silently dropped.
- An older language version produces `DDSG0002`.
- A missing compatible RTI runtime reference produces `DDSG0003`.
- Cyclic typedef aliases are rejected during semantic resolution with a source-
  located IDL diagnostic; they do not produce generated output.
- Cancellation is observed during input traversal and preprocessing. Macro-work
  or output limits fail the file with a stable source-located generator failure;
  partial preprocessed text is never passed to the parser.

## 6.3 Wire compatibility evidence

The manually dispatched wire-compatibility workflow targets positive corpus
cases that Wireloom can generate and for which RTI Connext DDS 7.7.0 can provide a peer,
using `rtiddsgen` or a runtime `DynamicType` where needed. It builds a
Wireloom-generated C# type and an RTI-generated C++ reference type for each
case. Every curated fixture runs in its own process exchange on a unique topic
for C#→C#, C++→C++, C#→C++, and C++→C#. The report records case, fixture,
writer and reader languages, endpoint RTI versions, and status. Readers compare
the complete expected sample after checking sample validity; successful
discovery alone does not count as a pass. The generated Markdown report groups
each fixture into one row with four pairing status cells and a problem/defect
column populated for failures; JSON retains the full structured metadata.

Fixtures include independently absent and present optional members, optional
empty/single/multiple collection values, union branch choices, string
boundaries, and contrasting values for the remaining supported shapes. The
catalog determines the scenario count, so every case can contribute several
independently reported scenarios. The `02-multiple` case wraps both top-level
declarations in one topic sample, and enum-only IDLs use a wrapper topic
member. Both peers request reliable delivery. Data-representation cases select
XCDR2 explicitly in both peers. C# uses generated typed readers and writers to
exercise Wireloom's generated conversion paths. The C++ reference peer uses
RTI `DynamicData` except where RTI omits needed string template
specializations; those cases use RTI-generated typed C++ readers/writers.
Diagnostic output includes match counters, read/take counts, sample validity,
and selected type-consistency diagnostics. Artifacts contain structured
results and sanitized endpoint logs only.

The exact 7.7.0 workflow is a runtime evidence baseline, not evidence for other
RTI releases. RTI 7.3.1 remains a follow-up because the current generator emits
APIs absent from its C# package. Wireloom needs explicit target-compatibility
configuration and version-aware emission before that runtime can join the
matrix.

Known case limitations remain visible in each result. The RTI-positive
`09-optional-aggregate-member` is excluded because Wireloom reports DDSG0001;
it will enter the wire matrix when generator support is implemented.
`05-array-of-sequences`
is included despite warning DDSG0105: Wireloom currently maps the C# members to
flat sequences while RTI C++ preserves the IDL arrays. Same-language controls
pass but cross-language endpoint discovery fails because RTI reports different
member type kinds. A focused RTI-generated C# oracle control reproduced this
shape limitation. This case's wire test therefore does not establish
preservation of the two-slot array shape. Aggregate alias members now use the
underlying generated struct or union type, and all four `03-alias-aggregate`
and all eight `07-union-aliases` exchanges pass. The optional-string-sequence fixtures expose RTI C++ interoperability failures for
present wide-string sequences: C++ readers time out on the wide-only and
multiple-value cases, while C# readers receive values that differ from the
fixture. Absent, narrow-only, and empty optional states pass. These are
recorded failures and diagnostics, not exclusions. The full catalog run
completed 364 of 372 scenarios; the report lists the eight failures by pairing.
The wchar union's C++ peer uses a manually constructed RTI `DynamicType` because
`rtiddsgen` cannot parse wchar union labels. For `10-flat-data-binding`, C++
generation removes only the C#-ignored `@language_binding(FLAT_DATA)` annotation
while retaining the type shape and XCDR2 requirement; this does not validate
RTI's FlatData-specific C++ layout.
