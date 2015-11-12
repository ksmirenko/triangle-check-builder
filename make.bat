@echo off
if exist ".git" goto :repositoryExists
rem Git doesn't allow to clone into non-empty directoty
git.exe clone --progress -v "https://github.com/ksmirenko/triangle-check" %cd%\temp > log.txt 2>&1
xcopy /e /q /h /y %cd%\temp %cd%
goto :build

:repositoryExists
if not exist out goto :packagesExist
echo A current build exists.
set /p delBuild=Do you want to overwrite it [y/n]?: 
if %delBuild% == n goto :EOF
rmdir /s /q out
rm log.txt
goto :packagesExist

:packagesExist
if exist packages goto :update
echo Downloading necessary packages
getNuget.bat
goto :update

:update
echo Updating source code
git.exe pull --progress -v "https://github.com/ksmirenko/triangle-check" >> log.txt 2>&1
goto :build

:build
mkdir out
echo Building solution
msbuild "%cd%\TriangleCheck.sln" /p:Configuration=Release /p:OutDir=..\..\out /p:TargetFramework=v4.5.1 >> log.txt 2>&1

sendEmail.bat