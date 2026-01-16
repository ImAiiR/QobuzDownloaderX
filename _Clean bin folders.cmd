@ECHO OFF

:: Debug folder
ECHO:[.] Cleaning Debug folder...
(
	RD  /Q /S ".\QobuzDownloaderX\bin\Debug\logs"
	RD  /Q /S ".\QobuzDownloaderX\bin\Debug\qbdlx-temp"
	DEL /F /Q ".\QobuzDownloaderX\bin\Debug\Latest_Error.log"
	DEL /F /Q ".\QobuzDownloaderX\bin\Debug\post_template.txt"
)>NUL 2>&1
CALL :ResetUserConfig ".\QobuzDownloaderX\bin\Debug\user.config"
ECHO:[+] Debug folder has been cleaned.

:: Release folder
ECHO:[.] Cleaning Release folder...
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
PAUSE
EXIT

:ResetUserConfig
DEL /Q "%~1"
IF EXIST "%~1" (
	COLOR 0C
    ECHO: Error deleting "%~1"
    PAUSE
    EXIT /B
)

(
	echo ^<?xml version="1.0" encoding="utf-8"?^>
	echo ^<settings^>
	echo   ^<currentLanguage^>EN^</currentLanguage^>
	echo   ^<currentTheme^>Titanium^</currentTheme^>
	echo ^</settings^>
) > "%~1"
EXIT /B