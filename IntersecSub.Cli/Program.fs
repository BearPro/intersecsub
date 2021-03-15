open System
open IntersecSub.Core
open FSharp.Json


[<EntryPoint>]
let main argv =
    let input = Console.ReadLine()
    let data = Json.deserialize<list<list<int>>> input |> List.map Set.ofList
    match data with
    | [a; b; c] -> 
        let r = intersecSub a b c |> Set.toList |> Json.serialize
        printfn "%s" r
        ()
    | _ -> failwith "Invalid"
    0