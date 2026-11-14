[CmdletBinding()]
param(
    [string] $CliAssembly = (Join-Path $PSScriptRoot 'bin/Release/net10.0/Wireloom.Dds.Cpp.dll')
)

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false

$repository = (Resolve-Path -LiteralPath (Join-Path $PSScriptRoot '../..')).Path
$coverageVerifier = Join-Path $repository 'scripts/Verify-CppCoverage.ps1'
$inventory = & $coverageVerifier -RepositoryRoot $repository -PassThru
$cli = (Resolve-Path -LiteralPath $CliAssembly).Path
$manifestIds = @($inventory.ManifestIds)
$corpusCases = [System.Collections.Generic.Dictionary[string, object]]::new([StringComparer]::Ordinal)
foreach ($case in $inventory.Cases) {
    $corpusCases.Add([string] $case.id, $case)
}
$rtiRejectedCases = @($inventory.RejectedCases)
$rtiRejectedIds = @($rtiRejectedCases | ForEach-Object { [string] $_.case })
$featureDirectory = Join-Path $repository 'docs/corpus/7.7.0/generator/cpp/features'

function Get-DuplicateIds([string[]] $ids) {
    @($ids | Group-Object | Where-Object Count -gt 1 | ForEach-Object Name | Sort-Object)
}

function Get-MissingIds([string[]] $expected, [string[]] $actual) {
    $actualSet = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    foreach ($id in $actual) { [void]$actualSet.Add($id) }
    @($expected | Where-Object { -not $actualSet.Contains($_) } | Sort-Object -Unique)
}

function Get-UnexpectedIds([string[]] $expected, [string[]] $actual) {
    $expectedSet = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    foreach ($id in $expected) { [void]$expectedSet.Add($id) }
    @($actual | Where-Object { -not $expectedSet.Contains($_) } | Sort-Object -Unique)
}

$idl = Join-Path $repository 'docs/corpus/idl/features/10-annotations.idl'
$retained = Join-Path $repository 'docs/corpus/7.7.0/generator/cpp/features/10-annotations'
$scratch = Join-Path $repository "artifacts/cpp-cli-check/$([Guid]::NewGuid().ToString('N'))"
New-Item -ItemType Directory -Path $scratch -Force | Out-Null
$comparisonFailures = [System.Collections.Generic.List[string]]::new()

$generated = Join-Path $scratch 'generated'
& dotnet $cli generate $idl $generated
if ($LASTEXITCODE -ne 0) {
    throw 'C044 failed to generate.'
}

$expectedFiles = @(
    '10-annotations.cxx',
    '10-annotations.hpp',
    '10-annotationsPlugin.cxx',
    '10-annotationsPlugin.hpp'
)
$actualFiles = @(Get-ChildItem -LiteralPath $generated -File | Select-Object -ExpandProperty Name)
if (Compare-Object -ReferenceObject $expectedFiles -DifferenceObject $actualFiles -CaseSensitive) {
    throw 'C044 did not generate the expected four files.'
}

foreach ($file in $expectedFiles) {
    $expected = Get-FileHash -LiteralPath (Join-Path $retained $file)
    $actual = Get-FileHash -LiteralPath (Join-Path $generated $file)
    if ($expected.Hash -ne $actual.Hash) {
        $comparisonFailures.Add("10-annotations/$file")
    }
}

