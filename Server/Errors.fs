module Errors

open System

type Error =
    | HtmlDownloadFailed of Exception
    | ParsingTagesmenueFailed of Exception