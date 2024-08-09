using Akka.Actor;
using Akka.Event;
using Akka.Restaurant.Messages;
using Akka.Restaurant.Messages.DrinksWorkflow;
using Akka.Restaurant.Messages.FoodWorkflow;

namespace Akka.Restaurant.Actors
{
    internal class CustomerActor : ReceiveActor
    {
        public int CountOfPeople { get; set; }
        public Guid CustomerId { get; set; }
        public ILoggingAdapter _logger { get; set; }
        public CustomerActor(int countOfPeople, Guid customerId)
        {
            CountOfPeople = countOfPeople;
            CustomerId = customerId;
            _logger = Context.GetLogger();

            Receive<ServerGreeting>(msg =>
            {
                _logger.Info($"Received Server Greeting from {msg.ServerName}");
            });
            Receive<RequestDrinkOrder>(msg =>
            {
                _logger.Info($"RequestDrinkOrder");
                var drinkOrder = ChooseWhatToOrder(msg.DrinkMenu);
                Sender.Tell(new DrinkOrder(drinkOrder));
            });
            Receive<Drinks>(msg =>
            {
                _logger.Info($"Received my/our drinks");
            });
            Receive<RequestFoodOrder>(msg =>
            {
                var foodOrder = ChooseWhatToOrder(msg.Menu);
                Sender.Tell(new FoodOrder(foodOrder));
            });
            Receive<FoodOrderDelivery>(msg =>
            {
                _logger.Info($"Food Order was Delivered");
            });
            Receive<RequestDessertOrder>(msg =>
            {
                var rando = Random.Shared.Next(20);
                if (rando % 2 == 0)
                {
                    _logger.Info($"Ordering Dessert!");
                    var dessertOrder = ChooseWhatToOrder(msg.Menu);
                    Sender.Tell(new FoodOrder(dessertOrder, true));
                }
                else
                {
                    _logger.Info($"I don't like dessert");
                    Sender.Tell(new RequestCheck());
                }
            });
            Receive<RequestPayment>(msg =>
            {
                _logger.Info($"Here's our gold card!");
                Sender.Tell(new Payment());
            });
        }

        public List<string> ChooseWhatToOrder(List<string> menu)
        {
            var order = new List<string>();
            for (var i = 0; i < CountOfPeople; i++)
            {
                var index = Random.Shared.Next(0, CountOfPeople - 1);
                order.Add(menu[index]);
            }
            return order;
        }
    }
}
