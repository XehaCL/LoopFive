param([switch]$RunTests)
$ErrorActionPreference = 'Stop'
& (Join-Path $PSScriptRoot 'src/BUILD.ps1') -RunTests:$RunTests
