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

type Counter = Template<"wwwroot/counter.html">

let update message model =
    match message with
    | Increment -> { model with value = model.value + 1 }
    | Decrement -> {model with value = model.value - 1 }
    | Reset -> { model with value = 0 }

let view model dispatch =
    Counter()
        .Decrement(fun _ -> dispatch Decrement)
        .Value(string model.value)
        .Increment(fun _ -> dispatch Increment)
        .Reset(fun _ -> dispatch Reset)
        .Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
