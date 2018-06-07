open System
open System.Net.Http
open Indexer
open Types

[<EntryPoint>]
let main argv  =

    let token = 
        argv
        |> Array.head

    printfn "%s" token

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

    let client = new HttpClient()
    
    let messages = SlackImporter.getMessages client token |> Async.RunSynchronously
    
    let client = getClient
    createIndex client |> ignore

    messages 
        |> List.iter (fun message ->
            let start = DateTime(1970,1,1,0,0,0,DateTimeKind.Utc) 
            let user = 
                users
                |> Array.filter (fun users -> users.Id = message.User) 
                |> Array.map (fun users -> users.DisplayName) 
                |> Array.tryHead

            printfn "user = %s" message.User
            printfn "text = %s" message.Text
            { UserName = 
                match user with
                | Some x -> x
                | None -> ""
              DisplayName = 
                match user with
                | Some x -> x
                | None -> ""
              ChannelName = message.ChannelName
              TimeStamp =  start.AddSeconds(message.TimeStamp)
              Text = message.Text }
            |> index client
            |> ignore)   

    indexMessages
    |> Array.iter(fun message -> do index client message |> ignore)

 
    0
