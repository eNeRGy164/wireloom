# Shared compiler API

Callers of `IdlCompiler` supply `IdlInput` objects directly. Constructor options
correspond to the build settings, with one important default difference:
`IdlInput.generate` defaults to `true`; automatically tracked MSBuild includes
have `Generate="false"`.

This API is for repository C# projects referencing the
`Wireloom.Dds.Compiler` project. The normal analyzer package bundles the same
compiler assembly beside the generator for analyzer loading; it does not add a
compiler application runtime dependency.

```csharp
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using Wireloom;

var inputs = new List<IdlInput>
{
    new(
        path: "Telemetry.idl",
        text:
            """
            #include "Shared.idl"
            struct Sample { sequence<long, SAMPLE_BOUND> values[2]; };
            """,
        sequenceArrayMapping: IdlSequenceArrayMapping.Idl,
        generate: true,
        strict: false,
        defines: ["SAMPLE_BOUND=4"],
        undefines: ["LEGACY_LAYOUT"],
        includeDirectories: []),
    new(
        path: "Shared.idl",
        text: "struct Shared { long id; };",
        generate: false)
};

var profile = IdlCompiler.GetProfileForRuntimeVersion(new Version(7, 7, 0));
var result = IdlCompiler.Compile(inputs, profile, MD5.HashData, CancellationToken.None);

foreach (var diagnostic in result.Diagnostics)
{
    Console.WriteLine($"{diagnostic.Severity} {diagnostic.Id}: {diagnostic.Path}:{diagnostic.Offset}: {diagnostic.Message}");
}

if (result.Success)
{
    foreach (var document in result.Documents)
    {
        Console.WriteLine($"{document.HintName}: {document.Source.Length} characters");
    }
}
```

`path` provides input identity and diagnostic/include locations; `text`
provides the source. Supply included files as inputs too: an include directory
searches the supplied inputs rather than loading arbitrary files from disk.
`strict`, `defines`, `undefines`, and `includeDirectories` retain the same
batch semantics. Define, undefine, and include-directory lists are copied when
the input is created, and the compiler snapshots the input collection at call
entry. `sequenceArrayMapping` remains file-owned; omitting it selects
`RtiCSharp`. Set `enableEscapeChar: true` on every generation root to enable
leading-underscore normalization for the batch. Included inputs inherit this
setting. Includes resolve only against supplied inputs; the compiler does not
read from disk.

Select `IdlCompilerProfile.Rti731` or `IdlCompilerProfile.Rti770` for each
compile. `GetProfileForRuntimeVersion` maps supported runtime versions and
throws when the version is below 7.3.1. `IdlCompilationResult.Success` is based
on error diagnostics, so an empty document list can be either successful or a
failure. `DDSG0001` and unsupported-profile `DDSG0004` errors return no
documents; warnings and informational diagnostics can accompany documents.
Diagnostics retain the input path, complete source text, and zero-based UTF-16
offset. Cancellation and unexpected implementation exceptions propagate to
the caller.

## Host services and failure handling

The host supplies a synchronous `Func<byte[], byte[]>` that returns the complete
16-byte MD5 digest. The compiler owns UTF-8 encoding, DDS member-ID selection,
byte order, and masking. Use a deterministic provider, and make any captured
state safe when the provider is shared across concurrent calls.

A null provider fails at entry. A failed provider or an invalid digest raises
`InvalidOperationException`; cancellation propagates. These faults do not become
ordinary IDL diagnostics. Expected IDL failures instead return error diagnostics
and no documents. Check `Success`, not just whether `Documents` is empty.

The browser host supplies its worker-local JavaScript provider; it cannot use
this native `MD5.HashData` example. See [ADR-002](../architecture/adr/ADR-002-inject-member-id-md5-per-compilation.md)
for the provider decision and [the worker protocol](playground-protocol.md)
for the browser boundary. The [compiler API source](../../src/Wireloom.Dds.Compiler/Api/IdlCompiler.cs)
and [input model](../../src/Wireloom.Dds.Compiler/Api/IdlInput.cs) define the contract.

[Documentation](../README.md) · [Consumer configuration](../guides/configuration.md)
