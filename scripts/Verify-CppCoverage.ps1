[CmdletBinding()]
param(
    [string] $RepositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
)

$ErrorActionPreference = 'Stop'

$manifestPath = Join-Path $RepositoryRoot 'docs/corpus/manifest.json'
$coveragePath = Join-Path $RepositoryRoot 'docs/corpus/CPP-COVERAGE.md'
$featureDirectory = Join-Path $RepositoryRoot 'docs/corpus/7.7.0/generator/cpp/features'
$evidenceDirectory = Join-Path $RepositoryRoot 'docs/corpus/7.7.0/cpp-evidence'
$registryPath = Join-Path $evidenceDirectory 'type-scope.json'
$updaterPath = Join-Path $RepositoryRoot 'scripts/Update-CppCoverage.ps1'

$manifest = [IO.File]::ReadAllText($manifestPath) | ConvertFrom-Json
$expectedCases = @{}
foreach ($case in $manifest.positiveCases) {
    $caseId = [IO.Path]::GetFileNameWithoutExtension([string]$case.idl)
    $expectedCases[$caseId] = $case
}
function Assert-NoDuplicateJsonProperties([System.Text.Json.JsonElement] $element, [string] $path) {
    if ($element.ValueKind -eq [System.Text.Json.JsonValueKind]::Object) {
        $names = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
        foreach ($property in $element.EnumerateObject()) {
            if (-not $names.Add($property.Name)) {
                throw "JSON object '$path' contains duplicate property '$($property.Name)'."
            }
            Assert-NoDuplicateJsonProperties $property.Value "$path.$($property.Name)"
        }
    } elseif ($element.ValueKind -eq [System.Text.Json.JsonValueKind]::Array) {
        $index = 0
        foreach ($item in $element.EnumerateArray()) {
            Assert-NoDuplicateJsonProperties $item "$path[$index]"
            $index++
        }
    }
}

$registryJson = [System.Text.Json.JsonDocument]::Parse([IO.File]::ReadAllText($registryPath))
try {
    Assert-NoDuplicateJsonProperties $registryJson.RootElement 'type-scope.json'
} finally {
    $registryJson.Dispose()
}
$registry = [IO.File]::ReadAllText($registryPath) | ConvertFrom-Json
if ($registry.wireloomBindings.c -cne 'Not targeted' -or
    $registry.wireloomBindings.traditionalCpp -cne 'Not targeted' -or
    $registry.wireloomBindings.modernCpp -cne 'targeted') {
    throw 'The scope registry API targets do not match the documented Modern C++ scope.'
}

