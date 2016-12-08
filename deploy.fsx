// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Azure.Kudu


let appReferences  =
    !! "/**/*.csproj"
    ++ "/**/*.fsproj"

let buildDir  = "./build/"

// Build and stage the web application
Target "Build" (fun _ ->
    // compile all projects below src/app/
    MSBuildDebug buildDir "Build" appReferences
    |> Log "AppBuild-Output: "
)

// Promote all staged files into the real application
Target "Deploy" kuduSync

// Set up dependencies
"Build"
==> "Deploy"

RunTargetOrDefault "Deploy"