@echo off
setlocal

REM === Configuration ===
set "pathFolder=C:\Dataloader\BQP_PO\PO"
set "logs=C:\Dataloader\BQP_PO\Logs"
set "archived=C:\Dataloader\BQP_PO\Archived"
set "error=C:\Dataloader\BQP_PO\Error"  REM Not used in logic, but defined for parity

REM === Check if source folder exists ===
if not exist "%pathFolder%" (
    echo No se encontró la carpeta PO.
    goto end
)

REM === Ensure archived folder exists ===
if not exist "%archived%" (
    mkdir "%archived%"
)

REM === Process each file in the PO folder ===
for %%F in ("%pathFolder%\*") do (
    echo Procesando archivo: %%~nxF

    REM Run FADataLoader
    FADataLoader.exe -n "1" -i "%%F" -l "%logs%" -ds "http://localhost:9000" -a "10.10.141.80:1999" -u "dataloader" -p "Dataloader123!"

    REM Generate unique filename if exists in Archived
    set "dest=%%~nxF"
    if exist "%archived%\%%~nxF" (
        set "datetime=%date:~10,4%%date:~4,2%%date:~7,2%%time:~0,2%%time:~3,2%%time:~6,2%"
        set "dest=%%~nF_%datetime%%%~xF"
    )

    call :moveFile "%%F" "%archived%\%dest%"
)

echo Proceso completado correctamente.
goto end

:moveFile
move /Y %1 %2 >nul
exit /b

:end
pause

