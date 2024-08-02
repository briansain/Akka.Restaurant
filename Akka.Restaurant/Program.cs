using Akka.Actor;
using Akka.Hosting;
using Akka.Logger.Serilog;
using Akka.Restaurant.Actors;
using Akka.Restaurant.Actors.Cooks;
using Akka.Restaurant.Actors.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Akka.Restaurant
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .WriteTo.Console())
                .ConfigureServices(services =>
                {
                    services.AddAkka("restaurant-service", builder =>
                    {
                        builder.ConfigureLoggers(configLoggers =>
                        {
                            configLoggers.LogLevel = Event.LogLevel.DebugLevel;
                            configLoggers.LogConfigOnStart = true;
                            configLoggers.ClearLoggers();
                            configLoggers.AddLogger<SerilogLogger>();
                        })
                        .WithActors((actorSystem, registry, dependencyInj) =>
                        {
                            var serverManager = actorSystem.ActorOf(dependencyInj.Props<ServerManager>(), "server-manager");
                            registry.Register<ServerManager>(serverManager);
                            var hostessActor = actorSystem.ActorOf(dependencyInj.Props<HostessActor>(), "hostess-actor");
                            registry.Register<HostessActor>(hostessActor);
                            var cookManager = actorSystem.ActorOf(dependencyInj.Props<CookManager>(), "cook-manager");
                            registry.Register<CookManager>(cookManager);
                        });
                    });
                    services.AddHostedService<AkkaHostedService>();
                }).Build().Run();
        }
    }
}
