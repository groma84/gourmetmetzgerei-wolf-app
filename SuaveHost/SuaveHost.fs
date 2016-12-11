open FSharp.Configuration

open Suave                 // always open suave
open Suave.Successful      // for OK-result
open Suave.Web             // for config
open Suave.Operators
open Suave.Filters

open Newtonsoft.Json
open Newtonsoft.Json.Serialization

open System
open System.Net

open Server


type Config = YamlConfig<"./config.yaml">

[<EntryPoint>]
let main (args : string[]) =
    let port = args.[0]
    let suaveConfig =
        { defaultConfig with
              bindings = [ HttpBinding.mk HTTP IPAddress.Loopback (uint16 port) ]
              listenTimeout = TimeSpan.FromMilliseconds 3000. }

    let config = Config()

   // let tm = 
  //  let result = (Server.getTagesmenue config.Urls.Mittagsmenue.AbsoluteUri)
   //     match result with
    //        | Success s -> "OK"
    //        | Failure f -> "FAIL"

    let asJson (input) : WebPart =   
        let jsonSerializerSettings = new JsonSerializerSettings()
        jsonSerializerSettings.ContractResolver <- new CamelCasePropertyNamesContractResolver()
        JsonConvert.SerializeObject(input, jsonSerializerSettings)
        |> OK
        >=> Writers.setMimeType "application/json; charset=utf-8"

  

    let app : WebPart =
        choose
            [ 
                path "/" >=> OK "Hello World! MGr mit Routing";
                path "/tagesmenue" >=> asJson (Server.getTagesmenue config.Urls.Mittagsmenue.AbsoluteUri);
            ]

    // rest of application
    startWebServer suaveConfig app

    0 // main return value?