# Regenerate the matrix in memory and compare it with the retained Markdown so
# proof scopes and capability statuses cannot drift from the evidence registry.
& $updaterPath -RepositoryRoot $RepositoryRoot -Check
$registryCases = @{}
foreach ($scope in $registry.cases) {
    if (-not $expectedCases.ContainsKey([string]$scope.case)) { throw "Scope registry has unknown case '$($scope.case)'." }
    if ($registryCases.ContainsKey([string]$scope.case)) { throw "Scope registry duplicates case '$($scope.case)'." }
    $registryCases[[string]$scope.case] = $scope
    if ($scope.report -and -not (Test-Path -LiteralPath (Join-Path $evidenceDirectory $scope.report))) { throw "Missing scope report '$($scope.report)'." }
    if ($scope.linuxReport -and -not (Test-Path -LiteralPath (Join-Path $evidenceDirectory $scope.linuxReport))) { throw "Missing Linux scope report '$($scope.linuxReport)'." }
    if ($scope.fixture -and -not (Test-Path -LiteralPath (Join-Path $RepositoryRoot $scope.fixture))) { throw "Missing scope fixture '$($scope.fixture)'." }
    if ($scope.typeEvidence) {
        foreach ($typeEvidence in $scope.typeEvidence.PSObject.Properties) {
            if ($typeEvidence.Value.report -and -not (Test-Path -LiteralPath (Join-Path $evidenceDirectory $typeEvidence.Value.report))) { throw "Missing type report '$($typeEvidence.Value.report)' for '$($typeEvidence.Name)'." }
            if ($typeEvidence.Value.linuxReport -and -not (Test-Path -LiteralPath (Join-Path $evidenceDirectory $typeEvidence.Value.linuxReport))) { throw "Missing type Linux report '$($typeEvidence.Value.linuxReport)' for '$($typeEvidence.Name)'." }
            if ($typeEvidence.Value.fixture -and -not (Test-Path -LiteralPath (Join-Path $RepositoryRoot $typeEvidence.Value.fixture))) { throw "Missing type fixture '$($typeEvidence.Value.fixture)' for '$($typeEvidence.Name)'." }
        }
    }
}
$rejectedCases = @{}
foreach ($rejected in $registry.rtiRejectedCases) {
    $caseId = [string]$rejected.case
    if (-not $expectedCases.ContainsKey($caseId)) { throw "RTI-rejected registry has unknown positive case '$caseId'." }
    if ($rejectedCases.ContainsKey($caseId) -or $registryCases.ContainsKey($caseId)) { throw "Case '$caseId' is duplicated or both supported and rejected in type-scope.json." }
    $rejectedCases[$caseId] = $rejected
    if (-not (Test-Path -LiteralPath (Join-Path $evidenceDirectory $rejected.report))) { throw "Missing rejection report '$($rejected.report)' for '$caseId'." }
    if ($rejected.linuxReport -and -not (Test-Path -LiteralPath (Join-Path $evidenceDirectory $rejected.linuxReport))) { throw "Missing Linux rejection report '$($rejected.linuxReport)' for '$caseId'." }
    if (-not (Test-Path -LiteralPath (Join-Path $RepositoryRoot $rejected.rtiLog))) { throw "Missing RTI rejection log '$($rejected.rtiLog)' for '$caseId'." }
    if (@($rejected.types).Count -eq 0) { throw "RTI-rejected case '$caseId' must name its IDL declarations." }
    $retainedHeader = Join-Path $featureDirectory "$caseId/$caseId.hpp"
    if (Test-Path -LiteralPath $retainedHeader) { throw "RTI-rejected case '$caseId' unexpectedly retains generated output: $retainedHeader" }
}

$coverageText = [IO.File]::ReadAllText($coveragePath)
$tableMatch = [regex]::Match(
    $coverageText,
    '(?ms)^## Type-level evidence matrix\s*\r?\n(?<table>(?:^\|.*\r?\n)+)')
if (-not $tableMatch.Success) {
    throw "Could not find the type-level evidence matrix in $coveragePath."
}

$expectedHeader = @(
    'Case',
    'IDL type',
    'IDL kind',
    'Proof scope',
    'Generation',
    'Wireloom C binding',
    'Wireloom Traditional C++ binding',
    'Wireloom Modern C++ binding',
    'DynamicType',
    'Registration / DataWriter',
    'XCDR1 CDR',
    'XCDR2 CDR',
    'Live DDS',
    'Evidence'
)

function Get-Cells([string] $line) {
    $trimmed = $line.Trim()
    if (-not $trimmed.StartsWith('|') -or -not $trimmed.EndsWith('|')) {
        throw "Malformed matrix row: $line"
    }

    @($trimmed.Trim('|').Split('|') | ForEach-Object { $_.Trim() })
}

$tableLines = @($tableMatch.Groups['table'].Value -split '\r?\n' | Where-Object { $_ })
$headerCells = @(Get-Cells $tableLines[0])
if (($headerCells -join "`n") -cne ($expectedHeader -join "`n")) {
    throw "Unexpected matrix columns. Expected: $($expectedHeader -join ' | ')"
}

