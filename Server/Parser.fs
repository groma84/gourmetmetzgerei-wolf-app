module Parser

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

type Woche = {
    Tagesmenues: Tagesmenu list;
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

        ok { Tagesmenues = tagesmenues }
    with
        | ex -> fail (Error.ParsingTagesmenueFailed ex)