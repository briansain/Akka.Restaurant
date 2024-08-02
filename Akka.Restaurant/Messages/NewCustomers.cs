using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Restaurant.Messages
{
    internal class NewCustomers
    {
        public int NumberOfCustomers { get; set; }
        public Guid CustomerId { get; set; }
    }
}
