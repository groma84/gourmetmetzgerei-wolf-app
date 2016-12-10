#load "Server.fs"
#r "../packages/FSharp.Data/lib/net40/FSharp.Data.dll"

open FSharp.Data
open Server


type Tagesmenu = {
    WochentagName: string;
  //  Preis: string;
}

let convertToDouble s =
  let (ok,f) = System.Double.TryParse(s)
  if ok then f else nan

let extractInnertext htmlNode
    htmlNode.InnerText()

// Define your library scripting code here
let results = HtmlDocument.Load(@"http://www.gourmetmetzgerei-wolf.de/de/mittagsmenue")

let divs = results.Descendants ["div"]
let tage = divs |> Seq.filter(fun node -> 
    node.HasClass("day")
)
let tag = 
    tage 
    |> Seq.map(fun node ->
        let t = {
            WochentagName =  node.Elements ["h2"]   
                                |> Seq.map extractInnertext
                                |> Seq.head;

        }
      
        t
    )

let preise = 
    tage
    |> Seq.map(fun node ->
        node.Descendants ["span"]
    )