# IDL fuzzing

Wireloom fuzzes the IDL compiler with FsCheck in
[`FsCheckIdlCompilerFuzzingSpecs`](../../tests/Wireloom.Dds.Generator.Tests/Fuzzing/FsCheckIdlCompilerFuzzingSpecs.cs).
The property selects structured IDL strings derived from valid seeds and
bounded insert/delete/replace mutations. Each case must either compile or be
rejected by the compiler's expected `IdlException` boundary; an unrelated
exception fails the property and FsCheck reports the reproducing input.

Run the CI-facing fuzzing pass with:

```powershell
dotnet test tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj `
  --no-restore --configuration Release -- --filter-class Wireloom.Dds.Generator.Tests.FsCheckIdlCompilerFuzzingSpecs
```

Set `WIRELOOM_FSCHECK_TESTS` to change the number of generated cases; the
default is 1,000 locally and CI runs 10,000 cases. The generator combines 15
seeds with zero, one, or two bounded mutations, selecting mutation positions
and characters independently. The seeds cover declarations, nesting, aliases,
collections, enums, unions, preprocessing, annotations, and inheritance. This
is still a bounded property test rather than a grammar-complete IDL generator.
