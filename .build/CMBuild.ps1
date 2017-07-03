$svnExe = [io.path]::Combine($env:ProgramFiles, [io.path]::Combine('TortoiseSVN', [io.path]::Combine('bin', 'svn.exe')))

function getVersionMajorMinor {
	$CurDir = [Environment]::CurrentDirectory
	$VersionFile = Join-Path -Path $CurDir -ChildPath "Version.targets"
	[xml]$tempXml = Get-Content $VersionFile
    $VersionMajor = $tempXml.Project.PropertyGroup.VersionMajor
    $VersionMinor = $tempXml.Project.PropertyGroup.VersionMinor
    return "$VersionMajor.$VersionMinor"
}

function getExternalVersions {
    [xml]$tempXml = & $svnExe info .\framework --xml
    $VersionFramework = $tempXml.info.entry.revision
    [xml]$tempXml = & $svnExe info .\.build --xml
    $VersiondotBuild = $tempXml.info.entry.revision
    return $VersionFramework, $VersiondotBuild
}

function getVersionBuild {
    [xml]$tempXml = & $svnExe info .\ --xml
    $VersionClient = $tempXml.info.entry.revision
    return $VersionClient
}

function setSvnVersions ($VersionFramework, $VersiondotBuild) {
	$tempVar0 = & $svnExe propset Framework $versionFramework . --non-interactive --trust-server-cert
	Write-Host "Setting Framework external to" $VersionFramework "`r"
	$tempVar1 = & $svnExe propset dotBuild $VersiondotBuild . --non-interactive --trust-server-cert
	Write-Host "Setting dotBuild external to" $VersiondotBuild "`r"
	$svnComment = '"CMBuild: Setting properties for tagging. Framework: {0}; dotBuild: {1}."' -f $VersionFramework, $VersiondotBuild
	$tempVar2 = & $svnExe commit . -m $svnComment --non-interactive --trust-server-cert
	return $tempVar2
}

function getSvnVersions {
	$VersionFramework, $VersiondotBuild = getExternalVersions
	$tempVar = setSvnVersions $VersionFramework $VersiondotBuild
	if ($tempVar -eq $null) {
		Write-Host "External properties unchanged. Commit aborted.`r"
		return $tempVar
	}
	Write-Host "External properties set. Commit successful.`r"
	$VersionClient = getVersionBuild
	return $VersionClient
}

Write-Host "TeamCity Build Counter: " $env:BUILD_NUMBER
$CmVersionRevision = $env:BUILD_NUMBER
$Versions = getVersionMajorMinor
$VersionClient0 = "0"
$VersionClient1 = "0"
$iCount = 0

while (($VersionClient0 -eq $VersionClient1) -and ($iCount -lt 3)) {
	$iCount++
	& $svnExe update . --non-interactive --trust-server-cert
	$VersionClient0 = getVersionBuild
	$VersionClient1 = getSvnVersions
	"Start Client Version: {0}`r" -f $VersionClient0
	"End Client Version: {0}`r" -f $VersionClient1
}

if ($VersionClient1 -eq $null) {
	$VersionClient1 = $VersionClient0
}

Write-Host "CmVersionRevision:" "$CmVersionRevision"
Write-Host "##teamcity[setParameter name='env.CmVersionRevision' value='$CmVersionRevision']"
Write-Host "Full Build Number:" "$versions.$VersionClient1.$CmVersionRevision"
Write-Host "##teamcity[setParameter name='env.FullBuildNumber' value='$versions.$VersionClient1.$CmVersionRevision']"
$env:CMVersionRevision = "$CmVersionRevision"
$env:FullBuildNumber = "$versions.$VersionClient1.$CmVersionRevision"
Write-Host "##teamcity[buildNumber '$versions.$VersionClient1.$CmVersionRevision']"

