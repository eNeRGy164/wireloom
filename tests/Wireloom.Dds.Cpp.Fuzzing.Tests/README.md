# C++ compiler fuzzing tests

This suite uses the same IDL seeds, mutation characters, and mutation logic as
the C# fuzzing suite. The input corpus is copied here; keep its content
synchronized with
[the C# suite](../Wireloom.Dds.CSharp.Fuzzing.Tests/FsCheckIdlCompilerFuzzingSpecs.cs).

Backend assertions stay C++ specific. For each implemented RTI profile, mutated
input must return deterministic documents or diagnostics through the public
compiler API. Failed compilations must return no generated documents; successful
documents must have unique file names and LF line endings. Unsupported C++
shapes may be rejected with diagnostics. This suite does not compile generated
C++ or claim runtime compatibility.

```powershell
dotnet restore tests/Wireloom.Dds.Cpp.Fuzzing.Tests/Wireloom.Dds.Cpp.Fuzzing.Tests.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.Cpp.Fuzzing.Tests/Wireloom.Dds.Cpp.Fuzzing.Tests.csproj --configuration Release --no-restore
```

`WIRELOOM_FSCHECK_TESTS` selects the case count per profile: 1,000 by default,
clamped to 1–100,000. Preserve FsCheck replay information and the failing IDL
when investigating a counterexample. Fuzzing runs without coverage instrumentation.

[General fuzzing runbook](../../docs/testing/fuzzing.md) ·
[Compiler tests](../Wireloom.Dds.Cpp.Compiler.Tests/README.md)
