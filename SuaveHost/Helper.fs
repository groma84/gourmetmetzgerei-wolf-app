namespace GmwApp.SuaveHost

open Chessie.ErrorHandling

module Helper =
    let log twoTrackInput = 
        let success(x,msgs) = ()
        let failure msgs = printf "ERROR. %A" msgs
        eitherTee success failure twoTrackInput 