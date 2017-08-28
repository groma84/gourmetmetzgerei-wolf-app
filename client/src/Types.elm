module Types exposing (..)

import RemoteData exposing (WebData)


type Tab
    = Tagesmenues
    | Angebote
    | Impressum


type alias Tagesmenue =
    { wochentag : String
    , gerichte : List Gericht
    }


type alias Gericht =
    { gericht : String
    , preis : Float
    }


type alias Angebotsgruppe =
    { gruppe : String
    , angebote : List Angebot
    }


type alias Angebot =
    { preis : Float
    , menge : Int
    , bezeichnung : String
    }


type alias Model =
    { activeTab : Tab
    , tagesmenues : WebData (List Tagesmenue)
    , angebotsgruppen : WebData (List Angebotsgruppe)
    }
