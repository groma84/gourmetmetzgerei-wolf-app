// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"
#r "./packages/WinSCP/lib/WinSCPnet.dll"

open System
open System.IO
open Fake
open WinSCP

let run' timeout cmd args dir =
    if execProcess (fun info ->
        info.FileName <- cmd
        if not (String.IsNullOrWhiteSpace dir) then
            info.WorkingDirectory <- dir
        info.Arguments <- args
    ) timeout |> not then
        failwithf "Error while running '%s' with args: %s" cmd args

let run = run' System.TimeSpan.MaxValue

let exePath = "./packages/WinSCP/content/WinSCP.exe"

type AzureCreds = {
    url : string
    username : string
    password : string
}

let credsFile = "azure_deployment_credentials.txt"

let creds =
    if System.IO.File.Exists credsFile then
        let lines = System.IO.File.ReadAllLines credsFile
        {
            url = lines.[0]
            username = lines.[1]
            password = lines.[2]
        }
    else
        failwith <| (sprintf "Credentials file %s does not exist. Three lines: Url, Username, Password" credsFile)

let connectionString = 
    sprintf "ftps://%s:%s@%s" creds.username creds.password creds.url

let ftpCommand =
    sprintf """ /command "open %s" "synchronize remote -delete -criteria=size "".\deploy"" ""/site/wwwroot"" " "exit" """ connectionString


let logfile = "azureuploadlog.txt"
let logCommand = sprintf "/log=%s" logfile

let commands = [
    "/console"
    "/nointeractiveinput"
    logCommand
    ftpCommand
]

let finalCommand =
    List.fold (fun acc s -> acc + " " + s) "" commands


System.IO.File.Delete logfile
run exePath finalCommand "."
