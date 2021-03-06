param(
    [Parameter(Mandatory=$true)][string]$slnFile = ""
    )
	
$slnDir = Get-Location
$slnFile = [io.path]::Combine($slnDir, $slnFile)

$lines = @()
Get-Content $slnFile | ForEach-Object { 
    if ($_.ToLower().Contains(".tests.csproj")) {
        $lines += $_
    }
}

$testProjects = @()

$lines | ForEach-Object { 
    $testProjects += $_.Split('=')[1].Split(',')[1].Trim().Trim('"')
}

function FindAssemblyName ($data) {
    $assembly = $null
    $pg = $data.Project.PropertyGroup.Count
    for ($c=0; $c -lt $pg; $c++) {
        $assembly = $projData.Project.PropertyGroup[$c].AssemblyName
        if ($assembly -ne $null) {
            return $assembly
        }
    }
}

# Write each assembly to its own variable. Powershell adds white space between entries (at beginning of each entry) when concatenating.

$n = 1
$assemblyName = ""
foreach ($tp in $testProjects) {
    $projFile = [io.path]::Combine($slnDir, $tp)
    [xml]$projData = Get-Content $projFile
    $projDir = Split-Path $tp
    $projOutDir = $projDir + '\bin\Release'
    $assemblyName = FindAssemblyName $projData
    $newAssembly = [io.path]::Combine( $slnDir, [io.path]::Combine($projOutDir, $assemblyName)) + ".dll"
    Write-Host $newAssembly
    Write-Host "##teamcity[setParameter name='system.testassembly$n' value='$newAssembly']"
    $n ++
}
