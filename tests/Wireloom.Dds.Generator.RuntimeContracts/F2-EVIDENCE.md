# F2: RTI C# alias formatting comparison

In the initial October 15, 2026 comparison, RTI-generated C# reproduced all 15 F2 formatting
failures. This finding is a shared RTI formatting-path limitation, rather than
evidence that Wireloom's formatting delegation differs from RTI. Successful
formatting remains the desired managed contract; reproducing RTI exceptions
does not make those exceptions mandatory Wireloom behavior.

## Method and scope

RTI Connext DDS 7.7.0.1 / rtiddsgen 4.7.0.1 generated C# directly from the
existing [alias fixture](Fixtures/aliases.idl), without edits to the generated
files. Both generated source and plugin compiled against the locked
`Rti.ConnextDds` 7.7.0 dependency in the existing `RtiOracleProbe` project.

The [typed formatting probe](../Wireloom.Dds.Generator.WireCompatibility/RtiOracleProbe/AliasFormattingProbe.cs)
executes the same 15 populated shapes as `RuntimeGapSpecs.FormatsAliasValues`,
plus a struct and two struct-alias controls. It runs separately against RTI
and freshly emitted Wireloom sources. The results below describe the initial
pre-fix comparison. There is no runtime Roslyn compilation
or reflection-based invocation. Reports record returned strings and complete
exception chains; native diagnostics remain in the logs.

Both generators emit the same delegation:

```csharp
public override string ToString() => ScalarSupport.Instance.ToString(this);
```

This calls the generated type's support instance, which uses RTI's
`TypePluginHarness<TManaged, TNative>.ToDynamicData` formatting path.

## Observed results

Windows (.NET 10.0.12) and Linux (.NET 10.0.11) produced matching outcomes and
exception types for each implementation. Each of the four runs recorded
18 observations: 15 failures and three successful controls. These are
observations, not 18 passing runtime contracts.

| Shapes                                                        | RTI C# outcome                                                                                     | Wireloom outcome                                                                                                       |
| ------------------------------------------------------------- | -------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------- |
| `scalar`, `scalar-chain`, `enum`, `enum-chain`, `text`        | `Omg.Dds.Core.DdsException` during DynamicData construction                                        | Same                                                                                                                   |
| `wide-text`                                                   | `Omg.Dds.Core.DdsException` during DynamicData construction, after support initialization succeeds | F3 first: `TypeInitializationException` → `TargetInvocationException` → `DdsException` in `DynamicType.SetAnnotations` |
| `numbers`, `numbers-chain`, `children`, `texts`, `wide-texts` | `Omg.Dds.Core.PreconditionNotMetException` from `DynamicData.FromCdrBuffer`                        | Same                                                                                                                   |
| `vector`, `matrix`, `matrix-chain`, `points`                  | `Omg.Dds.Core.PreconditionNotMetException` from `DynamicData.FromCdrBuffer`                        | Same                                                                                                                   |
| Struct, struct alias, struct alias chain                      | Return readable member values, including `id: 17` and `text: "hello"`                              | Same                                                                                                                   |

RTI's native diagnostics identify invalid DynamicData root kinds `DDS_TK_LONG`,
`DDS_TK_ENUM`, `DDS_TK_STRING`, and `DDS_TK_WSTRING` for the first group.
The collection diagnostics state that `from_cdr_buffer` is unsupported for
root kinds `DDS_TK_SEQUENCE` and `DDS_TK_ARRAY`. These diagnostics and stack
traces locate the failing path; they do not establish that every RTI formatting
API or every collection use is unsupported. Collection members inside a struct
are outside this probe.

F3 is separate: RTI's wide-string alias plugin supplies `WideStringValue` and
`TypeKind.WideString`; Wireloom currently supplies the narrow-string forms.
Correcting F3 would let this shape reach the F2 formatting limitation.

## Reproduction

Run from the repository root. The RTI image is the existing licensed toolchain;
the probe creates no DDS participants and performs no wire exchanges.

