[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string] $Package,
    [string] $PreviousPackage,
    [string] $CMakeGenerator,
    [string] $OutputRoot = (Join-Path $PSScriptRoot '../../artifacts/cpp-package-tests'),
    [string] $PlatformToolset
)

$ErrorActionPreference = 'Stop'
$repo = (Resolve-Path (Join-Path $PSScriptRoot '../..')).Path
$packagePath = (Resolve-Path -LiteralPath $Package).Path
$run = Join-Path ([IO.Path]::GetFullPath($OutputRoot)) ([DateTime]::UtcNow.ToString('yyyyMMddTHHmmssfffZ'))
$expanded = Join-Path $run 'package with spaces'
[IO.Compression.ZipFile]::ExtractToDirectory($packagePath, $expanded)

function Read-PackageVersion([string] $Directory) {
    $packageManifests = @(Get-ChildItem -LiteralPath $Directory -Filter '*.nuspec' -File)
    if ($packageManifests.Count -ne 1) { throw 'Expected one package manifest.' }
    [xml] $packageManifest = Get-Content -LiteralPath $packageManifests[0].FullName -Raw
    $version = [string] $packageManifest.package.metadata.version
    if ($packageManifest.package.metadata.id -ne 'Wireloom.Dds.Cpp.Generator' -or
        $version -notmatch '^\d+\.\d+\.\d+(?:-[0-9A-Za-z.-]+)?$') {
        throw 'Expected the Wireloom C++ generator package with an explicit version.'
    }
    return $version
}

$packageVersion = Read-PackageVersion $expanded
if ($PreviousPackage) {
    $previousPackagePath = (Resolve-Path -LiteralPath $PreviousPackage).Path
    $previousExpanded = Join-Path $run 'previous package with spaces'
    [IO.Compression.ZipFile]::ExtractToDirectory($previousPackagePath, $previousExpanded)
    $previousVersion = Read-PackageVersion $previousExpanded
    if ($previousVersion -eq $packageVersion) { throw 'Upgrade verification requires two distinct package versions.' }
}

$expectedAssets = @(
    'tools/win-x64/Wireloom.Dds.Cpp.exe',
    'tools/linux-x64/Wireloom.Dds.Cpp',
    'build/native/Wireloom.Dds.Cpp.Generator.props',
    'build/native/Wireloom.Dds.Cpp.Generator.targets',
    'build/cmake/WireloomCppConfig.cmake'
)
foreach ($asset in $expectedAssets) {
    if (-not (Test-Path -LiteralPath (Join-Path $expanded $asset))) {
        throw "Missing package asset: $asset"
    }
}
$managed = @(Get-ChildItem (Join-Path $expanded 'tools') -File -Recurse |
    Where-Object { $_.Extension -in @('.dll', '.pdb', '.json') })
if ($managed.Count) { throw 'Native tool payload contains managed runtime or debug assets.' }

$rid = if ($IsWindows) { 'win-x64' } elseif ($IsLinux) { 'linux-x64' } else { throw 'Unsupported verification host.' }
$toolName = if ($IsWindows) { 'Wireloom.Dds.Cpp.exe' } else { 'Wireloom.Dds.Cpp' }
$tool = Join-Path $expanded "tools/$rid/$toolName"
if ($IsLinux) {
    & chmod +x $tool
    if ($LASTEXITCODE) { throw 'Could not set the restored executable permission.' }
}

