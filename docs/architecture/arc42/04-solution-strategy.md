# 4. Solution strategy

1. **Use a managed Roslyn incremental generator.**  
   The package participates in the normal compiler invocation and does not
   require a second generator toolchain.
2. **Separate input discovery, semantic meaning, and emission.**  
   Include resolution and preprocessing feed a target-independent semantic model;
   the front end moves through explicit parse, bind, and validate phases before
   resolved emission plans feed managed, native, and type-support emitters.
3. **Make roots and include behavior explicit.**  
   `DdsIdl` starts generation; included IDL is tracked for invalidation but
   does not become another root.
4. **Fail visibly at unsupported boundaries.**  
   Unsupported declarations and invalid input produce source-located diagnostics
   rather than partial output.
5. **Advance compatibility by case.**  
   The retained oracle capture uses RTI 7.7.0 / `rtiddsgen` 4.7.0; the target
   compatibility floor is RTI 7.3.1+ because that release ships the same
   generator version. A feature becomes a compatibility claim only after the
   relevant source-shape, runtime, and interoperability evidence exists.
