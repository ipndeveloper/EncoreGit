Param([String]$Id,[String]$Version,[Switch]$Not)
Begin
{
    Push-Location -Path:(Split-Path -Path:$PSCommandPath -Parent);
}
Process
{
    .\..\Get-Item.ps1 -Name:'packages.config' | ForEach-Object {
        $packages = [xml](Get-Content -Path:$_.FileNames(0));
        $project = $_.ContainingProject
        $xpath = ('//package[@id="{0}"]' -f $Id)
        if($Version -ne '')
        {
            $xpath = ('//package[@id="{0}" and @version = "{1}" ]' -f $Id,$Version)
            if($Not)
            {
                $xpath = ('//package[@id = "{0}" and @version != "{1}" ]' -f $Id,$Version)
            }
        }
        $node = $packages.SelectSingleNode($xpath);
        if([Boolean]$node)
        {
            $location = Pop-Location;
            Write-Output @{
                Project=$project;
                Version=$node.Attributes['version'].Value;
            }
            Push-Location $location;
        }
    }
}
End
{
    Pop-Location;
}