# Optional aggregate union alias

This consumer compares the IDL typedef `ChoiceAlias` with the canonical
`Choice` type in the exact corpus input `09-optional-aggregate-member.idl`.
It checks that the alias compiles as a Modern C++ topic type, that its RTI
AliasType points to the union, how registration names the C++ typedef, and
that default, aggregate payload, and number states serialize identically in
XCDR1/XCDR2. Live DDS is tested separately with two RTI controls and mixed
RTI/Wireloom peers.

The fixture builds and runs in the pinned RTI Linux container described by its
evidence report. It does not regenerate the corpus output.
