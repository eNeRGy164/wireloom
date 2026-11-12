[CmdletBinding()]
param(
    [string] $RepositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path,
    [switch] $Check
)

$ErrorActionPreference = 'Stop'

$manifest = [IO.File]::ReadAllText((Join-Path $RepositoryRoot 'docs/corpus/manifest.json')) | ConvertFrom-Json
$registryPath = Join-Path $RepositoryRoot 'docs/corpus/7.7.0/cpp-evidence/type-scope.json'
$registry = [IO.File]::ReadAllText($registryPath) | ConvertFrom-Json
$featureDirectory = Join-Path $RepositoryRoot 'docs/corpus/7.7.0/generator/cpp/features'
$evidenceDirectory = Join-Path $RepositoryRoot 'docs/corpus/7.7.0/cpp-evidence'
$coveragePath = Join-Path $RepositoryRoot 'docs/corpus/CPP-COVERAGE.md'

function Get-TypeDeclarations([string] $caseId) {
    $rejectedCase = @($registry.rtiRejectedCases | Where-Object { $_.case -eq $caseId } | Select-Object -First 1)
    if ($rejectedCase.Count -gt 0) {
        return @($rejectedCase[0].types | ForEach-Object {
            [pscustomobject]@{ Symbol = [string]$_.symbol; Kind = [string]$_.kind }
        })
    }

    $headerPath = Join-Path $featureDirectory "$caseId/$caseId.hpp"
    if (-not (Test-Path -LiteralPath $headerPath)) {
        throw "Missing generated C++ header for '$caseId': $headerPath"
    }

    $headerText = [IO.File]::ReadAllText($headerPath)
    $declarations = [System.Collections.Generic.List[object]]::new()
    $matches = [regex]::Matches($headerText, '(?m)^/// @brief IDL (?<kind>struct|union|enumeration|typedef|valuetype) (?<name>[A-Za-z_][A-Za-z0-9_.:]*)\.?\s*$')
    foreach ($match in $matches) {
        $kind = if ($match.Groups['kind'].Value -eq 'enumeration') { 'enum' } else { $match.Groups['kind'].Value }
        $declarations.Add([pscustomobject]@{
            Symbol = $match.Groups['name'].Value.TrimEnd('.').Replace('.', '::')
            Kind = $kind
        })
    }
    if ($caseId -eq '10-flat-data-binding') {
        $declarations.Add([pscustomobject]@{ Symbol = 'CorpusFlatDataBinding::Sample'; Kind = 'struct' })
    }
    @($declarations | Sort-Object Symbol -Unique)
}

function Get-CaseScope($case) {
    @($registry.cases | Where-Object { $_.case -eq $case }) | Select-Object -First 1
}

function Get-RtiRejectedCase($case) {
    @($registry.rtiRejectedCases | Where-Object { $_.case -eq $case }) | Select-Object -First 1
}

function Get-SymbolArray($case, [string] $property) {
    if ($null -eq $case -or $null -eq $case.PSObject.Properties[$property]) { return @() }
    @($case.$property | ForEach-Object { [string]$_ })
}

function Test-TypeEvidence($case, [string] $symbol) {
    if ($null -eq $case -or $null -eq $case.typeEvidence) { return $false }
    if ($null -ne $case.typeEvidence.PSObject.Properties[$symbol]) { return $true }
    return @($case.typeEvidence.PSObject.Properties | Where-Object {
        (($_.Name -split '::')[-1]) -eq $symbol
    }).Count -gt 0
}

function Get-CdrMembership($case, [string] $symbol, [string] $version) {
    if ($null -eq $case) { return $null }
    if ($case.rowScopedEvidence -and -not (Test-TypeEvidence $case $symbol)) { return $null }
    $notApplicable = Get-SymbolArray $case "notApplicable$version"
    if ($notApplicable -contains $symbol) { return 'Not applicable' }
    $explicit = Get-SymbolArray $case "cdr$version"
    if ($explicit -contains $symbol) { return 'Verified' }
    foreach ($root in $case.cdrRoots.PSObject.Properties) {
        $members = @($root.Value | ForEach-Object { [string]$_ })
        if ($members -contains $symbol) { return 'Verified' }
    }
    $null
}

