﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Shared.Events
{
    public class ProductUpdatedInboxToConsumerEvent
    {
        public ProductUpdatedEvent @event { get; set; }
    }
}
