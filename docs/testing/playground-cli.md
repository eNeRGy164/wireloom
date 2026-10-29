# Advanced browser verification

Use this low-level procedure when investigating an individual verification
phase. For normal qualification, prefer the owned-server
[release verifier](playground-qualification.md#reproduce). Commands below state
whether to run from `playground/` or the repository root.

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
`ui`, `workspace`, `recovery`, and `ui-faults`. Run each through the same pinned CLI command
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
compares all nine examples with native output on both profiles; native
example baselines are generated directly from the visitor-facing sources.
