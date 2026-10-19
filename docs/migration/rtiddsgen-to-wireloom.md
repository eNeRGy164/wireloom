# Migrating from `rtiddsgen` to Wireloom

This guide helps teams replace RTI-generated C# type support with
`Wireloom.Dds.Generator`. It is based on the retained RTI Connext DDS 7.7.0 /
`rtiddsgen` 4.7.0 oracle. It does not qualify other RTI versions or promise
drop-in compatibility for every IDL shape or generator option.

Wireloom generates C# contracts and RTI-specific adapters at build time. The
consumer still supplies and owns the RTI runtime package and application
configuration. Generation and compilation evidence do not, by themselves,
prove DDS wire interoperability.

## Migration procedure

1. **Inventory the contract.** List every IDL root and included file, all
   preprocessor definitions and include directories, the RTI runtime version,
   and each `rtiddsgen` option used in the existing build. Record which options
   affect data shape, C# API, preprocessing, output layout, or just examples
   and makefiles. Use the option map below; investigate unverified entries
   before proceeding.
2. **Capture a reproducible baseline.** Preserve the current IDL, generated C#,
   project files, compiler settings, warnings, and successful build output.
   Record the exact RTI and `rtiddsgen` versions and the source revision. Keep
   this baseline available for API comparison and rollback.
3. **Remove old generated compile inputs.** Stop compiling the existing
   `rtiddsgen` type sources and plugin/support sources in the consumer project.
   Keep handwritten application code, IDL, QoS/configuration files, and any
   generated publisher/subscriber examples that the application still uses.
   Do not compile RTI-generated and Wireloom-generated definitions for the
   same IDL types into one assembly.
4. **Add the package and explicit IDL roots.** Add
   `Wireloom.Dds.Generator` as private build tooling and add an explicit
   `Rti.ConnextDds` package reference at the runtime version selected for the
   application. Declare intended roots with `DdsIdl`; use project properties
   or root metadata for include directories and preprocessor symbols. See the
   [package guide](../../src/Wireloom.Dds.Generator/README.md).

   For example, translate an RTI command such as
   `rtiddsgen -language C# -I Contracts\Shared -D PRODUCT=Widget Telemetry.idl`
   into project configuration:

   ```xml
   <PropertyGroup>
     <DdsIdlIncludeDirectories>$(MSBuildProjectDirectory)\Contracts\Shared</DdsIdlIncludeDirectories>
   </PropertyGroup>
   <ItemGroup>
     <PackageReference Include="Rti.ConnextDds" Version="7.7.0" />
     <PackageReference Include="Wireloom.Dds.Generator" Version="0.4.0" PrivateAssets="all" />
     <DdsIdl Include="Contracts\Telemetry.idl" Defines="PRODUCT=Widget" />
   </ItemGroup>
   ```

   This translates the example's language, include, define, and input intent;
   it does not reproduce RTI's generated file layout or prove that all IDL
   preprocessing expressions behave identically.

5. **Build and review diagnostics.** Compile the consumer and resolve every
   error. Review every warning and informational diagnostic, especially
   ignored annotations/interfaces and `DDSG0105` for direct arrays of
   sequences. Do not treat successful source generation as successful C#
   compilation. Check the [feature coverage matrix](../corpus/FEATURE-COVERAGE.md)
   for the exact IDL shape and retained evidence. For direct arrays of
   sequences, the suggested sequence-typedef workaround is not verified by
   the direct-form corpus case; qualify it separately before relying on it.
