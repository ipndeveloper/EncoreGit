param(
	[parameter(Mandatory=$true)][string]$tenantCode,	#3-letter short name
	[parameter(Mandatory=$true)][string]$dbtenantcode, 	#Used by transmogrifier; historical name of client when running client scripts;
	[parameter(Mandatory=$true)][string]$tenantName,	#Client name as used in ClientSettingsPreBuild project name.
	[parameter(Mandatory=$true)][string]$buildNumber,	#Full build number of build pattern.
	[parameter(Mandatory=$true)][string]$root,			#Sandbox root folder
	$verbosity='normal'
)

function PackageForDistribution($source, $package, $root){
	$exe = "{0}\.build\deploy.cmd" -f $root
	& $exe $source $package
}

function TransformOnly($projectFile, $winVer){
	Write-Host "TransformOnly Starting..."
	$projectRoot = Split-Path $projectFile
	$parameters = "Configuration=Release;Platform=AnyCPU;CMBuild=true;TransformOnlyBuild=true"
	& $msBuild $projectFile /t:TransformOnly /P:$parameters /V:$verbosity
}

$production = @{ EnvironmentCode = "PRD"; Configuration = "Release" }
$staging = @{ EnvironmentCode = "STG"; Configuration = "Staging" }
$qa = @{ EnvironmentCode = "QA"; Configuration = "Testing" }
$configurations = $production, $staging, $qa	#$production must be built first: faster build, successful package creation depends on order.

#Rename existing folder to reflect build number and prepare for limited build.
#$newRoot = [io.path]::Combine((Split-Path $root),$buildNumber)
#ren $root $newRoot
#if(Test-Path $newRoot){
#	$root = $newRoot
#	$parts = $buildNumber.Split('.')
#	$buildNumber = "{0}.{1}.{2}.0{3}" -f $parts
#}
#else{
#	throw "ERROR: Rename of root folder failed"
#}

