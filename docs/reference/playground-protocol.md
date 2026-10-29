# Playground worker protocol v4

The canonical contract is the .NET adapter DTOs in
[`PlaygroundCompilerAdapter.cs`](../../src/Wireloom.Playground.Browser/PlaygroundCompilerAdapter.cs).
[`protocol.ts`](../../playground/src/protocol.ts) mirrors their field names and
types. Adapter tests in Quality verify parity. The client and worker ship in
one static release, so no cross-release protocol compatibility window is
supported. Compile requests with a different protocol version are rejected.
Workspace records use version 4. Valid version-3 records are upgraded with
unbounded support disabled; other versions remain unsupported. Workspace
schema migration does not provide compatibility with older workers.

```json
{
  "protocolVersion": 4,
  "workerGeneration": 1,
  "requestId": 1,
  "workspaceRevision": 1,
  "profile": "7.7.0",
  "files": [
    {
      "path": "/workspace/main.idl",
      "source": "struct Sample { long value; };",
      "isGenerationRoot": true,
      "options": {
        "defines": [],
        "undefines": [],
        "strict": false
      }
    }
  ],
  "options": {
    "sequenceArrayMapping": "RtiCSharp",
    "sequenceSize": 100,
    "stringSize": 255,
    "includeDirectories": [],
    "enableEscapeChar": false,
    "unboundedSupport": false
  }
}
```

Root file options carry `Defines`, `Undefines`, and `Strict`, matching
`DdsIdl` item metadata. Workspace settings carry project sequence defaults,
unbounded support, escape-character behavior, and include directories. File
`unboundedSupport` is an optional Boolean override: omitted inherits the project
default, while explicit `false` disables it for that file. File options may be
omitted to select root defaults. Recovery-only file IDs, explicit folders,
and active selection are not sent to the compiler. The adapter uses only
supplied text and virtual paths; it never opens a host file.

| Input            | Limit or rule                                                                                        |
| :--------------- | :--------------------------------------------------------------------------------------------------- |
| Files            | 512, with 256 KiB total UTF-8 source                                                                 |
| Explicit folders | 512 in the client/recovery model                                                                     |
| Virtual path     | 1,024 UTF-16 code units, at most 32 levels below `/workspace/`                                       |
| Path segments    | Non-empty; no dot/traversal, backslash, control characters, or surrounding whitespace                |
| Paths            | Unique; files cannot also be parent folders                                                          |
| Settings         | At most 128 entries per list, each non-empty and at most 1,024 characters without control characters |
| Effective sizes  | Positive 32-bit integers                                                                             |
| Generated output | 512 documents and 16 MiB UTF-8 text                                                                  |

Client validation preserves the existing workspace when an operation fails.
The adapter independently rejects invalid identity/version/profile, paths,
settings, source bounds, or a request without a Root. Host errors are explicit
worker errors; they do not invent compiler diagnostic IDs or source locations.
Compiler errors preserve original diagnostic paths and offsets. Generated-output
limits return a `limit` response with no partial documents. Complete response
identity must match the dispatched snapshot and current workspace before the
client applies it.

The client and worker use this contract within one matching static bundle.
For user operations and local recovery, see the [workspace guide](../guides/playground-workspaces.md).
For compilation failures and provider behavior, see the [shared compiler API](compiler-api.md).

[Documentation](../README.md) · [Playground maintenance](../../playground/README.md)
