module Types

open System
open Nest

type User = {
    Id          : string
    Name        : string
    DisplayName : string
    TimeZone    : string
}

type Topic = {
    Value : string
}

type Purpose = {
    Value : string
}

type Channel = {
    Id      : string
    Name    : string
    Members : string[]
    Topic   : Topic
    Purpose : Purpose
}

type SlackMessage = {
    User      : User
    Text      : string
    TimeStamp : DateTime
    Type      : string
    Purpose   : string option
    Inviter   : User option
    SubType   : string option
}

[<ElasticsearchType(Name = "messages")>]
type MessageIndexItem = {
    [<Text(Name = "user_name")>]
    UserName : string
    
    [<Text(Name = "display_name")>]
    DisplayName : string
    
    [<Text(Name = "channel")>]
    ChannelName: string
    
    [<Date(Name = "timestamp")>]
    TimeStamp : DateTime
    
    [<Text(Name = "text")>]
    Text : string
}

type Message = {
    UserId    : string
    Text      : string
    TimeStamp : float
}
