module Main exposing (..)

import Html exposing (Html, text, div, img)
import Html.Attributes exposing (src)
import Date
import Task
import RemoteData exposing (..)

---- MODEL ----
type Tab  
    = Tagesmenues
    | Angebote
    | Impressum

type alias TagesmenueViewModel = 
    {
    }

type alias Tagesmenue =
    {

    }

type alias AngeboteViewModel =
    {

    }

type alias Angebot = 
    {

    }


type alias Model =
    {
        activeTab : Tab
        , currentDay : Date.Day
        , tagesmenueViewModel : TagesmenueViewModel
        , tagesmenueData : WebData (List Tagesmenue)
        , angeboteViewModel : AngeboteViewModel
        , angeboteData : WebData (List Angebot)
    }


init : ( Model, Cmd Msg )
init =
    ( {
        activeTab = Tagesmenues
        , currentDay = (0 |> Date.fromTime |> Date.dayOfWeek)
        , tagesmenueViewModel = {
        }
        , tagesmenueData = NotAsked
        , angeboteViewModel = {}
        , angeboteData = NotAsked
    },
    -- TODO: Task starten, um currentDate zu bestimmen 
    Task.perform CurrentDateReceived Date.now )



---- UPDATE ----


type Msg
    = NoOp
    | CurrentDateReceived Date.Date


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        NoOp ->
            ( model, Cmd.none )
                
        CurrentDateReceived date ->
            ( { model | currentDay = date |> Date.dayOfWeek }, Cmd.none )



---- VIEW ----


view : Model -> Html Msg
view model =
    div []
        [ img [ src "/logo.svg" ] []
        , div [] [ text "Your Elm App is working!" ]
        ]



---- PROGRAM ----


main : Program Never Model Msg
main =
    Html.program
        { view = view
        , init = init
        , update = update
        , subscriptions = always Sub.none
        }
