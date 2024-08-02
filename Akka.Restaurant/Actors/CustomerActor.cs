using Akka.Actor;
using Akka.Event;
using Akka.Restaurant.Messages;

namespace Akka.Restaurant.Actors
{
    internal class CustomerActor : ReceiveActor
    {
        public int CountOfPeople { get; set; }
        public Guid CustomerId { get; set; }
        public ILoggingAdapter _logger { get;set; }
        public CustomerActor(int countOfPeople, Guid customerId)
        {
            CountOfPeople = countOfPeople;
            CustomerId = customerId;
            _logger = Context.GetLogger();

            Receive<ServerGreeting>(msg =>
            {
                _logger.Info($"Received Server Greeting from {msg.ServerName}");
                // Do nothing??
            });
            Receive<RequestDrinkOrder>(msg =>
            {
                var drinkOrder = new List<string>();
                for (var i = 0; i < CountOfPeople; i++)
                {
                    var index = Random.Shared.Next(0, CountOfPeople - 1);
                    drinkOrder.Add(msg.AvailableDrinks[index]);
                }
                Sender.Tell(new DrinkOrder(drinkOrder));
            });
            Receive<Drinks>(msg =>
            {
                _logger.Info($"Received my/our drinks");
                // Do nothing??
            });
        }
    }
}
