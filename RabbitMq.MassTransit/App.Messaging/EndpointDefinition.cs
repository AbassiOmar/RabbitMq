using App.Messaging.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Messaging
{
    public class EndpointDefinition
    {
        public string Name { get; set; }
        public RetryPolicy RetryPolicy { get; set; }
    }
}
