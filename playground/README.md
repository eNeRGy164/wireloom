# Browser Playground

This folder contains the single-file discovery client, its checks, and the
maintained worker boundary. The non-packable .NET host is in
[`../src/Wireloom.Playground.Browser/`](../src/Wireloom.Playground.Browser/).
The host and client share the `Wireloom.Dds.Compiler`; no main-thread compiler
or fallback provider exists.

## Prerequisites

- .NET SDK selected by the repository `global.json` (10.0.400 feature band).
- Node.js 26.7.0 and npm 12.2.0. `.node-version` and `packageManager` record
  the exact versions.

## Build and checks

From this directory:

```shell
npm ci --ignore-scripts
npm run typecheck
npm run lint
npm test
npm run coverage
npm run build
npm run check:paths
npm audit --audit-level=low
```

`npm run build` performs a locked Release publish of the .NET browser host,
replaces the generated `public/dotnet` directory with the current publish
assets, then builds the root-based Vite client. `npm run check:paths` builds a
nested `/playground/` variant and verifies that both bundles contain the
worker, runtime, and MD5 assets at the expected base-relative paths. These are
static artifact checks; they do not replace browser loading or worker startup
verification.

Run `npm run build:coverage` and `npm run coverage:browser` after `npm run
coverage` to build a source-mapped review bundle and exercise the production UI
in headless Chromium. V8 browser coverage is mapped back to first-party source
and merged with Node unit-test coverage. The final JSON, Cobertura report, and
per-file summary are written to `coverage/coverage-final.json`,
`coverage/cobertura-coverage.xml`, and `coverage/browser-summary.json`. Node
unit-test output is kept separately under `coverage/unit/`. The combined
Cobertura report is uploaded to Coveralls by the repository quality workflow.
The coverage bundle is separate from the release bundle and is not a
deployment artifact.

Use `npm run preview -- --host 127.0.0.1` to serve the root build locally after
building. To serve a nested-base build, use a static server that mounts the
`dist-subpath` directory at `/playground/`.

## Scope and current evidence

The client provides Monaco IDL editing and read-only C# output, resizable
panes, file tabs, seven examples, both RTI profiles, clickable diagnostics,
single-file UTF-8 import, and versioned IndexedDB recovery. Edits debounce for
300 ms; examples, imports, restored source, and profile changes generate
automatically. The toolbar has no manual Generate action. Loading an example
or importing over edits asks before replacing
source. Reset deletes the previous recovery record and creates the default
workspace. Recovery stores source and profile; reload recompiles them.

Every successful document opens immediately in tabs and is listed in the
collapsible file explorer, using natural filename order (case-insensitive,
with numbers in numeric order). Filenames stay on one line; the explorer scrolls
horizontally for long names. Shared editor font controls default to 12 px and
remember a 10–20 px preference separately from the source workspace.
Generated tabs use an overlay scrollbar with a transparent track and thumb visible on
hover or keyboard focus. Mouse-wheel input over overflowing tabs scrolls them
horizontally, while Ctrl/Meta-wheel remains available to the browser.
The thumb is draggable and keyboard accessible; its overlay does not consume
tab height, so backgrounds and separators remain full height.
The font controls sit before the RTI profile in the workspace toolbar. The
explorer has its own pointer/keyboard divider; the editor divider changes only
the source/output ratio within the remaining editor area. Notices float at the
bottom right without affecting layout, close after five seconds, and pause
their timer while hovered or focused. They can also be dismissed explicitly.
Restored custom source has no persistent restore-description row.
IDL and C# files have local SVG icons in the explorer and tabs. Side gutters
shrink to 12 px at window widths up to 1280 px and 8 px up to 850 px.
The header has a Light / Dark toggle. First use resolves the current system
appearance immediately to Light or Dark and saves it as `wireloom-theme` in
localStorage. Both the page and Monaco apply the saved choice before the editor
chunk loads; later system or developer-tools preference changes do not switch
the appearance. Preference storage failure leaves the switch usable for the
current session.
Copy and individual downloads use
the compiler's original text; all-document downloads use a stored-entry ZIP.
Windows clipboard reads may normalize newlines. Pending edits mark previous
output outdated and disable exports; current failures clear documents and
diagnostic navigation. Empty successful results have no downloadable files.
Input remains collection-shaped internally, with normalized virtual paths
and generation-root metadata, while this UI exposes one input file.

The synchronous MD5 provider and its 21 Node tests are retained from commit
`f995a3f3`. Eight additional deterministic worker-state tests cover bounded
pending work, complete response identity, stale responses, replacement,
startup recovery, timeout identity across worker replacement, and draining
queued work when a replacement becomes ready.
Seven workspace tests cover UTF-8 boundaries, exact recovery, and rejected
schema/path/profile/input data (36 Node tests total). The Node coverage gate
requires 95% branches, 98% lines, and 80% functions across the protocol,
workspace, and MD5 modules. The Chromium coverage run also exercises all seven
examples against both RTI profiles and checks key editing, storage, export,
resize, theme, failure, and recovery flows. The combined first-party report
currently measures 93.47% branch and 92.88% line coverage.

