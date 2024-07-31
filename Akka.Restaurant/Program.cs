using Akka.Actor;
using Akka.Hosting;
using Akka.Logger.Serilog;
using Akka.Restaurant.Actors;
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
                        .WithActors((actorSystem, registry) =>
                        {
                            var echoActor = actorSystem.ActorOf(Props.Create<EchoActor>(), "echo-actor");
                            registry.Register<EchoActor>(echoActor);
                        });
                    });
                    services.AddHostedService<AkkaHostedService>();
                }).Build().Run();
        }
    }
}
