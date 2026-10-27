# 7. Deployment view

Wireloom is deployed as a NuGet analyzer/source-generator package rather than
as a service. The generated application still deploys with the consumer's
selected RTI Connext DDS runtime. The browser Playground is a separate static
deployment that carries a TypeScript client and .NET WebAssembly worker assets.

Effective finite limits for unbounded IDL values are build-time MSBuild
properties with per-file item metadata overrides. They are compiled into the
generated type metadata and do not add consumer runtime configuration.
`DdsIdlEnableEscapeChar` is a build-time project property that changes emitted
names and implicit hash member IDs when enabled; it has no runtime switch or
per-file override.

## 7.1 Infrastructure level 1

![Deployment view showing the developer or CI build host, NuGet feeds, generated application, RTI runtime, and compatibility evidence.](images/07/deployment-overview.png)

| Node                       | Deployed material                                                      | Role                                                                    |
| :------------------------- | :--------------------------------------------------------------------- | :---------------------------------------------------------------------- |
| Developer or CI build host | .NET 10 SDK, consumer project, IDL, Wireloom package, compiler project | Builds the shared core and adapter; runs generation and tests           |
| NuGet feed                 | `Wireloom.Dds.Generator` package and dependencies                      | Supplies MSBuild targets, adapter, and bundled compiler assembly        |
| Consumer output            | Application assembly plus generated source compiled into it            | Runs the DDS application                                                |
| RTI runtime environment    | Application-selected `Rti.ConnextDds` libraries                        | Owns DDS communication and serialization                                |
| Evidence repository        | Corpus IDL, retained oracle sources, and wire-compatibility peers      | Supports source-shape and DDS wire verification, not production runtime |

