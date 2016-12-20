module Server

open Chessie.ErrorHandling
open Errors

let getTagesmenue (url) =
    Downloader.download url
    >>= Parser.parseMittagsmenue
    
let getAngebote (url) =
    Downloader.download url
    >>= Parser.parseAngebote