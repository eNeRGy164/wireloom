# Wireloom code style

This guide records the readability and design conventions for maintained
Wireloom code. The root [`.editorconfig`](../.editorconfig) contains the
machine-enforceable formatting and analyzer preferences. When an analyzer
suggestion and this guide disagree, this guide describes the intended review
standard; update the configuration when the convention should become
enforceable.

## Readability first

- Prefer clear, vertically readable code over minimizing lines of code.
- Use four-space indentation for C# and keep the repository line-ending rules.
- Use blank lines to separate logical groups and the phases of a method.
- Put control-flow bodies on multiple lines. Always use braces for `if`,
  `else`, `for`, `foreach`, `while`, and similar statements, including bodies
  containing one statement.
- Put a blank line after a completed control-flow block when the following
  statement starts a different logical step. Do not compress unrelated work
  onto one line.
- Keep parameter lists on one line when they remain readable. Wrap them only
  when necessary, and use a stable, easy-to-scan layout when wrapping is
  unavoidable.
- Prefer interpolated strings to concatenation when values are inserted into a
  string.

## Branching and expressions

- Prefer an explicit `if`/`else` block over the conditional operator (`?:`) when
  the branch has meaningful behavior or the expression would be harder to
  scan.
- Keep early returns when they make a guard clause obvious, but still use
  braces and normal vertical spacing.
- Switch statements and switch expressions must give every arm or case its own
  readable line/block. Wrap non-trivial arms instead of placing several cases
  or arms on one line.
- Use expression-bodied members only when the complete expression is clearer
  than a block. Do not use expression syntax merely to shorten a method.
- Prefer current C# constructs when they improve the API or readability. In
  particular, use primary constructors by default for straightforward
  dependency/state wiring, and use modern `params` collection forms such as
  `params IReadOnlyList<T>` when a variable number of values is part of the
  API.

## APIs and documentation

- Every `public` and `internal` class, constructor, method, property, event,
  and other maintained API member has XML documentation. Document the purpose,
  parameters, return value, and relevant exceptions when they are not obvious.
- Use named arguments when a call contains multiple booleans, nullable values,
  or otherwise unclear positional values. This is preferred over comments that
  explain a positional argument at the call site.
- Give parameters and local values names that explain their role. Avoid
  abbreviations unless they are established domain terms.
- Keep public and internal APIs small. If a type is only an implementation
  detail, keep it at the narrowest accessibility that supports its use.

## Namespaces and reuse

- Keep a frequently reused static/helper class in the namespace of the parent
  structure when lower-level components depend on it. Do not create an awkward
  sibling namespace solely to hold a shared type; the namespace should show
  that the type has been lifted into the parent structure.
- Group code by responsibility and architectural boundary, not only by file
  size. A partial class may improve navigation, but it does not remove
  coupling between responsibilities.
- Prefer one shared model, naming plan, mapping service, or policy when several
  emitters make the same decision. Emitters should render decisions rather than
  rediscovering them independently.
- Restructure freely when it makes ownership or reuse clearer. Existing tests,
  corpus cases, generated-shape comparisons, and package integration checks are
  the safety net for behavior-preserving refactors.

## Tests and generated output

- Structure tests with explicit `// Arrange`, `// Act`, and `// Assert`
  sections. Use C# raw strings for multiline IDL.
- Add or update focused tests for each changed branch, diagnostic, generated
  shape, and invalidation path. Preserve the relevant corpus and package
  integration evidence.
- Do not hand-format generated or third-party oracle sources to match this
  guide. Keep authored generator code, tests, fixtures, and documentation
  readable; generated output is validated by its shape and compatibility tests.

## Enforcement and review

The editor configuration currently enforces or guides braces, indentation,
line breaks, file-scoped namespaces, primary constructors, modern C# patterns,
accessibility modifiers, and XML documentation diagnostics. The following
conventions still require code review because they depend on intent:

- blank-line grouping and conservative wrapping;
- choosing `if`/`else` over `?:` for readability;
- named arguments for unclear calls;
- namespace placement and responsibility boundaries;
- extracting shared decisions instead of duplicating them.

For generator changes, run the repository quality gates described in
[`CONTRIBUTING.md`](../CONTRIBUTING.md), including build, tests, coverage,
Community Qodana through `wslc`, and the CI Coveralls check.
