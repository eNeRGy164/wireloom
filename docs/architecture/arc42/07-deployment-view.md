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

The manually triggered wire-compatibility job starts with the exact
`rticom/connext-base:7.7.0` toolchain and selects the matching C# runtime
package through `WireCompatibilityRtiVersion`. The RTI license secret is
written to a runtime-only file inside the job and is not included in artifacts.
The runner builds one generated C# peer and one RTI-generated C++ peer per
case, then launches separate producer/consumer processes for every fixture and
language pairing. The scenario total is calculated from the fixture catalog.
The job uploads only the structured scenario report and sanitized endpoint
logs. Adding another RTI release requires an exact toolchain image and a
compatible Wireloom emission target; 7.3.1 currently needs that
target-compatibility work.

Preview publication uses GitHub Packages. Validated release tags publish to
NuGet.org with SBOM and provenance attestations.
