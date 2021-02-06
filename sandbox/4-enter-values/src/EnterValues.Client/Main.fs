module EnterValues.Client.Main

open Elmish
open Bolero
open Bolero.Html

type Model =
    {
        enteredValues: string list
    }

let initModel =
    {
        enteredValues = []
    }

type Message =
    | EnterValue of string
    | ClearEnteredValues

type EnterValues = Template<"wwwroot/enterValues.html">

let update message model =
    match message with
    | EnterValue value -> { model with enteredValues = model.enteredValues @ [value] }
    | ClearEnteredValues -> { model with enteredValues = [] }

let view model dispatch =

    EnterValues()
        .EnteredValues(forEach model.enteredValues (fun value -> EnterValues.EnteredValue().Value(value).Elt()))
        .NewValue("", fun value -> dispatch (EnterValue value))
        .ClearEnteredValues(fun _ -> dispatch ClearEnteredValues)
        .Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
