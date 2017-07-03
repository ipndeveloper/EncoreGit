@ECHO OFF
::: -- Prepare the processor --
@SETLOCAL ENABLEEXTENSIONS 
@SETLOCAL ENABLEDELAYEDEXPANSION 

:: -- Version History --
::           Version       YYYYMMDD Author         Description
SET "version=0.0.1"      &:20120804 Phillip Clark  initial version 
SET "title=%~nx0 - %version%"

ECHO.%title%

IF "%~1"=="" (
	ECHO.No path given.
	EXIT /B 1
) ELSE (
	SET "WHERE=%~f1"
)

:: delegate to powershell...
powershell -file "%~dpn0.ps1" "%WHERE%" %2 %3 %4
