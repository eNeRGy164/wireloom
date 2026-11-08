# Optional aggregate union branch

This consumer compiles the same C++11 assertions against retained RTI Connext
7.7.0 `rtiddsgen` and Wireloom output for the exact corpus IDL
`docs/corpus/idl/features/09-optional-aggregate-member.idl`. It checks the
standalone `Choice` topic API, union DynamicType/native TypeCode, registration
and DataWriter construction, and byte equality for default, aggregate payload,
and number branches in XCDR1 and XCDR2. The `@optional` annotation applies to
the payload branch; the fixture exercises that branch as an active union arm.

Reproduce inside the repository's pinned RTI Linux container using the paths,
toolchain, and read-only license mount documented in the associated evidence
report. Live DDS is assessed separately from local CDR captures.
