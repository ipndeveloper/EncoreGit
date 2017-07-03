@ECHO OFF 
SET BUILDTOOLS=".\.build"
CALL:locateBuildTools
ECHO.Using build tools at %BUILDTOOLS%
CALL:make_timestamp UNIQUE

%windir%\Microsoft.NET\framework64\v4.0.30319\msbuild.exe Packaging.targets /target:Production /property:BuildTools=%BUILDTOOLS% /flp1:v=detailed;logfile=package_%UNIQUE%.log /clp:v=minimal
GOTO:EXIT

:locateBuildTools
::
IF NOT EXIST %BUILDTOOLS% (
	SET BUILDTOOLS=".\.%BUILDTOOLS%"
	SET /A BUILDTOOLSSEARCH += 1
	IF "%BUILDTOOLSSEARCH%" == "10" (
		ECHO "Unable to locate .build tools directory, last looked for %BUILDTOOLS%"
		GOTO:FAIL
	)
	GOTO:locateBuildTools
)
pushd %BUILDTOOLS%
SET BUILDTOOLS=%CD%
popd
GOTO:EOF

:make_timestamp -- creates a timestamp and returns it's value in the variable given
::              -- %~1: reference to a variable to hold the timestamp
FOR /f "tokens=2-8 delims=/:. " %%A IN ("%date%:%time: =0%") DO SET "%~1=%%C%%A%%B_%%D%%E%%F%%G"
GOTO:EOF

:EXIT -- Displays the disposition and exits.
ECHO.
SET /P WAIT_RESULT=Script complete! (enter to continue)
GOTO:EOF