@echo off

set main=true
if exist missingFiles.txt del missingFiles.txt

call settings.bat
call cloneSources.bat

if exist out (
    echo A current build exists.
    set /p delBuild=Do you want to overwrite it [n if not, anything otherwise]?: 
    if "%delBuild%" == "n" goto :EOF
    rmdir /s /q out
    del log.txt
)

if not exist packages call getNuget.bat

echo Updating source code...
git.exe pull --progress -v "https://github.com/ksmirenko/triangle-check" >> log.txt 2>&1

call build.bat
call test.bat
call sendEmail.bat