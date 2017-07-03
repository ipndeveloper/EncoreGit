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
    $path = $null;
    $configuration = $null;
    foreach($item in $Project.ProjectItems)
    {
        if($item.Name -eq 'packages.config')
        {
            $path = $item.FileNames(0);
            $configuration = [xml](Get-Content -Path:$path);
            break;
        }
    }

    if($configuration -ne $null)
    {
        $nodes = $configuration.DocumentElement.SelectNodes(('//package[@id="{0}"]' -f $Id,$Version))
        if($nodes.Count -gt 0)
        {
            foreach($node in $nodes)
            {
                if(-not $found -and $node.Attributes['version'].Value -eq $Version)
                {
                    $found = $true;
                }
                else
                {
                    Write-Host ('Reparing {0} removing element {1} from configuration in {2}' -f $Id, $node.OuterXml,$path)
	                $node.ParentNode.RemoveChild($node) | Out-Null;
                    $configuration.Save($path) | Out-Null
                }
            }
        }
    }
}
End
{
    Pop-Location;
}