The Quality workflow continuously checks both exact RTI LTS packages on
Windows 2025 and Ubuntu 26.04. Its eight runtime/package jobs combine RTI
7.3.1 and 7.7.0 with consumer targets `net8.0` and `net10.0`; each packs a fresh
analyzer and compiles the package consumer with C# 12. Two separate corpus
jobs select the matching retained LTS oracles and verify applicability and
shard reporting. These jobs need no RTI license secret and retain runtime and
package test logs. A separate Ubuntu job copies the freshly packed analyzer and
consumer fixture into an `mcr.microsoft.com/dotnet/sdk:8.0.100` container, which
contains no .NET 10 SDK or runtime. It restores, compiles, and runs the broad
package suite with the .NET 8 compiler host for each RTI LTS package. The
repository SDK remains selected by `global.json` when producing the package.
See
[continuous LTS verification](../../guides/support-and-compatibility.md#continuous-lts-verification).

The manually triggered workflow runs eight whole-case shards for each qualified RTI profile. Licensed Docker jobs invoke the C# Runner to build selected peers and schedule domain-isolated exchanges; peer processes disable the unused RTI Monitoring Library. A license-free Verifier job checks shard evidence and produces the combined report. Build concurrency, exchange concurrency, container images, artifacts, and failure handling are documented in the [wire compatibility runbook](../../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#case-shards-and-combined-reports).
The checked-in samples/HelloWorld project consumes the released
Wireloom.Dds.Generator 0.5.0 package from NuGet.org and builds on Ubuntu 26.04
and Windows 2025. This compile-only CI check needs no RTI native library or
license; the sample README documents the licensed local DDS exchange.

`Wireloom.Dds.Compiler` is a non-packable `netstandard2.0` project. Its
assembly is built at the same version as the generator and is packed beside
`Wireloom.Dds.Generator.dll` under `analyzers/dotnet/cs`. The package remains
the sole published package and has no compiler package dependency. The
consumer loads both assemblies through the analyzer package and does not gain
an application runtime reference to the compiler. Repository project
references also build and load the compiler beside the generator.

## 7.2 Browser Playground deployment

The non-packable `Wireloom.Playground.Browser` project publishes the shared
compiler for `browser-wasm` using the pinned .NET 10.0.13 browser runtime. Its
published `_framework` assets and worker entry are copied into the Vite static
bundle. The npm project pins Node 26.7.0, npm 12.2.0, Vite 7.3.7, and
TypeScript 6.0.3 in `package.json` and `package-lock.json`. Vite builds with a
configurable base path so client chunks and the module worker can be served at
the site root or under a nested path.

The Quality workflow runs the pinned TypeScript compiler, strict ESLint,
Prettier, Node tests with branch/line/function thresholds, and a production
browser coverage run using Playwright's Chromium headless shell. Browser V8
coverage is source-mapped to first-party client code and merged with Node
coverage into Cobertura; a focused test run reports the linked C# adapter.
Coveralls receives those reports with the existing compiler and generator
reports. Community Qodana scans the .NET solution, including the browser
adapter. The workflow also builds and checks the root and nested static bundles.

Stage 1 adds locally bundled Monaco 0.57.0 and fflate 0.8.3. DOMPurify is
overridden to patched 3.4.17 until Monaco's upstream pin is updated. The build
emits upstream licenses/notices, an editor worker, a small bootstrap, and the
editor application chunk. The latter is about 719 kB gzip and exceeds Vite's
default chunk-size advisory; public-network loading remains release evidence
work. The footer records the shared Wireloom version and Git revision,
including a dirty-checkout suffix. Serve the complete `dist/` tree with its
matching `dotnet/` assets; partial asset replacement is not supported.

The current clean Release publish succeeds with interpreter assets and no
AOT/native relinking. The .NET runtime tree contains 63 files totaling
14,208,318 uncompressed bytes; the client bundle's `dotnet/` directory has 65
files and 14,211,428 bytes including the worker and MD5 module. On Windows Edge
156.0.4314.42, the worker resources transferred 5,798,222 bytes per hosting
path (5,792,822 encoded bytes). Fresh isolated-context readiness measured 149
ms at root and 159 ms at `/playground/`; reload readiness measured 124 ms and
122 ms. A 128-declaration compile that emitted the 512-document cap took
357.1 ms. The measured heartbeat maximum during that compile was 16.5 ms, and
a main-thread edit took 30.7 ms. These timings describe one representative
Edge host and are not cross-browser guarantees.

The accepted deployment requires static assets and workers to be served from
the same origin. In the observed Edge run, application requests were GETs to
loopback and no outbound request carrying source was observed. This is scoped
request evidence, not a general telemetry or privacy certification. Edge, Firefox,
and Playwright WebKit runs cover representative client flows; Safari is not
claimed. Browser qualification and its limits are recorded in the
[qualification scope](../../testing/playground-qualification.md). Public-network
evidence comes from the deployment checks below.

GitHub Pages is the selected public host. Quality builds a separate
`dist-pages/` candidate at `/wireloom/` by default, with an optional
`PLAYGROUND_BASE_PATH` repository variable for a later custom hostname.
PR builds validate and retain this candidate without publishing. The publication
workflow accepts only successful push Quality runs from this repository's
`main`, waits for Coveralls, and promotes the exact artifact after checking
its full revision, base path, and complete file inventory with SHA-256 hashes.
Qodana and other Quality jobs must also have passed. Automatic publication
rejects superseded revisions; serialized manual runs can redeploy a retained
successful artifact for rollback. Only the deploy job receives Pages write
and OIDC permissions, within the `github-pages` environment restricted to main.

After deployment, isolated Chromium checks the HTTPS host's complete asset
bytes, WASM/module MIME types, sample generation, diagnostics, footer, copy,
downloads, ZIP, and reload. Reports record the actual deployed URL and source
revision. A failure marks publication verification unsuccessful and requires
inspection or the documented rollback; it is not a successful public-host
qualification. GitHub's deployment revision is the publishing workflow's
revision; `deployment.json` identifies the artifact's source, including manual
rollback. All runtime assets remain same-origin static downloads. See the
[deployment runbook](../../guides/playground-deployment.md) and
[action intake](../dependency-intake/playground-pages-actions.md).

Preview publication uses GitHub Packages. Validated release tags publish to
NuGet.org with SBOM and provenance attestations.

> [!NOTE]
> Additional information: This architecture context is also summarized in
> [Architecture memory](../architecture-memory.md) and
> `.agents/architecture-memory.yaml` so implementation agents can use it
> efficiently in limited context windows.
> The arc42 chapters, linked ADRs, and contracts remain the primary source
> documents. If the derived memory differs, update the source docs first and
> refresh the derived memory.
