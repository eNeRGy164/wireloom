# Playground generation qualification

Qualification checks the built browser worker and workspace UI against native
shared-compiler results for both RTI profiles. It does not qualify generated
code compilation, RTI runtime behavior, or DDS interoperability.

## Reproduce

Use the repository-pinned .NET SDK, Node, npm, and locked dependencies. From
`playground/`, run:

```shell
npm ci --ignore-scripts
npm run build
npm run check:paths
npm run verify:release -- chromium
npm run verify:release -- msedge
npm run verify:release -- firefox
npm run verify:release -- webkit
```

Install the matching Playwright browser binaries with `npx playwright install`
if they are unavailable. `msedge` uses the installed Microsoft Edge channel.
Run the commands sequentially: the verifier owns ports 4183 and 4184 and
refuses to reuse an occupied port or overwrite an active run's reports. Its
exclusive `output/playwright/qualification.lock` records the owner PID; remove
a stale lock only after that process has stopped. Each browser phase has a
180-second verification deadline. It serves this checkout's root and nested
Release bundles and stops its own preview servers when finished. It creates
isolated browser contexts and does not use a visitor's recovery data.

The optional second argument after the browser is a comma-separated phase
list, for example `npm run verify:release -- firefox corpus,corpus-ui`.
Chromium and Edge default to all ten phases. Firefox and WebKit default to
`parity,corpus,corpus-ui,desktop,workspace,faults,limits`; native clipboard assertions and
the detailed Chromium UI/recovery suite are excluded. Other engines still
exercise editor input, imports, recovery, failure clearing, watchdog
replacement, stale results, and import/adapter limits through their portable
phases. Outside Chromium, multiline inputs and oversized-input checks use
UTF-8 import. Native multiline/oversized paste remains unqualified on Firefox
and WebKit; their reports record the import-based limit scope. Keyboard helpers
follow Monaco's user-agent bindings, including Command+A on Windows WebKit.

The command exits unsuccessfully if any selected check fails, a phase throws,
or no checks run. Check `passed: true` in
`output/playwright/qualification-<browser>.json`. The report retains browser,
Node and SDK versions, source revision/status, tracked-diff hash, verification
script hashes, original corpus input hashes and virtual paths, native baseline
hashes, every bundle file hash, timings, comparisons, and individual outcomes.
Preparation/build output is retained in the adjacent `.log`. Raw reports,
generated native baselines, and prepared runners are ignored local artifacts;
archive them with a release candidate before publishing. Rebuild and rerun
after changing the compiler, adapter, client, inputs, profiles, or bundle.
Each invocation replaces that browser's latest report; archive a full report
before a focused rerun.

