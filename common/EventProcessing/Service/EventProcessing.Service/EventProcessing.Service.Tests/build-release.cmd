@ECHO OFF
::: -- Prepare the processor --
@SETLOCAL ENABLEEXTENSIONS 
@SETLOCAL ENABLEDELAYEDEXPANSION 

:: -- Version History --
::           Version       YYYYMMDD Author         Description
SET "version=0.0.1"      &:20120729 Phillip Clark  initial version 
SET "version=0.0.2"      &:20131127 Phillip Clark  updated to use specified VS environ, or fallback to the latest version installed
SET "title=Build (%~nx0) - %version%"
TITLE %title%

SET "DISPOSITION=DISPOSITION UNKNOWN"
SET "UNIQUE=unknown"
CALL:make_timestamp UNIQUE

IF "%~1" NEQ "" (
	CALL :ParseCommandLineArg "%~1"	
)
IF %ERRORLEVEL% NEQ 0 (
	ECHO.Invalid argument: %~1
	CALL:PrintUsage
	GOTO :FAIL
)
IF "%~2" NEQ "" (
	CALL :ParseCommandLineArg "%~2"	
)
IF %ERRORLEVEL% NEQ 0 (
	ECHO.Invalid argument: %~2
	CALL:PrintUsage
	GOTO :FAIL
)
IF "%~3" NEQ "" (
	CALL :ParseCommandLineArg "%~3"	
)
IF %ERRORLEVEL% NEQ 0 (
	ECHO.Invalid argument: %~3
	CALL:PrintUsage
	GOTO :FAIL
)
IF "%~4" NEQ "" (
	CALL :ParseCommandLineArg "%~4"	
)
IF %ERRORLEVEL% NEQ 0 (
	ECHO.Invalid argument: %~4
	CALL:PrintUsage
	GOTO :FAIL
)

IF "%CFG%" == "" (
	SET "CFG=Release"
)
IF "%PLT%" == "" (
	SET PLT="AnyCPU"
)
IF "%VRB%" == "" (
	SET VRB="minimal"
)
IF "%FIL%" == "" (
	SET FIL="detailed"
)
IF "%VSE%" == "" (
	SET VSE="10.0"
)
ECHO.%VSE%
IF "%VSE%" == "11.0" (
	IF EXIST "C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat" (
		GOTO:load_11_vars
	)
)
GOTO:load_10_vars
	
:load_11_vars
CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat"
GOTO:go_build

:load_10_vars
CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat"

:go_build
FOR %%I IN (*.csproj) DO CALL :build_csproj "%%~nxI"	
SET "DISPOSITION=Success!"
GOTO:EXIT

:build_csproj
SET F="%~1"
ECHO.%~p1
ECHO. %~1
SET "CL=msbuild %F% /m /p:Configuration=%CFG%;Platform=%PLT% /t:Clean;Build /clp:v=%VRB% /flp1:v=%FIL%;logfile=build_%UNIQUE%.log"
%CL%
IF %ERRORLEVEL% NEQ 0 (
	ECHO.Build failed...
	GOTO:FAIL
)

GOTO:EOF

:ParseCommandLineArg -- Parses a command line argument and sets the corresponding variable
::                   -- %~1: the argument
SETLOCAL
SET C=%~1
SET A=%C:~0,3%
SET V=%C:~3%
SET "G="
IF "%A%" == "/p:" (
	SET "G=PLT"
) ELSE IF "%A%"	== "/P:" (
	SET "G=PLT"
) ELSE IF "%A%"	== "/c:" (
	SET "G=CFG"
) ELSE IF "%A%"	== "/C:" (
	SET "G=CFG"
) ELSE IF "%A%"	== "/v:" (
	SET "G=VRB"
) ELSE IF "%A%"	== "/V:" (
	SET "G=VRB"
) ELSE IF "%A%"	== "/e:" (
	SET "G=VSE"
) ELSE IF "%A%"	== "/E:" (
	SET "G=VSE"
) ELSE IF "%A%"	== "/f:" (
	SET "G=FIL"
) ELSE IF "%A%"	== "/F:" (
	SET "G=FIL"
) ELSE (
	VERIFY OTHER 2> NUL
)
ENDLOCAL&SET "%G%=%V%"
GOTO:EOF

:make_timestamp -- creates a timestamp and returns it's value in the variable given
::              -- %~1: reference to a variable to hold the timestamp
FOR /f "tokens=2-8 delims=/:. " %%A IN ("%date%:%time: =0%") DO SET "%~1=%%C%%A%%B_%%D%%E%%F%%G"
GOTO:EOF


:FAIL
::    -- %1: the current log file
::    -- %2: a failure message
:: CALL:log %~1 "%~2"
SET "DISPOSITION=FAILED"
COLOR 47
GOTO :EXIT

:EXIT -- Displays the disposition and exits.
ECHO.
SET /P WAIT_RESULT=Script complete: %DISPOSITION% (enter to continue)
GOTO:EOF
