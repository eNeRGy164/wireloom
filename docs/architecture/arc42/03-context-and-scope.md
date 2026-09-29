# 3. Context and scope

Wireloom is a build-time component between a .NET consumer project and the
generated C# contract consumed by the application. It does not sit in the DDS
data path.

## 3.1 Business context

The source for the context diagram is
[system-context.puml](images/03/system-context.puml).

| Neighbor                                  | Direction                 | Exchanged value / data                                                        |
| :---------------------------------------- | :------------------------ | :---------------------------------------------------------------------------- |
| .NET DDS consumer                         | to Wireloom               | `DdsIdl` roots, batch-wide include directories/defines, and runtime reference |
| Wireloom package                          | to consumer build         | Generated C# data types and RTI type-specific support                         |
| RTI Connext DDS runtime                   | to generated application  | Runtime APIs, serialization, and DDS communication                            |
| RTI Connext DDS 7.7.0 / `rtiddsgen` 4.7.0 | to compatibility evidence | Named oracle output for selected cases; target floor is RTI 7.3.1+            |
| CI and package feeds                      | to/from repository        | Restore, tests, package artifacts, SBOM, and attestations                     |

## 3.2 Technical context

| Interface              | Neighbor                | Protocol / format                         | Where documented                                                                                             |
| :--------------------- | :---------------------- | :---------------------------------------- | :----------------------------------------------------------------------------------------------------------- |
| `DdsIdl` MSBuild items | Consumer project        | MSBuild item metadata                     | [Repository README](../../../README.md)                                                                      |
| `AdditionalFiles`      | Roslyn                  | Compiler input and analyzer configuration | [`Wireloom.Dds.Generator.targets`](../../../src/Wireloom.Dds.Generator/build/Wireloom.Dds.Generator.targets) |
| IDL include graph      | Wireloom compiler       | Connext IDL text and `#include`           | [Corpus guide](../../corpus/README.md)                                                                       |
| Generated documents    | Consumer compilation    | C# source                                 | [Building block view](05-building-block-view.md)                                                             |
| Oracle fixtures        | Corpus compliance tests | IDL, JSON manifest, retained C#           | [Feature coverage](../../corpus/FEATURE-COVERAGE.md)                                                         |
