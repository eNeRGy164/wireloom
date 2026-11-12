# C++ generator package verification

Run from the repository root after assembling both host executables and packing:

```powershell
./tests/Wireloom.Dds.Cpp.Generator.PackageIntegration/Verify-Package.ps1 `
  -Package artifacts/package-test-feed/Wireloom.Dds.Cpp.Generator.1.0.1-preview.42.nupkg
```

The example version is a local candidate. The script reads the actual version
from the package manifest, including prerelease versions, and uses it in the
native NuGet manifest and imports; it does not assume version `1.0.0`.

The script checks the actual NuGet payload, runs the host executable with .NET
removed from PATH, compares generated files with the retained output for
`01-primitives` and `10-annotations`, and checks unsupported IDL diagnostics. Linux ZIP extraction restores
the executable permission before the direct CLI check.

Both hosts also load the packaged CMake configuration and verify generation
and incremental skipping through a target that requires no RTI headers. It
also installs the packaged CMake integration and host executable into a path
with spaces, switches the same consumer to that installation, and verifies a
clean rebuild.

The CMake checks edit private copies of root and included IDL. Timestamp-only
and comment-only edits must preserve all generated text, invoke generation
exactly once, and skip generation on the next unchanged build. Semantic edits
must update output. Deleting each generated file must regenerate it, and a
failed generation must retry until its input is repaired. These checks cover
the completion-stamp rule through observable build invocations.

Use `-CMakeGenerator Ninja` to run with Ninja instead of the host's default
generator. Supply its native compiler environment and have Ninja on PATH.
The package workflow checks Visual Studio on Windows and both Unix Makefiles
and Ninja on Linux.

To exercise a version change and rollback in the same consumer, retain the
previous package and pass it explicitly:

```powershell
./tests/Wireloom.Dds.Cpp.Generator.PackageIntegration/Verify-Package.ps1 `
  -Package artifacts/package-test-feed/Wireloom.Dds.Cpp.Generator.1.0.1-preview.43.nupkg `
  -PreviousPackage artifacts/package-test-feed/Wireloom.Dds.Cpp.Generator.1.0.1-preview.42.nupkg
```

This configures/builds the CMake consumer with the previous version, the new
version, and the previous version again. On Windows, it also changes the
`packages.config` version and project imports together, restores each pin from
an isolated local feed, and cleanly regenerates through MSBuild. Each step's
logs and the version transition are retained in the verification result.

On Windows, it restores the package through `packages.config` and invokes the
imported generation target with `Platform=ARM64` on an x64 host. It verifies
that a second unchanged invocation skips generation. The local restore
configuration accepts the unsigned candidate from a single local feed, with
no external source.

These checks require no RTI installation or license. The native fixture
documentation in the [wire harness](../Wireloom.Dds.Generator.WireCompatibility/README.md)
describes which generated shapes also have consumer, CDR, and live DDS evidence.

Evidence is retained under `artifacts/cpp-package-tests/`. The
[C++ package workflow](../../.github/workflows/cpp-package.yml) builds both native
payloads at one explicit version, assembles one candidate, and executes this
script on both hosts. The preview/release publishers call that same workflow
and publish its tested package after both consumer jobs pass.

The Windows CI generation check passes `-PlatformToolset v143` for its
Visual Studio 2022 runner. The licensed demo defaults to Visual Studio 2026
and `v145`; generation remains independent of the C++ target toolset.
