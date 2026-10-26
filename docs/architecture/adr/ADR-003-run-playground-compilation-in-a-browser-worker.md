# ADR-003 Run Playground compilation in a browser worker

- **Date:** 2026-10-26
- **Status:** Accepted
- **Approved by:** Michaël Hompus

## Context

The accepted issue #109 plan adds a browser Playground that calls the shared
`Wireloom.Dds.Compiler` without duplicating preprocessing, parsing, semantic
validation, or emission. Browser WebAssembly compilation is synchronous and
can block the page if it runs on the main thread. Member-ID hashing also needs
a synchronous provider because the compiler accepts a synchronous per-call
MD5 delegate. The existing provider spike established an interpreted .NET 10
browser build and a JavaScript MD5 implementation, but it did not qualify a
maintained worker, request scheduling, recovery, or the application bundle.

The application needs a client bundle that supports root and nested static
hosting while carrying the separately published .NET WebAssembly runtime
assets. Request and response identity must prevent late worker results from
replacing newer workspace state. The shared compiler and the sole published
generator package must remain unchanged.

## Decision

**Host the shared compiler in a dedicated .NET 10 WebAssembly module worker behind a typed TypeScript/Vite client.**

The browser project references `Wireloom.Dds.Compiler`; the
worker initializes .NET and the synchronous JavaScript MD5 provider in the
worker scope. The client sends bounded immutable workspace snapshots and
rejects responses unless protocol version, worker generation, request ID,
workspace revision, and profile all match. It keeps only the running request
and newest pending request. The current UI submits one generation root, while
the collection-shaped protocol accepts multiple virtual files for incremental
multi-file UI support. Vite builds the static client with a configurable base
path, and the published .NET runtime files are copied into the generated
client asset directory. There is no main-thread compilation fallback.

## Consequences

- Windows Edge 156.0.4314.42 completed a 128-declaration compile that produced
  512 documents in 357.1 ms. The main-thread edit during that run took 30.7 ms
  and the maximum observed heartbeat was 16.5 ms. This is representative host
  evidence, not a cross-browser performance guarantee.
- The compiler, RTI profile selection, MD5 digest validation, and member-ID
  semantics remain shared with native generation.
- A timed-out or failed worker must be replaced; same-worker cancellation does
  not interrupt synchronous compilation.
- The client maintains pinned npm tooling and a lockfile in addition to the
  repository's locked NuGet restore.
- The clean .NET runtime asset tree currently contains 63 files and
  14,208,318 uncompressed bytes. The Vite bundle's `dotnet/` directory
  currently contains 65 files and 14,211,428 bytes including the worker and
  MD5 module. In Windows Edge 156.0.4314.42, worker resources transferred
  5,798,222 bytes per root/nested path (5,792,822 encoded bytes). Fresh
  isolated-context readiness measured 149 ms at root and 159 ms nested;
  reload readiness measured 124 ms and 122 ms. These measurements do not cover
  other browser engines or deployment networks.
- Current source includes a 256 KiB total input cap, 512 document cap, and
  16 MiB generated-text cap. These are starting values, not hard browser memory
  guarantees; the observed Edge results do not qualify other browser targets.
- This host does not establish RTI wire compatibility or generated-source
  usability against an RTI runtime; broader release qualification remains
  tracked in #110.

## Considered options

1. **Run the .NET compiler on the main thread**
   - **Pros**: Simpler initialization and messaging.
   - **Cons**: Synchronous work blocks page interaction and can prevent useful
     timeout or recovery behavior.
2. **Duplicate compiler behavior in TypeScript**
   - **Pros**: Avoids loading .NET WebAssembly in the browser.
   - **Cons**: Creates a second parser/emitter and risks output and diagnostic
     drift from the shared compiler.
3. **Run the shared compiler in a dedicated WebAssembly worker (selected)**
   - **Pros**: Reuses the compiler and isolates synchronous work from the page.
   - **Cons**: Adds WebAssembly asset, startup, and worker-lifecycle concerns
     that need browser release evidence.

## More information

- Dependency review: [Browser Playground frontend intake](../dependency-intake/playground-frontend.md).
- Official [.NET on Web Workers guidance](https://learn.microsoft.com/en-us/aspnet/core/client-side/dotnet-on-webworkers?view=aspnetcore-10.0).
- Official [Vite production build guidance](https://vite.dev/guide/build).

## References

- Related building block: [Level 1 white-box overall system](../arc42/05-building-block-view.md#51-level-1-white-box-overall-system).
- Related runtime flow: [Browser Playground compilation](../arc42/06-runtime-view.md#64-browser-playground-compilation).
- Related concept: [Shared compiler API](../arc42/08-crosscutting-concepts.md#89-shared-compiler-api).
- Related code: `src/Wireloom.Playground.Browser/` and `playground/`.
