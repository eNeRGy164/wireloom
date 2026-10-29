# Playground local results — October 27, 2026

The qualification reports identify the source revision and uncommitted status,
input and bundle hashes, tool versions, and verification scope. The recorded
focused run on October 27, 2026, covered an uncommitted workspace in
Chromium 156.0.8078.4. Its exact revision/diff hash is not included in this
retained narrative; the original reports are needed to establish applicability.

| Check                        | Scope and result                                                        |
| :--------------------------- | :---------------------------------------------------------------------- |
| Node unit and Pages tests    | 65 passed; 1 symlink test skipped because Windows lacks link privileges |
| Chromium 156.0.8078.4        | All ten release phases passed (87/87 checks)                            |
| Chromium scenario coverage   | 18 example/profile combinations and all added workspace UX scenarios    |
| TypeScript, ESLint, Prettier | Passed                                                                  |
| Architecture memory          | Current                                                                 |

| Coverage scope               | Branches             | Lines                |
| :--------------------------- | :------------------- | :------------------- |
| Compiler                     | 6,658/7,338 (90.73%) | 8,959/9,203 (97.35%) |
| Browser adapter              | 110/114 (96.49%)     | 154/158 (97.47%)     |
| Direct Node TypeScript scope | 278/280 (99.29%)     | 100%                 |
| Node + first-party Chromium  | 88.46%               | 93.78%               |
| Source archive unit scope    | 100%                 | 100%                 |

The direct Node threshold gates only compiler-client, workspace-model,
recovery-saver, source-archive, worker-state, and MD5 modules. Combined
source-mapped browser coverage also includes UI modules and currently remains
below 99% lines and 98% branches; that broader TypeScript coverage target is
not met. CI and Coveralls were pending in this local record; no later result is inferred. The local results do
not establish deployment or public-host qualification. Folder-hierarchy import
is outside this scope and tracked in [#120](https://github.com/eNeRGy164/wireloom/issues/120).

## Retention and applicability

This page preserves the dated narrative, not a newly executed qualification.
The original ignored local reports are not retained with this page. The source
revision/diff hash, bundle hashes, and raw outcomes cannot be reconstructed from
these counts. Reproduce through the [current runbook](../playground-qualification.md)
and archive the matching report bundle before extending a claim.

Publication has its own [deployment checks](../../guides/playground-deployment.md).
