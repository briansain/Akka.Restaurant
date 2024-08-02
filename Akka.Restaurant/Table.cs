using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Restaurant
{
    internal class Table
    {
        public Guid TableId {get;set;}
        public int NumberOfSeats { get; set; }
        public bool HasCustomers { get; set; }

        public Table(int seats) 
        {
            NumberOfSeats = seats;
            HasCustomers = false;
            TableId = Guid.NewGuid();
        }

        public static List<Table> GetTables()
        {
            var tables = new List<Table>
            {
                new Table(5),
                new Table(4),
                new Table(2)
            };
            return tables;
        }
    }
}
