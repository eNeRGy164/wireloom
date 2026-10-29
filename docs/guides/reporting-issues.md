# Report a Wireloom problem

Report a generation, generated-API, build/IDE, analysis, or compatibility
problem through the [compatibility issue template](https://github.com/eNeRGy164/wireloom/issues/new?template=compatibility.md).
A small reproducer helps distinguish a Wireloom defect, an unsupported shape,
an RTI runtime limitation, and an environment problem.

## Include a reproducible contract

Provide the smallest IDL root and include files that still show the problem.
Keep virtual/relative paths, relevant includes, annotations, symbols, and bounds.
Include the project settings that select generation and its options. Remove
private contract details while checking that the reduced input still reproduces.

Record:

- Wireloom package version or source revision; resolved `Rti.ConnextDds` version.
- OS/architecture, `dotnet --info`, target framework, and C# language version.
- IDE and C# extension versions for interactive problems.
- Exact restore/build/test steps, diagnostics, expected behavior, and actual behavior.
- Analyzer name/version, rule IDs, and generated paths for analysis problems.
- Writer/reader language, exact RTI version, fixture/QoS, and report scope for DDS exchanges.
- Playground revision/profile/browser and operation for browser problems; note whether
  source ZIP or a reduced workspace reproduces it.

State which boundary fails: generation, C# compilation, generated API execution,
native conversion, or a directed DDS exchange. A successful step at one boundary
does not prove the next.

## Keep the evidence useful

Attach relevant sanitized text, not a screenshot of an entire build log.
Do not include license files, credentials, environment dumps, or private IDL.
Preserve failed outcomes and distinguish raw RTI references from adapted peers.
Use the [support policy](support-and-compatibility.md) and
[feature index](../corpus/FEATURE-COVERAGE.md) to identify known limitations.

Security vulnerabilities use the [private reporting policy](../../SECURITY.md),
not the public issue template. Wireloom support remains best effort; filing a
report does not establish a response-time or qualification commitment.

[Documentation](../README.md) · [Developer workflow](developer-workflow.md)
