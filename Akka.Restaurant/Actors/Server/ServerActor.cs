using Akka.Actor;
using Akka.Event;
using Akka.Restaurant.Messages;

namespace Akka.Restaurant.Actors.Server
{
    internal class ServerActor : ReceiveActor
    {
        public Dictionary<Guid, TableState> TableIds { get; set; } = new Dictionary<Guid, TableState>();
        private ILoggingAdapter _logger;
        public string ServerName { get; set; }
        public ServerActor(string serverName)
        {
            ServerName = serverName;
            Receive<NumAssignedTables>(msg =>
            {
                Sender.Tell(new NumAssignedTablesResponse(TableIds.Count, Self));
            });
            Receive<AssignTable>(msg =>
            {
                _logger.Info($"{ServerName} has been assigned Table {msg.TableId}");
                TableIds.Add(msg.TableId, new TableState(msg.TableId, msg.NewCustomers.CustomerId));
                var custActor = GetCustomerReference(msg.NewCustomers.CustomerId);
                custActor.Tell(new ServerGreeting(ServerName));
                Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromSeconds(3), custActor, new RequestDrinkOrder(), Self);
            });
            Receive<DrinkOrder>(msg =>
            {
                Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromSeconds(3), Sender, new Drinks(), Self);
            });
            _logger = Context.GetLogger();
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

        public TableState(Guid tableId, Guid customerId)
        {
            TableId = tableId;
            CustomerId = customerId;
            Workflow = Workflow.Greet;
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
        Receipt, 
        FullyPaid,
        CleanTable
    }
}

