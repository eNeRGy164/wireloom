# Generated C# quality comparison

This report measures compiler diagnostics and Qodana inspections in Wireloom
output and the retained RTI C# oracle output. It supports the README's C#
tooling claims. These checks do not measure serialization, interoperability,
or every analyzer's rules. Compiler diagnostics and Qodana findings are
reported separately.

## Qodana results

Community Qodana 2026.2.691 completed successfully using
`jetbrains/qodana-cdnet:2026.2-privileged` and the `qodana.recommended` profile.
The Wireloom scan was measured on October 10, 2026, from revision
`c6ebd9dc22a64f514214cb45518b5f63ae6e2029`. The retained RTI scan was measured
on October 9, 2026. Both used C# 12, .NET 10, XML documentation generation,
and `Rti.ConnextDds` 7.7.0.

### Raw findings

| Measure             | Wireloom | Retained RTI output |
| :------------------ | -------: | ------------------: |
| Corpus cases        |       67 |                  65 |
| C# source files     |      533 |                 130 |
| Errors              |        0 |                   2 |
| Warnings            |      915 |               5,994 |
| Suggestions / notes |      375 |               2,548 |
| All findings        |    1,290 |               8,544 |

The Wireloom column covers all accepted cases. The RTI column covers the 65
cases with output from both generators. Wireloom's additional two cases have
no retained RTI C# output. File counts differ because Wireloom separates
declarations into generated documents.

The staged Wireloom host projects disabled nullable analysis so generated
files could establish their own nullable context. The RTI host projects enabled
it. These scope and configuration differences mean the table is not a basis
for a percentage reduction. In particular, Wireloom's reviewed warning count
below must not be compared directly with RTI's raw total, which includes
errors, warnings, and notes.

### Wireloom warning review

Three inspection groups reflect intentional generated-code choices and are
excluded from the remaining-warning count. All findings remain in the raw
SARIF.

| Inspection                                         | Count | Reason for exclusion                                                                                                                                                |
| :------------------------------------------------- | ----: | :------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Redundant name qualifier beginning with `global::` |   553 | Fully qualified names protect against IDL declarations shadowing framework or generated types.                                                                      |
| Non-readonly member referenced in `GetHashCode()`  |   235 | The mutable model hashing pattern preserves RTI parity. Both implementations have 231 findings in the shared cases; Wireloom has four more in its additional cases. |
| Partial type with a single part                    |   121 | Partial data classes are an application extension point; the isolated projects contain no application-authored parts.                                               |

After these exclusions, **6 warnings remain**:

| Inspection                                               | Count | Assessment                                                                                                                                                |
| :------------------------------------------------------- | ----: | :-------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Parameter hides member                                   |     5 | Retained generated parameter/member names; no change is planned from this report.                                                                         |
| Condition always false according to nullable annotations |     1 | The recursive support `Instance is null` guard handles a real initialization-time state and must remain.                                                  |

The recursive support guard is required because constructing its plugin can
reenter the same support factory during dynamic-type creation, before the
static `Instance` assignment has completed. The backing field can be null
during that reentry despite the property's non-nullable annotation.

The scan reports no redundant imports, explicit array types, casts, method
type arguments, non-global qualifiers, or nullable return-type suggestions.
The 375 notes are separate from the warning count. Unused public API
suggestions partly reflect the absence of
application consumers in these isolated projects.

### Qodana measurement method

1. Export fresh Wireloom sources through the corpus export test. Copy all
   exported C# files and retained RTI oracle sources into separate temporary
   scan roots. Preserve source paths, case IDs, removed headers, and SHA-256
   hashes in an inventory.
2. Remove only the leading generated-code comment header and rename `.g.cs`
   to `.cs`. Preserve XML documentation, nullable directives, and source code.
   Set `generated_code = false` in the copied `.editorconfig` so Qodana inspects
   the staged copies as authored sources.
3. Place each corpus case in its own project to avoid collisions between shared
   IDLs. The two RTI cases importing `common.idl` reference a separate common
   project. Wireloom emits the included declarations within each case.
4. Reference `Rti.ConnextDds` 7.7.0 in every staged project. Keep the repository
   `global.json` visible; the container used SDK 10.0.400. Disable implicit
   usings, allow unsafe code, and generate XML documentation. Host nullable
   settings differ as described above. The runtime package and compile asset
   were verified in the restored projects.
5. Run Qodana with separate caches and result directories. Preserve retained
   oracle errors rather than editing those sources. The RTI scan disabled the
   partial-class inspection; the Wireloom raw results retain that group for
   explicit exclusion during review.
6. Match SARIF primary source locations to the staged inventory. Count errors,
   warnings, and notes separately. For Wireloom's reviewed warning count,
   exclude only the three inspection groups listed above; identify collision-safe
   qualifiers by SARIF-highlighted text beginning with `global::`.

## Compiler results

