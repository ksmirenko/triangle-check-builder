if "%main%"=="" goto :EOF
@echo off

if not exist "NuGet.exe"
    powershell -Command "(New-Object Net.WebClient).DownloadFile('http://nuget.org/nuget.exe', 'NuGet.exe')"

echo Downloading necessary packages...
"NuGet.exe" "restore" "TriangleCheck.sln" "-OutputDirectory" "packages"