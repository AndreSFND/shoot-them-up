echo OFF
cls
SET PATH=%PATH%;c:\windows\Microsoft.NET\Framework\v3.5

csc /o /out:"Shoot them up.exe" /target:exe /win32icon:res\icon.ico files\*.cs /reference:lib\sfmlnet-graphics-2.dll,lib\sfmlnet-system-2.dll,lib\sfmlnet-window-2.dll
pause


REM /target:winexe	Sem Console
REM /target:exe		Com Console