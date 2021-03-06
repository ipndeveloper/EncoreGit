param(
    [Parameter(Mandatory=$True)][string]$where
)

$buildOverlayDir = [System.IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Path)
$targetDir = (Resolve-Path ($where)).Path

function RemoveAssemblyVersionsFromFile($messages, $prj, [string] $filename) {
    $textout = New-Object Collections.Generic.List[String]
    $infile = New-Object System.IO.StreamReader -Arg $filename
    $list = New-Object Collections.Generic.List[String]
    $prj.IncrementTouchPoints()
    $line = $infile.ReadLine()
    while((!($line -eq $null))) {
        if ($line.Contains("AssemblyVersion") -or $line.Contains("AssemblyFileVersion") ) {
            $prj.IncrementTouchPoints()
            $textout.Add("!! removed: $line")
        } else {
            $list.Add($line)
        } 
        $line = $infile.ReadLine()   
    }
    $infile.Close()

    if ($textout.Count -gt 0) {
        $outfile = New-Object System.IO.StreamWriter -Arg $filename
        $list | foreach {
            $outfile.WriteLine($_)
        }
        $outfile.Close()
        $messages.Write(">> processing $filename", 'Red')
        $messages.Indent()
        $textout | foreach { 
            $messages.Write($_, 'Red') 
        }
        $messages.Outdent()
    }
}

function EnsureBuildConfigHasCorrectCodeContractSettings($messages, $prj, $config) {
    $cond = $config.GetAttribute("Condition")
    $messages.Write(">> Configuration: $cond", 'White')
    $messages.Indent()
    @(
        @{ 
        "Name" = "CodeContractsEnableRuntimeChecking" 
        "Value" = "True" 
        },
        @{ 
        "Name" = "CodeContractsRuntimeOnlyPublicSurface" 
        "Value" = "False" 
        },
        @{ 
        "Name" = "CodeContractsRuntimeThrowOnFailure" 
        "Value" = "True" 
        },
        @{ 
        "Name" = "CodeContractsRuntimeCallSiteRequires" 
        "Value" = "False" 
        },
        @{ 
        "Name" = "CodeContractsRuntimeSkipQuantifiers" 
        "Value" = "False" 
        },
        @{ 
        "Name" = "CodeContractsRunCodeAnalysis" 
        "Value" = "False" 
        },
        @{ 
        "Name" = "CodeContractsNonNullObligations" 
        "Value" = "False" 
        },
        @{ 
        "Name" = "CodeContractsBoundsObligations" 
        "Value" = "False" 
        },
        @{ 
        "Name" = "CodeContractsArithmeticObligations" 
        "Value" = "False" 
        },
        @{ 
        "Name" = "CodeContractsEnumObligations" 
        "Value" = "False" 
        },
        @{ 
        "Name" = "CodeContractsRedundantAssumptions" 
        "Value" = "False" 
        },
        @{ 
        "Name" = "CodeContractsRunInBackground" 
        "Value" = "True" 
        },
        @{ 
        "Name" = "CodeContractsUseBaseLine" 
        "Value" = "False" 
        },
        @{ 
        "Name" = "CodeContractsEmitXMLDocs" 
        "Value" = "True" 
        },
        @{ 
        "Name" = "CodeContractsRuntimeCheckingLevel" 
        "Value" = "Full" 
        },
        @{ 
        "Name" = "CodeContractsReferenceAssembly" 
        "Value" = "Build" 
        },
        @{ 
        "Name" = "CodeContractsAnalysisWarningLevel" 
        "Value" = "0" 
        },
        @{ 
        "Name" = "CodeAnalysisRuleSet" 
        "Value" = "BasicCorrectnessRules.ruleset" 
        }
    ) | foreach {
        $nodes = $config.GetElementsByTagName($_.Name)
        $setting = $_.Name + " = " + $_.Value
        if ($nodes.count -eq 0) {
            $prj.IncrementTouchPoints()            
            $node = $prj.Contents.CreateElement($_.Name)
            $node.InnerText = $_.Value
            $unused = $config.AppendChild($node)
            $prj.MarkUpdated()
            $messages.Write(">> Added missing code contract setting $setting", 'Yellow')
        } else {
            $item = $_
            $first = $null
            $removals = @()
            $nodes | foreach {
                $prj.IncrementTouchPoints()
                if ($first -eq $null) {
                    $first = $_
                    if ($first.InnerText -ne $item.Value) {
                        $first.InnerText = $item.Value
                        $prj.MarkUpdated()
                        $messages.Write(">> Corrected code contract setting $setting", 'Yellow')
                    }
                } else {
                    $removals += $_
                    $prj.MarkUpdated()
                    $messages.Write(">> Removed duplicate code contract setting $setting.", 'Yellow')
                }
            }
            $removals | foreach { $unused = $_.ParentNode.RemoveChild($_) }
        } 
    }
    $messages.Outdent()
}

