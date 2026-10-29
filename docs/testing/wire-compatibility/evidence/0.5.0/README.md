# Wireloom 0.5.0 retained wire evidence

These are the original combined artifacts from
[wire run 40254314909](https://github.com/eNeRGy164/wireloom/actions/runs/40254314909),
for tag source `01f66fe07a9065751430c3b71bbc72a2a7a4ad0e`.
They were retrieved on October 29, 2026 before the documented November 9
Actions artifact expiry. Archive byte counts and SHA-256 digests match the
metadata returned by GitHub.

| RTI   | Original artifact ID | Archive                            |  Bytes | SHA-256                                                            |
| :---- | :------------------- | :--------------------------------- | -----: | :----------------------------------------------------------------- |
| 7.3.1 | 12622157547          | [Original ZIP](wire-rti-7.3.1.zip) | 931841 | `9b023b02564d7c9bf6d59fee2a7216338d9a76739e43f43c20f4719d2f79f91f` |
| 7.7.0 | 12622555653          | [Original ZIP](wire-rti-7.7.0.zip) | 925611 | `823247b1d4c86e148012880c2042f11c3a501b8dee556ba2d554c6421fdbe71f` |

## Read the results

- [RTI 7.3.1 report](rti-7.3.1-results.md): 1,560 exchanges, 1,537 passed,
  23 failed, none not run.
- [RTI 7.7.0 report](rti-7.7.0-results.md): 1,555 exchanges, 1,551 passed,
  four failed, none not run.

The readable reports are byte-for-byte copies of `results.md` inside their
archives. Each ZIP also retains `results.json`, `plan.json`, `scenarios.tsv`,
per-case generation provenance, and sanitized logs. These combined artifacts
do not contain individual shard metadata; the source workflow is the context
for their combination. See [provenance.json](provenance.json) for local file
digests and source/artifact identifiers.

The archived reports retain failed exchanges and source-pinned workaround
links. A complete report does not mean all exchanges passed. These results
qualify their exact fixtures, profiles, and source; later changes do not rewrite
the 0.5.0 outcome.

[Release record](../../../../releases/0.5.0.md) · [Current wire runbook](../../../../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md)
