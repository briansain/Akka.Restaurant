using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Restaurant.Messages
{
    internal class AssignTable
    {
        public AssignTable(NewCustomers newCustomers, Guid tableId)
        {
            NewCustomers = newCustomers;
            TableId = tableId;
        }
        public NewCustomers NewCustomers { get; set; }
        public Guid TableId { get; set; }
    }
}
