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
  relevant arc42 chapters, decisions, and contracts. The chapters, decisions,
  and contracts are authoritative; update them first, then refresh the YAML.
- Keep derived architecture memory **YAML-only** in
  `.agents/architecture-memory.yaml`. Do not create or maintain a Markdown
  memory file, even when a skill or hook requests one by default. This
  repository override applies to creation, refresh, and validation. A missing
  Markdown warning from the shared validator does not override this policy;
  validate the YAML's structure, source paths, and freshness directly.
  After that validation passes, clear `.agents/architecture-memory.dirty` to
  acknowledge the completed refresh without recreating Markdown.
- Keep the YAML compact: retain stable boundaries, behavioral invariants,
  defaults with provenance, material gaps, and source links. Retrieve details
  from the sources instead of copying UI catalogs, measurements, decision
  chronology, or repeated chapter summaries into memory.
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
