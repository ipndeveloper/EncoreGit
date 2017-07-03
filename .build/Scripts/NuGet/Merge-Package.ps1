Param()
Begin
{
    Push-Location -Path:(Split-Path -Path:$PSCommandPath -Parent);
    &..\Diagnostics\Push-Log.ps1 -Path:'..\..\..\Merge-Package.{0:MM.dd.yyyy.HH}.log'
    Set-Variable -Scope:'Script' -Name:'Base' -Value:'..\..\..\';
    &..\Diagnostics\Write-Log.ps1 -Message:'Begin Merge-Package';
}
Process
{
    $solution = @{
        Packages = @{};
        Projects = @();
    }
    ..\Diagnostics\Write-Log.ps1 -Message:'Checking Solution Package Configurations'
    &.\..\Get-Item.ps1 -Name:'packages.config' | ForEach-Object {
       Push-Location -Path:(Split-Path -Path:$PSCommandPath -Parent);
       $project = @{
	            Packages = @{
                    Path = $_.FileNames(0);
                    Xml = [xml](Get-Content -Path:$_.FileNames(0));
                }
	            Container = $_.ContainingProject;
                Name = $_.ContainingProject.Name;
                Xml = [xml](Get-Content -Path:$_.ContainingProject.FullName);
	        }
        $solution.Projects += $project;
        foreach($node in $project.Packages.Xml.DocumentElement.SelectNodes('//package'))
        {
            $duplicates = $project.Packages.Xml.DocumentElement.SelectNodes(('//package[@id="{0}"]' -f $node.Attributes['id'].Value));
            if($duplicates.Count -gt 1)
            {
                $duplicates = $duplicates | Sort-Object @{Expression={$_.Attributes['version'].Value};Descending=$true};
	            $options = @{('&{0}-Skip' -f 0)=$null}
	            foreach($duplicate in $duplicates)
	            {
	                $options.Add(('&{0}: {1}' -f $options.Count, $duplicate.Attributes['version'].Value),$duplicate)
	            }
                &..\UI\Read-Choice.ps1 -Title:'Duplicate package reference found' -Message:('{0} is referenced multiple times in {1}' -f $node.Attributes['id'].Value,$project.Packages.Path) -Options:$options -Default:1 -Not | ForEach-Object {
	                if([Boolean]$_)
	                {
						&.\..\Diagnostics\Write-Log.ps1 -Message:'{0} was removed from {1}' -Arguments:@($_.OuterXml,$project.Packages.Path);
	                    $_.ParentNode.RemoveChild($_) | Out-Null;
                        $project.Packages.Xml.Save($project.Packages.Path) | Out-Null
                        &.\..\Diagnostics\Write-Log.ps1 -Level:'Verbose' -Message:'Changes saved to {0}' -Arguments:@($project.Packages.Path);
	                }
	            };
            }
        }
        ..\Diagnostics\Write-Log.ps1 -Message:'Evaluating {0}' -Arguments:@($project.Packages.Path);
        foreach($node in $project.Packages.Xml.DocumentElement.SelectNodes('//package'))
        {
            $id = $node.Attributes['id'].Value;
            $version = $node.Attributes['version'].Value;
            $package = $null;
            if($solution.Packages.ContainsKey($id))
            {
                $package = $solution.Packages[$id];
            }
            else
            {
                ..\Diagnostics\Write-Log.ps1 -Level:'Verbose' -Message:'Adding package {0}' -Arguments:@($id);
                $package = @{
                    Id=$id;
                    Versions = @{};
                }
                $solution.Packages.Add($id,$package)
            }
            if($package.Versions.ContainsKey($version))
            {
                $package.Versions[$version].Projects += $project
            }
            else
            {
                ..\Diagnostics\Write-Log.ps1 -Level:'Verbose' -Message:'Adding {0} version {1}' -Arguments:@($id,$version);
                $segments = $version.Split('.');
                $major = 0;
                $minor = 0;
                $build = 0;
                $revision= 0
                if($segments.Length -gt 0)
                {
                    if($segments[0] -match '^\d+$')
                    {
                        $major = $segments[0]
                    }
                }
                if($segments.Length -gt 1)
                {
                    if($segments[1] -match '^\d+$')
                    {
                        $minor = $segments[1]
                    }
                }
                if($segments.Length -gt 2)
                {
                    if($segments[2] -match '^\d+$')
                    {
                        $build = $segments[2]
                    }
                }
                if($segments.Length -gt 3)
                {
                    if($segments[3] -match '^\d+$')
                    {
                        $revision = $segments[3]
                    }
                }
                $package.Versions.Add($version,@{
                    Id=$version;
                    Projects = @($project)
                    Major = $major
                    Minor = $minor
                    Build = $build
                    Revision = $revision
                    Full= New-Object -TypeName:System.Version -ArgumentList:@($major,$minor,$build,$revision)
                });
            }
        }
        Pop-Location;
    }
    ..\Diagnostics\Write-Log.ps1 -Message:'Checking for version conflicts';
    foreach($package in $solution.Packages.Keys)
    {
        $package = $solution.Packages[$package];
        if($package.Versions.Count -gt 1)
        {
            $options = @{('&{0}-Skip' -f 0)=$null}
            $versions = $package.Versions.Values | Sort-Object @{Expression={$_.Full};Descending=$true}
            $message = New-Object -TypeName:System.Text.StringBuilder;
            $message.AppendLine() | Out-Null;
	        foreach($version in $versions)
            {
                $options.Add(('&{0}: {1} ({2})' -f $options.Count, $version.id,$version.Projects.Count),$version)
                $message.AppendLine($version.id) | Out-Null;
                foreach($project in $version.Projects)
                {
                    $message.Append(' ') | Out-Null;
                    $message.AppendLine($project.Name) | Out-Null;
                }
            }
            $selected = &..\UI\Read-Choice.ps1 -Title:'Package version conflict discovered' -Message:('{0} is referenced as the following versions:{1}' -f $package.Id,$message) -Options:$options -Default:1;
            if([Boolean]$selected)
            {
                ..\Diagnostics\Write-Log.ps1 -Message:'Upgrading to {0} {1}' -Arguments:@($package.Id,$selected.Id);
                foreach($version in $package.Versions.Keys)
                {
                    $version = $package.Versions[$version];
                    if($version.Id -ne $selected.Id)
                    {
                        foreach($project in $version.Projects)
                        {
                            if(-not (($version.Major -eq $selected.Major) -and ($version.Minor -eq $selected.Minor)))
                            {
                                &.\..\Diagnostics\Write-Log.ps1 -Level:'Warning' -Message:'{0} is upgrading from {1} to {2} which may contain breaking changes' -Arguments:@($project.Name,$version.Id,$selected.Id);
                            }
                        }
                    }
                }
				..\Diagnostics\Write-Log.ps1 -Message:'Reinstalling versions of {0} that do not match: {1}' -Arguments:@($package.Id,$selected.Id);
				.\..\NuGet\Reinstall-Package.ps1 -Id:$package.Id -Version:$selected.Id -IgnoreDependencies;
            }
        }
    }
    &..\Diagnostics\Write-Log.ps1 -Message:'Package changes have been successfully merged run build command to continue';
}
End
{
    &..\Diagnostics\Write-Log.ps1 -Message:'End Merge-Package';
    &..\Diagnostics\Pop-Log.ps1
    Pop-Location;
}