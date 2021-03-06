Param(
    [string]$zipFilePath = "c:\deployment\test.zip",
    [string]$packageRoot = "C:\Builds\Framework\Websites\DistributorBackOffice"
    )
# C:\Development\Trunk\Websites\DistributorBackOffice
# Invocation: .\MyFirst.ps1 test16.zip C:\Development\Trunk\Websites\DistributorBackOffice

function Get-MSWebDeployInstallPath(){
     return (get-childitem "HKLM:\SOFTWARE\Microsoft\IIS Extensions\MSDeploy" | Select -last 1).GetValue("InstallPath")
}

$webDeploy = Get-MSWebDeployInstallPath

# Delete package if it exists, so we get a clean new package.
# msDeploy will update an existing package.
$doesFileExist = Test-Path $zipFilePath
if($doesFileExist){ 
    DEL $zipFilePath 
    'Deleted existing package.'
}

$webConfig = [System.IO.Path]::Combine($packageRoot, 'Web.config')
$webConfig = $webConfig-replace "\\", "\\"
$webConfig = $webConfig-replace "\.", "\."

$webConfig = "skipAction=AddChild,objectName=filePath,absolutePath=" + $webConfig
$contentPath = "contentpath=" + $packageRoot + ",includeAcls=true"

$webConfig

# Create package
# Remove cs files
# Do not include the root web.config
# Rename the web.transformed.config file to web.config
& $webDeploy'msdeploy.exe' `
-verb:sync `
-source:$contentPath `
-dest:package=$zipFilePath `
-skip:'objectName=filePath,absolutePath=.*\.cs$' `
-skip:$webConfig `
-replace:'objectName=filePath,scopeAttributeName=path,match=web\.transformed\.config,replace=web.config'