# Optional aggregate PayloadAlias declaration

This fixture exercises `CorpusOptionalAggregate::PayloadAlias` as a standalone
Modern C++ topic root using the corpus IDL. RTI models the typedef as an
`AliasType` with a distinct DynamicType tag, while the C++ typedef resolves to
the canonical `Payload` storage type and topic name. The shared consumer checks
that API distinction, DynamicType, registration, default/positive/negative
XCDR1/XCDR2 captures, and live DDS against retained RTI 7.7.0 output.

Run in the pinned Linux RTI container described in the evidence report. No
generator or runtime behavior is changed by this fixture.
