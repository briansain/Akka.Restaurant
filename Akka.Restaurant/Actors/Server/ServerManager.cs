#pragma warning disable AK1003 // ReceiveAsync<T>() or ReceiveAnyAsync<T>() message handler without async lambda body

using Akka.Actor;
using Akka.Dispatch.SysMsg;
using Akka.Event;
using Akka.Restaurant.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Restaurant.Actors.Server
{
    internal class ServerManager : ReceiveActor
    {
        private ILoggingAdapter _logger;
        private List<string> serverNames =
        [
"Lex, Judgment Overseer",
"Hallvardur, the Keep Cerberus",
"Feofilakt, Existence Cerberus",
"Manauia, Adventure Overseer",
"Alessandri, Prospect Cerberus",
"Wimund, Genesis Steward",
"the Stand Sentinel",
"Final Creature Defender",
"Utopia Cerberus"
        ];
        public ServerManager()
        {
            var server1 = Context.ActorOf(Props.Create<ServerActor>(GetServerName()), "server-1"); //{[akka://restaurant-service/user/server-manager/server-1#337002186]}
            Context.Watch(server1);
            var server2 = Context.ActorOf(Props.Create<ServerActor>(GetServerName()), "server-2");
            Context.Watch(server2);

            _logger = Context.GetLogger();

            Receive<AssignTable>(msg =>
            {
                var askTasks = new List<Task<NumAssignedTablesResponse>>();
                foreach (var child in Context.GetChildren())
                {
                    askTasks.Add(child.Ask<NumAssignedTablesResponse>(new NumAssignedTables()));
                }

                Task.WaitAll(askTasks.ToArray());
                var assignedActor = askTasks.OrderBy(a => a.Result.NumAssignedTables).FirstOrDefault();
                _logger.Debug($"Assigned table {msg.TableId}");
                if (assignedActor == null)
                {
                    Context.GetLogger().Error($"Could not get any assigned actors");
                    throw new Exception();
                }

                assignedActor.Result.Self.Tell(msg);
            });
        }

        public string GetServerName()
        {
            var index = Random.Shared.Next(0, serverNames.Count);
            return serverNames[index];
        }

        /*
         * 
Lex, Judgment Overseer
Hallvardur, the Keep Cerberus
Feofilakt, Existence Cerberus
Manauia, Adventure Overseer
Alessandri, Prospect Cerberus
Wimund, Genesis Steward
the Stand Sentinel
Final Creature Defender
Utopia Cerberus
The Keep Watcher
*/

        
    }
}

#pragma warning restore AK1003 // ReceiveAsync<T>() or ReceiveAnyAsync<T>() message handler without async lambda body

