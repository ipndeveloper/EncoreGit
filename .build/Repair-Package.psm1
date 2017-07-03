function Repair-Package([string]$Id, [string]$Version, [Switch]$IgnoreDependencies, [Switch]$IncludePrerelease)
{
	$projectsToRestore = New-Object System.Collections.ArrayList
	$projects = Get-Project -All
	foreach($project in $projects)
	{
		Write-Host "Checking Project: " $project.ProjectName
		$package = Get-Package -ProjectName $project.ProjectName -ErrorAction SilentlyContinue | Where-Object { $_.Id -eq $Id } 
		if($package -ne $null)
		{
            Write-Host "    Uninstalling $Id"
			Uninstall-Package $Id -ProjectName $project.ProjectName -Force
			[void]$projectsToRestore.Add($project)
		}
	}
	foreach($project in $projectsToRestore)
	{
        Write-Host "Restoring $Id $Version to " $project.ProjectName
		Install-Package $Id -ProjectName $project.ProjectName -Version $Version -IgnoreDependencies:$IgnoreDependencies -IncludePrerelease:$IncludePrerelease | Out-Null
	}
}

function Uninstall-PackageFromSolution([string]$Id)
{
	$projects = Get-Project -All
	foreach($project in $projects)
	{
  		$package = Get-Package -ProjectName $project.ProjectName -ErrorAction SilentlyContinue | Where-Object { $_.Id -eq $Id }
		if($package -ne $null)
		{
			Uninstall-Package $Id -ProjectName $project.ProjectName -Force 
		}
	}
}
