module ViewHelper exposing (..)

import Html exposing (Html, div, i)
import Html.Attributes exposing (class)
import FormatNumber.Locales exposing (Locale)
import Update exposing (..)
import Tachyons exposing (classes)
import Tachyons.Classes exposing (tc)


germanLocaleTwoDecimals : Locale
germanLocaleTwoDecimals =
    Locale 2 "." "," "-" ""


loadingSpinner : Html Msg
loadingSpinner =
    div [ classes [ tc ] ]
        [ div [ class "cssload-container" ] []
        , div [ class "cssload-loading" ] [ i [] [], i [] [] ]
        ]
