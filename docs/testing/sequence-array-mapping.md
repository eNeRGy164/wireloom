# Sequence-array mapping

`DdsIdlSequenceArrayMapping=Idl` preserves the array and per-element sequence
layers for direct sequence arrays. The default `RtiCSharp` mapping remains
unchanged. Recursive direct sequence-array elements are rejected with
`DDSG0001`.

## Focused verification

A local live DDS run on October 20, 2026 passed 16/16 exchanges per RTI
version between Wireloom-generated C# and RTI C++ on RTI 7.3.1 and 7.7.0.
RTI C# is excluded because its direct-form binding flattens this IDL shape.
The result covers selected fixtures, not full-profile or all-language
qualification.

See the [consumer settings](../../src/Wireloom.Dds.Generator/README.md#choose-the-array-of-sequences-mapping)
and the [wire compatibility runbook](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md).
