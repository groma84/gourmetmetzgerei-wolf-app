namespace GmwApp.Data

open Chiron

module Types =
    type YearAndWeek = 
        {
            Year: int;
            Week: int;
        }
        static member ToJson (x:YearAndWeek) = json {
            do! Json.write "year" x.Year
            do! Json.write "week" x.Week
        }
        static member FromJson (_:YearAndWeek) = json {
            let! y = Json.read "year"
            let! w = Json.read "week"
            return { Year = y; Week = w }
        }
            

    type Preis = double
    type Gericht = string
    type Wochentag = string

    type GerichtMitPreis = 
        {
            Gericht: Gericht;
            Preis: Preis;
        }
        static member ToJson (x:GerichtMitPreis) = json {
            do! Json.write "item" x.Gericht
            do! Json.write "price" x.Preis
        }
        static member FromJson (_:GerichtMitPreis) = json {
            let! g = Json.read "item"
            let! p = Json.read "price"
            return { Gericht = g; Preis = p }
        }

    type Tagesmenu = 
        {
            Wochentag: Wochentag;
            Gerichte: GerichtMitPreis list;
        }
        static member ToJson (x:Tagesmenu) = json {
            do! Json.write "weekday" x.Wochentag
            do! Json.write "items" x.Gerichte
        }
        static member FromJson (_:Tagesmenu) = json {
            let! w = Json.read "weekday"
            let! g = Json.read "items"
            return { Wochentag = w; Gerichte = g }
        }

    type Eintrag = 
        {
            Preis: double;
            Menge: int option;
            Bezeichnung: string;
        }
        static member ToJson (x:Eintrag) = json {
            do! Json.write "price" x.Preis
            do! Json.write "amount" x.Menge
            do! Json.write "name" x.Bezeichnung
        }
        static member FromJson (_:Eintrag) = json {
            let! p = Json.read "price"
            let! a = Json.read "amount"
            let! n = Json.read "name"
            return { Preis = p; Menge = a; Bezeichnung = n }
        }

    type Angebot = 
        {
            Gruppe: string;
            Eintraege: Eintrag list;
        }
        static member ToJson (x:Angebot) = json {
            do! Json.write "group" x.Gruppe
            do! Json.write "entries" (Array.ofList x.Eintraege)
        }
        static member FromJson (_:Angebot) = json {
            let! g = Json.read "group"
            let! e = Json.read "entries"
            return { Gruppe = g; Eintraege = List.ofArray e }
        }

    type DataOrigin =
        | FromDatabase
        | ViaDownload