param(
    $nugetKey = (Get-ItemProperty 'HKCU:\Software\NetSteps\NuGet').'PublishKey',
    $majorVersion = 2,
    $minorVersion = 5,
	$preReleaseString = '',
    $nugetExePath = 'C:\Utils\Nuget.exe',
    $mode = "TFS",
    $pathToTfExe = "",
    $domainUserName = "",
    $domainPassword = "",
    $projectPath = 'C:\Development\NetSteps\Components\Promotions\Common\Promotions.Common.csproj',
    $msbuildPath = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe'
)
#
# Invoke a build with targets to invoke versioning, packaging and publishing.
#

CLS

if($mode -eq "SVN"){
    #
    # SVN Version, Build, Package, Publish
    #
    & $msbuildPath `
        $projectPath `
        '/t:Clean;Build;PackageForNuget' `
        "/p:NugetExePath=$nugetExePath;MajorVersion=$majorVersion;MinorVersion=$minorVersion;NugetKey=$nugetKey;Configuration=Release;RunSvnVersioning=True;PreReleaseString=$preReleaseString"
}

if($mode -eq "TFS"){
    #
    # TFS Version, Build, Package, Publish
    #
    & $msbuildPath `
        $projectPath `
        '/t:Clean;Build;PackageForNuget' `
        "/p:NugetExePath=$nugetExePath;MajorVersion=$majorVersion;MinorVersion=$minorVersion;NugetKey=$nugetKey;TFExePath=$pathToTfExe;TFExeUsername=$domainUserName;TFExePassword=$domainPassword;Configuration=Release;RunTfsVersioning=True;PreReleaseString=$preReleaseString"
}