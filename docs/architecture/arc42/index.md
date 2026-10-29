# Wireloom architecture

<!-- markdownlint-disable-next-line MD033 -->
<img src="../../assets/wireloom.svg" alt="Wireloom logo" width="360">

Wireloom generates C# data types and RTI type-specific support from supported [IDL](12-glossary.md#idl).  
It integrates generation into .NET builds and offers a supporting local browser Playground;
both use the [shared IDL compiler](05-building-block-view.md#531-shared-idl-compiler).  
The consumer application's RTI runtime remains responsible for DDS communication,
serialization, and representation negotiation.

| Chapter                                                        | Description                                                                              |
| :------------------------------------------------------------- | :--------------------------------------------------------------------------------------- |
| [1. Introduction and goals](01-introduction-and-goals.md)      | Drop-in replacement goals, quality priorities, and stakeholders                          |
| [2. Architecture constraints](02-architecture-constraints.md)  | Non-negotiable product boundaries and mandatory project policies                         |
| [3. Context and scope](03-context-and-scope.md)                | Product neighbors, expectations, and technical interfaces                                |
| [4. Solution strategy](04-solution-strategy.md)                | Architectural choices, rationale, consequences, and references                           |
| [5. Building block view](05-building-block-view.md)            | Product hierarchy, block responsibilities, and interfaces                                |
| [6. Runtime view](06-runtime-view.md)                          | Functional flows, alternatives, and shared compiler collaboration                        |
| [7. Deployment view](07-deployment-view.md)                    | .NET integration and Playground deployment, configuration, and publication               |
| [8. Cross-cutting concepts](08-crosscutting-concepts.md)       | Semantic identity, failures, compilation, ownership, evidence, and workspace consistency |
| [9. Architectural decisions](09-architectural-decisions.md)    | Evidence-qualified timeline, current choices, and historical background                  |
| [10. Quality requirements](10-quality-requirements.md)         | Stakeholder scenarios, acceptance measures, evidence, and open targets                   |
| [11. Risks and technical debt](11-risks-and-technical-debt.md) | Assessed risks, prioritized shortfalls, and open decisions                               |
| [12. Glossary](12-glossary.md)                                 | Linked definitions, abbreviations, and terminology                                       |

> [!NOTE]
> This architecture context is summarized in `.agents/architecture-memory.yaml` for implementation agents.  
> The arc42 chapters, linked ADRs, and contracts remain the primary sources.