6. **Compare the public API.** Compare representative generated C# and
   application call sites against the baseline. Review names, member types,
   nullability, collection wrappers, constructors, aliases, unions, and
   support/type metadata. The [API delta section](#generated-c-api-differences)
   identifies known differences. Update handwritten code deliberately; do not
   use a passing shape-comparison test as proof of identical APIs.
7. **Validate runtime behavior and interoperability separately.** Run the
   application's tests for construction, mutation, copies, optional values,
   union selection, native conversion, and type support. Then run wire checks
   for the application's important IDL shapes and peer directions using the
   [wire-compatibility workflow](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md).
   Runtime API tests are not wire tests, and a passing wire fixture does not
   qualify untested shapes or deployment environments.
8. **Verify deployment and rollback.** Confirm the packaged application
   contains the intended RTI runtime and native dependencies, loads them in
   each target environment, and uses the intended IDL/type configuration.
   Keep the previous generated sources and project configuration in the same
   release history. If API, runtime, or wire acceptance fails, restore the old
   generated compile inputs and package/configuration changes, then rebuild
   from the captured baseline.

## `rtiddsgen` option map

The option names below follow RTI's [Code Generator command-line reference][rti-codegen-args]
for the 7.7 release line. Wireloom does not parse or forward these switches.
The selected RTI NuGet reference also selects Wireloom's API profile: versions
7.3.1 through below 7.7.0 use the 7.3.1 profile, while 7.7.0 and later use
the 7.7.0 profile. There is no profile override. Rebuild and review generated
contracts when migrating across that boundary, and consult the
[support and compatibility guide](../guides/support-and-compatibility.md) for
the exact-version qualification status.
Translate the intent into MSBuild configuration or an explicit build-process
change. Classifications describe the current Wireloom package, not an intent to
add future support.

| RTI option or behavior                                               | Status      | Wireloom behavior and migration action                                                                                                                                                                                                                                                           |
| :------------------------------------------------------------------- | :---------- | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `-language C#`, `-namespace`                                         | Fixed       | Wireloom emits C# only and follows the IDL module structure for namespaces. Remove these options; neither has a setting.                                                                                                                                                                         |
| `-d <outdir>`                                                        | Fixed       | Roslyn controls generated-source placement. Use compiler-generated-file settings to emit physical files for inspection; do not expect an `rtiddsgen`-style output directory.                                                                                                                     |
| `-I <directory>`                                                     | Mapped      | Use `DdsIdlIncludeDirectories` or root-item `IncludeDirectories`. Include resolution and preprocessing are implemented by Wireloom, so validate the actual include graph.                                                                                                                        |
| `-D <name>[=<value>]`                                                | Mapped      | Use root-item `Defines`. Values from all roots are combined for the project generation batch.                                                                                                                                                                                                    |
| `-U <name>`                                                          | Mapped      | Use root-item `Undefines`. Values from all roots are combined for the project generation batch.                                                                                                                                                                                                  |
| `-ppDisable`                                                         | Unsupported | There is no switch to bypass preprocessing. Wireloom always runs its deterministic preprocessor subset.                                                                                                                                                                                          |
| `-ppPath`, `-ppOption`                                               | Unsupported | Wireloom does not invoke an external C preprocessor or accept its executable/arguments. Check the [preprocessor feature matrix](../PREPROCESSOR-FEATURES.md) for supported constructs.                                                                                                           |
| `-generateIncludeFiles`                                              | Fixed       | Included IDL files participate in the root's generation graph and are tracked inputs. Wireloom does not select separate output per include file.                                                                                                                                                 |
| `-create typefiles`, `-update typefiles`, `-replace`                 | Fixed       | Successful roots generate the package's supported C# contract and support sources during compilation. There is no overwrite mode or selectable type-file set.                                                                                                                                    |
| `-create examplefiles`, `-create makefiles`, `-example`, `-platform` | Ignored     | Wireloom generates neither application examples nor RTI platform project/make files. Keep or replace those application/build steps separately.                                                                                                                                                   |
| `-sequenceSize`, `-stringSize`, `-unboundedSupport`                  | Fixed       | Effective unbounded limits are 100 sequence elements, 255 narrow-string UTF-8 bytes, and 255 wide-string UTF-16 code units. These limits are not configurable through these RTI flags; see [effective limits](../../src/Wireloom.Dds.Generator/README.md#effective-limits-for-unbounded-values). |
| `-strict`                                                            | Unverified  | Wireloom exposes a `Strict` root metadata value that enables strict validation for the whole generation batch. Its behavior is not established as equivalent to RTI's `-strict`; compare diagnostics on the project's IDL before migrating this setting.                                         |

**Status meanings:** _Mapped_ has a Wireloom setting for the same configuration
intent, though semantics may differ. _Fixed_ is selected by the package or
build. _Ignored_ has no effect because it concerns output Wireloom does not
produce. _Unsupported_ has no corresponding behavior. _Unverified_ has a
similar-looking setting but no established equivalence.

This table covers common C# type-generation settings, not every option in the
RTI command-line reference. For any option not listed, check the exact
RTI-version manual and determine whether it affects the contract before
replacing the existing generation step.

## Generated C# API differences

The table describes known source-level differences against the retained RTI
7.7.0 oracle. It does not imply that every RTI version emits the same API. See
the [feature coverage matrix](../corpus/FEATURE-COVERAGE.md) and
[runtime contract coverage](../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md)
for the specific evidence. A C# API difference is not by itself a DDS wire
difference.

| IDL shape or behavior              | RTI 7.7.0 C#                                                                                                       | Wireloom C#                                                                                          | Source migration impact                                                                                                                                               |
| :--------------------------------- | :----------------------------------------------------------------------------------------------------------------- | :--------------------------------------------------------------------------------------------------- | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Optional collection members        | Optional metadata matches in the retained examples; RTI-generated source has no nullable reference annotations.    | Uses nullable annotations, such as `ISequence<int>?` and `int[]?`.                                   | With nullable analysis enabled, callers may see additional compiler warnings and should handle absence explicitly.                                                    |
| Direct arrays of sequence members  | RTI warns and ignores the direct sequence-array layer in the retained oracle.                                      | Reports `DDSG0105`; generated shape may not preserve IDL semantics.                                  | Treat this as a contract gap. The documented sequence-typedef workaround needs separate qualification for the application's shape.                                    |
| Union branch access                | Retained RTI output exposes branch members and rejects inactive-branch reads with `InvalidOperationException`.     | Exposes branch members and rejects inactive-branch reads with `InvalidOperationException`.           | The covered public behavior is the same; validate additional discriminator and default-branch shapes individually.                                                    |
| Alias `ToString` output            | RTI has known unsupported scalar/collection alias formatting paths in the retained baseline.                       | Adds typed formatting for aliases where the RTI path is unsupported.                                 | This improves diagnostic display and can change text observed in logs or snapshots. It does not change the DDS sample representation.                                 |
| Collection element alias mapping   | Primitive/string aliases erase; enum/aggregate aliases map to their targets; nested collection aliases stay named. | Follows the same target-kind mapping.                                                                | Wireloom 0.3.0 callers may need to use primitive values instead of scalar-wrapper collection elements.                                                                |
| Collection alias chains in members | Uses the alias that directly defines the sequence or array.                                                        | Follows the same member and collection-element mapping; standalone typedef classes remain available. | For `typedef sequence<long> Numbers; typedef Numbers NumbersChain;`, a `NumbersChain` member has type `Numbers`. Use `.numbers.Value` and assign `Numbers` instances. |

The runtime contract suite also exercises collection copying and ownership,
optional presence, native conversion, and cleanup. Those tests establish
Wireloom runtime behavior for their tested shapes; treat a behavior as an RTI
compatibility claim only where the retained RTI comparison provides that
evidence.

### Keep three kinds of compatibility evidence separate

- **Source/API compatibility** asks whether existing C# source compiles and
  observes the same public types, members, nullability, and runtime contracts.
  Use generated-source comparison and application compilation.
- **Runtime behavior** asks whether generated values, copies, optional presence,
  unions, native conversions, and type support behave as expected. Use the
  runtime contract suite and application tests.
- **DDS interoperability** asks whether independently generated peers exchange
  the selected data correctly. Use recorded wire-compatibility runs for the
  exact type, RTI release, profile, and peer direction.

An improvement to diagnostics such as `ToString`, or a difference accepted by
the source-shape comparison, does not establish source compatibility or wire
compatibility beyond the cases actually tested.

[rti-codegen-args]: https://community.rti.com/static/documentation/connext-dds/7.7.0.1/doc/manuals/connext_dds_professional/code_generator/users_manual/RTI_Code_Generator_UsersManual.pdf
