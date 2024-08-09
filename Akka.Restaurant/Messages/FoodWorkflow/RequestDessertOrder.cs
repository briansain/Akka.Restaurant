using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Restaurant.Messages.FoodWorkflow
{
	internal class RequestDessertOrder
	{
		public List<string> Menu =
		[
			"Chocolate Cake",
			"Churros",
			"Cookies",
			"Ice Cream"
		];
	}
}
