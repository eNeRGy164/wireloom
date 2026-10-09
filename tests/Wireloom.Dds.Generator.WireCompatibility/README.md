# Wire compatibility peers

This project compares Wireloom-generated C# with RTI-generated C# and C++ DDS
peers for selected positive corpus cases. RTI Connext 7.7.0 is the initial wire
baseline. The peer package version is selected by
`WireCompatibilityRtiVersion` (default `7.7.0`); use the matching exact RTI
toolchain to generate and build the reference peers.

Each case has a curated fixture catalog. Every fixture is sent in a separate
producer/consumer exchange with its own topic and report row across all nine
directed pairings of Wireloom C#, RTI C#, and RTI C++. Fixtures contrast optional presence per member,
empty/single/multiple sequence lengths, union branches, and string boundaries
where the IDL allows them. Readers compare the complete generated sample with
the fixture and report valid-sample and discovery counters. The JSON report
records case, fixture, pairing, endpoint runtime versions, and pass/fail status.
The Markdown report presents one row per case and fixture, with a status cell
for each pairing. For failures, its problem/defect column reports the observed
sanitized symptom and lists any registered expected-failure explanation
separately. The JSON keeps these in `observedProblem` and `expectedFailure`.

Wireloom-generated C# types and RTI-generated typed C# readers/writers exercise
generated conversions and DDS transport. The RTI C# peer is generated from the
same IDL with the pinned code generator, and generated files remain in scratch.
The RTI C++ reference peer normally uses `DynamicData`; typed generated C++ is
used where RTI omits DynamicData string template specializations. For present
optional wide-string sequence fixtures, the C++ peer uses DynamicData and RTI's
C API setters to avoid a crash in RTI 7.7.0's typed `std::wstring` sequence
serializer. Absent, empty, and narrow-only controls remain typed. All peers
request reliable delivery, and representation cases select XCDR2 explicitly.
## Implementation map for C# contributors

Wireloom's generated C# type is the implementation under test. The shell
runner builds the Wireloom C# peer, asks `rtiddsgen` for RTI C# and C++ reference
peers, and launches independent exchanges. Expected fixture assignments exist in
`FixtureCatalog.cs` and the native fixture helpers; keep values, optional
states, branch selection, and member paths aligned when changing a fixture.
`Native/main.cpp.in` owns the DynamicData C++ peer, while
`Native/typed-fixture-peer.hpp.in` handles typed C++ string cases that
DynamicData cannot populate.
`RtiOracleProbe/probe.sh` remains a focused diagnostic helper outside this
matrix.

The original baseline covered 224 scenarios (one fixture per case). The
expanded matrix adds independent exchanges for fixture variants, so its total
is computed from the fixture catalog and recorded as `expectedScenarioCount`
and `scenarioCount` in `results.json`; the human-readable `results.md` includes
✅/⛔/❔ status cells and a problem/defect column for failed exchanges. Known
expected failures remain ⛔; the problem column separates observed symptoms
from their expected-failure explanations, and `results.json` records both
fields and their aggregate count.
Cases that Wireloom cannot generate appear as not-implemented rows in both
reports. Their pairing cells show ⛔, while the problem column identifies the
diagnostic and links the tracked support issue. They are not counted as
executed DDS exchanges or as observed interoperability failures.
The IDL scenario cell uses `"` to repeat the case shown in the preceding row,
keeping fixture variations grouped without repeating the same filename.
Wireloom-positive selection is
independent of RTI oracle status. See the latest report for current outcomes;
the findings below remain known limitations until their related behavior is
fixed.

`05-array-of-sequences` remains in the matrix so its known cross-language
failure stays visible. DDSG0105 warns that its generated C# binding flattens
the IDL array shape, and RTI rejects the resulting C# and C++ types during
cross-language discovery. The report marks those failures as expected; the
same-language controls remain useful evidence. This is an unsupported
Wireloom mapping rather than a negative IDL syntax case.
`09-optional-aggregate-member` includes absent and present fixtures for an
optional struct value and an optional union value whose selected branch
contains a nested payload, including typedef aliases. Wireloom now generates
these values; each fixture is scheduled across all nine directed pairings, and
its wire outcomes remain pending an exact RTI 7.7.0 run.
`03-alias-aggregate` and `07-union-aliases` pass their fixture pairings with the
current aggregate-alias projection. The three-peer matrix reports current
scenario totals in its generated results. Expected `05-array-of-sequences`
failures involve Wireloom C# pairings because its generated binding flattens
the IDL array shape; RTI C# and C++ provide independent reference pairings. For
present wide-string fixtures in `09-optional-string-sequences`, the C++ peer
uses DynamicData and RTI's C API setters to exercise the same wire shape without
invoking RTI's crashing typed serializer. Absent, empty, and narrow-only
controls remain typed. The report identifies peer adaptations and expected
failures per scenario.
For the wide-character union, the
C++ peer uses a manually constructed RTI `DynamicType` that preserves the
corpus discriminator, labels, member IDs, and extensibility because
`rtiddsgen` cannot parse `wchar` union labels. For the C#-ignored
`@language_binding(FLAT_DATA)` annotation, C++ peer
generation drops only that annotation while preserving the type shape and
XCDR2 requirement; the result report records this adaptation. This verifies
standard C++ wire mapping, not RTI's FlatData-specific C++ layout. Consumers
verify received data against the expected fixture, not only endpoint
discovery.

