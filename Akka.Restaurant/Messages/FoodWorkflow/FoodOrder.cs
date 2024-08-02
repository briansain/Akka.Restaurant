using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Restaurant.Messages.FoodWorkflow
{
    internal class FoodOrder
    {
        public List<string> Order = new List<string>();
        public Guid OrderId { get; set; }
        public int CompletlyCookedFoods = 0;
        public Guid CustomerId { get; set; }
        public FoodOrder(List<string> order)
        {
            Order = order;
            OrderId = Guid.NewGuid();
        }
    }
}
