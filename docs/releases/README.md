# Release qualification records

This directory records what was verified for each published Wireloom package.
Start with the record for the exact package version you use. Each record links
the package and immutable GitHub release, identifies the source tag, and reports
the quality and release workflow results available for that source revision.

| Package                                                              | Source tag                                                                                                         | Published  | Record                          |
| :------------------------------------------------------------------- | :----------------------------------------------------------------------------------------------------------------- | :--------- | :------------------------------ |
| 0.5.0 (planned)                                                      | `v0.5.0` (source pending)                                                                                          | Pending    | [0.5.0 preparation](0.5.0.md)   |
| [0.4.0](https://www.nuget.org/packages/Wireloom.Dds.Generator/0.4.0) | [`v0.4.0`](https://github.com/eNeRGy164/wireloom/releases/tag/v0.4.0) · `2f77e824ec6ad0a46ee4640b045e1835d023983f` | 2026-10-17 | [0.4.0 qualification](0.4.0.md) |
| [0.3.0](https://www.nuget.org/packages/Wireloom.Dds.Generator/0.3.0) | [`v0.3.0`](https://github.com/eNeRGy164/wireloom/releases/tag/v0.3.0) · `71bdc406cd565b4bd9d441df470cafcd1c10b326` | 2026-10-05 | [0.3.0 qualification](0.3.0.md) |
| [0.2.0](https://www.nuget.org/packages/Wireloom.Dds.Generator/0.2.0) | [`v0.2.0`](https://github.com/eNeRGy164/wireloom/releases/tag/v0.2.0) · `785e580d1cfff642fc426b43a22de3814f05a501` | 2026-09-27 | [0.2.0 qualification](0.2.0.md) |
| [0.1.0](https://www.nuget.org/packages/Wireloom.Dds.Generator/0.1.0) | [`v0.1.0`](https://github.com/eNeRGy164/wireloom/releases/tag/v0.1.0) · `f69fe7c47d18d2760a0a5cd2e3f5876da8ec8783` | 2026-09-25 | [0.1.0 qualification](0.1.0.md) |

## Reading the records

Each record identifies the source tag and links to test evidence for that source
revision. Workflow links point to GitHub Actions; artifact downloads may expire
according to the workflow retention policy.

All four GitHub releases are immutable and have no attached release assets.
Existing NuGet package pages and release descriptions cannot be changed to add
links after publication. The current package README links to this stable index
for future releases; the link in the repository README provides the index for
all package versions.

For the release process, see [Preparing a release](PREPARING-A-RELEASE.md).
