# 6. Runtime view

Wireloom has no long-running runtime process. Its primary runtime scenario is
the consumer's build, followed later by the generated application's use of the
RTI runtime.

## 6.1 Consumer build and generation

1. MSBuild adds explicit `DdsIdl` roots and tracks other `.idl` files as
   non-generating inputs.
2. Roslyn supplies the files, item metadata, compiler options, and compilation
   references to `Generator`.
3. The generator requires C# 12 or later and a resolved RTI runtime reference of
   at least 7.3.1.
4. `IdlCompiler` resolves the include graph, preprocesses input, parses
   declarations, resolves symbols and types, and validates semantics.
5. Emitters add generated C# documents. An invalid or unsupported input instead
   produces `DDSG0001` at the original IDL location.
6. The consumer compiles generated types together with its hand-written code.

## 6.2 Failure paths

- Missing or cyclic includes stop compilation with an IDL diagnostic.
- Unsupported syntax is rejected rather than silently dropped.
- An older language version produces `DDSG0002`.
- A missing compatible RTI runtime reference produces `DDSG0003`.
- The known cyclic-alias corpus probe is currently isolated because the resolver
  overflows in-process instead of returning a normal `IdlException`.
