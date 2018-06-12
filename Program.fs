open System
open System.Net.Http
open Indexer
open System.IO
open System.Text
open System.Security.Cryptography
open Types.JsonTypes
open Types.ElasticTypes

let exitCode = 0

let calculateHash user text ts =
    (user + text + ts) 
    |> Seq.toArray
    |> Encoding.ASCII.GetBytes
    |> (new SHA256Managed()).ComputeHash
    |> Convert.ToBase64String

let mapAndIndexMessage client (users:User list) (message:JMessage)  =
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
    { Id = calculateHash message.User message.Text (message.TimeStamp |> string)
      UserName = 
        match userName with
        | Some x -> x
        | None -> ""
      DisplayName = 
        match displayName with
        | Some x -> x
        | None -> ""
      ChannelName = message.ChannelName
      TimeStamp = DateTimeOffset.FromUnixTimeSeconds(message.TimeStamp |> int64) |> (fun x -> x.DateTime) 
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
        >> List.iter(mapAndIndexMessage client users))
    let httpClient = new HttpClient()
    
    let messages = SlackImporter.getMessages httpClient token |> Async.RunSynchronously

    messages |> List.iter(mapAndIndexMessage client users)

    exitCode
