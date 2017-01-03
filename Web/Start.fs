namespace Web

open Fable.Core 
open Fable.Import

module Start = 
    Node.require.Invoke("core-js") |> ignore

    let element = Browser.document.getElementById "sample"
    element.innerText <- "Hello, world !!"