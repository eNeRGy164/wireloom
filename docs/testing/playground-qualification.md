# Playground generation qualification

This is the first-release verification scope for [#110](https://github.com/eNeRGy164/wireloom/issues/110),
using the Playground implementation from [#123](https://github.com/eNeRGy164/wireloom/pull/123).
It checks the built browser worker and the single-file UI against native
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
Chromium and Edge default to all nine phases. Firefox and WebKit default to
`parity,corpus,corpus-ui,desktop,faults,limits`; native clipboard assertions and
the detailed Chromium UI/recovery suite are excluded. Other engines still
exercise editor input, imports, recovery, failure clearing, watchdog
replacement, stale results, and import/adapter limits through their portable
phases. Multiline inputs use exact UTF-8 import outside Chromium: Playwright's
multiline `insertText` duplicated text in Monaco under Firefox 157.0. The
oversized editor-insertion probe also stalled in that automation path. Native
multiline/oversized paste remains unqualified on Firefox and WebKit; their
passing report explicitly records import-based limit scope.
The keyboard helper also follows Monaco's Macintosh user-agent binding:
Windows WebKit requires Command+A for source replacement despite reporting
`navigator.platform` as `Win32`.

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

## Single-file boundary

The UI imports and edits one `.idl` file. It cannot load its include closure or
set defines, include directories, strictness, mapping, or effective bound
overrides. An unresolved include produces a visible compiler diagnostic, clears
previous output, and disables exports. Multi-file editing and folder import
remain [#119](https://github.com/eNeRGy164/wireloom/issues/119) and
[#120](https://github.com/eNeRGy164/wireloom/issues/120).

The `14-autoid-scope` worker comparison supplies its retained include as an
include-only file. The root is remapped to `/workspace/14-autoid-scope.idl` and
the include to `/workspace/includes/autoid-hash-include.idl`, preserving the
root's include spelling and every source byte. This is adapter evidence only;
it is excluded from the UI matrix. The missing-include variant checks the
actual single-file limitation in both the worker and UI.

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
UTF-8, wrong extensions, and oversized files preserve the current successful
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

## Initial local evidence: October 26, 2026

The Release bundle was built on Windows with SDK 10.0.402, .NET runtime
10.0.13, Node 26.7.0, and npm 12.2.0, from the #109 baseline
`7b3de71ac2bbb114491dea3bf32333345d12234d` plus the #110 verification changes.
The baseline and bundle hashes are retained in the local reports described
above. These runs precede the P2 fixes being handled in #109 for active-request
restart and delayed save confirmations. Refresh qualification against that
updated implementation before release; this record does not close #110.

| Browser/engine on Windows | Scope                                                                                     | Result |
| :------------------------ | :---------------------------------------------------------------------------------------- | :----- |
| Chromium 156.0.8078.4     | All nine phases, 68 checks                                                                | Passed |
| Edge 156.0.4314.42        | All nine phases, 68 checks                                                                | Passed |
| Playwright Firefox 157.0  | Six scoped phases, 53 checks; import-based oversized input                                | Passed |
| Playwright WebKit 27.2    | Six scoped phases, 53 checks; import-based oversized input; supplementary engine evidence | Passed |

Every listed engine passed 76 corpus worker comparisons and 36 corpus UI
comparisons. The synthetic eight-case worker baseline and all seven UI
examples also passed under both profiles. The exclusive-run guard was checked
by starting a second invocation: it failed without changing the previous
Chromium report. Root and nested Release asset checks passed with 65 .NET host
assets per bundle.

Compiler tests passed **1,576/1,576**, including the separately measured
**28/28** browser-adapter tests. Each RTI corpus-compliance profile passed
**373 tests**, with one intentional export-only skip. The client passed strict
TypeScript, ESLint, Prettier, **36 Node tests**, and the coverage gate. The
dependency audit reported zero vulnerabilities. Fresh Community Qodana exited
successfully with **zero findings** and a successful SARIF invocation. Its
Windows-worktree Git metadata warning did not prevent analysis.

| Coverage scope              | Branches             | Lines                |
| :-------------------------- | :------------------- | :------------------- |
| Compiler                    | 6,496/7,338 (88.53%) | 8,775/9,203 (95.35%) |
| Browser adapter             | 64/66 (96.97%)       | 110/111 (99.10%)     |
| Node + first-party Chromium | 86/92 (93.47%)       | 1,443/1,557 (92.67%) |

The Chromium corpus run generated 512 documents in about 324 ms while a page
task ran in about 31 ms; the raw report retains the exact timings. These are
local observations with the scope described above. At the time, CI and
Coveralls for #110 were pending; their later results are recorded below.

## Refreshed evidence after the #109 P2 fixes

The browser host now includes #109's active-request restart and delayed-save
fixes, direct client/recovery tests, standalone fuzzing project, and coverage
gates. On merged revision
`d3c2c8cf4a6a66c5a1ae8836fa87b3c0fb91be99`, I rebuilt the root and nested
Release bundles and reran each browser matrix. Every report records that
revision, a clean source status, and hashes for all 146 bundle files.

| Browser/engine on Windows | Scope | Result |
| :--- | :--- | :--- |
| Chromium 156.0.8078.4 | All nine phases, 68 checks | Passed |
| Edge 156.0.4314.42 | All nine phases, 68 checks | Passed |
| Playwright Firefox 157.0 | Six scoped phases, 53 checks; import-based oversized input | Passed |
| Playwright WebKit 27.2 | Six scoped phases, 53 checks; import-based oversized input; supplementary engine evidence | Passed |

Both corpus-compliance profiles passed **373 tests**, each with one intentional
export-only skip. Compiler tests passed **1,601/1,601**. All **50 Node tests**
passed, including #109's active-restart and delayed-save regressions; direct
coverage measured **98.06% branches and 100% lines**. TypeScript, ESLint,
Prettier, asset-path checks, and browser coverage passed.

| Refreshed coverage scope    | Branches             | Lines                |
| :-------------------------- | :------------------- | :------------------- |
| Compiler                    | 6,656/7,338 (90.71%) | 8,959/9,203 (97.35%) |
| Node + first-party Chromium | 158/166 (95.18%)     | 1,528/1,645 (92.88%) |

These runs supersede the earlier browser matrix for this revision. Native
multiline/oversized paste on Firefox and WebKit remains unqualified; their
portable import and adapter-limit checks passed. Full accessibility review,
Safari/macOS, and public-host startup remain separate release gaps. WebKit
does not qualify Safari.

A full local Community Qodana scan completed analysis but reported six notes in
the unchanged `tests/Shared/CompilerTestSupport.cs`: three visibility
findings, two unused-member findings, and one unused-return finding. The PR
Qodana check passed in pull-request mode with no new problems in changed files.
The merged quality workflow and Coveralls also passed; Coveralls reported
**98.074%** coverage. The local full-scan notes remain baseline findings, not
new #110 findings. SARIF and logs remain under `.qodana/results/`.
