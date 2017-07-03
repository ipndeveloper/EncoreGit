@echo off

rem This is the line you should enter in the post-build field:
rem "C:\Development\Finance\Trunk\NetSteps.ClientSettingsPreBuild\Scripts\ApplyClientSettingsPreBuild.cmd" "C:\Development\Finance\Trunk\" "$(IsAutoBuild)" "$(OutDir)" "$(TargetDir)"

rem This must be called before using DeQuote
SetLocal EnableDelayedExpansion

set frameworkSolutionDir=%1
call :DeQuote frameworkSolutionDir

set scriptsDir=%frameworkSolutionDir%NetSteps.ClientSettingsPreBuild\Scripts\

if %2 == "True" (
    set frameworkSolutionDir="C:\Builds\2\ItWorks\ItWorks\Sources\Framework\Trunk\"
    call :DeQuote frameworkSolutionDir
    set scriptsDir="C:\Development\Framework\Trunk\Framework.ClientSettingsPreBuild\Scripts\"
    call :DeQuote scriptsDir
	set clientSettingsDir=%3
	call :DeQuote clientSettingsDir
	set websitesDir=C:\Builds\2\ItWorks\ItWorks\Sources\Framework\Trunk\Websites\
) else (
	set clientSettingsDir=%4..\..\
	call :DeQuote clientSettingsDir
	set websitesDir=%frameworkSolutionDir%Services\
)

set programFilesDir=%ProgramFiles(x86)%\
if "%programFilesDir%" == "\" set programFilesDir=%ProgramFiles%\
set tf="%programFilesDir%Microsoft Visual Studio 10.0\Common7\IDE\tf.exe"
set tfpt="%programFilesDir%Microsoft Team Foundation Server 2010 Power Tools\TFPT.EXE"

echo frameworkSolutionDir: %frameworkSolutionDir%
echo clientSettingsDir: %clientSettingsDir%
echo websitesDir: %websitesDir%
echo scriptsDir: %scriptsDir%

for /f %%w in (%scriptsDir%ApplyClientSettingsPreBuild.Websites.txt) do (

	echo spencer: "%clientSettingsDir%%%w\*" "%websitesDir%%%w\"    
    rem Copy client configs
    call :CopyIfDifferent "%clientSettingsDir%%%w\*" "%websitesDir%%%w\"    
    
)

rem Clean up and exit
set clientSettingsDir=
set websitesDir=
set scriptsDir=
set tf=
set tfpt=
goto :EOF

:DeQuote
set _DeQuoteVar=%1
call set _DeQuoteString=%%!_DeQuoteVar!%%
if [!_DeQuoteString:~0^,1!]==[^"] (
	if [!_DeQuoteString:~-1!]==[^"] (
		set _DeQuoteString=!_DeQuoteString:~1,-1!
	) else (goto :EOF)
) else (goto :EOF)
set !_DeQuoteVar!=!_DeQuoteString!
set _DeQuoteVar=
set _DeQuoteString=
goto :EOF

:CopyIfDifferent
set _CopyIfDifferentTargetDir=%2
call :DeQuote _CopyIfDifferentTargetDir
for %%f in (%1) do fc /b "%%f" "%_CopyIfDifferentTargetDir%%%~nxf" >nul 2>&1 || (
	rem This executes when fc throws an error (meaning the two files are different)
	%tf% checkout "%_CopyIfDifferentTargetDir%%%~nxf"
	copy "%%f" "%_CopyIfDifferentTargetDir%%%~nxf"
	if exist %tfpt%	%tfpt% uu /noget "%_CopyIfDifferentTargetDir%%%~nxf"
)
set _CopyIfDifferentTargetDir=
goto :EOF