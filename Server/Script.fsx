#r "../packages/FSharp.Data/lib/net40/FSharp.Data.dll"

open FSharp.Data

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

let convertToDouble s =
  let (ok,f) = System.Double.TryParse(s)
  if ok then f else nan

let extractInnertext (htmlNode : HtmlNode) =
    htmlNode.InnerText()

let getWochentag (htmlNode: HtmlNode) : Wochentag =
    htmlNode.Elements ["h2"]   
    |> Seq.map extractInnertext
    |> Seq.head
    

let splitAtSpace (str : string) =
    str.Split(' ')

let replaceCommaWithDot (str : string) =
    str.Replace(",", ".")

// Define your library scripting code here
//let results = HtmlDocument.Load(@"http://www.gourmetmetzgerei-wolf.de/de/mittagsmenue")
//let results = HtmlDocument.Load(@"D:\git-repos\groma84@github\gourmetmetzgerei-wolf-app\Server\mittagsmenue.html")
let results = HtmlDocument.Load(@"E:\git\gourmetmetzgerei-wolf-app\Server\mittagsmenue.html")


let divs = results.Descendants ["div"]
let tage = divs |> Seq.filter(fun node -> 
    node.HasClass("day")
)

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

let tag = 
    tage 
    |> Seq.map(fun tagesNode ->
        {
            Wochentag = getWochentag tagesNode;
            Gerichte = getGerichte tagesNode;
        }
      )

