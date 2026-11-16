param(
    [Parameter(Mandatory=$true)][string]$OldProject,
    [Parameter(Mandatory=$true)][string]$NewProject,
    [switch]$Apply
)
$ErrorActionPreference = 'Stop'
$sourceProject = (Resolve-Path -LiteralPath $OldProject).Path
$targetProject = (Resolve-Path -LiteralPath $NewProject).Path
$destinationRoot = Join-Path $targetProject 'Files'
$sources = @(
    (Join-Path $sourceProject 'Files'),
    (Join-Path $sourceProject 'bin\Debug\net8.0\Files'),
    (Join-Path $sourceProject 'bin\Release\net8.0\Files')
)
$copied = 0; $missing = 0; $conflicts = 0
foreach ($source in $sources) {
    if (-not (Test-Path -LiteralPath $source -PathType Container)) {
        Write-Host "Folder absent: $source"
        continue
    }
    $sourceRoot = (Resolve-Path -LiteralPath $source).Path.TrimEnd('\')
    foreach ($item in Get-ChildItem -LiteralPath $sourceRoot -File -Recurse) {
        if (($item.Attributes -band [IO.FileAttributes]::ReparsePoint) -ne 0) { continue }
        $relative = $item.FullName.Substring($sourceRoot.Length).TrimStart('\')
        $destination = Join-Path $destinationRoot $relative
        if (Test-Path -LiteralPath $destination) {
            $existingHash = (Get-FileHash -LiteralPath $destination -Algorithm SHA256).Hash
            $sourceHash = (Get-FileHash -LiteralPath $item.FullName -Algorithm SHA256).Hash
            if ($sourceHash -ne $existingHash) {
                $conflicts++
                Write-Warning "Different file already exists; left untouched: $destination"
            }
            continue
        }
        $missing++
        if ($Apply) {
            New-Item -ItemType Directory -Path (Split-Path -Parent $destination) -Force | Out-Null
            Copy-Item -LiteralPath $item.FullName -Destination $destination
            $copied++
            Write-Host "Copied: $relative"
        } else { Write-Host "Would copy: $relative" }
    }
}
Write-Host "Files found for copying: $missing; copied: $copied; conflicts left untouched: $conflicts"
if (-not $Apply) { Write-Host 'Preview only. Run again with -Apply to copy. Nothing was deleted or changed.' }
