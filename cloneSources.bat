if "%main%"=="" goto :EOF
if not exist ".git" (
    rem Git doesn't allow to clone into non-empty directoty
    git.exe clone --progress -v "%GIT_REPO%" %TEMP_DIR% > log.txt 2>&1
    if ERRORLEVEL 1 (
        set error=Cannot_clone_git
        sendEmail.bat
    )
    xcopy /e /q /h /y %TEMP_DIR% %cd%
)
