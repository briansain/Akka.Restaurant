using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Restaurant.Messages
{
    internal class NumAssignedTablesResponse
    {
        public int NumAssignedTables { get; set; }
        public IActorRef Self { get; set; }

        public NumAssignedTablesResponse(int numAssignedTables, IActorRef self)
        {
            NumAssignedTables = numAssignedTables;
            Self = self;
        }
    }
}