$idl = Join-Path $repo 'docs/corpus/idl/features/01-primitives.idl'
$generated = Join-Path $run 'native output'
$savedPath = $env:PATH
$savedDotnetRoot = $env:DOTNET_ROOT
try {
    $env:PATH = if ($IsWindows) { Join-Path $env:SystemRoot 'System32' } else { Join-Path $run 'absent-path' }
    $env:DOTNET_ROOT = Join-Path $run 'absent-dotnet'
    $help = & $tool --help 2>&1
    if ($LASTEXITCODE -ne 0 -or "$help" -notmatch 'Usage: Wireloom.Dds.Cpp generate') {
        throw 'Native help did not describe the packaged executable.'
    }
    $usage = & $tool invalid 2>&1
    if ($LASTEXITCODE -ne 2 -or "$usage" -notmatch 'Usage:') {
        throw 'Invalid arguments did not produce usage and exit code 2.'
    }
    foreach ($case in @('01-primitives', '10-annotations')) {
        $inputIdl = Join-Path $repo "docs/corpus/idl/features/$case.idl"
        $caseOutput = Join-Path $generated $case
        $output = & $tool generate $inputIdl $caseOutput 2>&1
        if ($LASTEXITCODE) { throw "Packaged generation failed for ${case}: $output" }
        foreach ($file in Get-ChildItem (Join-Path $repo "docs/corpus/7.7.0/generator/cpp/features/$case") -File) {
            $actual = Join-Path $caseOutput $file.Name
            if ((Get-FileHash $actual).Hash -ne (Get-FileHash $file.FullName).Hash) {
                throw "Packaged output differs from retained ${case}: $($file.Name)"
            }
        }
        if (@(Get-ChildItem $caseOutput -File).Count -ne 4) { throw "Expected four files for $case." }
    }

    $unsupported = Join-Path $run 'unsupported.idl'
    (Get-Content (Join-Path $repo 'docs/corpus/idl/features/10-annotations.idl') -Raw).Replace('@default(3)', '@unit("meters") @default(3)') | Set-Content $unsupported
    $errorOutput = & $tool generate $unsupported (Join-Path $run 'rejected') 2>&1
    if ($LASTEXITCODE -ne 1 -or "$errorOutput" -notmatch 'DDSGCPP001') {
        throw 'Unsupported IDL did not produce the expected source diagnostic and exit code.'
    }
    $errorOutput | Set-Content (Join-Path $run 'diagnostic.txt')
}
finally {
    $env:PATH = $savedPath
    $env:DOTNET_ROOT = $savedDotnetRoot
}

$cmake = 'cmake'
if ($IsWindows -and -not (Get-Command cmake -ErrorAction SilentlyContinue)) {
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio/Installer/vswhere.exe'
    $cmake = & $vswhere -latest -products '*' -find 'Common7/IDE/CommonExtensions/Microsoft/CMake/CMake/bin/cmake.exe' | Select-Object -First 1
    if (-not $cmake) { throw 'CMake 3.20 or later is required.' }
}
$cmakeBuild = Join-Path $run 'cmake build'
$cmakeSource = Join-Path $run 'cmake source'
New-Item $cmakeSource -ItemType Directory -Force | Out-Null
Copy-Item (Join-Path $PSScriptRoot 'CMakeLists.txt') $cmakeSource
$cmakeIdl = Join-Path $cmakeSource '01-primitives.idl'
$cmakeInclude = Join-Path $cmakeSource 'shared.idl'
Copy-Item $idl $cmakeIdl
'// Included input for incremental generation checks.' | Set-Content $cmakeInclude
Add-Content -LiteralPath $cmakeIdl -Value '#include "shared.idl"'
$cmakeGeneratorArguments = @()
if ($CMakeGenerator) { $cmakeGeneratorArguments = @('-G', $CMakeGenerator) }
$cmakeLog = & $cmake -S $cmakeSource -B $cmakeBuild @cmakeGeneratorArguments "-DWireloomCpp_DIR=$expanded/build/cmake" "-DWIRELOOM_IDL=$cmakeIdl" 2>&1
$cmakeLog | Set-Content (Join-Path $run 'cmake-configure.log')
if ($LASTEXITCODE) { throw 'Packaged CMake configuration failed.' }
$cmakeOutput = & $cmake --build $cmakeBuild 2>&1
$cmakeOutput | Set-Content (Join-Path $run 'cmake-build.log')
if ($LASTEXITCODE) { throw 'Packaged CMake generation failed.' }
$cmakeHeader = Join-Path $cmakeBuild 'wireloom/contract/01-primitives.hpp'
$cmakeTimestamp = (Get-Item $cmakeHeader).LastWriteTimeUtc
$cmakeOutput = & $cmake --build $cmakeBuild 2>&1
$cmakeOutput | Set-Content (Join-Path $run 'cmake-repeat.log')
if ($LASTEXITCODE -or "$cmakeOutput" -match 'Generating Wireloom C\+\+ sources' -or
    (Get-Item $cmakeHeader).LastWriteTimeUtc -ne $cmakeTimestamp) {
    throw 'Unchanged CMake build regenerated or failed.'
}

