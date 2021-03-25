module IntersecSub.Cli

open System
open IntersecSub.Core
open FSharp.Json

let run (inputStream: IO.TextReader) (outputStream: IO.TextWriter) =
    let input = inputStream.ReadToEnd()
    let data = Json.deserialize<list<list<int>>> input |> List.map Set.ofList
    match data with
    | [a; b; c] -> 
        let r = intersecSub a b c |> Set.toList |> Json.serializeU
        outputStream.WriteLine(r)
    | _ -> failwith "Invalid"

[<EntryPoint>]
let main argv =
    run Console.In Console.Out
    0