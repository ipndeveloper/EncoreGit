param(
    [Parameter(Mandatory=$true, Position=1)][string]$where
    , [Parameter(Mandatory=$false, Position=2)][Switch]$isAdmin
)

if($isAdmin)
{
    $mod = Join-Path -Path (Split-Path $PSCommandPath) -ChildPath "ClientRoot.psm1"
    Import-Module $mod
    if((Test-NSCLientInfo $where) -eq $false)
    {
        Enter-NSClientINfo $where
    }
    Set-ClientIISRoot $where
    Write-Host "Press any key to continue..." -ForegroundColor White
    $x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
}
else
{
    Start-Process -FilePath "cmd.exe" -Verb Runas -ArgumentList "/C powershell.exe -file $PSCommandPath -where $where -isAdmin" -WorkingDirectory $where
}

