param([string]$path)
. C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe $path\NetSteps.Core.Cache.dll
Write-Host "Press any key to continue..." -ForegroundColor White
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
