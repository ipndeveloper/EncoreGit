param(
    [Parameter(Mandatory=$True)][string]$where
)

function EditSolutionFile($file){
    #find netsteps.targets
    $text = (Get-Content $file.FullName) -join "`r`n"
    
    #find netsteps.targets stuff
    $regex = new-object Text.RegularExpressions.Regex 'Solution Items\\NetSteps\.Targets = Solution Items\\NetSteps.Targets', ('singleline')
    $text = $regex.Replace($text, "")
    
    #find nuget project stuff
    $regex = new-object Text.RegularExpressions.Regex 'Project\("{2150E333-8FDC-42A3-9474-1A3956D46DE8}"\) = "\.nuget".*?EndProject', ('singleline','multiline')
    $text = $regex.Replace($text, "")
    
    #find source binding crap
    $regex = new-object Text.RegularExpressions.Regex 'GlobalSection\(TeamFoundationVersionControl\).*?EndGlobalSection', ('singleline','multiline')
    $text = $regex.Replace($text, "")
    
    #find the vsmdi
    #$regex = new-object Text.RegularExpressions.Regex 'GlobalSection\(TestCaseManagementSettings\).*?EndGlobalSection', ('singleline','multiline')
    #$text = $regex.Replace($text, "")
    
    #$regex = new-object Text.RegularExpressions.Regex '.*\.vsmdi = .*\.vsmdi.*'
    #$text = $regex.Replace($text, "")
    
    #save modified content
    Set-Content $file.FullName $text -encoding UTF8
    
    $file.Name
}

Get-ChildItem "$where\*.sln" | `
    foreach { EditSolutionFile $_ }
