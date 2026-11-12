[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('win-x64', 'linux-x64')]
    [string] $RuntimeIdentifier,

    [string] $OutputRoot = (Join-Path $PSScriptRoot '..\artifacts\cpp-native-package'),

    [ValidateSet('Debug', 'Release')]
    [string] $Configuration = 'Release',

    [string] $PackageVersion
)

$ErrorActionPreference = 'Stop'

$isWindowsHost = [System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform(
    [System.Runtime.InteropServices.OSPlatform]::Windows)
$isLinuxHost = [System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform(
    [System.Runtime.InteropServices.OSPlatform]::Linux)
$hostArchitecture = [System.Runtime.InteropServices.RuntimeInformation]::OSArchitecture

if ($hostArchitecture -ne [System.Runtime.InteropServices.Architecture]::X64) {
    throw "Publishing '$RuntimeIdentifier' requires an x64 build host; found '$hostArchitecture'."
}

if ($RuntimeIdentifier.StartsWith('win-') -and -not $isWindowsHost) {
    throw "Publishing '$RuntimeIdentifier' requires a Windows build host."
}

if ($RuntimeIdentifier.StartsWith('linux-') -and -not $isLinuxHost) {
    throw "Publishing '$RuntimeIdentifier' requires a Linux build host."
}

$projectPath = Join-Path $PSScriptRoot '..\src\Wireloom.Dds.Cpp.Generator\Wireloom.Dds.Cpp.Generator.csproj'
$resolvedProject = (Resolve-Path -LiteralPath $projectPath).Path
if (-not $PackageVersion) {
    [xml] $buildProperties = Get-Content -LiteralPath (Join-Path $PSScriptRoot '../Directory.Build.props') -Raw
    $PackageVersion = ([string] $buildProperties.Project.PropertyGroup.WireloomVersion).Trim()
}
if ($PackageVersion -notmatch '^\d+\.\d+\.\d+(?:-[0-9A-Za-z.-]+)?$') {
    throw "Invalid package version: '$PackageVersion'."
}
$resolvedRoot = [System.IO.Path]::GetFullPath($OutputRoot)
$publishDirectory = Join-Path $resolvedRoot "publish\$RuntimeIdentifier"
$toolDirectory = Join-Path $resolvedRoot "tools\$RuntimeIdentifier"
$metadataDirectory = Join-Path $resolvedRoot "metadata\$RuntimeIdentifier"
$null = New-Item -ItemType Directory -Path $publishDirectory -Force
$null = New-Item -ItemType Directory -Path $toolDirectory -Force
$null = New-Item -ItemType Directory -Path $metadataDirectory -Force

Write-Host "Publishing $RuntimeIdentifier NativeAOT executable to $publishDirectory"
Push-Location (Split-Path -Parent $PSScriptRoot)
try {
    $publishOutput = & dotnet publish $resolvedProject `
        --configuration $Configuration `
        --output $publishDirectory `
        -p:WireloomCppNativeAot=true `
        -p:WireloomCppRuntimeIdentifier=$RuntimeIdentifier `
        -p:Version=$PackageVersion `
        -p:PackageVersion=$PackageVersion `
        -p:RestoreLockedMode=true `
        -p:DebugType=none 2>&1
}
finally {
    Pop-Location
}
$publishExitCode = $LASTEXITCODE
$publishLog = Join-Path $metadataDirectory 'publish.log'
$publishOutput | Set-Content -LiteralPath $publishLog -Encoding utf8

if ($publishExitCode -ne 0) {
    $publishOutput | Write-Host
    throw "dotnet publish failed with exit code $publishExitCode. See $publishLog."
}

$executableName = if ($RuntimeIdentifier.StartsWith('win-')) {
    'Wireloom.Dds.Cpp.exe'
}
else {
    'Wireloom.Dds.Cpp'
}

$executablePath = Join-Path $publishDirectory $executableName
if (-not (Test-Path -LiteralPath $executablePath -PathType Leaf)) {
    throw "Expected NativeAOT executable was not produced: $executablePath"
}

# Native AOT publish output must contain only the executable and native support files.
$null = Copy-Item -LiteralPath $executablePath -Destination (Join-Path $toolDirectory $executableName) -Force
$packagedExecutablePath = Join-Path $toolDirectory $executableName

$hashes = @([ordered]@{
    path = "tools/$RuntimeIdentifier/$executableName"
    sha256 = (Get-FileHash -LiteralPath $packagedExecutablePath -Algorithm SHA256).Hash.ToLowerInvariant()
    length = (Get-Item -LiteralPath $packagedExecutablePath).Length
})

$manifest = [ordered]@{
    packageVersion = $PackageVersion
    runtimeIdentifier = $RuntimeIdentifier
    configuration = $Configuration
    project = [System.IO.Path]::GetRelativePath((Split-Path -Parent $PSScriptRoot), $resolvedProject).Replace('\', '/')
    executable = $executableName
    warnings = @($publishOutput | Where-Object { $_ -match 'warning ' })
    files = @($hashes)
}
$manifest | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $metadataDirectory 'manifest.json') -Encoding utf8

Write-Host "Published $executableName ($((Get-Item -LiteralPath $packagedExecutablePath).Length) bytes)."
if ($manifest.warnings.Count -gt 0) {
    Write-Warning "Publish completed with $($manifest.warnings.Count) warning(s); see publish.log and manifest.json."
}
