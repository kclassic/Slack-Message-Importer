module Types

open System

type User = {
    Id          : string
    Name        : string
    DisplayName : string
    TimeZone    : TimeZoneInfo
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
    Members : User[]
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