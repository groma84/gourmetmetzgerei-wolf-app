open Suave                 // always open suave
open Suave.Successful      // for OK-result
open Suave.Web             // for config
open System
open System.Net

[<EntryPoint>]
let main [| port |] =
    let config =
        { defaultConfig with
              bindings = [ HttpBinding.mk HTTP IPAddress.Loopback (uint16 port) ]
              listenTimeout = TimeSpan.FromMilliseconds 3000. }

    // rest of application
    startWebServer config (OK "Hello World!")

    0 // main return value?
