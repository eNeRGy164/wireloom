<#
.SYNOPSIS
Exports Wireloom-generated C# sources for both supported RTI API profiles.

.DESCRIPTION
Runs the corpus compliance project once for each RTI profile, 7.3.1 and
7.7.0. Generated files are written to the matching profile folder:
docs/corpus/7.3.1/generator and docs/corpus/7.7.0/generator.

The script uses the repository's already-restored NuGet assets by passing
--no-restore. Run it from any working directory with the repository's pinned
.NET SDK available. It stops after the first failed profile and restores the
process environment variables CORPUS_RTI_PROFILE and
CORPUS_GENERATOR_EXPORT_ROOT to their original values when it exits.

Existing generated files with matching paths are overwritten. Files that no
longer correspond to accepted generated sources are not removed automatically;
review the working tree after exporting.

.EXAMPLE
PS> .\Export-CorpusGeneratorSources.ps1

Exports both profiles from the repository root. The script also works when
invoked by its full path from another directory.

.NOTES
See docs/corpus/README.md for corpus structure and generated-output evidence.
#>

$ErrorActionPreference = 'Stop'

$project = 'tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj'
$previousProfile = [Environment]::GetEnvironmentVariable('CORPUS_RTI_PROFILE', 'Process')
$previousExportRoot = [Environment]::GetEnvironmentVariable('CORPUS_GENERATOR_EXPORT_ROOT', 'Process')

Push-Location $PSScriptRoot
try {
    foreach ($version in @('7.3.1', '7.7.0')) {
        $env:CORPUS_RTI_PROFILE = $version
        $env:CORPUS_GENERATOR_EXPORT_ROOT = Join-Path $PSScriptRoot "docs/corpus/$version/generator"

        Write-Host "Exporting corpus sources for RTI profile $version..."
        dotnet test $project --no-restore
        if ($LASTEXITCODE -ne 0) {
            throw "Corpus source export failed for RTI profile $version (exit code $LASTEXITCODE)."
        }
    }
}
finally {
    Pop-Location

    if ($null -eq $previousProfile) {
        Remove-Item Env:CORPUS_RTI_PROFILE -ErrorAction SilentlyContinue
    }
    else {
        $env:CORPUS_RTI_PROFILE = $previousProfile
    }

    if ($null -eq $previousExportRoot) {
        Remove-Item Env:CORPUS_GENERATOR_EXPORT_ROOT -ErrorAction SilentlyContinue
    }
    else {
        $env:CORPUS_GENERATOR_EXPORT_ROOT = $previousExportRoot
    }
}
