﻿using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Messaging
{
    public interface IRegisterOrderCommand
    {
        string PickupName { get; }
        string PickupAddress { get; }
        string PickupCity { get; }

        string DeliverName { get; }
        string DeliverAddress { get; }
        string DeliverCity { get; }

        int Weight { get; }
        bool Fragile { get; }
        bool Oversized { get; }
    }
}
