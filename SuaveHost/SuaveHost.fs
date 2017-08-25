namespace GmwApp.SuaveHost

open FSharp.Configuration

open Suave                 // always open suave
open Suave.Successful      // for OK-result
open Suave.Web             // for config
open Suave.Operators
open Suave.Filters
open Suave.Writers // setHeader

open Microsoft.FSharpLu.Json.Compact

open System
open System.Net

open GmwApp.Server
open GmwApp.Data.Errors

module SuaveHost =
    type Config = YamlConfig<"config.yaml">
    
    [<EntryPoint>]
    let main (args : string[]) =
        let port = args.[0]

        let config = Config()

        let setOk (inputJson) = 
            inputJson |> OK >=> Writers.setMimeType "application/json; charset=utf-8"

        let app : WebPart =
            choose
                [ 
                    path "/" >=> Files.file "index.html" >=> setHeader "Cache-Control" "no-cache, no-store, must-revalidate"
                    path "/index.html" >=> Files.file "index.html" >=> setHeader "Cache-Control" "no-cache, no-store, must-revalidate"
            
                    path "/tagesmenue" >=> warbler (fun req -> (Server.getTagesmenue config.Database.DatabaseFile config.Urls.Mittagsmenue.AbsoluteUri DateTime.Now |> serialize |> setOk))
                    path "/angebote" >=> warbler (fun req -> (Server.getAngebote config.Database.DatabaseFile config.Urls.Angebote.AbsoluteUri DateTime.Now |> serialize |> setOk))
                    
                    Files.browseHome >=> setHeader "Cache-Control" "public, max-age=43200" // statische Files mit Caching für 30 Tage
                ]

        // Bootstrapping
        Db.bootstrap config.Database.DatabaseFile
        
    // was nicht in dieser Liste steht, wird von Files.browseHome auch nicht ausgeliefert
        let restrictiveMimeTypesMap = function
            | ".bmp" -> createMimeType "image/bmp" false
            | ".css" -> createMimeType "text/css" true
            | ".gif" -> createMimeType "image/gif" false
            | ".png" -> createMimeType "image/png" false
            | ".svg" -> createMimeType "image/svg+xml" true
            | ".ico" -> createMimeType "image/x-icon" false
    //        | ".xml" -> createMimeType "application/xml" true
            | ".js"  -> createMimeType "application/javascript" true
    //        | ".json" -> createMimeType "application/json" true
            | ".map"  -> createMimeType "application/json" true
            | ".htm"
            | ".html" -> createMimeType "text/html" true
    //        | ".jpe"
            | ".jpeg"
            | ".jpg" -> createMimeType "image/jpeg" false
    //        | ".exe" -> createMimeType "application/exe" false
            | ".pdf" -> createMimeType "application/pdf" false
    //        | ".txt" -> createMimeType "text/plain" true
            | ".ttf" -> createMimeType "application/x-font-ttf" true
            | ".otf" -> createMimeType "application/font-sfnt" true
            | ".woff" -> createMimeType "application/font-woff" false
            | ".woff2" -> createMimeType "application/font-woff2" false
            | ".eot" -> createMimeType "application/vnd.ms-fontobject" false
            | _      -> None

        let suaveConfig =
            { defaultConfig with
                bindings = [ HttpBinding.create HTTP IPAddress.Loopback (uint16 port) ]
                listenTimeout = TimeSpan.FromMilliseconds 3000.
                mimeTypesMap = restrictiveMimeTypesMap }

        // rest of application
        startWebServer suaveConfig app

        0 // main return value?
