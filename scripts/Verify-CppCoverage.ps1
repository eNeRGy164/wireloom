[CmdletBinding()]
param(
    [string] $RepositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path,
    [switch] $PassThru
)

$ErrorActionPreference = 'Stop'

$manifestPath = Join-Path $RepositoryRoot 'docs/corpus/manifest.json'
$featureDirectory = Join-Path $RepositoryRoot 'docs/corpus/7.7.0/generator/cpp/features'
$manifest = [IO.File]::ReadAllText($manifestPath) | ConvertFrom-Json
$manifestIds = @($manifest.positiveCases | ForEach-Object { [string] $_.id })
$duplicates = @($manifestIds | Group-Object -CaseSensitive | Where-Object Count -gt 1 | ForEach-Object Name)
if ($manifestIds.Count -eq 0 -or $duplicates.Count -gt 0 -or
    @($manifestIds | Where-Object { $_ -cnotmatch '^[A-Za-z0-9][A-Za-z0-9-]*$' }).Count -gt 0) {
    throw "The positive corpus must contain nonempty, unique case IDs. Duplicates: $($duplicates -join ', ')"
}

# Keep rejection expectations explicit. A missing export must never silently
# turn an accepted case into a rejected one, and a runtime peer exclusion alone
# does not imply that C++ generation is unsupported.
$rejectedCases = @(
    [pscustomobject]@{
        case = '07-union-wchar-label'
        diagnostic = "The C++ generator does not support 'wchar' union discriminators, matching RTI Connext DDS 7.7.0 rtiddsgen's parser rejection."
    }
)
$rejectedIds = @($rejectedCases | ForEach-Object { $_.case })
foreach ($rejectedCase in $rejectedCases) {
    $case = @($manifest.positiveCases | Where-Object { $_.id -ceq $rejectedCase.case })
    if ($case.Count -ne 1 -or
        [string]::IsNullOrWhiteSpace([string] $case[0].wireMatrix.excludedPeersByRtiVersion.'7.7.0'.'Wireloom C++')) {
        throw "Missing documented Wireloom C++ exclusion for '$($rejectedCase.case)' in the 7.7.0 corpus."
    }
}

foreach ($case in $manifest.positiveCases) {
    $input = Join-Path (Join-Path $RepositoryRoot 'docs/corpus') $case.idl
    if (-not (Test-Path -LiteralPath $input -PathType Leaf)) {
        throw "Missing IDL input for '$($case.id)': $input"
    }
}

$generatedIds = @($manifestIds | Where-Object { $_ -cnotin $rejectedIds })
$featureIds = @(Get-ChildItem -LiteralPath $featureDirectory -Directory | ForEach-Object Name)
$missing = @($generatedIds | Where-Object { $_ -cnotin $featureIds } | Sort-Object)
$unexpected = @($featureIds | Where-Object { $_ -cnotin $generatedIds } | Sort-Object)
if ($missing.Count -gt 0 -or $unexpected.Count -gt 0) {
    throw "C++ generation inventory mismatch. Missing exports: $($missing -join ', '); unexpected exports: $($unexpected -join ', ')"
}

foreach ($caseId in $generatedIds) {
    $expectedFiles = @("$caseId.cxx", "$caseId.hpp", "${caseId}Plugin.cxx", "${caseId}Plugin.hpp")
    $caseDirectory = Join-Path $featureDirectory $caseId
    $actualFiles = @(Get-ChildItem -LiteralPath $caseDirectory -File | ForEach-Object Name)
    if ($actualFiles.Count -ne $expectedFiles.Count -or
        (Compare-Object -ReferenceObject $expectedFiles -DifferenceObject $actualFiles -CaseSensitive)) {
        throw "C++ export '$caseId' must contain exactly its four generated files."
    }
}

# This checks retained generation inventory only. Runtime, CDR, and wire
# qualification belong to the existing native fixtures and release records.
if ($PassThru) {
    [pscustomobject]@{
        ManifestIds = $manifestIds
        Cases = @($manifest.positiveCases)
        GeneratedIds = $generatedIds
        RejectedCases = $rejectedCases
    }
} else {
    Write-Output "C++ generation inventory passed: $($manifestIds.Count) positive cases, $($generatedIds.Count) retained exports, $($rejectedIds.Count) explicit rejection."
}
