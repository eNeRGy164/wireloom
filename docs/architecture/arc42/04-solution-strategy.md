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
   Direct sequence-array mapping is selected independently: the project
   `DdsIdlSequenceArrayMapping` default can be overridden by a file's
   `SequenceArrayMapping` metadata. It belongs to the declaring file rather
   than an include traversal or the project-wide preprocessor environment.
4. **Fail visibly at unsupported boundaries.**  
   Invalid input and unsupported contracts that cannot produce safe managed
   output fail with source-located diagnostics. Deliberately ignored constructs
   and potentially lossy mappings report diagnostics while generation continues;
   consumers must review those outcomes. See the
   [diagnostic rules](08-crosscutting-concepts.md#82-diagnostics-and-source-locations).
5. **Select the RTI API profile at the host boundary.**
   The Roslyn adapter reports missing or below-minimum references and maps the
   resolved runtime version through the shared compiler API. Repository hosts
   select a supported profile explicitly. Compilation orchestration passes
   that profile to emission planning. Parsing, binding, and semantic
   validation remain independent of RTI. Versions 7.3.1 through below 7.7.0
   select the 7.3.1 profile; 7.7.0 and later select the 7.7.0 profile. Only
   exact 7.3.1 and 7.7.0 versions receive qualification claims. Profile
   mapping does not qualify every version in a mapped range.
6. **Use IDL and DDS-XTypes to define language meaning.**
   Interpret supported constructs using the OMG specifications. Retained RTI
   output tests compatibility with a particular generator and runtime; it does
   not define the set of valid IDL programs. When old design wording conflicts
   with a specification, correct the wording and implementation together.
   Record RTI extensions, implementation limits, and interoperability evidence
   separately; a language feature does not need to reproduce an RTI generator
   defect. See [value semantics](08-crosscutting-concepts.md#88-annotation-constant-and-default-semantics).
7. **Share one host-neutral compiler project.**
   Repository C# hosts call the same `netstandard2.0` compiler with in-memory
   inputs and receive documents and diagnostics together. The Roslyn adapter
   keeps MSBuild parsing, runtime discovery, and IDE/build presentation. The
   core has no Roslyn, MSBuild, or RTI references and is bundled beside the
   adapter in the existing generator package.
8. **Keep browser compilation in a dedicated worker.**
   The Playground sends bounded, immutable workspace snapshots to a .NET 10
   WebAssembly module worker, which loads the shared compiler and synchronous
   MD5 provider. The TypeScript client retains only the running request and
   newest pending request, and applies only results with a matching protocol,
   worker generation, request ID, workspace revision, and profile. Worker
   failure is visible and recoverable by replacing the worker; the application
   has no main-thread compilation fallback. The maintained worker has passed
   representative Edge acceptance on root and nested hosting; broader browser
   and release qualification remains separate evidence work.
   The single-file discovery UI layers Monaco models, lexical colors, local
   source/profile recovery, and current-result exports over that boundary.
   The compiler still owns all language meaning and generated text.