function Invoke-CMakeGenerationCheck([string] $Name, [bool] $ExpectGeneration, [bool] $ExpectSuccess = $true) {
    $buildOutput = & $cmake --build $cmakeBuild 2>&1
    $buildExitCode = $LASTEXITCODE
    $buildOutput | Set-Content (Join-Path $run "cmake-$Name.log")
    if (($buildExitCode -eq 0) -ne $ExpectSuccess) { throw "Unexpected CMake build result for $Name." }
    $generationCount = @($buildOutput | Select-String 'Generating Wireloom C\+\+ sources').Count
    $expectedCount = if ($ExpectGeneration) { 1 } else { 0 }
    if ($generationCount -ne $expectedCount) {
        throw "CMake $Name invoked generation $generationCount times; expected $expectedCount."
    }
}

$cmakeGeneratedFiles = @(Get-ChildItem (Split-Path $cmakeHeader -Parent) -File |
    Where-Object { $_.Extension -in @('.hpp', '.cxx') })
$cmakeHashes = @{}
foreach ($file in $cmakeGeneratedFiles) { $cmakeHashes[$file.Name] = (Get-FileHash $file.FullName).Hash }
foreach ($edit in @(
    @{ Name = 'root-touch'; Path = $cmakeIdl; TouchOnly = $true },
    @{ Name = 'root-comment'; Path = $cmakeIdl; TouchOnly = $false },
    @{ Name = 'include-touch'; Path = $cmakeInclude; TouchOnly = $true },
    @{ Name = 'include-comment'; Path = $cmakeInclude; TouchOnly = $false }
)) {
    Start-Sleep -Milliseconds 1100
    if ($edit.TouchOnly) {
        (Get-Item -LiteralPath $edit.Path).LastWriteTimeUtc = [DateTime]::UtcNow
    } else {
        Add-Content -LiteralPath $edit.Path -Value '// No change to generated C++.'
    }
    Invoke-CMakeGenerationCheck $edit.Name $true
    foreach ($file in $cmakeGeneratedFiles) {
        if ((Get-FileHash $file.FullName).Hash -ne $cmakeHashes[$file.Name]) {
            throw "Nonsemantic IDL edit changed generated contents: $($edit.Name)/$($file.Name)"
        }
    }
    Invoke-CMakeGenerationCheck "$($edit.Name)-repeat" $false
}

Start-Sleep -Milliseconds 1100
Add-Content -LiteralPath $cmakeIdl -Value 'const long PackageGenerationValue = 17;'
Invoke-CMakeGenerationCheck 'changed-output' $true
if ((Get-Content -LiteralPath $cmakeHeader -Raw) -notmatch 'PackageGenerationValue') {
    throw 'Semantic IDL edit did not update generated contents.'
}
Invoke-CMakeGenerationCheck 'changed-output-repeat' $false
foreach ($file in $cmakeGeneratedFiles) {
    Remove-Item -LiteralPath $file.FullName
    Invoke-CMakeGenerationCheck "missing-$($file.Name)" $true
    if (-not (Test-Path -LiteralPath $file.FullName)) { throw "Deleted output was not regenerated: $($file.Name)" }
    Invoke-CMakeGenerationCheck "missing-$($file.Name)-repeat" $false
}

