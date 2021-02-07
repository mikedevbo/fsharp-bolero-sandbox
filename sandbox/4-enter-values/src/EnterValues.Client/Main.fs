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
    | NewValue of string
    | ClearValues

type EnterValuesTemplate = Template<"wwwroot/enterValues.html">

let update message model =
    match message with
    | NewValue value -> { model with enteredValues = model.enteredValues @ [value] }
    | ClearValues -> { model with enteredValues = [] }

let view model dispatch =
    EnterValuesTemplate()
        .EnteredValues(forEach model.enteredValues (fun value -> EnterValuesTemplate.ShowValue().Value(value).Elt()))
        .NewValue("", fun value -> dispatch (NewValue value))
        .ClearValues(fun _ -> dispatch ClearValues)
        .Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
