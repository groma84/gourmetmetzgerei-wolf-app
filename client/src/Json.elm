module Json exposing (..)

import Json.Decode
import Json.Decode.Pipeline
import Json.Encode
import Types exposing (..)


decodeGericht : Json.Decode.Decoder Gericht
decodeGericht =
    Json.Decode.Pipeline.decode Gericht
        |> Json.Decode.Pipeline.required "Gericht" (Json.Decode.string)
        |> Json.Decode.Pipeline.required "Preis" (Json.Decode.float)


encodeGericht : Gericht -> Json.Encode.Value
encodeGericht record =
    Json.Encode.object
        [ ( "Gericht", Json.Encode.string <| record.gericht )
        , ( "Preis", Json.Encode.float <| record.preis )
        ]


decodeTagesmenue : Json.Decode.Decoder Tagesmenue
decodeTagesmenue =
    Json.Decode.Pipeline.decode Tagesmenue
        |> Json.Decode.Pipeline.required "Wochentag" (Json.Decode.string)
        |> Json.Decode.Pipeline.required "Gerichte" (Json.Decode.list decodeGericht)


encodeTagesmenue : Tagesmenue -> Json.Encode.Value
encodeTagesmenue record =
    Json.Encode.object
        [ ( "Wochentag", Json.Encode.string <| record.wochentag )
        , ( "Gerichte", Json.Encode.list <| List.map encodeGericht <| record.gerichte )
        ]


decodeAngebot : Json.Decode.Decoder Angebot
decodeAngebot =
    Json.Decode.Pipeline.decode Angebot
        |> Json.Decode.Pipeline.required "Preis" (Json.Decode.float)
        |> Json.Decode.Pipeline.required "Menge" (Json.Decode.int)
        |> Json.Decode.Pipeline.required "Bezeichnung" (Json.Decode.string)


encodeAngebot : Angebot -> Json.Encode.Value
encodeAngebot record =
    Json.Encode.object
        [ ( "Preis", Json.Encode.float <| record.preis )
        , ( "Menge", Json.Encode.int <| record.menge )
        , ( "Bezeichnung", Json.Encode.string <| record.bezeichnung )
        ]


decodeAngebotsgruppe : Json.Decode.Decoder Angebotsgruppe
decodeAngebotsgruppe =
    Json.Decode.Pipeline.decode Angebotsgruppe
        |> Json.Decode.Pipeline.required "Gruppe" (Json.Decode.string)
        |> Json.Decode.Pipeline.required "Eintraege" (Json.Decode.list decodeAngebot)


encodeAngebotsgruppe : Angebotsgruppe -> Json.Encode.Value
encodeAngebotsgruppe record =
    Json.Encode.object
        [ ( "Gruppe", Json.Encode.string <| record.gruppe )
        , ( "Eintraege", Json.Encode.list <| List.map encodeAngebot <| record.angebote )
        ]
