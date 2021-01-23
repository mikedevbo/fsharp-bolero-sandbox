module Counter.Client.Main

open Elmish
open Bolero
open Bolero.Html

type Model =
    {
        value: int
    }

let initModel =
    {
        value = 0
    }

type Message =
    | Increment
    | Decrement
    | Reset

let update message model =
    match message with
    | Increment -> { model with value = model.value + 1 }
    | Decrement -> {model with value = model.value - 1 }
    | Reset -> { model with value = 0 }

let view model dispatch =
    concat [
        div [] [
            button [on.click (fun _ -> dispatch Decrement)] [text "-"]
            text (string model.value)
            button [on.click (fun _ -> dispatch Increment)] [text "+"]
        ]

        div [] [
            button [on.click (fun _ -> dispatch Reset)] [text "Reset"]
        ]
    ]

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
