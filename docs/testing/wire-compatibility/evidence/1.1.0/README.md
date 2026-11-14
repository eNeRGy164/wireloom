# Wireloom 1.1.0 retained wire evidence

Original combined artifacts from [wire run 43648337346](https://github.com/eNeRGy164/wireloom/actions/runs/43648337346)
qualify tagged source `2727d85321259ddf53fbe8c11f8b4ed54a128cc0`. They were retained on November 15, 2026,
before artifact expiration. Archive sizes and SHA-256 digests match GitHub metadata.

| RTI   | GitHub artifact                                                                                     | Archive                            |     Bytes | SHA-256                                                            |
| ----- | --------------------------------------------------------------------------------------------------- | ---------------------------------- | --------: | ------------------------------------------------------------------ |
| 6.1.2 | [14134938200](https://github.com/eNeRGy164/wireloom/actions/runs/43648337346/artifacts/14134938200) | [Original ZIP](wire-rti-6.1.2.zip) | 1,848,460 | `359c2556bede0e87e9edf543268307fd69953cc95bb2073304ed39dc4b9dbbb7` |
| 7.3.1 | [14135791219](https://github.com/eNeRGy164/wireloom/actions/runs/43648337346/artifacts/14135791219) | [Original ZIP](wire-rti-7.3.1.zip) | 2,055,801 | `f398307f9bd71aa7e72ba7c6c758d24510da3d203afb25b7e70a174b3e7c637f` |
| 7.7.0 | [14135691397](https://github.com/eNeRGy164/wireloom/actions/runs/43648337346/artifacts/14135691397) | [Original ZIP](wire-rti-7.7.0.zip) | 2,053,611 | `1fba8ab1cbc63fa5e2ba49664590243e059f40ee5d3cb740c271c01df224a782` |

The readable [6.1.2](rti-6.1.2-results.md), [7.3.1](rti-7.3.1-results.md), and
[7.7.0](rti-7.7.0-results.md) reports are byte-for-byte copies of each archive's
`results.md`. Archives also retain machine-readable results, exchange plans,
clean-source provenance, and sanitized logs. [provenance.json](provenance.json)
records archive/report hashes, counts, and artifact identities.

All 8,173 planned exchanges passed: 2,521 for 6.1.2, 2,829 for 7.3.1, and
2,823 for 7.7.0. Preserve the named peer exclusions and adaptations in each report.
These Linux exchanges do not establish Windows application qualification or
direct local-CDR byte parity for every generated shape. See the
[release record](../../../../releases/1.1.0.md#wire-evidence-and-limitations).
