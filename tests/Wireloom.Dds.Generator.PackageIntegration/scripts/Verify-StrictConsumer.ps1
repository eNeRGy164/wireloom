#requires -Version 7.4
<#
.SYNOPSIS
Checks that a restored packed-package consumer rejects naming violations in strict mode.
.DESCRIPTION
Run after the ordinary package integration restore/test with matching package,
RTI, and framework values. The expected negative build must contain DDSG0001;
an unrelated build failure alone is not a successful check.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$PackageVersion,
    [ValidateSet('7.3.1', '7.7.0')][string]$RtiVersion = '7.7.0',
    [ValidateSet('net8.0', 'net10.0')][string]$Framework = 'net10.0',
    [string]$LogPath
)
$ErrorActionPreference = 'Stop'
$project = Join-Path $PSScriptRoot '../Wireloom.Dds.Generator.PackageIntegration.csproj'
$PSNativeCommandUseErrorActionPreference = $false
$output = & dotnet build $project --no-restore --configuration Release `
    "-p:WireloomDdsGeneratorPackageVersion=$PackageVersion" `
    "-p:RtiConnextDdsVersion=$RtiVersion" `
    "-p:WireloomConsumerTargetFramework=$Framework" `
    -p:LangVersion=12.0 -p:PackageIntegrationStrict=true 2>&1
$exitCode = $LASTEXITCODE
$text = $output -join "`n"
if ($LogPath) {
    $output | Out-File -LiteralPath $LogPath -Encoding utf8
}
if ($exitCode -eq 0 -or $text -notmatch 'error DDSG0001:.*ignoring case') {
    $output | Write-Host
    throw 'The strict consumer build did not reject an IDL naming violation as expected.'
}
Write-Host "Strict packed consumer rejected naming violations: RTI $RtiVersion, $Framework."
exit 0
