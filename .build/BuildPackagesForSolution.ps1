param(
    [parameter(Mandatory=$true)][string]$tenantCode,
    [parameter(Mandatory=$true)][string]$dbtenantcode, #used by transmogrifier; historical name of client when running client scripts.
    [parameter(Mandatory=$true)][string]$buildNumber,
    [parameter(Mandatory=$true)][string]$root,
    $verbosity='normal'
)

function PackageForDistribution($source, $package, $root){
    $exe = "{0}\.build\deploy.cmd" -f $root
    & $exe $source $package
}

function CopyBinRelease($projectFile, $config){
	Write-Host "CopyBinRelease Starting..."
	$configFile = Split-Path ($projectFile.Replace(".csproj", "*.config")) -Leaf
	if ($config -ne "Release"){
		Write-Host "Project: $projectFile"
		Write-Host "Copying files from bin\Release to bin\$config"
		$projectRoot = Split-Path $projectFile
		$markerFile = [io.path]::Combine($projectRoot, "CMTransform.marker")
		if(Test-Path $markerFile){
			Write-Host "Marker file found..."
			$source = [io.path]::Combine($projectRoot, [io.path]::Combine('bin', 'Release'))
			$target = [io.path]::Combine((Split-Path $source), $config)
			Copy-Item -Path "$source\*" -Destination $target -Recurse -Exclude $configFile -Force
		}
	}
}

function AssembleCWSFilesForPackaging($cwsConfigFile, $config){
	Write-Host "AssembleCWSFilesForPackaging Starting..."
	$projectRoot = Split-Path $cwsConfigFile
	$configFile = [io.path]::Combine($projectRoot, [io.path]::Combine('obj', [io.path]::Combine('Release', [io.path]::Combine('Package', [io.path]::Combine('PackageTmp', 'Web.config')))))
	Write-Host "Project: $projectFile"
	Write-Host "Copying package files from obj\Release to obj\$config"
	$markerFile = [io.path]::Combine($projectRoot, "CMTransformWeb.marker")
	if(Test-Path $markerFile){
		Write-Host "Marker file found..."
		$source = [io.path]::Combine($projectRoot, [io.path]::Combine('obj', [io.path]::Combine('Release', [io.path]::Combine('Package', 'PackageTmp'))))
		$target = [io.path]::Combine($projectRoot, [io.path]::Combine('obj', [io.path]::Combine('CWS{0}' -f $config, [io.path]::Combine('Package', 'PackageTmp'))))
		New-Item $target -type directory
		Copy-Item -Path "$source\*" -Destination $target -Recurse -Exclude 'Web.config' -Force
		$source = [io.path]::Combine($projectRoot, [io.path]::Combine('bin', [io.path]::Combine($config, 'Web.CWS.config')))
		$target = [io.path]::Combine($projectRoot, [io.path]::Combine('obj', [io.path]::Combine('CWS{0}' -f $config, [io.path]::Combine('Package', [io.path]::Combine('PackageTmp','Web.config')))))
		Copy-Item $source $target
	}
}

