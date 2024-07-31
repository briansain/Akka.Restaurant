using Akka.Actor;
using Akka.Hosting;
using Akka.Restaurant.Actors;
using Microsoft.Extensions.Hosting;

namespace Akka.Restaurant
{
    internal class AkkaHostedService : IHostedService
    {
        private ActorRegistry _registry;
        
        public AkkaHostedService(ActorRegistry actorRegistry)
        {
            _registry = actorRegistry;
        }
        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            var echoActor = _registry.Get<EchoActor>();
            while(true)
            {
                var response = echoActor.Ask("Hello World");
                echoActor.Tell(42);
                echoActor.Ask(42.50);
                Thread.Sleep(3000);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
