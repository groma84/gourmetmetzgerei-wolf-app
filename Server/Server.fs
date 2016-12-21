namespace GmwApp.Server

open Chessie.ErrorHandling
open GmwApp.Data.Errors

module Server = 
    let getTagesmenue url date =      
        Downloader.download url
        >>= Parser.parseMittagsmenue
        
    let getAngebote (url) =
        Downloader.download url
        >>= Parser.parseAngebote