$cliCases = @('01-primitives', '01-boundaries', '01-int8-boundaries', '01-full-widths', '02-names-constants', '02-formatting', '02-multiple', '02-scopes', '02-constant-expressions', '03-enum-values-explicit', '03-enum-values-prefix', '03-alias-aggregate', '03-enums-aliases', '03-alias-collections', '04-strings', '04-boundaries', '04-unbounded-string-aliases', '04-string-alias-chains', '05-collections', '05-shapes', '05-array-of-sequences', '05-array-of-sequences-idl', '05-unbounded-sequence-aliases', '06-aggregates', '06-valuetypes', '06-alias-composition', '07-unions', '07-union-enum', '07-union-scoped', '07-union-multilabel', '07-union-short', '07-union-octet', '07-union-ushort', '07-union-ulong', '07-union-int8', '07-union-uint8', '07-union-int16', '07-union-uint16', '07-union-int32', '07-union-uint32', '07-union-long', '07-union-string-sequence-bounded-direct', '07-union-string-sequence-aliases', '07-union-short-boundaries', '07-union-char', '07-union-char-boundaries', '07-union-boolean', '07-union-aliases', '07-union-char-numeric', '07-union-char-numeric-255', '08-keys', '08-key-nested', '08-key-inherited', '08-key-boundaries', '08-key-union', '09-data-representation', '09-allowed-data-representation', '09-default', '09-extensibility', '09-evolution-optional', '09-optional-collections', '09-optional-aggregate-member', '09-optional-string-sequences', '09-optional-recursive-array', '09-id-gaps', '09-autoid-hash', '09-must-understand', '09-defaults-ranges', '10-default-literal-enum-array', '10-annotations-extended', '10-annotations-rti', '10-resolve-name-wstring-aliases', '10-flat-data-binding', '14-extensibility-full', '14-autoid-scope', '14-annotation-arguments', '14-member-declarators', '14-union-label-expressions', '14-enum-constant', '14-union-scoped-enum', '14-union-array-branch', '14-string-expression-bound', '14-typed-defaults', '14-unbounded-support', '14-unbounded-receive-lifecycle', '11-conditionals', '11-comments', '11-macros', '11-preprocessing', '11-macro-operators', '11-preprocessor-advanced', '11-include-search')

# C044 is the separate 10-annotations check above; include its manifest ID so
# the verifier's complete explicit case set is checked exactly once.
$cliCaseIds = @('10-annotations') + $cliCases + $rtiRejectedIds
$cliCaseDuplicates = @(Get-DuplicateIds $cliCaseIds)
$cliMissingIds = @(Get-MissingIds $manifestIds $cliCaseIds)
$cliUnexpectedIds = @(Get-UnexpectedIds $manifestIds $cliCaseIds)
if ($cliCaseDuplicates.Count -gt 0 -or $cliMissingIds.Count -gt 0 -or $cliUnexpectedIds.Count -gt 0) {
    $cliProblems = [System.Collections.Generic.List[string]]::new()
    if ($cliCaseDuplicates.Count -gt 0) {
        $cliProblems.Add("duplicate IDs: $($cliCaseDuplicates -join ', ')")
    }
    if ($cliMissingIds.Count -gt 0) {
        $cliProblems.Add("missing IDs: $($cliMissingIds -join ', ')")
    }
    if ($cliUnexpectedIds.Count -gt 0) {
        $cliProblems.Add("unexpected IDs: $($cliUnexpectedIds -join ', ')")
    }
    throw "Verify-Cli explicit case inventory mismatch: $($cliProblems -join '; ')"
}

foreach ($rejectedCase in $rtiRejectedCases) {
    $case = [string]$rejectedCase.case
    $caseIdl = Join-Path $repository "docs/corpus/idl/features/$case.idl"
    $caseOutput = Join-Path $scratch "rejected-$case"
    $diagnostic = & dotnet $cli generate $caseIdl $caseOutput 2>&1 | Out-String
    $expectedMessage = [string]$rejectedCase.diagnostic
    if ($LASTEXITCODE -ne 1 -or
        $diagnostic -notmatch "$([regex]::Escape($case)).idl\(2,25\): Error DDSGCPP001:" -or
        -not $diagnostic.Contains($expectedMessage) -or
        (Test-Path -LiteralPath $caseOutput)) {
        throw "$case must fail closed with its RTI-matched diagnostic and emit no files. Received: $diagnostic"
    }

    $retainedOutput = Join-Path $featureDirectory $case
    if (Test-Path -LiteralPath $retainedOutput) {
        throw "$case is an RTI-rejected C++ case and must not retain generated output: $retainedOutput"
    }
}

