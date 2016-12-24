namespace GmwApp.Server

open FSharp.Data
open GmwApp.Data.Errors 

module Downloader =
    let download url =   
        (Http.RequestString url)
