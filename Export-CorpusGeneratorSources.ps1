#requires -Version 7.4
<#
.SYNOPSIS
Exports Wireloom-generated C# and C++ sources for the selected RTI API profiles.

.DESCRIPTION
Runs the corpus compliance project and C++ generator for RTI 6.1.2, 7.3.1, and
7.7.0 by default. Each version manifest specifies its C# export root. C++ output
is retained separately under generator/cpp/features for every profile. Each
generator uses the same corpus case inputs and exact RTI profile. Detailed oracle
comparisons are a separate suite.

The script restores dependencies in locked mode, then tests in Release using
the repository SDK. It stops after the first failed profile and restores the
process environment variables when it exits.

Existing matching files are overwritten, but obsolete files are not deleted.
Use a new -OutputRoot for a clean comparison. A new evidence directory retains
input/output hashes, SDK details, and test logs.

.EXAMPLE
./Export-CorpusGeneratorSources.ps1 -Profile 6.1.2

Exports the 6.1.2 profile through its exact-runtime generation/compilation checks.

.EXAMPLE
./Export-CorpusGeneratorSources.ps1

Exports all three profiles. See docs/corpus/README.md for the corpus layout.
#>

[CmdletBinding()]
param(
    [ValidateSet('6.1.2', '7.3.1', '7.7.0')]
    [string[]]$Profile = @('6.1.2', '7.3.1', '7.7.0'),

    [string]$OutputRoot,

    [string]$EvidenceDirectory
)

$ErrorActionPreference = 'Stop'
. "$PSScriptRoot/docs/corpus/scripts/Evidence.Common.ps1"

$project = 'tests/Wireloom.Dds.CSharp.CorpusCompliance/Wireloom.Dds.CSharp.CorpusCompliance.csproj'
$cppProject = 'src/Wireloom.Dds.Cpp.Generator/Wireloom.Dds.Cpp.Generator.csproj'
$manifestPath = 'docs/corpus/manifest.json'
$cppRetainedRoot = Join-Path $PSScriptRoot 'docs/corpus/7.7.0/generator/cpp/features'
if (-not (Test-Path -LiteralPath $cppRetainedRoot -PathType Container)) {
    throw "Missing retained 7.7.0 C++ feature sources: $cppRetainedRoot"
}

$cppRetainedTopLevelFiles = @(Get-ChildItem -LiteralPath $cppRetainedRoot -File)
if ($cppRetainedTopLevelFiles.Count -gt 0) {
    throw "Unexpected top-level files under retained C++ feature sources: $($cppRetainedTopLevelFiles.Name -join ', ')"
}

$cppCaseDirectories = @(
    Get-ChildItem -LiteralPath $cppRetainedRoot -Directory |
        ForEach-Object { $_.Name } | Sort-Object
)
if ($cppCaseDirectories.Count -eq 0) {
    throw "No retained C++ feature case directories found under $cppRetainedRoot"
}

$corpusManifest = Get-Content -LiteralPath (Join-Path $PSScriptRoot $manifestPath) -Raw | ConvertFrom-Json
$corpusCases = @{}
foreach ($case in $corpusManifest.positiveCases) {
    if ($corpusCases.ContainsKey([string]$case.id)) {
        throw "Duplicate positive corpus case ID '$($case.id)'."
    }
    $corpusCases[[string]$case.id] = $case
}

$cppExpectedCaseIds = @(
    $corpusManifest.positiveCases |
        ForEach-Object { [string]$_.id } |
        Where-Object { $_ -ne '07-union-wchar-label' } |
        Sort-Object
)
if ((Compare-Object -ReferenceObject $cppExpectedCaseIds -DifferenceObject $cppCaseDirectories -CaseSensitive).Count -gt 0) {
    throw 'Retained C++ feature cases do not match the positive corpus inventory minus the RTI 7.7.0 wchar-union-label parser rejection.'
}

$cppProfileExpectedRejections = @{
    '6.1.2' = @{
        '09-must-understand' = [ordered]@{
            diagnosticCode = 'DDSGCPP001'
            diagnosticMessage = "The Wireloom C++ RTI 6.1.2 adapter cannot represent one or more members of 'Sample'."
            reason = 'The 6.1.2 C++ adapter has no supported DynamicType metadata path for must_understand on this appendable member.'
        }
    }
}

