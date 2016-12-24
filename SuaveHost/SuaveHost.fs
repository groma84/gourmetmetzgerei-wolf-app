namespace GmwApp.SuaveHost

open FSharp.Configuration

open Suave                 // always open suave
open Suave.Successful      // for OK-result
open Suave.Web             // for config
open Suave.Operators
open Suave.Filters

open Microsoft.FSharpLu.Json

open System
open System.Net

open GmwApp.Server
open GmwApp.Data.Errors

module SuaveHost =
    type Config = YamlConfig<"config.yaml">
    
    [<EntryPoint>]
    let main (args : string[]) =
        let port = args.[0]
        let suaveConfig =
            { defaultConfig with
                bindings = [ HttpBinding.mk HTTP IPAddress.Loopback (uint16 port) ]
                listenTimeout = TimeSpan.FromMilliseconds 3000. }

        let config = Config()

        let toJson input =
            Compact.serialize input 
                
        let toOutput (input) = 
            input |> toJson |> OK >=> Writers.setMimeType "application/json; charset=utf-8"

        let app : WebPart =
            choose
                [ 
                    path "/" >=> OK "Hello World! MGr mit Routing";
                    path "/tagesmenue" >=> warbler (fun req -> (Server.getTagesmenue config.Database.DatabaseFile config.Urls.Mittagsmenue.AbsoluteUri DateTime.Now |> toOutput))
                    path "/angebote" >=> warbler (fun req -> (Server.getAngebote config.Urls.Angebote.AbsoluteUri |> toOutput))
                ]

        // Bootstrapping
        Db.bootstrap config.Database.DatabaseFile
        

        // rest of application
        startWebServer suaveConfig app

        0 // main return value?
