namespace Akka.Restaurant.Messages.DrinksWorkflow
{
    internal class DrinkOrder
    {
        public List<string> Order { get; set; } = new List<string>();
        public DrinkOrder(List<string> order)
        {
            Order = order;
        }
    }
}
