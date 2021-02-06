module Counter.Client.Main

open Elmish
open Bolero
open Bolero.Html

type Model =
    {
        page: int
        value: int
        step: int
        key: string
        keyCode: string
    }

let initModel =
    {
        page = 1
        value = 0
        step = 1
        key = ""
        keyCode = ""
    }

type Message =
    | Increment
    | Decrement
    | Reset
    | SetStep of int
    | KeyDown of string * string

type Counter = Template<"wwwroot/counter.html">

let update message model =
    match message with
    | Increment -> { model with value = model.value + model.step }
    | Decrement -> { model with value = model.value - model.step }
    | Reset -> { model with value = 0; step = 1 }
    | SetStep step -> { model with step = step }
    | KeyDown (key, keyCode) -> { model with key = key; keyCode = keyCode }

let view model dispatch =
    Counter()
        .Decrement(fun _ -> dispatch Decrement)
        .Value(string model.value)
        .Increment(fun _ -> dispatch Increment)
        .Reset(fun _ -> dispatch Reset)
        .Step(string model.step, fun inputStep ->
            match System.Int32.TryParse inputStep with
            | true, step -> dispatch (SetStep (step))
            | false, _ -> ())
        .Key(model.key)
        .KeyCode(model.keyCode)
        .KeyDown(fun key -> dispatch (KeyDown (key.Key, key.Code)))
        .Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
