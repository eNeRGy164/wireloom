# ADR-005 Deliver the native C++ generator through NuGet

- **Date:** 2026-11-05
- **Status:** Accepted
- **Approved by:** Michaël Hompus

## Context

The C++ generator is implemented in .NET, but native consumer builds do not
otherwise need the repository's .NET SDK or runtime. Installing a NuGet client
or Visual Studio C++ tools does not establish that runtime prerequisite.
Generation must run during ordinary native builds without making consumers
provision the generator's implementation platform separately.

The [solution strategy](../arc42/04-solution-strategy.md#48-keep-c-and-c-generation-separate)
already calls for separate C++ delivery with native build entry points. This
decision selects the executable deployment model within that package.

## Decision

**Ship the C++ generator as self-contained Native AOT executables for the build host in its separate NuGet package.**

The package contains Windows x64 and Linux x64 executables together with
native MSBuild and CMake integration. Both integrations select the executable
by build-host OS and architecture, independently of the application's C++
target. Generation requires no installed .NET runtime or SDK; compiling and
running generated applications still requires the corresponding RTI native SDK.

## Consequences

- Consumers invoke a native tool directly through their build integration.
  The generator's managed implementation adds no .NET installation step.
- Producers build each host payload on its matching OS and assemble one
  package. Package checks must exercise the actual bundled executables on
  both hosts.
- The package embeds runtime code. Runtime security fixes require rebuilding
  and redistributing the generator, even when its IDL behavior is unchanged.
- Native toolchain, operating-system, and architecture requirements become
  part of the package's maintenance and validation scope.
- Packaging and generator execution qualify delivery; generated application
  compilation and DDS behavior need their own checks.

## Considered options

### Framework-dependent CLI

- **Pros:** Small deployment with one managed executable.
- **Cons:** Consumers must install a compatible .NET runtime.

### Self-contained managed CLI

- **Pros:** Requires no separately installed runtime.
- **Cons:** Carries a managed runtime payload for each host and retains
  host-specific distribution and servicing work.

### Native AOT CLI

- **Pros:** Provides direct native invocation without a consumer runtime
  installation; a local probe confirmed generation with .NET removed from PATH.
- **Cons:** Requires native publishing toolchains, AOT-compatible code, and
  separate host payloads with embedded-runtime servicing.

## More information

- [Native toolchain dependency review](../dependency-intake/cpp-native-aot.md)
- [Package verification](../../../tests/Wireloom.Dds.Cpp.Generator.PackageIntegration/README.md)

## References

- Related deployment: [Modern C++ build integration](../arc42/07-deployment-view.md#73-modern-c-build-integration)
- Related code: [Native package definition](../../../src/Wireloom.Dds.Cpp.Generator/Wireloom.Dds.Cpp.Generator.csproj)