```powershell
New-Item -ItemType Directory -Force tmp/rti-alias-formatting | Out-Null
wslc run --rm -v "${PWD}:/work" -w /work rticom/connext-base:7.7.0.1 `
  rtiddsgen -replace -ppDisable -language 'C#' -d /work/tmp/rti-alias-formatting `
  /work/tests/Wireloom.Dds.Generator.RuntimeContracts/Fixtures/aliases.idl

$probe = 'tests/Wireloom.Dds.Generator.WireCompatibility/RtiOracleProbe/RtiOracleProbe.csproj'
dotnet restore $probe --locked-mode
dotnet run --project $probe --no-restore --configuration Release `
  -p:RtiOracleProgram=AliasFormattingProbe.cs `
  -p:RtiOracleSourceDir=../../../tmp/rti-alias-formatting -- `
  tmp/rti-alias-formatting/rti-windows.json
```

For the Wireloom comparison, use a fresh output directory so old emitted
fixtures cannot contaminate the probe. Build the generator normally first;
limit the output-directory override to the runtime project.

```powershell
dotnet build src/Wireloom.Dds.Generator --no-restore --configuration Release
$generated = Join-Path $PWD 'tmp/wireloom-alias-formatting'
New-Item -ItemType Directory -Force $generated | Out-Null
dotnet build tests/Wireloom.Dds.Generator.RuntimeContracts --no-restore `
  --configuration Release --target:Rebuild -p:BuildProjectReferences=false `
  "-p:CompilerGeneratedFilesOutputPath=$generated"
dotnet run --project $probe --no-restore --configuration Release `
  -p:RtiOracleProgram=AliasFormattingProbe.cs `
  -p:RtiOracleSourceDir=../../../tmp/wireloom-alias-formatting/Wireloom.Dds.Generator/Wireloom.Generator -- `
  tmp/rti-alias-formatting/wireloom-windows.json
```

Linux used the same source pairs and probe under the cached .NET container,
with separate Linux `obj`/`bin` mounts and the existing NuGet cache. Reports
are retained locally under `tmp/rti-alias-formatting/{rti,wireloom}-{windows,linux}.json`.
The optional positional argument writes JSON directly to a file so native
diagnostics on standard output cannot corrupt the report. The probe collects
exceptions deliberately; exit zero indicates it completed the observations,
not that formatting succeeded. Inspect the outcomes and successful controls.

The initial investigation changed no generator behavior. The subsequent F2
repair implements the contract below, beyond the observed RTI C# baseline.
Its reproduction is now covered by normal typed runtime tests. This evidence makes no serialization or DDS wire
compatibility claim, and does not establish behavior in other RTI versions.

## C++ equivalent

A follow-up probe generated C++11 from the same IDL with rtiddsgen 4.7.0.1
and compiled it against the installed RTI C++ 7.7.0.0 libraries on Linux.
All 15 F2 shapes printed successfully through stream insertion (`operator<<`).
The authored [C++ probe](../Wireloom.Dds.Generator.WireCompatibility/RtiOracleProbe/AliasFormattingProbe.cpp)
includes RTI's `rti/util/ostream_operators.hpp` for standard collection and
wide-string stream support. The generated files were unmodified.

