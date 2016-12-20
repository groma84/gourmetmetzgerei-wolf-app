module Helper

open Chessie.ErrorHandling

let log twoTrackInput = 
    let success(x,msgs) = printfn "DEBUG. Success so far."
    let failure msgs = printf "ERROR. %A" msgs
    eitherTee success failure twoTrackInput 