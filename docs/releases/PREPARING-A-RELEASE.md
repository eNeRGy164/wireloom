# Preparing a release

A release has three stages: merge preparation, create the GitHub Release, then
merge a cleanup update with the actual release details.

## 1. Prepare and merge

- Choose the planned SemVer and tag, such as `0.5.0` and `v0.5.0`. Set
  `<WireloomVersion>` in `Directory.Build.props` to the version without the
  `v` prefix. Both the generator and its bundled compiler assembly use this
  shared version.
- Update examples in `src/Wireloom.Dds.Generator/README.md`, which ships in the
  package. Add the predicted version to `docs/releases/README.md` and create
  `docs/releases/<version>.md` with the source SHA and release details pending.
- Prepare a catchy release title and a summary of changes since the previous
  release.
- Merge all preparation changes to `main`. Run the
  [Quality workflow](../../.github/workflows/quality.yml) and
  [wire-compatibility workflow](../../.github/workflows/wire-compatibility.yml)
  on that exact merged commit. If it changes, merge and rerun both workflows.

Keep consumer references at the latest published version until the new package
is available. This includes `Directory.Packages.props`,
`samples/HelloWorld/packages.lock.json`, and examples in `README.md`,
`docs/guides/hello-world.md`, `docs/migration/rtiddsgen-to-wireloom.md`,
`samples/HelloWorld/README.md`, and
`tests/Wireloom.Dds.Generator.PackageIntegration/README.md`. Quality restores
the checked-in sample in locked mode, so advance these references after publish.

## 2. Create the release

In GitHub Releases, create a release for `vMAJOR.MINOR.PATCH` at the tested
`main` commit. Use the prepared title and change summary. Creating the release
creates the tag; GitVersion derives the package version from it, with no
separate confirmation. The tag triggers the release workflow. Check its result
and confirm the NuGet package is published.

## 3. Clean up and merge

- Replace pending fields in the version record with the actual tag SHA,
  publication link, workflow links, and results from tests on that source. Keep
  later `main` test runs out of the tagged version record.
- Archive the wire reports under
  `docs/testing/wire-compatibility/evidence/` and link them from the record.
- Advance `Directory.Packages.props` and refresh
  `samples/HelloWorld/packages.lock.json` with restore. Update the consumer
  examples listed above and current-version statements in
  `docs/architecture/arc42/07-deployment-view.md`,
  `docs/architecture/arc42/10-quality-requirements.md`, and
  `docs/architecture/architecture-memory.md`. Refresh
  `.agents/architecture-memory.yaml` if its source chapters change.
- Measure the published version's `.nupkg` and update the root README package
  size with that version and byte count. Do not present an older package's size
  as current.
- Run Quality against the published package references, then merge the cleanup
  changes to `main`. Keep historic version records and measurements tied to the
  artifacts they describe.

GitHub Releases and NuGet package contents are immutable; correct their
repository records in a cleanup commit instead of editing the published
artifacts.
