
function Test-NSCLientInfo
{
    param([parameter(Position=0, Mandatory=$true)][string]$where)
    $clientFile = "$where\.nsclient" 
    return (Test-Path $clientFile)
}

function Enter-NSClientInfo
{
    param([parameter(Position=0, Mandatory=$true)][string]$where)

    $clientName = Read-Host -Prompt "Enter client Full name (i.e. 'NetSteps Encore')"
    $clientShortName = Read-Host -Prompt "Enter client Short name (i.e. 'NetStepsEncore')"
    $clientCode = Read-Host -Prompt "Enter client Code name (i.e. 'NSE')"
    $clientGmpRootUrl = Read-Host -Prompt "Enter client GMP Root Url  (i.e. 'portal.netsteps.us')"
    $clientDwsRootUrl = Read-Host -Prompt "Enter client DWS Root Url  (i.e. 'workstation.netsteps.us')"
    $clientCwsRootUrl = Read-Host -Prompt "Enter client CWS Root Url  (i.e. 'www.netsteps.us')"
    $clientPwsRootUrl = Read-Host -Prompt "Enter client PWS Root Url  (i.e. '*.netsteps.us')"

    Set-NSClientInfo $where $clientName $clientShortName $clientCode $clientGmpRootUrl $clientDwsRootUrl $clientCwsRootUrl $clientPwsRootUrl
}

function Get-NSClientInfo
{
    param([parameter(Position=0, Mandatory=$true)][string]$where, [Switch]$ErrorIfMissing)
    
    $client = $null
    $clientFile = "$where\.nsclient" 

    if(Test-Path $clientFile)
    {
        $xclient = [XML](Get-Content $clientFile)

        $client = @{Name = ""; ShortName = ""; Code = ""; GmpRootUrl = ""; DwsRootUrl = ""; CwsRootUrl = ""; PwsRootUrl = ""}

        $client.Name = Select-Xml -XPath "//name" -Xml $xclient 
        $client.ShortName = Select-Xml -XPath "//shortname" -Xml $xclient
        $client.Code = Select-Xml -XPath "//code" -Xml $xclient
        $client.GmpRootUrl = Select-Xml -XPath "//gmprooturl" -Xml $xclient
        $client.DwsRootUrl = Select-Xml -XPath "//dwsrooturl" -Xml $xclient
        $client.CwsRootUrl = Select-Xml -XPath "//cwsrooturl" -Xml $xclient
        $client.PwsRootUrl = Select-Xml -XPath "//pwsrooturl" -Xml $xclient
    }

    if($ErrorIfMissing -and $client -eq $null)
    {
        Write-Error "NetSteps client information missing ($clientFile)"
        break
    }
 
    return $client
}

function Set-NSClientInfo
{
    param([parameter(Position=0, Mandatory=$true)][string]$where,
        [parameter(Position=1, Mandatory=$true)][string]$Name,
        [parameter(Position=2, Mandatory=$true)][string]$ShortName,
        [parameter(Position=3, Mandatory=$true)][string]$Code,
        [parameter(Position=4, Mandatory=$true)][string]$GmpRootUrl,
        [parameter(Position=5, Mandatory=$true)][string]$DwsRootUrl,
        [parameter(Position=6, Mandatory=$true)][string]$CwsRootUrl,
        [parameter(Position=7, Mandatory=$true)][string]$PwsRootUrl)

    $clientFile = "$where\.nsclient"

    if(Test-Path $clientFile)
    {
        Remove-Item $clientFile
    }

    $XmlWriterSettings = New-Object System.Xml.XmlWriterSettings
    $XmlWriterSettings.Indent = $true

    $XmlWriter = [System.Xml.XmlWriter]([System.XMl.XmlWriter]::Create($clientFile, $XmlWriterSettings))
    
    $XmlWriter.WriteStartElement("client")
    $XmlWriter.WriteElementString("name", $Name);
    $XmlWriter.WriteElementString("shortname", $ShortName);
    $XmlWriter.WriteElementString("code", $Code);
    $XmlWriter.WriteElementString("gmprooturl", $GmpRootUrl);
    $XmlWriter.WriteElementString("dwsrooturl", $DwsRootUrl);
    $XmlWriter.WriteElementString("cwsrooturl", $CwsRootUrl);
    $XmlWriter.WriteElementString("pwsrooturl", $PwsRootUrl);
    $XmlWriter.WriteEndElement()

    $XmlWriter.Flush()
    $XmlWriter.Close()
}

function Get-IISExpressAppCmdPath
{
    $iisExpressAppCmd = (Join-Path -Path $env:ProgramW6432 -ChildPath "IIS Express\appcmd.exe")
    
    if((Test-Path $iisExpressAppCmd) -eq $false)
    {
        Write-Error "Unable to locate IIS Express AppCmd.exe... is IIS Express installed?  Use 'Web Platform Installer' (google it) to do so..."
        break
    }

    return $iisExpressAppCmd
}

