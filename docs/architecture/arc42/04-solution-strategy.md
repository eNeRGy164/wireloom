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
   Invalid input and unsupported contracts that cannot produce safe managed
   output fail with source-located diagnostics. Deliberately ignored constructs
   and potentially lossy mappings report diagnostics while generation continues;
   consumers must review those outcomes. See the
   [diagnostic rules](08-crosscutting-concepts.md#82-diagnostics-and-source-locations).
5. **Select the RTI API profile from the resolved runtime reference.**
   The Roslyn adapter reports missing or below-minimum references; compilation
   orchestration selects one immutable profile and passes it to emission
   planning. Parsing, binding, and semantic validation remain independent of
   RTI. Versions 7.3.1 through below 7.7.0 select the 7.3.1 profile; 7.7.0 and
   later select the 7.7.0 profile. Only exact 7.3.1 and 7.7.0 versions receive
   qualification claims. Profile mapping does not qualify every version in a
   mapped range. A feature becomes a compatibility claim only after relevant
   source-shape, runtime, and interoperability evidence.