$validIdl = Get-Content -LiteralPath $cmakeIdl -Raw
Start-Sleep -Milliseconds 1100
Add-Content -LiteralPath $cmakeIdl -Value 'struct Incomplete {'
Invoke-CMakeGenerationCheck 'invalid-input' $true $false
Invoke-CMakeGenerationCheck 'invalid-input-repeat' $true $false
Start-Sleep -Milliseconds 1100
$validIdl | Set-Content -LiteralPath $cmakeIdl
Invoke-CMakeGenerationCheck 'repaired-input' $true
Invoke-CMakeGenerationCheck 'repaired-input-repeat' $false

$installBuild = Join-Path $run 'cmake install build'
$installPrefix = Join-Path $run "installed package $packageVersion"
$installConfigure = & $cmake -S "$expanded/build/cmake" -B $installBuild "-DWIRELOOM_CPP_PUBLISH_DIR=$expanded/tools/$rid" 2>&1
$installConfigure | Set-Content (Join-Path $run 'cmake-install-configure.log')
if ($LASTEXITCODE) { throw 'Packaged CMake installer configuration failed.' }
$install = & $cmake --install $installBuild --prefix $installPrefix 2>&1
$install | Set-Content (Join-Path $run 'cmake-install.log')
if ($LASTEXITCODE) { throw 'Packaged CMake installation failed.' }
$reconfigure = & $cmake -S $cmakeSource -B $cmakeBuild "-DWireloomCpp_DIR=$installPrefix/lib/cmake/WireloomCpp" "-DWIRELOOM_IDL=$cmakeIdl" 2>&1
$reconfigure | Set-Content (Join-Path $run 'cmake-installed-configure.log')
if ($LASTEXITCODE) { throw 'Installed CMake configuration failed.' }
$clean = & $cmake --build $cmakeBuild --target clean 2>&1
$clean | Set-Content (Join-Path $run 'cmake-clean.log')
if ($LASTEXITCODE) { throw 'CMake clean failed before installed-package generation.' }
$installedOutput = & $cmake --build $cmakeBuild 2>&1
$installedOutput | Set-Content (Join-Path $run 'cmake-installed-build.log')
if ($LASTEXITCODE -or -not (Test-Path -LiteralPath $cmakeHeader)) {
    throw 'Generation through the installed CMake package failed.'
}

if ($PreviousPackage) {
    foreach ($selection in @(
        @{ Version = $previousVersion; Root = $previousExpanded; Step = 'baseline' },
        @{ Version = $packageVersion; Root = $expanded; Step = 'upgrade' },
        @{ Version = $previousVersion; Root = $previousExpanded; Step = 'rollback' }
    )) {
        $switch = & $cmake -S $cmakeSource -B $cmakeBuild "-DWireloomCpp_DIR=$($selection.Root)/build/cmake" "-DWIRELOOM_IDL=$cmakeIdl" 2>&1
        $switch | Set-Content (Join-Path $run "cmake-$($selection.Step)-configure.log")
        if ($LASTEXITCODE) { throw "CMake $($selection.Step) configuration failed." }
        $rebuild = & $cmake --build $cmakeBuild --clean-first 2>&1
        $rebuild | Set-Content (Join-Path $run "cmake-$($selection.Step)-build.log")
        if ($LASTEXITCODE -or -not (Test-Path -LiteralPath $cmakeHeader)) {
            throw "CMake $($selection.Step) generation failed."
        }
    }
}

