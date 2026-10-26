# Playground deployment

The [Playground](https://energy164.github.io/wireloom/) uses GitHub Pages.
The TypeScript client, Monaco worker, .NET browser-WASM runtime, and compiler
worker are published together. No server compiler or RTI license is required
to run the website. See [source and limitations](../../playground/README.md)
and [generation qualification](../testing/playground-qualification.md).

## Repository setup

1. In repository **Settings > Pages**, select **GitHub Actions** as the build
   and deployment source. The initial expected URL is
   `https://energy164.github.io/wireloom/`.
2. Configure the `github-pages` environment to allow deployment only from the
   `main` branch. Keep the existing protection and review rules on `main`.
3. The optional repository variable `PLAYGROUND_BASE_PATH` defaults to
   `/wireloom/`. Use `/` when this repository has its own custom hostname.
   The publisher verifies this value against the actual Pages URL before
   deploying; a mismatch requires a new Quality build.
4. Merge the deployment workflow through the usual reviewed pull request.
   The first successful push Quality run containing the candidate artifact
   can publish. Earlier runs without that artifact cannot publish this site.

The workflow uses `GITHUB_TOKEN` and GitHub's OIDC token, with no personal
deployment credential. Only the deployment job has `pages: write` and
`id-token: write`.

## Pull requests and publication

The [Quality workflow](../../.github/workflows/quality.yml) retains root and
nested-path qualification, builds `playground/dist-pages/` at the configured
base, and runs a local browser smoke test against that exact bundle. The
`playground-pages` artifact is retained for 30 days. PR builds do this too;
they do not update the public website or receive a separate preview URL.

[`publish-playground.yml`](../../.github/workflows/publish-playground.yml)
starts after Quality completes on `main`. It independently checks the run's
repository, workflow path, event, branch, full source SHA, and successful
conclusion, then waits for `coverage/coveralls (push)` to succeed. Qodana and
the other Quality jobs must already have passed. Automatic publication skips
a superseded source revision, including a second check just before deployment.
Deployments are serialized.

The publisher downloads that specific run's candidate and verifies every file
against `deployment.json`: full source SHA, displayed revision, Wireloom
version, base path, tool versions, byte lengths, and SHA-256 hashes. Added,
missing, changed, or linked files are rejected. It packages the existing bytes
with the official Pages actions and does not rebuild or use the coverage
bundle. The complete `dotnet/_framework/` tree is preserved without Jekyll or
Git line-ending conversion. The manifest itself is served beside `index.html`.
GitHub's deployment record identifies the publishing workflow revision; the
manifest and workflow summary identify the validated source revision, including
when rolling back to an older source.

Publication is followed by an isolated Chromium check on the actual HTTPS URL.
It waits for the expected manifest, compares all deployed asset bytes and
WASM/module MIME types, records cache/encoding headers, and exercises cold
startup, the default Hello sample on both profiles, the version/preview footer,
copy, individual and ZIP downloads, reload, and invalid-IDL diagnostics.
The canonical site URL is also the sample URL: a fresh context automatically
generates Hello. Arbitrary snippet links are outside this deployment scope.

The `playground-public-verification` artifact retains the report for 30 days,
including failures. A failed check makes the publication workflow fail. It
does not silently roll back or certify the public site; inspect the report and
use the rollback procedure below. Existing browser qualification boundaries
still apply, including that Playwright WebKit does not qualify Safari.

## Local verification

From `playground/`, use the repository-pinned toolchain and locked dependencies:

```shell
npm ci --ignore-scripts
npm run build
npm run check:paths
npm run build:pages
npm run verify:pages
```

`build:pages` reuses the Release runtime from `build` and produces a separate
deployment bundle. `verify:pages` owns a preview server on port 4185 at the
manifest's base path; an occupied port is an error. It writes
`output/playwright/pages-smoke.json`. The report's `passed` field must be true.
Use the matching Playwright Chromium installation (`npx playwright install
--only-shell chromium`) if needed.

To compare a public deployment with a downloaded candidate artifact, place the
candidate in `playground/dist-pages/` and run:

```shell
npm run verify:pages -- https://energy164.github.io/wireloom/
```

`EXPECTED_SOURCE_SHA` optionally requires a particular full source SHA.
`PLAYGROUND_PAGES_DIRECTORY` selects another candidate directory relative to
`playground/`. Public verification requires HTTPS and a URL whose path exactly
matches the manifest. All browser contexts are isolated from visitor recovery
data. The existing `verify:release` corpus qualification remains separate and
unchanged.

## Rollback

1. Choose a successful **push** Quality run from this repository's `main`
   branch whose `playground-pages` artifact still exists. Check its SHA and
   original verification evidence. Keep the base path compatible with the
   currently configured hostname.
2. In **Actions > Publish Playground > Run workflow**, select `main` and enter
   that Quality run ID. Alternatively:

   ```shell
   gh workflow run publish-playground.yml --ref main -f quality_run_id=RUN_ID
   ```

3. The same provenance, Coveralls, hash, base-path, environment, and live
   verification gates apply. Manual runs explicitly allow an older validated
   source revision. Inspect the public verification report and displayed SHA.
4. If the artifact expired or uses an obsolete base path, prepare a reviewed
   revert on `main` and let a new successful Quality run build and publish it.

## Custom domain later

Keep the existing DNS provider. Verify domain ownership, configure the custom
hostname in Pages settings, and point its DNS records to GitHub Pages. For a
subdomain, the CNAME target is `energy164.github.io`, without `/wireloom/`.
With Actions publishing, a repository `CNAME` file is not required. Set
`PLAYGROUND_BASE_PATH` to `/` and publish a newly validated bundle during the
cutover. Wait for certificate provisioning, enforce HTTPS, and repeat the live
checks. Follow [GitHub's custom-domain instructions](https://docs.github.com/en/pages/configuring-a-custom-domain-for-your-github-pages-site/managing-a-custom-domain-for-your-github-pages-site).

Changing hostname changes the browser origin. Saved IndexedDB workspaces and
localStorage preferences stay on the previous origin; advise visitors to
download their IDL first and import it at the new URL. Keep a rollback path for
both DNS and the corresponding base-path bundle. Pages has limited response
header control; revisit Azure Static Web Apps if future shared-memory or
response-header requirements exceed this static deployment.
