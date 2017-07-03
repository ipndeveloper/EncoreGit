@echo off

rem This is the line you should enter in the pre-build field (adjust the number of ..\ as needed):
rem "$(SolutionDir)..\..\..\Framework\Trunk\Framework.ClientSettingsPreBuild\Scripts\ApplyClientSettingsPreBuild.cmd" "$(ProjectDir)" "$(SolutionDir)..\..\..\Framework\Trunk\" "$(IsAutoBuild)"

rem This must be called before using DeQuote
SetLocal EnableDelayedExpansion

set startDir=%CD%
@echo. Current dir: %currentDir%
set preBuildDir=%~f1
rem call :DeQuote preBuildDir

set frameworkSolutionDir=%~f2

rem call :DeQuote frameworkSolutionDir
set scriptsDir=%frameworkSolutionDir%Framework.ClientSettingsPreBuild\Scripts\

set isAutoBuild=%~3
rem call :DeQuote isAutoBuild

echo preBuildDir: %preBuildDir%
echo frameworkSolutionDir: %frameworkSolutionDir%
echo scriptsDir: %scriptsDir%
echo isAutoBuild: %isAutoBuild%

:Change directories before reading file (fix for paths with spaces)
cd "%scriptsDir%"

for /f "delims=" %%a in (ApplyClientSettingsPreBuild.Applications.txt) do (
    rem Copy client configs
  :Return to start directory before continuing
  cd %startDir%
	echo CopyFrom: %%a
  echo preBuildDir: "%preBuildDir%%%a\*"
  echo frameworkSolutionDir: "%frameworkSolutionDir%%%a\"
	if "%isAutoBuild%" == "True" (
		echo CopyFileWithAutoBuild: %isAutoBuild%
		call :CopyFiles "%preBuildDir%%%a\*" "%frameworkSolutionDir%%%a\"
	) else (
		call :CopyFilesIfDifferent "%preBuildDir%%%a\*" "%frameworkSolutionDir%%%a\"
	)
  cd %scriptsDir%
)

cd %startDir%

rem Clean up and exit
set preBuildDir=
set frameworkSolutionDir=
set scriptsDir=
set isAutoBuild=
goto :EOF

rem :DeQuote
rem set _DeQuoteVar=%1
rem call set _DeQuoteString=%%!_DeQuoteVar!%%
rem if [!_DeQuoteString:~0^,1!]==[^"] (
rem 	if [!_DeQuoteString:~-1!]==[^"] (
rem 		set _DeQuoteString=!_DeQuoteString:~1,-1!
rem 	) else (goto :EOF)
rem ) else (goto :EOF)
rem set !_DeQuoteVar!=!_DeQuoteString!
rem set _DeQuoteVar=
rem set _DeQuoteString=
rem goto :EOF

:CopyFiles
echo Calling: CopyFiles
set _CopyFilesTargetDir=%~2
rem call :DeQuote _CopyFilesTargetDir
for %%f in (%1) do (
	echo WillCopy: %%f 
	call :CopyFile "%%f" "%_CopyFilesTargetDir%%%~nxf"
)
set _CopyFilesTargetDir=
goto :EOF

:CopyFilesIfDifferent
echo Calling: CopyFilesIfDifferent
set _CopyFilesIfDifferentTargetDir=%~2
rem call :DeQuote _CopyFilesIfDifferentTargetDir
for %%f in (%1) do fc /b "%%f" "%_CopyFilesIfDifferentTargetDir%%%~nxf" >nul 2>&1 || (
	rem This executes when fc throws an error (meaning the two files are different)
	call :CopyFile "%%f" "%_CopyFilesIfDifferentTargetDir%%%~nxf"
)
set _CopyFilesIfDifferentTargetDir=
goto :EOF

:CopyFile
echo Updating %2
if exist %2 attrib -r %2
copy /y %1 %2
goto :EOF
