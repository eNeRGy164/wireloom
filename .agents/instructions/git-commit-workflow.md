# Git commit workflow

- Inspect the diff before committing and group files by intent.
- Keep one logical concern per commit and use short imperative subjects.
- Use `git commit --fixup <commit>` for corrections to earlier branch commits,
  including review feedback. Do not create generic feedback-only commits such
  as "Process feedback" or "Fix review comments".
- Prefer rebase with autosquash over merge commits; do not create squash commits unless explicitly required.
- Do not leave `fixup!` commits in final history.
- Rewrite and force-push a PR branch only when the user has explicitly
  authorized it. Use `git push --force-with-lease`, never an unconditional
  force push, and verify the rewritten history before pushing.
- No local hooks or CI enforcement are configured yet; review commit subjects and mixed-concern diffs manually.