C++ typedefs are aliases of underlying C++ types, rather than separate wrapper
classes with instance methods. RTI's generated enum/struct stream operators
and its collection stream helpers provide the relevant formatting path.
The separate [topic-level `rti::topic::to_string` API](https://community.rti.com/static/documentation/connext-dds/current/doc/api/connext_dds/api_cpp2/group__DDSTopicTraitsModule.html)
requires a valid topic type. All 15 scalar/enum/string/collection F2 shapes
reported `dds::topic::is_topic_type<T>::value == false`; the aggregate alias
control reported true. Calling that topic API on `Child` returned the same
multiline member notation seen in the C# controls.

| Shape                   | Observed C++ stream text                         |
| ----------------------- | ------------------------------------------------ |
| Scalar / scalar chain   | `17`                                             |
| Enum / enum chain       | `State::Ready ` (trailing space)                 |
| Narrow string           | `hello`                                          |
| Wide string             | `4e16754c`                                       |
| Number sequence / chain | `{17}`                                           |
| Child sequence          | `{[id: 17, text: hello]}`                        |
| Narrow string sequence  | `{hello}`                                        |
| Wide string sequence    | `{\u4e16\u754c}`                                 |
| Vector                  | `{17, 18}`                                       |
| Matrix / chain          | `{{17, 18, 19}, {20, 21, 22}}`                   |
| Points                  | `{[id: 17, text: hello], [id: 18, text: other]}` |

The exact wide-string spelling differs between the standalone string and
collection helper. These are observed C++ diagnostics, not proposed mandatory
C# output. The successful C++ path supports making managed alias formatting
usable; no DDS transport or serialization conclusion follows from it.

To reproduce after generating the C# fixture above:

```powershell
New-Item -ItemType Directory -Force tmp/rti-alias-formatting/cpp | Out-Null
wslc run --rm -v "${PWD}:/work" -w /work rticom/connext-base:7.7.0.1 `
  rtiddsgen -replace -ppDisable -language C++11 -d /work/tmp/rti-alias-formatting/cpp `
  /work/tests/Wireloom.Dds.Generator.RuntimeContracts/Fixtures/aliases.idl
Copy-Item tests/Wireloom.Dds.Generator.WireCompatibility/RtiOracleProbe/AliasFormattingProbe.cpp tmp/rti-alias-formatting/cpp/main.cpp
Copy-Item tests/Wireloom.Dds.Generator.WireCompatibility/Native/CMakeLists.txt tmp/rti-alias-formatting/cpp/CMakeLists.txt
wslc run --rm -v "${PWD}:/work" -w /work --entrypoint /bin/bash wireloom-rti-7.7.0 -lc `
  'cmake -S tmp/rti-alias-formatting/cpp -B tmp/rti-alias-formatting/cpp/build -DCMAKE_MODULE_PATH="$NDDSHOME/resource/cmake" -DCONNEXTDDS_ARCH=x64Linux4gcc8.5.0 && cmake --build tmp/rti-alias-formatting/cpp/build --parallel 2 && tmp/rti-alias-formatting/cpp/build/wire-peer'
```

## Implemented Wireloom output contract

Aliases should display their underlying values, and alias chains should add
no wrapper text. Use invariant numeric formatting, enum member names,
readable Unicode, escaped/quoted string elements in collections, and nested
brackets preserving array dimensions. An empty collection would be `[]`;
a null underlying reference would be `null`. Keep existing successful
aggregate formatting behavior.

Examples: `17`, `Ready`, `hello`, `世界`, `[17, 18]`, `["hello"]`, `["世界"]`,
and `[[17, 18, 19], [20, 21, 22]]`. Aggregate elements retain their original
RTI formatter, including its multiline member layout. `LongDouble` retains
RTI's decimal conversion with invariant output. Boolean and character values
use their .NET value representations (`True` and `x`, for example).

`AliasFormattingSpecs` asserts exact punctuation, escaping, Unicode, null/empty
handling, culture independence, collection order, alias chains, and array
dimensions separately from the RTI characterization. It also compares aggregate
output directly with the original RTI support formatter. The two remaining
skipped runtime contracts are F4/F5. F3's wide-string annotation correction is
included because the original support instance must initialize before its
formatter can be used.

Collection classes contain a one-line delegate, such as:

```csharp
public override string ToString() => NumbersSupport.Instance.ToString(this);
```

The existing `NumbersSupport.g.cs` contains
`public new string ToString(Numbers sample)`. String escaping stays with the
support class as a private helper. No extra generated companion, extension
method, shared runtime library, reflection, or dependency is needed.

RTI's [`TypeSupport<T>.ToString(T sample)`](https://community.rti.com/static/documentation/connext-dds/current/doc/api/connext_dds/api_csharp/classRti_1_1Dds_1_1Topics_1_1TypeSupport.html)
is nonvirtual. The generated method hides it with the same concrete alias
parameter. Calls through the generated support type use the managed formatter;
calls through a base `TypeSupport<T>` reference and explicit print-format
overloads retain RTI behavior. A separate runtime characterization demonstrates
that distinction. Null samples throw `ArgumentNullException`; a null value
inside a non-null sample formats as `null`.

The original support factory, singleton property, constructors, and managed
alias delegates are preserved. F3's narrow annotation kind/value on wide-string
aliases is corrected to RTI's `WideString` forms, so support initialization
succeeds. The original RTI source remains unchanged. Serialization and DDS
wire compatibility still require their own evidence.

An equivalent-case check found F8: string-array typedefs emit invalid native
generic calls (`CS8377`). The isolated
`KnownFailures/string-array-aliases.idl` reproduction retains that separate
gap. Formatting emission for those shapes is source-tested, but no compiled
runtime string-array support is claimed.
