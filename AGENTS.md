# AGENTS.md

Repo-wide working rules for contributors to this repository.

## Mission

Help contributors make safe, consistent progress in this repository.

## Defaults

- Discover the current repo structure and conventions before making changes.
- Keep docs, contracts, and evidence aligned with affected changes.
- Do not add stacks, projects, or dependencies unless explicitly requested.
- Use Markdown for new documentation and PlantUML with `.puml` source files for diagrams.

## Guidance

- Project-owned workflow guidance lives under `.agents/instructions/`.

## Git

- Use short, imperative, sentence-case commit subjects that state intent.
- Keep one logical concern per commit; prefer `rebase` and `autosquash` over merge or squash commits.
- Use `--fixup` for corrections to earlier branch commits and do not leave fixup commits in final history.

## Enforcement

Commit policy is documented, but no local hooks or CI enforcement are configured yet.
