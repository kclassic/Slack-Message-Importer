module SlackImporter

open System
open System.Net.Http
open Types
open Newtonsoft.Json


let deserialize<'a> x = JsonConvert.DeserializeObject<'a> x

let listChannels (client : HttpClient ) token = async {
    let url = "https://slack.com/api/channels.list?token=" + token
    let response = client.GetAsync(url |> Uri)
    response.Result.EnsureSuccessStatusCode() |> ignore
    let! channelsJson = response.Result.Content.ReadAsStringAsync() |> Async.AwaitTask
    let result = deserialize<ChannelResponse> channelsJson

    return result
}

let rec private getMessagesForChannel (client : HttpClient) token channelId channelName (cursor : string) (results : JMessage list) = async {
    let url = "https://slack.com/api/conversations.history?token="+ token + "&channel=" + channelId + "&cursor=" + cursor
    printfn "%s" url
    let response = client.GetAsync(url |> Uri)
    response.Result.EnsureSuccessStatusCode() |> ignore
    let! messagesJson = response.Result.Content.ReadAsStringAsync() |> Async.AwaitTask
    let result = deserialize<JMessages> messagesJson
    let messages =
        result.Messages
        |> List.map(fun x ->
            { ChannelName = channelName
              Type = x.Type
              User = x.User
              Text = x.Text
              TimeStamp = x.TimeStamp
              Edited = x.Edited
              ClientMsgId = x.ClientMsgId})

    if result.HasMore
    then
        return! 
            messages
            |> List.append results
            |> getMessagesForChannel client token channelId channelName result.ResponseMetadata.NextCursor
    else return results |> List.append messages
}

let getMessages ( client : HttpClient ) token = async {
    let! channels = listChannels client token
    let results = 
        List.collect (fun x -> (getMessagesForChannel client token x.Id x.Name "" [] |> Async.RunSynchronously)) channels.Channels
        
    return results
}
