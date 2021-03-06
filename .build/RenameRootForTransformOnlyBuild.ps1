param(
	[parameter(Mandatory=$true)][string]$buildNumber,	#Full build number of build pattern.
	[parameter(Mandatory=$true)][string]$root,			#Sandbox root folder
	$verbosity='normal'
)

#Rename existing folder to reflect build number and prepare for limited build.
Write-Host "##teamcity[setParameter name='env.OldBuildNumber' value='$buildNumber']"
$env:OldBuildNumber = "$buildNumber"
$newRoot = [io.path]::Combine((Split-Path $root),$buildNumber)
ren $root $newRoot
if(Test-Path $newRoot){
	$root = $newRoot
	Write-Host "##teamcity[setParameter name='env.newRoot' value='$root']"
}
else{
	throw "ERROR: Rename of root folder failed"
}
