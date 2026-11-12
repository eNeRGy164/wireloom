# C++ corpus compliance

This suite owns the supported generation-corpus cases previously embedded in
the combined C++ test project. It loads the shared IDL feature files and calls
the public C++ compiler API using the existing RTI 7.7.0 generation profile.
It checks acceptance, the four document names, deterministic LF layout, and
line-length rules independently of CLI filesystem behavior.

```powershell
dotnet restore tests/Wireloom.Dds.Cpp.CorpusCompliance/Wireloom.Dds.Cpp.CorpusCompliance.csproj --locked-mode
dotnet run --project tests/Wireloom.Dds.Cpp.CorpusCompliance/Wireloom.Dds.Cpp.CorpusCompliance.csproj --configuration Release --no-restore -- --filter-trait CorpusSuite=Generation
```

This is generation evidence for the selected supported subset. It does not
compile C++ against an RTI SDK or establish native serialization or DDS
interoperability; those checks remain in the
[wire qualification harness](../Wireloom.Dds.Generator.WireCompatibility/README.md).
Like the C# corpus project, this suite can be run separately from the main
solution. Its cases are also included in the C++ coverage-only build.

[Compiler tests](../Wireloom.Dds.Cpp.Compiler.Tests/README.md) ·
[Generator tests and coverage](../Wireloom.Dds.Cpp.Generator.Tests/README.md)