Measured on October 9, 2026, from source revision
`e751e4a44da1f34a895046bc350a7e77163ad09b` using .NET SDK 10.0.401 (allowed by
the repository's 10.0.400 pin and roll-forward policy), C# 12, .NET 10 reference
assemblies, and `Rti.ConnextDds` 7.7.0.

| Check across the same 65 corpus cases         | Wireloom | Retained RTI output |
| :-------------------------------------------- | -------: | ------------------: |
| Nullable compiler warnings                    |        0 |                 455 |
| Missing XML documentation warnings (`CS1591`) |        0 |               2,231 |
| All compiler warnings                         |        0 |               2,686 |
| Compiler errors                               |        0 |                   2 |

The RTI errors are both `CS1061` in `06-alias-composition`: the retained plugin
calls `FromNative` and `ToNative` on `Named`, which does not expose those
methods. The table retains that case rather than excluding a failing oracle.
This is an observation about the captured sources and this harness, not a claim
about every RTI release or generator configuration.

The nullable warning breakdown is:

| Diagnostic | Count |
| :--------- | ----: |
| `CS8603`   |     6 |
| `CS8604`   |   117 |
| `CS8618`   |    95 |
| `CS8625`   |     7 |
| `CS8765`   |   113 |
| `CS8767`   |   117 |

The complete Wireloom export contains 67 accepted cases and 533 C# documents.
All 67 compiled without errors or warnings under these checks. The shared
comparison covers 65 cases and 528 Wireloom documents versus 130 RTI files.
`07-union-wchar-label` and `12-macro-arity-linux` have no retained RTI C# output
and are excluded from the shared comparison. Each implementation is measured
over the complete source set for a case, including its support/plugin code;
file counts differ because Wireloom separates declarations into documents.

The corpus compliance run also passed **239 of 239 checks**, with the export
test enabled and no skips. This includes expected acceptance, public shape
comparison, optional aggregate generation, and well-formed generated XML
documentation. Compliance and compiler diagnostic checks are separate evidence.

### Compiler measurement method

1. Restore the existing corpus project in locked mode and export fresh managed
   output to an empty scratch directory:

   ```powershell
   dotnet restore tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --locked-mode
   $env:CORPUS_GENERATOR_EXPORT_ROOT = Join-Path $PWD '.tmp/readme-corpus'
   dotnet run --project tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj --no-restore --configuration Release
   Remove-Item Env:CORPUS_GENERATOR_EXPORT_ROOT
   ```

2. For each exported case, read all its `.g.cs` documents and locate the matching
   retained oracle directory under `oracles/`. Compile each implementation's
   source set separately with the SDK's Roslyn assemblies. No generator or
   oracle sources are edited.
3. Parse with `LanguageVersion.CSharp12` and `DocumentationMode.Diagnose`.
   Prepend `#nullable enable` to each in-memory source to apply nullable checking
   equally, including to files that compilers recognize as generated. Use
   `CSharpCompilationOptions` with `OutputKind.DynamicallyLinkedLibrary`,
   `NullableContextOptions.Enable`, and `allowUnsafe: true`, leaving the warning
   level at its default of 4. No warnings are suppressed.
4. Reference the .NET 10.0.12 reference assemblies and the RTI 7.7.0
   `netstandard2.0` assembly. First compile the retained `oracles/includes`
   source/plugin pair into a reference assembly. Reference it only for the RTI
   `11-include-search` and `11-preprocessing` cases: Wireloom already emits their
   included declarations, so adding that reference to Wireloom would introduce
   artificial duplicate-type warnings.
5. Count warning/error diagnostics returned by `CSharpCompilation.GetDiagnostics()`
   by case and diagnostic ID. Aggregate both implementations over the same case
   IDs for the comparison table. Count the two additional Wireloom cases
   separately.

These are compiler results with nullable and XML documentation checking enabled.
The separate Qodana scan above measures additional ReSharper inspections after
removing generated-code exclusions. StyleCop, custom analyzers, and arbitrary
consumer lint policies were not run against both output sets. The zero-warning
claim applies to the measured compiler checks, not universal linter silence.

## Package size

The local `Wireloom.Dds.Generator.0.3.0.nupkg` measured **169,012 bytes**, or
about **170 kB** (165 KiB). Its SHA-256 was
`508a704e6f3bb403f138ae9237639a7e894040d09b391baf97c7bc49f0edfbd3`.
This is a local package measurement, not an independently verified NuGet.org
artifact size. The generator package excludes the .NET SDK and RTI runtime;
release package sizes can change.

RTI's [example installation guide](https://www.rti.com/hubfs/_Collateral/Simplified-Real-Time-Data-Sharing.pdf)
lists approximately 2 GB for Connext in a Windows setup using Connext
Professional 6.1.1/6.1.2 or Drive 2.0.1. It is an illustration of the full
development installation footprint, not a measurement of `rtiddsgen` alone or
of the current 7.7.0 installation. Installed SDK size and compressed generator
package size are different measures; no size ratio is claimed.
