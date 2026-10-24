# 7. Deployment view

Wireloom is deployed as a NuGet analyzer/source-generator package rather than
as a service. The generated application still deploys with the consumer's
selected RTI Connext DDS runtime.

Effective finite limits for unbounded IDL values are build-time MSBuild
properties with per-file item metadata overrides. They are compiled into the
generated type metadata and do not add consumer runtime configuration.

## 7.1 Infrastructure level 1

![Deployment view showing the developer or CI build host, NuGet feeds, generated application, RTI runtime, and compatibility evidence.](images/07/deployment-overview.png)

| Node                       | Deployed material                                                 | Role                                                                    |
| :------------------------- | :---------------------------------------------------------------- | :---------------------------------------------------------------------- |
| Developer or CI build host | .NET 10 SDK, consumer project, IDL, Wireloom package              | Runs restore, compilation, generation, and tests                        |
| NuGet feed                 | `Wireloom.Dds.Generator` package and dependencies                 | Supplies the analyzer and MSBuild targets                               |
| Consumer output            | Application assembly plus generated source compiled into it       | Runs the DDS application                                                |
| RTI runtime environment    | Application-selected `Rti.ConnextDds` libraries                   | Owns DDS communication and serialization                                |
| Evidence repository        | Corpus IDL, retained oracle sources, and wire-compatibility peers | Supports source-shape and DDS wire verification, not production runtime |

The Quality workflow continuously checks both exact RTI LTS packages on
Windows 2025 and Ubuntu 24.04. Its eight runtime/package jobs combine RTI
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

The manually triggered wire-compatibility workflow runs both qualified LTS
profiles as a version matrix. Each version has eight isolated case-shard jobs
(up to eight probes per version) and its own report job, workflow summary, and
combined evidence artifact. Each profile uses a trimmed .NET SDK image with
the matching RTI native runtime/development libraries; C++ peer source is
checked into the corpus, so wire execution does not install Java or invoke
`rtiddsgen`. RTI license access is limited to peer-execution jobs; report
assembly runs without that secret. Docker build layers use a version-scoped
GitHub Actions cache. C++ compiler outputs use a persistent cache keyed by the
runner OS, RTI and `g++` versions, shard, and hash of the container, native
runner, scripts, and corpus oracle inputs. Restore prefixes allow reuse across
compatible input changes and older cache keys. GitHub's branch cache scope
still applies. The
[wire compatibility runbook](../../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#case-shards-and-combined-reports)
owns shard execution, image/cache handling, artifact contents, and report
assembly details.
Adding another LTS release requires a profile-specific corpus export and an
exact toolchain image, package, and generator version. Profile selection uses
the assembly resolved by the consumer build.

Preview publication uses GitHub Packages. Validated release tags publish to
NuGet.org with SBOM and provenance attestations.
