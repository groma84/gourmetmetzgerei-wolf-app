module Main exposing (..)

import Html exposing (Html, button, text, div, nav, section, ul, li)
import Html.Attributes exposing (id, type_)
import Html.Events exposing (onClick)
import Date
import Task
import RemoteData exposing (..)
import RemoteData.Http
import Json.Decode
import Tachyons exposing (classes)
import Tachyons.Classes exposing (..)
import MyTachyons exposing (..)
import Types exposing (..)
import Config
import Json exposing (..)
import Update exposing (..)
import ViewImpressum exposing (view)
import ViewAngebote exposing (view)
import ViewTagesmenues exposing (view)


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


type alias TabClasses =
    { tagesmenuesClasses : List String
    , angeboteClasses : List String
    , impressumClasses : List String
    }


view : Model -> Html Msg
view model =
    let
        visibleContent =
            case model.activeTab of
                Tagesmenues ->
                    ViewTagesmenues.view model.tagesmenues

                Angebote ->
                    ViewAngebote.view model.angebotsgruppen

                Impressum ->
                    ViewImpressum.view

        inactiveTabClasses =
            [ bn, lh_copy, button_reset, pointer, w_third ]

        activeTabClasses =
            [ ttu, bn, lh_copy, button_reset, pointer, w_third ]

        tabClasses =
            case model.activeTab of
                Tagesmenues ->
                    { tagesmenuesClasses = activeTabClasses
                    , angeboteClasses = inactiveTabClasses
                    , impressumClasses = inactiveTabClasses
                    }

                Angebote ->
                    { tagesmenuesClasses = inactiveTabClasses
                    , angeboteClasses = activeTabClasses
                    , impressumClasses = inactiveTabClasses
                    }

                Impressum ->
                    { tagesmenuesClasses = inactiveTabClasses
                    , angeboteClasses = inactiveTabClasses
                    , impressumClasses = activeTabClasses
                    }
    in
        div [ classes [ flex, flex_column, mw6, mauto, sans_serif, f6, f5_ns ] ]
            [ div []
                [ nav [ id "navigation" ]
                    [ ul [ classes [ bb, bt_0, bl_0, br_0, bc1, pa0, flex, justify_evenly ] ]
                        [ button [ type_ "button", onClick (SwitchTo Tagesmenues), classes (bgc2 :: tabClasses.tagesmenuesClasses) ] [ text "Tagesmenü" ]
                        , button [ type_ "button", onClick (SwitchTo Angebote), classes (bgc3 :: tabClasses.angeboteClasses) ] [ text "Angebote" ]
                        , button [ type_ "button", onClick (SwitchTo Impressum), classes (bgc4 :: tabClasses.impressumClasses) ] [ text "Impressum" ]
                        ]
                    ]
                , section [ id "content" ]
                    [ visibleContent
                    ]
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
