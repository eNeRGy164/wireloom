# Optional aggregate Holder

This fixture compiles the same consumer against retained RTI Connext 7.7.0
`rtiddsgen` output and Wireloom output for the corpus IDL
`docs/corpus/idl/features/09-optional-aggregate-member.idl`. It checks all four
optional members, including the aggregate `Choice` union and its typedef,
DynamicType, registration/DataWriter, six local CDR states in XCDR1/XCDR2, and
six live DDS peer pairings. No corpus inputs are synthesized or regenerated.

Run it in the pinned Linux RTI container described by the evidence report. Raw
runtime logs are retained outside documentation under `.tmp`.
