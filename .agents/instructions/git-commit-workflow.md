# Git commit workflow

- Inspect the diff before committing and group files by intent.
- Keep one logical concern per commit and use short imperative subjects.
- Use `git commit --fixup <commit>` for corrections to earlier branch commits.
- Prefer rebase with autosquash over merge commits; do not create squash commits unless explicitly required.
- Do not leave `fixup!` commits in final history.
- No local hooks or CI enforcement are configured yet; review commit subjects and mixed-concern diffs manually.
