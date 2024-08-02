using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Restaurant.Messages
{
    internal class RequestDrinkOrder
    {
        public List<string> AvailableDrinks =
        [
            "Water",
            "Mud Water",
            "Bubbly Water", 
            "Kool-aid"
        ];
    }
}
