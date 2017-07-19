if "%1"=="" goto :BADEND
if "%2"=="" goto :BADEND
if "%3"=="" goto :BADEND
if "%4"=="" goto :BADEND

@echo off
if "%PROCESSOR_ARCHITECTURE%"=="AMD64" goto 64BIT
if "%PROCESSOR_ARCHITEW6432%"=="AMD64" goto 64BIT
set PFPATH=%ProgramFiles%
goto ENDCOPY
:64BIT
set PFPATH=%ProgramFiles(x86)%
:ENDCOPY

call "%PFPATH%\Microsoft Visual Studio %VSVER%\VC\vcvarsall.bat" x86

echo ---------------- QUIET MODE ---------------------
echo --- ONLY ERRORS ARE DISPLAYED ------
echo.
echo. 

"%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe" %1 /t:%2 /p:Configuration="%3" /p:TargetFrameworkVersion=v%4 /v:q /clp:ErrorsOnly /m:8

goto END


:BADEND

echo wrong parameters

:END
