# 8. Cross-cutting concepts

These concepts describe rules maintainers must preserve when changing Wireloom.
They apply across compiler features, generated support, and host integrations.
Each section starts from a concrete example, then explains the rules that must
keep holding and the consequences of breaking them. Short term definitions
belong in the [glossary](12-glossary.md#121-definitions).

The surrounding chapters own the [solution strategy](04-solution-strategy.md),
[building blocks](05-building-block-view.md), [runtime flows](06-runtime-view.md),
[deployment and configuration entry points](07-deployment-view.md), and
[decisions](09-architectural-decisions.md). This chapter references those views
and their detailed guides rather than repeating them.

## 8.1 IDL meaning and identity

Two IDL values can have the same number while representing different kinds of
thing. For example:

```idl
enum EngineState { Stopped, Running };
enum DoorState { Closed, Open };

const EngineState InitialEngine = Stopped;
const DoorState InitialDoor = Closed;
```

`Stopped` and `Closed` both have numeric value zero. That does not make an engine
state a door state. This additional declaration must be rejected:

```idl
const DoorState WrongDoor = InitialEngine;
```

[Binding](12-glossary.md#binding) must retain the declared enum in the
[bound value](12-glossary.md#bound-value), rather than reduce both constants to
an untyped zero. Generated representations must use that same typed meaning.

Preserve these rules:

- Resolve names, types, constants, annotations, defaults, and member IDs into
  effective semantic facts. Managed initialization, native conversion, and
  [DynamicType](12-glossary.md#dynamic-type) metadata must consume those facts consistently.
- Preserve type identity and distinctions such as an absent annotation versus
  an explicit false value. An enum-valued constant must not become an untyped
  integer merely because its value is numeric.
- Keep [canonical IDL names](12-glossary.md#canonical-idl-name) consistent through
  binding, collision checks,
  metadata, and implicit member-ID hashing. Generated C# spelling is a
  projection of that identity, not an independent naming authority.
- Preserve the ownership of effective options. Batch settings and
  declaring-file settings have different scopes; reaching a declaration
  through another root or include must not silently transfer option ownership.
  The [configuration reference](../../guides/configuration.md#settings-at-a-glance)
  owns the exact scopes and precedence.
- Keep explicit IDL bounds separate from finite fallback sizes and from the
  RTI-style unbounded opt-in. The opt-in is declaration-owned, including for
  declarations reached through includes and aliases. Native lifecycle and DDS
  type metadata must derive from the same resolved policy.

When these rules diverge, an apparently correct C# value can carry different
native initialization or DDS type metadata. A language change therefore needs
review across all projections of the affected semantic fact.

The [semantic reference](../../guides/idl-semantics.md) owns detailed annotation,
constant, literal, and default rules. The
[language-analysis view](05-building-block-view.md#542-language-analysis) identifies
their implementation boundaries; [chapter 9](09-architectural-decisions.md)
records the decisions behind them.

## 8.2 Source provenance and failure reporting

A useful diagnostic identifies the supplied IDL that caused it, even after
preprocessing, includes, and identifier normalization. For example:

```idl
#define MISSING_TYPE Missing
module Outer { struct Broken { MISSING_TYPE value; }; };
```

Preprocessing replaces `MISSING_TYPE` with `Missing`. The unknown-type finding
must still point to the `MISSING_TYPE` use in the original IDL. This
[source provenance](12-glossary.md#source-provenance) lets the developer fix the
text they actually supplied.

Preserve the original input identity and zero-based
[UTF-16 offset](12-glossary.md#utf-16-offset) through
text transformations. Adapters translate that location into their host's
diagnostic or editor model; they must not substitute generated-C# locations
or invent source spans the compiler does not provide. This applies equally
to filesystem-backed and virtual inputs.

Keep failure categories distinct:

| Category                                                                           | Contract                                                                                   |
| :--------------------------------------------------------------------------------- | :----------------------------------------------------------------------------------------- |
| Invalid or unsupported IDL, including features unavailable in the selected profile | Expected compiler findings return diagnostics and no generated documents                   |
| Accepted ignored or potentially lossy behavior                                     | Generation may continue with a warning or informational diagnostic                         |
| Caller-contract violation or unexpected implementation fault                       | An exception propagates; it must not be disguised as an IDL finding                        |
| Cancellation                                                                       | Cancellation propagates; it is not successful partial compilation or an ordinary IDL error |

Diagnostic IDs are stable integration contracts. Core findings retain their
severity, message, and source origin when presented by a host. Semantic
acceptance and host diagnostic policy remain separate: promoting a warning
can fail a consumer build, while suppressing a warning cannot bypass a
semantic rejection. The
[diagnostic policy reference](../../guides/configuration.md#control-diagnostics)
and [strictness reference](../../guides/configuration.md#enable-strict-validation)
own the detailed policies; the
[package diagnostic catalog](../../../src/Wireloom.Dds.Generator/README.md#diagnostics)
owns individual IDs.

Incorrect origin mapping directs users to the wrong input. Collapsing failure
categories can hide implementation defects or make cancellation look like a
valid result. See the [correction flow](06-runtime-view.md#612-diagnose-and-correct-generation-problems)
and [shared compilation flow](06-runtime-view.md#64-c1-compile-an-in-memory-idl-batch)
for their host-visible effects.

## 8.3 Predictable compilation

A compilation is determined by its supplied sources, effective options,
selected [profile](12-glossary.md#compiler-profile), and platform-service results. State from an earlier or
concurrent call must not change that meaning.

For example, the compiler API copies an input's definition list:

```csharp
using Wireloom;

var defines = new List<string> { "FEATURE" };
var input = new IdlInput("sample.idl", "struct Sample { long value; };", defines);

defines.Clear();
// input.Defines still contains "FEATURE".
```

Clearing the caller's list must not change the input already created. The
same isolation principle applies to the
[snapshot](12-glossary.md#compilation-snapshot) captured for a compilation.

Preserve these rules:

- Each call owns its input snapshot and compilation state. Treat caller-supplied
  text and copied option collections as the inputs to that call; do not retain
  mutable semantic state across compilations.
- Keep source access and platform services explicit at the compiler boundary.
  The compiler uses supplied text rather than reaching into a host filesystem.
  A host-provided MD5 service must return the specified digest consistently;
  a host sharing that service across calls owns its concurrency safety.
- Preserve deterministic output ordering and unique
  [hint names](12-glossary.md#hint-name). A duplicate
  hint must fail rather than overwrite an earlier document. Deduplication must
  distinguish different preprocessed outputs of the same supplied source under
  different macro states. Tracked IDL dependencies must participate in
  invalidation; a cached result is valid only for its effective inputs.
- Bound preprocessing work, nesting, and output deterministically. Exceeding a
  safety bound must not pass partial preprocessed text into parsing.
  Cancellation remains observable and follows the failure contract above.

Leaking state or omitting an input from invalidation can make a successful
build depend on edit history. Time-based cutoffs alone would make acceptance
depend on machine speed; finite work limits provide a reproducible boundary.

The [safety-limit reference](../../guides/configuration.md#fixed-behavior-and-configurable-limits)
owns numeric limits and scope. The
[incremental baseline](../../performance/incremental-generator-baseline.md)
owns measured cache behavior; deterministic results do not imply per-root
incremental caching. The
[compiler contract](05-building-block-view.md#compiler-contract-and-payload-examples)
and [MD5 service decision](../adr/ADR-002-inject-member-id-md5-per-compilation.md)
define the relevant boundaries.

## 8.4 Generated values and resource ownership

Generated managed values, [native storage](12-glossary.md#native-storage), and
DDS type metadata describe the same IDL contract, but they have different
ownership and lifecycle rules.
A change to one representation must preserve the relationships to the others.

Consider a generated wrapper around a bounded sequence:

```idl
module RuntimeAliases {
    typedef sequence<long, 4> Numbers;
};
```

Its value constructor retains the supplied collection, while its copy
constructor creates an independent collection:

```csharp
using RuntimeAliases;
using Rti.Types;

var values = new Sequence<int> { 1, 2 };
var original = new Numbers(values);
var copy = new Numbers(original);

values[0] = 9;
// original.Value[0] is 9; copy.Value[0] is 1.
```

The constructors look similar, but promise different ownership. A maintainer
changing either path must preserve that distinction.

[Optional presence](12-glossary.md#optional-presence) is another distinction
that must survive representation changes:

| State                                 | Meaning                                                                  |
| :------------------------------------ | :----------------------------------------------------------------------- |
| Absent optional scalar                | No scalar value is present                                               |
| Present scalar with its default value | A scalar is present, even when its value is zero or another default      |
| Present empty collection              | A collection is present with no elements; it is not an absent collection |

Preserve these rules:

- Apply resolved defaults consistently to the initialization and metadata paths
  that support them. Managed construction, native initialization, and RTI
  missing-field restoration are different contracts; evidence for one does not
  establish the others.
- Distinguish reference retention from independent copying. Value constructors
  for alias wrappers can retain supplied aggregates or collections; tested copy
  constructors copy mutable nested values. Do not assume every constructor
  establishes independent ownership.
- Preserve collection dimensions, aliases, optional presence, union branch
  selection, and the selected mapping across managed/native conversions.
- Respect whether native conversion may allocate or must use existing storage.
  Release owned resources under the appropriate cleanup mode, including
  optional-only cleanup and inherited members.
- Keep equality, hashing, and diagnostic formatting aligned with their
  documented generated API contracts. Formatting is separate from DDS
  serialization.

Breaking these rules can produce shared mutable state where a caller expects
a copy, lose optional presence, or corrupt native resource handling while
ordinary managed tests still pass.

The [generated-value guide](../../guides/generated-value-semantics.md) owns
constructor, mutation, copying, and maintainer storage details. The
[runtime contract matrix](../../../tests/Wireloom.Dds.Generator.RuntimeContracts/README.md#coverage-matrix)
identifies covered shapes and gaps. Mapping settings belong to
[configuration](../../guides/configuration.md#choose-the-sequence-array-mapping);
the policy for deliberate developer-experience improvements belongs to
[the strategy](04-solution-strategy.md#45-generate-c-that-improves-the-developer-experience).

## 8.5 Compatibility and evidence

A compatibility claim has a scope. An API profile determines which support
surface is generated; qualification establishes what was actually checked
with named inputs, tool versions, and peers. Selecting a profile does not
qualify every runtime version mapped to it.

For example, suppose the Playground generates C# for `Sample.idl` with the
7.7.0 profile. This shows that generation completed for that input and profile.
It says nothing yet about compiling the generated application against RTI or
exchanging DDS samples with a C++ peer. Those claims need different evidence.

Keep these kinds of evidence distinct:

| Evidence                                         | What it establishes within its recorded scope                                                    |
| :----------------------------------------------- | :----------------------------------------------------------------------------------------------- |
| Language acceptance and generated contract shape | Supported inputs and the required generated declarations                                         |
| Exact generated-text comparison                  | Output equality for the compared inputs, profiles, and versions                                  |
| Packed-package consumption and compiled usage    | Whether the distributed integration loads and its generated API compiles                         |
| Generated API/runtime contracts                  | Observed managed behavior, native lifecycle/conversion, or metadata behavior                     |
| Directed DDS exchanges                           | Interoperability for the exercised writer/reader, type, representation, and runtime combinations |
| Generated-source analysis                        | Findings about emitted C# quality under the selected analysis rules                              |

Use one explicit profile consistently for the generated API and support paths.
Keep specification meaning, vendor behavior, and Wireloom implementation
limits distinguishable when interpreting a result.

Retain [evidence provenance](12-glossary.md#evidence-provenance) with a claim:
input/output hashes, source revision, toolchain/profile, and outcomes. Changed inputs require fresh evidence for
the changed scope; unknown historical provenance remains unknown.
Unavailable cases must remain distinguishable from failures and implementation
gaps. A complete, internally consistent report can still contain failing
exchanges.

Otherwise, successful generation or discovery can be mistaken for working
application behavior or interoperability.

The [support policy](../../guides/support-and-compatibility.md#rti-version-qualification)
owns profile mapping and qualification policy; the
[corpus index](../../corpus/FEATURE-COVERAGE.md) owns feature status.
The [LTS runbook](../../corpus/LTS-PROFILES.md) and
[wire runbook](../../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md)
own attribution and report contracts. [Chapter 10](10-quality-requirements.md)
owns acceptance criteria and evidence links; [chapter 9](09-architectural-decisions.md)
owns verification-strategy decisions.

## 8.6 Workspace consistency

An editable workspace and a compilation result have different identities and
lifetimes. The workspace is current user-owned source; generated output is a
derived result valid only for the snapshot that produced it.

For example, compare the workspace with a successful incoming result:

| Compared value     | Current workspace | Incoming result |
| :----------------- | :---------------- | :-------------- |
| Workspace revision | 13                | 12              |
| RTI profile        | 7.7.0             | 7.7.0           |

The profile matches, but the result belongs to older source or settings. It
must not replace the output or enable generated export. A
[current result](12-glossary.md#current-result) must match the
[workspace revision](12-glossary.md#workspace-revision) as well as the other
request identity fields.

Preserve these rules:

- Keep file identity separate from virtual path. Accepted moves and tab changes
  retain editor state; paths remain normalized and unique.
- Validate an entire proposed workspace change before applying it. Rejected or
  canceled operations leave the prior workspace intact. A path change must not
  silently rewrite IDL source.
- Accept a result only when worker generation, request, workspace revision, and
  profile match the dispatched snapshot and current workspace. Obsolete success
  and failure responses cannot replace current state.
- Permit generated export only for a current successful result with documents.
  Source backup represents the live workspace and remains independent of
  compilation success.
- Persist source, structure, roles, and settings rather than treating saved
  generated output as authoritative. Validate stored records before applying
  them, and preserve newer edits when recovery finishes late.
- Serialize recovery writes and deletion, and ignore obsolete completion
  callbacks. After successful reset deletion, older queued saves must not
  resurrect reset data. A save completion must not mark newer edits as saved.
  Storage failures remain explicit; editor preferences are separate from source recovery.

Breaking these rules can export obsolete contracts, discard edits, or restore
data the user has reset. Local recovery is a convenience with explicit failure
handling, not an alternative authority for compiler validity.
The [local-source constraint](02-architecture-constraints.md#21-product-and-execution-boundaries)
also applies to browser persistence and export.

The [workspace guide](../../guides/playground-workspaces.md) owns editing,
presentation, persistence formats, and protocol details. Its .NET/TypeScript
contract must remain aligned; deployment compatibility belongs to
[chapter 7](07-deployment-view.md#72-browser-playground-deployment).
The [preview](06-runtime-view.md#621-preview-the-current-workspace),
[workspace-change](06-runtime-view.md#622-change-a-workspace-safely), and
[recovery flows](06-runtime-view.md#624-restore-or-reset-the-local-workspace)
show these invariants in action.

---

[< 7. Deployment view](07-deployment-view.md) | [Index](index.md) | [9. Architectural decisions >](09-architectural-decisions.md)
