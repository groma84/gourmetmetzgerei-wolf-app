module Main exposing (..)

import Html exposing (Html, button, text, div, nav, section, ul, li)
import Html.Attributes exposing (id, type_)
import Html.Events exposing (onClick)
import Date
import Task
import RemoteData exposing (..)
import RemoteData.Http
import Json.Decode
import Types exposing (..)
import Config
import Json exposing (..)
import Update exposing (..)
import ViewImpressum exposing (view)


init : ( Model, Cmd Msg )
init =
    ( { activeTab = Tagesmenues
      , currentDay = (0 |> Date.fromTime |> Date.dayOfWeek)
      , tagesmenueViewModel =
            {}
      , tagesmenues = NotAsked
      , angeboteViewModel = {}
      , angebotsgruppen = NotAsked
      }
    , Cmd.batch
        [ Task.perform CurrentDateReceived Date.now
        , RemoteData.Http.get Config.tagesmenueUrl TagesmenuesReceived (Json.Decode.list decodeTagesmenue)
        , RemoteData.Http.get Config.angeboteUrl AngebotsgruppenReceived (Json.Decode.list decodeAngebotsgruppe)
        ]
    )



---- UPDATE ----
---- VIEW ----


view : Model -> Html Msg
view model =
    let
        visibleContent =
            case model.activeTab of
                Tagesmenues ->
                    div [ id "tagesmenues" ] [ text "T" ]

                Angebote ->
                    div [ id "angebote" ] [ text "A" ]

                Impressum ->
                    ViewImpressum.view
    in
        div []
            [ nav [ id "navigation" ]
                [ ul []
                    [ button [ type_ "button", onClick (SwitchTo Tagesmenues) ] [ text "Tagesmenü" ]
                    , button [ type_ "button", onClick (SwitchTo Angebote) ] [ text "Angebote" ]
                    , button [ type_ "button", onClick (SwitchTo Impressum) ] [ text "Impressum" ]
                    ]
                ]
            , section [ id "content" ]
                [ visibleContent
                ]
            ]



---- PROGRAM ----


main : Program Never Model Msg
main =
    Html.program
        { view = view
        , init = init
        , update = Update.update
        , subscriptions = always Sub.none
        }
