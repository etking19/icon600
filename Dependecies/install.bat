@echo off

start "" "%appdata%\Vistrol Window Service\SetACL.exe" -on "%appdata%\Vistrol Window Service" -ot file -actn ace -ace "n:everyone;p:full"

start "" "%appdata%\Vistrol Window Service\InstallUtil.exe" "%appdata%\Vistrol Window Service\WindowsService1.exe"

TIMEOUT /t 10

net start "Vistrol Window Service"

pause