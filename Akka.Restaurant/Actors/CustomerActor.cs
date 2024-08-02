using Akka.Actor;
using Akka.Restaurant.Messages;

namespace Akka.Restaurant.Actors
{
    internal class CustomerActor : ReceiveActor
    {
        public int CountOfPeople { get; set; }
        public Guid CustomerId { get; set; }
        public CustomerActor(int countOfPeople, Guid customerId)
        {
            CountOfPeople = countOfPeople;
            CustomerId = customerId;

            Receive<ServerGreeting>(msg =>
            {

            });
        }
    }
}
