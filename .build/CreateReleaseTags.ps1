param(
    [Parameter(Mandatory=$true)][string]$client = "",
    [Parameter(Mandatory=$true)][string]$branch = "",
    [Parameter(Mandatory=$true)][string]$buildnumber = "",
    [Parameter(Mandatory=$true)][string]$releasedate = "",
    [switch]$help
    )
Stop-Transcript | Out-Null
cls

function displayHelp {
#todo: display usage text if invalid parameter passed
    Write-Host "usage: powershell -file CreateReleaseTags.ps1 -client <clientname> -branch <clientbranchname> -buildnumber <1.2.3.4>"
    Write-Host "    -client      client name used for svn repo"
    Write-Host "    -branch      name of branch under branches"
    Write-Host "https://newsvn.netsteps.com/svn/<client>/branches/<branch>"
    Write-Host "    -buildnumber buildnumber of build to tag"
    Write-Host "    -releasedate Date release occurred. (01Jan2013)"
    Write-Host "    -help        print this message"
    exit
}

if ($help -eq $true) {
    displayHelp
}

if ($branch.ToLower() -ne 'trunk'){
	$branch = 'branches/{0}' -f $branch
}
$clientSvnPath = "https://newsvn.netsteps.com/svn/{0}/{1}" -f $client, $branch
$svnExe = "c:\Program Files\TortoiseSVN\bin\svn.exe"

function getSvnRevision ($path, $date) {
    $revision = $null
    $date0 = '{{"{0}"}}' -f $date

    $svnCmd = "{0} log {1} -r {2} --xml" -f $svnExe, $path, $date0
    Write-Host "Svn Command:" $svnCmd "`r"
    [xml]$myData = & $svnExe log $path -r $date0 --xml
    $revision = $myData.log.logentry.revision

    $i = 0
    while ($revision -eq $null){
        $i--
        $dt = [datetime]$date
#        $revision = ""
#        $dtText = [string]$dt.GetDateTimeFormats()[105]
        $revision = getSvnRevisionEx $path $dt $i
    }


    return $revision
}

function getExternals ($path) {
    $myHash = @{}
    $svnCmd = "{0} propget svn:externals {1}" -f $svnExe, $path
    Write-Host "Svn Command:" $svnCmd "`r"
    $myData = & $svnExe propget svn:externals $path
    foreach ( $line in $myData ){
        if ($line.Contains(" ")) {
            $lineParts = $line.Split(" ")
            $myHash.Add($lineParts[-1], $lineParts[-2])
        }
    }
    return $myHash
}

function getClientRevision ($bnum) {
	$numParts = $bnum.Split(".").Count
	if ($numParts -ne 4){
	   Write-Host "Invalid BuildNumber used:" $bnum "`r"
	}
    $rev = $bnum.Split(".")[2]
    return "-r{0}" -f $rev
}

function beginLog {
    Start-Transcript -path $logfile -Append
    Write-Host "Client:" $client "`r"
    Write-Host "Branch:" $branch "`r"
    Write-Host "SVN path to branch:" $clientSvnPath "`r"
    Write-Host "Build Number:" $buildnumber "`r"
    Write-Host "Release Date:" $releasedate "`r"
}

$logfile = "{0}_{1}Tags.log" -f ($client, $buildnumber)
beginLog
$clientRevision = getClientRevision $buildnumber
Write-Host "Client Revision:" $clientRevision "`r"

$startDir = Get-Location
$tempDir = [System.IO.Path]::GetTempPath()
Set-Location $tempDir
$baseDir = Join-Path $tempDir "CreateTag"
if(Test-Path $baseDir){
    "Removing Path {0}" -f $baseDir
    Remove-Item $baseDir -recurse -force
}
$result = New-Item $baseDir -type directory
Set-Location $baseDir
$releaseDir = Join-Path $baseDir "release"
$result = New-Item $releaseDir -type directory
$frameworkDir = Join-Path $baseDir "frameworkcopy"
$result = New-Item $frameworkDir -type directory
$clientDir = Join-Path $baseDir "clientcopy"
$result = New-Item $clientDir -type directory
Write-Host "Starting script in temp folder:" $baseDir "`r"

#Get empty copy of client branch
Write-Host "Get empty copy of client branch.`r"
$svnCmd = "{0} checkout {1} release {2} --depth empty" -f $svnExe, $clientSvnPath, $clientRevision
Write-Host "Svn Command:" $svnCmd
& $svnExe checkout $clientSvnPath release $clientRevision --depth empty

