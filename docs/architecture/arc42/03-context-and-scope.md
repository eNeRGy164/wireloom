# 3. Context and scope

Wireloom is the product boundary. It provides C# generation in the .NET build
and a supporting local browser Playground. Both belong inside Wireloom; users
interact with the product through these entry points. Internal components are
described in [chapter 5](05-building-block-view.md).

Consumer projects and applications, .NET build/IDE tooling, the RTI runtime,
package feeds, and hosting platforms remain outside this boundary. Wireloom
is independent of RTI. The consumer application uses the RTI runtime for
serialization and DDS communication after generation and compilation.

## 3.1 Business context

The diagram shows Wireloom's neighbors and their expectations of the product.
Each relationship expresses an expectation; it does not describe an execution
sequence or runtime call.

![Wireloom as one product, surrounded by four human roles and neighboring .NET and RTI platforms, with their expectations.](images/03/system-context.png)

| Neighbor                       | Responsibility outside Wireloom                                      | Expectation of Wireloom                                                                              |
| :----------------------------- | :------------------------------------------------------------------- | :--------------------------------------------------------------------------------------------------- |
| .NET DDS application developer | Maintain IDL and application code; validate adoption and upgrades    | Drop-in compatibility and clean, extensible generated C# without source-control or analysis noise    |
| Build and CI engineer          | Configure developer and automated build environments                 | Repeatable generation through normal restore and build with minimal generation-specific setup        |
| Wireloom maintainer            | Evolve the product and verify its behavior and compatibility         | Focused tests, clear diagnostics, and attributable compatibility evidence                            |
| Playground user                | Supply or edit IDL and choose preview settings                       | A reliable local preview of generated C# and diagnostics without installation                        |
| .NET build and IDE             | Host generation and present compilation results                      | Generation through normal build integration, with generated documents and source-located diagnostics |
| RTI Connext DDS runtime        | Provide the application's DDS APIs, serialization, and communication | Generated APIs and type-specific support compatible with the selected RTI runtime                    |

The human role names and their responsibilities come from
[chapter 1](01-introduction-and-goals.md#13-stakeholders). Their interactions
are traced in [chapter 6](06-runtime-view.md). The RTI relationship describes
the contract generated code must satisfy. DDS traffic stays between the
consumer application, the RTI runtime, and its peers.

## 3.2 Technical context

This view identifies information crossing the product boundary and who owns
each side. Interface details remain in the linked contracts and guides.

| External neighbor / boundary        | Direction and exchanged information                                                                                                           | Ownership                                                                                                       | Reference                                                                                                                                                                          |
| :---------------------------------- | :-------------------------------------------------------------------------------------------------------------------------------------------- | :-------------------------------------------------------------------------------------------------------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Consumer project                    | Project to Wireloom: IDL roots, include sources, generation options, and the resolved RTI reference                                           | The application team owns IDL and configuration; Wireloom owns its generation settings contract                 | [Configuration guide](../../guides/configuration.md)                                                                                                                               |
| .NET build and IDE                  | Host to Wireloom: Roslyn inputs, options, and references; Wireloom to host: generated C# documents and source-located diagnostics             | Microsoft owns host APIs; Wireloom owns its adapter and MSBuild integration                                     | [MSBuild targets](../../../src/Wireloom.Dds.Generator/build/Wireloom.Dds.Generator.targets), [build scenario](06-runtime-view.md#611-generate-or-regenerate-application-contracts) |
| [NuGet](12-glossary.md#nuget) feeds | Wireloom to feed: published analyzer package; feed to consumer build: restored package assets                                                 | Wireloom owns its package; feed operators own distribution; the application team selects its runtime dependency | [Package deployment](07-deployment-view.md#71-net-build-integration)                                                                                                               |
| Playground user and local files     | User to Wireloom: IDL, virtual paths, roles, and settings; Wireloom to user: generated C#, diagnostics, and file exports                      | The user owns source and local files; Wireloom owns the workspace and preview behavior                          | [Workspace guide](../../guides/playground-workspaces.md)                                                                                                                           |
| Static host and browser platform    | Wireloom release to host: static application assets; host to browser: assets over HTTPS; browser APIs provide local file and recovery storage | Wireloom owns the application assets; platform operators own serving and browser APIs                           | [Playground deployment](07-deployment-view.md#72-browser-playground-deployment), [deployment guide](../../guides/playground-deployment.md)                                         |

Generated C# becomes part of the consumer application. Application calls into
RTI happen outside Wireloom's execution boundary. Installing Wireloom does not
replace the application's runtime reference, licensing, or runtime setup; see
the [support boundary](../../guides/support-and-compatibility.md#support-boundary).

The shared compiler API and Playground client/worker protocol connect internal
blocks. Their structure and payloads are in
[chapter 5](05-building-block-view.md#531-shared-idl-compiler), with detailed
protocol rules in the [worker contract](../../guides/playground-workspaces.md#worker-contract-v3).

## 3.3 Development and qualification context

Wireloom maintainers use repository-owned tests and the wire harness to verify
the product. The following external facilities support that work. Retained
corpus and oracle evidence supports qualification; a consumer build does not
require that evidence or invoke the RTI reference generator.

| External neighbor                       | Information exchanged with Wireloom's qualification tooling                                                                               | Ownership                                                                                                                | Reference                                                                                                                                               |
| :-------------------------------------- | :---------------------------------------------------------------------------------------------------------------------------------------- | :----------------------------------------------------------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------ |
| RTI reference toolchains and test peers | IDL and fixture data into matching RTI tools/peers; versioned oracle output and directed DDS exchange results into qualification evidence | RTI owns vendor tools and runtime APIs; Wireloom maintainers own fixtures, peer adaptations, and evidence interpretation | [Exact LTS profiles](../../corpus/LTS-PROFILES.md), [wire runbook](../../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md)                   |
| CI and container platforms              | Wireloom build/test jobs execute on the platform; status, logs, and artifacts return to maintainers                                       | Wireloom maintainers own workflows and harness behavior; platform providers own the execution facilities                 | [Contribution guide](../../../CONTRIBUTING.md), [wire infrastructure](../../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md#github-actions) |

The [qualification policy](02-architecture-constraints.md#22-evidence-and-provenance-policies)
governs what these facilities can establish. Current feature status is recorded
in the [corpus index](../../corpus/FEATURE-COVERAGE.md), with remaining
qualification exposure in [R01](11-risks-and-technical-debt.md#r01).

---

[< 2. Architecture constraints](02-architecture-constraints.md) | [Index](index.md) | [4. Solution strategy >](04-solution-strategy.md)
