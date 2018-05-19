module Types

open System

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

type Message = {
    UserId    : string
    Text      : string
    TimeStamp : string
}