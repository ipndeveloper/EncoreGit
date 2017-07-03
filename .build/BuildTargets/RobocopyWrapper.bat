@echo off
robocopy %1 %2 /E /NP /NJS /XX
set/A errlev="%ERRORLEVEL% & 24"
exit/B %errlev%