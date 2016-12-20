module Parser

open System.Text.RegularExpressions
open FSharp.Data
open Chessie.ErrorHandling
open Errors

type Preis = double
type Gericht = string
type Wochentag =  string

type GerichtMitPreis = {
    Gericht: Gericht;
    Preis: Preis;
}

type Tagesmenu = {
    Wochentag: Wochentag;
    Gerichte: GerichtMitPreis list;
}

type Eintrag = {
    Preis: double;
    Menge: int option;
    Bezeichnung: string;
}

type Angebot = {
    Gruppe: string;
    Eintraege: Eintrag list;
}

type Woche = {
    Tagesmenues: Tagesmenu list;
    Angebote: Angebot list;
}

let parseMittagsmenue (html : string) =
    let extractInnertext (htmlNode : HtmlNode) =
        htmlNode.InnerText()

    let splitAtSpace (str : string) =
        str.Split(' ')

    let replaceCommaWithDot (str : string) =
        str.Replace(",", ".")
    
    let getWochentag (htmlNode: HtmlNode) : Wochentag =
        htmlNode.Elements ["h2"]   
        |> Seq.map extractInnertext
        |> Seq.head

    let getGerichte (tagesNode : HtmlNode) =
        tagesNode.Descendants ["div"]
            |> Seq.filter(fun divNode -> divNode.HasClass("menue"))
            |> Seq.map(fun menueNode ->
                    let preis = menueNode.Descendants ["span"]
                                |> Seq.head
                                |> extractInnertext
                                |> splitAtSpace 
                                |> Seq.last
                                |> replaceCommaWithDot
                                |> float

                    let gericht = menueNode.Descendants ["div"]
                                    |> Seq.head
                                    |> extractInnertext

                    {
                        Preis = preis;
                        Gericht = gericht;
                    }
                )
            |> List.ofSeq

    try
        let document = HtmlDocument.Parse html
        let tagesmenues = 
            document.Descendants ["div"]
            |> Seq.filter(fun node -> node.HasClass("day"))
            |> Seq.map(fun tagesNode ->
                {
                    Wochentag = getWochentag tagesNode;
                    Gerichte = getGerichte tagesNode;
                }
            )
            |> List.ofSeq

        ok tagesmenues
    with
        | ex -> fail (Error.ParsingTagesmenueFailed ex)

let parseAngebote (html : string) =
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

    let parsePreis entry =
        entry
        |> extractInnertext
        |> splitAtSpace
        |> Seq.last
        |> replaceCommaWithDot
        |> float;

    let parseMenge entry =
        entry
        |> extractInnertext
        |> limitToNumbers;

    let parseBezeichnung entry =
        entry
        |> extractInnertext;


    let parseRow (row : HtmlNode) =
        let entries = row.Descendants ["td"] |> Array.ofSeq
        {
            Preis = parsePreis entries.[2];
            Menge = parseMenge entries.[1];
            Bezeichnung = parseBezeichnung entries.[0];
        }

    let parseGruppenBezeichnung (gruppeNode : HtmlNode) =
        gruppeNode.Descendants ["h2"] 
        |> Seq.head 
        |> extractInnertext

    let parseAngebotsgruppe (gruppeNode : HtmlNode) =
        let items = gruppeNode.Descendants ["tr"]
                        |> Seq.map parseRow
                        |> List.ofSeq
        {
            Gruppe = parseGruppenBezeichnung gruppeNode;
            Eintraege = items;
        }

    let parseAngebotsgruppen (doc : HtmlDocument) =
        doc.Descendants ["div"]
        |> Seq.filter(fun node -> node.HasClass("angebote"))
        |> Seq.map parseAngebotsgruppe

    try
        let document = HtmlDocument.Parse html
        let angebote = parseAngebotsgruppen document        
        ok angebote
    with
        | ex -> fail (Error.ParsingAngeboteFailed ex)    