The CLI-compatible prepared runners remain usable through the procedure in
the [Playground README](../../playground/README.md#reproduce-browser-verification).
Set `WIRELOOM_VERIFY_ROOT` and `WIRELOOM_VERIFY_NESTED` when preparing them to
use different preview origins.

## Cases and assertions

[`prepare-corpus-cases.mjs`](../../playground/scripts/verification/prepare-corpus-cases.mjs)
reads unchanged UTF-8 inputs from the retained corpus manifest. It does not
copy or rewrite IDL fixtures into a second maintained corpus. Expected managed
acceptance follows the explicit map in
[`CorpusRepository.cs`](../../tests/Wireloom.Dds.Generator.CorpusCompliance/CorpusRepository.cs),
including the 7.3.1 rejection of `@must_understand`; RTI outcomes and manifest
folders do not determine Wireloom acceptance. Run the full corpus-compliance
suite for both profiles alongside this selection to keep those expectations
aligned.

| Area                                            | Retained case IDs                                             |
| :---------------------------------------------- | :------------------------------------------------------------ |
| Primitives and strings                          | `01-primitives`, `01-full-widths`, `04-strings`               |
| Enums, aggregates, inheritance, recursion       | `03-enums-aliases`, `06-aggregates`, `07-union-enum`          |
| Arrays, sequences, aliases, nested collections  | `05-shapes`                                                   |
| Hash member IDs and module policy               | `09-autoid-hash`, `14-autoid-scope`                           |
| Annotations and typed defaults                  | `09-must-understand`, `10-annotations`, `14-typed-defaults`   |
| Inline preprocessing                            | `11-preprocessor-advanced`                                    |
| Visible warnings and ignored declarations       | `12-unknown-annotation`, `12-interface`                       |
| Unsupported constructs and preprocessing errors | `12-unsupported-map`, `12-fixed`, `12-error-directive`        |
| Missing include                                 | The unchanged `14-autoid-scope` root with its include omitted |

There are 18 retained cases plus the missing-include variant. A native
exporter runs the public compiler with platform MD5 and explicit profiles;
the browser uses the worker-local JavaScript provider. Each case/profile runs
at both `/` and `/playground/`: **76 exact worker comparisons per engine**.
Assertions cover every generated document name and complete text, success,
diagnostic ID/severity/message/path/UTF-16 offset, and request/revision/profile/
worker identity. Diagnostics are compared in deterministic source order. A
failed result must contain no generated documents.

The 18 single-file cases run through the actual UI on both profiles:
**36 UI comparisons per engine**. They check visible diagnostic text and source
locations, complete output tabs and export text, zero-document success, and
disabled copy/download/ZIP on failed or empty output. Unknown annotations and
interfaces deliberately produce warnings; a successful generation does not
mean every input declaration was emitted. The separate UI phase exercises
actual browser downloads, clipboard input, and ZIP contents; corpus export
checks intercept download blobs to avoid hundreds of saves.

## Workspace boundary

The Source Explorer supports multiple virtual files, explicit folders, Root and
Include-only roles, Workspace Settings, and declaring-file Properties. The
worker compiles all Roots together with their supplied includes. Import uses
basenames in the selected virtual folder.
See the [workspace guide and canonical v3 contract](../guides/playground-workspaces.md).

The retained `14-autoid-scope` worker comparison supplies its unchanged include
closure. That case remains excluded from the 36-case corpus UI selection;
separate multi-file examples exercise includes through the actual UI on both
profiles and compare every generated source byte against native results. The
missing-include variant continues to check source-located failure behavior.

The workspace phase checks all import conflict choices and atomic Cancel,
selected-folder basenames, empty folder recovery, keyboard tree/context actions,
menu/drag moves, undo through tab switches and renames, configured-folder moves
without source rewriting, cross-file diagnostics, no-Root and empty workspaces,
and rejection of unsupported workspace record versions.

Presentation checks cover independent Source/Generated group disclosure,
Source-scoped hover/focus controls, default folder expansion with preserved
manual collapse, distinct file/folder context menus, and error alerts in both
themes. Dialog checks cover Escape cancellation and keyboard-operated role
and mapping toggles.

IDL Copy/Download checks preserve exact UTF-8/CRLF text, follow the selected
tab, work with invalid source, and are disabled without an active file. Source
ZIP checks preserve the live source tree, empty folders, roles, and settings
independently of compilation. A source named `workspace.json` must not collide
with the archive's metadata. Failed moves leave the workspace unchanged;
clipboard failures show an error alert.

## Interaction and failure checks

The parity phase also checks root/nested assets, repeated compilation, profile
switching, cold/warm local startup, and a main-thread heartbeat while 128
declarations generate 512 documents. The fault phases cover missing editor,
worker and runtime assets, provider errors, worker exceptions, ten-second
watchdog replacement, newest pending input/profile selection, disabled stale
exports, cleared diagnostic navigation, and successful recovery.

The Chromium/Edge limits phase checks oversized ASCII and multibyte editor
input before dispatch. Other engines check oversized imports while preserving
current output. All engines check invalid worker requests, the 512-document
cap, no partial output, and subsequent recovery. The detailed Chromium UI
suite also checks rejected imports: invalid
UTF-8, invalid virtual names, and oversized files preserve the current successful
workspace. Editing beyond the limit invalidates previous output. Cancellation
means rejecting stale results or terminating/replacing a worker; a synchronous
compilation is not interrupted by a queued cancellation message.

The desktop/UI phases cover every example/profile combination, keyboard tabs,
diagnostic focus, independent dividers, themes/font preferences, widths of 800,
1000 and 1440 pixels, import replacement/cancellation, reload/reset, and
unavailable/quota/older recovery data. These checks are scoped browser evidence,
not a complete accessibility audit.

## Evidence boundaries

The [retained corpus suite](../../tests/Wireloom.Dds.Generator.CorpusCompliance/README.md)
remains the compatibility source of truth. This browser selection supplements
its coverage and does not establish complete OMG IDL support or RTI
qualification. The package, runtime-contract, and licensed DDS wire suites
retain their own evidence and supported-version rules.

Windows Chromium, installed Edge, and Playwright Firefox checks apply to the
recorded desktop versions. Playwright WebKit is supplementary engine evidence;
it does not qualify Safari or macOS. Mobile, assistive-technology review,
public-network startup, caching/headers, and the final public host must be
assessed separately. Local cold/warm startup measurements are not Internet
latency measurements. Publication and host verification remain
[#111](https://github.com/eNeRGy164/wireloom/issues/111).

## Client quality checks

From `playground/`, run:

```shell
npm run typecheck
npm run lint
npm run format:check
npm test
npm run coverage
npm run build:coverage
npm run coverage:browser
npm audit --audit-level=low
npm run build:pages
npm run verify:pages
```

The Pages smoke check serves the local deployment candidate; public-host
verification requires the deployed HTTPS URL. Compiler/adapter tests, their
coverage reports, and Community Qodana use the repository's
[quality procedure](../../CONTRIBUTING.md#quality-evidence).

## Current local results

The qualification reports identify the source revision and uncommitted status,
input and bundle hashes, tool versions, and verification scope. The latest
focused run on October 27, 2026, covers the current uncommitted workspace
changes in Chromium 156.0.8078.4.

| Check                                | Scope and result                                                    |
| :----------------------------------- | :------------------------------------------------------------------ |
| Node unit and Pages tests            | 65 passed; 1 symlink test skipped because Windows lacks link privileges |
| Chromium 156.0.8078.4                | All ten release phases passed (87/87 checks)                        |
| Chromium scenario coverage           | 18 example/profile combinations and all added workspace UX scenarios |
| TypeScript, ESLint, Prettier         | Passed                                                              |
| Architecture memory                  | Current                                                            |

| Coverage scope              | Branches             | Lines                |
| :-------------------------- | :------------------- | :------------------- |
| Compiler                    | 6,658/7,338 (90.73%) | 8,959/9,203 (97.35%) |
| Browser adapter             | 110/114 (96.49%)     | 154/158 (97.47%)     |
| Direct Node TypeScript scope | 278/280 (99.29%)    | 100%                 |
| Node + first-party Chromium  | 88.46%              | 93.78%               |
| Source archive unit scope    | 100%                | 100%                 |

The direct Node threshold gates only compiler-client, workspace-model,
recovery-saver, source-archive, worker-state, and MD5 modules. Combined
source-mapped browser coverage also includes UI modules and currently remains
below 99% lines and 98% branches; that broader TypeScript coverage target is
not met. CI and Coveralls for these changes remain pending. The local results do
not establish deployment or public-host qualification. Folder-hierarchy import
is outside this scope and tracked in [#120](https://github.com/eNeRGy164/wireloom/issues/120).

## Evidence artifacts

| Artifact                                         | Contents                                        |
| :----------------------------------------------- | :---------------------------------------------- |
| `output/playwright/qualification-chromium.json`  | Full ten-phase Chromium qualification           |
| `output/playwright/qualification-firefox.json`   | Firefox workspace checks                        |
| `output/playwright/qualification-webkit.json`    | WebKit workspace checks                         |
| `output/playwright/pages-smoke.json`             | Local Pages candidate smoke checks              |
| `playground/coverage/unit/coverage-summary.json` | Direct Node coverage                            |
| `playground/coverage/browser-summary.json`       | Combined Node and first-party Chromium coverage |
| `coverage/compiler.cobertura.xml`                | Compiler coverage                               |
| `coverage/playground.cobertura.xml`              | Browser adapter coverage                        |
| `.qodana/results/qodana.sarif.json`              | Community Qodana findings                       |

Reports are ignored local artifacts. Retain the matching reports with the
release candidate; their source and bundle hashes determine applicability.
