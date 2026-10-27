# Architecture memory

This derived file is a compact retrieval aid. The [arc42 chapters](arc42/index.md), ADRs,
contracts, support policy, and evidence are authoritative. Update those sources
first, then refresh this file and the [machine-readable memory](../../.agents/architecture-memory.yaml).

## System model

Wireloom is a compile-time Roslyn source generator for RTI Connext IDL. It emits
C# and build integration; it is not in the DDS runtime data path and does not
provide RTI support. Repository projects target .NET 10. The generator adapter
and shared compiler target netstandard2.0 for the minimum .NET 8 / C# 12 host
with Roslyn 4.8.0. Consumers need RTI 7.3.1 or later; only RTI 7.3.1 and 7.7.0
are qualified release profiles.

## Compiler flow and boundaries

The non-packable compiler accepts in-memory inputs and an explicit RTI profile,
then returns generated documents and diagnostics. It has no Roslyn, MSBuild,
RTI, or filesystem dependency. The Roslyn adapter discovers the resolved RTI
version, selects a profile, translates diagnostics, and adds generated sources.

The pipeline preprocesses inputs, parses syntax, binds symbols and values,
validates semantics, plans profile-specific output, and emits documents. Parsing
retains annotation order and source spans. Binding resolves constants, defaults,
bounds, enum identity, and member IDs. Semantic parsing and validation stay
independent of RTI profiles; emission plans own profile-specific API shape and
initialization.

