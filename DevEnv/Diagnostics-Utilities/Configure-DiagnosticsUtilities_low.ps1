param([string]$path)
Start-Process -FilePath "cmd.exe" -Verb Runas -ArgumentList "/C powershell.exe -file $path\Configure-DiagnosticsUtilities_high.ps1 $path" -WorkingDirectory $path