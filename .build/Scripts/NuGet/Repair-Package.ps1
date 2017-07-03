Param([Parameter(Mandatory=$true)]
    [String]$Id,
    [Parameter(Mandatory=$true)]
    [String]$Version,
    [Parameter(Mandatory=$true)]
    $Project
)
Begin
{
    Push-Location -Path:(Split-Path -Path:$PSCommandPath -Parent);
}
Process
{
    $found = $false;
    $Configuration = $null;
    foreach($item in $Project.ProjectItems)
    {
        if($item.Name = 'packages.config')
        {
            $Configuration = [xml](Get-Content -Path:$item.FileNames(0));
            break;
        }
    }

    if($Configuration -ne $null)
    {
        foreach($node in $Configuration.DocumentElement.SelectNodes(('//package[@id="{0}"]' -f $Id,$Version)))
        {
            if(-not $found -and $node.Attributes['version'].Value -eq $Version)
            {
                $found = $true;
            }
            else
            {
                Write-Host ('Reparing {0} removing element {1} from configuration' -f $Id, $node.OuterXml)
	            $node.ParentNode.RemoveChild($node) | Out-Null;
                $Configuration.Save($Project.Packages.Path) | Out-Null
            }
        }
    }
}
End
{
    Pop-Location;
}