if ($IsWindows) {
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio/Installer/vswhere.exe'
    $msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild/**/Bin/MSBuild.exe' | Select-Object -First 1
    if (-not $msbuild) { throw 'Visual Studio MSBuild is required for the native NuGet restore check.' }
    $demo = Join-Path $run 'msbuild package check'
    $projectDirectory = Join-Path $demo 'project'
    New-Item $projectDirectory -ItemType Directory -Force | Out-Null
    $copiedIdl = Join-Path $demo 'docs/corpus/idl/features/01-primitives.idl'
    New-Item (Split-Path $copiedIdl -Parent) -ItemType Directory -Force | Out-Null
    Copy-Item $idl $copiedIdl
    @'
<packages>
  <package id="Wireloom.Dds.Cpp.Generator" version="1.0.0" targetFramework="native" />
</packages>
'@.Replace('1.0.0', $packageVersion) | Set-Content (Join-Path $projectDirectory 'packages.config')
    @'
<Project DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <ItemGroup Label="ProjectConfigurations">
    <ProjectConfiguration Include="Release|x64"><Configuration>Release</Configuration><Platform>x64</Platform></ProjectConfiguration>
    <ProjectConfiguration Include="Release|ARM64"><Configuration>Release</Configuration><Platform>ARM64</Platform></ProjectConfiguration>
  </ItemGroup>
  <PropertyGroup Label="Globals"><ProjectGuid>{7F317345-53C9-4F65-A7AC-21A992C64DF0}</ProjectGuid><Keyword>Win32Proj</Keyword><RootNamespace>PackageGenerationCheck</RootNamespace></PropertyGroup>
  <Import Project="$(VCTargetsPath)\Microsoft.Cpp.Default.props" />
  <PropertyGroup Label="Configuration"><ConfigurationType>Utility</ConfigurationType><UseDebugLibraries>false</UseDebugLibraries><PlatformToolset>v143</PlatformToolset></PropertyGroup>
  <Import Project="$(VCTargetsPath)\Microsoft.Cpp.props" />
  <PropertyGroup><IntDir>$(MSBuildProjectDirectory)\obj\$(Configuration)\</IntDir><OutputPath>$(MSBuildProjectDirectory)\bin\$(Configuration)\$(Platform)\</OutputPath><OutDir>$(OutputPath)</OutDir></PropertyGroup>
  <ItemGroup><WireloomIdl Include="..\docs\corpus\idl\features\01-primitives.idl" /></ItemGroup>
  <Import Project="..\packages\Wireloom.Dds.Cpp.Generator.1.0.0\build\native\Wireloom.Dds.Cpp.Generator.props" Condition="Exists('..\packages\Wireloom.Dds.Cpp.Generator.1.0.0\build\native\Wireloom.Dds.Cpp.Generator.props')" />
  <Import Project="..\packages\Wireloom.Dds.Cpp.Generator.1.0.0\build\native\Wireloom.Dds.Cpp.Generator.targets" Condition="Exists('..\packages\Wireloom.Dds.Cpp.Generator.1.0.0\build\native\Wireloom.Dds.Cpp.Generator.targets')" />
  <Import Project="$(VCTargetsPath)\Microsoft.Cpp.targets" />
</Project>
'@.Replace('1.0.0', $packageVersion) | Set-Content (Join-Path $projectDirectory 'package-generation-check.vcxproj')
    $config = Join-Path $demo 'NuGet.Config'
    $localFeed = Join-Path $demo 'feed'
    New-Item $localFeed -ItemType Directory -Force | Out-Null
    Copy-Item -LiteralPath $packagePath -Destination $localFeed
    if ($PreviousPackage) { Copy-Item -LiteralPath $previousPackagePath -Destination $localFeed }
    $feed = [Security.SecurityElement]::Escape($localFeed)
    # Only the locally built unsigned candidate is accepted; no external feed is configured.
    "<configuration><packageSources><clear/><add key=`"candidate`" value=`"$feed`"/></packageSources><config><add key=`"signatureValidationMode`" value=`"accept`"/><add key=`"repositoryPath`" value=`"packages`"/></config></configuration>" | Set-Content $config
    $project = Join-Path $projectDirectory 'package-generation-check.vcxproj'
    $toolsetArguments = @()
    if ($PlatformToolset) { $toolsetArguments = @("-p:PlatformToolset=$PlatformToolset") }
    $restore = & $msbuild $project @toolsetArguments -t:Restore -p:RestorePackagesConfig=true "-p:RestoreConfigFile=$config" -p:Configuration=Release -p:Platform=x64 -v:minimal 2>&1
    $restore | Set-Content (Join-Path $run 'restore.log')
    if ($LASTEXITCODE) { throw "Native NuGet restore failed. See $run/restore.log" }
    try {
        $env:PATH = Join-Path $env:SystemRoot 'System32'
        $env:DOTNET_ROOT = Join-Path $run 'absent-dotnet'
        $generationArguments = @($project, '-t:WireloomCppGenerate', '-p:Configuration=Release', '-p:Platform=ARM64', '-v:normal') + $toolsetArguments
        $first = & $msbuild @generationArguments 2>&1
        $first | Set-Content (Join-Path $run 'generation.log')
        if ($LASTEXITCODE) { throw 'Restored MSBuild integration failed generation.' }
        $header = Join-Path $projectDirectory 'obj/Release/wireloom/01-primitives.hpp'
        $timestamp = (Get-Item $header).LastWriteTimeUtc
        $repeat = & $msbuild @generationArguments 2>&1
        $repeat | Set-Content (Join-Path $run 'repeat.log')
        if ($LASTEXITCODE -or (Get-Item $header).LastWriteTimeUtc -ne $timestamp) {
            throw 'Unchanged MSBuild input regenerated or failed.'
        }
        if ($PreviousPackage) {
            $projectTemplate = Get-Content -LiteralPath $project -Raw
            $packagesConfigPath = Join-Path $projectDirectory 'packages.config'
            $packagesTemplate = Get-Content -LiteralPath $packagesConfigPath -Raw
            foreach ($selection in @(
                @{ Version = $previousVersion; Step = 'baseline' },
                @{ Version = $packageVersion; Step = 'upgrade' },
                @{ Version = $previousVersion; Step = 'rollback' }
            )) {
                $projectTemplate.Replace($packageVersion, $selection.Version) | Set-Content $project
                $packagesTemplate.Replace($packageVersion, $selection.Version) | Set-Content $packagesConfigPath
                $restore = & $msbuild $project @toolsetArguments -t:Restore -p:RestorePackagesConfig=true "-p:RestoreConfigFile=$config" -p:Configuration=Release -p:Platform=ARM64 -v:minimal 2>&1
                $restore | Set-Content (Join-Path $run "msbuild-$($selection.Step)-restore.log")
                if ($LASTEXITCODE) { throw "MSBuild $($selection.Step) restore failed." }
                $rebuildArguments = @($project, '-t:Clean;WireloomCppGenerate', '-p:Configuration=Release', '-p:Platform=ARM64', '-v:normal') + $toolsetArguments
                $rebuild = & $msbuild @rebuildArguments 2>&1
                $rebuild | Set-Content (Join-Path $run "msbuild-$($selection.Step)-build.log")
                if ($LASTEXITCODE -or -not (Test-Path -LiteralPath $header)) {
                    throw "MSBuild $($selection.Step) generation failed."
                }
            }
        }
    }
    finally {
        $env:PATH = $savedPath
        $env:DOTNET_ROOT = $savedDotnetRoot
    }
}

[ordered]@{
    package = [IO.Path]::GetFileName($packagePath)
    packageVersion = $packageVersion
    upgradeAndRollback = if ($PreviousPackage) { "passed; $previousVersion -> $packageVersion -> $previousVersion" } else { 'not requested; supply -PreviousPackage' }
    sha256 = (Get-FileHash $packagePath -Algorithm SHA256).Hash.ToLowerInvariant()
    runtimeIdentifier = $rid
    nativeGeneration = 'passed'
    retainedOutput = 'byte-identical; 01-primitives and 10-annotations'
    unsupportedInput = 'rejected'
    cmakeGeneration = 'passed; nonsemantic root/include edits settle; semantic edits and missing outputs regenerate; failed generation retries; installed package clean rebuild passed'
    cmakeGenerator = if ($CMakeGenerator) { $CMakeGenerator } else { 'host default' }
    msbuildRestoreAndGeneration = if ($IsWindows) { 'passed; ARM64 target on x64 host; repeat skipped' } else { 'Windows-only check' }
} | ConvertTo-Json | Set-Content (Join-Path $run 'result.json')
Write-Host "Native package verification passed: $run"
