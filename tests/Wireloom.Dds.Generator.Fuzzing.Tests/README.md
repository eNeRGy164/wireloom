# Compiler and generator fuzzing tests

This project runs bounded FsCheck properties against the public shared compiler
and Roslyn adapter, plus regressions exposed by those properties. It runs
separately from instrumented unit-test coverage.

Use the [fuzzing runbook](../../docs/testing/fuzzing.md) for restore/run commands,
the case-count setting, supported input scope, and failure interpretation.
Keep FsCheck replay information and the reduced reproducer when a property fails.

[Contributing](../../CONTRIBUTING.md#choose-validation) · [Compiler tests](../Wireloom.Dds.Compiler.Tests/README.md)
