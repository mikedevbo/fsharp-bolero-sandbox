module SimpleComposition.Client.Main

open Elmish
open Bolero
open Bolero.Html

type Model =
    {
        page: int
    }

let initModel =
    {
        page = 1
    }

type Message =
    | ShowCounter
    | ShowEnterValues
    | ShowViewComponents

type Main = Template<"wwwroot/main.html">
type Counter = Template<"wwwroot/counter.html">
type EnterValues = Template<"wwwroot/enterValues.html">
type ViewComponents = Template<"wwwroot/viewcomponents.html">

let update message model =
    match message with
    | ShowCounter -> { model with page = 1 }
    | ShowEnterValues -> { model with page = 2 }
    | ShowViewComponents -> { model with page = 3 }

let view model dispatch =

    let page =
        match model.page with
        | 1 ->
            Counter()
                .Elt()
        | 2 ->
            EnterValues()
                .Elt()
        | _ ->
            ViewComponents()
                .Elt()

    Main()
        .ShowCounter(fun _ -> dispatch ShowCounter)
        .ShowEnterValues(fun _ -> dispatch ShowEnterValues)
        .ShowViewComponents(fun _ -> dispatch ShowViewComponents)
        .Body(page)
        .Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
