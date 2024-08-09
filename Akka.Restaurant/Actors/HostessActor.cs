using Akka.Actor;
using Akka.Event;
using Akka.Hosting;
using Akka.Restaurant.Actors.Server;
using Akka.Restaurant.Messages;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Restaurant.Actors
{
    internal class HostessActor : ReceiveActor
    {
        public List<Table> Tables { get; set; }
        public IRequiredActor<ServerManager> ServerManager { get; set; }
        private ILoggingAdapter _logger;
        public HostessActor(IRequiredActor<ServerManager> serverManager)
        {
            Receive<NewCustomers>(msg =>
            {
                _logger.Debug($"Receive NewCustomers Message; Num of Customers:{msg.NumberOfCustomers}");
                var availableTable = Tables!.First(t => t.AvailableSeats >= msg.NumberOfCustomers && !t.HasCustomers);
                _logger.Debug($"Assigned customers to table {availableTable.TableId}");
                ServerManager!.ActorRef.Tell(new AssignTable(msg, availableTable.TableId));
                availableTable.HasCustomers = true;
            });

            ServerManager = serverManager;
            Tables = Table.GetTables();
            _logger = Context.GetLogger();
        }
    }
}
