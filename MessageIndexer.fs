module Indexer

open Types
open System
open Nest
open Elasticsearch.Net

//settings to connect to local elasticsearch image
let getClient = 
    let node ="http://localhost:9200" |> Uri
    let settings = new ConnectionSettings(node)
    settings
        .MapDefaultTypeIndices(fun x -> x.Add(typeof<MessageIndexItem>, "messages") |> ignore)
        |> Transport<IConnectionSettingsValues>
        |> ElasticClient
        :> IElasticClient

let testMessage = 
    { UserName = "testUserName"
      DisplayName = "testDisplayName"
      ChannelName = "testChannelName"
      TimeStamp = DateTime.Now
      Text = "this is a text message" }

let indexName ( name : string ) = IndexName.op_Implicit name
let messageIndex = indexName "messages"

let createIndex (client : IElasticClient) = 
    client.CreateIndex(messageIndex)


let index (client : IElasticClient) = 
    client.Index<MessageIndexItem>(testMessage)


    


    
