if "%main%"=="" goto :EOF
mkdir out
echo Building solution...
"%MSBUILD%" "%cd%\%SOLUTION_NAME%" /p:Configuration=Release /p:OutDir=%OUTDIR% /p:TargetFramework=v4.5.1 /p:ToolsDllPath=%TOOLS_DLL_PATH% >> log.txt 2>&1
if ERRORLEVEL 1 (
	set error=Build_failure
    echo ERROR: Build failed!
	sendEmail.bat
    goto :EOF
)
