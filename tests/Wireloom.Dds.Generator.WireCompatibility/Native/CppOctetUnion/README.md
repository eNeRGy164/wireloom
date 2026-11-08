# Octet discriminator union

This fixture qualifies the exact corpus `CorpusUnionOctet::Choice` declaration
against retained RTI 7.7.0 generated C++. It checks the octet discriminator,
labels 0 and 255, default branch selection, branch accessors, DynamicType,
registration, local XCDR1/XCDR2 bytes, and live DDS pairing outcomes.

Use the pinned Linux RTI container described in the evidence report. No
product behavior is modified by this fixture.
