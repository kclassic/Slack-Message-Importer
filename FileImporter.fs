module FileImporter
open FSharp.Data
open FSharp.Data.JsonExtensions
open Types

let getUserValues (path : string) = 
    let users = JsonValue.Load(path)
    let userArray = users.AsArray()

    userArray
    |> Array.map (fun x -> 
        { Id = (x?id.AsString())
          Name = (x?name.AsString())
          DisplayName = (x?profile?display_name.AsString())
          TimeZone = (x?tz.AsString()) })

let getChannels (path : string) = 
    let channelJson = JsonValue.Load(path)
    let channels = channelJson.AsArray()

    channels
    |> Array.map (fun x -> 
        { Id = (x?id.AsString()) 
          Name = (x?name.AsString())
          Members = (x?members.AsArray() |> Array.map (fun y -> y.AsString()))
          Topic = { Creator =" "; Value = (x?topic?value.AsString()); LastSet =0.0 }
          Purpose = { Creator = " "; Value = (x?purpose?value.AsString()); LastSet = 0.0 }})

let getMessages (path : string) =
    let messageJson = JsonValue.Load(path)
    let messages = messageJson.AsArray()

    messages
    |> Array.map (fun x ->
        { UserId = (x?user.AsString())
          Text = (x?text.AsString())
          TimeStamp = (x?ts.AsFloat()) })
