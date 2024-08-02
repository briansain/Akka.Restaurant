using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Restaurant.Messages.FoodWorkflow
{
    internal class CookTheFood
    {
        public string FoodToCook { get; set; }
        public Guid OrderId { get; set; }
        public Guid ServerId { get; set; }
        public Guid FoodId { get; set; }

        public CookTheFood(string foodToCook, Guid orderId, Guid serverId, Guid foodId)
        {
            FoodToCook = foodToCook;
            OrderId = orderId;
            ServerId = serverId;
            FoodId = foodId;
        }
    }
}
