# C++ Native AOT toolchain review

Reviewed November 5, 2026 for the Modern C++ generator's native package.

The producer uses the existing approved Microsoft SDK/NuGet sources and the
Microsoft Container Registry. No application dependency or package feed is
added. Linux uses the official SDK AOT image
`mcr.microsoft.com/dotnet/sdk:10.0.400-noble-aot@sha256:54ad66409fb7f9bfaf1b11355e846b46cd94803cfc813e46a7af5aa2c394ea33`
(Linux amd64 manifest). This supplies the compiler/linker prerequisites that
the ordinary SDK image lacks. Windows uses the repository SDK selection and
the Visual Studio native toolchain on the explicit runner.

The AOT runtime/compiler version is pinned centrally to 10.0.14. An initial
Linux restore correctly rejected 10.0.11 through NuGet audit; 10.0.14 is the
patched version in the [Microsoft security advisory](https://github.com/dotnet/runtime/security/advisories/GHSA-p3jp-9v83-76w9).
Keep audit and package signature validation enabled. RID-specific lock files
retain resolved producer dependencies. The published executable embeds runtime
code, so runtime security fixes require new package bytes.

This toolchain executes repository source during the build and embeds native
runtime dependencies in the output. CI runs with read-only repository access,
no publishing credentials, no license, explicit timeouts, and pinned actions.
Candidate restore accepts an unsigned package only from its isolated local
feed, matching the existing analyzer package smoke-test convention.

The pipeline retains executable hashes and publish warnings and tests the
combined NuGet payload on both hosts. Review image/runtime pins when changing
the SDK and for security updates. Re-run native package checks and the licensed
consumer/wire checks before promoting a changed candidate. Framework-dependent
and managed self-contained alternatives were evaluated before selecting
Native AOT.