function Get-ProofScope($case, [string] $symbol) {
    if ($null -eq $case) { return 'No runtime fixture' }
    if ($case.rowScopedEvidence -and -not (Test-TypeEvidence $case $symbol)) { return 'No runtime fixture' }
    $short = ($symbol -split '::')[-1]
    foreach ($root in $case.cdrRoots.PSObject.Properties) {
        $members = @($root.Value | ForEach-Object { [string]$_ })
        if ($members -contains $short) {
            if ($root.Name -eq $short) {
                if ((Get-SymbolArray $case 'registration') -contains $short) { return "Topic root: $short" }
                return "Local CDR root: $short"
            }
            return "Embedded in $($root.Name) graph (serialized, not registered independently)"
        }
    }
    if ((Get-SymbolArray $case 'liveDds') -contains $short) {
        $topicRoot = Get-SymbolArray $case 'registration' | Select-Object -First 1
        if ($topicRoot) { return "Embedded in $topicRoot graph (live DDS)" }
    }
    if ((Get-SymbolArray $case 'registration') -contains $short) { return "Topic root: $short" }
    if ((Get-SymbolArray $case 'dynamicType') -contains $short -or
        (Get-SymbolArray $case 'partialDynamicType') -contains $short) { return 'Metadata-only consumer target' }
    if ($case.compile) { return 'Consumer compile only' }
    'No runtime fixture'
}

function Get-Status($case, [string] $symbol, [string] $capability) {
    if ($null -eq $case) { return 'Not run' }
    if (@($case.notApplicableCapabilities | ForEach-Object { [string]$_ }) -contains $capability) { return 'Not applicable' }
    $short = ($symbol -split '::')[-1]
    $capabilityFailed = @()
    if ($case.failedCapabilities) {
        $failedProperty = $case.failedCapabilities.PSObject.Properties[$capability]
        if ($failedProperty) { $capabilityFailed = @($failedProperty.Value) }
    }
    if ($capabilityFailed -contains $short) { return 'Failed' }
    $capabilityFailClosed = @()
    if ($case.failClosedCapabilities) {
        $capabilityProperty = $case.failClosedCapabilities.PSObject.Properties[$capability]
        if ($capabilityProperty) { $capabilityFailClosed = @($capabilityProperty.Value) }
    }
    if ($capability -in @('DynamicType', 'Registration', 'XCDR1', 'XCDR2', 'Live DDS') -and
        ((Get-SymbolArray $case 'failClosed') -contains $short -or $capabilityFailClosed -contains $short)) {
        return 'Fail-closed'
    }

    if ($capability -eq 'Registration' -and
        (Get-SymbolArray $case 'notApplicableRegistration') -contains $short) { return 'Not applicable' }
    if ($capability -eq 'Live DDS' -and
        (Get-SymbolArray $case 'notApplicableLiveDds') -contains $short) { return 'Not applicable' }

    switch ($capability) {
        'DynamicType' {
            if ((Get-SymbolArray $case 'partialDynamicType') -contains $short) { return 'Partial' }
            if ((Get-SymbolArray $case 'dynamicType') -contains $short) { return 'Verified' }
        }
        'Registration' {
            if ((Get-SymbolArray $case 'registration') -contains $short) { return 'Verified' }
        }
        'XCDR1' {
            $status = Get-CdrMembership $case $short 'Xcdr1'
            if ($status) { return $status }
        }
        'XCDR2' {
            $status = Get-CdrMembership $case $short 'Xcdr2'
            if ($status) { return $status }
        }
        'Live DDS' {
            if ((Get-SymbolArray $case 'liveDds') -contains $short) { return 'Verified' }
        }
    }

    $hasCaseEvidence = $false
    if ($case.report -and (Test-Path (Join-Path $evidenceDirectory $case.report))) { $hasCaseEvidence = $true }
    if ($case.fixture -and (Test-Path (Join-Path $RepositoryRoot $case.fixture))) { $hasCaseEvidence = $true }
    if ($hasCaseEvidence) { 'Not directly tested' } else { 'Not run' }
}

