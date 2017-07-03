@ECHO OFF
pushd .\Cache-Counters
SET CC=%CD%
popd
@powershell -file .\Cache-Counters\Configure-CacheCounters_low.ps1 %CC%