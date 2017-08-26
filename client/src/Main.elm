module Main exposing (..)

import Html exposing (Html, text, div, img)
import Html.Attributes exposing (src)
import Date
import Task
import RemoteData exposing (..)
import RemoteData.Http
import Json.Decode

import Types exposing (..)
import Config
import Json exposing (..)


init : ( Model, Cmd Msg )
init =
    ( {
        activeTab = Tagesmenues
        , currentDay = (0 |> Date.fromTime |> Date.dayOfWeek)
        , tagesmenueViewModel = {
        }
        , tagesmenues = NotAsked
        , angeboteViewModel = {}
        , angebotsgruppen = NotAsked
    },
    Cmd.batch [
        Task.perform CurrentDateReceived Date.now 
        , RemoteData.Http.get Config.tagesmenueUrl TagesmenuesReceived (Json.Decode.list decodeTagesmenue)
        , RemoteData.Http.get Config.angeboteUrl AngebotsgruppenReceived (Json.Decode.list decodeAngebotsgruppe)
    ]
    )



---- UPDATE ----


type Msg
    = NoOp
    | CurrentDateReceived Date.Date
    | TagesmenuesReceived (WebData (List Tagesmenue))
    | AngebotsgruppenReceived (WebData (List Angebotsgruppe))


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        NoOp ->
            ( model, Cmd.none )
                
        CurrentDateReceived date ->
            ( { model | currentDay = date |> Date.dayOfWeek }, Cmd.none )
        
        TagesmenuesReceived received ->
            ( { model | tagesmenues = received }, Cmd.none )

        AngebotsgruppenReceived received ->
            ( { model | angebotsgruppen = received }, Cmd.none )
        




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
