@ECHO OFF
COLOR 07

:: Debug folder
ECHO:[·] Cleaning Debug folder...
(
	RD  /Q /S ".\QobuzDownloaderX\bin\Debug\logs"
	RD  /Q /S ".\QobuzDownloaderX\bin\Debug\qbdlx-temp"
	DEL /F /Q ".\QobuzDownloaderX\bin\Debug\Latest_Error.log"
	DEL /F /Q ".\QobuzDownloaderX\bin\Debug\post_template.txt"
)>NUL 2>&1
CALL :ResetUserConfig ".\QobuzDownloaderX\bin\Debug\user.config"
ECHO:[+] Debug folder has been cleaned.
ECHO+

:: Release folder
ECHO:[·] Cleaning Release folder...
(
	RD  /Q /S ".\QobuzDownloaderX\bin\Release\logs"
	RD  /Q /S ".\QobuzDownloaderX\bin\Release\qbdlx-temp"
	DEL /F /Q ".\QobuzDownloaderX\bin\Release\Latest_Error.log"
	DEL /F /Q ".\QobuzDownloaderX\bin\Release\post_template.txt"
	DEL /F /Q ".\QobuzDownloaderX\bin\Release\Newtonsoft.Json.xml"
	DEL /F /Q ".\QobuzDownloaderX\bin\Release\*.pdb"
)>NUL 2>&1
CALL :ResetUserConfig ".\QobuzDownloaderX\bin\Release\user.config"
ECHO:[+] Release folder has been cleaned.
ECHO+

COLOR 0A
PAUSE
EXIT

:ResetUserConfig
DEL /Q "%~f1" 2>NUL
IF EXIST "%~f1" (
	COLOR 0C
    ECHO:Error deleting file: "%~f1"
    PAUSE
    EXIT 1
)

IF EXIST "%~dp1" (
	powershell -noprofile -command ^
	    "$xml = '<?xml version=\"1.0\" encoding=\"utf-8\"?>' + \"`r`n\" + " ^
	    "'<settings>' + \"`r`n\" + " ^
	    "'  <currentLanguage>EN</currentLanguage>' + \"`r`n\" + " ^
	    "'  <currentTheme>Titanium</currentTheme>' + \"`r`n\" + " ^
	    "'</settings>'; " ^
	    "Set-Content -LiteralPath '%~1' -Value $xml -Encoding UTF8"
)
EXIT /B