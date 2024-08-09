using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Restaurant.Messages.FoodWorkflow
{
    internal class FoodIsCooked
    {
        public Guid OrderId { get; set; }
        public Guid ServerId { get; set; }
        public string Food { get; set; }

        public FoodIsCooked(Guid orderId, Guid serverId, string food)
        {
            OrderId = orderId;
            Food = food;
            ServerId = serverId;
        }
    }
}
