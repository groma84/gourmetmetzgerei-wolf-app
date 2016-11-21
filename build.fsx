// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing

// Directories
let buildDir  = "./build/"
let deployDir = "./deploy/"
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
    CleanDirs [buildDir; deployDir; testDir]
)

Target "Build" (fun _ ->
    // compile all projects below src/app/
    MSBuildDebug buildDir "Build" appReferences
    |> Log "AppBuild-Output: "
)

Target "Deploy" (fun _ ->
    !! (buildDir + "/**/*.*")
    -- "*.zip"
    |> Zip buildDir (deployDir + "ApplicationName." + version + ".zip")
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

// Build order
"Clean"
    ==> "Build"
    ==> "BuildTest"
    ==> "Tests"
    ==> "Deploy"

// start build
// RunTargetOrDefault "Build"
RunTargetOrDefault "Tests"
