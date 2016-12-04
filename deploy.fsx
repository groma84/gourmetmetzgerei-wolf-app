// --------------------------------------------------------------------------------------
// FAKE deploy script
// --------------------------------------------------------------------------------------
 
#r @"packages/FAKE/tools/FakeLib.dll"
 
open System
open System.IO
open Fake
  
Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
  
// Step 2. Use the packages
  
#r "packages/Suave/lib/net40/Suave.dll"
  
open Suave // always open suave
open Suave.Http.Successful // for OK-result
open Suave.Web // for config
open Suave.Types
open System.Net
 
let port = Sockets.Port.Parse <| getBuildParamOrDefault "port" "8083"
 
let serverConfig = 
    { defaultConfig with
       bindings = [ HttpBinding.mk HTTP IPAddress.Loopback port ]
    }
  
startWebServer defaultConfig (OK "Hello World!")
