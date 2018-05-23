open System
open Indexer

[<EntryPoint>]
let main _ =
    let users = FileImporter.getUserValues (__SOURCE_DIRECTORY__ + "/TestData/users.json")
    let channels = FileImporter.getChannels (__SOURCE_DIRECTORY__ + "/TestData/channels.json")
    let someMessages = FileImporter.getMessages (__SOURCE_DIRECTORY__ + "/TestData/general/2017-12-15.json")

    users
    |> Array.iter (fun x -> 
        printfn "| Id: %s | name: %s | displayname: %s | TimeZone: %s |" 
            x.Id x.Name x.DisplayName x.TimeZone)

    printfn "%s%s" "==================" Environment.NewLine
    printfn "%s%s" "==================" Environment.NewLine

    channels
    |> Array.iter (fun x ->
        printfn "| id: %s | name: %s | purpose: %s | topic: %s |" 
            x.Id x.Name x.Purpose.Value x.Topic.Value)

    printfn "%s%s" "==================" Environment.NewLine
    printfn "%s%s" "==================" Environment.NewLine

    someMessages
    |> Array.iter (fun x -> 
        printfn "user: %s, message: %s" x.UserId x.Text)

    let esClient = getClient
    createIndex esClient |> ignore
    index esClient |> ignore
    0
