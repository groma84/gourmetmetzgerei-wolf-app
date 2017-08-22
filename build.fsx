// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open System
open System.IO
open Fake
open Fake.Testing.Expecto

// Directories
let buildDir  = "./build/"
let clientDir = "./client/"
let deployDir = "./deploy/"
let testOutputDir = "./tests/"


type AzureCreds = {
    url : string
    username : string
    password : string
}


// Helper
let run' timeout cmd args dir =
    if execProcess (fun info ->
        info.FileName <- cmd
        if not (String.IsNullOrWhiteSpace dir) then
            info.WorkingDirectory <- dir
        info.Arguments <- args
    ) timeout |> not then
        failwithf "Error while running '%s' with args: %s" cmd args

let run = run' System.TimeSpan.MaxValue

let platformTool tool winTool =
    let tool = if isUnix then tool else winTool
    tool
    |> ProcessHelper.tryFindFileOnPath
    |> function Some t -> t | _ -> failwithf "%s not found" tool

let yarnTool = platformTool "yarn" "yarn.cmd"
let elmAppTool = platformTool "elm-app" "elm-app.cmd"
let winScpExePath = "./packages/WinSCP/content/WinSCP.exe"

let all _ = true

// Filesets
let appReferences  =
    !! "/**/*.csproj"
    ++ "/**/*.fsproj"

let testAssemblies = 
    "tests/*.Test.exe"

// version info
let version = "0.1"  // or retrieve from CI server

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; testOutputDir; deployDir;] 
)

Target "BuildLocal" (fun _ ->
    let target = "Build"
    let parameters = [
            ("Configuration", "Debug"); 
            ("BuildProjectReferences", "true");
            ("TreatWarningsAsErrors", "true");
        ]
    let solution = [ "./SuaveHost/SuaveHost.fsproj" ]
    MSBuild buildDir target parameters solution
    |> Log "MSBuild Output: "
)
Target "BuildDeploy" (fun _ ->
    let target = "Build"
    let parameters = [
            ("Configuration", "Release"); 
            ("BuildProjectReferences", "true");
            ("TreatWarningsAsErrors", "true");
        ]
    let solution = [ "./SuaveHost/SuaveHost.fsproj" ]
    MSBuild deployDir target parameters solution
    |> Log "MSBuild Output: "
)

Target "BuildTests" (fun _ ->
    let target = "Build"
    let parameters = [
            ("Configuration", "Release"); 
            ("BuildProjectReferences", "true");
            ("TreatWarningsAsErrors", "true");
        ]
    let solution = [ "./Berechnung.Test/Berechnung.Test.fsproj" ]
    MSBuild testOutputDir target parameters solution
    |> Log "MSBuild Output: "
)

Target "BuildClient" (fun _ ->
    printfn "Yarn version:"
    run yarnTool "--version" clientDir
    //run yarnTool "install" clientDir
    printfn "Running elm-app to build the Elm app:"
    run elmAppTool "build" clientDir
)

Target "RunTests" (fun _ ->
    !! testAssemblies
    |> Expecto (fun p ->
        { p with
            Parallel = false} )
)

Target "CopyLocal" (fun _ ->
    let copyToBuildDir = CopyFile buildDir   
    copyToBuildDir "SuaveHost/config.yaml"

    CopyFile (buildDir + "/SuaveHost.exe.config") "SuaveHost/app.config"
)

Target "CopyDeploy" (fun _ ->
    let copyToTempDir = CopyFile deployDir
    copyToTempDir "web.config"
    copyToTempDir "SuaveHost/config.yaml"
    CopyFile (deployDir + "/SuaveHost.exe.config") "SuaveHost/app.config"
)

Target "CopyClientDeploy" (fun _ ->
    printfn "*** Copying client to deployment directory ***"
    // TODO
)

Target "UploadToAzure" (fun _ ->
    printfn "*** Uploading to Azure ***"
    

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
        sprintf """ /command "open %s" "synchronize remote -delete -criteria=either "".\deploy"" ""/site/wwwroot"" " "exit" """ connectionString

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
    run winScpExePath finalCommand "."

    )

// Promote all staged files into the real application
Target "Deploy" (fun _ ->
    printfn "*** Finished deployment ***"
)

// Build order
"Clean"
    ==> "BuildLocal"
    ==> "CopyLocal"

// Set up dependencies
"Clean"
    ==> "BuildDeploy"
    ==> "CopyDeploy"
    //==> "RunTests"
    ==> "BuildClient"    
    ==> "CopyClientDeploy"
    ==> "UploadToAzure"
    ==> "Deploy"

RunTargetOrDefault "CopyLocal"
//RunTargetOrDefault "Deploy"