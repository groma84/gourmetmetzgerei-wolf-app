# Notwendige Programme
- Node.js (z.B. via Nodist) <https://github.com/marcelklehr/nodist/releases>
- yarn <https://yarnpkg.com/en/docs/install>
- Visual Studio Code <https://code.visualstudio.com/Download>
- Visual Studio Code Extensions "elm" und "Ionide-fsharp"

# "Solution" einrichten
setup.bat ausführen
build.cmd ausführen

# Daten-Speicherort konfigurieren
Wird in config.yaml hinterlegt.

# E-Mail-Ordner konfigureren
Wird in config.yaml als PickupDirectory eingestellt - leider muss man hier einen absoluten Pfad angeben, weil 
die .NET Smtp-Implementierung das so will.
