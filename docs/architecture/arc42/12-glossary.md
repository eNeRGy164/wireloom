# 12. Glossary

## 12.1 Definitions

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
