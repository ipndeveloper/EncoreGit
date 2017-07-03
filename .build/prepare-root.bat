@ECHO OFF
@powershell -file "%~dp0fix-solution.ps1" "%~f1"
@powershell -file "%~dp0FixSolutionFile.ps1" "%~f1"
:: repeat the fancy fix, it takes multiple passes to get it right...
@powershell -file "%~dp0fix-solution.ps1" "%~f1"

