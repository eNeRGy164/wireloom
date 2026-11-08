# Boolean discriminator union

This fixture uses the exact corpus IDL at
`docs/corpus/idl/features/07-union-boolean.idl`. CMake compiles one C++11
consumer against retained RTI 7.7.0 output and Wireloom output on Windows x64
and in a pinned Linux amd64 Docker image.

The consumer checks the bool discriminator API, default false construction
selecting the explicit `disabled` arm, both branch APIs and inactive-access
exceptions, the TRUE/FALSE labels, DynamicType/native TypeCode metadata, and
typed registration with `Topic<Choice>` and `DataWriter<Choice>`. It checks
RTI-compatible member swap across the enabled and disabled arms. CTest compares
the complete local XCDR1 and XCDR2 capture bytes for default/empty-disabled,
enabled, and nonempty-disabled states.

This qualifies the exact boolean union shape for local metadata, registration,
and CDR only. It does not test live DDS exchange or qualify other union shapes.
See the Windows
[`07-union-boolean.windows-x64.json`](../../../../docs/corpus/7.7.0/cpp-evidence/07-union-boolean.windows-x64.json)
and Linux
[`07-union-boolean.linux-x64.json`](../../../../docs/corpus/7.7.0/cpp-evidence/07-union-boolean.linux-x64.json)
reports and the [C++ coverage matrix](../../../../docs/corpus/CPP-COVERAGE.md).
