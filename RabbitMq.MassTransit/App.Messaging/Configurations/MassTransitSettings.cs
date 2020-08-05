

namespace App.Messaging.Configurations
{
    public class MassTransitSettings
    {
        public RabbitMQConnectionSettings RabbitMQ { get; set; }
        public EndpointDefinition[] Endpoints { get; set; }
        public string DefaultEndpointName { get; set; }
        public string RetryEndpointName { get; set; }
        public string RADLADEndpointName { get; set; }
    }
}
