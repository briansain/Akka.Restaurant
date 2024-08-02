#pragma warning disable AK1004 // ScheduleTellOnce() and ScheduleTellRepeatedly() can cause memory leak if not properly canceled

using Akka.Actor;
using Akka.Event;
using Akka.Restaurant.Messages;
using Akka.Restaurant.Messages.FoodWorkflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Restaurant.Actors.Cooks
{
    internal class CookActor : ReceiveActor
    {
        public Guid CookId { get; set; }
        public List<CookTheFood> BacklogOfFood { get; set; } = new List<CookTheFood>();
        public ILoggingAdapter _logger { get; set; }
        public CookActor(Guid cookId)
        {
            Become(Waiting);
            CookId = cookId;
            _logger = Context.GetLogger();
        }

        public void Waiting()
        {
            Receive<RequestCookStatus>(msg =>
            {
                Sender.Tell(new CookStatusResponse(CookStatus.Waiting, CookId, BacklogOfFood.Count));
            });
            Receive<CookTheFood>(msg =>
            {
                _logger.Debug($"Cooking the food");
                Become(Cooking);
                if (BacklogOfFood.Contains(msg))
                {
                    BacklogOfFood.Remove(msg);
                }
                Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromSeconds(4), Self, new FoodIsCooked(msg.OrderId, msg.FoodId, msg.ServerId, msg.FoodToCook), Self);
            });
        }

        private void Cooking()
        {
            Receive<RequestCookStatus>(msg =>
            {
                Sender.Tell(new CookStatusResponse(CookStatus.Cooking, CookId, BacklogOfFood.Count));
            });
            Receive<CookTheFood>(msg =>
            {
                BacklogOfFood.Add(msg);
            });
            Receive<FoodIsCooked>(msg =>
            {
                _logger.Debug("Food is Cooked");
                var server = Context.System.ActorSelection($"/user/server-manager/server-{msg.ServerId}");
                server.Tell(msg);
                Become(Waiting);
                if (BacklogOfFood.Count > 0)
                {
                    Self.Tell(BacklogOfFood[0]);
                }
            });
        }
    }

    public enum CookStatus
    {
        Waiting,
        Cooking
    }
}
#pragma warning restore AK1004 // ScheduleTellOnce() and ScheduleTellRepeatedly() can cause memory leak if not properly canceled