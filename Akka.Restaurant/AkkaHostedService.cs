using Akka.Actor;
using Akka.Hosting;
using Akka.Restaurant.Actors;
using Akka.Restaurant.Messages;
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
            var hostessActor = _registry.Get<HostessActor>();
            for(var i = 0; i < 2; i++)
            { 
                var newCustomerMessage = new NewCustomers()
                {
                    NumberOfCustomers = 3
                };
                hostessActor.Tell(newCustomerMessage);

                Thread.Sleep(3000);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
