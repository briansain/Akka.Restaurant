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
        public Guid FoodId { get; set; }
        public Guid ServerId { get; set; }
        public string Food { get; set; }

        public FoodIsCooked(Guid orderId, Guid foodId, Guid serverId, string food)
        {
            OrderId = orderId;
            Food = food;
            FoodId = foodId;
            ServerId = serverId;
        }
    }
}
