namespace GmwApp.Server

open FSharp.Data
open Chessie.ErrorHandling
open GmwApp.Data.Errors 

module Downloader =
    let download url =   
        try
            ok (Http.RequestString url)
        with
            | ex -> fail (Error.HtmlDownloadFailed ex)  