$statusColumns = @{
    4 = @('Verified', 'Partial', 'Not run', 'Fail-closed')
    5 = @('Not targeted')
    6 = @('Not targeted')
    7 = @('Verified', 'Partial', 'Failed', 'Not run', 'Fail-closed')
    8 = @('Verified', 'Partial', 'Failed', 'Not run', 'Not directly tested', 'Fail-closed', 'Not applicable')
    9 = @('Verified', 'Partial', 'Failed', 'Not run', 'Not directly tested', 'Fail-closed', 'Not applicable')
    10 = @('Verified', 'Partial', 'Failed', 'Not run', 'Not directly tested', 'Fail-closed', 'Not applicable')
    11 = @('Verified', 'Partial', 'Failed', 'Not run', 'Not directly tested', 'Fail-closed', 'Not applicable')
    12 = @('Verified', 'Partial', 'Failed', 'Not run', 'Not directly tested', 'Fail-closed', 'Not applicable')
}

$rows = [System.Collections.Generic.List[object]]::new()
$seen = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
for ($index = 2; $index -lt $tableLines.Count; $index++) {
    $cells = @(Get-Cells $tableLines[$index])
    if ($cells.Count -ne $expectedHeader.Count) {
        throw "Matrix row $($index + 1) has $($cells.Count) cells; expected $($expectedHeader.Count): $($tableLines[$index])"
    }

    $caseId = $cells[0].Trim('`')
    $symbol = $cells[1].Trim('`')
    $kind = $cells[2]
    $proofScope = $cells[3]
    if (-not $expectedCases.ContainsKey($caseId)) {
        throw "Matrix contains unknown positive case '$caseId'."
    }

    $key = "$caseId|$symbol"
    if (-not $seen.Add($key)) {
        throw "Matrix contains duplicate type row '$key'."
    }

    if ($kind -notin @('struct', 'union', 'enum', 'typedef', 'valuetype')) {
        throw "Matrix type '$symbol' in '$caseId' has unsupported IDL kind '$kind'."
    }

    foreach ($column in $statusColumns.Keys) {
        if ($cells[$column] -notin $statusColumns[$column]) {
            throw "Matrix type '$symbol' in '$caseId' has invalid '$($expectedHeader[$column])' status '$($cells[$column])'."
        }
    }

    if ([string]::IsNullOrWhiteSpace($proofScope)) { throw "Matrix type '$symbol' in '$caseId' has no proof scope." }
    if ([string]::IsNullOrWhiteSpace($cells[13])) {
        throw "Matrix type '$symbol' in '$caseId' has no evidence link."
    }

    foreach ($link in [regex]::Matches($cells[13], '\[[^\]]+\]\((?<path>[^)]+)\)')) {
        $target = $link.Groups['path'].Value
        if ($target -match '^https?://') {
            continue
        }

        $resolvedTarget = Join-Path (Split-Path -Parent $coveragePath) $target
        if (-not (Test-Path -LiteralPath $resolvedTarget)) {
            throw "Evidence link for '$key' does not exist: $target"
        }
    }

    $rows.Add([pscustomobject]@{
        Case = $caseId
        Symbol = $symbol
        Kind = $kind
    })
}

$coveredCases = @($rows | ForEach-Object Case | Sort-Object -Unique)
$missingCases = @($expectedCases.Keys | Where-Object { $_ -notin $coveredCases })
if ($missingCases.Count -gt 0) {
    throw "Matrix is missing positive corpus cases: $($missingCases -join ', ')"
}

