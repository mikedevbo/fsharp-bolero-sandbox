module ViewComponents.Client.Main

open Elmish
open Bolero
open Bolero.Html
open System

type Model =
    {
        Nickname: string
        Website: string
        Sport: string
        NicknameComp: string
        WebsiteComp: string
        SportComp: string
    }

let initModel =
    {
        Nickname = ""
        Website = ""
        Sport = ""
        NicknameComp = ""
        WebsiteComp = ""
        SportComp = ""
    }

type Message =
    | SetNickname of string
    | SetWebsite of string
    | SetSport of string
    | SetNicknameComp of string
    | SetWebsiteComp of string
    | SetSportComp of string

let update message model =
    match message with
    | SetNickname value -> {model with Nickname = value}
    | SetWebsite value -> {model with Website = value}
    | SetSport value -> {model with Sport = value}
    | SetNicknameComp value -> {model with NicknameComp = value}
    | SetWebsiteComp value -> {model with WebsiteComp = value}
    | SetSportComp value -> {model with SportComp = value}

type ViewComponents = Template<"wwwroot/viewComponents.html">

let NicknameLabel = "Nickname"
let WebsiteLabel = "Website"
let SportLabel = "Sport"

let viewInput (label:string) value setValue =
    ViewComponents.InputValue()
        .Label(label)
        .Value(value, fun vp -> setValue vp)
        .ValueRenderDate(DateTime.Now.ToString("ss.ffff"))
        .Elt()

type InputComponentModel = { label: string; value: string }

type InputComponent() =
    inherit ElmishComponent<InputComponentModel, string>()

    override this.ShouldRender(oldModel, newModel) =
        oldModel.value <> newModel.value

    override this.View model dispatch =
        viewInput model.label model.value dispatch

let view model dispatch =
    ViewComponents()
        .NickNameInput(viewInput NicknameLabel model.Nickname (fun value -> dispatch (SetNickname value)))
        .WebsiteInput(viewInput WebsiteLabel model.Website (fun value -> dispatch (SetWebsite value)))
        .SportInput(viewInput SportLabel model.Sport (fun value -> dispatch (SetSport value)))
        .NickNameInputComp(ecomp<InputComponent, _, _> [] {label = NicknameLabel; value = model.NicknameComp} (fun value -> dispatch (SetNicknameComp value)))
        .WebsiteInputComp(ecomp<InputComponent, _, _> [] {label = WebsiteLabel; value = model.WebsiteComp} (fun value -> dispatch (SetWebsiteComp value)))
        .SportInputComp(ecomp<InputComponent, _, _> [] {label = SportLabel; value = model.SportComp} (fun value -> dispatch (SetSportComp value)))
        .Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
