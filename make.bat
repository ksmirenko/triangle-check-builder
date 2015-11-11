@echo off
if exist ".git" goto :continue
git.exe clone --progress -v "https://github.com/ksmirenko/triangle-check" %cd%\temp > log.txt 2>&1
xcopy /e /q /h /y %cd%\temp %cd%
goto :build
:continue
if not exist out goto :update
ECHO A current build exists.
set /p delBuild=Do you want to overwrite it [y/n]?: 
if %delBuild% == n goto :EOF
rmdir /s /q out
goto :update

:update
echo Git pulling in progress.
git.exe pull --progress -v "https://github.com/ksmirenko/triangle-check" >> log.txt 2>&1
goto :build

:build
mkdir out
echo Solution building in progress.
msbuild "%cd%\TriangleCheck.sln" /p:Configuration=Release /p:OutDir=..\..\out /p:TargetFramework=v4.0 >> log.txt 2>&1

sendEmail.bat