foreach ($caseId in $cppCaseDirectories) {
    if (-not $corpusCases.ContainsKey($caseId)) {
        throw "Retained C++ case '$caseId' is missing from $manifestPath."
    }

    $retainedCasePath = Join-Path $cppRetainedRoot $caseId
    $expectedRetainedFiles = @("$caseId.cxx", "$caseId.hpp", "${caseId}Plugin.cxx", "${caseId}Plugin.hpp") | Sort-Object
    $actualRetainedFiles = @(
        Get-ChildItem -LiteralPath $retainedCasePath -File |
            ForEach-Object { $_.Name } | Sort-Object
    )
    if ((Compare-Object -ReferenceObject $expectedRetainedFiles -DifferenceObject $actualRetainedFiles -CaseSensitive).Count -gt 0) {
        throw "Retained C++ case '$caseId' does not contain exactly its four generated source files."
    }
}

function Get-CppGenerationArguments {
    param(
        [Parameter(Mandatory)] $Case,
        [Parameter(Mandatory)] [string] $IdlPath,
        [Parameter(Mandatory)] [string] $OutputPath,
        [Parameter(Mandatory)] [string] $Version
    )

    $arguments = [Collections.Generic.List[string]]::new()
    $arguments.Add('generate')
    $arguments.Add($IdlPath)
    $arguments.Add($OutputPath)
    $arguments.Add('--rti-version')
    $arguments.Add($Version)

    foreach ($define in @($Case.defines)) {
        if (-not [string]::IsNullOrWhiteSpace([string]$define)) {
            $arguments.Add('--define')
            $arguments.Add([string]$define)
        }
    }

    # The shared manifest records RTI C++ unbounded requirements as -unboundedSupport.
    # Wireloom uses the equivalent --unbounded-support switch.
    $rtiCppOptions = @($Case.wireMatrix.rtiCppCodegenOptions)
    if (@($Case.rtiddsgenOptions) -contains '-unboundedSupport' -or
        $rtiCppOptions -contains '-unboundedSupport') {
        $arguments.Add('--unbounded-support')
    }

    # Corpus includes are rooted at docs/corpus/idl; adding both shared include
    # locations mirrors Verify-Cli while allowing the manifest to own case IDs
    # and IDL paths without a second hand-maintained option table.
    $arguments.Add('--include-directory')
    $arguments.Add((Join-Path $PSScriptRoot 'docs/corpus/idl'))
    $arguments.Add('--include-directory')
    $arguments.Add((Join-Path $PSScriptRoot 'docs/corpus/idl/includes'))
    return $arguments.ToArray()
}

if (!$OutputRoot) {
    $OutputRoot = 'docs/corpus'
}

$output = [IO.Path]::GetFullPath($OutputRoot, $PSScriptRoot)

if (!$EvidenceDirectory) {
    $timestamp = [DateTime]::UtcNow.ToString('yyyyMMddTHHmmssZ')
    $runId = [Guid]::NewGuid().ToString('N').Substring(0, 8)
    $EvidenceDirectory = "artifacts/corpus-export-$timestamp-$runId"
}

$evidence = [IO.Path]::GetFullPath($EvidenceDirectory, $PSScriptRoot)

if (Test-Path -LiteralPath $evidence) {
    throw 'EvidenceDirectory must be new to preserve earlier runs.'
}

New-Item -ItemType Directory -Force -Path $evidence | Out-Null

$inputs = @(
    'src/Wireloom.Dds.Idl'
    'src/Wireloom.Dds.CSharp.Compiler'
    'src/Wireloom.Dds.CSharp.Generator'
    'src/Wireloom.Dds.Cpp.Compiler'
    'src/Wireloom.Dds.Cpp.Generator'
    'tests/Wireloom.Dds.CSharp.CorpusCompliance'
    'docs/corpus/idl'
    'docs/corpus/manifest.json'
    'docs/corpus/7.7.0/generator/cpp/features'
    'docs/corpus/6.1.2/manifest.json'
    'docs/corpus/7.3.1/manifest.json'
    'docs/corpus/7.7.0/manifest.json'
    'docs/corpus/scripts/Evidence.Common.ps1'
    'Export-CorpusGeneratorSources.ps1'
    'global.json'
    'Directory.Build.props'
    'Directory.Packages.props'
    'tests/Directory.Build.props'
    'NuGet.config'
)

