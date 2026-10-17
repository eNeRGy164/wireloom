# Version-specific corpus data

The root `idl/` tree is the shared case inventory. This directory holds
version-scoped manifests, RTI C# oracle output, and Wireloom generated-source
exports for each qualified LTS target. RTI-generated sources remain third-party
reference material and are subject to the applicable RTI license and review.

The runtime package version selects the matching API profile. In code, profile
identities follow the `rtiddsgen` version represented by that API surface:

| Resolved `Rti.ConnextDds` version  | Profile identifier | Matching `rtiddsgen` |
| :--------------------------------- | :----------------- | :------------------- |
| 7.3.1 through versions below 7.7.0 | `Rtiddsgen431`     | 4.3.1                |
| 7.7.0 and later                    | `Rtiddsgen470`    | 4.7.0              |

- [RTI 7.3.1](7.3.1/FEATURE-COVERAGE.md): captured `rtiddsgen` output and
  Wireloom 7.3.1-profile exports; exact-package compilation and runtime
  contracts pass, while wire qualification is pending.
- [RTI 7.7.0](7.7.0/FEATURE-COVERAGE.md): retained oracle data and the current
  qualification evidence index.
