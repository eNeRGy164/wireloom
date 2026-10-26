# Publish the Playground: issue #111

- **Issue:** [Publish the Wireloom Playground as a static website](https://github.com/eNeRGy164/wireloom/issues/111)
- **Status:** GitHub Pages selected; implementation and rollout tracked below.
- **Hosting selection date:** October 26, 2026.
- **Research date:** October 26, 2026.

## Selected hosting

The implementation adds a validated Pages candidate to Quality and a separate
publisher with main-only provenance gates, artifact hash checks, rollback, and
public-host verification. See the [deployment runbook](../guides/playground-deployment.md).
CI and the first public deployment must pass before issue completion.

Use GitHub Pages for the initial public Playground. Michaël Hompus selected
this host on October 26, 2026. Add a custom domain later when needed, keeping
the DNS provider independent of the website host. Azure Static Web Apps remains
an alternative if deployed verification exposes a host limitation. No compiler
service or new application stack is needed.

The expected project URL is `https://energy164.github.io/wireloom/`, subject to
the account's existing Pages/custom-domain configuration. Pages supports
project sites directly from their repository and is available for public
repositories on GitHub Free. [GitHub Pages overview](https://docs.github.com/en/pages/getting-started-with-github-pages/what-is-github-pages)

| Option                       | Custom domain later                                                | Reasons to choose it                                                                                   |
| ---------------------------- | ------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------ |
| GitHub Pages                 | Yes, with HTTPS and DNS at the existing provider.                  | Fits the existing repository and static build with the fewest resources to operate.                    |
| Azure Static Web Apps        | Yes, with automatically managed TLS certificates and external DNS. | Adds configurable response headers, MIME mappings, routing, and preview environments.                  |
| Azure Storage static website | Yes; HTTPS on a custom domain needs Azure Front Door or Azure CDN. | Consider when a storage/edge deployment already exists; otherwise it adds resources for this use case. |

GitHub documents custom-domain HTTPS support. Azure documents managed
certificates for Static Web Apps and Front Door/CDN requirements for HTTPS on
Storage custom domains. [GitHub HTTPS](https://docs.github.com/en/pages/getting-started-with-github-pages/securing-your-github-pages-site-with-https),
[Static Web Apps domains](https://learn.microsoft.com/en-us/azure/static-web-apps/custom-domain),
[Storage custom domains](https://learn.microsoft.com/en-us/azure/storage/blobs/storage-custom-domain-name)

GitHub Pages allows a published site up to 1 GB and has a soft monthly bandwidth
limit of 100 GB. Static Web Apps Free allows a 250 MB app, two custom domains,
three staging environments, and no SLA. The current runtime assets are about
14 MB uncompressed; measure the complete release bundle before deployment.
[Pages limits](https://docs.github.com/en/pages/getting-started-with-github-pages/github-pages-limits),
[Static Web Apps plans](https://learn.microsoft.com/en-us/azure/static-web-apps/plans),
[current deployment evidence](../architecture/arc42/07-deployment-view.md#72-browser-playground-deployment)

## Adding our own domain later

The DNS provider and website host can remain separate. For a future hostname
such as `playground.example.com`:

1. Verify domain ownership using GitHub's DNS TXT challenge.
2. Set the custom hostname in this repository's Pages settings, then add a DNS
   CNAME targeting `energy164.github.io`, without `/wireloom/`.
3. Build the application with base path `/` instead of `/wireloom/` and deploy
   the matching bundle as part of the domain cutover.
4. Wait for certificate provisioning, enforce HTTPS, and repeat the live checks.

The hostname is illustrative; selecting or purchasing a domain is a later
decision. With an Actions publishing source, GitHub uses the Pages setting and
does not require a repository `CNAME` file. Apex domains are also supported
using GitHub's documented DNS records. [Domain verification](https://docs.github.com/en/pages/configuring-a-custom-domain-for-your-github-pages-site/verifying-your-custom-domain-for-github-pages),
[domain configuration](https://docs.github.com/en/pages/configuring-a-custom-domain-for-your-github-pages-site/managing-a-custom-domain-for-your-github-pages-site),
[Vite base paths](https://vite.dev/guide/static-deploy.html#github-pages)

## Hosting checks that determine the choice

- Publish the complete `playground/dist/` artifact through the Pages Actions
  workflow (the proposed deployment build below uses `dist-pages/`). Keep the
  `dotnet/_framework/` tree and all published runtime files
  intact. Avoid rebuilding with Jekyll or committing generated runtime assets,
  which can omit underscore directories or alter bytes through Git line-ending
  conversion. Check deployed asset hashes rather than assuming integrity.
  Microsoft's WebAssembly hosting guidance describes both failure modes;
  its Blazor-specific loader instructions are not needed for this Vite client.
  [Pages Actions](https://docs.github.com/en/pages/getting-started-with-github-pages/using-custom-workflows-with-github-pages),
  [WebAssembly deployment notes](https://learn.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/webassembly/github-pages?view=aspnetcore-10.0)
- Verify `.wasm` MIME types, module-worker loading, response encoding, cache
  behavior, and cold/warm startup on the actual HTTPS host. Do not assume that
  published `.br` files will be served with content negotiation. Preserve the
  runtime's integrity behavior and deploy each client/runtime bundle together.
  [WebAssembly deployment notes](https://learn.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/webassembly/github-pages?view=aspnetcore-10.0)
- Treat Pages as a host without configurable response headers. A Pages team
  response confirms this limitation. The existing worker uses message passing;
  it does not require WebAssembly threading or shared-memory isolation. If a
  future requirement needs COOP/COEP or precise cache/security headers, revisit
  Azure Static Web Apps, which exposes header and MIME configuration.
  [Pages team response](https://github.com/orgs/community/discussions/54257),
  [worker source](../../playground/src/worker.mjs),
  [Static Web Apps configuration](https://learn.microsoft.com/en-us/azure/static-web-apps/configuration)

## Implementation plan

### 1. Establish the deployment target

- Check the account's existing Pages site and inherited custom-domain settings.
  Confirm the actual URL before putting it in the README; the expected initial
  base path is `/wireloom/`.
- Enable this repository's Pages publishing source as GitHub Actions during
  rollout. Use the `github-pages` environment with deployment restricted to
  `main`. Domain purchase and DNS changes are a later step.
- Deploy reviewed revisions from `main` after their Quality run succeeds.
  Record the full source SHA, Wireloom version, build tools, base path, and
  bundle hashes with the deployment evidence.

### 2. Produce and verify the deployment bundle

- Reuse `global.json`, `.node-version`, the pinned npm version, locked NuGet
  restore, and `npm ci --ignore-scripts`. Reuse the existing browser publish;
  add no new application project, package, or compiler service.
- Preserve the existing root `dist/` and `/playground/` `dist-subpath/` checks.
  Build a separate `dist-pages/` with Vite's base set to the confirmed Pages
  path. Make the base configurable so a custom domain can use `/` later.
- Extend the bundle verifier to check the actual deployment path, including
  client chunks, Monaco worker, .NET module worker, MD5 module, `_framework/`,
  and third-party notices. Check the complete bundle size against host limits.
- Keep the current version/revision footer and preview limitations visible.
  Verify the packaged footer reports the intended clean source revision.
- Run a browser smoke check against this deployment bundle mounted at its
  real base path, then retain that exact bundle and its SHA-256 manifest as a
  Quality artifact. Keep source-mapped coverage output separate.

### 3. Publish only a validated artifact

- Add `.github/workflows/publish-playground.yml`, triggered by a successful
  `Quality` run for a push to this repository's `main` branch. Verify the
  originating repository, event, branch, conclusion, and exact source SHA;
  never publish artifacts from pull-request runs.
- Download the deployment artifact from that specific run. Promote the
  checked bytes rather than rebuilding them after validation. Keep privilege
  out of build/test steps; only the deployment job needs `pages: write` and
  `id-token: write`, with `actions: read` where artifact retrieval requires it.
- Use the official Pages artifact/deployment actions and repository action
  pinning conventions. Review new action intake and regenerate
  `.github/workflows/actions.lock` using its documented tooling.
- Give jobs explicit runners and timeouts. Serialize publication to prevent
  overlapping deployments and reject superseded runs so an older build cannot
  replace a newer deployment. Report the deployed URL and SHA in the summary.
- Document rollback by redeploying a retained, successful artifact from a
  reviewed `main` revision. Verify its hashes and run the same live checks.
  Rebuild and revalidate that revision if its artifact has expired.

### 4. Verify the public site

- Run a bounded HTTPS readiness check followed by isolated browser checks
  against the deployed URL. Confirm the expected revision before exercising
  the UI; retain failures as well as passing reports.
- Check page and worker startup, default Hello sample generation on both RTI
  profiles, a visible invalid-IDL diagnostic, profile changes, copy, individual
  download, and ZIP contents. Exercise cold load and reload, and capture
  network/runtime errors and asset headers/hashes.
- Treat the deployed canonical URL as the sample URL: a fresh browser loads
  and generates the built-in Hello example. The client currently has no
  example query-parameter routing. Shareable arbitrary snippets or additional
  sample-link behavior would be separate scope.
- Reuse the existing release qualification and its supported-browser limits.
  Adapt the runners for a remote deployment target: `qualify.mjs` currently
  starts local servers and uses fixed root/nested URLs. Keep local parity,
  corpus, failure-recovery, and coverage checks intact.
- Require the existing Quality gates, Community Qodana, and Coveralls to pass
  for implementation changes. Retain public-host smoke evidence with the
  source SHA, artifact hashes, URL, and browser version. A failed live check
  fails publication verification and triggers the documented rollback path.

### 5. Document and finish issue #111

- Add a concise live Playground link to `README.md`, beside its source and
  limitations. Update `playground/README.md` with publishing, verification,
  rollback, and future custom-domain instructions.
- Update arc42 chapter 7 and affected quality/risk references to distinguish
  public hosting from local qualification. Refresh derived architecture memory
  after those authoritative sources change. This proposal does not change the
  accepted runtime architecture or claim a deployed result.
- Complete the issue only after the actual public URL passes the checks above
  and the repeatable deployment path is documented.

## Custom-domain cutover considerations

Changing from `energy164.github.io` to a custom hostname changes the browser
origin. The current client stores recovery in IndexedDB and preferences in
localStorage, so existing saved workspaces will remain on the old origin;
they will not migrate through DNS. Document an IDL download/import step before
cutover. This is an inference from the client's storage design, not a hosting
service limitation. See the [client](../../playground/src/main.ts) and
[workspace storage](../../playground/src/workspace.ts).

Keep the DNS and base-path change together, preserve a rollback artifact for
the prior hostname/path, and rerun public-host verification after HTTPS is
ready. A custom domain does not require moving hosting to Azure.
