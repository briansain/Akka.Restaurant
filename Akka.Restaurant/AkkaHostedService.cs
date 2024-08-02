using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Hosting;
using Akka.Restaurant.Actors;
using Akka.Restaurant.Messages;
using Microsoft.Extensions.Hosting;

namespace Akka.Restaurant
{
    internal class AkkaHostedService : IHostedService
    {
        private ActorRegistry _registry;
        private ActorSystem _system;
        
        public AkkaHostedService(ActorSystem actorySystem, ActorRegistry actorRegistry)
        {
            _registry = actorRegistry;
            _system = actorySystem;
        }
        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            var hostessActor = _registry.Get<HostessActor>();
            var di = DependencyResolver.For(_system);
            for (var i = 0; i < 2; i++)
            {
                var customerId = Guid.NewGuid();
                var numOfCustomers = 3;
                _system.ActorOf(Props.Create<CustomerActor>(numOfCustomers, customerId), $"customer-{customerId}");

                var newCustomerMessage = new NewCustomers()
                {
                    NumberOfCustomers = numOfCustomers,
                    CustomerId = customerId
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
