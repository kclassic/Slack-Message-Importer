open System
open Indexer
open Types

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

 
    let indexMessages = 
        someMessages 
        |> Array.map (fun x ->
            let start = DateTime(1970,1,1,0,0,0,DateTimeKind.Utc) 
            { UserName = 
                (users 
                |> Array.filter (fun y -> y.Id = x.UserId) 
                |> Array.map (fun z -> z.Name) 
                |> Array.head)
              DisplayName = 
                (users 
                |> Array.filter (fun y -> y.Id = x.UserId) 
                |> Array.map (fun z -> z.DisplayName) 
                |> Array.head)

              ChannelName = "general"
              TimeStamp =  start.AddSeconds(float x.TimeStamp)
              Text = x.Text })   

    let client = getClient
    createIndex client |> ignore

    indexMessages
    |> Array.iter (fun x -> do index client x |> ignore)
    
    0
