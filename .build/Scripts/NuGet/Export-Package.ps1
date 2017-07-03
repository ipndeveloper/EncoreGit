Param([String]$Repository,[String]$Name='*',$Path = 'nupkg')
Begin
{
    Push-Location -Path:(Split-Path -Path:$PSCommandPath -Parent);
    Import-Module -Name:'..\..\Modules\PSCX\PSCX.psd1';
}
Process
{
    Get-ChildItem -Path:$Repository -Include:"$Name.nupkg" -Recurse | ForEach-Object {
        $destination = Join-Path -Path:$_.Directory -ChildPath:$Path
        if(-not (Test-Path -Path:$destination))
        {
            Write-Host ('Exporting {0} to {1}' -f $_.Name,$destination);
            New-Item -Path:$destination -Type:'Directory' -Force | Out-Null;
            Expand-Archive -Path:$_.FullName -OutputPath:$destination -Force | Out-Null;
            Write-Output $destination;
        }
    }
}
End
{
    Pop-Location;
}