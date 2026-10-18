# Shared evidence helpers. Hashes identify bytes, not claims of compatibility.
function Write-EvidenceJson {
    param([string]$Path, [object]$Value)
    [IO.File]::WriteAllText($Path, ($Value | ConvertTo-Json -Depth 30) + "`n", [Text.UTF8Encoding]::new($false))
}

function Get-EvidenceSnapshot {
    param([string]$Root, [string[]]$Paths)
    $files = foreach ($path in $Paths) {
        $item = Get-Item -LiteralPath (Join-Path $Root $path)
        if ($item.PSIsContainer) {
            Get-ChildItem -LiteralPath $item.FullName -Recurse -File |
                Where-Object { $_.FullName -notmatch '[\\/](obj|bin)[\\/]' }
        }
        else { $item }
    }
    $hashes = [Collections.Generic.SortedDictionary[string, string]]::new([StringComparer]::Ordinal)
    foreach ($file in $files) {
        $relative = [IO.Path]::GetRelativePath($Root, $file.FullName).Replace('\', '/')
        $hashes[$relative] = (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash.ToLowerInvariant()
    }
    $entries = @(foreach ($entry in $hashes.GetEnumerator()) {
        [ordered]@{ path = $entry.Key; sha256 = $entry.Value }
    })
    $canonical = ($entries | ForEach-Object { "$($_.path)`t$($_.sha256)" }) -join "`n"
    $digest = [Security.Cryptography.SHA256]::HashData([Text.Encoding]::UTF8.GetBytes($canonical))
    [ordered]@{
        sha256 = [Convert]::ToHexString($digest).ToLowerInvariant()
        files = $entries
    }
}

function Get-EvidenceRevision {
    param([string]$Root)
    $revision = & git -C $Root rev-parse HEAD
    if ($LASTEXITCODE -ne 0) { throw 'Cannot identify source revision.' }
    $status = @(& git -C $Root status --porcelain --untracked-files=normal)
    if ($LASTEXITCODE -ne 0) { throw 'Cannot identify working-tree state.' }
    [ordered]@{ commit = "$revision"; workingTreeDirty = $status.Count -gt 0 }
}
