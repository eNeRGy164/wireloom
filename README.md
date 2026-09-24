# Wireloom

This repository is being prepared as a starting point for future development.

## Prerequisites

.NET 10.0.401 SDK or newer.

## Project layout

```text
.
├── .agents/
│   └── instructions/
├── docs/
│   ├── architecture/
│   ├── requirements/
│   ├── security/
│   └── testing/
├── evidence/
├── src/
└── tests/
```

- `.agents/` contains project-owned contributor workflow guidance.
- `docs/` contains architecture, requirements, security, and testing documentation.
- `evidence/` contains supporting evidence and records.
- `src/` is reserved for production source code.
- `tests/` is reserved for automated tests.

## Package publishing

GitHub Actions publishes preview packages from `main` to GitHub Packages. Release
packages are published to NuGet.org only for `v*.*.*` tags after the test suite,
SBOM generation, provenance attestation, and release-tag validation succeed.