$header = @'
# C++ corpus coverage

This is a type-level evidence matrix for the positive IDL cases and the RTI Connext DDS 7.7.0 C++ profile. Each row is one emitted type or an explicit generator rejection. A generated output or passing case-level test does not automatically qualify every declaration in that case.

## API scope

RTI Connext offers three distinct consumer API families: **C**, **Traditional C++**, and **Modern C++** ([RTI 7.7.0 documentation](https://community.rti.com/documentation/rti-connext-770)). Wireloom's experimental C++ generator currently targets only RTI **Modern C++**. The binding columns below describe what Wireloom-generated code and its consumer fixtures support; `Not targeted` means Wireloom does not generate that binding, not that RTI lacks it. Some Modern C++ runtime bridges use RTI C ABI TypeCode and interpreter structures internally; that implementation detail is not evidence of a generated C consumer binding. The retained RTI corpus C++ output is generated for the Modern C++ API. See the scope in [arc42 chapter 1](../architecture/arc42/01-introduction-and-goals.md) and [ADR-004](../architecture/adr/ADR-004-share-idl-analysis-across-generators.md).

## Status meanings

- **Verified**: a retained report or reproducible fixture explicitly demonstrates this type and capability.
- **Partial**: evidence covers only part of the type's shape or required states.
- **Not directly tested**: related case evidence exists, but it does not name or assert this declaration individually.
- **Failed**: a direct check found an incompatibility or failure for this type and capability.
- **Not run**: no retained result demonstrates the capability.
- **Fail-closed**: the generator deliberately refuses this runtime path; no compatibility is claimed.
- **Not applicable**: the capability does not apply to the type or case.
- **Not targeted**: the binding is outside this generator's supported API surface.

Qualification is scoped to the platform and states named by each linked report. When a case has both Windows and Linux evidence, a `Verified` cell records the passing evidence and readers should inspect both reports for platform-specific failures or limits.

Proof scope names whether evidence tests a topic root, an embedded declaration, metadata only, or a deliberate RTI-matched rejection. Generation is verified separately by the corpus CLI comparison. DynamicType means explicit metadata inspection. Registration / DataWriter is separate from local XCDR serialization. Live DDS refers only to an actual mixed-peer sample exchange; local CDR equality does not qualify transport.

## Type-level evidence matrix
'@

$table = [System.Collections.Generic.List[string]]::new()
$table.Add('| Case | IDL type | IDL kind | Proof scope | Generation | Wireloom C binding | Wireloom Traditional C++ binding | Wireloom Modern C++ binding | DynamicType | Registration / DataWriter | XCDR1 CDR | XCDR2 CDR | Live DDS | Evidence |')
$table.Add('| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |')

foreach ($corpusCase in $manifest.positiveCases) {
    $caseId = [IO.Path]::GetFileNameWithoutExtension([string]$corpusCase.idl)
    $scope = Get-CaseScope $caseId
    $rejected = Get-RtiRejectedCase $caseId
    $types = @(Get-TypeDeclarations $caseId)
    foreach ($type in $types) {
        $symbol = $type.Symbol
        $links = [System.Collections.Generic.List[string]]::new()
        $links.Add("[IDL](idl/features/$caseId.idl)")
        if ($rejected) {
            $links.Add("[rejection report](7.7.0/cpp-evidence/$($rejected.report))")
            if ($rejected.linuxReport) { $links.Add("[$([IO.Path]::GetFileNameWithoutExtension([string]$rejected.linuxReport))](7.7.0/cpp-evidence/$($rejected.linuxReport))") }
            $links.Add("[RTI C++ parser log]($($rejected.rtiLog.Replace('docs/corpus/', '')))" )
        } else {
            $links.Add("[generated header](7.7.0/generator/cpp/features/$caseId/$caseId.hpp)")
        }
        $typeEvidence = $null
        if ($scope -and $scope.typeEvidence) {
            $typeEvidenceProperty = $scope.typeEvidence.PSObject.Properties[$symbol]
            if ($typeEvidenceProperty) { $typeEvidence = $typeEvidenceProperty.Value }
        }
        $evidenceSource = if ($typeEvidence) { $typeEvidence } else { $scope }
        if ($evidenceSource) {
            foreach ($reportProperty in @('report', 'linuxReport')) {
                $report = [string]$evidenceSource.$reportProperty
                if ($report -and (Test-Path -LiteralPath (Join-Path $evidenceDirectory $report))) {
                    $links.Add("[$([IO.Path]::GetFileNameWithoutExtension($report))](7.7.0/cpp-evidence/$report)")
                }
            }
        }
        $fixture = if ($typeEvidence) { [string]$typeEvidence.fixture } elseif ($scope) { [string]$scope.fixture } else { '' }
        if ($fixture -and (Test-Path -LiteralPath (Join-Path $RepositoryRoot $fixture))) {
            $links.Add("[consumer fixture](../../$($fixture.Replace('\', '/')))")
        }
        if ((-not $scope -or ($scope.typeEvidence -and -not $typeEvidence)) -and -not $rejected) {
            $links.Add('[CLI verifier](../../src/Wireloom.Dds.Cpp.Generator/Verify-Cli.ps1)')
        }
        $evidence = ($links | Sort-Object -Unique) -join '<br />'
        $shortSymbol = ($symbol -split '::')[-1]
        $bindingVerified = $scope -and ($scope.compile -or ((Get-SymbolArray $scope 'compiledTypes') -contains $shortSymbol))
        $cells = @(
            $caseId,
            "``$symbol``",
            $type.Kind,
            $(if ($rejected) { 'Rejected by RTI parser' } else { Get-ProofScope $scope $symbol }),
            $(if ($rejected) { 'Fail-closed' } else { 'Verified' }),
            'Not targeted',
            'Not targeted',
            $(if ($rejected) { 'Fail-closed' } elseif ($scope -and ((Get-SymbolArray $scope 'failedModernCppBinding') -contains $shortSymbol)) { 'Failed' } elseif ($bindingVerified) { 'Verified' } else { 'Not run' }),
            (Get-Status $(if ($rejected) { $rejected } else { $scope }) $symbol 'DynamicType'),
            (Get-Status $(if ($rejected) { $rejected } else { $scope }) $symbol 'Registration'),
            (Get-Status $(if ($rejected) { $rejected } else { $scope }) $symbol 'XCDR1'),
            (Get-Status $(if ($rejected) { $rejected } else { $scope }) $symbol 'XCDR2'),
            (Get-Status $(if ($rejected) { $rejected } else { $scope }) $symbol 'Live DDS'),
            $evidence
        )
        $table.Add('| ' + ($cells -join ' | ') + ' |')
    }
}

$footer = @'

The scope registry is [`type-scope.json`](7.7.0/cpp-evidence/type-scope.json). It identifies verified generator cases and RTI-matched C++ parser rejections. Generated-header inventories, rejection records, and registry references are checked by [`scripts/Verify-CppCoverage.ps1`](../scripts/Verify-CppCoverage.ps1). Reproduce generation and rejection behavior with [`Verify-Cli.ps1`](../../src/Wireloom.Dds.Cpp.Generator/Verify-Cli.ps1). Missing runtime evidence is reported as `Not run` or `Not directly tested`, not inferred from a generated directory.
'@
$expectedContent = $header + "`n" + ($table -join "`n") + $footer
if ($Check) {
    $actualContent = [IO.File]::ReadAllText($coveragePath)
    if ($actualContent -cne $expectedContent) {
        throw "Coverage Markdown is out of sync with the manifest, generated headers, or type-scope registry: $coveragePath. Run Update-CppCoverage.ps1."
    }
    Write-Output "C++ coverage Markdown matches the generator, manifest, generated headers, and type-scope registry."
    return
}

[IO.File]::WriteAllText($coveragePath, $expectedContent, [Text.UTF8Encoding]::new($false))
Write-Output "Updated type-level C++ coverage matrix: $($manifest.positiveCases.Count) positive cases, $($table.Count - 2) emitted IDL type rows."
