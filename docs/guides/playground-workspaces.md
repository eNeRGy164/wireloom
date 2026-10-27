# Playground workspaces

The Source Explorer organizes a virtual `/workspace/` tree. Source tabs open in
the left IDL editor; generated C# opens in the right read-only editor. Each
source has its own editor model, selection, and undo history. Renaming or moving
a file preserves that history for the current session.

## Files, folders, and import

Use the **New File** and **New Folder** icon buttons or the Source context menu to create items in the
selected folder. Source and Generated have independent collapsible headers.
Select a header or press Enter/Space while it is focused to fold that group.
Selecting a file uses its parent as the destination. The
context menu supports Rename, Move to, Delete, and file Properties. The compact
Explorer toolbar also offers **Collapse Folders** and **More Actions**, with
tooltips and accessible names. These icons appear while the Source group is hovered
or contains keyboard focus, and remain visible on touch devices. A blank line
separates Generated from Source. Folders expand on load and when created;
manual collapse choices remain intact during workspace edits. Dragging an
item onto a folder also moves it. Tree arrow keys, Home, End, Enter, Space,
and the standard context-menu key operate the Explorer; no custom editor
shortcuts are added.

**Import Files…** supports multiple UTF-8 files. It uses browser-provided
basenames in the selected virtual folder, or `/workspace/` by default. The
picker does not expose the original directory hierarchy. Resolve conflicts
with Replace, Skip, Rename, or Cancel. The entire import applies after all
choices and validation succeed; Cancel leaves the workspace unchanged.

Moves and renames require Replace, Rename, or Cancel when a destination exists.
Replacing a folder removes its contents; deleting a non-empty folder requires
confirmation. Explicit empty folders persist through reload. Moving or renaming
an item changes its virtual path without rewriting IDL source. Configured
include-folder paths follow folder moves; deleting a folder removes its include
setting.

## Compiler roles and settings

Every new file defaults to **Root**. The file context menu can mark it **Root**
or **Include-only** directly. In **Properties**, the two-option role toggle selects **Include-only**
to make it available through includes without starting generation itself. All
Roots compile together in one batch under the selected RTI profile. With files
but no Roots, the workspace reports “Mark at least one file as Root” and clears
generated output. An empty workspace waits for a new or imported file.

**Workspace Settings** configures defines, undefines, include folders, strict
validation, and escape-character handling for the complete batch. Enter one
definition (`NAME` or `NAME=value`) or undefinition per line. Undefines apply
after defines. Each Root starts with the compiler's initial macro environment.

Quoted includes search the including file's folder first, then configured
include folders. Angle includes search configured folders only. No folders
are configured initially. Folder context menus add or remove that folder from
include search. File menus contain only file actions, including the role toggle
and Properties. Selected include folders are searched in ordinal
alphabetical order, so the first matching folder takes precedence.

File **Properties** uses an **RTI C# / IDL** toggle for sequence-array mapping
and number fields for effective unbounded
sequence/string sizes for declarations in that file, including Include-only
files. Defaults are `RtiCSharp`, 100, and 255. These settings do not flow from a
Root into declarations in an included file. See the
[compiler configuration guide](configuration.md#settings-at-a-glance).

The **Workspace: Root and includes** and **Workspace: preprocessor settings**
examples demonstrate these controls. Existing single-file examples remain
available. Selecting a diagnostic opens the corresponding source tab at its
compiler-provided UTF-16 location. Escape closes dialogs and discards unapplied
changes, including settings, properties, name prompts, and import conflicts.
Properties toggles also support the standard radio-group arrow keys.

## Results, exports, and local recovery

Source, path, role, settings, and profile changes invalidate the current result.
Previous output is visibly outdated while a new request is pending and cannot
be exported. Current failures clear output. The Generated pane's Copy, Download,
and ZIP controls export the current successful C# documents.

The IDL pane's **Copy** and **Download** buttons save the active file's current
source, including unfinished or invalid IDL. They are disabled when no source
file is open. Download uses the virtual basename and preserves the source text.

The Source pane's **ZIP** button saves `wireloom-sources.zip`, including current
source text under `workspace/`, explicit empty folders, and a `workspace.json`
record containing source, profile, roles, and settings. Source backup remains
available for unfinished, oversized, rootless, and empty workspaces; it does
not depend on successful compilation. Downloading does not change imported
host files.

Failed operations use a red notice with an **Error** label and alert semantics
in both themes. Notices can be dismissed; hovering or focusing pauses their timer.

The existing `wireloom-playground` IndexedDB database stores a version-2
workspace record: file identities, paths, sources, file settings, roles,
explicit folders, active file, workspace settings, and profile. A valid version-1
single-file record migrates with its exact source, virtual basename, and profile,
as a Root with default settings. Unknown or malformed versions report a
recovery error and leave editing available. Reload regenerates output.

## Worker contract v2

The canonical contract is the .NET adapter DTOs in
[`PlaygroundCompilerAdapter.cs`](../../src/Wireloom.Playground.Browser/PlaygroundCompilerAdapter.cs).
[`protocol.ts`](../../playground/src/protocol.ts) mirrors their field names and
types. Adapter tests in Quality verify parity. The client and worker ship in
one static release, so no cross-release protocol compatibility window is
supported. Version-1 compile requests are rejected; recovery migration is a
separate concern.

```json
{
  "protocolVersion": 2,
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
        "sequenceArrayMapping": "RtiCSharp",
        "sequenceSize": 100,
        "stringSize": 255
      }
    }
  ],
  "options": {
    "defines": [],
    "undefines": [],
    "includeDirectories": [],
    "strict": false,
    "enableEscapeChar": false
  }
}
```

File options may be omitted to select defaults. Workspace settings use the
complete shape above. Recovery-only file IDs, explicit folders, and active
selection are not sent to the compiler. The adapter uses only supplied text
and virtual paths; it never opens a host file.

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
