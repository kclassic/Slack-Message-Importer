module Types

open System
open Newtonsoft.Json
open Nest

module JsonTypes =
    type Profile = {
        [<JsonProperty("display_name")>]
        DisplayName : string
    }
    type User = {
        [<JsonProperty("id")>]
        Id          : string
        [<JsonProperty("name")>]
        Name        : string
        [<JsonProperty("profile")>]
        Profile : Profile
        [<JsonProperty("tz")>]
        TimeZone    : string
    }

    type Topic = {
        [<JsonProperty("value")>]
        Value   : string
        [<JsonProperty("creator")>]
        Creator : string
        [<JsonProperty("last_set")>]
        LastSet : float
    }

    type Purpose = {
        [<JsonProperty("value")>]
        Value   : string
        [<JsonProperty("creator")>]
        Creator : string
        [<JsonProperty("last_set")>]
        LastSet : float
    }

    type JChannel = {
        [<JsonProperty("id")>]
        Id             : string
        [<JsonProperty("name")>]
        Name           : string
        [<JsonProperty("is_channel")>]
        IsChannel      : bool
        [<JsonProperty("created")>]
        Created        : int
        [<JsonProperty("is_archived")>]
        IsArchived     : bool
        [<JsonProperty("is_general")>]
        IsGeneral      : bool
        [<JsonProperty("unlinked")>]
        Unlinked       : int
        [<JsonProperty("creator")>]
        Creator        : string
        [<JsonProperty("name_normalized")>]
        NameNormalized : string
        [<JsonProperty("is_shared")>]
        IsShared       : bool
        [<JsonProperty("is_org_shared")>]
        IsOrgShared    : bool
        [<JsonProperty("is_member")>]
        IsMember       : bool
        [<JsonProperty("is_private")>]
        IsPrivate      : bool
        [<JsonProperty("is_mpim")>]
        IsMpim         : bool
        [<JsonProperty("members")>]
        Members        : string list
        [<JsonProperty("topic")>]
        Topic          : Topic
        [<JsonProperty("purpose")>]
        Purpose        : Purpose
        [<JsonProperty("previous_names")>]
        PreviousName   : string list
        [<JsonProperty("num_members")>]
        NumMembers     : int
    }

    type Edited = {
        [<JsonProperty("user")>]
        User: string
        [<JsonProperty("ts")>]
        TimeStamp : float
    }

    type JMessage = {
        [<JsonProperty("type")>]
        Type : string
        [<JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)>]
        User : string
        [<JsonProperty("text")>]
        Text : string
        [<JsonProperty("ts")>]
        TimeStamp   : float
        [<JsonProperty("edited", NullValueHandling = NullValueHandling.Ignore)>]
        Edited  : Edited
        [<JsonProperty("client_msg_id", NullValueHandling = NullValueHandling.Ignore)>]
        ClientMsgId : string
        [<JsonIgnore>]
        ChannelName : string
    }

    type Cursor = {
        [<JsonProperty("next_cursor")>]
        NextCursor : string
    }

    type JMessages = {
        [<JsonProperty("ok")>]
        Ok               : bool
        [<JsonProperty("messages")>]
        Messages         : JMessage list 
        [<JsonProperty("has_more")>]
        HasMore          : bool
        [<JsonProperty("is_limited")>]
        IsLimited        : bool
        [<JsonProperty("pin_count")>]
        PinCount         : int
        [<JsonProperty("response_metadata")>]
        ResponseMetadata : Cursor
        ChannelName : string
    }

    type ChannelResponse = {
        [<JsonProperty("ok")>]
        Ok       : bool
        [<JsonProperty("channels")>]    
        Channels : JChannel list

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
        TimeStamp : float
    }

module ElasticTypes =
    [<ElasticsearchType(Name = "messages")>]
    type MessageIndexItem = {
        [<Id("_id")>]
        Id : string

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