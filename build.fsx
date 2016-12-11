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

Target "BuildLocal" (fun _ ->
    MSBuildDebug buildDir "Build" appReferences
    |> Log "AppBuild-Output buildDir: "
)
Target "BuildKudu" (fun _ ->
    MSBuildDebug deploymentTemp "Build" appReferences
    |> Log "AppBuild-Output deploymentTemp: "
)

let testDlls = !! (testDir + "/*Test.dll")
Target "BuildTest" (fun _ ->
    MSBuildDebug testDir "Build" testReferences
        |> Log "TestBuild-Output: "
)
Target "RunTests" (fun _ ->
    testDlls
        |> NUnit3  (fun p -> 
            {p with
                ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe";
                Workers = Some 1;
                WorkingDir = testDir;
                })
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
    ==> "CopyLocal"
    ==> "BuildTest"
    ==> "RunTests"

// Set up dependencies
"Clean"
    ==> "BuildKudu"
    ==> "CopyKudu"
    ==> "Deploy"

// RunTargetOrDefault "CopyLocal"
RunTargetOrDefault "Deploy"