#Build ClientSettingsPreBuild to distribute config and/or Enrollment.xml changes.
$msBuild = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
$projectFile = "{0}\{1}.ClientSettingsPreBuild\{1}.ClientSettingsPreBuild.csproj" -f $root, $tenantName
if(Test-Path $projectFile){
	$solutionDir = $root.replace("\","/") + '\'
#	$cmd = "{0} {1} /t:build /P:Configuration=Release;Platform=AnyCPU;SolutionDir={2}/ /V:{3}" -f $msBuild, $projectFile, $root.replace("\","/"), $verbosity
	Write-Host "solutionDir: " $solutionDir
	$parameters = "Configuration=Release;Platform=AnyCPU;SolutionDir={0}" -f $solutionDir
	"{0} {1} /t:build /V:{2} /P:{3}" -f $msBuild,$projectFile,$verbosity,$parameters
	& $msBuild $projectFile /t:build /V:$verbosity /P:$parameters
}

foreach($configuration in $configurations){
	$sdlLocation = "E:\SDL\Deploy2.0\{0}\{1}\{2}" -f $tenantCode, $configuration.EnvironmentCode, $buildNumber
	$databaseScripts = "{0}\DatabaseScripts" -f $sdlLocation

	if(Test-Path $databaseScripts){
		"Removing Path {0}" -f $sdlLocation
		Remove-Item $sdlLocation -recurse
	}

	"Adding Path {0}" -f $databaseScripts
	$result = New-Item $databaseScripts -type directory

	$frameworkPath = "{0}\Framework" -f $root
	$framework = ""
	if(Test-Path $frameworkPath){
		$framework = "Framework\"
	}
	if($framework){
		& ('{0}\.build\NetSteps\Transmogrifier\SqlTransmogrifier.exe' -f $root) -s $root\Framework\SQL\Deployments -c FRAMEWORK -b true -t Template2 -coredb '$CoreDatabaseName$' -commdb '$CommissionsDatabaseName$' -maildb '$MailDatabaseName$' -td $databaseScripts
		#wait between transmogrify calls
		$pings = & PING 127.0.0.1 -n 6
	}
	& ('{0}\.build\NetSteps\Transmogrifier\SqlTransmogrifier.exe' -f $root) -s $root\SQL\Deployments -c $dbtenantCode -b true -t Template2 -coredb '$CoreDatabaseName$' -commdb '$CommissionsDatabaseName$' -maildb '$MailDatabaseName$' -td $databaseScripts
		
	#-------------------------------------------   
	#--------------------AUT--------------------
	#-------------------------------------------
	$projectFile = "{0}\{1}ConsoleApplications\AutoshipProcessor\AutoshipProcessor.csproj" -f $root, $framework
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
		$source = "{0}\{2}ConsoleApplications\AutoshipProcessor\bin\{1}\" -f $root, $configuration.Configuration, $framework
		$package = "{0}\AUT-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}
	
	#-------------------------------------------   
	#--------------------QUE--------------------
	#-------------------------------------------
	$projectFile = "{0}\{1}Windows Services\QueueProcessing\NetSteps.QueueProcessing.Host\NetSteps.QueueProcessing.Host.csproj" -f $root, $framework
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
		$source = "{0}\{2}Windows Services\QueueProcessing\NetSteps.QueueProcessing.Host\bin\{1}\" -f $root, $configuration.Configuration, $framework
		$package = "{0}\QUE-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}
	
	#-------------------------------------------	 
	#--------------------EVP--------------------
	#-------------------------------------------
	$projectFile = "{0}\Services\EventProcessing.Host\EventProcessing.Host.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
		$source = "{0}\Services\EventProcessing.Host\bin\{1}\" -f $root, $configuration.Configuration
		$package = "{0}\EVP-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}

	#-------------------------------------------	 
	#--------------------EVA--------------------
	#-------------------------------------------
	$projectFile = "{0}\Services\EventProcessing.Assemblies\EventProcessing.Assemblies.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
		$source = "{0}\Services\EventProcessing.Assemblies\bin\{1}\" -f $root, $configuration.Configuration
		$package = "{0}\EVA-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}		

	#-------------------------------------------	 
	#--------------------RST--------------------
	#-------------------------------------------
	$projectFile = "{0}\{1}Websites\Encore.ApiSite\Encore.ApiSite.csproj" -f $root, $framework
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
		$source = "{0}\{2}Websites\Encore.ApiSite\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration, $framework
		$package = "{0}\RST-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}
	
	#-------------------------------------------	 
	#--------------------DWS--------------------
	#-------------------------------------------
	$projectFile = "{0}\{1}Websites\DistributorBackOffice\DistributorBackOffice.csproj" -f $root, $framework
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
		$source = "{0}\{2}Websites\DistributorBackOffice\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration, $framework
		$package = "{0}\DWS-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}
	
	#-------------------------------------------	 
	#--------------------GMP--------------------
	#-------------------------------------------
	$projectFile = "{0}\{1}Websites\nsCore\nsCore.csproj" -f $root, $framework
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
		$source = "{0}\{2}Websites\nsCore\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration, $framework
		$package = "{0}\GMP-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}
	
	#-----------------------------------------------	 
	#--------------------PWS/CWS--------------------
	#-----------------------------------------------
	$projectFile = "{0}\{1}Websites\nsDistributor\nsDistributor.csproj" -f $root, $framework
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
		$source = "{0}\{2}Websites\nsDistributor\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration, $framework
		$package = "{0}\PWS-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	
		$cwsConfigFile = "{0}\{1}Websites\nsDistributor\Web.CWS.{2}.config" -f $root, $framework, $configuration.Configuration
		$package = "{0}\CWS-{1}.zip" -f $sdlLocation, $configuration.Configuration
		if(Test-Path $cwsConfigFile){
			$source = "{0}\{2}Websites\nsDistributor\obj\CWS{1}\Package\PackageTmp" -f $root, $configuration.Configuration, $framework
		}
		PackageForDistribution $source $package $root
	}
	
	#-------------------------------------------	 
	#--------------------WCF--------------------
	#-------------------------------------------
	$projectFile = "{0}\Websites\WCFAPI\WCFAPI.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
		$source = "{0}\Websites\WCFAPI\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
		$package = "{0}\WCF-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}

	#---------------------------------------------	 
	#--------------------Miche--------------------
	#---------------------------------------------
	$projectFile = "{0}\Websites\HttpRedirect\HttpRedirectModule.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
		$source = "{0}\Websites\HttpRedirect\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
		$package = "{0}\MRE-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}	

	$projectFile = "{0}\ConsoleApplications\CreateThumbnails\CreateThumbnails.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
		$source = "{0}\ConsoleApplications\CreateThumbnails\bin\{1}\" -f $root, $configuration.Configuration
		$package = "{0}\MTH-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}

	#-------------------------------------------------	 
	#--------------------PartyLite--------------------
	#-------------------------------------------------
	$projectFile = "{0}\Websites\PartyLite.DistributorBackOfficeMobile\DistributorBackOfficeMobile.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
		$source = "{0}\Websites\PartyLite.DistributorBackOfficeMobile\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
		$package = "{0}\PDM-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}

	$projectFile = "{0}\Websites\PartyLite.Services.Api.Website\PartyLite.Services.Api.Website.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
		$source = "{0}\Websites\PartyLite.Services.Api.Website\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
		$package = "{0}\PAP-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}

	$projectFile = "{0}\Websites\WebService.Mobile\WebService.Mobile.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
		$source = "{0}\Websites\WebService.Mobile\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
		$package = "{0}\PWM-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}		

	$projectFile = "{0}\PartyLite.ShippingMethods\PartyLite.Tasks.ShippingMethods.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
		$source = "{0}\PartyLite.ShippingMethods\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
		$package = "{0}\PSM-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}		

	#-------------------------------------------------	 
	#--------------------JewelKade--------------------
	#-------------------------------------------------
	$projectFile = "{0}\Integrations\Services\OrderFulfillment\Service\WebService\IntegrationsService.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
	$source = "{0}\Integrations\Services\OrderFulfillment\Service\WebService\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
	$package = "{0}\JIS-{1}.zip" -f $sdlLocation, $configuration.Configuration
	PackageForDistribution $source $package $root
	}

	$projectFile = "{0}\GPIntegration\GPIntegration\GPIntegration.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
	$source = "{0}\GPIntegration\GPIntegration\bin\{1}\" -f $root, $configuration.Configuration
	$package = "{0}\JGP-{1}.zip" -f $sdlLocation, $configuration.Configuration
	PackageForDistribution $source $package $root
	}
	#-------------------------------------------------	 
	#--------------------Natura-----------------------
	#-------------------------------------------------
	$projectFile = "{0}\NaturaMexico API\NaturaMexico.ApiSite\NaturaMexico.ApiSite.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
	$source = "{0}\NaturaMexico API\NaturaMexico.ApiSite\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
	$package = "{0}\NAP-{1}.zip" -f $sdlLocation, $configuration.Configuration
	PackageForDistribution $source $package $root
	}

	$projectFile = "{0}\NaturaLegacy\Common\Websites\nsCore4\nsCore.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
	$source = "{0}\NaturaLegacy\Common\Websites\nsCore4\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
	$package = "{0}\NAD-{1}.zip" -f $sdlLocation, $configuration.Configuration
	PackageForDistribution $source $package $root
	}

	$projectFile = "{0}\NaturaLegacy\Natura\Natura.Corporate\Natura.Corporate.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
	$source = "{0}\NaturaLegacy\Natura\Natura.Corporate\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
	$package = "{0}\NCO-{1}.zip" -f $sdlLocation, $configuration.Configuration
	PackageForDistribution $source $package $root
	}

	$projectFile = "{0}\NaturaLegacy\Natura\Natura.Social\Natura.Social.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
	$source = "{0}\NaturaLegacy\Natura\Natura.Social\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
	$package = "{0}\NSO-{1}.zip" -f $sdlLocation, $configuration.Configuration
	PackageForDistribution $source $package $root
	}

	$projectFile = "{0}\NaturaLegacy\Natura\UpdateFriendlyUrls\UpdateFriendlyUrls.csproj" -f $root
	if(Test-Path $projectFile){
		if ($configuration.Configuration  -eq "Release"){
			TransformOnly $projectFile 'win64'
		}
	$source = "{0}\NaturaLegacy\Natura\UpdateFriendlyUrls\bin\{1}\" -f $root, $configuration.Configuration
	$package = "{0}\NFR-{1}.zip" -f $sdlLocation, $configuration.Configuration
	PackageForDistribution $source $package $root
	}
}