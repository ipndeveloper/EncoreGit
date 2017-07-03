Param([String]$Repository,[String]$Path)
Begin
{
    Push-Location -Path:(Split-Path -Path:$PSCommandPath -Parent);
}
Process
{
    Write-Host ('Restoring {0} to {1}' -f $Path,$Repository);
    Invoke-Expression -Command:('..\..\3rdParty\nuget\nuget.exe install "{0}" -OutputDirectory "{1}" -NonInteractive' -f $Path,$Repository);
}
End
{
    Pop-Location;
}