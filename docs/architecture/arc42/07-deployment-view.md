# 7. Deployment view

Wireloom is deployed as a NuGet analyzer/source-generator package rather than
as a service. The generated application still deploys with the consumer's
selected RTI Connext DDS runtime.

## 7.1 Infrastructure level 1

The source for the deployment diagram is
[deployment-overview.puml](images/07/deployment-overview.puml).

| Node                       | Deployed material                                           | Role                                                        |
| :------------------------- | :---------------------------------------------------------- | :---------------------------------------------------------- |
| Developer or CI build host | .NET 10 SDK, consumer project, IDL, Wireloom package        | Runs restore, compilation, generation, and tests            |
| NuGet feed                 | `Wireloom.Dds.Generator` package and dependencies           | Supplies the analyzer and MSBuild targets                   |
| Consumer output            | Application assembly plus generated source compiled into it | Runs the DDS application                                    |
| RTI runtime environment    | Application-selected `Rti.ConnextDds` libraries             | Owns DDS communication and serialization                    |
| Evidence repository        | Corpus IDL, retained oracle sources, and wire-compatibility peers | Supports source-shape and DDS wire verification, not production runtime |

The manually triggered wire-compatibility workflow starts four isolated shard
jobs concurrently. Each loads its test image through a cached Docker build,
reusing package and .NET SDK installation layers. Only shard zero exports the
shared build cache. This avoids serial image export and artifact transfer
before testing. Each job uses the exact
`rticom/connext-base:7.7.0` toolchain and selects the matching C# runtime
package through `WireCompatibilityRtiVersion`. The RTI license secret is
written to a runtime-only file inside the job and is not included in artifacts.
The runner builds a Wireloom-generated C# peer, an RTI-generated C# peer from
the same IDL using the pinned code generator, and an RTI-generated C++ peer per case. It then
launches separate producer/consumer processes for every fixture and all nine
directed sender/receiver pairings. The report distinguishes the implementation
and language of each endpoint; the scenario total is calculated from the
fixture catalog.
Each shard uploads its structured report, sanitized endpoint logs, scenario
rows, and deterministic partition plan. C++ compiler caches are scoped by
shard; one shard saves the common NuGet cache to avoid concurrent writes to
the same key. A separate .NET-only report job downloads same-run artifacts
using the SHA-pinned, GitHub-owned `actions/download-artifact` action, validates
complete coverage, and uploads the combined evidence under the original
artifact name. This job has no RTI license secret. GitHub Actions dependency
updates remain covered by the existing weekly Dependabot configuration.
Adding another RTI release requires an exact toolchain image and a
compatible Wireloom emission target; 7.3.1 currently needs that
target-compatibility work.

Preview publication uses GitHub Packages. Validated release tags publish to
NuGet.org with SBOM and provenance attestations.
