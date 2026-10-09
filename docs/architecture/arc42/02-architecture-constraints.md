# 2. Architecture constraints

## 2.1 Organizational constraints

| Constraint                                             | Rationale                                                                           |
| :----------------------------------------------------- | :---------------------------------------------------------------------------------- |
| Feature claims require retained evidence               | The project distinguishes implemented source generation from verified compatibility |
| RTI-generated reference material is license-controlled | Oracle sources are retained only under the applicable repository review rules       |

## 2.2 Technical constraints

| Constraint                                                                   | Rationale                                            |
| :--------------------------------------------------------------------------- | :--------------------------------------------------- |
| The generator targets `netstandard2.0`                                       | Roslyn analyzer compatibility boundary               |
| Repository projects default to .NET 10; SDK `10.0.400`                      | Repository and Qodana-compatible SDK policy          |
| A resolved `Rti.ConnextDds` reference of at least 7.3.1 is required          | Generated support targets the RTI runtime surface    |
| Retained RTI oracle sources use Connext DDS 7.7.0 / `rtiddsgen` 4.7.0        | Compatibility evidence capture baseline              |
| C# 12 or later is required by the generator host                             | Current emitted-source baseline                      |
| Generation runs through Roslyn `AdditionalFiles` and MSBuild `DdsIdl` items  | Keeps the package inside the normal .NET build graph |
| Root metadata is combined for one project-wide generation batch              | Current compiler behavior for defines and includes   |
| Package versions, lock files, NuGet audit, and CI action pins are controlled | Reproducible and reviewable supply chain             |

Consumer support and repository defaults have separate scopes:

| Constraint | Type | Rationale | Design impact | Source |
| :-- | :-- | :-- | :-- | :-- |
| Consumer applications support .NET 8 and higher, with C# 12 or later | Technical | Keep generation available to applications on supported .NET baselines | Generated APIs must be available on .NET 8; consumers do not inherit the repository target | [Consumer requirements](../../../README.md#support-and-compatibility) |
| Repository projects default to `net10.0` with the pinned SDK | Convention | Maintain one SDK baseline for development, tests, and Qodana | Consumer compatibility checks may override the target without changing repository defaults | [Build defaults](../../../Directory.Build.props), [SDK policy](../../../global.json) |

## 2.3 Conventions

| Convention                                                       | Scope                                 |
| :--------------------------------------------------------------- | :------------------------------------ |
| Markdown **arc42** chapters and **PlantUML** source              | Architecture documentation            |
| Central package management                                       | .NET dependencies                     |
| **xUnit v3** on **Microsoft Testing Platform** with **Shouldly** | Automated tests                       |
| Stable corpus case IDs and explicit evidence states              | Compatibility documentation and tests |
