Param([Parameter(Position=0, 
    Mandatory=$true, 
    ValueFromPipeline=$true,
    ValueFromPipelineByPropertyName=$true)]
    $Project,[String]$Name='*',[Switch]$Unresolved)
Begin
{}
Process
{
    foreach($reference in $Project.Object.References)
    {
        if((-not $Unresolved -or ($Unresolved -and -not $reference.Resolved)) -and $reference.Name -like $Name)
        {
            $result = [PSObject]::AsPSObject($reference);
            if($reference.Resolved)
            {
                [Xml]$container = Get-Content -Path:$_.ContainingProject.FullName
                $namespaces = New-Object Xml.XmlNamespaceManager $container.NameTable
                $namespaces.AddNamespace('p', 'http://schemas.microsoft.com/developer/msbuild/2003')
                $result = $result | Add-Member -NotePropertyName:'ContainingProjectNamespace' -NotePropertyValue:$namespaces;
                $result = $result | Add-Member -NotePropertyName:'ContainingProjectXml' -NotePropertyValue:$container;
                $node = $container.DocumentElement.SelectSingleNode(('//p:Reference[starts-with(@Include,"{0}")]/HintPath/text()' -f $reference.Name),$namespaces);
                $result = $result | Add-Member -NotePropertyName:'HintPath' -NotePropertyValue:$node;
            }
            Write-Output $result;
        }
    }
}
End
{}