$cppRetainedBaseline = Get-EvidenceSnapshot $PSScriptRoot @('docs/corpus/7.7.0/generator/cpp/features')
$stabilityInputs = @($inputs)
$retainedCppOutput = [IO.Path]::GetFullPath((Join-Path (Join-Path $output '7.7.0') 'generator/cpp/features'))
if ([string]::Equals($retainedCppOutput, $cppRetainedRoot, [StringComparison]::OrdinalIgnoreCase)) {
    $stabilityInputs = @($inputs | Where-Object { $_ -ne 'docs/corpus/7.7.0/generator/cpp/features' })
}

$before = Get-EvidenceSnapshot $PSScriptRoot $stabilityInputs
$revision = Get-EvidenceRevision $PSScriptRoot

$previousProfile = [Environment]::GetEnvironmentVariable('CORPUS_RTI_PROFILE', 'Process')
$previousExportRoot = [Environment]::GetEnvironmentVariable('CORPUS_GENERATOR_EXPORT_ROOT', 'Process')

Push-Location $PSScriptRoot

try {
    $sdkVersion = dotnet --version

    if ($LASTEXITCODE -ne 0) {
        throw 'Cannot identify the .NET SDK version.'
    }

    $sdkVersion | Out-File "$evidence/dotnet-version.txt" -Encoding utf8

    $restoreOutput = & dotnet restore $project --locked-mode 2>&1
    $restoreExitCode = $LASTEXITCODE
    $restoreOutput | Tee-Object "$evidence/restore.txt"

    if ($restoreExitCode -ne 0) {
        throw "Corpus locked restore failed (exit code $restoreExitCode)."
    }

    $cppRestoreOutput = & dotnet restore $cppProject --locked-mode 2>&1
    $cppRestoreExitCode = $LASTEXITCODE
    $cppRestoreOutput | Tee-Object "$evidence/cpp-restore.txt"
    if ($cppRestoreExitCode -ne 0) {
        throw "C++ generator locked restore failed (exit code $cppRestoreExitCode)."
    }

    $cppBuildOutput = & dotnet build $cppProject --no-restore --configuration Release 2>&1
    $cppBuildExitCode = $LASTEXITCODE
    $cppBuildOutput | Tee-Object "$evidence/cpp-build.txt"
    if ($cppBuildExitCode -ne 0) {
        throw "C++ generator build failed (exit code $cppBuildExitCode)."
    }

    $cppAssembly = Join-Path $PSScriptRoot 'src/Wireloom.Dds.Cpp.Generator/bin/Release/net10.0/Wireloom.Dds.Cpp.dll'
    if (-not (Test-Path -LiteralPath $cppAssembly -PathType Leaf)) {
        throw "C++ generator assembly was not produced: $cppAssembly"
    }

    foreach ($version in $Profile | Select-Object -Unique) {
        $env:CORPUS_RTI_PROFILE = $version
        $profileManifestPath = Join-Path $PSScriptRoot "docs/corpus/$version/manifest.json"
        $profileManifest = Get-Content -LiteralPath $profileManifestPath -Raw | ConvertFrom-Json
        if ([string]::IsNullOrWhiteSpace($profileManifest.generatorSourceRoot)) {
            throw "Missing generatorSourceRoot in the $version corpus manifest."
        }

        $env:CORPUS_GENERATOR_EXPORT_ROOT = Join-Path (Join-Path $output $version) $profileManifest.generatorSourceRoot
        $startedAt = [DateTime]::UtcNow.ToString('o')
        $profileCppExpectedRejections = if ($cppProfileExpectedRejections.ContainsKey($version)) {
            $cppProfileExpectedRejections[$version]
        }
        else {
            @{}
        }
        $profileCppCaseDirectories = @(
            $cppCaseDirectories | Where-Object { -not $profileCppExpectedRejections.ContainsKey($_) }
        )

        $testArguments = @(
            'run'
            '--project', $project
            '--no-restore'
            '--configuration', 'Release'
            '--'
            '--filter-trait', 'CorpusSuite=Generation'
        )

        Write-Host "Exporting corpus sources for RTI profile $version..."
        Push-Location -LiteralPath $PSScriptRoot
        try {
            & dotnet @testArguments | Tee-Object "$evidence/$version-tests.txt"
            $testExitCode = $LASTEXITCODE
        }
        finally {
            Pop-Location
        }

        if ($testExitCode -ne 0) {
            throw "Corpus source export failed for RTI profile $version (exit code $testExitCode)."
        }

        $after = Get-EvidenceSnapshot $PSScriptRoot $stabilityInputs

        if ($after.sha256 -ne $before.sha256) {
            throw 'Export inputs changed during execution; do not promote this run.'
        }

        $outputFiles = @(
            Get-ChildItem -LiteralPath $env:CORPUS_GENERATOR_EXPORT_ROOT -Recurse -File -Filter '*.g.cs' |
                ForEach-Object {
                    [IO.Path]::GetRelativePath($env:CORPUS_GENERATOR_EXPORT_ROOT, $_.FullName)
                }
        )

        $outputSnapshot = Get-EvidenceSnapshot $env:CORPUS_GENERATOR_EXPORT_ROOT $outputFiles
        $relativeOutput = [IO.Path]::GetRelativePath($PSScriptRoot, $env:CORPUS_GENERATOR_EXPORT_ROOT)
        $relativeOutput = $relativeOutput.Replace([IO.Path]::DirectorySeparatorChar, [char]'/')

        $exportEvidence = [ordered]@{
            schemaVersion = 1
            kind = 'wireloom-corpus-export'
            profile = $version
            startedAtUtc = $startedAt
            completedAtUtc = [DateTime]::UtcNow.ToString('o')
            source = $revision
            inputs = $before
            configuration = 'Release'
            sdk = $sdkVersion
            testExitCode = 0
            testArguments = $testArguments
            outputDirectory = $relativeOutput
            output = $outputSnapshot
            staleOutputPolicy = 'Existing files are not deleted. Use a new OutputRoot for an uncontaminated export.'
        }

        Write-EvidenceJson "$evidence/$version.json" $exportEvidence

        $cppOutputRoot = Join-Path (Join-Path $output $version) 'generator/cpp/features'
        foreach ($caseId in $profileCppCaseDirectories) {
            $case = $corpusCases[$caseId]
            $idlPath = Join-Path (Join-Path $PSScriptRoot 'docs/corpus') ([string]$case.idl)
            if (-not (Test-Path -LiteralPath $idlPath -PathType Leaf)) {
                throw "C++ corpus case '$caseId' refers to missing IDL: $($case.idl)"
            }

            $caseOutput = Join-Path $cppOutputRoot $caseId
            $caseArguments = Get-CppGenerationArguments -Case $case -IdlPath $idlPath -OutputPath $caseOutput -Version $version
            $caseLogPath = Join-Path $evidence "$version-cpp-$caseId.txt"
            $cppOutput = & dotnet $cppAssembly @caseArguments 2>&1
            $cppExitCode = $LASTEXITCODE
            $cppOutput | Out-File -LiteralPath $caseLogPath -Encoding utf8
            if ($cppExitCode -ne 0) {
                throw "C++ corpus generation rejected RTI $version case '$caseId' (exit code $cppExitCode). See $caseLogPath."
            }

            $expectedFiles = @("$caseId.cxx", "$caseId.hpp", "${caseId}Plugin.cxx", "${caseId}Plugin.hpp") | Sort-Object
            $actualFiles = @(
                Get-ChildItem -LiteralPath $caseOutput -File |
                    ForEach-Object { $_.Name } | Sort-Object
            )
            if ((Compare-Object -ReferenceObject $expectedFiles -DifferenceObject $actualFiles -CaseSensitive).Count -gt 0) {
                throw "C++ corpus case '$caseId' for RTI $version did not produce exactly the expected four files."
            }
        }

        foreach ($caseId in $profileCppExpectedRejections.Keys) {
            $case = $corpusCases[$caseId]
            $idlPath = Join-Path (Join-Path $PSScriptRoot 'docs/corpus') ([string]$case.idl)
            if (-not (Test-Path -LiteralPath $idlPath -PathType Leaf)) {
                throw "Expected C++ rejection case '$caseId' refers to missing IDL: $($case.idl)"
            }

            $caseOutput = Join-Path $cppOutputRoot $caseId
            $caseArguments = Get-CppGenerationArguments -Case $case -IdlPath $idlPath -OutputPath $caseOutput -Version $version
            $caseLogPath = Join-Path $evidence "$version-cpp-$caseId.txt"
            $cppOutput = & dotnet $cppAssembly @caseArguments 2>&1
            $cppExitCode = $LASTEXITCODE
            $cppOutputText = $cppOutput | Out-String
            $cppOutput | Out-File -LiteralPath $caseLogPath -Encoding utf8
            if ($cppExitCode -eq 0) {
                throw "C++ corpus case '$caseId' unexpectedly succeeded for RTI $version; update the profile case inventory."
            }
            $expectedRejection = $profileCppExpectedRejections[$caseId]
            if ($cppOutputText -notmatch [regex]::Escape([string]$expectedRejection.diagnosticCode) -or $cppOutputText -notmatch [regex]::Escape([string]$expectedRejection.diagnosticMessage)) {
                throw "C++ corpus case '$caseId' for RTI $version failed with an unexpected diagnostic. See $caseLogPath."
            }
            if (Test-Path -LiteralPath $caseOutput -PathType Container) {
                $unexpectedFiles = @(Get-ChildItem -LiteralPath $caseOutput -File)
                if ($unexpectedFiles.Count -gt 0) {
                    throw "C++ corpus case '$caseId' for RTI $version wrote files despite its expected rejection."
                }
            }
        }

        $actualCppCaseDirectories = @(
            Get-ChildItem -LiteralPath $cppOutputRoot -Directory |
                ForEach-Object { $_.Name } | Sort-Object
        )
        if ((Compare-Object -ReferenceObject $profileCppCaseDirectories -DifferenceObject $actualCppCaseDirectories -CaseSensitive).Count -gt 0) {
            throw "C++ export for RTI $version contains missing or unexpected top-level case directories."
        }

        $afterCpp = Get-EvidenceSnapshot $PSScriptRoot $stabilityInputs
        if ($afterCpp.sha256 -ne $before.sha256) {
            throw 'Export inputs changed during C++ generation; do not promote this run.'
        }

        $cppOutputFiles = @(
            Get-ChildItem -LiteralPath $cppOutputRoot -Recurse -File -Filter '*.cxx' |
                ForEach-Object { [IO.Path]::GetRelativePath($cppOutputRoot, $_.FullName) }
        ) + @(
            Get-ChildItem -LiteralPath $cppOutputRoot -Recurse -File -Filter '*.hpp' |
                ForEach-Object { [IO.Path]::GetRelativePath($cppOutputRoot, $_.FullName) }
        )
        $cppOutputSnapshot = Get-EvidenceSnapshot $cppOutputRoot $cppOutputFiles
        $cppRelativeOutput = [IO.Path]::GetRelativePath($PSScriptRoot, $cppOutputRoot)
        $cppRelativeOutput = $cppRelativeOutput.Replace([IO.Path]::DirectorySeparatorChar, [char]'/')
        $cppEvidence = [ordered]@{
            schemaVersion = 1
            kind = 'wireloom-cpp-corpus-export'
            profile = $version
            startedAtUtc = $startedAt
            completedAtUtc = [DateTime]::UtcNow.ToString('o')
            source = $revision
            inputs = $before
            configuration = 'Release'
            sdk = $sdkVersion
            generatorAssembly = 'src/Wireloom.Dds.Cpp.Generator/bin/Release/net10.0/Wireloom.Dds.Cpp.dll'
            caseCount = $profileCppCaseDirectories.Count
            caseIds = $profileCppCaseDirectories
            retainedBaseline = $cppRetainedBaseline
            expectedRejections = @(
                foreach ($caseId in $profileCppExpectedRejections.Keys) {
                    [ordered]@{
                        id = $caseId
                        diagnosticCode = $profileCppExpectedRejections[$caseId].diagnosticCode
                        diagnosticMessage = $profileCppExpectedRejections[$caseId].diagnosticMessage
                        reason = $profileCppExpectedRejections[$caseId].reason
                        evidenceLog = "$version-cpp-$caseId.txt"
                    }
                }
            )
            rtiVersionArgument = $version
            outputDirectory = $cppRelativeOutput
            output = $cppOutputSnapshot
            staleOutputPolicy = 'Existing files are not deleted. Use a new OutputRoot for an uncontaminated export.'
        }
        Write-EvidenceJson "$evidence/$version-cpp.json" $cppEvidence
    }

    Write-Host "Export provenance and test logs: $evidence"
}
finally {
    Pop-Location

    [Environment]::SetEnvironmentVariable('CORPUS_RTI_PROFILE', $previousProfile, 'Process')
    [Environment]::SetEnvironmentVariable('CORPUS_GENERATOR_EXPORT_ROOT', $previousExportRoot, 'Process')
}
