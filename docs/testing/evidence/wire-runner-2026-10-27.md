# Wire runner validation — October 27, 2026

These measurements and comparisons are historical observations. The source
revision, dirty-tree snapshot, and raw local reports were not retained in this
narrative. Ignored artifact paths below identify the original local locations,
not downloadable evidence. Do not use these counts to qualify a later source.
Current commands are in the [wire runbook](../../../tests/Wireloom.Dds.Generator.WireCompatibility/README.md).

## Full-matrix throughput validation

On 2026-10-27, the complete RTI 7.7.0 matrix ran on the pinned Docker image
with eight domain-isolated exchanges active per scheduler. All 1,555 exchanges
were recorded: 1,549 passed and the same six known interoperability failures
remained. Elapsed time was 11m45s after sharing project references, compared
with 13m17s at concurrency eight without reference reuse and 18m35s in the
preceding single-shard run at concurrency four. Representative shard 0/8 runs
passed all 198 exchanges in 2m00s before reference reuse and 1m46s afterward.
A 7.3.1 shard 0/8 run completed in 1m37s with 191 exchanges recorded: 181
passed, 10 failed, and none were unrun. These are single-host measurements; the
combined GitHub workflow time still depends on runner startup, image/cache
state, and scheduling and needs confirmation on a dispatched workflow.

With the bounded C# and native build queues enabled, a fresh 7.7.0 shard 0/8
completed all 198 exchanges in 108 seconds; a fresh 7.3.1 shard 0/8 completed
191 exchanges in 102 seconds, with the same 181 passes and 10 known failures.
The 7.7.0 shard's 10 non-bootstrap Wireloom peer builds each took 2.1–3.1
seconds while four builds ran concurrently. A four-case 7.7.0 smoke run passed
54/54 exchanges in 51 seconds. These timings include local container execution
with the NuGet cache mounted; the full combined GitHub workflow has not yet been
measured on this implementation.

