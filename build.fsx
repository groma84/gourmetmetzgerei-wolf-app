// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing
open Fake.Azure.Kudu

// Directories
let buildDir  = "./build/"
let testDir = "./test/"


// Filesets
let appReferences  =
    !! "/**/*.csproj"
    ++ "/**/*.fsproj"

let testReferences  =
    !! "/**/*Test.csproj"
    ++ "/**/*Test.fsproj"

// version info
let version = "0.1"  // or retrieve from CI server

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; testDir; deploymentTemp;] 
)

Target "Build" (fun _ ->
    MSBuildDebug buildDir "Build" appReferences
    |> Log "AppBuild-Output buildDir: "

    MSBuildDebug deploymentTemp "Build" appReferences
    |> Log "AppBuild-Output deploymentTemp: "
)

let testDlls = !! (testDir + "/*Test.dll")
Target "BuildTest" (fun _ ->
    MSBuildDebug testDir "Build" testReferences
        |> Log "TestBuild-Output: "
)
Target "Tests" (fun _ ->
    testDlls
        |> NUnit3  (fun p -> 
            {p with
                ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe";
                Workers = Some 1;
                WorkingDir = testDir;
                })
)

Target "Copy" (fun _ ->
    let copyToTempDir = CopyFile deploymentTemp
    let copyToBuildDir = CopyFile buildDir 
    
    copyToTempDir "web.config"
    copyToBuildDir "web.config"

    copyToTempDir "SuaveHost/config.yaml"
    copyToBuildDir "SuaveHost/config.yaml"

    CopyFile (buildDir + "/SuaveHost.exe.config") "SuaveHost/app.config"
    CopyFile (deploymentTemp + "/SuaveHost.exe.config") "SuaveHost/app.config"
)

// Promote all staged files into the real application
Target "Deploy" kuduSync

// Build order
"Clean"
    ==> "Build"
    ==> "Copy"
    ==> "BuildTest"
    ==> "Tests"

// Set up dependencies
"Clean"
    ==>"Build"
    ==> "Copy"
    ==> "Deploy"

RunTargetOrDefault "Copy"
//RunTargetOrDefault "Deploy"