function Set-IISSite
{
    param([Parameter(Mandatory=$true)][string]$name,
        [Parameter(Mandatory=$true)][string]$url,
        [Parameter(Mandatory=$true)][string]$path)

    $iisExpressAppCmd = Get-IISExpressAppCmdPath

    $existing = & $iisExpressAppCmd list site $name
    if($existing -ne $null -and $existing -ne "")
    {
        Write-Host "Executing: $iisExpressAppCmd delete site $name"
        & $iisExpressAppCmd delete site $name      
    }

    $furl = [System.String]::Format("http://{0}:80", $url)
    & $iisExpressAppCmd add site /name:"$name" /bindings:"$furl" /physicalpath:"$path"
}

function Set-IISFileUploadsVdir
{
     param([Parameter(Mandatory=$true)][string]$siteName,
        [Parameter(Mandatory=$true)][string]$shortName)

    $iisExpressAppCmd = Get-IISExpressAppCmdPath

    $phypath = Join-Path "\\netsteps.local\fileupload\DEV" -ChildPath $shortName
    & $iisExpressAppCmd add vdir /app.name:"$siteName/" /path:"/fileuploads" /physicalpath:"$phypath"
}

function Set-ClientHostRecord
{
     param([Parameter(Mandatory=$true)][string]$name,
        [Parameter(Mandatory=$true)][string]$domain)

    $host = Get-HostFileHost -HostName $domain
    if($host -eq $null)
    {
        Set-HostFileHost -Address 127.0.0.1 -HostNames @($domain) -Comment $name
    }
}

function Set-UrlAcl
{
    param([Parameter(Mandatory=$true)][string]$domain)

    $furl = [System.String]::Format("http://{0}:80/", $domain)
    & netsh http add urlacl url=$furl user=everyone
}

function Set-ClientIISRoot
{
    param([Parameter(Mandatory=$true)][string]$where)

    $client = Get-NSClientInfo $where

    if($client -eq $null -or $client.Code -eq $null -or $client.Code -eq "")
    {
        Enter-NSClientInfo $where
        $client = Get-NSClientInfo $where -ErrorIfMissing
    }

    $GMP = @{Name=""; Url=""; Path="";}
    $GMP.Name = "{0}.GMP" -f $client.Code 
    $GMP.Url = [System.String]::Format("{0}.{1}.dev.netsteps.us", $client.GmpRootUrl, ([string]$client.ShortName).ToLower())
    $GMP.Path = Join-Path -Path $where -ChildPath "Framework\Websites\nsCore"
    
    Set-IISSite $GMP.Name $GMP.Url $GMP.Path
    Set-IISFileUploadsVdir $GMP.Name $client.ShortName
    Set-ClientHostRecord -name $GMP.Name -domain $GMP.Url
    Set-UrlAcl $GMP.Url

    $DWS = @{Name=""; Url=""; Path="";}
    $DWS.Name = "{0}.DWS" -f $client.Code 
    $DWS.Url = [System.String]::Format("{0}.{1}.dev.netsteps.us", $client.DwsRootUrl, ([string]$client.ShortName).ToLower())
    $DWS.Path = Join-Path -Path $where -ChildPath "Framework\Websites\DistributorBackOffice"
    
    Set-IISSite $DWS.Name $DWS.Url $DWS.Path
    Set-IISFileUploadsVdir $DWS.Name $client.ShortName
    Set-ClientHostRecord -name $DWS.Name -domain $DWS.Url
    Set-UrlAcl $DWS.Url

    $CWS = @{Name=""; Url=""; Path="";}
    $CWS.Name = "{0}.CWS" -f $client.Code 
    $CWS.Url = [System.String]::Format("{0}.{1}.dev.netsteps.us", $client.CwsRootUrl, ([string]$client.ShortName).ToLower())
    $CWS.Path = Join-Path -Path $where -ChildPath "Framework\Websites\nsDistributor"
    
    Set-IISSite $CWS.Name $CWS.Url $CWS.Path
    Set-IISFileUploadsVdir $CWS.Name $client.ShortName
    Set-ClientHostRecord -name $CWS.Name -domain $CWS.Url
    Set-UrlAcl $CWS.Url

    $PWS = @{Name=""; Url=""; Path="";}
    $PWS.Name = "{0}.PWS" -f $client.Code 
    $PWS.Url = [System.String]::Format("{0}.{1}.dev.netsteps.us", $client.PwsRootUrl, ([string]$client.ShortName).ToLower())
    $PWS.Path = Join-Path -Path $where -ChildPath "Framework\Websites\nsDistributor"
    
    Set-IISSite $PWS.Name $PWS.Url $PWS.Path
    Set-IISFileUploadsVdir $PWS.Name $client.ShortName
    $purl = Select-String -InputObject $PWS.Url -Pattern "\*"
    if($purl -ne $null -or $purl -ne "")
    {
        $purl = ("base." + ($PWS.Url -split "\*.", 2)[1])
    }
    else
    {
        $purl = $PWS.Url
    }
    Set-ClientHostRecord -name $PWS.Name -domain $purl
    Set-UrlAcl $purl
    
}

$mod = (Split-Path $PSCommandPath)
$mod = Join-Path -Path $mod -ChildPath "\modules\ns\NetSteps.Powershell.Hosts.dll"
Import-Module $mod