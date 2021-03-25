module IntersecSub.IntegrityTests

open Xunit
open IntersecSub
open System.IO

[<Fact>]
let ``Result written to output`` () =
    let inputJson = "[[1, 2], [1, 2], [2]]"
    use inputStream = new StringReader(inputJson)
    use outputStream = new StringWriter()
    Cli.run inputStream outputStream
    let result = outputStream.ToString()
    Assert.Equal("[1]\n", result)

[<Fact>]
let ``Error on not correct json`` () =
    let inputJson = "NOT A JSON"
    use inputStream = new StringReader(inputJson)
    use outputStream = new StringWriter()
    Assert.ThrowsAny(fun () -> Cli.run inputStream outputStream) |> ignore
    ()
