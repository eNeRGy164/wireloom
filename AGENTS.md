# AGENTS.md

Repo-wide working rules for contributors to this repository.

## Mission

Help contributors make safe, consistent progress in this repository.

## Defaults

- Discover the current repo structure and conventions before making changes.
- Keep docs, contracts, and evidence aligned with affected changes.
- Do not add stacks, projects, or dependencies unless explicitly requested.
- Use Markdown for new documentation and PlantUML with `.puml` source files for diagrams.
- Use the repository-pinned .NET SDK and central package management for .NET projects.
- Keep package versions in `Directory.Packages.props`; the source-generator package is packable while test projects should opt out.

## Guidance

- Project-owned workflow guidance lives under `.agents/instructions/`.

## Git

- Use short, imperative, sentence-case commit subjects that state intent.
- Keep one logical concern per commit; prefer `rebase` and `autosquash` over merge or squash commits.
- Use `--fixup` for corrections to earlier branch commits and do not leave fixup commits in final history.

## Enforcement

Commit policy is documented, but no local hooks or CI enforcement are configured yet.

## .NET defaults

- Target `net10.0` with root namespace `Wireloom`.
- Use `.agents/architecture-memory.yaml` as the machine-readable source for current .NET defaults.

## Architecture guardrails

Before proposing or implementing architecture-affecting changes:

- Read `.agents/architecture-memory.yaml` first when it exists.
- Open the arc42 chapters in `docs/architecture/arc42/`, linked decisions, and contracts when more detail is needed.
- Treat the arc42 chapters and linked source documents as the source of truth; refresh the derived memory after architecture changes.
- If a request conflicts with the architecture documentation, explain the conflict and propose a documentation, ADR, code, or combined change.

## Documentation conventions

- Documentation language: US English
- Documentation format: Markdown
- Preferred diagram tool: PlantUML
- Store PlantUML sources under `docs/architecture/arc42/images/<chapter>/`.
