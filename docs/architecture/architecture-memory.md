# Architecture memory

The current .NET defaults are derived from the `dotnet-bootstrap` skill seed defaults and are provisional until confirmed or overridden in architecture source documentation or an ADR.

## Repository defaults

- Target framework: .NET 10 (`net10.0`)
- SDK: `10.0.401`
- Root namespace: `Wireloom`
- Package output: NuGet package
- Runtime identifiers: none; the source generator is runtime-neutral
- Test platform: Microsoft.Testing.Platform with xUnit v3
- Assertions: Shouldly
- Mocking: NSubstitute
- Integration testing: Testcontainers

Machine-readable defaults are maintained in `.agents/architecture-memory.yaml`.
