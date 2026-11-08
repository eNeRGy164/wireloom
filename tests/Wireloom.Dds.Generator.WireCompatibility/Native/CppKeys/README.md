# `08-keys` RTI/Wireloom consumer

This isolated C++11 fixture compiles the same consumer against the retained
RTI 7.7.0 Modern C++ output and generated Wireloom output for the exact
`docs/corpus/idl/features/08-keys.idl` corpus source.

The API checks cover all three value types, `topic_type_name`, `is_topic_type`,
DynamicType member order/types, native key and required-member flags, typed type
registration, and Topic/DataWriter creation. Instance registration proves that
`Identity` and nested `Sample.identity` distinguish changes to either key
component (`tenant` and `name`) while ignoring non-key payload changes. CDR checks round-trip default and
populated values for `Simple`, `Identity`, and `Sample` under XCDR1 and XCDR2,
then compare the complete capture files byte for byte.

Configure/build/run inside the repository-pinned `rticom/connext-base:7.7.0`
Docker image using the host RTI license mounted read-only at
`/run/secrets/rti_license.dat`. RTI and Wireloom peers use the same source and
are built with C++11. The fixture does not claim live DDS exchange or qualify
other key shapes such as arrays, inheritance, or unions.
