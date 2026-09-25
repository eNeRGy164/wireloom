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
