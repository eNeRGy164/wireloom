# Character discriminator union

This fixture uses the exact corpus IDL at
`docs/corpus/idl/features/07-union-char.idl`. CMake compiles the same
`peer.cpp` consumer against the retained RTI 7.7.0 generated files and the
Wireloom output with C++11 on Windows x64.

The consumer checks the plain-char discriminator API, ASCII labels `'a'` and
`'z'`, the default `boolean` branch, `UnionType` and native TypeCode metadata,
and typed registration with `Topic<Choice>` and `DataWriter<Choice>`. It also
exercises equality and swap while the `other` branch is active, covering the
generated storage qualification fixed for this branch name. Local captures
cover default, letter, text, and default-branch values under XCDR1 and XCDR2;
CTest compares each full RTI and Wireloom capture byte for byte.

This qualifies local metadata, registration, and CDR only for this exact IDL
shape. It does not test live DDS exchange or qualify other char-discriminator
forms. See the
[`07-union-char.windows-x64.json`](../../../../docs/corpus/7.7.0/cpp-evidence/07-union-char.windows-x64.json)
report and the [C++ coverage matrix](../../../../docs/corpus/CPP-COVERAGE.md).
