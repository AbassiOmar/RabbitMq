using System;
using System.Collections.Generic;
using System.Text;

namespace App.Messaging.Configurations
{
    public class Exponential
    {
        public TimeSpan MinInterval { get; set; }
        public TimeSpan MaxInterval { get; set; }
        public TimeSpan IntervalDelta { get; set; }
        public int Count { get; set; }
    }
}
