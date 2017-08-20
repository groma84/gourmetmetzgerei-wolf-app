echo "Bootstrapping paket..."
.paket\paket.bootstrapper.exe

echo "Install global client stuff with yarn..."
call yarn global add elm create-elm-app elm-format

echo "Installing Elm packages..."
cd client
call elm-package install --yes
echo "Building Elm app once to get all files in place..."
call elm-app build
cd ..

echo "Building the server once to get all libraries..."
call build.cmd

echo "Copying elm-format runner to local npm directory to make it globally available for VS Code..."
call copy elm-format.cmd "%APPDATA%\..\Local\npm\" /Y
