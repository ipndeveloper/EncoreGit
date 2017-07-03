echo off
cls
echo Output Directory: %~dp0Deployments
@start %~dp0..\..\..\.build\NetSteps\Transmogrifier\SQLTransmogrifierFileGenerator.exe -td %~dp0Deployments -t NewScript -bc 1_9_0