See the [building block view](arc42/05-building-block-view.md),
[compiler runtime](arc42/06-runtime-view.md#61-consumer-build-and-generation),
and [shared compiler API](arc42/08-crosscutting-concepts.md#89-shared-compiler-api).

## Compatibility and configuration

OMG IDL and DDS-XTypes define language semantics. RTI-generated output and wire
runs provide versioned compatibility evidence, not a language ceiling. RTI C#
is the managed API baseline; deliberate improvements need separate behavior and
wire evidence. Exact profile qualification is limited to RTI 7.3.1 and 7.7.0.

- Strict validation defaults to false. Any generation root enables it across
  reachable inputs; strict failures stop the batch before emission.
- DDSG0108 reports naming findings in permissive mode. Reserved keywords fail
  in every interface body.
- Sequence arrays default to RtiCSharp mapping; Idl can be selected per file.
- Unbounded limits default to 100 sequence elements and 255 string bytes or
  code units. Explicit IDL bounds take precedence.

See [configuration](../guides/configuration.md),
[diagnostics](arc42/08-crosscutting-concepts.md#82-diagnostics-and-source-locations),
and [evidence vocabulary](arc42/08-crosscutting-concepts.md#81-evidence-vocabulary).

## Wire compatibility harness

The harness uses C# for fixture cataloging, deterministic planning, bounded
case builds and exchange scheduling, and evidence checks. Bash handles RTI
environment setup and per-case source staging. One bootstrap Wireloom build
prepares shared project references; remaining managed peers build concurrently
with isolated intermediate, generated-source, and output paths. Independent
native peers use a second bounded build queue. Each shard finishes peer builds
before one C# scheduler draws from the complete exchange plan. It starts each
reader before its writer and leases a unique active DDS domain to each
concurrent exchange, bounded to 16 by default. Peer subprocesses disable RTI
Monitoring Library 2.0 because the matrix does not consume its telemetry
participant. The mounted NuGet cache is used by restore and per-case builds.
Whole-case shards remain the outer build and evidence partition. The report
merger rejects incomplete, duplicate, or inconsistent evidence. See the
[wire runbook](../tests/Wireloom.Dds.Generator.WireCompatibility/README.md)
and [runtime view](arc42/06-runtime-view.md#63-wire-compatibility-evidence).

## Browser Playground

The desktop-first Playground uses a TypeScript/Vite client and a
non-packable .NET 10 browser-WASM host in a dedicated module worker. The worker
runs the shared compiler and synchronous JavaScript MD5 provider. Versioned
requests include worker, request, revision, profile, and file identity; the
client keeps one active and the newest pending request, and rejects stale
responses. Protocol v3 is owned by the .NET adapter DTOs, with a TypeScript
mirror checked in Quality. All Roots compile together with Include-only files
available through includes. Root properties carry Defines, Undefines, and
Strict; workspace properties carry sequence mapping/size defaults, escape
handling, and include folders. File-level sequence and string size preferences
override project defaults until reset. Include folders are searched alphabetically.

The UI provides Monaco IDL and read-only C# editors, examples, diagnostics,
generated-file tabs, individual downloads, and ZIP export. Source is stored
locally. The virtual Source tree supports explicit empty folders and atomic
imports, with stable per-file models preserving undo through tab switches and
moves. Recovery stores v3 workspace records and reports unsupported formats. Rootless and empty
workspaces clear output without compilation. Configured folders follow moves;
source include text stays unchanged. See the
[workspace guide and contract](../guides/playground-workspaces.md).
Source and Generated groups collapse independently with a blank line between
them; Explorer actions appear
on Source-group hover/focus. Folders expand by default and retain manual collapse
during edits. File menus change roles; folder menus configure include search. Source ZIP
saves the live tree, empty folders, and settings independently of generation.
Escape cancels dialogs, and Properties uses role/mapping toggles.
Active IDL Copy/Download works independently of compilation. Operation failures
use themed red Error notices with alert semantics.
Workspace checks pass in Chromium, Firefox, and WebKit; #119 CI and Coveralls
remain pending. Scoped Edge, Firefox, and Playwright WebKit checks are documented; a
WebKit result does not qualify Safari. Chromium coverage maps to first-party
code. Direct Node tests cover worker replacement and recovery-save ordering;
their coverage is gated at 98% branches and 99% lines. Combined source-mapped
Chromium coverage includes browser-only UI paths and is tracked separately.
ADR-003 is Accepted.
Generation qualification and its limits are recorded with issue #110's evidence.

GitHub Pages hosts the static bundle. Quality validates a separate configured-base
candidate on PRs and main. Publication accepts successful main push Quality
runs and Coveralls results, verifies the exact artifact inventory and source
SHA, and deploys without rebuilding. Only the environment-restricted deploy job
has Pages write/OIDC permissions. Superseded automatic runs are rejected;
manual runs can roll back to retained successful candidates. Public HTTPS
checks verify assets, MIME types, sample/profile generation, diagnostics,
exports, revision/scope, and reload. See the
[deployment runbook](../guides/playground-deployment.md).

See [ADR-003](adr/ADR-003-run-playground-compilation-in-a-browser-worker.md),
[browser runtime](arc42/06-runtime-view.md#64-browser-playground-compilation),
and [qualification scope](../testing/playground-qualification.md).

## Packaging and quality

The published package remains Wireloom.Dds.Generator. The shared compiler is
bundled beside it at the same version and is not a separate package. The sample
uses version 0.5.0 without native DDS libraries or an RTI license.

Quality CI runs exact-profile corpus and package/runtime checks, Community
Qodana, and Coveralls. Playground checks add strict TypeScript, ESLint,
Prettier, direct Node and Chromium coverage, and focused C# adapter tests. The
compiler suite requires 90% branch and 95% line coverage; its fuzz suite is
separate and excluded from coverage. Coveralls receives browser and adapter
Cobertura reports with compiler and generator reports. See [quality
requirements](arc42/10-quality-requirements.md),
[deployment](arc42/07-deployment-view.md), and the
[frontend dependency review](dependency-intake/playground-frontend.md).

Browser workspace quality scenarios require atomic changes, preserved editor
undo, and exact local recovery.

Release-bundle qualification adds 76 exact native/worker corpus comparisons
and 36 single-file UI comparisons per engine across both profiles, plus
limits, responsiveness, stale-result rejection, and recovery. Quality CI
retains Chromium reports with input/bundle hashes. The retained corpus remains
the compatibility source of truth; preview checks do not compile against RTI
or establish runtime/wire behavior. See the
[qualification guide](../testing/playground-qualification.md).

## Defaults and decisions

| Concern                | Default or decision                                  | State    | Source                    |
| :--------------------- | :--------------------------------------------------- | :------- | :------------------------ |
| SDK                    | .NET SDK 10.0.400, latestFeature roll-forward        | Explicit | global.json               |
| Repository target      | net10.0                                              | Explicit | Chapter 2                 |
| Root namespace         | Wireloom                                             | Detected | Repository files          |
| Generator and compiler | netstandard2.0; compiler is bundled and non-packable | Explicit | Chapters 2 and 7; ADR-001 |
| Consumer minimum       | .NET 8, C# 12, Roslyn 4.8.0                          | Explicit | Chapter 2                 |
| RTI profiles           | 7.3.1 and 7.7.0 are qualified                        | Explicit | Support policy            |
| Strict validation      | false; enabled batch-wide by any generation root     | Explicit | Chapter 8                 |
| Sequence arrays        | RtiCSharp by default; Idl is a file-level option     | Explicit | Chapter 8                 |
| Unit tests             | xUnit v3 on Microsoft.Testing.Platform               | Explicit | Repository conventions    |
| Assertions             | Shouldly                                             | Detected | Repository files          |

No provisional bootstrap defaults are currently recorded for this repository.
ADR-001 (shared compiler), ADR-002 (per-compilation MD5), and ADR-003 (browser
worker host) are Accepted. Michaël Hompus approved ADR-003 on October 26, 2026.
See [chapter 9](arc42/09-architectural-decisions.md) for decision history.

## Open risks

- Compatibility beyond retained cases and exact RTI profiles remains
  unqualified; wire checks require licensed RTI environments.
- Recursive sequence typedefs are unsupported. Self-recursive sequence wire
  checks require TypeObject v1; RTI 7.7.0 default TypeObject v2 rejects the
  recursive SCC. Mutual recursive structs remain unqualified.
- RTI 7.3.1 C# RPC output for C157/C159 fails compilation; see issue #75.
- Browser qualification remains scoped to the recorded evidence. Pages hosting
  is tracked in issue #111; live verification must pass for each deployment.
  Failed checks require inspection/manual rollback within artifact retention.
  A custom hostname changes both the base path and browser storage origin.
- Keep generator and bundled compiler assembly versions aligned. Preserve
  direct-caller path behavior and source-formatting parity.

Details and reproduction instructions remain in
[chapter 11](arc42/11-risks-and-technical-debt.md), the
[support policy](../guides/support-and-compatibility.md), and the source map.

## Source map

- [arc42 chapters](arc42/index.md)
- [Corpus and feature coverage](../corpus/README.md)
- [Support and compatibility](../guides/support-and-compatibility.md)
- [Configuration](../guides/configuration.md)
- [Playground workspaces and contract](../guides/playground-workspaces.md)
- [Runtime contract matrix](../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md)
- [Wire compatibility runbook](../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md)
- [ADR-001: shared compiler](adr/ADR-001-share-a-host-neutral-compiler-project.md)
- [ADR-002: per-compilation MD5](adr/ADR-002-inject-member-id-md5-per-compilation.md)
- [ADR-003: browser worker host](adr/ADR-003-run-playground-compilation-in-a-browser-worker.md)
- [Browser dependency review](dependency-intake/playground-frontend.md)
- [Machine-readable architecture memory](../../.agents/architecture-memory.yaml)
