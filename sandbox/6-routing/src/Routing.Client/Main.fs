module Routing.Client.Main

open Elmish
open Bolero
open Bolero.Html

type Page =
    | [<EndPoint "/">] Main
    | [<EndPoint "/counter">] Counter
    | [<EndPoint "/entervalues">] EnterValues
    | [<EndPoint "/viewcomponents">] ViewComponents

type Model =
    {
        page: Page
    }

let initModel =
    {
        page = Main
    }

type Message =
    | SetPage of Page

type MainTemplate = Template<"wwwroot/main.html">
type CounterTemplate = Template<"wwwroot/counter.html">
type EnterValuesTemplate = Template<"wwwroot/entervalues.html">
type ViewComponentsTemplate = Template<"wwwroot/viewcomponents.html">

let router = Router.infer SetPage (fun m -> m.page)

let update message model =
    match message with
    | SetPage p -> { model with page = p }

let view model dispatch =
    let main =
        MainTemplate()
            .MainLink(router.Link Main)
            .CounterLink(router.Link Counter)
            .EnterValuesLink(router.Link EnterValues)
            .ViewComponentsLink(router.Link ViewComponents)

    match model.page with
    | Main ->
        main.Body("").Elt()
    | Counter ->
        main.Body(CounterTemplate().Elt()).Elt()
    | EnterValues ->
        main.Body(EnterValuesTemplate().Elt()).Elt()
    | ViewComponents ->
        main.Body(ViewComponentsTemplate().Elt()).Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
        |> Program.withRouter router
