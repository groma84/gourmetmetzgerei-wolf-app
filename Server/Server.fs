module Server

open Railway
open Errors

let getTagesmenue (url) : Result<Parser.Woche, Error> =
    Downloader.download url
    >>= Parser.parseMittagsmenue
    