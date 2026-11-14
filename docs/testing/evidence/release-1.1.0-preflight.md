# Wireloom 1.1.0 release preflight

The November 14, 2026 audit examined merged preparation source
`30d93a4e8edac7b85c4d7ff35496475a9df547da` and the artifacts produced for it.
The [machine-readable record](release-1.1.0-preflight.json) retains counts,
artifact IDs/digests, candidate hashes, and outstanding gates.

**Release clearance remains blocked.** The audit found and corrected two
source issues. Merge the corrections and repeat qualification on the exact
source selected for the release. Native walkthrough checks passed with the
supplied RTI license. The owner is verifying NuGet.org trusted publishing.

## Verified preparation evidence

| Gate                               | Evidence                                                                             | Result                                                                                                                        |
| :--------------------------------- | :----------------------------------------------------------------------------------- | :---------------------------------------------------------------------------------------------------------------------------- |
| Quality and C# consumers           | [Quality run](https://github.com/eNeRGy164/wireloom/actions/runs/43634610009)        | Successful; all 12 Windows/Linux, RTI, and framework cells passed, along with the three .NET 8 minimum-host consumers.        |
| Coverage upload                    | [Coveralls build](https://coveralls.io/builds/83044550)                              | Successful for the preparation SHA.                                                                                           |
| Licensed wire exchange             | [Wire run](https://github.com/eNeRGy164/wireloom/actions/runs/43634609930)           | All 8,173 planned exchanges passed with clean, unchanged source provenance.                                                   |
| Native generator delivery          | [Native package run](https://github.com/eNeRGy164/wireloom/actions/runs/43634609928) | Both hosts verified the same 1.1.0 package bytes, including CMake and native MSBuild.                                         |
| Preview publication and provenance | [Preview run](https://github.com/eNeRGy164/wireloom/actions/runs/43635707711)        | Both packages published as `1.0.1-ci.32`; signed provenance verified against the preparation SHA.                             |
| Playground                         | [Publication run](https://github.com/eNeRGy164/wireloom/actions/runs/43635707650)    | Chromium qualification and public HTTPS verification passed. A fresh CLI fetch matched the retained deployment manifest hash. |

The live Playground reports version `1.1.0` and source `30d93a4e`. Its manifest
SHA-256 is `efea15b6f37962959d97cff8bd3eb75acdcfadba4d9de81f96a4957076fe8bce`.
The preview package version is independently calculated by GitVersion; it is
not a stable 1.1.0 publication.

### Wire scope

| Exact RTI profile | Passed / planned | Exchanges involving Wireloom C++ | Adapted exchanges |
| :---------------- | ---------------: | -------------------------------: | ----------------: |
| 6.1.2             |    2,521 / 2,521 |                            1,093 |               304 |
| 7.3.1             |    2,829 / 2,829 |                            1,223 |               374 |
| 7.7.0             |    2,823 / 2,823 |                            1,214 |               362 |

The reports cover both directions between Wireloom C++ and RTI C++, RTI C#,
and Wireloom C#, plus Wireloom C++ loopback. Preserve their named peer
exclusions, adapted oracle paths, fixtures, and representations when citing a
case. These Linux results do not establish Windows application qualification
or direct local-CDR byte comparisons for every generated shape. The existing
C114 and FlatData limits remain in effect.

All downloaded ZIPs matched GitHub's artifact digests. The local CLI token
lacks `read:packages`, so a direct GitHub Packages metadata/read check was
unavailable. Successful publisher logs and verified retained artifacts
establish publication and the documented artifact-acquisition path.

## Corrected blockers

### PowerShell CMake arguments

PowerShell split the unquoted `-DWIRELOOM_CPP_RTI_VERSION=7.7.0` argument into
`-DWIRELOOM_CPP_RTI_VERSION=7` and `.7.0`. The documented native build then
failed with an invalid RTI version. The package README and Windows walkthrough
now quote the complete argument. The corrected command configures and builds
the application successfully.

### Legacy native SDK guard

The RTI 6.1.2 sample-access adapter previously compiled after overriding the
SDK revision macro to an unqualified value. Only the legacy FlatData path had
the existing exact-version guard. The common legacy preamble now applies that
guard before emitting its internal-interface adapter.

Three new regression cases failed before the fix and pass afterward. Native
compiler probes for 6.1.2, 7.3.1, and 7.7.0 each compiled the matching SDK and
rejected major, minor, release, and revision mismatches. Retained exports were
regenerated through all three profile runs; each C# corpus run passed 176 tests.
The retained [native ABI verifier](../../../tests/Wireloom.Dds.Cpp.Generator.PackageIntegration/Verify-NativeAbi.ps1)
and its [usage instructions](../../../tests/Wireloom.Dds.Cpp.Generator.PackageIntegration/README.md#native-sdk-guard-checks)
reproduce the native compiler checks.

The corrected local candidate has SHA-256
`ec7f42a8d4a415db30e604822c1ac01a000fea07b89e33eabc4d99f044bf3ef8`.
Both hosts passed installation, unchanged and changed builds, missing-output
recovery, and `1.0.1-ci.32 -> 1.1.0 -> 1.0.1-ci.32` upgrade/rollback in the
same consumer. Windows also passed 55 MSBuild regression scenarios. Native
Windows, native Linux, and managed tools emitted identical guarded 6.1.2 output.

The combined C++ suite passed 407 tests. Coverage was 3,564/3,959 branches
(90.02%) and 8,017/8,206 lines (97.70%). The CLI verifier passed 94 cases,
372 file comparisons, and seven diagnostic checks. Fresh Community Qodana
completed successfully with zero findings.

## Native walkthrough qualification

The documented C++ application builds against RTI 7.7.0.0 on Windows 11 Pro
x64 (build 10.0.26300) with MSVC 19.44/v143 and CMake 4.3.1-msvc1, and on
Ubuntu 24.04.4 x64 with GCC 13.3/CMake 3.28.3. The corrected candidate passed
clean builds, unchanged builds, all ten payloads in order, both completion
lines, publisher/subscriber missing-peer timeouts, invalid arguments, and
graceful interruption on both platforms. Windows used real console Ctrl+C
events; Linux used SIGINT in a network-isolated container. The wire reports
separately establish the named UDPv4 exchanges.

Windows application configurations for RTI 6.1.2 and 7.3.1 have not been
qualified by this audit. Keep them outside claimed native application scope
until their exact SDK/toolchain checks exist.

## Outstanding release gates

- Merge the ABI and package README corrections, then require successful
  Quality/Coveralls, wire, native package, and Playground checks for the final
  selected source. Preparation-source results above remain identified separately.
- The owner must verify NuGet.org trusted publishing for the C++ package ID.
- Retain final-source wire ZIPs and readable reports under the version's
  permanent evidence archive. Preview/native artifacts expire after 14 days;
  retain the final publishing packages, hashes, SBOM, and attestations too.
