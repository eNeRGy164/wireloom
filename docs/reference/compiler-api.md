# Compiler APIs

The C# and C++ compiler facades use the shared IDL front end. Each backend owns
its profile, mapping policy, and generated document type; both use the same
immutable host request and generic result contracts. The standalone C++
generator owns its CLI and native package contracts. Callers supply `IdlInput`
objects directly. Constructor options correspond to the build settings, with
one important default difference:
`IdlInput.generate` defaults to `true`; automatically tracked MSBuild includes
have `Generate="false"`.

This API is for repository C# projects referencing the
`Wireloom.Dds.CSharp.Compiler` project. The normal analyzer package bundles the same
C# compiler and shared IDL assemblies beside the generator for analyzer loading.
Both are build assets.

The shared IDL API is in `Wireloom.Dds.Idl`; the C# compiler API is in
`Wireloom.Dds.CSharp.Compiler`. Project namespaces follow their owning source
project. The C# generator package ID is `Wireloom.Dds.Generator`, while the
native C++ generator package ID is `Wireloom.Dds.Cpp.Generator`.

The shared IDL assembly exposes `IdlAnalyzer.Analyze` and its target-independent
bound declaration model for sibling compiler backends. Parsing, binding, and
symbol-table implementation details remain internal. The model types live with
the semantic model; the analyzer and immutable input/result contracts live in
the IDL project's `Api` folder. Public C# accessibility supports this project
boundary and does not add the IDL assembly to the consumer-facing stability
contract.

These direct C# APIs are internal integration interfaces, even where their
C# accessibility is public. They are outside the
[1.x public stability contract](../guides/support-and-compatibility.md#public-contract).

```csharp
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using Wireloom.Dds.Idl;
using Wireloom.Dds.CSharp.Compiler;

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
        unboundedSupport: true,
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

var request = new IdlCompilationRequest(inputs, MD5.HashData, CancellationToken.None);
var profile = IdlCompiler.GetProfileForRuntimeVersion(new Version(7, 7, 0));
var result = IdlCompiler.Compile(request, profile);

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
`RtiCSharp`. The request snapshots the input collection and carries the
required host MD5 provider and cancellation token. Set
`enableEscapeChar: true` on every generation root to enable
leading-underscore normalization for the batch. Included inputs inherit this
setting. `unboundedSupport` is file-owned: set it on each input whose
declarations should use RTI-style unbounded sequence and string semantics. It
defaults to `false`; includes do not inherit a root's value. Includes resolve
only against supplied inputs; the compiler does not read from disk.

Select `IdlCompilerProfile.Rti612`, `IdlCompilerProfile.Rti731`, or
`IdlCompilerProfile.Rti770` for each compile. `GetProfileForRuntimeVersion`
maps supported runtime versions and
throws when the version is below 6.1.2.
`IdlCompilationResult<TDocument>.Success` is based on error diagnostics, so an
empty document list can be either successful or a
failure. `DDSG0001` and unsupported-profile `DDSG0004` errors return no
documents; warnings and informational diagnostics can accompany documents.
Diagnostics retain the input path, complete source text, and zero-based UTF-16
offset. Cancellation and unexpected implementation exceptions propagate to
the caller.

## Host services and failure handling

The shared `IdlCompilationRequest` carries a synchronous
`Func<byte[], byte[]>` that returns the complete 16-byte MD5 digest. The
compiler owns UTF-8 encoding, DDS member-ID selection,
byte order, and masking. Use a deterministic provider, and make any captured
state safe when the provider is shared across concurrent calls.

A null provider fails at entry. A failed provider or an invalid digest raises
`InvalidOperationException`; cancellation propagates. These faults do not become
ordinary IDL diagnostics. Expected IDL failures instead return error diagnostics
and no documents. Check `Success`, not just whether `Documents` is empty.

The C++ compiler uses the same `IdlCompilationRequest` and returns
`IdlCompilationResult<GeneratedCppSource>` from `CppCompiler.Compile`. Its
`CppCompilerProfile` and `GeneratedCppSource.FileName` remain C++-specific.
The facade shares input, MD5, cancellation, diagnostics, and success semantics
without introducing a common compiler interface or inheritance hierarchy.

The browser host supplies its worker-local JavaScript provider; it cannot use
this native `MD5.HashData` example. See [ADR-002](../architecture/adr/ADR-002-inject-member-id-md5-per-compilation.md)
for the provider decision and [the worker protocol](playground-protocol.md)
for the browser boundary. The [compiler API source](../../src/Wireloom.Dds.CSharp.Compiler/Api/Compiler.cs)
and [input model](../../src/Wireloom.Dds.Idl/Api/IdlInput.cs) define the contract.

[Documentation](../README.md) · [Consumer configuration](../guides/configuration.md)
