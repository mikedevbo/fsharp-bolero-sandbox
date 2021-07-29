module Remoting.Client.Client.Main

open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Remoting.Contracts

type Model =
    {
        Text: string
    }

let initModel =
    {
        Text = ""
    }, Cmd.none

type Message =
    | GetHello
    | ShowHello of string
    | ShowError of exn    

let update simpleService message model =
    match message with
    | GetHello -> model, Cmd.OfAsync.either simpleService.GetHello () ShowHello ShowError
    | ShowHello text -> {model with Text = text}, Cmd.none
    | ShowError exn -> {model with Text = exn.Message}, Cmd.none


let view model dispatch =
    concat [
        div [] [
            button [on.click (fun _ -> dispatch GetHello)] [text "GetHello"]
        ]

        div [] [
            text model.Text
        ]
    ]    

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        let simpleService = this.Remote<SimpleService>()
        Program.mkProgram (fun _ -> initModel) (update simpleService) view
