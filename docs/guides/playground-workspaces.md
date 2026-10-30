# Playground workspaces

The Source Explorer organizes a virtual `/workspace/` tree. Source tabs open in
the left IDL editor; generated C# opens in the right read-only editor. Each
source has its own editor model, selection, and undo history. Renaming or moving
a file preserves that history for the current session.

The desktop layout fits a viewport of at least 1300 × 985 pixels without a
page scrollbar. Editors use the remaining height, and long diagnostic lists
scroll within their panel. Smaller screens retain page scrolling so all
content remains reachable.

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
generated output. A fresh or reset workspace is empty; its editor offers
shortcuts to create a blank IDL file or load the **Hello, Wireloom** example.

Root **Properties** configures defines, undefines, and strict validation for
that `DdsIdl` root. Enter one definition (`NAME` or `NAME=value`) or
undefinition per line. Settings from all Roots configure one generation batch:
defines are combined, undefines apply afterward, and any Root enabling strict
validation enables it for the whole batch. Each Root starts with the same
initial macro environment; in-file macro changes do not carry to another Root.
Use separate projects/builds for variants that need independent external
symbols or strictness.

**Workspace Settings** configures project defaults for sequence-array mapping,
unbounded support, sequence size, and string size. Explanatory text describes
each setting, including the difference between RTI C# flattening and IDL array
preservation. It also configures
escape-character handling for the batch and include folders. Defaults are
`RtiCSharp`, 100, and 255. File Properties shows these effective project
values by default. Changing a file's sequence or string size saves a file-level
preference that remains in effect when project defaults change; **Reset to
project default** clears that override.

RTI-style unbounded support defaults to disabled and applies to sequences,
narrow strings, and wide strings together. File Properties shows the effective
**Enabled** or **Disabled** value and whether it inherits the project default.
Changing the toggle creates a file override; the conditional **Reset to project
default** link clears it. This applies to declarations in that file,
including include-only files. A root override does not flow into included
files. Explicit IDL bounds remain authoritative. Size controls are disabled
where the effective mode is unbounded; their values are retained and remain
validated. Disabling unbounded support restores those finite settings.

Quoted includes search the including file's folder first, then configured
include folders. Angle includes search configured folders only. No folders
are configured initially. Folder context menus add or remove that folder from
include search. File menus contain only file actions, including the role toggle
and Properties. Selected include folders are searched in ordinal
alphabetical order, so the first matching folder takes precedence.

See the
[compiler configuration guide](configuration.md#settings-at-a-glance).

The **Workspace: Root and includes** and **Workspace: preprocessor settings**
examples demonstrate these controls. Existing single-file examples remain
available. Selecting a diagnostic opens the corresponding source tab at its
compiler-provided UTF-16 location. Escape or clicking outside a dialog closes it and discards unapplied
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
source text under `workspace/`, a `Wireloom.csproj` preview in that folder,
explicit empty folders, and a `workspace.json` record containing source, profile,
roles, and settings. The project preview uses paths relative to `workspace/` and
reflects the current generator settings. Source backup remains
available for unfinished, oversized, rootless, and empty workspaces; it does
not depend on successful compilation. Downloading does not change imported
host files.

Failed operations use a red notice with an **Error** label and alert semantics
in both themes. Notices can be dismissed; hovering or focusing pauses their timer.

The `wireloom-playground` IndexedDB database stores a version-4
workspace record: file identities, paths, sources, file settings, roles,
explicit folders, active file, workspace settings, and profile. Valid version-3
records are upgraded in memory with unbounded support disabled and file modes
inherited. The restored workspace is saved in version-4 format through the
normal recovery-save path; a failed save does not prevent reading the source.
Other versions and malformed records report a recovery error and leave editing
available. Reload regenerates output. Source ZIP exports use version 4, which
older clients reject instead of ignoring unbounded settings.

## Editor services and preferences

Monaco supplies lexical IDL/C# colors, models, markers, and UTF-16 position
mapping; it supplies no semantic IDL or C# service. The compiler remains the
authority on language validity and profile behavior.

Editor font size is an optional localStorage preference (10–20 px, default
12 px), separate from source/profile recovery. Unavailable preference storage
does not prevent changing the font in either editor.
Theme is a separate `wireloom-theme` localStorage preference with Light and
Dark choices. On first use the bootstrap resolves the current system appearance
to one of those choices and saves it. It applies the saved choice before the
editor chunk loads and updates both page color scheme and Monaco. Later system
or developer-tools preference changes do not override it. Preference storage
failure leaves the current session's choice usable.

## Workspace limits

The workspace accepts at most 512 source files, 512 explicit folders, and
256 KiB of total UTF-8 source for compilation. Generated output is limited to
512 documents and 16 MiB of UTF-8 text. Source ZIP remains available when
compilation is ineligible. See the [protocol limits](../reference/playground-protocol.md)
for path/settings validation and host rejection rules.

## Worker contract v4

The browser client and compiler worker ship together. Their request/response
schema, validation limits, and failure categories are documented in the
[maintainer protocol reference](../reference/playground-protocol.md). Source
ZIP and local recovery remain user operations described above.
