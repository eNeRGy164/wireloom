# Enum-discriminated union typedef chains

This fixture uses the exact corpus IDL at
`docs/corpus/idl/features/07-union-aliases.idl`. One C++11 consumer compiles
against retained RTI 7.7.0 output and Wireloom output in the pinned Linux
amd64 Docker image.

The consumer checks that `ChoiceAlias` and `ChoiceAlias2` remain C++ aliases of
the canonical `Choice` value class, while their AliasType tags preserve both
DynamicType layers. It checks the enum discriminator and labels, union branch
API/inactive access, Holder's final-alias member metadata, native TypeCode
representation, typed registration/DataWriter creation for `Choice` and
`Holder`, and RTI-compatible swap across union arms. Default and non-default
Choice/Holder values round-trip under XCDR1 and XCDR2, with CTest comparing the
complete RTI and Wireloom capture files byte for byte.

This qualifies the exact typedef-chain case for local API, metadata,
registration, and CDR only. It does not establish live DDS or qualify other
union alias or aggregate shapes. See the
[RTI 7.7.0 corpus profile](../../../../docs/corpus/7.7.0/FEATURE-COVERAGE.md)
report and the [Modern C++ generator guide](../../../../src/Wireloom.Dds.Cpp.Generator/README.md).