#Discover externals information
Write-Host "Discover externals information.`r"
Set-Location $releaseDir
$svnFrameworkRevision = & $svnExe propget Framework .
$svnBuildRevision = & $svnExe propget dotBuild .
Write-Host ".build Revision:" $svnBuildRevision "`r"
Write-Host "Framework Revision:" $svnFrameworkRevision "`r"
$clientExternals = getExternals $clientSvnPath
Write-Host "Client .build External:" $clientExternals.".build" "`r"
Write-Host "Client Framework External:" $clientExternals.Framework "`r"

#Tag Framework
Write-Host "Create Framework Tag`r"
Set-Location $baseDir

if ($branch.ToLower() -eq 'trunk'){
	$frameworkTagFolder = $clientExternals.Framework.Replace("trunk", "tags/trunk").Replace("branches", "tags")
	$clientTagFolder = $clientSvnPath.Replace("trunk", "tags/trunk")
}
else{
	$frameworkTagFolder = $clientExternals.Framework.Replace("branches", "tags")
	$clientTagFolder = $clientSvnPath.Replace("branches", "tags")
}
$frameworkTagFolder = [string]::join("_", ($frameworkTagFolder, $client, $buildnumber))
$clientTagFolder = [string]::join("_", ($clientTagFolder, $buildnumber))

Write-Host "FrameworkTagFolder:" $frameworkTagFolder "`r"
Write-Host "ClientTagFolder:" $clientTagFolder "`r"

$svnComment = '"Creating Tag for {0} {1} release @{2}. Released on {3}."' -f $client, $branch, $buildnumber, $releasedate
$svnCmd = "{0} copy {1} {2} -r {3} -m {4}" -f $svnExe, $clientExternals.Framework, $frameworkTagFolder, $svnFrameworkRevision, $svnComment
Write-Host "svn Command:" $svnCmd "`r"
& $svnExe copy $clientExternals.Framework $frameworkTagFolder -r $svnFrameworkRevision -m $svnComment

#Set new properties for Framework and store new version
Write-Host "Set new properties for Framework and store new version.`r"
$buildString = "-r{0} {1} .build" -f $svnBuildRevision, $clientExternals.".build"

$svnCmd = "{0} checkout {1} frameworkcopy --depth empty" -f $svnExe, $frameworkTagFolder
Write-Host "svn Command:" $svnCmd "`r"
& $svnExe checkout $frameworkTagFolder frameworkcopy --depth empty
Set-Location $frameworkDir
Set-Content framework.txt $buildString

Write-Host "Setting externals for framework.`r"
$svnCmd = "{0} propset svn:externals --file framework.txt ."-f $svnExe
Write-Host "svn Command:" $svnCmd "`r"
& $svnExe propset svn:externals --file framework.txt .

$svnCommitComment = '"Locking svn:externals property to released version(s)."'
$svnCmd = "{0} commit . -m {1}" -f $svnExe, $svnCommitComment
Write-Host "svn Command:" $svnCmd "`r"
& $svnExe commit . -m $svnCommitComment
$svnCmd = "{0} log -l 1 --xml" -f $svnExe
Write-Host "svn Command:" $svnCmd "`r"
[xml]$svnData = & $svnExe log -l 1 --xml
$svnCommitRev = $svnData.log.logentry.revision
Write-Host "svnCommitRev:" $svnCommitRev "`r"

Set-Location $baseDir
Write-Host "Resetting location:" $baseDir "`r"

#Tag Client
Write-Host "Create Client Tag`r"
$clientRev = $clientRevision.Replace("-r","")
$svnCmd = "{0} copy {1} {2} -r {3} -m {4}" -f $svnExe, $clientSvnPath, $clientTagFolder, $clientRev, $svnComment
Write-Host "svn Command:" $svnCmd "`r"
& $svnExe copy $clientSvnPath $clientTagFolder -r $clientRev -m $svnComment

#Set new properties for Client
Write-Host "Set new properties for Client and store new version.`r"
$frameworkString = "-r{0} {1} Framework" -f $svnCommitRev, $frameworkTagFolder

$svnCmd = "{0} checkout {1} clientcopy --depth empty" -f $svnExe, $clientTagFolder
Write-Host "svn Command:" $svnCmd "`r"
& $svnExe checkout $clientTagFolder clientcopy --depth empty
Set-Location $clientDir
Set-Content client.txt $buildString
Add-Content client.txt $frameworkString

Write-Host "Setting externals for client.`r"
$svnCmd = "{0} propset svn:externals --file client.txt ." -f $svnExe
Write-Host "svn Command:" $svnCmd "`r"
& $svnExe propset svn:externals --file client.txt .
$svnCmd = "{0} commit . -m {1}" -f $svnExe, $svnCommitComment
Write-Host "svn Command:" $svnCmd "`r"
& $svnExe commit . -m $svnCommitComment

Set-Location $startDir