# Check each matrix row against either generated header declarations or the
# explicit type inventory for a case the RTI C++ parser rejects.
foreach ($caseId in $expectedCases.Keys) {
    $headerPath = Join-Path $featureDirectory "$caseId/$caseId.hpp"
    $declarations = [System.Collections.Generic.Dictionary[string, string]]::new([StringComparer]::Ordinal)
    if ($rejectedCases.ContainsKey($caseId)) {
        foreach ($type in $rejectedCases[$caseId].types) {
            $declarations[[string]$type.symbol] = [string]$type.kind
        }
    } else {
        if (-not (Test-Path -LiteralPath $headerPath)) {
            throw "Missing retained generated header for positive case '$caseId': $headerPath"
        }

        $header = [IO.File]::ReadAllText($headerPath)
        $matches = [regex]::Matches(
            $header,
            '(?m)^/// @brief IDL (?<kind>struct|union|enumeration|typedef|valuetype) (?<name>[A-Za-z_][A-Za-z0-9_.:]*)\.?\s*$')
        foreach ($match in $matches) {
            $symbol = $match.Groups['name'].Value.TrimEnd('.').Replace('.', '::')
            $kind = switch ($match.Groups['kind'].Value) {
                'enumeration' { 'enum' }
                default { $match.Groups['kind'].Value }
            }
            $declarations[$symbol] = $kind
        }
    }

    # FlatData's IDL struct intentionally emits an RTI FlatData value alias and
    # has no `@brief IDL struct` comment on that alias.
    if ($caseId -eq '10-flat-data-binding') {
        $declarations['CorpusFlatDataBinding::Sample'] = 'struct'
    }

    $actualRows = @($rows | Where-Object Case -CEQ $caseId)
    $actualSymbols = @($actualRows | ForEach-Object Symbol | Sort-Object -Unique)
    $expectedSymbols = @($declarations.Keys | Sort-Object)

    $missingSymbols = @($expectedSymbols | Where-Object { $_ -notin $actualSymbols })
    $unexpectedSymbols = @($actualSymbols | Where-Object { -not $declarations.ContainsKey($_) })
    if ($missingSymbols.Count -gt 0 -or $unexpectedSymbols.Count -gt 0) {
        throw "Type inventory mismatch for '$caseId'. Missing: [$($missingSymbols -join ', ')]; unexpected: [$($unexpectedSymbols -join ', ')]"
    }

    foreach ($row in $actualRows) {
        if ($declarations[$row.Symbol] -cne $row.Kind) {
            throw "Type kind mismatch for '$caseId' '$($row.Symbol)': matrix says '$($row.Kind)', generated header says '$($declarations[$row.Symbol])'."
        }
    }

    $scope = $registryCases[$caseId]
    if ($scope) {
        $wrapperTypes = @()
        $wrapperPath = Join-Path $RepositoryRoot "tests/Wireloom.Dds.Generator.WireCompatibility/IdlWrappers/$caseId.idl"
        if (Test-Path -LiteralPath $wrapperPath) {
            $wrapperText = [IO.File]::ReadAllText($wrapperPath)
            $wrapperTypes = @([regex]::Matches($wrapperText, '(?m)^\s*struct\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)') |
                ForEach-Object { $_.Groups['name'].Value })
        }
        $listed = @()
        foreach ($property in @('dynamicType', 'partialDynamicType', 'registration', 'failClosed', 'liveDds')) { $listed += @($scope.$property) }
        if ($scope.failClosedCapabilities) {
            $supportedCapabilities = @('DynamicType', 'Registration', 'XCDR1', 'XCDR2', 'Live DDS')
            foreach ($capability in $scope.failClosedCapabilities.PSObject.Properties) {
                if ($capability.Name -notin $supportedCapabilities) {
                    throw "Scope registry case '$caseId' has unknown fail-closed capability '$($capability.Name)'."
                }
                $listed += @($capability.Value)
            }
        }
        foreach ($root in $scope.cdrRoots.PSObject.Properties) { $listed += @($root.Value) }
        foreach ($property in @('cdrXcdr1', 'cdrXcdr2', 'notApplicableXcdr1', 'notApplicableXcdr2')) { $listed += @($scope.$property) }
        foreach ($name in $listed | Sort-Object -Unique) {
            if ($name -notin @($actualRows | ForEach-Object { ($_.Symbol -split '::')[-1] }) -and
                $name -notin $wrapperTypes) {
                throw "Scope registry symbol '$name' does not exist in case '$caseId'."
            }
        }
    }
}

Write-Output "C++ type coverage structure passed: $($expectedCases.Count) positive IDL cases, $($rows.Count) type rows, including $($rejectedCases.Count) explicit RTI-matched rejections."