The complete unsharded 7.7.0 rerun with the parallel build queues completed in
12m03s: 1,549 of 1,555 exchanges passed, with the same six known failures and no
unrun exchanges. This is a full correctness run, not a proxy for the eight-shard
workflow critical path: peer setup spans all 91 cases in one container. The
shard measurements above are the relevant local comparison. The previous
Bash runner completed GitHub Actions run
[#51 in 4m 23s](https://github.com/eNeRGy164/wireloom/actions/runs/40441553061);
the C# runner translation still needs a full workflow run to confirm that
timing with the new entrypoint.

The previous GitHub run (40254314909) identifies the shard critical path more
precisely. Its slowest 7.3.1 shard (index 6) took 9m34s end to end, with 7m16s
inside the matrix runner; its slowest 7.7.0 shard (index 6) took 8m58s end to
end, with 6m25s inside the runner. Replaying those same shard assignments with
the initial concurrency-eight setting took 2m12s for 7.3.1 (194 exchanges, 4
known failures) and 2m00s for 7.7.0 (198/198 passed). These local runs confirm
the heavy shard cases no longer have the old serial-build cost.

The scheduler was then raised to 16 exchanges. Under a four-CPU Docker limit,
7.3.1 shard 6 took 2m13s at concurrency eight and 2m03s at concurrency 16,
with the same 194 passes and four known failures. The 7.7.0 shard 6 took 1m56s
at concurrency 16 with all 198 exchanges passing. These local runs exclude
GitHub job startup, image/cache restoration, teardown, and report-job time.

## Runner structure validation (2026-10-27)

The component refactor was checked against the saved application from before
the refactor using the existing RTI 7.7.0 wire container with four CPUs. The
selection was `01-primitives`, `05-array-of-sequences-idl`, `06-aggregates`,
`07-union-wchar-label`, `09-optional-aggregate-member`, and `14-enum-constant`.
All 75 scenario rows were identical: 73 passed, two failed, and none were left
unrun. Both versions reported the same two `direct-recursive-sequence`
failures. Their endpoint logs show successful exits and `PASS` lines; the
existing scheduler prefix check expects a trailing space that the native
recursive peer does not emit. This remains a separate scheduler follow-up.

The RTI 7.3.1 compile-only path compiled all three selected native peers:
`07-union-char-numeric-255`, `09-optional-recursive-array`, and
`14-enum-constant`. Release builds and the license-free harness also passed,
including in-process native build failure handling, report recovery, renderer failure,
stale-artifact isolation, and profile-specific fixture selection. Local
reports and the comparison inputs are under
`artifacts/wire-runner-architecture-7.7.0/`. This focused comparison does not
replace a complete profile matrix run.

The subsequent in-process planning migration was compared with saved outputs
from the original planner for every shard on both profiles: eight shards,
90 cases, and 1,560 exchanges on 7.3.1; eight shards, 91 cases, and 1,555
exchanges on 7.7.0. Plan/shard JSON, scenario rows, and selected case order
matched in all 16 comparisons. The existing shard verifier passed 1,386
checks on 7.3.1 and 1,387 on 7.7.0. Comparison inputs and artifacts are under
`artifacts/wire-planning-migration/`.

The provenance migration preserved all 181 saved evidence documents byte for
byte: 90 eligible cases on 7.3.1 and 91 on 7.7.0. Eight provenance artifacts
from actual Docker builds also matched the original method byte for byte.
The focused selection was `01-primitives`, `05-array-of-sequences-idl`,
`14-autoid-scope`, and `14-enum-constant`, covering canonical, mapping, wrapper,
and transitive-include evidence. All 43 exchanges passed on 7.7.0. On 7.3.1,
38 passed and five failed because the captured enum-constant RTI C# control
contains duplicate declarations. Repeating the run with the original
provenance step produced identical scenario rows. This control-source issue
remains a separate follow-up. Both profile report/shard verifiers passed,
including their missing and tampered provenance checks. Evidence is under
`artifacts/wire-provenance-migration/`.

The pairing migration preserved all 812 ordered queries from the original
commands, covering every positive case, profile-aware fixture, and versionless
query. All 181 eligible case/profile combinations retained the same RTI C#
build requirement when derived from the typed scenario plan. All 16 original
shard plans still matched, and report/shard verification passed 1,386 checks
on 7.3.1 and 1,387 on 7.7.0. The license-free harness also covers directed
overrides, profile exclusions, invalid rules, and peers unused by every
fixture. Comparison inputs and results are under
`artifacts/wire-pairings-migration/`.

The fixture lookup migration preserved all 182 original command queries across
91 positive cases and both profiles, including the order of all 359 returned
fixture names. Release build and license-free harness checks passed, covering
default and named fixtures, RTI-only controls, and sparse typed-default profile
isolation. Comparison inputs and results are under
`artifacts/wire-fixtures-migration/`.

The case-field migration compared all five fields for all 91 positive cases:
455 original command outputs matched both the new typed metadata and the
standalone adapter byte for byte. Release build and license-free harness checks
passed, including defaults, ordered lists, ordinal identity, per-run snapshot
isolation, and native IDL adaptation staging on both profiles. The report/shard
verifiers passed 1,386 checks on 7.3.1 and 1,387 on 7.7.0. Evidence is under
`artifacts/wire-fields-migration/`.

The RTI discovery migration matched 566 queries across 176 selected checked-in
C# captures on both profiles. Queries covered the RTI-only `-` root and every
available topic basename; selected types and standalone JSON matched the
original method byte for byte. Release build and license-free harness checks
passed, including all case preferences, nested sources, duplicate candidates,
ordinal ties, missing support, and handled builder failure without a registry
handoff. The report/shard verifiers passed 1,386 checks on 7.3.1 and 1,387 on
7.7.0. Evidence is under `artifacts/wire-rti-discovery-migration/`.

The oracle-selection migration preserved all 364 managed/native queries across
91 positive cases and both profiles: 355 selected paths and nine unavailable
selections. Standalone output matched the original byte for byte. Release build
and license-free harness checks passed, including explicit-mode suppression,
conventional fallback, per-peer selection, and per-run/profile snapshots.
Metadata staging and RTI discovery checks now run before any helper utility is
built. Both report/shard verifiers passed: 1,386 checks on 7.3.1 and 1,387 on
7.7.0. Evidence is under `artifacts/wire-oracle-selection-migration/`.

The combined discovery/info migration matched 276 discovery queries and 345
case-info lookups against the original commands using both profiles' retained
generator sources and a saved 7.7.0 full-run source tree. All 255 successful
discovery results retained byte-identical registry JSON and info text; the 21
existing rejection cases retained their errors. Release build and license-free
harness checks passed, including shared IDL inputs, namespaces, all required
roots, wrapper aliases, missing support, and registry/info compatibility. Both
report/shard verifiers passed: 1,386 checks on 7.3.1 and 1,387 on 7.7.0.
Evidence is under `artifacts/wire-case-discovery-migration/`.
