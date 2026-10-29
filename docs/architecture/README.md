# Architecture

## Purpose

This directory contains the repository's architecture source documents. The
arc42 chapters describe the product goals, shared compiler, .NET build
integration, supporting Playground, evidence, and deployment boundaries.

## Start here

- [arc42 architecture index](arc42/index.md)
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
