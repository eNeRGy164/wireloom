# 7. Deployment view

Wireloom is deployed as a NuGet analyzer/source-generator package rather than
as a service. The generated application still deploys with the consumer's
selected RTI Connext DDS runtime.

## 7.1 Infrastructure level 1

| Node                       | Deployed material                                           | Role                                                        |
| :------------------------- | :---------------------------------------------------------- | :---------------------------------------------------------- |
| Developer or CI build host | .NET 10 SDK, consumer project, IDL, Wireloom package        | Runs restore, compilation, generation, and tests            |
| NuGet feed                 | `Wireloom.Dds.Generator` package and dependencies           | Supplies the analyzer and MSBuild targets                   |
| Consumer output            | Application assembly plus generated source compiled into it | Runs the DDS application                                    |
| RTI runtime environment    | Application-selected `Rti.ConnextDds` libraries             | Owns DDS communication and serialization                    |
| Evidence repository        | Corpus IDL and retained oracle sources                      | Supports compatibility verification, not production runtime |

Preview publication uses GitHub Packages. Validated release tags publish to
NuGet.org with SBOM and provenance attestations.