foreach ($case in $cliCases) {
    $manifestCase = $corpusCases[$case]
    $caseIdl = Join-Path (Join-Path $repository 'docs/corpus') $manifestCase.idl
    $caseRetained = Join-Path $repository "docs/corpus/7.7.0/generator/cpp/features/$case"
    $caseGenerated = Join-Path $scratch "generated-$case"
    $arguments = @($cli, 'generate', $caseIdl, $caseGenerated, '--rti-version', '7.7.0')
    foreach ($define in @($manifestCase.defines)) {
        if (-not [string]::IsNullOrWhiteSpace([string] $define)) {
            $arguments += @('--define', [string] $define)
        }
    }
    if (@($manifestCase.rtiddsgenOptions) -contains '-unboundedSupport' -or
        @($manifestCase.wireMatrix.rtiCppCodegenOptions) -contains '-unboundedSupport') {
        $arguments += '--unbounded-support'
    }
    $arguments += @('--include-directory', (Join-Path $repository 'docs/corpus/idl'))
    $arguments += @('--include-directory', (Join-Path $repository 'docs/corpus/idl/includes'))

    & dotnet @arguments
    if ($LASTEXITCODE -ne 0) {
        throw "$case failed to generate."
    }

    $caseFiles = @(
        "$case.cxx",
        "$case.hpp",
        "${case}Plugin.cxx",
        "${case}Plugin.hpp"
    )
    $actualCaseFiles = @(Get-ChildItem -LiteralPath $caseGenerated -File | Select-Object -ExpandProperty Name)
    if (Compare-Object -ReferenceObject $caseFiles -DifferenceObject $actualCaseFiles -CaseSensitive) {
        throw "$case did not generate the expected four files."
    }

    foreach ($file in $caseFiles) {
        $expected = Get-FileHash -LiteralPath (Join-Path $caseRetained $file)
        $actual = Get-FileHash -LiteralPath (Join-Path $caseGenerated $file)
        if ($expected.Hash -ne $actual.Hash) {
            $comparisonFailures.Add("$case/$file")
        }
    }
}

$source = [IO.File]::ReadAllText($idl)
$invalidInput = Join-Path $scratch 'invalid-input'
$changedInput = Join-Path $scratch 'changed-input'
New-Item -ItemType Directory -Path $invalidInput, $changedInput | Out-Null

$invalidIdl = Join-Path $invalidInput '10-annotations.idl'
[IO.File]::WriteAllText($invalidIdl, $source.Replace(
    '@default(3) long value;',
    '@optional sequence<long, 4> invalid;',
    [StringComparison]::Ordinal))
$invalidOutput = Join-Path $scratch 'invalid-output'
$diagnostic = & dotnet $cli generate $invalidIdl $invalidOutput 2>&1 | Out-String
if ($LASTEXITCODE -ne 1 -or $diagnostic -notmatch '10-annotations\.idl\([1-9]\d*,[1-9]\d*\): Error DDSGCPP001:' -or
    (Test-Path -LiteralPath $invalidOutput)) {
    throw "Unsupported fields must produce a source-located error without generating output. Received: $diagnostic"
}

$unsupportedAnnotations = @(
    @{ Name = 'unit'; Original = '@default(3)'; Replacement = '@unit("meters") @default(3)'; Annotation = '@unit'; Message = 'supports @unit only with a bounded @range on a long member' },
    @{ Name = 'representation'; Original = '@topic'; Replacement = '@data_representation(XCDR) @topic'; Annotation = '@data_representation'; Message = 'supports only @data_representation(XCDR2)' },
    @{ Name = 'allowed-representation'; Original = '@topic'; Replacement = '@allowed_data_representation(XCDR) @topic'; Annotation = '@allowed_data_representation'; Message = 'supports only @allowed_data_representation(XCDR2)' },
    @{ Name = 'module'; Original = 'module CorpusAnnotations'; Replacement = '@autoid(SEQUENTIAL) module CorpusAnnotations'; Annotation = '@autoid'; Message = 'supports only @autoid(HASH)' }
)
foreach ($case in $unsupportedAnnotations) {
    $caseInput = Join-Path $scratch $case.Name
    New-Item -ItemType Directory -Path $caseInput | Out-Null
    $caseIdl = Join-Path $caseInput '10-annotations.idl'
    $caseSource = $source.Replace($case.Original, $case.Replacement, [StringComparison]::Ordinal)
    [IO.File]::WriteAllText($caseIdl, $caseSource)
    $annotationOffset = $caseSource.IndexOf($case.Annotation, [StringComparison]::Ordinal)
    $prefix = $caseSource.Substring(0, $annotationOffset)
    $line = 1 + ($prefix.ToCharArray() | Where-Object { $_ -eq "`n" }).Count
    $column = $prefix.Length - $prefix.LastIndexOf("`n", [StringComparison]::Ordinal)
    $caseOutput = Join-Path $caseInput 'generated'
    $diagnostic = & dotnet $cli generate $caseIdl $caseOutput 2>&1 | Out-String
    $expectedLocation = "10-annotations.idl($line,$column): Error DDSGCPP001:"
    $expectedMessage = if ($case.Message) { $case.Message } else { "annotation '$($case.Annotation)'" }
    if ($LASTEXITCODE -ne 1 -or -not $diagnostic.Contains($expectedLocation) -or
        -not $diagnostic.Contains($expectedMessage) -or (Test-Path -LiteralPath $caseOutput)) {
        throw "Unsupported annotation must fail at its source location. Expected $expectedLocation Received: $diagnostic"
    }
}

