param(
    [Parameter(Mandatory=$true)][string]$Root = "",
	[Parameter(Mandatory=$true)][string]$BuildToolsRoot  = ""
    )
	
$nugetExe = "{0}3rdParty\Nuget\nuget.exe" -f $BuildToolsRoot
$packages = "{0}packages" -f $Root
$sources = (Resolve-Path ("{0}\..\common\NugetRepo\packages" -f $BuildToolsRoot)).Path

$files = Get-ChildItem $Root -recurse -Include packages.config | Where-Object{$_.GetType() -eq [System.IO.FileInfo]}  | select FullName

foreach ($file in $files) {
    $fullName = $file.FullName
    Write-Host "Loading packages from $fullName"
    & $nugetExe install $fullName -source "$sources;https://nuget.org/api/v2/" -o "$packages" | Write-Output
}