function EnsureCodeContractsAreOnForAllBuildCondigurations($messages, $prj) {
    $node = $prj.Contents.SelectSingleNode("//p:CodeContractsAssemblyMode", $prj.Namespace)
    $prj.IncrementTouchPoints()
    if ($node -eq $null) {
        # No CodeContractsAssemblyMode, create one...
        $sibling = $prj.Contents.SelectSingleNode("//p:ProjectGuid", $prj.Namespace)
        if ($sibling -eq $null) {
            $messages.Write("!! Unable to insert CodeContractsAssemblyMode node due to missing ProjectGuid.", 'Red')
        } else {
            $insert = $prj.Contents.CreateElement('CodeContractsAssemblyMode')
            $insert.InnerText = '1'
            $unused = $sibling.ParentNode.AppendChild($insert)
            $messages.Write(">> Added missing CodeContractsAssemblyMode=1", 'Yellow')
        }                        
    } else {
        if ($node.InnerText -ne '1') {
            $node.InnerText = '1'
            $messages.Write(">> Corrected CodeContractsAssemblyMode=1", 'Yellow')
        }
    }

    $configs = $prj.Contents.SelectNodes("//p:PropertyGroup[contains(@Condition,'`$(Configuration)|')]", $prj.Namespace)
    $configs | foreach {
        EnsureBuildConfigHasCorrectCodeContractSettings $messages $prj $_
    }
}

function EnsureProjectUsesGenerateddVersionStamp($messages, $prj, $projDir) {
    $asminfo = $prj.Contents.SelectSingleNode("//p:Compile[contains(@Include,'Properties\AssemblyInfo.cs')]", $prj.Namespace)
    $prj.IncrementTouchPoints()
    if ($asminfo -eq $null) {
        $messages.Write("!! Missing reference to file: Properties\AssemblyInfo.cs", 'Red')
    } else {
        $file = [System.String]::Concat($projDir, '\Properties\AssemblyInfo.cs')
        if (Test-Path $file) {
            RemoveAssemblyVersionsFromFile $messages $prj $file 
        }
        $node = $prj.Contents.SelectSingleNode("//p:Compile[contains(@Include,'Properties\AssemblyInfo.version.cs')]", $prj.Namespace)
        if ($node -eq $null) {
            $insert = $prj.Contents.CreateElement('Compile')
            $insert.SetAttribute('Include','Properties\AssemblyInfo.version.cs')
            $unused = $asminfo.ParentNode.PrependChild($insert)
            $messages.Write(">> Added Compile: Properties\AssemblyInfo.version.cs", 'Yellow')
        }
    }
}

function RemoveUnusedProjectNodes($messages, $prj, $remove) {
    $remove | foreach {
        $prj.IncrementTouchPoints()
        $node = $prj.Contents.SelectNodes($_, $prj.Namespace)
        if ($node.count -gt 0) {
            $item = $_
            $node | foreach { 
                $unused = $_.ParentNode.RemoveChild($_)
                $prj.MarkUpdated()
                $messages.Write(">> Removed unused item: xpath($item).", 'Yellow')
            }
        }
    }
}

