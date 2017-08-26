module Types exposing (..)

import Date
import RemoteData exposing (WebData)


type Tab
    = Tagesmenues
    | Angebote
    | Impressum


type alias TagesmenueViewModel =
    {}


type alias Tagesmenue =
    { wochentag : String
    , gerichte : List Gericht
    }


type alias Gericht =
    { gericht : String
    , preis : Float
    }


type alias AngeboteViewModel =
    {}


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
    , currentDay : Date.Day
    , tagesmenueViewModel : TagesmenueViewModel
    , tagesmenues : WebData (List Tagesmenue)
    , angeboteViewModel : AngeboteViewModel
    , angebotsgruppen : WebData (List Angebotsgruppe)
    }
