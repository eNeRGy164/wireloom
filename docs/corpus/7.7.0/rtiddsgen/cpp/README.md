# RTI C++11 oracle output

This directory contains RTI Connext DDS 7.7.0 C++11 output from `rtiddsgen`
4.7.0. The `manifest.json` records each shared corpus case, canonical IDL path,
exact generation options, adaptations, status, and generated filenames. Case
folders contain the generated output and diagnostic log; IDL remains in the
shared corpus.

The generator may require case-specific options such as
`-unboundedSupport`; these are recorded in the manifest. `10-flat-data-binding`
uses a final fixed-size struct because RTI requires final or mutable
extensibility for FlatData. The RTI 7.7.0 C++ capture retains the FlatData
binding; previous PLAIN captures are not FlatData evidence. C++ generation
results do not establish that the generated code compiles or that the feature
is wire-compatible.

RTI-generated sources are third-party reference material and may be subject to
RTI license terms. Retain file-level notices and do not redistribute outside
the applicable license.

[Corpus guide](../../../README.md) · [LTS reproduction](../../../LTS-PROFILES.md)
