module LogicTest

open NUnit.Framework
open FsUnit

open Logic

[<Test>]
let ``Should output Hello World`` () =
    HelloWorld.helloWorldString |> should equal "Hello World"
