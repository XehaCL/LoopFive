param([switch]$RunTests)
$ErrorActionPreference = 'Stop'
$compiler = Join-Path $env:WINDIR 'Microsoft.NET/Framework64/v4.0.30319/csc.exe'
if (-not (Test-Path -LiteralPath $compiler)) { throw 'Se requiere Windows de 64 bits con .NET Framework 4.x.' }
$buildDirectory = Join-Path $PSScriptRoot 'build'
New-Item -ItemType Directory -Force -Path $buildDirectory | Out-Null
$excluded = @('Tests','ExtendedTests','UiTests','Preview','KeyboardTests','AudioQualityTests','CleanupTests','ThemeTests','FxQualityTests','LanguageTests','OptionsLayoutTests','FxEditingTests','ResearchDspTests','ResearchUiTests','HardwareUiTests','RecordingSourceTests')
$sourceFiles = @(Get-ChildItem -LiteralPath $PSScriptRoot -Filter '*.cs' | Where-Object { $_.BaseName -notin $excluded } | ForEach-Object { $_.FullName })
$appFile = Join-Path $buildDirectory 'LoopFive.exe'
$arguments = @('/nologo','/target:winexe','/platform:x64','/optimize+','/reference:System.Runtime.Serialization.dll','/reference:System.Windows.Forms.dll','/reference:System.Drawing.dll',('/out:' + $appFile),('/win32icon:' + (Join-Path $PSScriptRoot 'app.ico')),('/win32manifest:' + (Join-Path $PSScriptRoot 'app.manifest'))) + $sourceFiles
& $compiler @arguments
if ($LASTEXITCODE -ne 0) { throw 'No se pudo compilar la aplicación.' }
Write-Output "Aplicación compilada: $appFile"
if ($RunTests) {
 $coreNames = @('Model.cs','Dsp.cs','Engine.cs','Rhythm.cs','Storage.cs','NativeAudio.cs','Midi.cs','MidiPattern.cs','Demo.cs','CaptureQueue.cs','FxDetail.cs','FxAlgorithms.cs','RecordingSource.cs')
 $coreSources = @($coreNames | ForEach-Object { Join-Path $PSScriptRoot $_ })
 $core = Join-Path $buildDirectory 'Core.dll'
 $arguments = @('/nologo','/target:library','/platform:x64','/optimize+','/reference:System.Runtime.Serialization.dll',('/out:' + $core)) + $coreSources
 & $compiler @arguments
 if ($LASTEXITCODE -ne 0) { throw 'No se pudo compilar el motor de pruebas.' }
 foreach ($test in @('Tests','ExtendedTests','UiTests','KeyboardTests','AudioQualityTests','CleanupTests','ThemeTests','FxQualityTests','LanguageTests','OptionsLayoutTests','FxEditingTests','ResearchDspTests','ResearchUiTests','HardwareUiTests','RecordingSourceTests')) {
  $target = Join-Path $buildDirectory ($test + '.exe')
  $reference = if ($test -in @('UiTests','KeyboardTests','ThemeTests','LanguageTests','OptionsLayoutTests','FxEditingTests','ResearchDspTests','ResearchUiTests','HardwareUiTests','RecordingSourceTests')) { $appFile } else { $core }
  $arguments = @('/nologo','/platform:x64',('/reference:' + $reference),'/reference:System.Windows.Forms.dll','/reference:System.Drawing.dll',('/out:' + $target),(Join-Path $PSScriptRoot ($test + '.cs')))
  & $compiler @arguments
  if ($LASTEXITCODE -ne 0) { throw "No se pudo compilar $test." }
  Push-Location $buildDirectory
  try { & $target; if ($LASTEXITCODE -ne 0) { throw "Falló $test." } } finally { Pop-Location }
 }
}