function AssembleFilesForPackaging($projectFile, $config, $winVer){
	Write-Host "AssembleFilesForPackaging Starting..."
	$projectRoot = Split-Path $projectFile
	$tempPackage = "{0}\temp.zip" -f $sdlLocation
	$parameters = "Configuration={0};PackageLocation={1};AutoParameterizationWebConfigConnectionStrings=False;Platform=AnyCPU" -f $config, $tempPackage
	if ($config -eq "Release"){
		"Verbosity={0}" -f $verbosity
		"winVer={0}" -f $winVer
		$cmd = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild"
		if ($winVer = 'win32'){
			$cmd = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild"
		}
		& $cmd $projectFile /t:Package /P:$parameters /V:$verbosity
		$source = [io.path]::Combine($projectRoot, 'bin')
		$target = [io.path]::Combine($projectRoot, 'obj\release\Package\PackageTmp\bin')
		if((Test-Path $target) -ne $true){
			New-Item $target -itemtype directory
		}
		#Copy-Item -Path "$source\*" -Destination $target -Recurse -Force
		& robocopy $source $target /fp
	}
	else{
		$configFile = [io.path]::Combine($projectRoot, [io.path]::Combine('obj', [io.path]::Combine('Release', [io.path]::Combine('Package', [io.path]::Combine('PackageTmp', 'Web.config')))))
		Write-Host "Project: $projectFile"
		Write-Host "Copying package files from obj\Release to obj\$config"
		$markerFile = [io.path]::Combine($projectRoot, "CMTransformWeb.marker")
		if(Test-Path $markerFile){
			Write-Host "Marker file found..."
			$source = [io.path]::Combine($projectRoot, [io.path]::Combine('obj', [io.path]::Combine('Release', [io.path]::Combine('Package', 'PackageTmp'))))
			$target = [io.path]::Combine($projectRoot, [io.path]::Combine('obj', [io.path]::Combine($config, [io.path]::Combine('Package', 'PackageTmp'))))
			New-Item $target -type directory
			Copy-Item -Path "$source\*" -Destination $target -Recurse -Exclude $configFile -Force
			$source = [io.path]::Combine($projectRoot, [io.path]::Combine('bin', [io.path]::Combine($config, 'Web.config')))
			Copy-Item $source $target
		}
	}
}

$production = @{ EnvironmentCode = "PRD"; Configuration = "Release" }
$staging = @{ EnvironmentCode = "STG"; Configuration = "Staging" }
$qa = @{ EnvironmentCode = "QA"; Configuration = "Testing" }
$configurations = $production, $staging, $qa	#$production must be built first: faster build, successful package creation depends on order.

