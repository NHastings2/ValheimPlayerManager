using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ValheimPlayerManager.Web_Server;
using ValheimPlayerManager.Web_Server.Endpoints;

//Create new service host to handle application
var host = new HostBuilder()
    .ConfigureServices((hostContext, services) => services.AddHostedService<ValheimPlayerService>())
    .UseConsoleLifetime()
    .Build();

//Run service
await host.RunAsync();

/// <summary>
/// Valheim Service to handle player connections
/// </summary>
class ValheimPlayerService : IHostedService
{
    private readonly IHostApplicationLifetime hostApplicationLifetime;

    private WebServer webServer;

    /// <summary>
    /// Create new service handler
    /// </summary>
    /// <param name="applicationLifetime"></param>
    public ValheimPlayerService(IHostApplicationLifetime applicationLifetime)
    {
        hostApplicationLifetime = applicationLifetime;
        //Create new Web Server
        webServer = new WebServer("http://+:80/");
        //Add Server Endpoints
        webServer.AddEndpoint(new Joined());
        webServer.AddEndpoint(new Disconnected());
    }

    /// <summary>
    /// Start Service
    /// </summary>
    /// <returns>Status of task</returns>
    public Task StartAsync(CancellationToken _)
    {
        //Check if enviornment variable is set
        if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("STEAM_APIKEY")))
            //If not, throw error
            return Task.FromException(new Exception("STEAM_APIKEY NOT SET"));

        //Check if enviornment variable is set
        if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("DISCORD_WEBHOOK")))
            //If not, throw error
            return Task.FromException(new Exception("DISCORD_WEBHOOK NOT SET"));

        //Start Web Server
        webServer.Start();
        //Send message to log
        Console.WriteLine("Started Service");
        //Return completed start
        return Task.CompletedTask;
    }

    /// <summary>
    /// Stop Service
    /// </summary>
    /// <returns>Status of task</returns>
    public Task StopAsync(CancellationToken _)
    {
        //Stop Web Server
        webServer.Stop();
        //Write message to log
        Console.WriteLine("Stopped Service");
        //Return completed stop
        return Task.CompletedTask;
    }
}