module ViewTagesmenues exposing (view)

import Html exposing (Html, text, div, span, ol, ul, li)
import Html.Attributes exposing (id)
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
                            li [ classes [ mb2 ] ]
                                [ span [ classes [ dib, w_80, pa1 ] ] [ text gericht.gericht ]
                                , span [ classes [ dib, w_10, pa1, ml2, v_top ] ] [ text ((gericht.preis |> formatN) ++ " €") ]
                                ]

                        einTag menue =
                            li [ classes [ striped__light_gray, pa2 ] ]
                                [ Html.h2 [ classes [ ma1, f6, f3_ns ] ] [ text menue.wochentag ]
                                , ul [ classes [ list, pl3 ] ] (List.map einGericht menue.gerichte)
                                ]
                    in
                        ol [ classes [ list, pa0, ma0 ] ] (List.map einTag s)
    in
        div [ id "tagesmenues", classes [] ] [ content ]
