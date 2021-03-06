$pkg = "Core"

$it = "$pkg.csproj"
$where = Split-Path -parent $MyInvocation.MyCommand.Definition
cd $where

$spec = $where + "\$pkg.nuspec"
$spec

# Ensure we know where the dll is located in relation to the script:
if (Test-Path ".\$it") {

    # record the machine architecture:
    $machineArch = (Get-WmiObject Win32_OperatingSystem).OSArchitecture

    # get the corresponding .NET framework directory:
    if ($machineArch -eq "64-bit"){$frameworkDir="$env:windir\Microsoft.NET\Framework64"} else {$frameworkDir="$env:windir\Microsoft.NET\Framework"}

    # get the highest version of the framework available:
    ls $frameworkDir | ? { $_.PSIsContainer -and $_.Name -match '^v\d.[\d\.]+' } | % { $latestFramework = $_.Name }

    Set-Alias msbuild "$frameworkDir\$latestFramework\msbuild.exe"
    Set-Alias nuget "..\..\build-tools\nuget.exe"
    
    $info = svn.exe info --xml 2>&1
    if($info -is [System.Management.Automation.ErrorRecord]) {
        'Unable to retrieve SVN revision number'
        exit
    }
    else {
        $infoXml = [xml]$info
        
        [xml] $prj = Get-Content ".\$it"
        $prj.GetElementsByTagName("MajorVersion") | foreach { 
            $maj = $_.InnerText
        }
        $prj.GetElementsByTagName("MinorVersion") | foreach { 
            $min = $_.InnerText
        }
        $revision = $infoXml.info.entry.revision        
        
        $sinceMidnight = [System.DateTime]::Now.Subtract([System.DateTime]::Today);
        $secs = [System.Math]::Round($sinceMidnight.TotalSeconds * (65535 / 86400))
        
        $version = [System.String]::Concat($maj, '.', $min, '.', $revision, '.', $secs)
                
        Get-ChildItem -r -filter AssemblyInfo.cs | ForEach-Object {
                $filename = $_.Directory.ToString() + '\' + $_.Name
                $filename + ' -> ' + $version
                                 
                (Get-Content $filename) | ForEach-Object {
                    % {$_ -replace "^(.*AssemblyVersion.{1}).*(.{2})`$", "`$1`"$version`"`$2" } |
                    % {$_ -replace "^(.*AssemblyFileVersion.{1}).*(.{2})`$", "`$1`"$version`"`$2" }
                } | Set-Content $filename
        }
    }
    
    msbuild $it '/t:Clean;Rebuild' '/property:Configuration=Release;Platform=AnyCPU' '/verbosity:d'    
        
    nuget spec -F $it    
    
    [xml] $specxml = Get-Content $spec
    $specxml.GetElementsByTagName("id") | foreach { 
        $_.InnerText = $pkg
    }
    $specxml.GetElementsByTagName("title") | foreach { 
        $_.InnerText = [System.String]::Concat($pkg, ' Release')
    }
    $specxml.GetElementsByTagName("licenseUrl") | foreach { 
        $licenseUrl = $_
    }
    $licenseUrl.ParentNode.RemoveChild($licenseUrl)
    $specxml.GetElementsByTagName("projectUrl") | foreach { 
        $_.InnerText = "http://intranet/NetSteps%20Wiki/$pkg.aspx"
    }
    $specxml.GetElementsByTagName("iconUrl") | foreach { 
        $iconUrl = $_
    }
    $iconUrl.ParentNode.RemoveChild($iconUrl)
    $specxml.GetElementsByTagName("tags") | foreach { 
        $tags = $_
    }
    $tags.ParentNode.RemoveChild($tags)
    $specxml.GetElementsByTagName("releaseNotes") | foreach { 
        $releaseNotes = $_
    }
    $releaseNotes.ParentNode.RemoveChild($releaseNotes)
    
    $specxml.Save($spec)
        
    nuget pack -Verbose -OutputDirectory .\ -Properties Configuration=Release`;Platform=AnyCPU
    nuget push "$pkg.$version.nupkg" -s http://nuget1.netsteps.com/ 1mdj7cvjf87d9
    nuget push "$pkg.$version.nupkg" -s http://nuget2.netsteps.com/ 1mdj7cvjf87d9
}
else {$(Throw "Can't find the project, ensure you are running this script from the proper directory and try again.")}
