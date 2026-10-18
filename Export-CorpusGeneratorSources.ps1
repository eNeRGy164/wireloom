#requires -Version 7.4
<#
.SYNOPSIS
Exports Wireloom-generated C# sources for both supported RTI API profiles.

.DESCRIPTION
Runs the corpus compliance project once for each RTI profile, 7.3.1 and
7.7.0. Generated files are written to the matching profile folder:
docs/corpus/7.3.1/generator and docs/corpus/7.7.0/generator.

The script restores corpus dependencies in locked mode, then tests in Release.
Run it from any working directory with the repository's pinned
.NET SDK available. It stops after the first failed profile and restores the
process environment variables CORPUS_RTI_PROFILE and
CORPUS_GENERATOR_EXPORT_ROOT to their original values when it exits.

Existing generated files with matching paths are overwritten. Files that no
longer correspond to accepted generated sources are not removed automatically;
review the working tree after exporting. Use -OutputRoot with a new directory
for a clean comparison. A new artifact directory retains source/input/output
hashes, SDK details, and test logs; set -EvidenceDirectory to name it explicitly.

.EXAMPLE
PS> .\Export-CorpusGeneratorSources.ps1

Exports both profiles from the repository root. The script also works when
invoked by its full path from another directory.

.NOTES
See docs/corpus/README.md for corpus structure and generated-output evidence.
#>

[CmdletBinding()]
param(
    [ValidateSet('7.3.1', '7.7.0')][string[]]$Profile = @('7.3.1', '7.7.0'),
    [string]$OutputRoot,
    [string]$EvidenceDirectory
)
$ErrorActionPreference = 'Stop'
. "$PSScriptRoot/docs/corpus/scripts/Evidence.Common.ps1"
$project = 'tests/Wireloom.Dds.Generator.CorpusCompliance/Wireloom.Dds.Generator.CorpusCompliance.csproj'
if (!$OutputRoot) { $OutputRoot = 'docs/corpus' }
$output = [IO.Path]::GetFullPath($OutputRoot, $PSScriptRoot)
if (!$EvidenceDirectory) {
    $EvidenceDirectory = "artifacts/corpus-export-$([DateTime]::UtcNow.ToString('yyyyMMddTHHmmssZ'))-$([Guid]::NewGuid().ToString('N').Substring(0,8))"
}
$evidence = [IO.Path]::GetFullPath($EvidenceDirectory, $PSScriptRoot)
if (Test-Path -LiteralPath $evidence) { throw 'EvidenceDirectory must be new to preserve earlier runs.' }
New-Item -ItemType Directory -Force -Path $evidence | Out-Null
$inputs = @('src/Wireloom.Dds.Generator', 'tests/Wireloom.Dds.Generator.CorpusCompliance',
    'docs/corpus/idl', 'docs/corpus/manifest.json', 'docs/corpus/scripts/Evidence.Common.ps1',
    'Export-CorpusGeneratorSources.ps1', 'global.json', 'Directory.Build.props',
    'Directory.Packages.props', 'tests/Directory.Build.props', 'NuGet.config')
$before = Get-EvidenceSnapshot $PSScriptRoot $inputs
$revision = Get-EvidenceRevision $PSScriptRoot
$previousProfile = [Environment]::GetEnvironmentVariable('CORPUS_RTI_PROFILE', 'Process')
$previousExportRoot = [Environment]::GetEnvironmentVariable('CORPUS_GENERATOR_EXPORT_ROOT', 'Process')
Push-Location $PSScriptRoot
try {
    dotnet --info | Out-File "$evidence/dotnet-info.txt" -Encoding utf8
    dotnet restore $project --locked-mode
    if ($LASTEXITCODE -ne 0) { throw 'Corpus locked restore failed.' }
    foreach ($version in $Profile | Select-Object -Unique) {
        $env:CORPUS_RTI_PROFILE = $version
        $env:CORPUS_GENERATOR_EXPORT_ROOT = Join-Path $output "$version/generator"
        $startedAt = [DateTime]::UtcNow.ToString('o')
        Write-Host "Exporting corpus sources for RTI profile $version..."
        dotnet test $project --no-restore --configuration Release | Tee-Object "$evidence/$version-tests.txt"
        if ($LASTEXITCODE -ne 0) { throw "Corpus source export failed for RTI profile $version (exit code $LASTEXITCODE)." }
        $after = Get-EvidenceSnapshot $PSScriptRoot $inputs
        if ($after.sha256 -ne $before.sha256) { throw 'Export inputs changed during execution; do not promote this run.' }
        Write-EvidenceJson "$evidence/$version.json" ([ordered]@{
            schemaVersion = 1; kind = 'wireloom-corpus-export'; profile = $version
            startedAtUtc = $startedAt; completedAtUtc = [DateTime]::UtcNow.ToString('o')
            source = $revision; inputs = $before; configuration = 'Release'
            sdk = (dotnet --version); testExitCode = 0
            outputDirectory = [IO.Path]::GetRelativePath($PSScriptRoot, $env:CORPUS_GENERATOR_EXPORT_ROOT).Replace('\', '/')
            output = Get-EvidenceSnapshot $env:CORPUS_GENERATOR_EXPORT_ROOT @(
                Get-ChildItem -LiteralPath $env:CORPUS_GENERATOR_EXPORT_ROOT -Recurse -File -Filter '*.g.cs' |
                    ForEach-Object { [IO.Path]::GetRelativePath($env:CORPUS_GENERATOR_EXPORT_ROOT, $_.FullName) }
            )
            staleOutputPolicy = 'Existing files are not deleted. Use a new OutputRoot for an uncontaminated export.'
        })
    }
    Write-Host "Export provenance and test logs: $evidence"
}
finally {
    Pop-Location
    [Environment]::SetEnvironmentVariable('CORPUS_RTI_PROFILE', $previousProfile, 'Process')
    [Environment]::SetEnvironmentVariable('CORPUS_GENERATOR_EXPORT_ROOT', $previousExportRoot, 'Process')
}
