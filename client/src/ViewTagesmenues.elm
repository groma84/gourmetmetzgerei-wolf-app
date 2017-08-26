module ViewTagesmenues exposing (..)

import Html exposing (Html, text, div, h1, h2, h3, span, section, p, ol, ul, li, a, input)
import Html.Attributes exposing (id, href, type_, value, readonly)
import Tachyons exposing (classes)
import Tachyons.Classes exposing (..)
import RemoteData exposing (..)
import FormatNumber exposing (format)
import Update exposing (..)
import Types exposing (..)
import ViewHelper exposing (loadingSpinner, germanLocaleTwoDecimals)


view : WebData (List Tagesmenue) -> Html Msg
view tagesmenues =
    let
        formatN n =
            format germanLocaleTwoDecimals n

        content =
            case tagesmenues of
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
                        einGericht gericht =
                            li []
                                [ span [] [ text gericht.gericht ]
                                , span [] [ text ((gericht.preis |> formatN) ++ " €") ]
                                ]

                        einTag menue =
                            li []
                                [ Html.h2 [] [ text menue.wochentag ]
                                , ul [] (List.map einGericht menue.gerichte)
                                ]
                    in
                        ol [] (List.map einTag s)
    in
        div [ id "tagesmenues" ] [ content ]
