set NUNIT_CONSOLE=%cd%\packages\NUnit.Runners.2.6.4\tools\nunit-console
set NUGET=%cd%\build-tools\NuGet.exe
set MSBUILD=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe
set MAILSEND=%cd%\build-tools\mailsend.exe

set TEMP_DIR=%cd%\temp
set GIT_REPO=https://github.com/ksmirenko/triangle-check
set SOLUTION_NAME=TriangleCheck.sln
set TOOLS_DLL_PATH=%cd%\out\CoreLib.dll
set OUTDIR=%~dp0\out
set TEST_LIB=%cd%\out\Test.dll
set EMAIL_PROPS=%cd%\build-tools\emailProperties.txt
set EXPECTED_FILES=%cd%\build-tools\expectedFiles.txt
set MISSING_FILES=%cd%\missingFiles.txt