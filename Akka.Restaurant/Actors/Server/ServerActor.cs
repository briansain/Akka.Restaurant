using Akka.Actor;
using Akka.Event;
using Akka.Hosting;
using Akka.Restaurant.Actors.Cooks;
using Akka.Restaurant.Messages;
using Akka.Restaurant.Messages.DrinksWorkflow;
using Akka.Restaurant.Messages.FoodWorkflow;

namespace Akka.Restaurant.Actors.Server
{
    internal class ServerActor : ReceiveActor
    {
        public Dictionary<Guid, TableState> TableStates { get; set; } = new Dictionary<Guid, TableState>();
        private ILoggingAdapter _logger;
        private IRequiredActor<CookManager> _cookManager;
        private List<FoodOrder> _foodOrders = new List<FoodOrder>();
        public string ServerName { get; set; }
        public ServerActor(IRequiredActor<CookManager> cookManager, string serverName)
        {
            ServerName = serverName;
            _cookManager = cookManager;
            _logger = Context.GetLogger();

			Receive<NumAssignedTables>(msg =>
            {
                Sender.Tell(new NumAssignedTablesResponse(TableStates.Count, Self));
            });
            Receive<AssignTable>(msg =>
            {
                _logger.Info($"{ServerName} has been assigned Table {msg.TableId}");
                TableStates.Add(msg.TableId, new TableState(msg.TableId, msg.NewCustomers.CustomerId));
                var custActor = GetCustomerReference(msg.NewCustomers.CustomerId);
                custActor.Tell(new ServerGreeting(ServerName));
                Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromSeconds(3), custActor, new RequestDrinkOrder(), Self);
            });
            Receive<DrinkOrder>(msg =>
            {
                _logger.Info($"Received drink order");
                Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromSeconds(3), Sender, new Drinks(), Self);
                Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromSeconds(6), Sender, new RequestFoodOrder(), Self);
            });
            Receive<FoodOrder>(msg =>
			{
                _logger.Info($"Received food order; Num items: {msg.Order.Count}");
				var customerId = Guid.Parse(Sender.Path.Name.Remove(0, 9));
				msg.CustomerId = customerId;
				_foodOrders.Add(msg);

				_cookManager.ActorRef.Tell(msg);
            });
            Receive<FoodIsCooked>(msg =>
            {
                var order = _foodOrders.Single(f => f.OrderId == msg.OrderId);
                order.CompletlyCookedFoods++;
                if (order.CompletlyCookedFoods == order.Order.Count)
                {
                    _logger.Info($"Delivering food to customers");
                    var customer = GetCustomerReference(order.CustomerId);
                    customer.Tell(new FoodOrderDelivery());
                    _foodOrders.Remove(order);

                    object message = order.IsDessert ? new RequestPayment() : new RequestDessertOrder();

					Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromSeconds(6), customer, message, Self);
				}
			});
            Receive<RequestCheck>(msg =>
            {
                _logger.Info($"Requesting Check");
                Sender.Tell(new RequestPayment());
            });
            Receive<Payment>(msg =>
            {
                _logger.Info($"Received Payment!!");
                Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromSeconds(6), Sender, PoisonPill.Instance, Self);
            });
        }

        public ActorSelection GetCustomerReference(Guid customerId)
        {
            return Context.System.ActorSelection($"/user/customer-{customerId}");
        }
    }

    internal class TableState
    {
        public Guid TableId { get; set; }
        public Guid CustomerId { get; set; }
        public Workflow Workflow { get; set; }
        public FoodOrder FoodOrder { get; set; }

        public TableState(Guid tableId, Guid customerId)
        {
            TableId = tableId;
            CustomerId = customerId;
            Workflow = Workflow.Greet;
            FoodOrder = null;
        }
    }

    internal enum Workflow
    { 
        Greet,
        Drinks,
        FoodOrder,
        FoodDelivery,
        DessertOrder,
        DessertDelivery,
        Check, 
        FullyPaid,
        CleanTable
    }
}

