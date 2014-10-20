@echo off

start "" "%appdata%\Vistrol Window Service\InstallUtil.exe" /u "%appdata%\Vistrol Window Service\WindowsService1.exe"
if ERRORLEVEL 1 goto error
exit
:error
echo There was a problem uninstall service

pause