To compare the retained RTI-generated C# binding directly, run
`RtiOracleProbe/probe.sh` inside the helper container with the repository
mounted at `/workspace`, `GITHUB_WORKSPACE=/workspace`, and the RTI license
mounted read-only at `/run/secrets/rti_license.dat`. It compiles the retained
RTI C# oracle sources and pairs them with an RTI-generated C++ peer. This
control passed C#↔C# and reproduced both cross-language discovery failures,
with RTI reporting that the member types have different type kinds.

The runner writes `artifacts/wire-compatibility/results.md` and
`results.json`; these generated files are local run output and are not
committed. The manually triggered GitHub Actions workflow uploads them as the
`wire-compatibility-rti-7.7.0` artifact, even when a build or exchange fails.
The manually triggered workflow runs all wire-testable
Wireloom-positive corpus cases and all nine same-version directed pairings,
with each fixture as an independent scenario, using the immutable exact RTI
7.7.0 image. Configure the repository secret
`RTI_CONNEXT_LICENSE` with the license contents before dispatching it. The
secret is written to a runtime-only file inside the job and excluded from
artifacts. The workflow retains the structured and Markdown results plus status-only
sanitized endpoint logs for 14 days. Each scenario result records its fixture
kind; C++ endpoint logs additionally record match counters and reader sample
counters.

For local runs, build the helper image once from the repository root:

```powershell
wslc build -f tests/Wireloom.Dds.Generator.WireCompatibility/Containerfile `
  -t wireloom-rti-7.7.0 .
```

The image pins the exact RTI 7.7.0 base image and installs .NET 10.0.400 plus
the C++ build tools, including ccache. GitHub Actions builds this same image
with BuildKit's GitHub Actions cache, scoped to RTI 7.7.0, so its package
installation and .NET setup layers can be reused on later runs. The immutable
RTI base image still needs to be pulled by each fresh runner. The image does
not contain a license. The shell harness uses the .NET 10 C# file-based utility
for manifest reading, type discovery, IDL include discovery, and report
generation; Python is not required. The matrix step succeeds when it generates
the report, even if peer builds or exchanges fail; those outcomes are recorded
as failed or not-run rows for review. Setup checks that prevent report
generation still fail the workflow. Run the container
with the repository mounted at `/workspace`, the license mounted read-only at
`/run/secrets/rti_license.dat`, and `WIRE_COMPATIBILITY_CASE_FILTER=05-collections`
for the focused non-default fixture. Omit the filter to run all 56
wire-testable Wireloom-positive cases.
Generated build outputs and artifacts are written to the mounted repository;
the runner always checks its selected RTI version against the supported initial
baseline.

## Build and discover one C# case

From the repository root:

```powershell
dotnet build tests/Wireloom.Dds.Generator.WireCompatibility/Wireloom.Dds.Generator.WireCompatibility.csproj `
  -c Release `
  -p:WireCompatibilityCase=01-primitives `
  -p:WireCompatibilityRtiVersion=7.7.0

python tests/Wireloom.Dds.Generator.WireCompatibility/scripts/discover_cases.py `
  docs/corpus/manifest.json `
  tests/Wireloom.Dds.Generator.WireCompatibility/obj/wireloom-generated `
  tests/Wireloom.Dds.Generator.WireCompatibility/obj/cases.json `
  01-primitives
```

The discovery helper resolves the generated C# and corresponding C++ type name
from Wireloom output and the corpus manifest. It can select one case for local
smoke runs or all positive cases after the case outputs have been built.

## Run two local C# processes

Make the RTI license available to the process through `RTI_LICENSE_FILE`, then
start one peer with `--role reader` and another with `--role writer`. Give both
the same unique topic and pass the case, generated type name, and runtime
version. Both endpoints must exit successfully before recording a pass.

The RTI package version and future Wireloom generator target API version are
separate concepts. When Wireloom adds target-version-aware emission, the
workflow should pass that target explicitly instead of inferring it from the
runtime package. This enables testing an older wire endpoint without silently
changing which RTI APIs Wireloom emits.
