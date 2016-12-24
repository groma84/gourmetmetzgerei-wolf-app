namespace GmwApp.Data

module Types =
    type YearAndWeek =
        {
            Year: int;
            Week: int;
        }

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

    type DataOrigin =
        | FromDatabase
        | ViaDownload