# PR feedback and quality workflow

Use this workflow for defects, GitHub review feedback, generator-quality work,
corpus findings, and RTI-oracle discrepancies. It keeps a focused pull request
from becoming a sequence of isolated reviewer-driven patches.

## Investigate before changing code

1. Inspect the current PR feedback and the current branch. A comment may refer
   to an outdated diff or already be addressed by a later commit.
2. Reproduce the behavior with a focused test, a corpus case, or a package
   integration scenario before treating a finding as a defect. Read the code
   around the reported line and trace the architectural boundary; do not apply
   suggestions mechanically.
3. Search the affected subsystem for equivalent paths, aliases, generated
   forms, and platform-specific variants. Add regression coverage for the
   underlying rule, not merely the reported spelling.
4. For IDL compatibility questions, use the corpus and RTI oracle deliberately:
   - If RTI supports the case and the behavior fits Wireloom's supported scope,
     implement and test it.
   - If RTI does not support it, establish whether the limitation belongs to
     OMG IDL, RTI, or only RTI's generator. Wireloom should be equal or better
     where that produces a coherent supported behavior.
   - If support is unknown, add a focused oracle probe before drawing a
     conclusion.
   - Leave unrelated newly discovered cases unsupported for the next feature;
     record an issue instead of silently broadening the PR.

## Keep ownership and scope clear

- Prefer a shared semantic model, naming plan, mapping service, or validation
  policy when several emitters make the same decision. Renderers should consume
  those decisions rather than rediscovering them.
- Preserve the parse, bind, validate, and emit boundaries. Invalid semantics
  must be diagnosed before emission so diagnostics retain useful source
  locations and invalid generated code is not produced.
- Classify each accepted finding as either in scope for the current PR or a
  separately tracked issue. Explain intentional open limitations in the PR
  description and feature-coverage evidence.
- Do not resolve or reply to GitHub review conversations without explicit user
  authorization. A resolved conversation means the current behavior has been
  verified, not only that code has changed nearby.

## Implement readably

Follow [the code style guide](../../docs/CODE-STYLE.md). In particular, retain
clear responsibility boundaries, use blank lines to show logical phases, use
braces for control flow, favor readable `if`/`else` blocks over dense
expressions, and use named arguments when positional values are unclear.
Public and internal APIs need XML documentation. Refactor freely when it
clarifies ownership or removes duplicated decisions, but preserve behavior with
tests and corpus evidence.

## Verify the complete change

Run the smallest focused test while iterating, then run the relevant complete
quality evidence before declaring the work ready:

1. Restore in locked mode, then build the affected projects in Release.
2. Run unit tests and the applicable corpus-compliance tests.
3. Run fresh package integration when package assets, MSBuild targets,
   additional-file handling, or generated consumer code may be affected.
4. Run coverage and inspect branch coverage first, then line coverage. Add
   tests for newly introduced branches and error paths rather than accepting
   avoidable coverage regressions.
5. Run Community Qodana using the documented `wslc` command in
   [`CONTRIBUTING.md`](../../CONTRIBUTING.md#quality-evidence), keep
   `global.json` visible, and confirm the SARIF result is successful with no
   new problems.
6. After pushing, confirm that CI and Coveralls pass. Local green tests do not
   substitute for the CI coverage upload and package/platform checks.

Report the actual commands, pass counts, coverage counters, and any intentional
skips or limitations. Do not claim a quality gate passed when it was not run.

## Preserve a reviewable history

- Put new behavior in logical, independently reviewable commits.
- Make corrections to an existing branch concern with `git commit --fixup` and
  autosquash them into that concern. Do not create a separate generic
  feedback-processing commit.
- Inspect the final diff, commit history, and `git diff --check` after an
  autosquash. Do not leave `fixup!` commits in the PR.
- Only rewrite and force-push after the user has authorized it; use
  `--force-with-lease`.
