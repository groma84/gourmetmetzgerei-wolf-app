module Server

open Chessie.ErrorHandling
open Errors

let getTagesmenue (url) =
    Downloader.download url
    >>= Parser.parseMittagsmenue
    