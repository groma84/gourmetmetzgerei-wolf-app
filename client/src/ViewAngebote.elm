module ViewAngebote exposing (..)

import Html exposing (Html, text, div, h1, h2, h3, span, section, p, ol, ul, li, a, input)
import Html.Attributes exposing (id, href, type_, value, readonly)
import Tachyons exposing (classes)
import Tachyons.Classes exposing (..)
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
                            li []
                                [ span [] [ text angebot.bezeichnung ]
                                , span [] [ text ((angebot.preis |> formatN) ++ " € für " ++ (angebot.menge |> toString) ++ "gr.") ]
                                ]

                        eineGruppe grp =
                            li []
                                [ Html.h2 [] [ text grp.gruppe ]
                                , ul [] (List.map einAngebot grp.angebote)
                                ]
                    in
                        ol [] (List.map eineGruppe s)
    in
        div [ id "angebote" ] [ content ]
