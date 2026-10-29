# 12. Glossary

Short definitions of the language, tooling, and abbreviations used in Wireloom's architecture.
The [concepts chapter](08-crosscutting-concepts.md) explains the maintenance rules and worked examples.

**Start here:** [DDS](#dds) · [IDL](#idl) · [RTI](#rti) · [Shared IDL compiler](#shared-idl-compiler) ·
[Roslyn](#roslyn) · [Playground](#playground) · [Qualification](#qualification)

## 12.1 Definitions

[A](#a) · [B](#b) · [C](#c) · [D](#d) · [E](#e) · [F](#f) · [G](#g) · [H](#h) · [I](#i) · [J](#j) · [K](#k) · [L](#l) ·
[M](#m) · [N](#n) · [O](#o) · [P](#p) · [Q](#q) · [R](#r) · [S](#s) · [T](#t) · [U](#u) · [V](#v) · [W](#w) · [X](#x) ·
[Z](#z)

### A

#### AdditionalFiles

An MSBuild item supplying tracked files to Roslyn analyzers and source generators. Wireloom uses these inputs for
generation roots and includes; root selection is explicit in the [configuration contract](../../guides/configuration.md#select-generation-roots-and-included-files).

#### ADR

_Architectural decision record._ A record of an architectural choice, its context, alternatives, and consequences. See
the [decision history](09-architectural-decisions.md).

#### Aggregate

An IDL type that groups members, such as a struct, union, or value type.

#### Alias

A declared name for another IDL type, introduced by `typedef`. Its generated C# representation can be a wrapper rather
than a C# language alias.

#### Analyzer

A component loaded by the .NET compiler to inspect code and report diagnostics. A [source generator](#source-generator)
contributes source code; both can be distributed in an analyzer package.

#### Analyzer package

A NuGet package containing components loaded during compilation. Wireloom's package contains its generator, matching
shared compiler, and MSBuild integration; it adds no application runtime dependency.

#### Annotation application

An IDL `@name` with optional arguments, such as `@key` or `@default(5)`, supplying information about a declaration. See
the [semantic reference](../../guides/idl-semantics.md#annotation-applications).

#### API

_Application programming interface._ The types, members, or operations that code exposes to other code.

#### Array

A fixed-size, possibly multidimensional collection declared in IDL. A [sequence](#sequence) has a variable number of
elements.

#### Artifact

A retained build or verification output, such as a NuGet package, Playground bundle, or exchange report.

#### Attestation

An attributable statement about an artifact, such as which build produced it or which dependencies it contains. See
[package publication](07-deployment-view.md#713-package-production-and-distribution).

### B

#### Base path

The URL path prefix used to locate a site's assets, such as `/wireloom/`. The Playground bundle is built for a
particular base path.

#### Batch

Inputs compiled together in one compiler invocation. Generation roots share the batch; some settings apply to the whole
batch and others to the declaring file.

#### BCL

_Base Class Library._ The standard .NET libraries, including the standard .NET MD5 implementation referred to in
[ADR-002](../adr/ADR-002-inject-member-id-md5-per-compilation.md).

#### Binding

Resolving parsed names, types, and expressions against declarations while preserving their typed meaning. See
[IDL meaning and identity](08-crosscutting-concepts.md#81-idl-meaning-and-identity).

#### Black box

A building block shown through its responsibilities and interfaces without expanding its internals. See the
[building-block notation](05-building-block-view.md#51-hierarchy-and-notation).

#### Bound value

A resolved value that retains its IDL kind and type identity: two enumerators with the same number can still belong to
different enums. See [IDL meaning](08-crosscutting-concepts.md#81-idl-meaning-and-identity).

#### Browser compilation preview

Generated C# and diagnostics produced locally from IDL in the Playground. It does not compile that C# against RTI or
establish runtime or wire compatibility.

#### Browser worker

A browser execution context that runs work separately from the page's main thread. The Playground uses a
[compiler worker](#compiler-worker) and a separate Monaco editor worker.

#### Build time

The stage when project inputs are evaluated, source is generated, and the application is compiled. Generated application
code executes later at runtime.

### C

<a id="delegate"></a>

#### Callback

A callable function supplied by one component for another to invoke, represented by a delegate in C#.
The MD5 provider is a compiler-host callback.

#### Canonical IDL name

The IDL name after any enabled identifier normalization, used consistently for binding, type metadata, and member-ID
hashing. Generated C# spelling is a projection of that identity.

#### CDR

_Common Data Representation._ A binary encoding framework underlying the [XCDR](#xcdr) representations used for DDS
samples.

#### CI

_Continuous integration._ Automated build and verification when changes are submitted or integrated; Wireloom uses
GitHub Actions.

<a id="corpus"></a>

#### Compatibility corpus

The retained manifest, IDL fixtures, RTI oracle captures, and status indexes used for feature-level verification. Often
shortened to **corpus**; see the [corpus guide](../../corpus/README.md).

#### Compilation result

An `IdlCompilationResult` containing generated documents and diagnostics. Expected compiler errors return diagnostics
without documents; caller errors, cancellation, and unexpected faults propagate separately.

#### Compilation snapshot

Inputs and settings captured for one compilation, insulated from later caller or editor changes. See
[predictable compilation](08-crosscutting-concepts.md#83-predictable-compilation).

#### Compiler core

An alias for the [Shared IDL compiler](#shared-idl-compiler).

#### Compiler host

The environment invoking or loading compiler work, such as an SDK/IDE hosting Roslyn or the browser hosting WebAssembly.
It is separate from the consumer application's target framework.

#### Compiler profile

An explicit `IdlCompilerProfile` selecting generated RTI support APIs. Direct callers supply it; the Roslyn adapter maps
the resolved RTI assembly version to it. See [RTI API profile](#rti-api-profile).

#### Compiler worker

The dedicated browser module worker hosting the .NET WebAssembly compiler and its MD5 provider. It keeps synchronous
compilation away from the page's main thread.

#### Connext IDL

IDL accepted by RTI Connext's tools and type system. Wireloom's implemented and qualified subset is recorded in the
[feature index](../../corpus/FEATURE-COVERAGE.md).

#### Constant expression

An IDL expression evaluated from literals, operators, and named constants after its referenced symbols are available.

#### Consumer

A project or application using Wireloom-generated contracts. A **consumer build** runs generation; the **consumer
application** later executes the compiled code with RTI.

#### Container

An isolated process environment built from an image containing tools and dependencies. Qualification uses exact RTI
toolchain images; containers are not required for consumer generation.

#### Coverage

A measure of exercised code lines or decision branches during tests. It is distinct from feature coverage and does not
prove every quality requirement or exchange passed.

#### Coveralls

The service receiving and reporting the repository's test-coverage results. See the
[quality procedure](../../../CONTRIBUTING.md#quality-evidence).

#### Current result

A Playground result whose request, worker, workspace revision, and profile identities still match current state. See
[workspace consistency](08-crosscutting-concepts.md#86-workspace-consistency).

### D

#### Data representation

The encoding used for DDS sample bytes, such as XCDR or XCDR2. It is distinct from the metadata describing the sample's
type.

#### DDS

_Data Distribution Service._ The communication standard used to exchange typed data between applications; RTI Connext
supplies the runtime implementation used by Wireloom consumers.

#### DDS-XTypes

_Extensible and Dynamic Topic Types for DDS_, also **XTypes**. The type system and rules for type description,
evolution, and data representation; see the
[OMG specification](https://www.omg.org/spec/DDS-XTypes/1.3/About-DDS-XTypes).

#### DdsIdl root

An explicit MSBuild `DdsIdl` item that starts generation. Roots can reach shared includes; a declaration's file-owned
settings remain associated with its declaring file.

#### Debounce

Delaying a request briefly while edits settle so intermediate states need not all be compiled.

#### Declaring file

The IDL file physically containing a declaration. File-owned mapping and effective-size settings follow that file,
including when it is reached through another root.

#### Determinism

Equivalent effective inputs and deterministic platform services produce stable outputs and diagnostics.
This property is separate from caching or reusing previous work.

#### Diagnostic

A compiler finding with an identifier, severity, message, and source location where available. Errors, accepted
warnings, and host failures have different meanings.

#### Directed exchange

One verification from a specified writer to a specified reader, using a declared fixture, RTI release, and
representation.

#### Discovery

The DDS process of finding compatible participants and endpoints. Finding a reader and writer does not itself prove that
sample values exchange correctly.

#### Discriminator

The value in an IDL union that selects which branch is active.

#### DLL

_Dynamic-link library._ A compiled library file; the generator and shared compiler assemblies are distributed as DLLs.

#### Docker Desktop

The local container platform used for RTI capture and Qodana procedures. See the
[quality procedure](../../../CONTRIBUTING.md#quality-evidence).

#### Drop-in replacement

Wireloom's adoption goal: existing supported application usage keeps working with the same IDL and DDS behavior. Exact
supported scope and deliberate differences remain explicit in the
[support policy](../../guides/support-and-compatibility.md).

#### DTO

_Data transfer object._ A data-only structure exchanged across an interface, such as the .NET/TypeScript workspace
request and response models.

#### DynamicData

An RTI runtime object for reading or writing a sample by member name or ID without a generated application data class.

<a id="dynamic-type"></a>

#### DynamicType

An RTI runtime description of a DDS type and its members, rather than a sample value. See
[IDL meaning and metadata](08-crosscutting-concepts.md#81-idl-meaning-and-identity).

### E

#### Effective limit

The finite sequence or string bound applied when IDL has no explicit bound. Hosts supply defaults and declaring-file
overrides; see [configuration](../../guides/configuration.md#fixed-behavior-and-configurable-limits).

<a id="emission-planning"></a>

#### Emission

Producing C# source documents from analyzed IDL and emission decisions. **Emission planning** determines generated
shapes and support choices before or during rendering.

#### Enum

A type declaring named values, such as `Stopped` and `Running`. The enum's identity matters even when a member has the
same numeric value as one in another enum.

#### Escaped IDL identifier

An IDL name whose leading underscore can be removed by the `DdsIdlEnableEscapeChar` normalization option. This is
separate from escaping a keyword in generated C#.

#### Evidence identifiers

Stable labels used in these docs: `C###` identifies a corpus case, `Q-##` a quality scenario, `R-##` a risk, `D-##` a
debt/capability gap, and `O-##` an open decision.

#### Evidence provenance

The record connecting a claim to its inputs, source revision, toolchain, profile, and outcomes. See
[compatibility and evidence](08-crosscutting-concepts.md#85-compatibility-and-evidence).

#### Extensibility

DDS-XTypes rules controlling type evolution: `final` fixes the structure, `appendable` allows members to be appended,
and `mutable` supports evolution through member identities. Matching and data-representation settings also affect
whether peers can exchange the evolved type.

### F

#### Fixture

A controlled input or data setup used by a test. An IDL fixture tests a declaration shape; a
[wire fixture](#wire-fixture) supplies an actual sample variant for exchange.

### G

#### Generated source

C# documents emitted by the shared IDL compiler. The Roslyn adapter adds them to a consumer compilation; the Playground
presents them as preview output.

#### Generation root

An input explicitly selected to start generation: a [DdsIdl root](#ddsidl-root) in MSBuild or a [Root](#root-playground)
file in the Playground.

#### GitHub Actions

GitHub's workflow service running repository builds, tests, qualification, and publication jobs.

#### GitHub Pages

The static HTTPS hosting service used for the public Playground. It serves the bundle; compilation runs in the visitor's
browser.

### H

#### Hash

A digest or fingerprint calculated from input bytes. Artifact hashes support identity/integrity checks; DDS member-ID
derivation is a separate use of hashing.

#### Hint name

A name identifying one generated C# document, unique within a compilation. See
[predictable compilation](08-crosscutting-concepts.md#83-predictable-compilation).

#### Host-neutral

Independent of a particular execution host. Wireloom's shared compiler receives in-memory inputs and platform services
rather than depending on Roslyn, a filesystem, or browser APIs.

#### HTTP

_Hypertext Transfer Protocol._ The request/response protocol browsers use to retrieve pages and assets; the public
Playground uses its secure form, HTTPS.

#### HTTPS

_Hypertext Transfer Protocol Secure._ [HTTP](#http) over an encrypted, authenticated connection, used to deliver the
public Playground.

### I

#### ID

_Identifier._ A value naming something, such as a request or workspace file; a [member ID](#member-id) has a specific
DDS type-identity role.

#### IDE

_Integrated development environment._ Tooling for editing, building, navigation, and diagnostics; Wireloom's required
interactive environments are VS Code with C# tooling and Visual Studio 2026.

#### IDL

_Interface Definition Language._ A language describing data types and interfaces independently of an implementation
language; Wireloom turns supported data declarations into C#.

#### Include closure

The files reachable from a root by following `#include` directives, including transitive includes. Different roots can
share files in their closures.

#### Include-only (Playground)

A source-file role making a virtual file available through includes without independently starting generation. Its
declaring-file options remain effective.

#### Included IDL

A tracked source reached through `#include`. Inclusion describes how it is reached; the same supplied file can also be
explicitly selected as a generation root.

#### Incremental generator

A Roslyn source generator whose inputs and transformations are tracked for reuse across compilations. Wireloom currently
collects output work into a shared batch.

#### IndexedDB

The browser's structured local database used for saved Playground workspaces. Data belongs to its browser origin and
depends on storage availability.

#### IntelliSense

The editor's code-completion, symbol, and documentation assistance. Generated C# support does not imply a separate IDL
semantic language server.

#### Interop

Short for interoperability: passing calls or values across language/runtime boundaries, such as JavaScript/.NET or
managed/native RTI storage. DDS wire compatibility is a separate, specifically qualified exchange contract.

### J

<a id="js"></a>

#### JavaScript

The programming language running the browser client infrastructure and verification scripts; often abbreviated **JS**.

#### JSON

_JavaScript Object Notation._ The text data format used for workspace messages, saved records, and many verification
reports.

### K

#### Key

An IDL member or set of members identifying a DDS data instance. Distinct key values distinguish instances within a
topic.

### L

#### Local recovery

Restoring a valid saved Playground workspace from browser-origin storage. It preserves source and settings within that
contract; it is not a durable backup guarantee.

#### localStorage

The browser's origin-local key/value storage used for editor preferences, separately from IndexedDB workspace recovery.

<a id="lock-file"></a>

#### Lockfile

A retained record of exact resolved dependency versions used for reproducible package restore, such as
`packages.lock.json` or `package-lock.json`.

#### LTS

_Long-term support._ A vendor's supported release line; Wireloom's exact RTI qualification targets and evidence are
recorded in the [LTS profiles](../../corpus/LTS-PROFILES.md).

### M

#### Macro

A preprocessor definition expanded into source text, optionally with arguments. Macro expansion is processed before IDL
parsing.

#### Managed code

Code and values governed by the .NET runtime. Native RTI storage has separate representation and resource-lifecycle
rules.

#### Manifest

A structured inventory describing a corpus, planned run, or artifact's contents and identities. Its meaning depends on
which inventory it describes.

#### MD5

_Message-Digest Algorithm 5._ The digest algorithm used in DDS member-ID derivation; the host supplies digest production
per compilation. See [ADR-002](../adr/ADR-002-inject-member-id-md5-per-compilation.md).

#### Member ID

The numeric identity assigned to a field or union branch in DDS type metadata, explicitly or through the selected
automatic-ID rules.

#### Metadata

Information describing a type, member, document, or artifact rather than its application value. DDS type metadata
describes the sample's structure, not the sample itself.

<a id="mtp"></a>

#### Microsoft.Testing.Platform

The test execution platform used by the repository, also abbreviated **MTP**. Its runner options differ from legacy
test-runner filtering; see the [test procedure](../../../CONTRIBUTING.md#tests-and-fixtures).

#### MIME

_Multipurpose Internet Mail Extensions._ MIME types identify served content formats, including the JavaScript and
WebAssembly assets needed by browser workers.

#### Module worker

A browser worker loaded as a JavaScript module, allowing module imports. The Playground uses one for its compiler host.

#### Monaco

The browser editor library used to edit IDL and display generated C# in the Playground.

#### MSBuild

_Microsoft Build Engine._ The .NET project evaluation and build system; Wireloom's targets translate project items and
settings into tracked generator inputs.

### N

#### Native storage

RTI-compatible memory with explicit initialization, conversion, and cleanup rules. See
[generated values and resource ownership](08-crosscutting-concepts.md#84-generated-values-and-resource-ownership).

#### .NET

The development platform and runtime used by Wireloom and its consumer applications. Producer SDK, compiler host, and
application target framework are separate choices.

#### netstandard2.0

The .NET Standard 2.0 API contract targeted by the shared compiler and generator assemblies. It is not the consumer
application's target or the browser runtime version.

#### Node.js

The JavaScript runtime used by frontend build tools and repository verification scripts outside the browser.

#### npm

The package-management and script-running tool used for the Playground's JavaScript dependencies and build checks.

#### NuGet

The .NET package distribution and restore system used to deliver Wireloom's analyzer package and resolve consumer
dependencies.

#### Nullable annotation

C# type information indicating whether a reference may be null, used by compiler analysis.
It is separate from whether a DDS optional member is absent or present.

### O

#### OIDC

_OpenID Connect._ An identity protocol used by workflows to obtain short-lived identity tokens, including the Pages
deployment job's publication access.

#### OMG

_Object Management Group._ The standards organization publishing the IDL, DDS, and DDS-XTypes specifications relevant to
Wireloom.

#### Optional presence

Whether an optional member is absent or present; a present member can still contain a default or empty value. See
[generated-value semantics](../../guides/generated-value-semantics.md).

#### Origin

A browser security/storage boundary determined by URL scheme, hostname, and port. Changing the origin does not
automatically move saved Playground workspaces.

#### OS

_Operating system._ Qualification records name the OS alongside tool, browser, and SDK versions.

### P

#### Parsing

Recognizing the structure of supported IDL syntax and constructing declarations for later binding and validation.

#### Partial type

A C# type with multiple declarations, often in separate files, allowing generated definitions and handwritten
extensions to remain separate.

#### Peer

A participating process or implementation in a DDS exchange, such as generated Wireloom C#, RTI C#, or RTI C++ code.

#### Playground

Wireloom's supporting browser demo/evaluation application for editing IDL and inspecting locally generated C# and
diagnostics.

#### PR

_Pull request._ A proposed repository change reviewed and validated before integration.

#### Preprocessing

Expanding includes and macros and selecting conditional source before parsing, while retaining original source
locations.

#### Profile mapping

Selecting a generated API profile from the consumer's resolved RTI version. Mapping a version to a profile does not
establish qualification for that exact release.

#### Promotion

Publishing the already validated artifact bytes to an environment, rather than rebuilding them during publication.

#### Publication candidate

A built artifact considered for release, with source identity and validation evidence. Eligibility checks determine
whether it may be promoted.

### Q

#### Qodana

JetBrains' code-quality analysis platform used by the repository and potentially by consumers. See the
[quality procedure](../../../CONTRIBUTING.md#quality-evidence).

#### Qualification

Evidence establishing a declared behavior for named inputs, exact tool/runtime versions, and environment or peer scope.
A passing generation check alone does not qualify DDS behavior.

#### Quality gate

A required validation result that a change or artifact must satisfy before the relevant integration or publication step.

### R

<a id="datareader"></a>

#### Reader

A DDS receiving endpoint, also called a **DataReader**. It receives samples from compatible writers.

#### Recursive type

A type that refers to itself, directly or through other types. Support and qualification depend on the shape and peer
setup; see [compatibility risks](11-risks-and-technical-debt.md#r01).

#### Rollback

Restoring a previous deployment by promoting an eligible retained candidate. The Playground uses a manual, validated
rollback path.

#### Root (Playground)

A virtual source-file role that starts generation as part of the workspace batch. All Root files compile together.

#### Roslyn

The .NET compiler platform providing the C# compiler, generator APIs, syntax models, and diagnostics used by .NET
build/IDE integration.

#### Roslyn adapter

Wireloom's source generator translating tracked project inputs and compiler results into Roslyn's contracts, including
RTI profile selection and generated-document delivery.

#### RPC

_Remote procedure call._ Invoking a service operation across process boundaries; IDL service declarations and RTI RPC
bindings have separate support limits from ordinary data types.

#### RTI

_Real-Time Innovations._ The vendor of Connext DDS and `rtiddsgen`; Wireloom is an independent project. See
[RTI's company information](https://www.rti.com/company/about-rti).

#### RTI API profile

The generated support API surface matching a known RTI baseline. The Roslyn adapter selects it from the resolved runtime
version; the [support policy](../../guides/support-and-compatibility.md#rti-version-qualification) owns version mapping
and exact qualification.

<a id="oracle-capture"></a>

#### RTI oracle

Retained output or observed behavior from a named RTI Connext/`rtiddsgen` version for a compatibility case. An **oracle
capture** records that evidence and its provenance.

#### RTI runtime

The application-selected RTI Connext DDS libraries responsible for communication, serialization, and representation
negotiation after generation.

#### rtiddsgen

RTI's code-generation tool for translating IDL into language bindings and support code. Consumer builds using Wireloom
do not invoke it.

#### Runner

Repository verification tooling that prepares peers, executes planned exchanges, and retains outcomes and provenance.
See [supporting verification](05-building-block-view.md#55-supporting-verification-tooling).

#### Runtime

The execution environment and libraries used while code runs. Distinguish the consumer's RTI runtime, .NET runtime, and
browser WebAssembly runtime.

#### Runtime contract

An executable expectation about generated API/value/native-resource behavior. It is separate from source-shape
comparisons and DDS wire qualification.

### S

#### Sample

One application data value published or received through DDS, such as one instance of a generated struct.

#### SBOM

_Software bill of materials._ An inventory of software components/dependencies supplied with an artifact to support
supply-chain review.

#### SCC

_Strongly connected component._ A group in a directed graph where each node can reach the others; recursive DDS type
graphs can form SCCs.

#### Scope

The declaration region in which a name is visible or resolved, such as an IDL module. Configuration scope separately
describes which files or batch a setting affects.

#### SDK

_Software development kit._ Tools used to build software; the .NET SDK includes the compiler/build host selected for a
consumer build.

#### Semantic model

Declarations and resolved type/value facts representing what supported IDL means, rather than only how its text is
spelled.

#### Semantic validation

Checking resolved IDL against supported language rules and target/profile restrictions before unsafe output can be
emitted.

#### Sequence

A variable-length ordered collection declared in IDL, with an explicit bound or host-applied effective limit under
Wireloom's current finite mode.

#### Sequence-array mapping

The declaring-file choice between default `RtiCSharp` flattening of direct sequence arrays and opt-in `Idl` preservation
of the array/sequence layers. See [configuration](../../guides/configuration.md#choose-the-sequence-array-mapping).

#### Serialization

Converting a sample value into an exchange representation; **deserialization** reconstructs a value from that
representation.

#### SHA

_Secure Hash Algorithm._ A hash-algorithm family; repository references use **commit SHA** for a Git revision's object
ID and artifact manifests use content hashes.

#### Shard

A planned subset of a larger verification run, allowing independent execution with later completeness/consistency
checks.

#### Shared IDL compiler

The host-neutral `Wireloom.Dds.Compiler` implementation that preprocesses, parses, binds, validates, and emits C# from
IDL. Also called the **compiler core**; see its [building block](05-building-block-view.md#531-shared-idl-compiler).

#### SLA

_Service-level agreement._ A stated service commitment, such as a response-time obligation; historical timings and
internal watchdogs are not such a commitment.

#### SonarQube

A code-analysis platform used in consumer environments; qualification of generated-code handling is separate from
Wireloom's repository Qodana scan.

#### Source generator

A .NET compiler extension contributing source documents during compilation. Wireloom's incremental generator adapts the
shared compiler to this interface.

#### Source provenance

The connection between processed IDL and its supplied input path and original location. See
[source provenance and failure reporting](08-crosscutting-concepts.md#82-source-provenance-and-failure-reporting).

#### Stale result

A response whose request, worker, workspace revision, or profile no longer matches current state. It cannot replace or
export the current generated result.

#### Strict validation

The batch-wide semantic policy rejecting covered IDL specification violations before emission. It is distinct from
warnings-as-errors and identifier escaping.

#### Struct

An IDL data type grouping named members into one sample shape. Its generated C# representation follows the applicable
mapping.

#### Symbol

A named declaration represented during compilation, such as a type, member, enumerator, or constant. Binding resolves
references to the appropriate symbol.

### T

#### Target framework

The .NET API/runtime target of a project, expressed by a name such as `net8.0` or `net10.0`. It is distinct from the
SDK compiling it.

#### Toolchain

The specific tools and versions used together to build or qualify a result, such as an RTI generator, C++ compiler, and
runtime image.

#### Topic

A named DDS data stream associated with a data type; writers and readers communicate samples through matching topics and
compatible configuration.

#### Type-specific support

Generated `TypeSupport`, plugin, metadata, and conversion APIs consumed by the RTI runtime for one data type.

#### TypeCode

An RTI type-description API representation used by bindings and type-support code. It describes structure, not sample
contents, and is distinct from [TypeObject](#typeobject).

#### Typedef

The IDL declaration introducing an [alias](#alias) for a type.

#### TypeObject

A DDS-XTypes type-description structure exchanged for type discovery and consistency checks. Its v1/v2 representation is
distinct from the XCDR version encoding sample data.

#### TypeScript

The language adding static types to JavaScript used for the Playground client; its output runs as JavaScript in the
browser.

### U

#### UI

_User interface._ The controls and presentation through which a person interacts with a system, including the Playground
editor and workspace explorer.

#### Unbounded support

RTI-style allocation/deserialization behavior for sequences or strings declared without finite bounds. General opt-in
support is missing in Wireloom; see [D-01](11-risks-and-technical-debt.md#d01).

#### Union

An IDL type with alternative branches selected by a discriminator; the selected branch determines which member value is
active.

#### URL

_Uniform Resource Locator._ An address identifying a resource, such as the Playground site or an asset within its base
path.

#### UTF-16 offset

A zero-based position in **Unicode Transformation Format, 16-bit** code units, as used by C# and JavaScript strings. It
is not a byte offset or necessarily a count of visible characters.

#### UTF-8

_Unicode Transformation Format, 8-bit._ A text encoding with variable-length byte sequences, used for source files and
exports.

### V

#### Verifier

Repository tooling checking and combining retained exchange reports for assignment, completeness, uniqueness, and
consistency without rerunning licensed DDS exchanges.

#### Virtual workspace

Browser-owned source files and explicit folders under `/workspace/`, with virtual paths independent of host filesystem
paths or file handles.

#### Vite

The frontend build tool assembling the Playground's browser code and static assets.

#### VS Code

_Visual Studio Code._ The editor used with documented C# tooling as one required Wireloom interactive environment,
alongside Visual Studio 2026.

### W

#### Watchdog

A deadline mechanism detecting startup or compilation that has not completed, allowing fault presentation or worker
recovery. It is not an accepted user-response-time budget.

#### WCAG

_Web Content Accessibility Guidelines._ Accessibility guidance; Wireloom requires core keyboard/focus/label/error
behavior, while no formal WCAG conformance level is selected.

<a id="wasm"></a>

#### WebAssembly

The portable execution format used to run the .NET compiler in the browser. Also written **Wasm** or **WASM**.

#### White box

A building-block view expanding a parent into its children and showing their interfaces and neighbors.

#### Wire compatibility

The ability of generated types to exchange the expected DDS samples under a declared exact runtime, peer, and
data-representation scope.

#### Wire compatibility matrix

The planned set of directed exchanges for selected corpus cases, fixtures, peers, and exact RTI releases. Complete
report coverage is separate from every exchange passing.

#### Wire fixture

A deterministic, complete sample variant for one corpus case, exchanged independently with its own result for each
planned pairing.

#### Workspace revision

An increasing version of source, paths, roles, settings, and profile, used to discard obsolete results. See
[workspace consistency](08-crosscutting-concepts.md#86-workspace-consistency).

<a id="datawriter"></a>

#### Writer

A DDS publishing endpoint, also called a **DataWriter**. A directed test exchange pairs a particular writer with a
reader.

### X

#### XCDR

_Extended Common Data Representation._ A binary sample encoding defined by DDS-XTypes; XCDR and XCDR2 are distinct
versions. See
[RTI's data-representation guide](https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/extensible_types_guide/extensible_types/Data_Representation.htm).

#### XCDR2

Version 2 of the Extended CDR sample encoding, separate from TypeObject metadata representation versions. See
[XCDR](#xcdr).

#### XML

_Extensible Markup Language._ The structured text format used for .NET project files and generated API documentation.

#### XTypes

An alias for [DDS-XTypes](#dds-xtypes).

### Z

#### ZIP

An archive format used to export source workspaces or generated files, preserving multiple entries in one download.

---

[< 11. Risks and technical debt](11-risks-and-technical-debt.md) | [Index](index.md)
