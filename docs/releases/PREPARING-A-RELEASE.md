# Preparing a release

A release has three stages: merge preparation, create the GitHub Release, then
merge a cleanup update with the actual release details.

For the first Modern C++ release, also complete the
[C++ qualification gates](#first-modern-c-release).
Record the C++ package and its native matrix explicitly; a C# package release
or generation-only candidate does not qualify the native application workflow.

## 1. Prepare and merge

- Choose the planned SemVer and tag, such as `1.0.0` and `v1.0.0`. Set
  `<WireloomVersion>` in `Directory.Build.props` to the version without the
  `v` prefix. Both the generator and its bundled compiler assembly use this
  shared version.
- Match the private Playground package version in `playground/package.json`
  and the root package metadata in `playground/package-lock.json` to
  `<WireloomVersion>`. The browser build and deployment manifest read the
  version from `Directory.Build.props`.
- Update examples in `src/Wireloom.Dds.CSharp.Generator/README.md`, which ships in the
  package. Add the predicted version to `docs/releases/README.md` and create
  `docs/releases/<version>.md` with the source SHA and release details pending.
- Review the C++ package's
  [acquisition and upgrade instructions](../../src/Wireloom.Dds.Cpp.Generator/README.md#obtain-and-pin-the-generator).
  Both package IDs use the selected release version. Before the first C++
  publication, configure NuGet.org trusted publishing for
  `Wireloom.Dds.Cpp.Generator` under the release workflow's existing OIDC
  identity, and verify its GitHub Packages repository access. Check a preview
  installation with an explicit version on each host, then change the pin and
  roll back using the same consumer project. Keep the package, hashes, and logs.
- Review [unshipped diagnostics](../../src/Wireloom.Dds.CSharp.Generator/AnalyzerReleases.Unshipped.md)
  against the descriptor catalog. Move newly released entries into
  [shipped diagnostic history](../../src/Wireloom.Dds.CSharp.Generator/AnalyzerReleases.Shipped.md)
  under the chosen release; record verified changes/removals without rewriting
  earlier releases.
- Prepare a catchy release title and a summary of changes since the previous
  release.
- Merge all preparation changes to `main`. Run the
  [Quality workflow](../../.github/workflows/quality.yml) and
  [wire-compatibility workflow](../../.github/workflows/wire-compatibility.yml)
  on that exact merged commit. If it changes, merge and rerun both workflows.
  Inspect both combined wire reports for unexpected failures. Keep the
  combined artifacts available for publication and archival. The publisher
  checks successful main runs for the tag SHA and both wire artifacts.

Keep consumer references at the latest published version until the new package
is available. This includes `Directory.Packages.props`,
`samples/HelloWorld/packages.lock.json`, and examples in `README.md`,
`docs/guides/hello-world.md`, `docs/migration/rtiddsgen-to-wireloom.md`,
`samples/HelloWorld/README.md`, and
`tests/Wireloom.Dds.CSharp.Generator.PackageIntegration/README.md`. Quality restores
the checked-in sample in locked mode, so advance these references after publish.

### First Modern C++ release

The [C++ release promise](../guides/support-and-compatibility.md#modern-c-support-and-compatibility)
defines public stability, platform/profile targets, and capability boundaries.
Qualify that scope before creating the first stable C++ release.

Use the [C++ Hello World walkthrough](../guides/hello-world.md#modern-c-walkthrough)
as the first package-to-application exercise. Extract its three source files
into a fresh directory, acquire the exact candidate package, and follow the
native build/run steps on each advertised configuration. Retain the package
hash, compiler/CMake versions, RTI SDK build and platform identifier, generated
files, clean and unchanged build logs, both peer logs, and exit codes. Verify
all ten payloads and both completion lines. Check Ctrl+C and a missing-peer
timeout too. A successful package-only generation check does not replace this
application exercise.

Include timestamp-only and comment-only edits to root and included IDL in
incremental-build qualification. Each edit must generate once and then skip
generation on the next unchanged build, including when emitted text stays
identical. Also verify regeneration of deleted outputs and retries after a
failed generator invocation. The [package checks](../../tests/Wireloom.Dds.Cpp.Generator.PackageIntegration/README.md)
exercise these cases with the candidate's bundled CMake integration.

The maintainers own the C++ support contract and versioned release record. Before
publication, that record must identify the package/version, source revision,
artifact hashes, exact supported configurations, capability levels, exclusions,
and retained evidence. There are no qualified first-release matrix entries
until those results exist.

| Gate                     | Required completion evidence                                                                                                                                                                                                      |
| :----------------------- | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Native delivery          | [Package checks](../../tests/Wireloom.Dds.Cpp.Generator.PackageIntegration/README.md) pass for both bundled host tools using the exact candidate bytes; preview/release publication includes the C++ package.                     |
| Complete native workflow | Fresh package-based walkthroughs build and exchange samples on each advertised Windows/MSVC and Linux/GCC configuration. Exercise includes, options, clean/rebuild, unchanged builds, and supported configuration changes.        |
| Capability qualification | Exact-profile consumer, lifecycle, CDR, and mixed-peer reports establish every advertised level. Inspect individual wire outcomes: a completed green workflow alone is insufficient.                                              |
| Compatibility regression | Relevant tests, branch/line coverage, Community Qodana, CI/Coveralls, and shared-front-end C# and Playground checks pass on the release revision.                                                                                 |
| RTI adapter review       | Record the outcome of the internal-interface support/licensing review retained in [ADR-006](../architecture/adr/ADR-006-use-stl-aware-sample-access-for-modern-cpp.md), together with applicable ABI guards and SDK restrictions. |
| Release documentation    | Publish the version-specific matrix, known limits, migration/rollback guidance, and exact evidence links under the [release cleanup](#3-clean-up-and-merge).                                                                      |

Missing evidence is a release gap. Resolve it, or narrow the published scope
and state the exclusion explicitly; do not promote generation/API evidence
into a runtime claim. Keep any generation-only capabilities separately labeled.

## 2. Create the release

In GitHub Releases, create a release for `vMAJOR.MINOR.PATCH` at the tested
`main` commit. Use the prepared title and change summary. Creating the release
creates the tag; GitVersion derives the package version from it, with no
separate confirmation. The tag triggers the release workflow, which
builds both native tools at the selected version, verifies the combined C++
package on both hosts, and smoke-tests the C# package before attesting and
publishing those bytes. Check its result and confirm both package IDs are
published at that version. Publishing to two package IDs is not atomic; if
one push fails, rerun the failed publication job while its artifact is retained.
That job downloads and checks the retained hashes without rebuilding. Existing
versions are skipped rather than replaced. Confirm both feed versions before
announcing the release.

## 3. Clean up and merge

- Replace pending fields in the version record with the actual tag SHA,
  publication link, workflow links, and results from tests on that source. Keep
  later `main` test runs out of the tagged version record.
- Archive the publishing artifact's C# and C++ packages, `SHA256SUMS`, and SBOM,
  plus provenance links and both native consumer artifacts. Record the exact
  C++ package hash and acquisition links in the version record. Preview
  publication and native consumer artifacts expire after 14 days; release
  publication artifacts after 90 days. Copy evidence needed beyond those
  windows to the version's permanent archive.
- Archive the wire reports under
  `docs/testing/wire-compatibility/evidence/<version>/` and link them from the
  record. Preserve the original sanitized combined archives, extract readable
  reports without changing their bytes, and record workflow/artifact IDs, exact
  source SHA, file sizes, and SHA-256 digests. Verify the archive digests against
  GitHub metadata before retention; retain failures as well as passes. See the
  [1.0.0 evidence record](../testing/wire-compatibility/evidence/1.0.0/README.md).
- Advance `Directory.Packages.props` and refresh
  `samples/HelloWorld/packages.lock.json` with restore. Update the consumer
  examples listed above and current-version statements in
  `docs/architecture/arc42/07-deployment-view.md` and
  `docs/architecture/arc42/10-quality-requirements.md`. Refresh only
  `.agents/architecture-memory.yaml` if its source chapters change.
- After the first C++ publication, replace the unpublished-package notice in
  `src/Wireloom.Dds.Cpp.Generator/README.md` with the actual version and release
  record. Verify its acquisition commands against the published package.
- Measure the published version's `.nupkg` and update the root README package
  size with that version and byte count. Do not present an older package's size
  as current.
- Run Quality against the published package references, then merge the cleanup
  changes to `main`. Keep historic version records and measurements tied to the
  artifacts they describe.

GitHub Releases and NuGet package contents are immutable; correct their
repository records in a cleanup commit instead of editing the published
artifacts.
