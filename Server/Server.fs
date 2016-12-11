module Server

let getTagesmenue url =
    let html = Downloader.download url
    let tagesmenue = 
        match html with
        | Some x -> Some(Parser.parseMittagsmenue x)
        | None -> None
      
    tagesmenue
