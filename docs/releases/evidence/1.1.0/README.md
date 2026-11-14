# Wireloom 1.1.0 retained publication evidence

The [successful release retry](https://github.com/eNeRGy164/wireloom/actions/runs/43648866763/attempts/2)
published both packages from `2727d85321259ddf53fbe8c11f8b4ed54a128cc0`. The original publication ZIP
retains the two tested packages, their [SHA256SUMS](SHA256SUMS), and the
[generated SBOM](wireloom-dds-generator.sbom.cdx.json). The native consumer ZIPs
retain both host reports and build logs. Quality archives are from the exact-source
[Quality run](https://github.com/eNeRGy164/wireloom/actions/runs/43648337273).

| Original artifact                                               | GitHub artifact                                                                                     |      Bytes | SHA-256                                                            |
| --------------------------------------------------------------- | --------------------------------------------------------------------------------------------------- | ---------: | ------------------------------------------------------------------ |
| [wireloom-dds-generator-release-1.1.0](release-publication.zip) | [14135482726](https://github.com/eNeRGy164/wireloom/actions/runs/43648866763/artifacts/14135482726) |  5,011,215 | `70e911da0d046242fe8f4210f709450bcbc667bbf0c66e0aa9ffaf4cb3b5cc52` |
| [cpp-consumer-ubuntu-24.04](cpp-consumer-ubuntu-24.04.zip)      | [14135398790](https://github.com/eNeRGy164/wireloom/actions/runs/43648866763/artifacts/14135398790) | 13,797,221 | `65a852f879ca05ef483f5cf2fb67782af69feaea90c3658c121969c6af640e3a` |
| [cpp-consumer-windows-2025](cpp-consumer-windows-2025.zip)      | [14135299423](https://github.com/eNeRGy164/wireloom/actions/runs/43648866763/artifacts/14135299423) | 20,352,544 | `6606b10c41814653dbfb0bd5412a57fa8a91a5fcb62abc77527ec8e19d048f3e` |
| [qodana-community-report](qodana-community-report.zip)          | [14135845625](https://github.com/eNeRGy164/wireloom/actions/runs/43648337273/artifacts/14135845625) |  1,867,166 | `4ff500a71cb562141c89180e62b1120e3ce7b01a4668e66b8aced991d43e0872` |
| [playground-qualification](playground-qualification.zip)        | [14135720897](https://github.com/eNeRGy164/wireloom/actions/runs/43648337273/artifacts/14135720897) |    281,921 | `49e642eb24517af0fd3908c7898a25dc27f4a0719fb3ed4a6a7ffd3f09dd3320` |
| [combined-coverage](combined-coverage.zip)                      | [14135401059](https://github.com/eNeRGy164/wireloom/actions/runs/43648337273/artifacts/14135401059) |    367,484 | `dbfd6060646ca267194003e1d36d59ad742e8c6c2fb74b5a706e0c0351bf83e8` |

All ZIP digests were verified against GitHub metadata before retention on
November 15, 2026. [provenance.json](provenance.json) records their identities.
The [C#](csharp-attestation.json) and [C++](cpp-attestation.json) verification
records retain signed provenance; [provenance-summary.json](provenance-summary.json)
identifies the tag and signing workflow.

The retained [public deployment manifest](public-deployment.json) identifies
version 1.1.0 and the tagged source. Its SHA-256 is
`1007d57a2cc5de92302935c480623097c20adf33f16fe0e66cdcf8631e738868`.

NuGet.org adds repository signatures after upload, changing package ZIP hashes.
[Payload comparison](nuget-payload-verification.json) verified every original
file against the downloaded packages; only the repository signature was added.
[Signature verification](nuget-signature-verification.txt) passed for both.
The [verification record](verification.json) distinguishes registry hashes from
the pre-signing hashes in `SHA256SUMS`.

The generated SBOM contains metadata but no package components. It is retained
unchanged and is not a dependency inventory. This is a
[potential follow-up](../../1.1.0-follow-ups.md), not a claim of complete SBOM coverage.

Wire archives are [retained separately](../../../testing/wire-compatibility/evidence/1.1.0/README.md).
The [release record](../../1.1.0.md) defines qualified scope and limitations.
