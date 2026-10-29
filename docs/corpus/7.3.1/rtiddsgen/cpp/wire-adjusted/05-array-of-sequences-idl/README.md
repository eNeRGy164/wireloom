# RTI C++ wire oracle reused for C149

This directory reuses the C018 generated source because C018 and C149 IDL bytes are identical.
Source filenames are normalized to `idl.*`, and generated header includes are updated to match. No
additional `rtiddsgen` invocation is required. See [manifest.json](manifest.json) for source and staged hashes.

[Corpus guide](../../../../../README.md) · [LTS reproduction](../../../../../LTS-PROFILES.md)
