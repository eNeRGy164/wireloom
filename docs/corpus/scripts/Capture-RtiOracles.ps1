#requires -Version 7.4
<#
.SYNOPSIS
Captures exact-LTS RTI C# and C++ oracles with source and toolchain identity.
.DESCRIPTION
Runs one disposable pinned RTI container. Outputs go to a new directory under
artifacts, never directly over retained oracles. Requires PowerShell 7, Git,
and Docker with access to the pinned images. Preprocessing captures require
cpp in the image or package access.
.EXAMPLE
./docs/corpus/scripts/Capture-RtiOracles.ps1 -Version 7.3.1 -CaseId 09-autoid-hash,09-must-understand
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)][ValidateSet('7.3.1', '7.7.0')][string]$Version,
    [string[]]$CaseId = @(),
    [ValidateSet('csharp', 'cpp')][string[]]$Language = @('csharp', 'cpp'),
    [string]$OutputDirectory,
    [switch]$DisablePreprocessor,
    [switch]$WireWrapper
)
$ErrorActionPreference = 'Stop'
. "$PSScriptRoot/Evidence.Common.ps1"
$repo = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../../..'))
$images = @{
    '7.3.1' = 'rticom/connext-base:7.3.1@sha256:f33a61ec6381a8fb73cbb019fbb5c081f434dc5217eb14283fad995306be0f04'
    '7.7.0' = 'rticom/connext-base:7.7.0@sha256:efe4d35807a4aadaa59f0e84545d4c6b70328af5b0c0ca0498e63ecc1e16e1ca'
}
$generator = if ($Version -eq '7.3.1') { '4.3.1' } else { '4.7.0' }
$manifest = Get-Content -Raw -LiteralPath "$repo/docs/corpus/manifest.json" | ConvertFrom-Json
$cases = @($manifest.positiveCases) + @($manifest.negativeCases) + @($manifest.integrationEntryPoints | ForEach-Object {
    [pscustomobject]@{ id = [IO.Path]::GetFileNameWithoutExtension($_); idl = $_; defines = @() }
})
foreach ($id in $CaseId) {
    if ($id -notin $cases.id) { throw "Unknown corpus case: $id" }
}
if ($CaseId.Count -gt 0) { $cases = @($cases | Where-Object id -In $CaseId) }
if (!$OutputDirectory) {
    $OutputDirectory = "artifacts/rti-capture-$Version-$([DateTime]::UtcNow.ToString('yyyyMMddTHHmmssZ'))-$([Guid]::NewGuid().ToString('N').Substring(0,8))"
}
$output = [IO.Path]::GetFullPath($OutputDirectory, $repo)
$relative = [IO.Path]::GetRelativePath($repo, $output).Replace('\', '/')
if (!$relative.StartsWith('artifacts/', [StringComparison]::Ordinal) -or (Test-Path -LiteralPath $output)) {
    throw 'OutputDirectory must be a new directory under repository artifacts/.'
}
New-Item -ItemType Directory -Path "$output/.capture/jobs" -Force | Out-Null
$identityPaths = @('docs/corpus/idl', 'docs/corpus/manifest.json', 'docs/corpus/scripts')
if ($WireWrapper) { $identityPaths += 'tests/Wireloom.Dds.Generator.WireCompatibility/IdlWrappers' }
$identity = Get-EvidenceSnapshot $repo $identityPaths
$revision = Get-EvidenceRevision $repo
$jobs = @()
$utf8 = [Text.UTF8Encoding]::new($false)
foreach ($case in $cases) {
    foreach ($lang in $Language | Select-Object -Unique) {
        $jobName = "$($case.id)-$lang"
        $job = "$output/.capture/jobs/$jobName"
        New-Item -ItemType Directory -Path "$job/generated" -Force | Out-Null
        $source = "$repo/docs/corpus/$($case.idl)"
        $input = "/work/docs/corpus/$($case.idl)"
        if ($WireWrapper) {
            $wrapper = "$repo/tests/Wireloom.Dds.Generator.WireCompatibility/IdlWrappers/$($case.id).idl"
            if (!(Test-Path -LiteralPath $wrapper)) { throw "No wire wrapper exists for $($case.id)." }
            $source = "$job/idl.idl"
            Copy-Item -LiteralPath $wrapper -Destination $source
            $input = "/work/$relative/.capture/jobs/$jobName/idl.idl"
        }
        $adaptations = @()
        if ($lang -eq 'cpp' -and $case.id -eq '10-flat-data-binding') {
            $text = [IO.File]::ReadAllText($source) -replace '(?m)^.*@language_binding\(FLAT_DATA\).*\r?\n', ''
            [IO.File]::WriteAllText("$job/$([IO.Path]::GetFileName($source))", $text, $utf8)
            $source = "$job/$([IO.Path]::GetFileName($source))"
            $input = "/work/$relative/.capture/jobs/$jobName/$([IO.Path]::GetFileName($source))"
            $adaptations = @('Remove @language_binding(FLAT_DATA) for a plain C++ oracle; the canonical extensibility is incompatible with FlatData.')
        }
        $options = @('-language', $(if ($lang -eq 'cpp') { 'C++11' } else { 'C#' }), '-replace')
        if ($DisablePreprocessor) { $options += '-ppDisable' }
        if ($case.strict) { $options += '-strict' }
        foreach ($option in $case.rtiddsgenOptions) { $options += $option }
        if ($Version -eq '7.7.0') { $options += '-generateIncludeFiles' }
        if ($lang -eq 'cpp') {
            foreach ($option in $case.wireMatrix.rtiCppCodegenOptions) {
                if ($option -notin $options) { $options += $option }
            }
        }
        foreach ($define in $case.defines) { $options += "-D$define" }
        foreach ($include in $manifest.includeDirectories) { $options += @('-I', "/work/docs/corpus/$include") }
        $arguments = $options + @('-d', "/work/$relative/.capture/jobs/$jobName/generated", $input)
        if (@($arguments | Where-Object { $_ -match '[\r\n]' }).Count -gt 0) { throw 'Codegen arguments must be single-line values.' }
        [IO.File]::WriteAllText("$job/arguments.txt", ($arguments -join "`n") + "`n", $utf8)
        $destination = if ($WireWrapper) { "$lang/wire-wrapped/$($case.id)" } else { "$lang/$($case.idl -replace '^idl/', '' -replace '\.idl$', '')" }
        [IO.File]::WriteAllText("$job/destination.txt", $destination + "`n", $utf8)
        $jobs += [ordered]@{
            id = $case.id; language = $lang; idl = $case.idl
            sourceSha256 = (Get-FileHash -LiteralPath "$repo/docs/corpus/$($case.idl)" -Algorithm SHA256).Hash.ToLowerInvariant()
            generatedInputSha256 = (Get-FileHash -LiteralPath $source -Algorithm SHA256).Hash.ToLowerInvariant()
            options = $options; arguments = $arguments; adaptations = $adaptations
            outputDirectory = $destination; job = $jobName
        }
    }
}
$request = [ordered]@{
    schemaVersion = 1; kind = 'rti-oracle-capture'; rtiVersion = $Version
    rtiddsgenVersion = $generator; image = $images[$Version]
    startedAtUtc = [DateTime]::UtcNow.ToString('o'); source = $revision; inputs = $identity
    preprocessorDisabled = [bool]$DisablePreprocessor; wireWrapper = [bool]$WireWrapper; cases = $jobs
}
Write-EvidenceJson "$output/capture-request.json" $request
$preprocess = if ($DisablePreprocessor) { 'disabled' } else { 'enabled' }
& docker run --rm --user 0:0 --volume "${repo}:/work" --entrypoint bash $images[$Version] `
    /work/docs/corpus/scripts/capture-rti-oracles.sh "/work/$relative" $Version $generator $preprocess
if ($LASTEXITCODE -ne 0) { throw "Capture setup/toolchain failed; partial evidence remains in $output" }
foreach ($case in $jobs) {
    $case.exitCode = [int][IO.File]::ReadAllText("$output/.capture/jobs/$($case.job)/exit-code.txt")
    $case.outcome = if ($case.exitCode -eq 0) { 'generated' } else { 'rejected-or-failed' }
    $case.log = "$($case.outputDirectory)/rtiddsgen.log"
    $case.output = Get-EvidenceSnapshot $output @($case.outputDirectory)
    $case.Remove('job')
}
$after = Get-EvidenceSnapshot $repo $identityPaths
if ($after.sha256 -ne $identity.sha256) { throw 'Capture inputs changed during execution; do not promote this run.' }
$request.completedAtUtc = [DateTime]::UtcNow.ToString('o')
$request.toolchain = Get-EvidenceSnapshot $output @('toolchain.txt')
$request.generatedCaseCount = @($jobs | Where-Object outcome -EQ generated).Count
$request.failedCaseCount = $jobs.Count - $request.generatedCaseCount
Write-EvidenceJson "$output/capture.json" $request
Write-Host "Capture complete: $output/capture.json ($($request.generatedCaseCount) generated; $($request.failedCaseCount) rejected/failed)."
