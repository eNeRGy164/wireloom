# Release qualification records

This directory records what was verified for each published Wireloom package.
Start with the record for the exact package version you use. Each record links
the package and immutable GitHub release, identifies the source tag, and reports
the quality and release workflow results available for that source revision.

## Published releases

| Package                                                                                                                                          | Source tag                                                                                                         | Published  | Record                          |
| :----------------------------------------------------------------------------------------------------------------------------------------------- | :----------------------------------------------------------------------------------------------------------------- | :--------- | :------------------------------ |
| [1.1.0 C#](https://www.nuget.org/packages/Wireloom.Dds.Generator/1.1.0) · [C++](https://www.nuget.org/packages/Wireloom.Dds.Cpp.Generator/1.1.0) | [`v1.1.0`](https://github.com/eNeRGy164/wireloom/releases/tag/v1.1.0) · `2727d85321259ddf53fbe8c11f8b4ed54a128cc0` | 2026-11-14 | [1.1.0 qualification](1.1.0.md) |
| [1.0.0](https://www.nuget.org/packages/Wireloom.Dds.Generator/1.0.0)                                                                             | [`v1.0.0`](https://github.com/eNeRGy164/wireloom/releases/tag/v1.0.0) · `7ab7834a66358915993e09cba308e1c66183f70d` | 2026-11-03 | [1.0.0 qualification](1.0.0.md) |
| [0.5.0](https://www.nuget.org/packages/Wireloom.Dds.Generator/0.5.0)                                                                             | [`v0.5.0`](https://github.com/eNeRGy164/wireloom/releases/tag/v0.5.0) · `01f66fe07a9065751430c3b71bbc72a2a7a4ad0e` | 2026-10-26 | [0.5.0 qualification](0.5.0.md) |
| [0.4.0](https://www.nuget.org/packages/Wireloom.Dds.Generator/0.4.0)                                                                             | [`v0.4.0`](https://github.com/eNeRGy164/wireloom/releases/tag/v0.4.0) · `2f77e824ec6ad0a46ee4640b045e1835d023983f` | 2026-10-17 | [0.4.0 qualification](0.4.0.md) |
| [0.3.0](https://www.nuget.org/packages/Wireloom.Dds.Generator/0.3.0)                                                                             | [`v0.3.0`](https://github.com/eNeRGy164/wireloom/releases/tag/v0.3.0) · `71bdc406cd565b4bd9d441df470cafcd1c10b326` | 2026-10-05 | [0.3.0 qualification](0.3.0.md) |
| [0.2.0](https://www.nuget.org/packages/Wireloom.Dds.Generator/0.2.0)                                                                             | [`v0.2.0`](https://github.com/eNeRGy164/wireloom/releases/tag/v0.2.0) · `785e580d1cfff642fc426b43a22de3814f05a501` | 2026-09-27 | [0.2.0 qualification](0.2.0.md) |
| [0.1.0](https://www.nuget.org/packages/Wireloom.Dds.Generator/0.1.0)                                                                             | [`v0.1.0`](https://github.com/eNeRGy164/wireloom/releases/tag/v0.1.0) · `f69fe7c47d18d2760a0a5cd2e3f5876da8ec8783` | 2026-09-25 | [0.1.0 qualification](0.1.0.md) |

## Reading the records

Each record identifies the source tag and links to test evidence for that source
revision. Workflow links point to GitHub Actions; artifact downloads may expire
according to the workflow retention policy. The
[1.1.0 wire evidence](../testing/wire-compatibility/evidence/1.1.0/README.md),
[1.0.0 wire evidence](../testing/wire-compatibility/evidence/1.0.0/README.md)
and [0.5.0 wire evidence](../testing/wire-compatibility/evidence/0.5.0/README.md)
are retained with their original archive hashes. Older records
report only the evidence known for their tags; absent details remain unknown.

Published GitHub releases are immutable and have no attached release assets.
Package READMEs are fixed at publication; version 1.1.0 links its exact release
record for later qualification clarifications. This index and the repository
README provide navigation across released versions.

For the release process, see [Preparing a release](PREPARING-A-RELEASE.md).
