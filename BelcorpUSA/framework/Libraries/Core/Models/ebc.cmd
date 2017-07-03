@echo OFF
::: -- Prepare the processor --
@SETLOCAL ENABLEEXTENSIONS 
@SETLOCAL ENABLEDELAYEDEXPANSION 

:: -- Version History --
::           Version       YYYYMMDD Author         Description
SET "version=0.0.1"      &:20120729 Phillip Clark  initial version 
SET "version=0.0.2"      &:20120804 Phillip Clark  modified for new build-layout... DevTools is now .build
SET "title=Execute DevTools (%~nx0) - %version%"

TITLE %title%
SET "DISPOSITION=DISPOSITION UNKNOWN"
SET "WHERE="
SET "UNIQUE=unknown"
CALL:make_timestamp UNIQUE

SET WRKFILE=%TMP%\%UNIQUE%.tmp
ECHO.Working file: %WRKFILE%

FOR /F "tokens=3" %%A IN ('REG QUERY HKLM\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell /v ExecutionPolicy') DO SET PS1_POLICY=%%A

ECHO.Powershell execution policy is %PS1_POLICY%
IF "%PS1_POLICY%" NEQ "Unrestricted" (
	GOTO :ChangePowershellExecutionPolicy
)

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

IF "%WHERE%" NEQ "" (
	CALL:NormalizePath "%WHERE%" WHERE
)
ECHO.WHERE = %WHERE%
ECHO.WHAT  = %WHAT%

IF NOT EXIST "%WHERE%" (
	CALL:PrintUsage
	GOTO :FAIL
)
	
CALL :ProbeForSiblingPath "%WHERE%" ".build" TOOLS
IF %ERRORLEVEL% NEQ 0 (
	ECHO.No .build found. %TOOLS%
	GOTO :FAIL
)
IF EXIST "%TOOLS%\trunk" (
	SET "TOOLS=%TOOLS%\trunk\"
) ELSE IF EXIST "%TOOLS%" (
	SET "TOOLS=%TOOLS%\"
)
SET "escaped=%TOOLS:\=\\%"
@"%TOOLS%3rdParty\svn.exe" info "%escaped%" > %WRKFILE%
for /F "tokens=1,2" %%a in (%WRKFILE%) do (
	IF "%%a" == "Revision:" (
		SET "TOOLSREV=%%b"
	)
)
ECHO.Tools revision: %TOOLSREV%

:: Execute the initial command...
SET "CL=CALL "%TOOLS%%WHAT%" "%WHERE%" %3 %4 %5 %6 %7 %8 %9"
%CL%
IF %ERRORLEVEL% NEQ 0 (
	ECHO.Error executing: %CL%
	GOTO:FAIL
)
SET "DISPOSITION=Success!"

GOTO:EXIT

:ProbeForSiblingPath -- Beginning in the target path, probes for a sibling path
::                   -- %~1: the target path
::                   -- %~2: the sibling path
::                   -- %~3: a variable where the result will be returned
SETLOCAL
SET "P=%~f1"
SET "WRK=%P:~0,3%"
SET "CANDIDATE=%P%\%~2"
SET "R="
IF "%P:~-3%" NEQ "%WRK%" (
	IF EXIST "%CANDIDATE%" (
		SET "R=%CANDIDATE%"
	) ELSE (
		IF EXIST "%~f1\.." (
			CALL :ProbeForSiblingPath "%~f1\.." %2 R
		)
	)
) ELSE (
	VERIFY OTHER 2> NUL
)
ENDLOCAL&SET "%~3=%R%"
GOTO :EOF

:NormalizePath -- Normalizes (expands) an input path to a full path name.
::             -- %~1: the input path
::             -- %~2: a variable where the result will be returned
SETLOCAL
SET P=%~f1
ENDLOCAL&SET "%~2=%P%"
GOTO :EOF

:ParseCommandLineArg -- Parses a command line argument and sets the corresponding variable
::                   -- %~1: the argument
SETLOCAL
SET C=%~1
SET A=%C:~0,3%
SET V=%C:~3%
SET "G="
IF "%A%" == "/p:" (
	SET "G=WHERE"
) ELSE IF "%A%"	== "/P:" (
	SET "G=WHERE"
) ELSE IF "%A%"	== "/c:" (
	SET "G=WHAT"
) ELSE IF "%A%"	== "/C:" (
	SET "G=WHAT"
) ELSE (
	VERIFY OTHER 2> NUL
)
ENDLOCAL&SET "%G%=%V%"
GOTO:EOF

:make_timestamp -- creates a timestamp and returns it's value in the variable given
::              -- %~1: reference to a variable to hold the timestamp
FOR /f "tokens=2-8 delims=/:. " %%A IN ("%date%:%time: =0%") DO SET "%~1=%%C%%A%%B_%%D%%E%%F%%G"
GOTO:EOF

:PrintUsage -- Prints usage text
ECHO.
ECHO.  %title%
ECHO.  Establishes a DevTools path and executes the tool/command specified.
ECHO.  
ECHO.  Arguments:
ECHO.             /p:{path} -- specifies a target path
ECHO.             /c:{cmd}  -- specifies which command to execute.
ECHO.             /a:{args} -- a quoted list of arguments given to the
ECHO.                          target command (as-is).
ECHO.
ECHO.  Examples:
ECHO.
ECHO.             ebc /p:. /c:fancy-fix
ECHO.
GOTO:EOF

:ChangePowershellExecutionPolicy -- Prints instructions for changing Powershell execution policy
ECHO.
ECHO.  __________________________________________________________________________
ECHO.  Your Powershell Execution policy is set to %PS1_POLICY%. In order to execute
ECHO.  build tools your policy must be changed to allow the execution of scripts 
ECHO.  from the command line.
ECHO.
ECHO.  Run Powershell as an administrator and execute the following
ECHO.  command:
ECHO.
ECHO.    Set-ExecutionPolicy Unrestricted
ECHO.
ECHO.
	
GOTO:FAIL

:FAIL
::    -- %1: the current log file
::    -- %2: a failure message
:: CALL:log %~1 "%~2"
SET "DISPOSITION=FAILED"
GOTO :EXIT

:EXIT -- Displays the disposition and exits.
ECHO.
SET /P WAIT_RESULT=Script complete: %DISPOSITION% (enter to continue)
GOTO :EOF
