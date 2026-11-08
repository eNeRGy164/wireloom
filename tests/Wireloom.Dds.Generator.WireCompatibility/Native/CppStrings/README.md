# C014 string wire fixture

This RTI Connext 7.7.0 fixture compiles the retained RTI and Wireloom C++11
outputs for the corpus `04-strings.idl`. It checks typed topic registration and
DataWriter creation, then compares local XCDR1 and XCDR2 CDR bytes for empty,
partial, and boundary values. The boundary sample reaches all four effective
string limits: 255 elements for each source-unbounded string and 8 for each
explicitly bounded string. Each peer round-trips its CDR before writing it.

These checks establish native local CDR compatibility only. The fixture does
not start two participants or claim live DDS sample exchange.
