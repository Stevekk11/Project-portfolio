param(
    [switch]$Clean = $true
)

$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $projectRoot

$python = if (Test-Path '.\.venv\Scripts\python.exe') {
    (Resolve-Path '.\.venv\Scripts\python.exe').Path
} else {
    'python'
}

if ($Clean) {
    Remove-Item '.\build' -Recurse -Force -ErrorAction SilentlyContinue
    Remove-Item '.\dist' -Recurse -Force -ErrorAction SilentlyContinue
}

& $python -m pip install --upgrade pip
& $python -m pip install -r '.\requirements.txt'
& $python -m PyInstaller --noconfirm '.\app.spec'

Write-Host ''
Write-Host 'Build dokončen.' -ForegroundColor Green
Write-Host 'Portable složka:' (Join-Path $projectRoot 'dist\YOLODetectionStudio')
Write-Host 'Spustitelný soubor:' (Join-Path $projectRoot 'dist\YOLODetectionStudio\YOLODetectionStudio.exe')

