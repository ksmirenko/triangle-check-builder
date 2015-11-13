if "%main%"=="" goto :EOF
mkdir out
echo Building solution...
msbuild "%cd%\TriangleCheck.sln" /p:Configuration=Release /p:OutDir=%OUTDIR% /p:TargetFramework=v4.5.1 /p:ToolsDllPath=%cd%\out\CoreLib.dll >> log.txt 2>&1
if ERRORLEVEL 1 (
	set error=Build_failure
	goto :sendEmail
)