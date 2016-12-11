module Downloader

open FSharp.Data
open Railway
open Errors

let download url =   
    try
        succeed (Http.RequestString url)
    with
        | ex -> fail (Error.HtmlDownloadFailed ex)  
