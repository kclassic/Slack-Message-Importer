module FileImporter
open Types
open Newtonsoft.Json
open System.IO

let desereliazeFromFile<'a> path = 
    path 
    |> File.ReadAllText 
    |> JsonConvert.DeserializeObject<'a>

let getChannelName path =
    path 
    |> Path.GetDirectoryName 
    |> (fun x -> x.Split(Path.DirectorySeparatorChar)) 
    |> (fun x -> x.[x.Length - 1])
    
let getUserValues (path : string) = 
    desereliazeFromFile<User list> path

let getMessages (path : string) =
    let messages = desereliazeFromFile<JMessage list> path
    let channelName = getChannelName path

    messages
    |> List.map(fun x ->
        { ChannelName = channelName
          Type = x.Type
          User = x.User
          Text = x.Text
          TimeStamp = x.TimeStamp
          Edited = x.Edited
          ClientMsgId = x.ClientMsgId})