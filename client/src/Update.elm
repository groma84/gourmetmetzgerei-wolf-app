module Update exposing (..)

import RemoteData exposing (WebData)
import Types exposing (..)


type Msg
    = NoOp
    | TagesmenuesReceived (WebData (List Tagesmenue))
    | AngebotsgruppenReceived (WebData (List Angebotsgruppe))
    | SwitchTo Tab


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        NoOp ->
            ( model, Cmd.none )

        TagesmenuesReceived received ->
            ( { model | tagesmenues = received }, Cmd.none )

        AngebotsgruppenReceived received ->
            ( { model | angebotsgruppen = received }, Cmd.none )

        SwitchTo newTab ->
            ( { model | activeTab = newTab }, Cmd.none )
