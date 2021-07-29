namespace Remoting.Client.Client

open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Microsoft.Extensions.DependencyInjection
open System
open System.Net.Http
open Bolero.Remoting.Client

module Program =
    
    [<EntryPoint>]
    let Main args =
        let builder = WebAssemblyHostBuilder.CreateDefault(args)
        builder.RootComponents.Add<Main.MyApp>("#main")
        builder.Services.AddScoped<HttpClient>(fun _ ->
            new HttpClient(BaseAddress = Uri builder.HostEnvironment.BaseAddress)) |> ignore
        builder.Services.AddRemoting((fun httpClient -> httpClient.BaseAddress <- Uri("http://localhost:1234"))) |> ignore
        builder.Build().RunAsync() |> ignore
        0
