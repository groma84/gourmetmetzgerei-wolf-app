module Downloader

open FSharp.Data

let download url =   
    try
        Some(Http.RequestString(url))
    with
        | _ -> None   
