open System
open System.Net.Http
open Indexer
open Types
open System.IO

let exitCode = 0

let mapAndIndexMessage client (users:User list) (message:JMessage)  =
    let start = DateTime(1970,1,1,0,0,0,DateTimeKind.Utc) 
    let displayName = 
        users
        |> List.filter (fun user -> user.Id = message.User) 
        |> List.map (fun user -> user.Profile.DisplayName) 
        |> List.tryHead

    let userName = 
        users
        |> List.filter (fun user -> user.Id = message.User) 
        |> List.map (fun user -> user.Name) 
        |> List.tryHead

    printfn "user = %s" message.User
    printfn "text = %s" message.Text
    { UserName = 
        match userName with
        | Some x -> x
        | None -> ""
      DisplayName = 
        match displayName with
        | Some x -> x
        | None -> ""
      ChannelName = message.ChannelName
      TimeStamp =  start.AddSeconds(message.TimeStamp)
      Text = message.Text }
    |> index client
    |> ignore

[<EntryPoint>]
let main argv  =

    let token = 
        argv
        |> Array.head

    let files =  
        DirectoryInfo(__SOURCE_DIRECTORY__ + "/TestData/").GetDirectories "*" 
        |> Array.collect (fun x -> Directory.EnumerateFiles(x.FullName) |> Seq.toArray)
        
    files
    |> Array.iter(fun x -> printfn "%s" x)

    let users = FileImporter.getUserValues (__SOURCE_DIRECTORY__ + "/TestData/users.json")

    let client = getClient
    createIndex client |> ignore

    files 
    |> Array.iter(FileImporter.getMessages 
        >> List.iter (fun message -> do mapAndIndexMessage client users message))

    let httpClient = new HttpClient()
    
    let messages = SlackImporter.getMessages httpClient token |> Async.RunSynchronously

    messages |> List.iter (fun message -> do mapAndIndexMessage client users message)   

    exitCode
