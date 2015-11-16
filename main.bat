@echo off

set main=true
if exist missingFiles.txt del missingFiles.txt

call settings.bat
call cloneSources.bat

if exist out (
    rmdir /s /q out
    del log.txt
)

echo Downloading necessary packages...
"%NUGET%" "restore" "%SOLUTION_NAME%" "-OutputDirectory" "packages"

echo Updating source code...
git.exe pull --progress -v "%GIT_REPO%" >> log.txt 2>&1
if ERRORLEVEL 1 (
   set error=Cannot_clone_git
   echo ERROR: Could not clone git repo!
   sendEmail.bat
)

call build.bat
call test.bat
call sendEmail.bat