foreach($configuration in $configurations){
        $sdlLocation = "\\ns-cor-dpl03-e\SDL\SDL\Deploy2.0\{0}\{1}\{2}" -f $tenantCode, $configuration.EnvironmentCode, $buildNumber
        $databaseScripts = "{0}\DatabaseScripts" -f $sdlLocation
        
        if(Test-Path $databaseScripts){
            "Removing Path {0}" -f $sdlLocation
            Remove-Item $sdlLocation -Recurse -Force
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
        
        
        $tempPackage = "{0}\temp.zip" -f $sdlLocation
        $parameters = "Configuration={0};PackageLocation={1};AutoParameterizationWebConfigConnectionStrings=False;Platform=AnyCPU" -f $configuration.Configuration, $tempPackage
        
        #-------------------------------------------   
        #--------------------AUT--------------------
        #-------------------------------------------
        $projectFile = "{0}\{1}ConsoleApplications\AutoshipProcessor\AutoshipProcessor.csproj" -f $root, $framework
        CopyBinRelease $projectFile $configuration.Configuration
         if(Test-Path $projectFile){
            $source = "{0}\{2}ConsoleApplications\AutoshipProcessor\bin\{1}\" -f $root, $configuration.Configuration, $framework
            $package = "{0}\AUT-{1}.zip" -f $sdlLocation, $configuration.Configuration
            PackageForDistribution $source $package $root
        }
        
        #-------------------------------------------   
        #--------------------QUE--------------------
        #-------------------------------------------
        $projectFile = "{0}\{1}Windows Services\QueueProcessing\NetSteps.QueueProcessing.Host\NetSteps.QueueProcessing.Host.csproj" -f $root, $framework
        CopyBinRelease $projectFile $configuration.Configuration
        if(Test-Path $projectFile){
            $source = "{0}\{2}Windows Services\QueueProcessing\NetSteps.QueueProcessing.Host\bin\{1}\" -f $root, $configuration.Configuration, $framework
            $package = "{0}\QUE-{1}.zip" -f $sdlLocation, $configuration.Configuration
            PackageForDistribution $source $package $root
        }
        
	#-------------------------------------------     
        #--------------------EVP--------------------
        #-------------------------------------------
        $projectFile = "{0}\Services\EventProcessing.Host\EventProcessing.Host.csproj" -f $root
        if(Test-Path $projectFile){
            AssembleFilesForPackaging $projectFile $configuration.Configuration 'win64'
            $source = "{0}\Services\EventProcessing.Host\bin\{1}\" -f $root, $configuration.Configuration
            $package = "{0}\EVP-{1}.zip" -f $sdlLocation, $configuration.Configuration
            PackageForDistribution $source $package $root
        }

	#-------------------------------------------     
        #--------------------EVA--------------------
        #-------------------------------------------
        $projectFile = "{0}\Services\EventProcessing.Assemblies\EventProcessing.Assemblies.csproj" -f $root
        if(Test-Path $projectFile){
            AssembleFilesForPackaging $projectFile $configuration.Configuration 'win64'
            $source = "{0}\Services\EventProcessing.Assemblies\bin\{1}\" -f $root, $configuration.Configuration
            $package = "{0}\EVA-{1}.zip" -f $sdlLocation, $configuration.Configuration
            PackageForDistribution $source $package $root
        }        

        #-------------------------------------------     
        #--------------------RST--------------------
        #-------------------------------------------
        $projectFile = "{0}\{1}Websites\Encore.ApiSite\Encore.ApiSite.csproj" -f $root, $framework
        if(Test-Path $projectFile){
            AssembleFilesForPackaging $projectFile $configuration.Configuration 'win64'
            $source = "{0}\{2}Websites\Encore.ApiSite\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration, $framework
            $package = "{0}\RST-{1}.zip" -f $sdlLocation, $configuration.Configuration
            PackageForDistribution $source $package $root
        }
        
        #-------------------------------------------     
        #--------------------DWS--------------------
        #-------------------------------------------
        $projectFile = "{0}\{1}Websites\DistributorBackOffice\DistributorBackOffice.csproj" -f $root, $framework
        if(Test-Path $projectFile){
            AssembleFilesForPackaging $projectFile $configuration.Configuration 'win64'
            $source = "{0}\{2}Websites\DistributorBackOffice\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration, $framework
            $package = "{0}\DWS-{1}.zip" -f $sdlLocation, $configuration.Configuration
            PackageForDistribution $source $package $root
        }
        
        #-------------------------------------------     
        #--------------------GMP--------------------
        #-------------------------------------------
        $projectFile = "{0}\{1}Websites\nsCore\nsCore.csproj" -f $root, $framework
        if(Test-Path $projectFile){
            AssembleFilesForPackaging $projectFile $configuration.Configuration 'win64'
            $source = "{0}\{2}Websites\nsCore\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration, $framework
            $package = "{0}\GMP-{1}.zip" -f $sdlLocation, $configuration.Configuration
            PackageForDistribution $source $package $root
        }
        
        #-----------------------------------------------     
        #--------------------PWS/CWS--------------------
        #-----------------------------------------------
        $projectFile = "{0}\{1}Websites\nsDistributor\nsDistributor.csproj" -f $root, $framework
        if(Test-Path $projectFile){
            AssembleFilesForPackaging $projectFile $configuration.Configuration 'win64'
            $source = "{0}\{2}Websites\nsDistributor\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration, $framework
            $package = "{0}\PWS-{1}.zip" -f $sdlLocation, $configuration.Configuration
            PackageForDistribution $source $package $root
        
            $cwsConfigFile = "{0}\{1}Websites\nsDistributor\Web.CWS.{2}.config" -f $root, $framework, $configuration.Configuration
            $package = "{0}\CWS-{1}.zip" -f $sdlLocation, $configuration.Configuration
            if(Test-Path $cwsConfigFile){
            	AssembleCWSFilesForPackaging $cwsConfigFile $configuration.Configuration
            	$source = "{0}\{2}Websites\nsDistributor\obj\CWS{1}\Package\PackageTmp" -f $root, $configuration.Configuration, $framework
            }
            PackageForDistribution $source $package $root
        }
        
        #-------------------------------------------     
        #--------------------WCF--------------------
        #-------------------------------------------
		$projectFile = "{0}\Websites\WCFAPI\WCFAPI.csproj" -f $root
        if(Test-Path $projectFile){
            AssembleFilesForPackaging $projectFile $configuration.Configuration 'win64'
            $source = "{0}\Websites\WCFAPI\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
            $package = "{0}\WCF-{1}.zip" -f $sdlLocation, $configuration.Configuration
            PackageForDistribution $source $package $root
        }

        #---------------------------------------------     
        #--------------------Miche--------------------
        #---------------------------------------------
        $projectFile = "{0}\Websites\HttpRedirect\HttpRedirectModule.csproj" -f $root
        if(Test-Path $projectFile){
            AssembleFilesForPackaging $projectFile $configuration.Configuration 'win64'
            $source = "{0}\Websites\HttpRedirect\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
            $package = "{0}\MRE-{1}.zip" -f $sdlLocation, $configuration.Configuration
            PackageForDistribution $source $package $root
        }    

        $projectFile = "{0}\ConsoleApplications\CreateThumbnails\CreateThumbnails.csproj" -f $root
        CopyBinRelease $projectFile $configuration.Configuration
        if(Test-Path $projectFile){
            $source = "{0}\ConsoleApplications\CreateThumbnails\bin\{1}\" -f $root, $configuration.Configuration
            $package = "{0}\MTH-{1}.zip" -f $sdlLocation, $configuration.Configuration
            PackageForDistribution $source $package $root
        }

        #-------------------------------------------------     
        #--------------------PartyLite--------------------
        #-------------------------------------------------
	$projectFile = "{0}\Websites\PartyLite.DistributorBackOfficeMobile\DistributorBackOfficeMobile.csproj" -f $root
        if(Test-Path $projectFile){
            AssembleFilesForPackaging $projectFile $configuration.Configuration 'win64'
            $source = "{0}\Websites\PartyLite.DistributorBackOfficeMobile\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
            $package = "{0}\PDM-{1}.zip" -f $sdlLocation, $configuration.Configuration
            PackageForDistribution $source $package $root
        }

	$projectFile = "{0}\Websites\PartyLite.Services.Api.Website\PartyLite.Services.Api.Website.csproj" -f $root
        if(Test-Path $projectFile){
            AssembleFilesForPackaging $projectFile $configuration.Configuration 'win64'
            $source = "{0}\Websites\PartyLite.Services.Api.Website\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
            $package = "{0}\PAP-{1}.zip" -f $sdlLocation, $configuration.Configuration
            PackageForDistribution $source $package $root
        }

	$projectFile = "{0}\Websites\WebService.Mobile\WebService.Mobile.csproj" -f $root
        if(Test-Path $projectFile){
            AssembleFilesForPackaging $projectFile $configuration.Configuration 'win64'
            $source = "{0}\Websites\WebService.Mobile\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
            $package = "{0}\PWM-{1}.zip" -f $sdlLocation, $configuration.Configuration
            PackageForDistribution $source $package $root
        }        

	$projectFile = "{0}\PartyLite.ShippingMethods\PartyLite.Tasks.ShippingMethods.csproj" -f $root
        if(Test-Path $projectFile){
            AssembleFilesForPackaging $projectFile $configuration.Configuration 'win64'
            $source = "{0}\PartyLite.ShippingMethods\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
            $package = "{0}\PSM-{1}.zip" -f $sdlLocation, $configuration.Configuration
            PackageForDistribution $source $package $root
        }        

        #-------------------------------------------------     
        #--------------------JewelKade--------------------
        #-------------------------------------------------
	$projectFile = "{0}\Integrations\Services\OrderFulfillment\Service\WebService\IntegrationsService.csproj" -f $root
	if(Test-Path $projectFile){
		AssembleFilesForPackaging $projectFile $configuration.Configuration 'win64'
		$source = "{0}\Integrations\Services\OrderFulfillment\Service\WebService\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
		$package = "{0}\JIS-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}

	$projectFile = "{0}\GPIntegration\GPIntegration\GPIntegration.csproj" -f $root
	CopyBinRelease $projectFile $configuration.Configuration
	if(Test-Path $projectFile){
		$source = "{0}\GPIntegration\GPIntegration\bin\{1}\" -f $root, $configuration.Configuration
		$package = "{0}\JGP-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}
	
	$projectFile = "{0}\ShipmentTrackingImport\ShipmentTrackingImport.csproj" -f $root
	CopyBinRelease $projectFile $configuration.Configuration
	if(Test-Path $projectFile){
		$source = "{0}\ShipmentTrackingImport\bin\{1}\" -f $root, $configuration.Configuration
		$package = "{0}\STI-{1}.zip" -f $sdlLocation, $configuration.Configuration
		PackageForDistribution $source $package $root
	}
		
        #-------------------------------------------------     
        #--------------------Natura-----------------------
        #-------------------------------------------------
	$projectFile = "{0}\NaturaMexico API\NaturaMexico.ApiSite\NaturaMexico.ApiSite.csproj" -f $root
    if(Test-Path $projectFile){
        AssembleFilesForPackaging $projectFile $configuration.Configuration 'win64'
        $source = "{0}\NaturaMexico API\NaturaMexico.ApiSite\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
        $package = "{0}\NAP-{1}.zip" -f $sdlLocation, $configuration.Configuration
        PackageForDistribution $source $package $root
    }

	$projectFile = "{0}\NaturaLegacy\Common\Websites\nsCore4\nsCore.csproj" -f $root
    if(Test-Path $projectFile){
        AssembleFilesForPackaging $projectFile $configuration.Configuration 'win64'
        $source = "{0}\NaturaLegacy\Common\Websites\nsCore4\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
        $package = "{0}\NAD-{1}.zip" -f $sdlLocation, $configuration.Configuration
        PackageForDistribution $source $package $root
    }

	$projectFile = "{0}\NaturaLegacy\Natura\Natura.Corporate\Natura.Corporate.csproj" -f $root
    if(Test-Path $projectFile){
        AssembleFilesForPackaging $projectFile $configuration.Configuration 'win32'
        $source = "{0}\NaturaLegacy\Natura\Natura.Corporate\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
        $package = "{0}\NCO-{1}.zip" -f $sdlLocation, $configuration.Configuration
        PackageForDistribution $source $package $root
    }

	$projectFile = "{0}\NaturaLegacy\Natura\Natura.Social\Natura.Social.csproj" -f $root
    if(Test-Path $projectFile){
        AssembleFilesForPackaging $projectFile $configuration.Configuration 'win32'
        $source = "{0}\NaturaLegacy\Natura\Natura.Social\obj\{1}\Package\PackageTmp" -f $root, $configuration.Configuration
        $package = "{0}\NSO-{1}.zip" -f $sdlLocation, $configuration.Configuration
        PackageForDistribution $source $package $root
    }

	$projectFile = "{0}\NaturaLegacy\Natura\UpdateFriendlyUrls\UpdateFriendlyUrls.csproj" -f $root
	CopyBinRelease $projectFile $configuration.Configuration
	if(Test-Path $projectFile){
        $source = "{0}\NaturaLegacy\Natura\UpdateFriendlyUrls\bin\{1}\" -f $root, $configuration.Configuration
        $package = "{0}\NFR-{1}.zip" -f $sdlLocation, $configuration.Configuration
        PackageForDistribution $source $package $root
    }
	Write-Host ('Removing temp files from {0}' -f $sdlLocation);
	Remove-Item ("{0}\temp*.*" -f $sdlLocation) -Force -ErrorAction:SilentlyContinue
}