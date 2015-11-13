if "%main%"=="" goto :EOF
echo Sending email...
@echo off

powershell -Command "(New-Object Net.WebClient).DownloadFile('https://mailsend.googlecode.com/files/mailsend1.17b14.exe', 'mailsend.exe')"
for /f "delims=" %%A in (%cd%\emailProperties.txt) do set %%A

if not exist missingFiles.txt (
	call %cd%\mailsend.exe -to %to% -from %from% -ssl -port 465 -auth -smtp %smtp% -sub %sub% +cc +bc -v -user %user% -pass %pass% -M "Build_successful" >nul
	goto EOF
)

SetLocal EnableDelayedExpansion
set content=
for /F "delims=" %%i in (%cd%\missingFiles.txt) do set content=!content! %%i

call %cd%\mailsend.exe -to %to% -from %from% -ssl -port 465 -auth -smtp %smtp% -sub %sub% +cc +bc -v -user %user% -pass %pass% -attach %attach% -M "%content%" >nul
EndLocal