# Optional aggregate Payload declaration

This fixture tests `CorpusOptionalAggregate::Payload` as a standalone Modern
C++ topic root using the exact `09-optional-aggregate-member.idl` input. The
same consumer checks its public API, DynamicType, registration/DataWriter,
default/positive/negative CDR states in XCDR1 and XCDR2, and mixed live DDS
pairings against retained RTI 7.7.0 generated output.

Use the pinned Linux RTI container described in the evidence report. No
generator or runtime behavior is changed by this fixture.
