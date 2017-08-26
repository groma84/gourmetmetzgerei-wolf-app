module Json exposing (..)

import Json.Decode
import Json.Decode.Pipeline
import Json.Encode
import Types exposing (..)


decodeGericht : Json.Decode.Decoder Gericht
decodeGericht =
    Json.Decode.Pipeline.decode Gericht
        |> Json.Decode.Pipeline.required "gericht" (Json.Decode.string)
        |> Json.Decode.Pipeline.required "preis" (Json.Decode.float)

encodeGericht : Gericht -> Json.Encode.Value
encodeGericht record =
    Json.Encode.object
        [ ("gericht",  Json.Encode.string <| record.gericht)
        , ("preis",  Json.Encode.float <| record.preis)
        ]

decodeTagesmenue : Json.Decode.Decoder Tagesmenue
decodeTagesmenue =
    Json.Decode.Pipeline.decode Tagesmenue
        |> Json.Decode.Pipeline.required "wochentag" (Json.Decode.string)
        |> Json.Decode.Pipeline.required "gerichte" (Json.Decode.list decodeGericht)

encodeTagesmenue : Tagesmenue -> Json.Encode.Value
encodeTagesmenue record =
    Json.Encode.object
        [ ("wochentag",  Json.Encode.string <| record.wochentag)
        , ("gerichte",  Json.Encode.list <| List.map encodeGericht <| record.gerichte)
        ]

decodeAngebot : Json.Decode.Decoder Angebot
decodeAngebot =
    Json.Decode.Pipeline.decode Angebot
        |> Json.Decode.Pipeline.required "preis" (Json.Decode.float)
        |> Json.Decode.Pipeline.required "menge" (Json.Decode.int)
        |> Json.Decode.Pipeline.required "bezeichnung" (Json.Decode.string)

encodeAngebot : Angebot -> Json.Encode.Value
encodeAngebot record =
    Json.Encode.object
        [ ("preis",  Json.Encode.float <| record.preis)
        , ("menge",  Json.Encode.int <| record.menge)
        , ("bezeichnung",  Json.Encode.string <| record.bezeichnung)
        ]

decodeAngebotsgruppe : Json.Decode.Decoder Angebotsgruppe
decodeAngebotsgruppe =
    Json.Decode.Pipeline.decode Angebotsgruppe
        |> Json.Decode.Pipeline.required "gruppe" (Json.Decode.string)
        |> Json.Decode.Pipeline.required "angebote" (Json.Decode.list decodeAngebot)

encodeAngebotsgruppe : Angebotsgruppe -> Json.Encode.Value
encodeAngebotsgruppe record =
    Json.Encode.object
        [ ("gruppe",  Json.Encode.string <| record.gruppe)
        , ("angebote",  Json.Encode.list <| List.map encodeAngebot <| record.angebote)
        ]