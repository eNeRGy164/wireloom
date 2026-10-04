# IDL fuzzing

Wireloom fuzzes the public IDL compiler and Roslyn generator adapter with FsCheck in
[`FsCheckIdlCompilerFuzzingSpecs`](../../tests/Wireloom.Dds.Generator.Tests/Fuzzing/FsCheckIdlCompilerFuzzingSpecs.cs).
The property selects structured IDL strings derived from valid seeds and
bounded insert/delete/replace mutations. The compiler property exercises the
public `IdlCompiler.CompileSources` overload and `IdlInput` options. The
generator property sends the same cases through the Roslyn adapter with
build-style `Generate`, `Strict`, `Defines`, `Undefines`, and
`IncludeDirectories` metadata. Each case must either compile or be rejected by
the compiler's expected `IdlException` boundary; an unrelated exception fails
the property and FsCheck reports the reproducing input.

The NuGet MSBuild target is a separate integration surface. It is exercised by
the packed-package consumer in
[`PackageConsumptionSpecs`](../../tests/Wireloom.Dds.Generator.PackageIntegration/PackageConsumptionSpecs.cs),
which verifies `DdsIdl` roots, project and per-root include directories,
additional-file handling, and per-root include tracking. A full MSBuild build
is intentionally not launched for every fuzz case.

Run the CI-facing fuzzing pass with:

```powershell
dotnet test tests/Wireloom.Dds.Generator.Tests/Wireloom.Dds.Generator.Tests.csproj `
  --no-restore --configuration Release -- --filter-class Wireloom.Dds.Generator.Tests.FsCheckIdlCompilerFuzzingSpecs
```

Set `WIRELOOM_FSCHECK_TESTS` to change the number of generated cases per
property; the default is 1,000 per property locally, and CI runs 10,000 per
property (20,000 total). The generator combines 15 seeds with zero, one, or
two bounded mutations, selecting mutation positions and characters
independently, including the end-of-input position for insertion mutations.
The seeds cover declarations, nesting, aliases,
collections, enums, unions, preprocessing, annotations, and inheritance. This
is still a bounded property test rather than a grammar-complete IDL generator.

## Defects exposed by the fuzzing gate

The fuzzing gate has exposed and led to regression tests for:

- malformed union members and bounded string sequences that previously allowed
  invalid generated C# to reach the Roslyn compilation;
- duplicate union discriminator labels that previously emitted duplicate C#
  switch cases;
- recursive value-type members, including aliases, fixed arrays, unions, and
  inheritance paths, that produced invalid generated layouts instead of a
  diagnostic;
- preprocessed integral and floating-point expressions whose repeated unary
  signs could become C# `++` or `--` tokens;
- mutation cases at the end of the input, which previously could escape the
  mutation generator itself.
