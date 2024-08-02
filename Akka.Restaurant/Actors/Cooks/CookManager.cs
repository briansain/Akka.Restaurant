using Akka.Actor;
using Akka.Event;
using Akka.Restaurant.Messages;
using Akka.Restaurant.Messages.FoodWorkflow;

namespace Akka.Restaurant.Actors.Cooks
{
    internal class CookManager : ReceiveActor
    {
        public List<FoodOrder> Orders = new List<FoodOrder>();
        public ILoggingAdapter _loggerAdapter;
        public CookManager()
        {
            _loggerAdapter = Context.GetLogger();
            for(var i = 0; i < 4; i++)
            {
                var cookId = Guid.NewGuid();
                var child = Context.ActorOf(Props.Create<CookActor>(cookId), $"cook-{cookId}");
                Context.Watch(child);
            }

            Receive<FoodOrder>(msg =>
            { 
                /*Cheeseburger, Lasgna*/
                /*Cook1, Cook2*/
                var tasks = new List<Task<CookStatusResponse>>();
                var senderId = Guid.Parse(Sender.Path.Name.Remove(0, 7));
                foreach(var child in Context.GetChildren())
                {
                    tasks.Add(child.Ask<CookStatusResponse>(new RequestCookStatus()));
                }
                Task.WaitAll(tasks.ToArray());
                foreach(var foodItem in msg.Order)
                {
                    var cook = tasks.OrderBy(t => t.Result.CookStatus).ThenBy(t => t.Result.NumBacklogOrders).FirstOrDefault();
                    if (cook == null)
                    {
                        _loggerAdapter.Error($"Cook Manager couldn't find child cook");
                        throw new Exception();
                    }
                    cook.Result.NumBacklogOrders++;
                    var childCook = Context.Child($"cook-{cook.Result.CookId}");
                    childCook.Tell(new CookTheFood(foodItem, msg.OrderId, senderId, Guid.NewGuid()));       
                }
            });
        }


    }
}