$resolveNameSource = [IO.File]::ReadAllText((Join-Path $repository 'docs/corpus/idl/features/10-resolve-name-wstring-aliases.idl'))
$resolveNameInput = Join-Path $scratch 'resolve-name-true-input'
New-Item -ItemType Directory -Path $resolveNameInput | Out-Null
$resolveNameIdl = Join-Path $resolveNameInput '10-resolve-name-wstring-aliases.idl'
[IO.File]::WriteAllText($resolveNameIdl, $resolveNameSource.Replace(
    '@resolve_name(false)',
    '@resolve_name(true)',
    [StringComparison]::Ordinal))
$resolveNameOutput = Join-Path $resolveNameInput 'generated'
$resolveNameDiagnostic = & dotnet $cli generate $resolveNameIdl $resolveNameOutput 2>&1 | Out-String
if ($LASTEXITCODE -ne 1 -or
    $resolveNameDiagnostic -notmatch '10-resolve-name-wstring-aliases\.idl\([1-9]\d*,[1-9]\d*\): Error DDSGCPP001:' -or
    $resolveNameDiagnostic -notmatch 'supports only @resolve_name\(false\)' -or
    (Test-Path -LiteralPath $resolveNameOutput)) {
    throw "Unsupported @resolve_name(true) must fail at its source location without generating output. Received: $resolveNameDiagnostic"
}

$changedIdl = Join-Path $changedInput '10-annotations.idl'
[IO.File]::WriteAllText($changedIdl, $source.Replace('@default(3)', '@default(42)', [StringComparison]::Ordinal))
$changedOutput = Join-Path $scratch 'changed-output'
& dotnet $cli generate $changedIdl $changedOutput
if ($LASTEXITCODE -ne 0 -or
    -not [IO.File]::ReadAllText((Join-Path $changedOutput '10-annotations.hpp')).Contains('std::int32_t value = 42;')) {
    throw 'The generated type did not use the changed IDL default.'
}

$report = [ordered]@{
    status = if ($comparisonFailures.Count -eq 0) { 'Passed' } else { 'Failed' }
    profile = '7.7.0'
    positiveCases = $manifestIds.Count
    generatedCases = $cliCases.Count + 1
    rejectedCases = $rtiRejectedCases.Count
    comparedFiles = ($cliCases.Count + 1) * 4
    mismatchedFiles = @($comparisonFailures)
    diagnosticChecks = $rtiRejectedCases.Count + $unsupportedAnnotations.Count + 2
    changedDefaultVerified = $true
}
$reportPath = Join-Path $scratch 'verification.json'
$report | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath $reportPath
if ($comparisonFailures.Count -gt 0) {
    throw "C++ CLI output differs from $($comparisonFailures.Count) retained files. Review the differences before refreshing corpus evidence. Report: $reportPath"
}

Write-Output "C++ CLI checks passed: $($manifestIds.Count) cases, $($report.comparedFiles) files, $($report.diagnosticChecks) diagnostic checks. Evidence: $scratch"
