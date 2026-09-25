# 8. Cross-cutting concepts

## 8.1 Evidence vocabulary

The project distinguishes RTI acceptance, managed implementation, generated
contract shape, runtime eligibility, and verified wire compatibility. The
corpus feature index is the authoritative status view for source generation.

## 8.2 Diagnostics and source locations

Generator-owned diagnostics use stable IDs. IDL failures retain the original
input path and offset so a consumer can fix the source rather than inspect a
generated file. Unsupported constructs are errors, not silently ignored output.

## 8.3 Determinism and invalidation

Physical IDL files are canonical inputs. Shared files are emitted once per
project, generated hint names are deterministic, and edits to tracked includes
invalidate generation. Per-root incremental caching is not yet implemented.

## 8.4 Dependency and supply-chain boundaries

The consumer owns the RTI runtime reference. Roslyn compiler packages remain
private build dependencies. Central package management, lock files, NuGet audit,
pinned GitHub Actions, SBOM generation, and provenance attestation protect the
package path.

## 8.5 Test separation

Fast in-memory tests validate compiler invariants; corpus tests validate
feature-level acceptance and output shape; packed-package tests validate the
actual NuGet consumption path.

## 8.6 RTI-compatible generated value semantics

The generated API and native plugin are compatibility artifacts. When the RTI
oracle exposes an observable implementation detail, Wireloom preserves it even
when a different implementation would be more elegant or produce stronger
hashes.

- Array hash codes use the first element, not the array length. This preserves
  RTI's intentionally weak hash behavior. Empty arrays are not a supported
  input for this generated hash path.
- Primitive arrays use direct `SequenceEqual`. Arrays containing aggregate
  elements use the casted `Cast<T>().SequenceEqual(...)` path required by the
  runtime collection API.
- Newly allocated CLR arrays are trusted to be zero-initialized. The generator
  does not emit redundant per-element initialization, and each default-created
  sample owns a newly allocated array; caller-supplied arrays remain caller
  owned by the value constructor.
- Sequence members with array dimensions retain those dimensions in the
  emission model even though the managed public shape remains the RTI sequence
  shape. Integer sequence arrays use `NativeSeq`; bounded string sequence
  arrays use `NativeStringSeq` with the string bound passed through
  `FromNative`, `Initialize`, and `ToNative`.
- `optionalsOnly` cleanup is context-sensitive. Types with direct optional
  members destroy those members and return before required cleanup. Derived
  types return after base cleanup because the base may contain optional
  members. Direct strings, nested aggregates, aggregate-element collections,
  and sequence arrays retain the RTI outer guard. Primitive-only fields and
  primitive-only collections delegate cleanup to their member wrapper and do
  not need an outer guard.
