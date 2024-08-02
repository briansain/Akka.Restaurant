using Akka.Restaurant.Actors.Cooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Restaurant.Messages.FoodWorkflow
{
    internal class CookStatusResponse
    {
        public CookStatus CookStatus { get; set; }
        public Guid CookId { get; set; }
        public int NumBacklogOrders { get; set; }
        public CookStatusResponse(CookStatus cookStatus, Guid cookId, int numBacklogOrders)
        {
            CookStatus = cookStatus;
            CookId = cookId;
            NumBacklogOrders = numBacklogOrders;
        }
    }
}
