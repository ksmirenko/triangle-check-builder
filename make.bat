@echo off

if exist ".git" goto :repositoryExists
rem Git doesn't allow to clone into non-empty directoty
git.exe clone --progress -v "https://github.com/ksmirenko/triangle-check" %cd%\temp > log.txt 2>&1
xcopy /e /q /h /y %cd%\temp %cd%
goto :build

:repositoryExists
if not exist out goto :checkPackages
echo A current build exists.
set /p delBuild=Do you want to overwrite it [n if not, anything otherwise]?: 
if %delBuild% == n goto :EOF
rmdir /s /q out
rm log.txt

:checkPackages
if exist packages goto :update
echo Downloading necessary packages...
call getNuget.bat

:update
echo Updating source code...
git.exe pull --progress -v "https://github.com/ksmirenko/triangle-check" >> log.txt 2>&1

:build
mkdir out
echo Building solution...
msbuild "%cd%\TriangleCheck.sln" /p:Configuration=Release /p:OutDir=..\..\out /p:TargetFramework=v4.5.1 >> log.txt 2>&1
echo Checking build...
for /f %%i in (%cd%\expectedFiles.txt) do (
    if not exist "%cd%\out\%%i" echo %%i haven't been created! >> log.txt 2>&1
    )

:test
echo Testing...
"%cd%\packages\NUnit.Runners.2.6.4\tools\nunit-console" "%cd%\out\Test.dll" >> log.txt 2>&1

:sendEmail
call sendEmail.bat