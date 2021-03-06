param(
    [Parameter(Mandatory=$True)][string]$where,
    [Parameter(Mandatory=$True)][string]$newMajorVersion,
    [Parameter(Mandatory=$True)][string]$newMinorVersion
)

$devToolsPath = [System.IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Path)
$versionroot = (Resolve-Path ($where)).Path

$versionRootFileName = 'VersionRoot.targets'
$versionFileName = 'Version.targets'

function Outp([string] $text, [string] $cl) {
    $text    
}

function FixVersionFile([string] $where) {
    $commentary = ''
    $preamble = ''
    $needsSave = $false
    $versionRootFile = [System.String]::Concat($where, '\', $versionRootFileName)
    $versionFile = [System.String]::Concat($where, '\', $versionFileName)
    
    if (!(Test-Path $versionRootFile)) {
        Copy-Item ("$devToolsPath\$versionRootFileName") $where
        $preamble = "`n  $versionRootFileName << restored missing file"                    
    }
    if (!(Test-Path $versionFile)) {
        Copy-Item ("$devToolsPath\$versionFileName") $where
        $preamble = "`n  $versionFileName << restored missing file"                    
    } else {
        $preamble = "`n  $versionFileName"                    
    }    
    
    $proj = [xml](Get-Content $versionFile)    
    $nsmgr = New-Object System.Xml.XmlNamespaceManager -ArgumentList $proj.NameTable
    $nsmgr.AddNamespace('p','http://schemas.microsoft.com/developer/msbuild/2003')
    
    # Ensure the NSVersionMajor is correct:
    $versionMajor = $proj.SelectNodes("//p:VersionMajor", $nsmgr)
    if ($versionMajor.count -gt 0) {
        $node = $proj.Project
        if (!($node -eq $null)) {
            $versionMajor | foreach {
                if ($_.InnerText -ne $newMajorVersion) {
                    $thisParent = $_.ParentNode
                    $insert = $proj.CreateElement('VersionMajor')
                    $insert.InnerText = $newMajorVersion
                    [void]$thisParent.ReplaceChild($insert, $_)
                    $commentary = "$commentary`n    >> Modified VersionMajor to: $newMajorVersion"
                    $needsSave = $true               
                }
            }
        }
    }
       
    # Ensure the NSVersionMinor is correct:
    $versionMinor = $proj.SelectNodes("//p:VersionMinor", $nsmgr)
    if ($versionMinor.count -gt 0) {
        $node = $proj.Project
        if (!($node -eq $null)) {
            $versionMinor | foreach { 
                if ($_.InnerText -ne $newMinorVersion) {
                    $thisParent = $_.ParentNode
                    $insert = $proj.CreateElement('VersionMinor')
                    $insert.InnerText = $newMinorVersion
                    [void]$thisParent.ReplaceChild($insert, $_)
                    $commentary = "$commentary`n    >> Modified VersionMinor to: $newMinorVersion"
                    $needsSave = $true               
                } 
            }
            
        }
    }
       
    if ($needsSave) {
        $proj = [xml] $proj.OuterXml.Replace(" xmlns=`"`"", "")
        $proj.Save($versionFile);
        Outp "$where$preamble$commentary" "Magenta"                
    }
}

function MakeVersionRoot([string] $where) {
    if ($where -ne $devToolsPath) {
        Outp "`nMarking version root at $where"
        FixVersionFile $where 
    } else { 
       'Cannot establish a version root without a target path'
    }
}

if (Test-Path $versionroot) { MakeVersionRoot $versionroot }
