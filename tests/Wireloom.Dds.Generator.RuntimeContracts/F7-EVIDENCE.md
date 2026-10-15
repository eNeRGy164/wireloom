# F7 resolution: string typedef chains

## Finding

Scalar aliases targeting another string typedef previously lost the resolved
string kind and bound. Generated `Value` properties were non-nullable but
uninitialized, bounded aliases fell back to the effective bound of 255, and
wide-string chains used narrow-string DynamicType and native storage.

The behavior was reproduced with one-hop unbounded aliases and bounded
`string<16>` / `wstring<8>` aliases. A two-hop probe showed the same loss at
greater alias depth. RTI Connext DDS 7.7.0.1 / `rtiddsgen` 4.7.0.1 accepts the
IDL and emits initialized properties, preserves `[Bound(16)]` and `[Bound(8)]`,
and uses `CreateWideString` / `NativeWstring` for wide aliases. The captured RTI
outputs were compiled against `Rti.ConnextDds` 7.7.0 with nullable analysis
disabled, matching the existing RTI output baseline. They were not tested
through DDS communication.

## Repair

For scalar aliases, emission planning now resolves the declared target before
erasing aliases. When that target resolves to a string, the plan carries the
underlying `StringEmissionType` through the managed, plugin, and native
emitters. This preserves narrow/wide kind, explicit and effective bounds, the
managed `string.Empty` initializer, and the correct native string storage.
Non-string scalar aliases keep their existing flattening behavior. Named
string aliases used as collection elements also retain their prior alias
identity and collection behavior.

The regular fixture at
[`Fixtures/string-alias-chains.idl`](Fixtures/string-alias-chains.idl) covers
unbounded and bounded narrow/wide aliases at one and two hops. Typed managed
tests verify initialization and copy independence, DynamicType tests verify
kind and bounds under both visibility modes, and native tests round-trip the
bounded chains with each allocation flag combination. Generator tests inspect
the generated managed, plugin, and native source.

## Validation

- Opt-in reproduction build, Release: passed with 0 warnings and 0 errors after the repair.
- Runtime contract suite: 503 passed, 0 failed, 0 skipped.
- Generator test suite: 1,245 passed, 0 failed, 0 skipped.
- RTI 7.7.0.1 C# generation: accepted original and bounded-chain IDL variants; generated C# source retains initialization, bounds, and wide-string representations.
- DDS wire interoperability for string alias chains was not tested as part of this repair.
