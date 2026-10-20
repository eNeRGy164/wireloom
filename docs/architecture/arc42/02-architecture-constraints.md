# 2. Architecture constraints

## 2.1 Organizational constraints

| Constraint                                             | Rationale                                                                           |
| :----------------------------------------------------- | :---------------------------------------------------------------------------------- |
| Feature claims require retained evidence               | The project distinguishes implemented source generation from verified compatibility |
| RTI-generated reference material is license-controlled | Oracle sources are retained only under the applicable repository review rules       |

## 2.2 Technical constraints

| Constraint                                                                   | Rationale                                                                       |
| :--------------------------------------------------------------------------- | :------------------------------------------------------------------------------ |
| The generator targets `netstandard2.0`                                       | Roslyn analyzer compatibility boundary                                          |
| Repository projects default to .NET 10; SDK `10.0.400`                       | Repository and Qodana-compatible SDK policy                                     |
| A resolved `Rti.ConnextDds` reference of at least 7.3.1 is required          | Lower versions report `DDSG0003`; assembly version selects an immutable profile |
| 7.3.1 and 7.7.0 are the supported LTS profiles                               | Only the exact LTS versions are qualification targets                           |
| Retained RTI oracle sources are organized by exact LTS version               | Each profile uses matching `Rti.ConnextDds` and `rtiddsgen` evidence            |
| C# 12 or later is required by the generator host                             | Current emitted-source baseline                                                 |
| Analyzer references Roslyn 4.8.0                                             | Keeps the package loadable by the first .NET 8 C# 12 SDK host                   |
| Generation runs through Roslyn `AdditionalFiles` and MSBuild `DdsIdl` items  | Keeps the package inside the normal .NET build graph                            |
| Preprocessing and strictness metadata is combined for one project-wide batch | Mapping overrides are file-owned; defines and includes remain batch-wide        |
| Package versions, lock files, NuGet audit, and CI action pins are controlled | Reproducible and reviewable supply chain                                        |

Consumer support and repository defaults have separate scopes:

| Constraint                                                           | Type       | Rationale                                                             | Design impact                                                                              | Source                                                                               |
| :------------------------------------------------------------------- | :--------- | :-------------------------------------------------------------------- | :----------------------------------------------------------------------------------------- | :----------------------------------------------------------------------------------- |
| Consumer applications support .NET 8 and higher, with C# 12 or later | Technical  | Keep generation available to applications on supported .NET baselines | Generated APIs must be available on .NET 8; consumers do not inherit the repository target | [Consumer requirements](../../../README.md#support-and-compatibility)                |
| Repository projects default to `net10.0` with the pinned SDK         | Convention | Maintain one SDK baseline for development, tests, and Qodana          | Consumer compatibility checks may override the target without changing repository defaults | [Build defaults](../../../Directory.Build.props), [SDK policy](../../../global.json) |

RTI release qualification is also separate from the package guard. The exact
qualified-version list, upgrade procedure, and wire-matrix target are in the
[support and compatibility policy](../../guides/support-and-compatibility.md).

## 2.3 Conventions

| Convention                                                       | Scope                                 |
| :--------------------------------------------------------------- | :------------------------------------ |
| Markdown **arc42** chapters and **PlantUML** source              | Architecture documentation            |
| Central package management                                       | .NET dependencies                     |
| **xUnit v3** on **Microsoft Testing Platform** with **Shouldly** | Automated tests                       |
| Stable corpus case IDs and explicit evidence states              | Compatibility documentation and tests |
