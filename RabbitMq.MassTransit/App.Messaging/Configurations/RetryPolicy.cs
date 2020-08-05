using System;
using System.Collections.Generic;
using System.Text;

namespace App.Messaging.Configurations
{
    public class RetryPolicy
    {
        public Interval Interval { get; set; }
        public Exponential Exponential { get; set; }
        public int? Immediate { get; set; }
        public TimeSpan[] Intervals { get; set; }
    }
}
