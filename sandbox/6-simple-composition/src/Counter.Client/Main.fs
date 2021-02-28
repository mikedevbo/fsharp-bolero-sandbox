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
        enteredValues: string list
    }

let initModel =
    {
        page = 1
        value = 0
        step = 1
        key = ""
        keyCode = ""
        enteredValues = []
    }

type Message =
    | Increment
    | Decrement
    | Reset
    | SetStep of int
    | KeyDown of string * string
    | EnterValue of string
    | ClearEnteredValues
    | ShowCounter
    | ShowEnterValues

type Main = Template<"wwwroot/main.html">
type Counter = Template<"wwwroot/counter.html">
type EnterValues = Template<"wwwroot/enterValues.html">

let update message model =
    match message with
    | Increment -> { model with value = model.value + model.step }
    | Decrement -> { model with value = model.value - model.step }
    | Reset -> { model with value = 0; step = 1 }
    | SetStep step -> { model with step = int step }
    | KeyDown (key, keyCode) -> { model with key = key; keyCode = keyCode }
    | EnterValue value -> { model with enteredValues = model.enteredValues @ [value] }
    | ClearEnteredValues -> { model with enteredValues = [] }
    | ShowCounter -> { model with page = 1 }
    | ShowEnterValues -> { model with page = 2 }

let view model dispatch =

    let page =
        match model.page with
        | 1 ->
            Counter()
                .Step(string model.step, fun step -> dispatch (SetStep (int step)))
                .Key(model.key)
                .KeyCode(model.keyCode)
                .KeyDown(fun key -> dispatch (KeyDown (key.Key, key.Code)))
                .Decrement(fun _ -> dispatch Decrement)
                .Value(string model.value)
                .Increment(fun _ -> dispatch Increment)
                .Reset(fun _ -> dispatch Reset)
                .Elt()
        | _ ->
            EnterValues()
                .EnterdValues(forEach model.enteredValues (fun value -> EnterValues.EnteredValue().Value(value).Elt()))
                .NewValue("", fun value -> dispatch (EnterValue value))
                .ClearEnteredValues(fun _ -> dispatch ClearEnteredValues)
                .Elt()

    Main()
        .ShowCounter(fun _ -> dispatch ShowCounter)
        .ShowEnterValues(fun _ -> dispatch ShowEnterValues)
        .Body(page)
        .Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
