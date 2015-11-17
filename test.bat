if "%main%"=="" goto :EOF
echo Testing...
"%NUNIT_CONSOLE%" "%TEST_LIB%" >> log.txt 2>&1
if ERRORLEVEL 1 (
	set error=Test_failure
    echo ERROR: Testing failed!
	sendEmail.bat
)
