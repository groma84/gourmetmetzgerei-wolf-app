// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open System
open System.IO
open Fake
open Fake.Testing.Expecto
open Fake.Azure.Kudu

// Directories
let buildDir  = "./build/"
let clientDir = "./client/"
let testOutputDir = "./tests/"

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
    CleanDirs [buildDir; testOutputDir; deploymentTemp;] 
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
Target "BuildKudu" (fun _ ->
    let target = "Build"
    let parameters = [
            ("Configuration", "Release"); 
            ("BuildProjectReferences", "true");
            ("TreatWarningsAsErrors", "true");
        ]
    let solution = [ "./SuaveHost/SuaveHost.fsproj" ]
    MSBuild deploymentTemp target parameters solution
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

Target "CopyKudu" (fun _ ->
    let copyToTempDir = CopyFile deploymentTemp
    copyToTempDir "web.config"
    copyToTempDir "SuaveHost/config.yaml"
    CopyFile (deploymentTemp + "/SuaveHost.exe.config") "SuaveHost/app.config"
)

// Promote all staged files into the real application
Target "Deploy" kuduSync

// Build order
"Clean"
    ==> "BuildLocal"
    ==> "BuildClient"
    ==> "CopyLocal"

// Set up dependencies
"Clean"
    ==> "BuildKudu"
    ==> "CopyKudu"
    ==> "Deploy"

// RunTargetOrDefault "CopyLocal"
RunTargetOrDefault "Deploy"