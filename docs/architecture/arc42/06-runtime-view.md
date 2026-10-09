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
cases that Wireloom can generate and for which RTI Connext DDS 7.7.0 can provide
reference peers, using generated C# and C++ sources or a runtime `DynamicType`
where needed. It builds three endpoints per case: Wireloom-generated C#,
RTI-generated C#, and RTI-generated C++. Every curated fixture runs in its own
process exchange on a unique topic for all nine directed sender/receiver
pairings. The report names both implementation and language for each endpoint,
plus case, fixture, runtime versions, and status. Readers compare the complete
expected sample after checking sample validity; successful discovery alone does
not count as a pass. Markdown has one status cell per pairing and a
problem/defect column; JSON retains the full structured metadata.

Fixtures include independently absent and present optional members, optional
empty/single/multiple collection values, union branch choices, string
boundaries, and contrasting values for the remaining supported shapes. The
catalog determines the scenario count, so every case can contribute several
independently reported scenarios. The `02-multiple` case wraps both top-level
declarations in one topic sample, and enum-only IDLs use a wrapper topic
member. All peers request reliable delivery. Data-representation cases select
XCDR2 explicitly in all applicable peers. Wireloom C# uses generated typed
readers and writers to exercise its conversion paths. RTI C# uses the pinned
code generator to produce a typed peer from the same IDL input; generated files
stay in scratch and are not uploaded. RTI C++ uses `DynamicData` except where
RTI omits required string template specializations; those cases use generated
typed readers/writers. For present optional wide-string sequence fixtures, the
C++ peer uses DynamicData and RTI's C API setters to avoid a crash in RTI
7.7.0's typed `std::wstring` sequence serializer. Absent, empty, and
narrow-only controls remain typed. Diagnostic output includes match counters,
read/take counts, sample validity, and selected type-consistency diagnostics.
Artifacts contain structured results and sanitized endpoint logs only.

The exact 7.7.0 workflow is a runtime evidence baseline, not evidence for other
RTI releases. RTI 7.3.1 remains a follow-up because the current generator emits
APIs absent from its C# package. Wireloom needs explicit target-compatibility
configuration and version-aware emission before that runtime can join the
matrix.

The RTI-positive `09-optional-aggregate-member` now generates successfully.
Its absent and present fixtures carry nested struct and union payload values,
including typedef aliases, and are scheduled as independent exchanges across
all nine directed pairings. The latest licensed matrix predates this
implementation, so those exchanges still need an exact RTI 7.7.0 run before
they count as interoperability evidence.
`05-array-of-sequences`
is included despite warning DDSG0105: Wireloom currently maps the C# members to
flat sequences while RTI C# and RTI C++ preserve the IDL arrays. Pairings that
include Wireloom C# fail endpoint discovery because RTI reports different
member type kinds. The report marks those Wireloom cross-peer failures as
expected; this case's wire test does not establish preservation of the two-slot
array shape. Aggregate alias members use the underlying
generated struct or union type in managed APIs and native conversions; for
optional struct members with aggregate values, DynamicType metadata retains
the declared typedef alias as RTI does. All four `03-alias-aggregate` and all
eight `07-union-aliases` exchanges pass. The expanded matrix reports its current
totals in generated results. The expected `05-array-of-sequences` failures
involve Wireloom C# pairings. The three-peer matrix uses DynamicData and RTI's
C API setters for present wide-string fixtures in `09-optional-string-sequences`
to exercise their intended wire shape without invoking RTI's crashing typed
serializer. Absent, empty, and narrow-only controls remain typed. C# verification
uses independent fixture values, and C++ controls check optional presence
separately from sequence length and values.
The wchar union's C++ peer uses a manually constructed RTI `DynamicType` because
`rtiddsgen` cannot parse wchar union labels. For `10-flat-data-binding`, C++
generation removes only the C#-ignored `@language_binding(FLAT_DATA)` annotation
while retaining the type shape and XCDR2 requirement; this does not validate
RTI's FlatData-specific C++ layout.
