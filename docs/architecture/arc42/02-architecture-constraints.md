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
| Consumer and test projects use .NET 10; SDK `10.0.401` is pinned             | Repository build baseline                            |
| A resolved `Rti.ConnextDds` reference of at least 7.3.1 is required          | Generated support targets the RTI runtime surface    |
| C# 12 or later is required by the generator host                             | Current emitted-source baseline                      |
| Generation runs through Roslyn `AdditionalFiles` and MSBuild `DdsIdl` items  | Keeps the package inside the normal .NET build graph |
| Package versions, lock files, NuGet audit, and CI action pins are controlled | Reproducible and reviewable supply chain             |

## 2.3 Conventions

| Convention                                                       | Scope                                 |
| :--------------------------------------------------------------- | :------------------------------------ |
| Markdown **arc42** chapters and **PlantUML** source              | Architecture documentation            |
| Central package management                                       | .NET dependencies                     |
| **xUnit v3** on **Microsoft Testing Platform** with **Shouldly** | Automated tests                       |
| Stable corpus case IDs and explicit evidence states              | Compatibility documentation and tests |
