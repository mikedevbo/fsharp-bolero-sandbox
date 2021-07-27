namespace Remoting.Server

open Bolero.Remoting.Server
open Microsoft.AspNetCore.Hosting
open Remoting.Contracts
open Microsoft.Extensions.Primitives

type SimpleServiceHandler(ctx: IRemoteContext, env: IWebHostEnvironment) =
    inherit RemoteHandler<SimpleService>()
    do ctx.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", StringValues("*"))

    override this.Handler =
        {
            GetHello = fun _ -> async {
                // ctx.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", new StringValues("*"))
                return "Hello from Simple Service!"
            }
        }