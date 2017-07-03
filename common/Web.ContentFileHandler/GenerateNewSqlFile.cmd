for %%a in ("%cd%") do set lastFolder=%%~nxa

.build\NetSteps\SqlFileGenerator\SQLTransmogrifierFileGenerator.exe -td %cd%\sql\deployments -bc %lastFolder% -t NewScript