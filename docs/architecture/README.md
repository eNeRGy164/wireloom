# Architecture

## Purpose

This directory contains the repository's architecture source documents. The
arc42 chapters describe the product goals, shared compiler, .NET build
integration, supporting Playground, evidence, and deployment boundaries.

## Start here

- [arc42 architecture index](arc42/index.md)
- [accepted decisions](#decisions)
- [dependency reviews](#dependency-reviews)
- [agent architecture memory](../../.agents/architecture-memory.yaml) (YAML only)

## What belongs here

- Architecture descriptions and PlantUML source files.
- Decisions, constraints, quality scenarios, and documented risks.

## What does not belong here

- Implementation code.
- Generated build output or undocumented compatibility claims.

## Naming conventions

Use descriptive Markdown and `.puml` filenames. Keep arc42 chapter numbers in
Markdown headings. Keep diagram sources under `arc42/images/<chapter>/`,
with rendered images beside their `.puml` sources.

Use the shared [Wireloom diagram theme](arc42/images/wireloom-theme.puml)
with the default top-to-bottom layout. Keep view-specific legends in their
chapter directory; chapter 5 uses a separate
[building-block legend](arc42/images/05/legend.puml).

## Decisions

These accepted records preserve the context and alternatives at their decision
dates. Current behavior belongs in the linked chapters and guides.

- [ADR-001: Shared compiler](adr/ADR-001-share-a-host-neutral-compiler-project.md)
- [ADR-002: Per-compilation MD5 provider](adr/ADR-002-inject-member-id-md5-per-compilation.md)
- [ADR-003: Browser compilation worker](adr/ADR-003-run-playground-compilation-in-a-browser-worker.md)
- [Decision timeline](arc42/09-architectural-decisions.md)

## Dependency reviews

- [Playground frontend](dependency-intake/playground-frontend.md)
- [Pages publishing actions](dependency-intake/playground-pages-actions.md)

The [hosting background](arc42/09-architectural-decisions.md#923-playground-hosting-and-artifact-promotion) retains the
comparison behind the Pages choice. Use the [deployment runbook](../guides/playground-deployment.md)
for operations. Dated dependency pins and measurements are review snapshots,
not a substitute for the current lockfiles and qualification.

[Documentation by task](../README.md)
