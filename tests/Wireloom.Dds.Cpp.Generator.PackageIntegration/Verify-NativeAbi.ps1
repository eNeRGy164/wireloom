[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [ValidateSet('6.1.2', '7.3.1', '7.7.0')]
    [string] $Profile,
    [Parameter(Mandatory)]
    [string] $Source,
    [Parameter(Mandatory)]
    [string] $NddsHome,
    [string] $Compiler = 'g++',
    [string] $OutputRoot = (Join-Path $PSScriptRoot "../../artifacts/cpp-abi-checks/$([DateTime]::UtcNow.ToString('yyyyMMddTHHmmssfffZ'))")
)

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
$sdk = (Resolve-Path -LiteralPath $NddsHome).Path
$sourceFile = (Resolve-Path -LiteralPath $Source).Path
$runRoot = [IO.Path]::GetFullPath($OutputRoot)
New-Item $runRoot -ItemType Directory -Force | Out-Null
$compilerArguments = @(
    '-std=c++11', '-DRTI_UNIX', '-DRTI_LINUX', '-DRTI_64BIT',
    "-I$sdk/include", "-I$sdk/include/ndds", "-I$sdk/include/ndds/hpp",
    '-c', $sourceFile, '-o', (Join-Path $runRoot 'adapter.o')
)
& $Compiler @compilerArguments *> (Join-Path $runRoot 'matching-sdk.log')
if ($LASTEXITCODE) { throw "Matching RTI $Profile SDK did not compile. See $runRoot." }

$checks = [Collections.Generic.List[object]]::new()
$checks.Add(@{ component = 'matching'; accepted = $true })
foreach ($component in @('MAJOR', 'MINOR', 'RELEASE', 'REVISION')) {
    $override = Join-Path $runRoot "wrong-$component.h"
    @"
#include <ndds/ndds_version.h>
#undef RTI_DDS_VERSION_$component
#define RTI_DDS_VERSION_$component 99
"@ | Set-Content -LiteralPath $override
    $wrongArguments = @('-include', $override) + $compilerArguments
    $log = Join-Path $runRoot "wrong-$component.log"
    & $Compiler @wrongArguments *> $log
    $rejected = $LASTEXITCODE -ne 0 -and
        (Get-Content -LiteralPath $log -Raw).Contains("requires RTI Connext DDS $Profile.")
    $checks.Add(@{ component = $component; rejected = $rejected })
    if (-not $rejected) { throw "RTI $Profile $component mismatch was not rejected by its exact SDK guard." }
}

[ordered]@{
    profile = $Profile
    source = $Source
    compiler = (& $Compiler -dumpfullversion -dumpversion)
    checks = @($checks)
} | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $runRoot 'result.json')
Write-Host "RTI $Profile ABI check passed: matching SDK compiled; major/minor/release/revision mismatches rejected."