function EnsureProjectHasHomeSolutionNode($messages, $prj, [string] $slnFile) {
    #
    # Ensures the project has a HomeSolution node in the project's xml
    #
    $res = $true
    $homeSolution = $prj.Contents.SelectNodes("//p:HomeSolution", $prj.Namespace)
    if ($homeSolution.count -eq 0) {
        $prj.IncrementTouchPoints()
        $sibling = $prj.Contents.SelectSingleNode("//p:ProjectGuid", $prj.Namespace)
        if ($sibling -eq $null) {
            $messages.Write("!! Unable to insert HomeSolution node due to missing ProjectGuid.", 'Red')
            return $false
        }
        $node = $prj.Contents.CreateElement('HomeSolution')
        $node.InnerText = $slnFile
        $unused = $sibling.ParentNode.PrependChild($node)
        $prj.MarkUpdated()
        $messages.Write(">> Inserted HomeSolution node", 'Yellow')
    } else {
        $first = $null
        $homeSolution | foreach {
            $prj.IncrementTouchPoints()
            if ($first -eq $null) {
                $first = $_
                if ($first.InnerText -ne $slnFile) {
                    $elsewhere = $first.InnerText
                    $messages.Write(">> HomeSolution is $elsewhere... skipping", 'Gray')
                    $res  = $false
                    return
                }
            } else {
                $unused = $_.ParentNode.RemoveChild($_)
                $prj.MarkUpdated()
                $messages.Write(">> Removed duplicate HomeSolution.", 'Yellow')
            }
        }
    }
    return $res
}

