using Akka.Actor;
using Akka.Event;
using Akka.Restaurant.Messages;

namespace Akka.Restaurant.Actors.Server
{
    internal class ServerActor : ReceiveActor
    {
        public List<Guid> TableIds { get; set; } = new List<Guid>();
        private ILoggingAdapter _logger;
        public ServerActor()
        {
            Receive<NumAssignedTables>(msg =>
            {
                Sender.Tell(new NumAssignedTablesResponse(TableIds.Count, Self));
            });
            Receive<AssignTable>(msg =>
            {
                _logger.Debug($"Assigned Table {msg.TableId}");
                TableIds.Add(msg.TableId);
            });
            _logger = Context.GetLogger();
        }
    }
}
