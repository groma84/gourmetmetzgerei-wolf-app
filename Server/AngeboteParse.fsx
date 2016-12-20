#r "../packages/FSharp.Data/lib/net40/FSharp.Data.dll"

open FSharp.Data
open System.Text.RegularExpressions


let convertToDouble s =
  let (ok,f) = System.Double.TryParse(s)
  if ok then f else nan

let extractInnertext (htmlNode : HtmlNode) =
    htmlNode.InnerText()

let splitAtSpace (str : string) =
    str.Split(' ')

let replaceCommaWithDot (str : string) =
    str.Replace(",", ".")

let (|Regex|_|) pattern input =
        let m = Regex.Match(input, pattern)
        if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
        else None


let limitToNumbers input =
    let wert = match input with
                | Regex "([0-9]+)" [num] -> num |> int |> Option.Some
                | _ -> None 
    wert
                        
// Define your library scripting code here
//let results = HtmlDocument.Load(@"http://www.gourmetmetzgerei-wolf.de/de/mittagsmenue")
let html = HtmlDocument.Load(@"D:\git-repos\groma84@github\gourmetmetzgerei-wolf-app\Server\angebote.html")
//let html = HtmlDocument.Load(@"E:\git\gourmetmetzgerei-wolf-app\Server\angebote.html")

type Eintrag = {
    Preis: double;
    Menge: int option;
    Bezeichnung: string;
}

type Angebot = {
    Gruppe: string;
    Eintraege: Eintrag list;
}

(*
let angebotGruppen = 
    html.Descendants ["div"]
    |> Seq.filter(fun node -> node.HasClass("angebote"))
    |> Seq.map(fun (gruppeNode : HtmlNode) -> 
        let gruppenBezeichnung = gruppeNode.Descendants ["h2"] |> Seq.head |> extractInnertext
        let items = gruppeNode.Descendants ["tr"]
                    |> Seq.map(fun row ->
                        let entries = row.Descendants ["td"] |> Array.ofSeq
                        
                        {
                            Preis = entries.[2]
                                |> extractInnertext
                                |> splitAtSpace
                                |> Seq.last
                                |> replaceCommaWithDot
                                |> float;
                            Menge = entries.[1]
                                |> extractInnertext
                                |> limitToNumbers
                                |> Seq.head
                                |> Option.map (fun numString -> numString |> int);
                            Bezeichnung = entries.[0]
                                |> extractInnertext;
                        }
                    )
        
        {
            Gruppe = gruppenBezeichnung;
            Eintraege = items |> List.ofSeq
        }
    )
*)
let x = 
    html.Descendants ["div"]
    |> Seq.filter(fun node -> node.HasClass("angebote"))
    |> Seq.map(fun (gruppeNode : HtmlNode) -> 
        let gruppenBezeichnung = gruppeNode.Descendants ["h2"] |> Seq.head |> extractInnertext

        let items = gruppeNode.Descendants ["tr"]
                        |> Seq.map(fun row ->
                            let entries = row.Descendants ["td"] |> Array.ofSeq

                            let p = entries.[2]
                                    |> extractInnertext
                                    |> splitAtSpace
                                    |> Seq.last
                                    |> replaceCommaWithDot
                                    |> float;

                            let m = entries.[1]
                                    |> extractInnertext
                                    |> limitToNumbers

                            let b = entries.[0]
                                    |> extractInnertext;

                            {
                                Preis = p;
                                Menge = m;
                                Bezeichnung = b;
                            }
                        )
        items
    )