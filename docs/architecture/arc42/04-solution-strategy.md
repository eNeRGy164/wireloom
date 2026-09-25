# 4. Solution strategy

1. **Use a managed Roslyn incremental generator.**  
   The package participates in the normal compiler invocation and does not
   require a second generator toolchain.
2. **Separate input discovery, semantic meaning, and emission.**  
   Include resolution and preprocessing feed a target-independent semantic model;
   resolved emission plans then feed managed, native, and type-support emitters.
3. **Make roots and include behavior explicit.**  
   `DdsIdl` starts generation; included IDL is tracked for invalidation but
   does not become another root.
4. **Fail visibly at unsupported boundaries.**  
   Unsupported declarations and invalid input produce source-located diagnostics
   rather than partial output.
5. **Advance compatibility by case.**  
   A feature becomes a compatibility claim only after the relevant RTI oracle,
   source-shape, runtime, and interoperability evidence exists.
