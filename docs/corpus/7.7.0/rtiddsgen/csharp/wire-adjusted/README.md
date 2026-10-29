# Adjusted C# wire oracle

This directory contains a wire-test copy of the RTI-generated C# oracle. The captured oracle under
`../negative/10-resolve-name-wstring-aliases/` remains unchanged and remains the source of truth for
raw generator output.
Adjustment: in `10-resolve-name-wstring-aliasesPlugin.cs`, qualify the two
`Implementation.*Unmanaged` references with the IDL module `CorpusResolveNameWstringAliases`. This
repairs the captured RTI output's `CS0400` / `CS8377` compile failure. No other source changes are
intended.
The wire matrix compiles this checked-in adjusted copy directly. It does not invoke `rtiddsgen` for
the RTI C# peer. The matching RTI C++ peer still needs the generated-source alias-tag correction
documented in `tests/Wireloom.Dds.Generator.WireCompatibility/WORKAROUNDS.md`.

[Corpus guide](../../../../README.md) · [LTS reproduction](../../../../LTS-PROFILES.md)
