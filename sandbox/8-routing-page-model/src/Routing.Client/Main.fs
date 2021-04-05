module Routing.Client.Main

open Elmish
open Bolero
open Bolero.Html

type CounterModel =
    {
        value: int
    }

type EnterValuesModel =
    {
        value: string
    }

type ViewComponentsModel =
    {
        value: string
    }

type Page =
    | [<EndPoint "/">] Main
    | [<EndPoint "/counter">] Counter of PageModel<CounterModel>
    | [<EndPoint "/entervalues">] EnterValues of PageModel<EnterValuesModel>
    | [<EndPoint "/viewcomponents">] ViewComponents of PageModel<ViewComponentsModel>

type Model =
    {
        page: Page
    }

let initModel =
    {
        page = Main
    }

let defaultModel = function
    | Main -> ()
    | Counter model -> Router.definePageModel model { value = 0 }
    | EnterValues model -> Router.definePageModel model { value = "" }
    | ViewComponents model -> Router.definePageModel model { value ="" }

type Message =
    | SetPage of Page
    | Increment
    | Decrement
    | EnterValue of string
    | EnterViewComponentValue of string

type MainTemplate = Template<"wwwroot/main.html">
type CounterTemplate = Template<"wwwroot/counter.html">
type EnterValuesTemplate = Template<"wwwroot/entervalues.html">
type ViewComponentsTemplate = Template<"wwwroot/viewcomponents.html">

type InputComponent() =
    inherit ElmishComponent<string, string>()

    override this.ShouldRender(oldModel, newModel) =
        oldModel <> newModel

    override this.View model dispatch =
        ViewComponentsTemplate()
            .ViewComponentValue(model, fun value -> dispatch value)
            .Elt()
let router = Router.inferWithModel SetPage (fun m -> m.page) defaultModel

let update message model =
    match message with
    | SetPage p -> { model with page = p }
    | Increment ->
        match model.page with
        | Counter pageModel -> { model with page = Counter { Model = { pageModel.Model with value = pageModel.Model.value + 1 } } }
        | _ -> model
    | Decrement ->
        match model.page with
        | Counter pageModel -> { model with page = Counter { Model = { pageModel.Model with value = pageModel.Model.value - 1 } } }
        | _ -> model
    | EnterValue value ->
        match model.page with
        | EnterValues pageModel -> { model with page = EnterValues { Model = { pageModel.Model with value = value } } }
        | _ -> model
    | EnterViewComponentValue value ->
        match model.page with
        | ViewComponents pageModel -> { model with page = ViewComponents { Model = { pageModel.Model with value = value } } }
        | _ -> model

let view model dispatch =
    let main =
        MainTemplate()
            .MainLink(router.Link Main)
            .CounterLink(router.Link (Counter Router.noModel))
            .EnterValuesLink(router.Link (EnterValues Router.noModel))
            .ViewComponentsLink(router.Link (ViewComponents Router.noModel))

    let counter (pageModel: CounterModel) =
        CounterTemplate()
            .Value(string pageModel.value)
            .Increment(fun _ -> dispatch Increment)
            .Decrement(fun _ -> dispatch Decrement)

    let enterValues (pageModel: EnterValuesModel) =
        EnterValuesTemplate()
            .EnterValue(pageModel.value, fun value -> dispatch (EnterValue value))

    let viewComponents (pageModel: ViewComponentsModel) =
        ecomp<InputComponent, _, _> [] pageModel.value (fun value -> dispatch (EnterViewComponentValue value))

    match model.page with
    | Main ->
        main.Body("").Elt()
    | Counter pageModel ->
        main.Body((counter pageModel.Model).Elt()).Elt()
    | EnterValues pageModel ->
        main.Body((enterValues pageModel.Model).Elt()).Elt()
    | ViewComponents pageModel->
        main.Body((viewComponents pageModel.Model)).Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
        |> Program.withRouter router
