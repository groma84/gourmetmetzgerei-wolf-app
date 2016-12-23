namespace GmwApp.Data

open System

module Errors =
    type Error =
        | HtmlDownloadFailed of Exception
        | ParsingTagesmenueFailed of Exception
        | ParsingAngeboteFailed of Exception 
        | CreateDatabaseFailed of Exception
        | CreateVersionTableFailed of Exception
        | LoadMenuFailed of Exception
