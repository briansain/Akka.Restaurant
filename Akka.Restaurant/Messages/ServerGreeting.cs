namespace Akka.Restaurant.Messages
{
    internal class ServerGreeting
    {
        public string ServerName { get; set; }
        public ServerGreeting(string serverName)
        {
            ServerName = serverName;
        }
    }
}
