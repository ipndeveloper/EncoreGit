Param([Parameter(Mandatory=$true)]
    [String]$Id,
    [String]$Version,
    [String]$ProjectName,
    [Switch]$IgnoreDependencies
)
Begin
{
    Push-Location -Path:(Split-Path -Path:$PSCommandPath -Parent);
}
Process
{
    if([Boolean]$ProjectName)
    {
        Uninstall-Package -Id:$Id -ProjectName:$ProjectName -Force:$IngoreDependencies -ErrorAction:Continue 
        Install-Package -Id:$Id -Version:$Version -ProjectName:$ProjectName -IgnoreDependencies:$IgnoreDependencies -ErrorAction:Continue
    }
    else
    {
		$projects = Get-Project -all | where { Get-Package -project $_.Name| Where-Object { $_.Id -eq $Id -and $_.Version -ne $Version } } | Select-Object -Property Name
		foreach($project in $projects){ Uninstall-Package -id:$Id -projectName:$project.Name -force -ErrorAction:Continue }
		foreach($project in $projects){ Install-Package -id:$Id -projectName:$project.Name -version:$Version -ignoredependencies:$IgnoreDependencies -ErrorAction:Continue }
    }
}
End
{
    Pop-Location;
}