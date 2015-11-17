if "%main%"=="" goto :EOF
echo Sending email...
@echo off

for /f "delims=" %%A in (%EMAIL_PROPS%) do set %%A

if not "%error%"=="" (
	call %MAILSEND% -to %to% -from %from% -ssl -port 465 -auth -smtp %smtp% -sub %sub% +cc +bc -v -user %user% -pass %pass% -attach %attach% -M %error% >nul
	goto :finish
)

if not exist %MISSING_FILES% (
	call %MAILSEND% -to %to% -from %from% -ssl -port 465 -auth -smtp %smtp% -sub %sub% +cc +bc -v -user %user% -pass %pass% -M "Build_successful" >nul
	goto :finish
)

SetLocal EnableDelayedExpansion
set content=
for /F "delims=" %%i in (%MISSING_FILES%) do set content=!content! %%i
call %MAILSEND% -to %to% -from %from% -ssl -port 465 -auth -smtp %smtp% -sub %sub% +cc +bc -v -user %user% -pass %pass% -attach %attach% -M "%content%" >nul
EndLocal

:finish
echo Email sent successfully.
