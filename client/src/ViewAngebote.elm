module ViewAngebote exposing (..)

import Html exposing (Html, text, div, h1, h2, h3, span, section, p, ol, ul, li, a, input)
import Html.Attributes exposing (id, href, type_, value, readonly)
import Tachyons exposing (classes)
import Tachyons.Classes exposing (..)
import MyTachyons exposing (..)
import RemoteData exposing (..)
import FormatNumber exposing (format)
import Update exposing (..)
import Types exposing (..)
import ViewHelper exposing (loadingSpinner, germanLocaleTwoDecimals)


view : WebData (List Angebotsgruppe) -> Html Msg
view angebotsgruppen =
    let
        formatN n =
            format germanLocaleTwoDecimals n

        content =
            case angebotsgruppen of
                NotAsked ->
                    loadingSpinner

                Loading ->
                    loadingSpinner

                Failure f ->
                    div [ classes [ red ] ]
                        [ Html.h1 [] [ text "Es ist leider ein Fehler aufgetreten" ]
                        , span [] [ text <| toString <| f ]
                        ]

                Success s ->
                    let
                        einAngebot angebot =
                            li [ classes [ striped__light_gray, mb2 ] ]
                                [ span [ classes [ dib, w_70, pa1 ] ] [ text angebot.bezeichnung ]
                                , span [ classes [ dib, w_25, pa1, ml2, v_top ] ] [ text ((angebot.preis |> formatN) ++ " € für " ++ (angebot.menge |> toString) ++ "gr.") ]
                                ]

                        eineGruppe grp =
                            li [ classes [ pa2, bb, bt_0, bl_0, br_0, bc1 ] ]
                                [ Html.h2 [ classes [ ma1, f6, f3_ns ] ] [ text grp.gruppe ]
                                , ul [ classes [ list, pl3 ] ] (List.map einAngebot grp.angebote)
                                ]
                    in
                        ol [ classes [ list, pa0, ma0 ] ] (List.map eineGruppe s)
    in
        div [ id "angebote" ] [ content ]