Stage 0 browser verification passed on Windows Edge 156.0.4314.42: 32 exact
native/browser comparisons across both profiles and root/nested hosting,
repeated compilation, profile switching, responsiveness, startup/provider
failures, real watchdog replacement, and input/result limits. The existing
square SVG is used for the page logo and favicon at both base paths. Broader
browser/release qualification remains #110.
See the [evidence record](../docs/plans/109-browser-playground-stage0-evidence.md).

The [Stage 1 evidence](../docs/plans/109-browser-playground-stage1-evidence.md)
records the implemented UI and scoped Edge checks. Monaco and ZIP packages
are pinned and audited; the production build includes upstream licenses in
`third-party-notices.txt`. The footer records the Wireloom version and Git
revision, with a `-dirty` suffix for a modified checkout. The editor chunk is
larger than Vite's default advisory threshold; public-network loading and
broader desktop compatibility remain #110 checks.

## Reproduce browser verification

Build first, then serve the root bundle on port 4173 and the nested bundle on
4174 in separate terminals:

```shell
npm run preview -- --host 127.0.0.1 --port 4173 --strictPort
npm run preview -- --host 127.0.0.1 --port 4174 --strictPort --outDir dist-subpath --base /playground/
```

From the repository root, export native expectations and prepare each CLI
verification function:

```shell
dotnet run --file playground/scripts/verification/export-native-baseline.cs -- playground/scripts/verification/cases.json output/playwright/native-baseline.json
node playground/scripts/verification/prepare-browser-verification.mjs parity
node playground/scripts/verification/prepare-browser-verification.mjs faults
node playground/scripts/verification/prepare-browser-verification.mjs limits
node playground/scripts/verification/prepare-example-cases.mjs
dotnet run --file playground/scripts/verification/export-native-baseline.cs -- output/playwright/example-cases.json output/playwright/example-native-baseline.json
```

Use Playwright CLI 0.1.22 (the version verified here) to open an isolated
browser session, then run the prepared functions. The `--raw` results are JSON:

```shell
npx --yes --package @playwright/cli@0.1.22 playwright-cli -s=wireloom-verify open about:blank --browser msedge
npx --yes --package @playwright/cli@0.1.22 playwright-cli -s=wireloom-verify --raw run-code --filename output/playwright/run-parity.mjs
npx --yes --package @playwright/cli@0.1.22 playwright-cli -s=wireloom-verify --raw run-code --filename output/playwright/run-faults.mjs
npx --yes --package @playwright/cli@0.1.22 playwright-cli -s=wireloom-verify --raw run-code --filename output/playwright/run-limits.mjs
npx --yes --package @playwright/cli@0.1.22 playwright-cli -s=wireloom-verify close
```

The fault checks intentionally abort local asset requests, inject a transient
MD5 error and an uncaught worker exception, and stall a worker to exercise the
real ten-second watchdog. The responsiveness check compiles 128 declarations
into 512 documents while a main-thread heartbeat and DOM task run. These operations run in
temporary browser contexts against the published bundle. Generated runners,
baselines, screenshots, and reports belong in ignored `output/playwright/`.
After exporting the native baseline, the preparation script also accepts
`ui`, `recovery`, and `ui-faults`. Run each through the same pinned CLI command
with `output/playwright/run-<phase>.mjs`. `ui` checks examples, exact native
downloads/ZIP, clipboard API input, empty/failure states, diagnostic focus,
themes, and resizing. `recovery` checks import, replacement, reload, reset,
and storage failures. `ui-faults` checks editor/runtime asset recovery and
busy-worker replacement. A report's `passed` field must be `true`; CLI process
completion alone does not establish a pass. All test contexts are isolated
from the visitor's saved workspace.

For manual UX review, use a normal Edge window or restore normal behavior in
the automated preview window first. Playwright intercepts native file choosers
and stores captured downloads under GUIDs even when the browser's suggested
filename is correct. This can make Open IDL appear unresponsive and a download
appear unnamed in the automation window. Scripted checks belong in isolated
contexts. Before handing a managed window back for manual review, remove its
`filechooser` listeners, disable `Page.setInterceptFileChooserDialog`, and
restore `Browser.setDownloadBehavior` to `default` for that context. Preserve
the user's workspace when clearing queued intercepted file-picker requests.
See [Playwright download paths](https://playwright.dev/docs/api/class-download#download-path)
and [Chromium file-chooser interception](https://chromedevtools.github.io/devtools-protocol/tot/Page/#method-setInterceptFileChooserDialog).

Choose **Type showcase & XML docs** to explore every supported primitive
spelling, narrow/wide strings, bounded/unbounded sequences, nested collections,
arrays, aliases, enums, defaults/ranges, optional/keyed aggregate members,
inheritance, unions, and value types. Start with the generated
`Showcase.PrimitiveTypes.g.cs`, `Showcase.TextTypes.g.cs`,
`Showcase.CollectionTypes.g.cs`, and `Showcase.RangesAndDefaults.g.cs`.
Comments distinguish explicit IDL bounds from the effective defaults of
100 sequence elements and 255 string bytes/code units. The UI verification
compares all seven examples with native output on both profiles; native
example baselines are generated directly from the visitor-facing sources.
