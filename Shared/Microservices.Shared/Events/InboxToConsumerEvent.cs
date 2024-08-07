using Microservices.Shared.Events.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Shared.Events
{
    public class OrderCreatedInboxToConsumerEvent : BaseEvent
    {
        public OrderCreatedEvent @event { get; set; }
    }
}
