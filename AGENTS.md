# Repository instructions

- Inspect repository conventions before changing files. Keep docs, contracts,
  and evidence aligned with affected behavior. Do not add projects, stacks, or
  dependencies unless requested.
- Use US English and Markdown for docs; use PlantUML `.puml` files under
  `docs/architecture/arc42/images/<chapter>/` for diagrams.
- For .NET, use the SDK pinned in `global.json`, central package management,
  `net10.0`, and root namespace `Wireloom`. Keep package versions in
  `Directory.Packages.props`; the generator is packable and test projects opt
  out. See `.agents/architecture-memory.yaml` for current defaults.
- Before architecture changes, read `.agents/architecture-memory.yaml` and the
  relevant arc42 chapters, decisions, and contracts. Those sources are
  authoritative; refresh derived architecture memory after changes.
- Tests use Microsoft.Testing.Platform. Put runner filters after `--` (for
  example, `-- --filter-method FullyQualifiedName~Namespace.Type.Method`); use
  `--filter-class`, `--filter-namespace`, or `--filter-trait` for wider filters.
  Do not use legacy `--filter`. Structure tests with `// Arrange`, `// Act`,
  and `// Assert`; use C# raw strings for multiline IDL.
- For generator quality changes, run the relevant build and tests, verify
  branch coverage, run Community Qodana, and confirm Coveralls passes after CI.
  See [CONTRIBUTING.md](CONTRIBUTING.md#quality-evidence) for commands and
  report checks. Keep `global.json` visible during Qodana.
- Follow the [PR feedback and quality workflow](.agents/instructions/pr-feedback-and-quality-workflow.md)
  when investigating defects, review feedback, corpus/oracle compatibility, or
  generator-quality changes. Confirm whether feedback applies to the current
  source before changing it, check for equivalent cases, and keep unrelated
  capability work in a tracked issue rather than expanding a focused PR.
- Follow [the repository code style](docs/CODE-STYLE.md) for readability,
  namespaces, API documentation, modern C# usage, and review-only conventions
  that cannot be expressed reliably in `.editorconfig`.
- Follow [the repository Git workflow](.agents/instructions/git-commit-workflow.md)
  for commit subjects and history.
