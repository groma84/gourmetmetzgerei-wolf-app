open Suave                 // always open suave
open Suave.Successful      // for OK-result
open Suave.Web             // for config
open Suave.Operators
open Suave.Filters

open System
open System.Net
open FSharp.Configuration

type Config = YamlConfig<"./config.yaml">

[<EntryPoint>]
let main (args : string[]) =
    let port = args.[0]
    let suaveConfig =
        { defaultConfig with
              bindings = [ HttpBinding.mk HTTP IPAddress.Loopback (uint16 port) ]
              listenTimeout = TimeSpan.FromMilliseconds 3000. }


    let config = Config()

    let app : WebPart =
        choose
            [ path "/" >=> OK "Hello World! MGr mit Routing" ]

    // rest of application
    startWebServer suaveConfig app

    0 // main return value?
