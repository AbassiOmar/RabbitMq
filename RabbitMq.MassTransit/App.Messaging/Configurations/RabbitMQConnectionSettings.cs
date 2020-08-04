
namespace App.Messaging.Configurations
{
    public class RabbitMQConnectionSettings
    {
        public string ApplicationName { get; set; }
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public string QueueName { get; set; }
    }
}
