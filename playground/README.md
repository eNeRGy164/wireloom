# Browser Playground

This folder contains the multi-file workspace client, its checks, and the
maintained worker boundary. The non-packable .NET host is in
[`../src/Wireloom.Playground.Browser/`](../src/Wireloom.Playground.Browser/).
The host and client share the `Wireloom.Dds.Compiler`; no main-thread compiler
or fallback provider exists.

For the retained-corpus matrix, reproducible release verification command,
workspace behavior, compiler settings, and evidence boundaries, see
[Playground qualification](../docs/testing/playground-qualification.md).

The public [Playground](https://energy164.github.io/wireloom/) is hosted on
GitHub Pages. Pull requests build and verify a deployment candidate without
changing the public site. Successful Quality runs from pushes to `main` can
publish their validated artifact. See the
[deployment runbook](../docs/guides/playground-deployment.md) for setup,
publication gates, live verification, rollback, and custom domains.

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

## Repository layout

| Location                        | Responsibility                                                                       |
| :------------------------------ | :----------------------------------------------------------------------------------- |
| `src/`                          | Workspace UI/model, compiler client, worker state, local recovery, and source export |
| `src/protocol.ts`               | TypeScript mirror of the browser adapter DTOs                                        |
| `src/worker.mjs`, `src/md5.mjs` | Worker-local compiler startup and synchronous MD5 provider                           |
| `scripts/verification/`         | Native baseline export, prepared browser phases, and qualification orchestration     |
| `scripts/*pages*`               | Deployment candidate manifest, build, and verification                               |
| `public/dotnet/`                | Generated browser-host assets replaced by the build                                  |

## Choose a workflow

- [Use the workspace](../docs/guides/playground-workspaces.md): files, folders,
  roots, settings, exports, and local recovery.
- [Maintain the worker boundary](../docs/reference/playground-protocol.md):
  mirrored DTOs, identity, limits, and error handling.
- [Qualify the browser release](../docs/testing/playground-qualification.md):
  native parity, actual UI operations, fault handling, and browser scope.
- [Publish or roll back](../docs/guides/playground-deployment.md): candidate
  provenance, live HTTPS checks, and hosting configuration.

The client compiles all selected Roots and supplied includes locally through
the shared compiler. Preview results establish generated text and diagnostics;
they do not qualify RTI compilation, native behavior, or DDS exchanges.

## Reproduce browser verification

Use `npm run verify:release -- chromium` after building. The
[qualification runbook](../docs/testing/playground-qualification.md#reproduce)
owns the sequential browser commands, phases, report locations, and pass rules.
For individual Playwright CLI phases and managed-window troubleshooting, use
[advanced browser verification](../docs/testing/playground-cli.md).

[Documentation](../docs/README.md) · [Contribution and quality gates](../CONTRIBUTING.md#choose-validation)
