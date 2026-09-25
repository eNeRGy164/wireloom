# Wireloom architecture

Wireloom is a managed .NET source generator that turns an explicit subset of
RTI Connext DDS IDL into C# data types and RTI type-specific support during a
Roslyn build. The RTI runtime remains responsible for DDS communication,
serialization, and representation negotiation.

Owner:  
Last reviewed: 2026-09-25

| Chapter                                                        | Status  | Description                                          |
| :------------------------------------------------------------- | :------ | :--------------------------------------------------- |
| [1. Introduction and goals](01-introduction-and-goals.md)      | draft   | Purpose, goals, and stakeholders                     |
| [2. Architecture constraints](02-architecture-constraints.md)  | current | Technical and repository constraints                 |
| [3. Context and scope](03-context-and-scope.md)                | draft   | System boundary and neighbors                        |
| [4. Solution strategy](04-solution-strategy.md)                | current | Main architectural approach                          |
| [5. Building block view](05-building-block-view.md)            | current | Compiler and package structure                       |
| [6. Runtime view](06-runtime-view.md)                          | draft   | Build-time generation flow and failures              |
| [7. Deployment view](07-deployment-view.md)                    | draft   | Consumer, CI, package, and runtime nodes             |
| [8. Cross-cutting concepts](08-crosscutting-concepts.md)       | current | Evidence, diagnostics, determinism, and supply chain |
| [9. Architectural decisions](09-architectural-decisions.md)    | current | Decisions embodied by the implementation             |
| [10. Quality requirements](10-quality-requirements.md)         | draft   | Measurable quality scenarios                         |
| [11. Risks and technical debt](11-risks-and-technical-debt.md) | current | Known limitations and follow-up work                 |
| [12. Glossary](12-glossary.md)                                 | current | Project terminology                                  |

> [!NOTE]
> Additional information: This architecture context is also summarized in [Architecture memory](../architecture-memory.md) and `.agents/architecture-memory.yaml` so implementation agents can use it efficiently in limited context windows.
> The arc42 chapters, linked ADRs, and contracts remain the primary source documents. If the derived memory differs, update the source docs first and refresh the derived memory.
