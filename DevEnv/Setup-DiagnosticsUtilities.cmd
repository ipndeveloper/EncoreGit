@ECHO OFF
pushd .\Diagnostics-Utilities
SET DU=%CD%
popd
@powershell -file .\Diagnostics-Utilities\Configure-DiagnosticsUtilities_low.ps1 %DU%