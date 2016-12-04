::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Restore packages
:: ----------------

IF NOT EXIST ".paket\paket.bootstrapper.exe" (
  echo Downloading paket.bootstrapper.exe
  mkdir .paket
  curl https://github.com/fsprojects/Paket/releases/download/1.2.0/paket.bootstrapper.exe -L --insecure -o .paket\paket.bootstrapper.exe
)

.paket\paket.bootstrapper.exe
IF !ERRORLEVEL! NEQ 0 goto error

.paket\paket.exe restore
IF !ERRORLEVEL! NEQ 0 goto error

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Run  FAKE build script
:: ----------------------

packages\FAKE\tools\FAKE.exe deploy.fsx
IF !ERRORLEVEL! NEQ 0 goto error