Param($Container=$dte.Solution,[String]$Project='*',[String]$Name= '*')
Begin
{
	Push-Location -Path:(Split-Path -Path:$PSCommandPath -Parent);
}
Process
{
    Function Recurse
    {
        Param($Container)
	    foreach($item in $Container.ProjectItems)
	    {
            if($item.ContainingProject.Name -like $Project)
            {
		        if($item.Name -like $Name)
		        {
			        Write-Output $item;
		        }
            }
		    if([Boolean]$item.SubProject)
		    {
			    Recurse -Container:$item.SubProject;
		    }
	    }
    }
    Recurse -Container:$Container
}
End
{
	Pop-Location;
}