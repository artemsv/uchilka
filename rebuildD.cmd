@echo off

SET VSVER=14.0

CD /D "%~dp0"

call msBuildCore_VS2017.cmd uchilka.sln Rebuild Debug 4.5

IF "%1"=="nopause" GOTO :End
pause

:End