#r "nuget: EmbedIO, 3.4.3"

using EmbedIO;
using EmbedIO.WebApi;
using EmbedIO.Actions;
using EmbedIO.Files;

WebServer CreateWebServer(string url)
{
    var server = new WebServer(o => o
            .WithUrlPrefix(url)
            .WithMode(HttpListenerMode.EmbedIO))
        .WithLocalSessionManager()
        .WithStaticFolder("/static/", "./webroot", true, m => m.WithContentCaching(true)) // Add static files after other modules to avoid conflicts
        .WithModule(new ActionModule("/api", HttpVerbs.Get, HandleApi))
        .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendDataAsync(new { Message = "Hello!" })))
        ;
    return server;
}

Task HandleApi(IHttpContext ctx)
{
    return ctx.SendDataAsync("api");
}

var url = "http://*:5000/";
var server = CreateWebServer(url);
server.RunAsync();
Console.ReadKey();
