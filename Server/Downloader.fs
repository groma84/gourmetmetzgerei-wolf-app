module Downloader

open FSharp.Data
open Chessie.ErrorHandling
open Errors 

let download url =   
    try
        ok (Http.RequestString url)
    with
        | ex -> fail (Error.HtmlDownloadFailed ex)  
