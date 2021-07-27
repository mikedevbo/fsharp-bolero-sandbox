module Remoting.Contracts

open Bolero.Remoting

type SimpleService =
    {
        GetHello: unit -> Async<string>
    }

    interface IRemoteService with
        member this.BasePath = "/simpleservice"