function EnsureNuGetPackageReferencesAreCorrectlyRooted($messages, $prj, [string] $backpath) {
    $packagesDir = $backpath + 'packages\'
    $nodes = $prj.Contents.SelectNodes("//p:HintPath[contains(text(),'packages\')]", $prj.Namespace)
    $nodes | foreach {
        $prj.IncrementTouchPoints()
        if ((!($_.InnerText.StartsWith($packagesDir)))) {
            $was = $_.InnerText
            $ofs = $_.InnerText.IndexOf('packages\')
            $willBe = [System.String]::Concat($packagesDir, $_.InnerText.Substring($ofs + 9))
            $_.InnerText = $willBe
            $prj.MarkUpdated()
            $messages.Write(">> fixed reference: $willbe", 'Yellow')
            $messages.Write(">>             was: $was", 'Yellow')
        }
    }
}

function EnsureBuildOverlayFromSourceToTarget($messages, [string] $sourceDir, [string] $targetDir) {
    Get-ChildItem ($sourceDir + '\*') | foreach {
        $item = $_.Name
        if ($item.StartsWith("remove-me-")) {
            $remove = Join-Path -Path $targetDir -ChildPath $item.Substring(10)
            if (Test-Path $remove) {
                Remove-Item $remove
                $messages.Write(">> removed obsolete build file: $remove", 'Yellow')
            }
        } else {
            $copy = Join-Path -Path $targetDir -ChildPath $item
            if (Test-Path $copy) {
                $diff = Compare-Object $(Get-Content $_.FullName) $(Get-Content $copy)
                if ($diff.count -gt 0) {
                    Copy-Item $_.FullName $copy -Force
                    $messages.Write(">> updated build file copied: $copy", 'Yellow')
                }
            } else {
                Copy-Item $_.FullName $copy -Force
                $messages.Write(">> missing build file restored: $copy", 'Yellow')
            }
        }
    }
}

function EnsureProjectReferencesBuildOverlayTargets($messages, $prj, [string] $prjDir, $backpath) {
    $beforeTargets = '$(MSBuildThisFileDirectory)' + $backpath + "csproj-import-before.targets"
    $beforeCondition = 'Exists(''' + $beforeTargets + ''')'
    $nodes = $prj.Contents.SelectNodes('//p:Import[contains(@Project,''' + $beforeTargets + ''')]', $prj.Namespace)
    if ($nodes.count -gt 0) {
        $item = $_
        $first = $null
        $removals = @()
        $nodes | foreach {
            $prj.IncrementTouchPoints()
            if ($first -eq $null) {
                $first = $_
                if ($first.GetAttribute('Project') -ne $beforeTargets -or $first.GetAttribute('Condition') -ne $beforeCondition) {
                    $first.SetAttribute('Project', $beforeTargets)
                    $first.SetAttribute('Condition',$beforeCondition)
                    $prj.MarkUpdated()
                    $messages.Write(">> corrected import $beforeTargets", 'Yellow')
                }
            } else {
                $removals += $_
                $prj.MarkUpdated()
                $messages.Write(">> removed duplicate import $beforeTargets.", 'Yellow')
            }
        }
        $removals | foreach { $unused = $prj.Contents.Project.RemoveChild($_) }
    } else {
        $prj.IncrementTouchPoints()
        $insert = $prj.Contents.CreateElement('Import')
        $insert.SetAttribute('Project', $beforeTargets)
        $insert.SetAttribute('Condition',$beforeCondition)
        $temp = $prj.Contents.Project.PrependChild($insert)
        $prj.MarkUpdated()
        $messages.Write(">> Inserted missing import at beginning of project: csproj-import-before.targets", 'Yellow')
    }

    $afterTargets = '$(MSBuildThisFileDirectory)' + $backpath + "csproj-import-after.targets"
    $afterCondition = 'Exists(''' + $afterTargets + ''')'
    $nodes = $prj.Contents.SelectNodes('//p:Import[contains(@Project,''' + $afterTargets + ''')]', $prj.Namespace)
    if ($nodes.count -gt 0) {
        $item = $_
        $first = $null
        $removals = @()
        $nodes | foreach {
            $prj.IncrementTouchPoints()
            if ($first -eq $null) {
                $first = $_
                if ($first.GetAttribute('Project') -ne $afterTargets -or $first.GetAttribute('Condition') -ne $afterCondition) {
                    $first.SetAttribute('Project', $afterTargets)
                    $first.SetAttribute('Condition',$afterCondition)
                    $prj.MarkUpdated()
                    $messages.Write(">> corrected import $afterTargets", 'Yellow')
                }
            } else {
                $removals += $_
                $prj.MarkUpdated()
                $messages.Write(">> removed duplicate import $afterTargets.", 'Yellow')
            }
        }
        $removals | foreach { $unused = $prj.Contents.Project.RemoveChild($_) }
    } else {
        $prj.IncrementTouchPoints()
        $insert = $prj.Contents.CreateElement('Import')
        $insert.SetAttribute('Project', $afterTargets)
        $insert.SetAttribute('Condition',$afterCondition)
        $temp = $prj.Contents.Project.AppendChild($insert)
        $prj.MarkUpdated()
        $messages.Write(">> Inserted missing import at end of project: csproj-import-after.targets", 'Yellow')
    }
}

function EnsureProjectHasCorrectSolutionDir($messages, $prj, [string] $backpath) {
    $condition = '$(SolutionDir) == '''' Or $(SolutionDir) == ''*Undefined*'''
    $nodes = $prj.Contents.SelectNodes("//p:SolutionDir", $prj.Namespace)
    if ($nodes.count -eq 0) {
        $prj.IncrementTouchPoints()        
        $sibling = $prj.Contents.SelectSingleNode("//p:ProjectGuid", $prj.Namespace)
        if ($sibling -eq $null) {
            $messages.Write("!! Unable to insert SolutionDir node due to missing ProjectGuid.", 'Red')
            return $false
        }
        $node = $prj.Contents.CreateElement('SolutionDir')
        $node.SetAttribute('Condition', $condition)
        $node.InnerText = $backpath
        $unused = $sibling.ParentNode.PrependChild($node)
        $prj.MarkUpdated()
        $messages.Write(">> Inserted SolutionDir = $backpath", 'Yellow')
    } else {
        $first = $null
        $nodes | foreach {
            $prj.IncrementTouchPoints()            
            if ($first -eq $null) {
                $first = $_
                if ($first.InnerText -ne $backpath) {
                    $first.InnerText = $backpath
                    $first.SetAttribute('Condition', $condition)
                    $prj.MarkUpdated()
                    $messages.Write(">> Corrected SolutionDir = $backpath.", 'Yellow')
                }
            } else {
                $unused = $_.ParentNode.RemoveChild($_)
                $prj.MarkUpdated()
                $messages.Write(">> Removed duplicate SolutionDir.", 'Yellow')
            }
        }
    } 
}

function CreateMessagesObj() {
    return New-Object Object | 
        Add-Member NoteProperty IndentValue 0 -PassThru |
        Add-member ScriptMethod Indent {
            $this.IndentValue = $this.IndentValue + 1;
        } -PassThru |
        Add-member ScriptMethod Outdent {
            $this.IndentValue = $this.IndentValue - 1;
        } -PassThru |
        Add-member ScriptMethod Write {
            param($text, $color)
            $indent = "  " * $this.IndentValue
            $message = "$indent$text"
            Write-Host $message -foregroundcolor $color
        } -PassThru
}

function CreateNuGetPackageVersion($version) {
    return New-Object Object | 
        Add-Member NoteProperty Version $version -PassThru |
        Add-Member NoteProperty Projects $() -PassThru |
        Add-member ScriptMethod AddProject {
            param($prjName)
            $this.Projects += $prjName
        } -PassThru    
}

function CreateNuGetPackage($package) {
    return New-Object Object | 
        Add-Member NoteProperty Package $package -PassThru |
        Add-Member NoteProperty Versions @{} -PassThru |
        Add-member ScriptMethod AddProject {
            param($prjName, $version)
            if ($this.Versions.ContainsKey($version)) {
                $rec = $this.Versions.Get_Item($version)
                $rec.AddProject($prjName)
            } else {
                $rec = CreateNuGetPackageVersion($version)
                $rec.AddProject($prjName)
                $this.Versions.Add($version, $rec)
            }
        } -PassThru    
}

function CreateSolutionObj([string] $solutionPath) {
    return New-Object Object | 
        Add-Member NoteProperty SolutionPath $solutionPath -PassThru |
        Add-Member NoteProperty TouchPoints 0 -PassThru |
        Add-Member NoteProperty FixCount 0 -PassThru |
        Add-Member NoteProperty Errors @{} -PassThru |
        Add-Member NoteProperty NuGetPackages @{} -PassThru |
        Add-member ScriptMethod AddProjectFixes {
            param($touchPoints, $fixes)
            $this.TouchPoints = $this.TouchPoints + $touchPoints
            $this.FixCount = $this.FixCount + $fixes
        } -PassThru |
        Add-member ScriptMethod RecordError {
            param($prjName, [array]$errors)
            if ((!($this.Errors.ContainsKey($prjName)))) {
                $rec = @{ "Project" = $prjName; "Errors" = ,$errors }
                $this.Errors.Add($prjName, $rec)
                return $rec
            }
        } -PassThru |
        Add-member ScriptMethod RecordNuGetPackageReference {
            param($package, $version, $prjName)
            if ($this.NuGetPackages.ContainsKey($package)) {
                $rec = $this.NuGetPackages.Get_Item($package)
                $rec.AddProject($prjName, $version)
            } else {
                $rec = CreateNuGetPackage($package)
                $rec.AddProject($prjName, $version)
                $this.NuGetPackages.Add($package, $rec)
            }
        } -PassThru |
        Add-member ScriptMethod Close {
            param($messages)
            $errorCount = 0
            $errors = $this.Errors
            $errors.GetEnumerator() | foreach {
                $errorCount = $errorCount + $_.Value.Errors.count
            }
            $text = 'Solution Total: checked ' + $this.TouchPoints + ' items'
            $color = 'White'
            if ($this.FixCount -gt 0) {
                $text = $text + ', fixed ' + $this.FixCount             
                $color = 'Yellow'
            }
            if ($errorCount -gt 0) {
                $text = $text + '; there are ' + $errorCount + ' errors that need to be fixed.'     
                $messages.Write($text, 'Red')
            } else {
                $messages.Write($text + '.', $color)
            }
            if ($errorCount -gt 0) {
                $messages.Indent()
                $this.Errors.GetEnumerator() | Sort-Object  Name | foreach {
                    $item = $_.Value
                    $messages.Write($item.Project, 'Red')
                    $messages.Indent()
                    $item.Errors | foreach { 
                        $messages.Write($_, 'Red') 
                    }
                    $messages.Outdent()
                }
                $messages.Outdent()
            }          
        } -PassThru
}

function CreateProjectObj($solution, [string] $prjPath) {
        [xml]$xml = Get-Content -Path $prjPath
        $ns = New-Object System.Xml.XmlNamespaceManager -ArgumentList $xml.NameTable
        $ns.AddNamespace('p','http://schemas.microsoft.com/developer/msbuild/2003')
        return New-Object Object | 
            Add-Member NoteProperty Solution $solution -PassThru |
            Add-Member NoteProperty ProjectPath $prjPath -PassThru |
            Add-Member NoteProperty Contents $xml -PassThru |
            Add-Member NoteProperty Namespace $ns -PassThru |
            Add-Member NoteProperty NeedsSave $false -PassThru |
            Add-Member NoteProperty TouchPoints 0 -PassThru |            
            Add-Member NoteProperty FixCount 0 -PassThru |
            Add-Member NoteProperty Errors $null -PassThru |
            Add-member ScriptMethod MarkUpdated {
                $this.NeedsSave = $true
                $this.FixCount = $this.FixCount + 1
            } -PassThru |
            Add-member ScriptMethod RecordError {
                param($text)
                if ($this.Errors -eq $null) {
                    $this.Errors = $this.Solution.RecordError($this.ProjectPath, (,$text))
                } else {
                    $this.Errors.Errors += $text
                }
            } -PassThru |
            Add-member ScriptMethod IncrementTouchPoints {
                $this.TouchPoints = $this.TouchPoints + 1
            } -PassThru |
            Add-member ScriptMethod Close {
                param($messages)
                $errors = $this.Errors.Errors
                $color = 'White'
                $text = 'Project Total: checked ' + $this.TouchPoints + ' items'
                if ($this.NeedsSave) {
                    $tmp = [xml] $this.Contents.OuterXml.Replace(" xmlns=`"`"", "")
                    $tmp.Save($prjPath)
                    $text = $text + ', fixed ' + $this.FixCount
                    $color = 'Yellow'
                }
                if ($errors.length -gt 0) {
                    $text = $text + '; there are ' + $errors.count + ' errors that need to be fixed.'
                    $color = 'Red'
                }
                $messages.Write($text, $color)
                if ($errors -ne $null -and $errors.length -gt 0) {
                    $messages.Indent()
                    $errors | foreach { 
                        $messages.Write($_, 'Red') 
                    }
                    $messages.Outdent()
                }
                $this.Solution.AddProjectFixes($this.TouchPoints, $this.FixCount)
            } -PassThru
}

function SanityCheckNugetPackagesForProject($messages, $prj, [string]$prjDir, $backpath) {    
    $packagesConfig = Join-Path -Path $prjDir -ChildPath 'packages.config'
    if (Test-Path $packagesConfig) {        
        $thisPackages = @()
        $pkgs = [xml](Get-Content $packagesConfig)
        $pkgs.SelectNodes("//packages/package") | foreach {     
            $prj.IncrementTouchPoints()                   
            if ($thisPackages -contains $_.id) {
                $text = 'project file contains redundant package reference: ' + $_.id + ' ' + $_.version
                $prj.RecordError($text)
            } else {
                $thisPackages += $_.id
            }

            # check to see if the project references the same version
            $reference = $backpath + 'packages\' + $_.id + '.' + $_.version
            $nodes = $prj.Contents.SelectNodes("//p:HintPath[contains(text(),'$reference')]", $prj.Namespace)
            if ($nodes.count -eq 0) {
                $unused = $_.ParentNode.RemoveChild($_)
                $prj.MarkUpdated()
                $messages.Write(">> Removed unused package reference: " + $_.id + ".", 'Yellow')
            }
            $prj.IncrementTouchPoints()                   
        }
        $pkgs.Save($packagesConfig)
    }            
}

function SanityCheckProjectToSolution($messages, [string] $prjPath, $solution, [string] $slnPath) {
    $messages.Write(">> processing project $prjPath", 'White')
    $messages.Indent()

    # Calculate the relative path to the solution file...
    $prjFile = $prjPath | Split-Path -Leaf
    $prjDir = $prjPath | Split-Path 
    Set-Location $prjDir
    $slnFile = $slnPath | Split-Path -Leaf
    $slnBackpath = (Get-Item $slnPath | Resolve-Path -Relative).Replace($slnFile, '')

    if ((!(Test-Path $prjPath))) {
        $messages.Write("!! The file is missing: $prjFile", 'Red')
    } else {
        $prj = CreateProjectObj $solution $prjFile
        
        $isHomeSolution = (EnsureProjectHasHomeSolutionNode $messages $prj $slnFile)
        if ($isHomeSolution) {
            # Remove unused nodes in the project xml...
            $removeThese = @(
                "//p:None[contains(@Include,'Properties\AssemblyInfo.version.tt')]", # in some projects that originated as FlitBit code
                "//p:Content[contains(@Include,'Properties\Versioning.xml')]",       # in some projects that originated as FlitBit code
                "//p:Import[contains(@Project,'csproj-import-before\*')]",           # early revision of build tools
                "//p:Import[contains(@Project,'csproj-import-after\*')]",            # early revision of build tools
                "//p:Import[contains(@Project,'NetSteps.targets')]",                 # early revision of build tools (Montane's hack)
                "//p:Import[contains(@Project,'.nuget')]",                           # nuget package restore
                "//p:Import[contains(@Project,'Override.targets')]",                 # Overrides are included when necessary by csproj-import-after.targets                
                '//p:Import[contains(@Project,''$(MSBuildThisFileDirectory)\csproj-import-before.targets'')]', # previous build tools strategy
                '//p:Import[contains(@Project,''$(MSBuildThisFileDirectory)\csproj-import-after.targets'')]'   # previous build tools strategy
            )
            RemoveUnusedProjectNodes $messages $prj $removeThese 
            
            EnsureProjectUsesGenerateddVersionStamp $messages $prj $prjDir
            
            EnsureCodeContractsAreOnForAllBuildCondigurations $messages $prj
            
            $projectOverlayDir = Join-Path -Path $buildOverlayDir -ChildPath "csproj-placements"
            EnsureBuildOverlayFromSourceToTarget $messages $projectOverlayDir $prjDir

            EnsureProjectReferencesBuildOverlayTargets $messages $prj $prjDir $slnBackpath

            EnsureProjectHasCorrectSolutionDir $messages $prj $slnBackpath

            EnsureNuGetPackageReferencesAreCorrectlyRooted $messages $prj $slnBackpath

            SanityCheckNugetPackagesForProject $messages $prj $prjDir $slnBackpath
            
            $prj.Close($messages) # saves if necessary
        }
    }
    $messages.Outdent()      
}

# from https://gist.github.com/jstangroome/557222
$SolutionProjectPattern = @"
(?x)
^ Project \( " \{ FAE04EC0-301F-11D3-BF4B-00C04F79EFBC \} " \)
\s* = \s*
" (?<name> [^"]* ) " , \s+
" (?<path> [^"]* ) " , \s+
"@

function FixSolutionsInFolder([string] $where) {
    $messages = CreateMessagesObj
    if ($where -ne $buildOverlayDir) {
        $any_sln = [System.String]::Concat($up, "\*.sln")
        $messages.Write(">> looking for solutions in $where", 'White')
        $messages.Indent();
        Get-ChildItem $where -filter *.sln | foreach {
            $slnPath = $_.FullName
            $solution = CreateSolutionObj $slnPath
            $messages.Write(">> processing solution $slnPath", 'White')
            $messages.Indent()
            Get-Content -Path $_.FullName | foreach {
                if ($_ -match $SolutionProjectPattern) {
                    Set-Location $where
                    $prj = $Matches['path']
                    $prjPath = $where | Join-Path -ChildPath $prj
                    $prjPath = ($prjPath | Resolve-Path).ProviderPath
                    
                    SanityCheckProjectToSolution $messages $prjPath $solution $slnPath
                }
            }
            $solution.Close($messages)
            $messages.Outdent()
        }
        $messages.Outdent()
        Set-Location $where
        $slnOverlayDir = Join-Path -Path $buildOverlayDir -ChildPath "sln-placements"
        EnsureBuildOverlayFromSourceToTarget $messages $slnOverlayDir $where
                    
    } else { 
       'Cannot run without a target path'
    }
}

if (Test-Path $targetDir) { FixSolutionsInFolder $targetDir }
