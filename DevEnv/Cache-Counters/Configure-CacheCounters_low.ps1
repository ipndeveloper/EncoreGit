param([string]$path)
Start-Process -FilePath "cmd.exe" -Verb Runas -ArgumentList "/C powershell.exe -file $path\Configure-CacheCounters_high.ps1 $path" -WorkingDirectory $path