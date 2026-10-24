# 12. Glossary

## 12.1 Definitions

- **Strict validation** — A batch-wide semantic policy that rejects supported
  IDL specification violations before emission. It is distinct from compiler
  warnings-as-errors and identifier escaping.
- **Annotation application** — One `@name` with an optional balanced argument
  list. The parser captures its source order, raw text, and location before
  known built-ins are interpreted; unknown applications stay opaque.
- **Constant expression** — An IDL `const_expr` evaluated after referenced
  symbols are available. Supported operators and type rules follow OMG IDL;
  RTI output serves as compatibility evidence.
- **Sequence-array mapping** — The file-owned choice between default
  `RtiCSharp` flattening of direct sequence arrays and opt-in `Idl` preservation
  of the outer array and independently sized sequence elements. It does not
  select an RTI runtime version or a general IDL compliance profile.
- **Effective limit** — The finite sequence or string bound used for an IDL
  value with no explicit bound. Project settings provide the default and IDL
  file metadata can override it.
- **Connext IDL** — The RTI Connext-supported subset of OMG Interface Definition
  Language used to declare DDS user data types.
- **DdsIdl root** — An explicit MSBuild item that starts generation and owns a
  reachable include closure.
- **Included IDL** — An IDL file reached through `#include`; it is tracked as an
  input but is not independently generated as a root.
- **Generated source** — C# documents added to the consumer compilation by the
  Roslyn generator.
- **RTI runtime** — The application-selected RTI Connext DDS libraries that own
  communication, serialization, and representation negotiation.
- **Type-specific support** — Generated `TypeSupport`, plugin, metadata, and
  conversion surfaces consumed by the RTI runtime for a data type.
- **RTI oracle** — Recorded behavior of a named RTI Connext and `rtiddsgen`
  version for one compatibility case.
- **Compatibility corpus** — The manifest, IDL fixtures, oracle sources, and
  status index used for feature-level verification.
- **Wire compatibility** — The ability of generated types to exchange DDS
  samples correctly under a defined runtime and representation profile.
- **Wire compatibility matrix** — The manually dispatched set of DDS
  producer/consumer scenarios across the positive Wireloom corpus, C#/C++
  peers, curated fixtures, and named RTI runtime versions.
- **Wire fixture** — A deterministic, complete sample variant for one corpus
  case. Each fixture is exchanged independently and receives its own result
  entry across the configured language pairings.
- **RTI API profile** — The generated-code API surface selected automatically
  from the resolved `Rti.ConnextDds` assembly version. Versions from 7.3.1 to
  below 7.7.0 select the 7.3.1 profile; 7.7.0 and later select the 7.7.0
  profile. The profile does not imply qualification for intermediate or later
  RTI releases.
- **DDS-XTypes (XTypes)** — The DDS type system for describing data types and
  their compatibility and evolution rules. See RTI's
  [Extensible Types Guide](https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/extensible_types_guide/RTI_ConnextDDS_CoreLibraries_ExtensibleTypes_Guide.pdf).
- **XCDR2** — The second-generation XCDR data representation used to encode
  DDS samples. It is a wire representation, separate from the type metadata
  used to describe a sample's structure. See the RTI
  [Extensible Types Guide](https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/extensible_types_guide/RTI_ConnextDDS_CoreLibraries_ExtensibleTypes_Guide.pdf).
- **TypeObject** — A serialized DDS-XTypes representation of type information
  used for type discovery and consistency checks; it describes a type rather
  than carrying a topic sample. See the RTI
  [Extensible Types Guide](https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/extensible_types_guide/RTI_ConnextDDS_CoreLibraries_ExtensibleTypes_Guide.pdf).
- **TypeCode** — An RTI API representation of a type description used by
  language bindings and type-support APIs. It is distinct from sample data and
  from the TypeObject wire representation. See the RTI
  [Extensible Types Guide](https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/extensible_types_guide/RTI_ConnextDDS_CoreLibraries_ExtensibleTypes_Guide.pdf).
- **DynamicType** — An RTI runtime object that describes a DDS type, including
  its members and their types. It can be used to build type metadata at
  runtime. See the RTI [DDS-XTypes API reference](https://community.rti.com/static/documentation/connext-dds/current/doc/api/connext_dds/api_cpp2/group__DDSXTypesModule.html).
- **DynamicData** — An RTI runtime value object for creating, reading, or
  writing a sample by member name or ID without relying on a generated
  application data class. See the RTI [DDS-XTypes API reference](https://community.rti.com/static/documentation/connext-dds/current/doc/api/connext_dds/api_cpp2/group__DDSXTypesModule.html).
- **Extensibility** — A DDS-XTypes setting that determines which type changes,
  such as adding or reordering members, can remain compatible. Extensibility
  alone does not guarantee that two peers can exchange a type; their type
  consistency settings and representations also matter. See the RTI
  [Extensible Types Guide](https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/extensible_types_guide/RTI_ConnextDDS_CoreLibraries_ExtensibleTypes_Guide.pdf).
- **Optional presence** — Whether an optional member is absent or present.
  A present member can contain a default or empty value, so presence is
  separate from the member's value. See the
  [generated value semantics guide](../../guides/generated-value-semantics.md).
- **Qodana** — JetBrains' code-quality analysis platform, used in this
  repository to inspect code and report maintainability issues. See the
  [Qodana documentation](https://www.jetbrains.com/help/qodana/about-qodana.html)
  and the local [quality procedure](../../../CONTRIBUTING.md#quality-evidence).
- **Docker Desktop** — the local container runtime used to run pinned RTI
  capture images and the Linux-based Qodana image. See the local
  [quality procedure](../../../CONTRIBUTING.md#quality-evidence).
