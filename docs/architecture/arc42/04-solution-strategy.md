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
5. **Advance compatibility by case and explicit target.**
   The retained oracle capture and initial wire baseline use RTI 7.7.0 /
   `rtiddsgen` 4.7.0. The accepted runtime package floor (7.3.1) does not
   guarantee that emitted code compiles against every accepted package version.
   Add an explicit generator target version and version-aware emission before
   claiming older API compatibility. A feature becomes a compatibility claim
   only after relevant source-shape, runtime, and interoperability evidence.
